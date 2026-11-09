"""install_art.py <mod images dir> <art dir>...: resize Codex art into the mod's image folders.
Files named ref_* or _* and *_work drafts are skipped."""
import re
import sys, pathlib
from PIL import Image, ImageFilter
dst = pathlib.Path(sys.argv[1])
srcs = [pathlib.Path(p) for p in sys.argv[2:]]
POWERS = {"drowsy_power", "hibernate_power", "bamboo_power", "bamboo_grove_power", "iron_hide_power", "big_appetite_power",
          "promise_power", "snoring_power", "lazy_afternoon_power", "short_temper_power", "dream_catcher_power",
          "procrastination_power", "dream_power_power", "dig_in_power", "national_treasure_power", "bottomless_stomach_power",
          "pincer_power", "lazy_sunday_power", "rolling_thunder_power",
          "morning_person_power", "grumpy_morning_power", "grudge_power", "warm_and_fuzzy_power", "night_light_power",
          "nap_habit_power", "do_not_disturb_power", "energy_saving_mode_power", "sleep_like_a_log_power",
          "dreamland_power", "hoarder_power", "bear_stance_power", "snack_addict_power", "belly_bounce_power",
          "feeding_time_power", "second_stomach_power", "bamboo_shoots_power", "fury_of_the_fluffy_power",
          "hulking_paws_power", "partner_in_crime_power"}
POWERS.update({"night_watch_power", "fortress_nap_power", "dream_gate_power", "deep_script_power"})
POWERS.update({"full_power", "munch_memory_power", "wake_memory_power", "roll_power", "digest_power", "belly_fortress_power",
               "counter_curl_power", "bottomless_buffet_power", "return_to_sender_power", "alpha_panda_power",
               "alpha_panda_ready_power", "rolling_feast_power", "crunchy_shoot_power"})
# Enchantment trackers reuse their enchantment icon.
POWERS.update({"well_rested_memory_power", "freshly_cut_power"})
RELICS = {"eucalyptus_leaf", "bamboo_basket", "couple_rings", "spare_key", "fluffy_pillow", "alarm_clock", "night_cap",
          "body_pillow", "chopsticks", "rice_bowl", "golden_bamboo", "lunch_box", "eucalyptus_crown", "bamboo_hamper",
          "hammock", "five_more_minutes", "shared_blanket", "beanbag", "bamboo_barbell", "picnic_mat"}
RELICS.update({"sleepy_ledger", "dream_catcher_bell", "bamboo_sled", "split_bamboo", "passing_note"})
RELICS.update({"moonbell_collar", "eclipse_chime"})
# Black Cat batch 4.
POWERS.update({"black_thread_power", "borrowed_rhythm_power", "candle_at_noon_power", "cat_of_the_threshold_power", "catnap_ledger_power", "catwalk_king_power", "constellation_nap_power", "dark_moon_contract_power", "ebon_calendar_power", "evening_wash_power", "feline_velocity_power", "guardian_of_the_eaves_power", "hunters_pause_power", "immaculate_coat_power", "inked_window_power", "inkwell_sky_power", "lantern_shadow_power", "midnight_oracle_power", "night_atlas_power", "omen_reader_power", "purrfect_order_power", "quiet_company_power", "quiet_pact_power", "scrupulous_power", "secret_signal_power", "shared_nightshift_power", "soft_authority_power", "tandem_prowl_power", "velvet_eclipse_power"})
RELICS.update({"velvet_pouch", "windowsill_token", "quiet_hourglass", "gutter_crown", "comet_comb", "lantern_of_one_window", "curiosity_string"})
POTIONS = {"warm_milk", "strong_coffee", "bamboo_juice", "honey_jar", "lucid_tea", "crunchy_shoot", "half_and_half",
           "pawprint_draught", "moonmilk_flask", "ninth_spark"}
# Same 2.13:1 shape as the vanilla 3440x1613 portraits; the event screen fits them into 2560x1200.
EVENTS = {"picnic_for_two", "stargazing", "little_quarrel", "photo_booth", "the_last_dumpling", "borrowed_hammock"}
EVENTS.update({"eucalyptus_tollbooth", "honeyed_hive", "bamboo_appraiser", "rainy_day_argument", "dream_exchange",
               "the_summit_picnic"})
# Vanilla enchantment icons are 64x64.
ENCHANTMENTS = {"well_rested", "freshly_cut"}
ORB = re.compile(r"(koala|panda|cat)_orb_layer_[1-5]")
HAND = re.compile(r"(koala|panda|cat)_hand_(point|rock|paper|scissors)")
# Footprint slot icons on the Black Cat's health bar.
FOOTPRINT = re.compile(r"footprint_(attack|skill|power|empty|lock)")
# Already at final size; copied as-is.
CHARUI = {"cat_body", "character_icon_cat", "map_marker_cat", "char_select_cat", "char_select_cat_locked"}
# Character height in pixels; BaseLib shows these sprites at native size (vanilla shop Ironclad is ~470px tall).
REST_SITE = {"koala_rest_site": 430, "panda_rest_site": 460, "cat_rest_site": 350}
MERCHANT = {"koala_merchant": 400, "panda_merchant": 450, "cat_merchant": 380}
SELECT_BG = {"koala_char_select_bg", "panda_char_select_bg", "cat_char_select_bg"}
# BaseLib centres the rest-site sprite at y = -0.1 * height, so on a 630px canvas its bottom edge sits at +252,
# the same ground line as the vanilla Ironclad's hitbox.
REST_SITE_CANVAS_HEIGHT = 630


