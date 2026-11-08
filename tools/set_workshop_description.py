"""set_workshop_description.py <ModUploader dir> [lang ...]: update the Workshop description per language, without uploading content or adding a change note.

ModUploader only uploads the English description from workshop/workshop.json. Run this after each upload
so the Traditional Chinese description (workshop/description.zh-TW.txt) stays in sync. Uses the
steam_api64.dll that ships with ModUploader; Steam must be running and logged in as the item's owner.

    python tools/set_workshop_description.py D:/path/to/ModUploader            # tchinese only
    python tools/set_workshop_description.py D:/path/to/ModUploader english tchinese
"""
import ctypes as C
import json
import os
import pathlib
import sys
import time

APP_ID = 2868840
WORKSHOP = pathlib.Path(__file__).resolve().parent.parent / "workshop"
SOURCES = {
    "english": lambda: json.loads((WORKSHOP / "workshop.json").read_text(encoding="utf-8"))["description"],
    "tchinese": lambda: (WORKSHOP / "description.zh-TW.txt").read_text(encoding="utf-8").strip(),
}
SUBMIT_ITEM_UPDATE_RESULT = 3404  # k_iSteamUGCCallbacks + 4


def bind(dll, name, restype, *argtypes):
    fn = getattr(dll, name)
    fn.restype, fn.argtypes = restype, list(argtypes)
    return fn


def main():
    uploader = pathlib.Path(sys.argv[1]).resolve()
    langs = sys.argv[2:] or ["tchinese"]
    item = int((WORKSHOP / "mod_id.txt").read_text().strip())
    texts = {lang: SOURCES[lang]().encode("utf-8") for lang in langs}
    for lang, data in texts.items():
        assert len(data) < 8000, f"{lang} description is {len(data)} bytes; Steam allows 8000"

    # SteamAPI_Init looks for steam_appid.txt in the working directory.
    os.chdir(uploader)
    dll = C.CDLL(str(uploader / "steam_api64.dll"))
    err = C.create_string_buffer(1024)
    if bind(dll, "SteamAPI_InitFlat", C.c_int, C.c_char_p)(err) != 0:
        sys.exit(f"Steam init failed: {err.value.decode(errors='replace')}")
    ugc = bind(dll, "SteamAPI_SteamUGC_v021", C.c_void_p)()
    utils = bind(dll, "SteamAPI_SteamUtils_v010", C.c_void_p)()
    start = bind(dll, "SteamAPI_ISteamUGC_StartItemUpdate", C.c_uint64, C.c_void_p, C.c_uint32, C.c_uint64)
    set_lang = bind(dll, "SteamAPI_ISteamUGC_SetItemUpdateLanguage", C.c_bool, C.c_void_p, C.c_uint64, C.c_char_p)
    set_desc = bind(dll, "SteamAPI_ISteamUGC_SetItemDescription", C.c_bool, C.c_void_p, C.c_uint64, C.c_char_p)
    submit = bind(dll, "SteamAPI_ISteamUGC_SubmitItemUpdate", C.c_uint64, C.c_void_p, C.c_uint64, C.c_char_p)
    completed = bind(dll, "SteamAPI_ISteamUtils_IsAPICallCompleted", C.c_bool, C.c_void_p, C.c_uint64, C.POINTER(C.c_bool))
    get_result = bind(dll, "SteamAPI_ISteamUtils_GetAPICallResult", C.c_bool,
                      C.c_void_p, C.c_uint64, C.c_void_p, C.c_int, C.c_int, C.POINTER(C.c_bool))
    run_callbacks = bind(dll, "SteamAPI_RunCallbacks", None)

    failed_any = False
    for lang, data in texts.items():
        handle = start(ugc, APP_ID, item)
        if not (set_lang(ugc, handle, lang.encode()) and set_desc(ugc, handle, data)):
            sys.exit(f"{lang}: Steam rejected the update fields")
        # No change note, so the item's change log is left alone.
        call = submit(ugc, handle, None)
        io_failed = C.c_bool()
        for _ in range(600):
            run_callbacks()
            if completed(utils, call, C.byref(io_failed)):
                break
            time.sleep(0.1)
        buf = (C.c_ubyte * 16)()
        got = get_result(utils, call, buf, 16, SUBMIT_ITEM_UPDATE_RESULT, C.byref(io_failed))
        eresult = int.from_bytes(bytes(buf[:4]), "little")
        ok = got and not io_failed.value and eresult == 1
        failed_any |= not ok
        print(f"{lang}: {'updated' if ok else f'failed (EResult {eresult})'}"
              + ("; accept the Workshop legal agreement first" if buf[4] else ""))
    sys.exit(1 if failed_any else 0)


if __name__ == "__main__":
    main()
