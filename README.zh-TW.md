[English](README.md)

# SanaCkoala

《殺戮尖塔 2》的角色 mod，新增無尾熊和熊貓兩個可玩角色。單人可以玩，兩個角色在多人連線時也能互相配合。

Steam 工作坊：[SanaCkoala](https://steamcommunity.com/sharedfiles/filedetails/?id=3835466853)

## 角色

### 無尾熊

70 HP。起始遺物「尤加利葉」，每場戰鬥開始獲得 3 層睡意。

- **睡意**：回合結束時，獲得等同睡意層數的格擋。
- **醒來**：部分卡牌會一次移除全部睡意，換成強力效果。
- 三種玩法：一直疊睡意、不醒來（打呼、夢遊、夢境結界）；疊滿再醒來，換成爆發傷害或能量（起床氣、大夢初醒、火氣很大）；或是保留手牌，越放越強（慢慢來、蜷成一團、拖延症）。

### 熊貓

80 HP。起始遺物「竹籃」，每場戰鬥開始獲得 3 根竹子。

- **竹子**：用卡牌和能力收集的資源。
- **啃 N**：手上有 N 根以上竹子時，卡牌會吃掉 N 根，觸發額外效果。
- 三種玩法：囤竹子再大口啃（開動了、竹子雪崩、無底胃）；把格擋變成傷害（肚皮撞、壓路機、厚脂肪）；或是累積力量（高蛋白竹子、千斤掌、國寶）。

無尾熊有 36 張卡、熊貓有 35 張，都包含自己的打擊和防禦。每個角色另有起始遺物、4 個遺物和 2 瓶藥水。

## 多人共用內容

以下內容只在兩人以上的對局出現。

| 內容 | 效果 |
|---|---|
| 牽手 | 兩個角色起始牌組都有的基本卡。你和一名隊友獲得格擋。 |
| 分你一口 | 0 費。你和一名隊友各獲得能量。消耗。 |
| 吵架 | 造成傷害，所有玩家各失去 2 點生命。 |
| 和好 | 你和一名隊友回復生命。消耗。 |
| 助攻 | 造成傷害；本回合隊友下一次打中這個敵人時，額外造成傷害。 |
| 約定 | 稀有能力。你和一名隊友在每回合開始時獲得格擋。 |
| 一起睡午覺 | 無尾熊的卡。你和一名隊友各獲得睡意。 |
| 分你竹子 | 熊貓的卡。一名隊友獲得格擋；啃 2 時隊友再獲得力量。 |
| 我罩你 | 熊貓的卡。獲得格擋，再讓一名隊友獲得等同你格擋值的格擋。 |
| 兩個共用遺物 | 戰鬥開始時所有玩家獲得格擋；在休息處回血時讓隊友額外回血。 |

## 安裝

在 Steam 工作坊訂閱即可。這個 mod 需要 [BaseLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127)，訂閱時 Steam 會提示一起訂閱。

- 多人連線時，每位玩家都要裝同一個版本。
- 不要另外在遊戲的 `mods` 資料夾放一份手動安裝的版本。
- 有 mod 時遊戲使用另一組存檔，不會動到原本的進度。
- 在全新的 mod 存檔上，原版會跳過第一章的先古之民 Neow，第一個寶箱也會給每位玩家同一個教學遺物。這個 mod 會保留 Neow，教學遺物只給其中一位玩家。

只在遊戲 v0.107.1 測試過。

## 從原始碼建置

需要：

- .NET 9 SDK
- MegaDot 4.5.1（Godot 4.5.1 mono），用來匯出 `.pck`
- 已安裝《殺戮尖塔 2》

在 `Directory.Build.props` 設定 `GodotPath`；如果找不到遊戲，再設定 `Sts2Path`。這個檔案存的是本機路徑，所以不進 git。

```bash
dotnet publish SanaCkoala.csproj
```

`dotnet build` 會把 DLL 複製到遊戲的 `mods/SanaCkoala`。`dotnet publish` 另外匯出含圖片和文字的 `SanaCkoala.pck`。

mod id 是 `SanaCkoala`。C# 命名空間和卡牌 id 保留原本的 `SpireDuo` 前綴，舊版存下的對局仍然讀得到。

## 專案結構

| 路徑 | 內容 |
|---|---|
| `SpireDuoCode/Character` | 角色、卡池、遺物池 |
| `SpireDuoCode/Cards` | 所有卡牌 |
| `SpireDuoCode/Powers` | 睡意、竹子等能力 |
| `SpireDuoCode/Relics` | 起始遺物、角色遺物與共用遺物 |
| `SpireDuoCode/Potions` | 角色藥水 |
| `SpireDuoCode/Patches` | 處理全新 mod 存檔的 Harmony 補丁 |
| `SanaCkoala/localization` | `eng`、`zht`、`zhs` 三種語言的文字 |
| `SanaCkoala/images` | 卡圖、圖示、角色圖 |
| `tools/gen_localization.py` | 產生全部語言檔，`zhs` 由 `zht` 轉換 |
| `tools/install_art.py` | 把原始圖縮成 mod 需要的尺寸並放進圖片資料夾 |
| `workshop/` | Mega Crit ModUploader 用的工作坊上架資料夾 |

要改文字請改 `tools/gen_localization.py`，不要直接改 JSON，改完執行：

```bash
python tools/gen_localization.py .
```

## 美術

卡圖、圖示和角色圖由 AI 繪圖工具（OpenAI Codex）產生。

這是玩家自製的 mod，與 Mega Crit 沒有關聯，也未經其認可。
