"""install_art.py <mod images dir> <art dir>...: resize Codex art into the mod's image folders. Files named ref_* are skipped."""
import sys, pathlib
from PIL import Image, ImageFilter
dst = pathlib.Path(sys.argv[1])
srcs = [pathlib.Path(p) for p in sys.argv[2:]]
POWERS = {"drowsy_power", "hibernate_power", "bamboo_power", "bamboo_grove_power", "iron_hide_power", "big_appetite_power",
          "promise_power", "snoring_power", "lazy_afternoon_power", "short_temper_power", "dream_catcher_power",
          "procrastination_power", "dream_power_power", "dig_in_power", "national_treasure_power", "bottomless_stomach_power",
          "pincer_power"}
RELICS = {"eucalyptus_leaf", "bamboo_basket", "couple_rings", "spare_key", "fluffy_pillow", "alarm_clock", "night_cap",
          "body_pillow", "chopsticks", "rice_bowl", "golden_bamboo", "lunch_box"}
POTIONS = {"warm_milk", "strong_coffee", "bamboo_juice", "honey_jar"}


def outline(im, grow):
    """White silhouette grown a few pixels, like the template relic_outline.png."""
    alpha = im.getchannel("A").point(lambda a: 255 if a > 40 else 0).filter(ImageFilter.MaxFilter(grow))
    out = Image.new("RGBA", im.size, (255, 255, 255, 0))
    out.putalpha(alpha.filter(ImageFilter.GaussianBlur(0.8)))
    return out


n = {"card": 0, "power": 0, "relic": 0, "potion": 0}
for f in sorted(p for d in srcs for p in d.glob("*.png") if not p.name.startswith("ref_")):
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
    else:
        im = im.convert("RGB")
        if im.size != (1000, 760):
            im = im.resize((1000, 760), Image.LANCZOS)
        im.save(dst / "card_portraits" / "big" / f.name)
        im.resize((250, 190), Image.LANCZOS).save(dst / "card_portraits" / f.name)
        n["card"] += 1
print(n)
