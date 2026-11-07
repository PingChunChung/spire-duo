[繁體中文](README.zh-TW.md)

# SanaCkoala

A Slay the Spire 2 mod that adds two playable characters, the Koala and the Panda. Both work in singleplayer and are built to play well together in co-op.

Steam Workshop: [SanaCkoala](https://steamcommunity.com/sharedfiles/filedetails/?id=3835466853)

## Characters

### The Koala

70 HP. Starter relic **Eucalyptus Leaf**: start each combat with 3 Drowsy.

- **Drowsy**: at the end of your turn, gain Block equal to your Drowsy.
- **Waking up**: some cards remove all of your Drowsy at once for a big effect.
- Three ways to play: keep stacking Drowsy and never wake up (Snoring, Sleepwalk, Dream Shield); stack it, then wake up for burst damage or Energy (Wake-Up Call, Wide Awake, Short Temper); or Retain cards that grow stronger the longer you hold them (Take It Slow, Curl Up, Procrastination).

### The Panda

80 HP. Starter relic **Bamboo Basket**: start each combat with 3 Bamboo.

- **Bamboo** is a resource gathered with cards and powers.
- **Munch N**: if you have at least N Bamboo, the card spends it and triggers a bonus effect.
- Three ways to play: hoard Bamboo and munch it (Dig In, Bamboo Avalanche, Bottomless Stomach); turn Block into damage (Belly Bump, Steamroller, Thick Fat); or build Strength (Bamboo Protein, Heavy Paw, National Treasure).

The Koala has 36 cards and the Panda has 35, counting their own Strike and Defend. Each character also has a starter relic, four more relics and two potions.

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
| Nap Together | Koala card. You and an ally gain Drowsy. |
| Share Bamboo | Panda card. An ally gains Block; Munch 2 gives them Strength. |
| Shelter | Panda card. Gain Block, then an ally gains Block equal to yours. |
| Two shared relics | Block for every player at the start of combat, and extra healing for allies at Rest Sites. |

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
| `SpireDuoCode/Patches` | Harmony patches for fresh modded saves |
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
