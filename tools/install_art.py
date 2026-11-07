"""install_art.py <codex-art dir> <mod images dir>: resize Codex art into the mod's image folders."""
import sys, pathlib
from PIL import Image, ImageFilter
src, dst = map(pathlib.Path, sys.argv[1:3])
POWERS = {"drowsy_power", "hibernate_power", "bamboo_power", "bamboo_grove_power", "iron_hide_power", "big_appetite_power", "promise_power"}
RELICS = {"eucalyptus_leaf", "bamboo_basket", "couple_rings", "spare_key"}
n = {"card": 0, "power": 0, "relic": 0}
for f in sorted(p for d in ("koala", "panda", "icons") for p in (src / d).glob("*.png") if not p.name.startswith("ref_")):
    im = Image.open(f)
    stem = f.stem
    if stem in POWERS:
        im = im.convert("RGBA")
        im.resize((256, 256), Image.LANCZOS).save(dst / "powers" / "big" / f.name)
        im.resize((64, 64), Image.LANCZOS).save(dst / "powers" / f.name)
        n["power"] += 1
    elif stem in RELICS:
        im = im.convert("RGBA")
        im.resize((256, 256), Image.LANCZOS).save(dst / "relics" / "big" / f.name)
        small = im.resize((94, 94), Image.LANCZOS)
        small.save(dst / "relics" / f.name)
        # Outline: white silhouette grown a few pixels, like the template relic_outline.png.
        alpha = small.getchannel("A").point(lambda a: 255 if a > 40 else 0).filter(ImageFilter.MaxFilter(7))
        outline = Image.new("RGBA", small.size, (255, 255, 255, 0))
        outline.putalpha(alpha.filter(ImageFilter.GaussianBlur(0.8)))
        outline.save(dst / "relics" / f"{stem}_outline.png")
        n["relic"] += 1
    else:
        im = im.convert("RGB")
        if im.size != (1000, 760):
            im = im.resize((1000, 760), Image.LANCZOS)
        im.save(dst / "card_portraits" / "big" / f.name)
        im.resize((250, 190), Image.LANCZOS).save(dst / "card_portraits" / f.name)
        n["card"] += 1
print(n)
