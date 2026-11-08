[繁體中文](README.zh-TW.md)

# SanaCkoala

A Slay the Spire 2 mod that adds two playable characters, the Koala and the Panda. Both work in singleplayer and are built to play well together in co-op.

Steam Workshop: [SanaCkoala](https://steamcommunity.com/sharedfiles/filedetails/?id=3835466853)

## Characters

### The Koala

70 HP. Starter relic **Eucalyptus Leaf**: start each combat with 3 Drowsy. Touch of Orobas upgrades it to **Eucalyptus Crown** (6 Drowsy).

- **Drowsy**: at the end of your turn, gain Block equal to your Drowsy.
- **Waking up**: some cards remove all of your Drowsy at once for a big effect.
- Three ways to play: keep stacking Drowsy and never wake up (Snoring, Sleep Like a Log, Dreamland); stack it, then wake up for burst damage or Energy (Wake-Up Call, Wide Awake, Tantrum, Morning Person); or Retain cards that grow stronger the longer you hold them (Take It Slow, Long Overdue, Grudge).

### The Panda

80 HP. Starter relic **Bamboo Basket**: start each combat with 3 Bamboo. Touch of Orobas upgrades it to **Bamboo Hamper** (6 Bamboo).

- **Bamboo** is a resource gathered with cards and powers.
- **Munch N**: if you have at least N Bamboo, the card spends it and triggers a bonus effect.
- Three ways to play: hoard Bamboo and munch it (Dig In, Bottomless Stomach, Second Stomach); turn Block into damage (Belly Bump, Pancake Press, Belly Bounce); or build Strength (National Treasure, Hulking Paws, Fury of the Fluffy).

Each character has 87 cards: 3 Basic, 20 Common, 36 Uncommon, 26 Rare and 2 Ancient, the same split as the vanilla characters. Each also has seven relics besides the starter relic, and two potions.

## Ancient cards and events

- **Archaic Tooth** turns Doze into Deep Sleep and Gather Bamboo into Bamboo Harvest.
- **Dusty Tome** gives the Koala Lazy Sunday and the Panda Rolling Thunder.
- Four events appear only in runs with the Koala or the Panda: Picnic for Two (act 1), Stargazing (act 2), Little Quarrel (acts 2 and 3) and Photo Booth (any act). Stargazing and Little Quarrel need more than one player.

## Co-op content

These appear only in runs with more than one player.

| Content | Effect |
|---|---|
| Hold Hands | Basic card in both starting decks. You and an ally gain Block. |
| Share a Bite | 0 cost. You and an ally each gain Energy. Exhaust. |
| Quarrel | Deal damage. Every player loses 2 HP. |
| Make Up | You and an ally heal. Exhaust. |
| Assist | Deal damage. This turn, an ally's next hit on that enemy deals extra damage. |
| Promise | Rare power. You and an ally gain Block at the start of each turn. |
| Two shared relics | Block for every player at the start of combat, and extra healing for allies at Rest Sites. |
| Koala cards | Nap Together, Pillow Toss, Snuggle, Wake-Up Kiss and Pajama Party hand out Drowsy, Block, cards and Energy to allies. |
| Panda cards | Share Bamboo, Shelter, Panda Pile, Piggyback Charge, Carry You, Feeding Time and Partner in Crime protect allies, give them Energy and cards, or hit harder together. |
| Character relics | Shared Blanket (Koala): allies gain Block at the end of your turn. Picnic Mat (Panda): allies gain Block whenever you Munch. |
| Events | Stargazing and Little Quarrel. |

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

## Project layout

| Path | Contents |
|---|---|
| `SpireDuoCode/Character` | Character models, card pools, relic pools |
| `SpireDuoCode/Cards` | All cards |
| `SpireDuoCode/Powers` | Drowsy, Bamboo and other powers |
| `SpireDuoCode/Relics` | Starter, character and shared relics |
| `SpireDuoCode/Potions` | Character potions |
| `SpireDuoCode/Events` | The four events |
| `SpireDuoCode/Patches` | Harmony patches for fresh modded saves, upgraded starter relics, Archaic Tooth and the shop sprite |
| `SanaCkoala/localization` | `eng`, `zht`, `zhs` text |
| `SanaCkoala/images` | Card art, icons, character images |
| `tools/gen_localization.py` | Writes all localization files; `zhs` is converted from `zht` |
| `tools/install_art.py` | Resizes source art into the mod's image folders |
| `workshop/` | Steam Workshop workspace for Mega Crit's ModUploader |

Edit text in `tools/gen_localization.py`, not in the JSON files, then run:

```bash
python tools/gen_localization.py .
```

## Art

Card art, icons and character images were generated with AI image tools (OpenAI Codex).

Fan-made mod, not affiliated with or endorsed by Mega Crit.
