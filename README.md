[繁體中文](README.zh-TW.md)

# SanaCkoala

A Slay the Spire 2 mod that adds two playable characters, the Koala and the Panda, plus one more who is waiting to be found. They all work in singleplayer and are built to play well together in co-op.

Steam Workshop: [SanaCkoala](https://steamcommunity.com/sharedfiles/filedetails/?id=3835466853)

## Characters

### The Koala

70 HP. Starter relic **Eucalyptus Leaf**: start each combat with 3 Drowsy. Touch of Orobas upgrades it to **Eucalyptus Crown** (6 Drowsy).

- **Drowsy**: at the end of your turn, gain Block equal to your Drowsy.
- **Waking up**: some cards remove all of your Drowsy at once for a big effect.
- Three ways to play: build a **Hibernation Fortress** that stays Drowsy (Sleep Debt, Night Watch, Fortress Nap); bank Drowsy for an **Alarm Burst** (Alarm Drill, Snooze Again, Rude Alarm); or manage a **Dream / Retain** hand (Dream Journal, Dream Gate, Deep Script).

### The Panda

80 HP. Starter relic **Bamboo Basket**: start each combat with 3 Bamboo. Touch of Orobas upgrades it to **Bamboo Hamper** (6 Bamboo).

- **Bamboo** is a resource gathered with cards and powers.
- **Munch N**: if you have at least N Bamboo, the card spends it and triggers a bonus effect.
- Four ways to play: turn the first Munch into **Full** menu choices (Taste Test, Packed Lunch, Second Course); trade Block for counterattacks (Soft Landing, Counter Curl, Belly Fortress); build multi-hit Strength turns (Paw Practice, Protein Shake, Maul Rhythm); or build and spend **Roll** momentum (Roll Start, Downhill, Pinball Panda).

### ???

Something is waiting at the top of the Spire. Take the Koala and the Panda up there and see who shows up. And that may not be the last pair of paws.

Each character has 87 cards: 3 Basic, 20 Common, 36 Uncommon, 26 Rare and 2 Ancient, the same split as the vanilla characters. Each also has seven relics besides the starter relic, and three potions (Common, Uncommon and Rare).

## Ancient cards and events

- **Archaic Tooth** turns Doze into Deep Sleep and Gather Bamboo into Bamboo Harvest.
- **Dusty Tome** gives the Koala Lazy Sunday and the Panda Rolling Thunder.
- Twelve events appear only in runs with the Koala or the Panda: Picnic for Two, The Last Dumpling, Eucalyptus Tollbooth and Honeyed Hive (act 1); Stargazing, Borrowed Hammock, Bamboo Appraiser and Rainy-Day Argument (act 2); Little Quarrel, Dream Exchange and The Summit Picnic (act 3); and Photo Booth (any act). Shared-vote events require more than one player.

## Co-op content

These appear only in runs with more than one player.

| Content | Effect |
|---|---|
| Hold Hands | Basic card in both starting decks. You and a chosen ally each gain 5 Block (7 upgraded). |
| Share a Bite | 0 cost. You and an ally each gain Energy. Exhaust. |
| Quarrel | Deal damage. Every player loses 2 HP. |
| Make Up | You and an ally heal. Exhaust. |
| Assist | Deal damage. This turn, an ally's next hit on that enemy deals extra damage. |
| Promise | Rare power. You and an ally gain Block at the start of each turn. |
| Shared relics | Couple Rings gives every player Block at combat start; Spare Key heals the lowest-HP ally at Rest Sites; Passing Note lets the ally with the fewest cards in hand draw after a resource gain. |
| Half-and-Half | Event-only potion. You and a chosen ally each gain 5 Block and draw 1. |
| Koala cards | Nap Together, Pillow Toss, Snuggle, Wake-Up Kiss and Pajama Party hand out Drowsy, Block, cards and Energy to allies. |
| Panda cards | Share Bamboo, Shelter, Panda Pile, Piggyback Charge, Carry You, Feeding Time and Partner in Crime protect allies, give them Energy and cards, or hit harder together. |
| Character relics | Shared Blanket (Koala): allies gain Block at the end of your turn. Picnic Mat (Panda): allies gain Block whenever you Munch. |
| Events | Stargazing, The Last Dumpling, Borrowed Hammock, Little Quarrel, Honeyed Hive, Rainy-Day Argument and The Summit Picnic. |

## Memory Album

A **Memory Album** button on the main menu opens an achievement album: 56 achievements on seven pages (Journey, Koala, Panda, two hidden pages, Together and Keepsakes). 45 can be earned in singleplayer. The other 11 need a co-op run; any teammate and any characters count.

- Achievements are Bronze, Silver or Gold, worth 1, 2 or 3 points (109 in total). Counting achievements show a progress bar and are marked "Almost there" at 70%. Three are hidden until earned.
- A banner with the medal appears when you earn one, the album button shows a red dot, and new entries pop in the next time you open the album.
- One page stays hidden until you find who it belongs to.
- The end-of-run screen lists up to three achievements earned that run and up to three that are close.
- At 20, 45, 70 and 109 points the album gets a new background and a pin appears next to the album button. Gold achievements give a title shown at the top of the album.
- Achievements are keepsakes only. They never change cards, rewards, events or combat. They are earned for the whole party and saved in each player's own progress file.

## Installation

Subscribe on the Steam Workshop. The mod requires [BaseLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127), and Steam asks you to subscribe to it as well.

- For multiplayer, every player needs the same version of this mod.
- Do not also keep a manual copy in the game's `mods` folder.
- Modded play uses a separate set of save files.
- On a fresh modded save the game would skip Neow in act 1 and give every player the same tutorial relic from the first chest. The mod keeps Neow and gives the tutorial relic to one player only.

Tested on game version v0.107.1 only.

## Building from source

Requirements:

- .NET 9 SDK
- MegaDot 4.5.1 (Godot 4.5.1 mono) to export the `.pck`
- Slay the Spire 2 installed

Set `GodotPath` and, if the game is not found automatically, `Sts2Path` in `Directory.Build.props`. This file is ignored by git because it holds local paths.

```bash
dotnet publish SanaCkoala.csproj
```

`dotnet build` copies the DLL into the game's `mods/SanaCkoala` folder. `dotnet publish` also exports `SanaCkoala.pck` with images and localization.

The mod id is `SanaCkoala`. C# namespaces and card ids keep the original `SpireDuo` prefix, so runs saved with earlier versions still load.

To test the hidden character without unlocking it, start the game with the environment variable `SANACKOALA_UNLOCK_ALL=1`. `SANACKOALA_ALBUM_ALL=1` shows every Memory Album entry as earned without saving anything. The dev console command `sanackoala_achievement <ID>` earns one achievement, `sanackoala_achievement unseen` replays the reveal, and `sanackoala_achievement reset` clears achievements and stats (the hidden character stays unlocked).

## Project layout

| Path | Contents |
|---|---|
| `SpireDuoCode/Character` | Character models, card pools, relic pools |
| `SpireDuoCode/Cards` | All cards |
| `SpireDuoCode/Powers` | Drowsy, Bamboo and other powers |
| `SpireDuoCode/Progress` | Mod-owned progress file, achievements and the Memory Album |
| `SpireDuoCode/Relics` | Starter, character and shared relics |
| `SpireDuoCode/Potions` | Character potions |
| `SpireDuoCode/Events` | The twelve events |
| `SpireDuoCode/Patches` | Harmony patches for fresh modded saves, upgraded starter relics, Archaic Tooth and the shop sprite |
| `SanaCkoala/localization` | `eng`, `zht`, `zhs` text |
| `SanaCkoala/images` | Card art, icons, character images |
| `tools/gen_localization.py` | Writes all localization files; `zhs` is converted from `zht` |
| `tools/install_art.py` | Resizes source art into the mod's image folders |
| `tools/set_workshop_description.py` | Updates the Workshop description per language |
| `workshop/` | Steam Workshop workspace for Mega Crit's ModUploader |

Edit text in `tools/gen_localization.py`, not in the JSON files, then run:

```bash
python tools/gen_localization.py .
```

## Publishing to the Workshop

Every update needs both an English and a Traditional Chinese description.

1. Update `workshop/workshop.json` (English description, and a change note with English and Chinese bullet lists) and `workshop/description.zh-TW.txt`.
2. Bump `version` in `SanaCkoala.json`, run `dotnet publish`, and copy the DLL, `.pck` and `.json` into `workshop/content`.
3. Upload with ModUploader. It reads the item id from `workshop/mod_id.txt` and uploads the English description.
4. Upload the Chinese description:

```bash
python tools/set_workshop_description.py <ModUploader folder>
```

Pass `english tchinese` to update both languages without uploading the mod again or adding a change note.

## Art

Card art, icons and character images were generated with AI image tools (OpenAI Codex).

Fan-made mod, not affiliated with or endorsed by Mega Crit.