def outline(im, grow):
    """White silhouette grown a few pixels, like the template relic_outline.png."""
    alpha = im.getchannel("A").point(lambda a: 255 if a > 40 else 0).filter(ImageFilter.MaxFilter(grow))
    out = Image.new("RGBA", im.size, (255, 255, 255, 0))
    out.putalpha(alpha.filter(ImageFilter.GaussianBlur(0.8)))
    return out


def fit_height(im, height):
    """Crop to the visible pixels and scale to the given height, resampling with premultiplied alpha."""
    im = im.convert("RGBA")
    im = im.crop(im.getchannel("A").getbbox())
    width = round(im.width * height / im.height)
    return im.convert("RGBa").resize((width, height), Image.LANCZOS).convert("RGBA")


def on_canvas(im, size):
    """Bottom-centre the image on a transparent canvas."""
    canvas = Image.new("RGBA", size, (0, 0, 0, 0))
    canvas.alpha_composite(im, ((size[0] - im.width) // 2, size[1] - im.height))
    return canvas


n = {"card": 0, "power": 0, "relic": 0, "potion": 0, "charui": 0, "event": 0, "enchantment": 0}
for f in sorted(p for d in srcs for p in d.glob("*.png") if not p.name.startswith(("ref_", "_")) and not p.stem.endswith("_work")):
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
        outline(small, 7).save(dst / "relics" / f"{stem}_outline.png")
        n["relic"] += 1
    elif stem in POTIONS:
        im = im.convert("RGBA").resize((256, 256), Image.LANCZOS)
        im.save(dst / "potions" / f.name)
        outline(im, 9).save(dst / "potions" / "outline" / f.name)
        n["potion"] += 1
    elif stem in ENCHANTMENTS:
        (dst / "enchantments").mkdir(exist_ok=True)
        im.convert("RGBA").resize((64, 64), Image.LANCZOS).save(dst / "enchantments" / f.name)
        n["enchantment"] += 1
    elif stem in EVENTS:
        (dst / "events").mkdir(exist_ok=True)
        im.convert("RGB").resize((1920, 900), Image.LANCZOS).save(dst / "events" / f.name)
        n["event"] += 1
    elif FOOTPRINT.fullmatch(stem):
        (dst / "charui" / "footprints").mkdir(exist_ok=True)
        im.convert("RGBA").resize((64, 64), Image.LANCZOS).save(dst / "charui" / "footprints" / f.name)
        n["charui"] += 1
    elif stem in CHARUI:
        im.convert("RGBA").save(dst / "charui" / f.name)
        n["charui"] += 1
    elif ORB.fullmatch(stem):
        (dst / "charui" / "orb").mkdir(exist_ok=True)
        im.convert("RGBA").resize((256, 256), Image.LANCZOS).save(dst / "charui" / "orb" / f.name)
        n["charui"] += 1
    elif HAND.fullmatch(stem):
        (dst / "charui" / "hands").mkdir(exist_ok=True)
        im.convert("RGBA").resize((422, 1200), Image.LANCZOS).save(dst / "charui" / "hands" / f.name)
        n["charui"] += 1
    elif stem in REST_SITE:
        im = fit_height(im, REST_SITE[stem])
        on_canvas(im, (im.width + 20, REST_SITE_CANVAS_HEIGHT)).save(dst / "charui" / f.name)
        n["charui"] += 1
    elif stem in MERCHANT:
        im = fit_height(im, MERCHANT[stem])
        on_canvas(im, (im.width + 20, im.height + 10)).save(dst / "charui" / f.name)
        n["charui"] += 1
    elif stem in SELECT_BG:
        # Codex put the character left of centre, where the select screen's info panel sits; mirror it to the right.
        im = im.convert("RGB").resize((1920, 1080), Image.LANCZOS).transpose(Image.FLIP_LEFT_RIGHT)
        im.save(dst / "charui" / f.name)
        n["charui"] += 1
    else:
        im = im.convert("RGB")
        if im.size != (1000, 760):
            im = im.resize((1000, 760), Image.LANCZOS)
        im.save(dst / "card_portraits" / "big" / f.name)
        im.resize((250, 190), Image.LANCZOS).save(dst / "card_portraits" / f.name)
        n["card"] += 1

# The co-op vote and Ancient dialogue icons draw this silhouette behind the character icon.
for who in ("koala", "panda", "cat"):
    icon = Image.open(dst / "charui" / f"character_icon_{who}.png").convert("RGBA")
    outline(icon, 7).save(dst / "charui" / f"character_icon_{who}_outline.png")
print(n)
