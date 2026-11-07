[English](README.md)

# SanaCkoala

《殺戮尖塔 2》的角色 mod，新增無尾熊和熊貓兩個可玩角色。單人可以玩，兩個角色在多人連線時也能互相配合。

Steam 工作坊：[SanaCkoala](https://steamcommunity.com/sharedfiles/filedetails/?id=3835466853)

## 角色

### 無尾熊

70 HP。起始遺物「尤加利葉」，每場戰鬥開始獲得 3 層睡意。

- **睡意**：回合結束時，獲得等同睡意層數的格擋。
- 很多卡牌會累積睡意。起床類卡牌一次移除全部睡意，換成傷害。

### 熊貓

80 HP。起始遺物「竹籃」，每場戰鬥開始獲得 3 根竹子。

- **竹子**：用卡牌和能力收集的資源。
- **啃 N**：手上有 N 根以上竹子時，卡牌會吃掉 N 根，觸發額外效果。

兩個角色各有 22 張卡，包含自己的打擊和防禦，分布在普通、罕見和稀有三種稀有度。

## 多人共用內容

以下內容只在兩人以上的對局出現。

| 內容 | 效果 |
|---|---|
| 牽手 | 兩個角色起始牌組都有的基本卡。你和一名隊友獲得格擋。 |
| 分你一口 | 0 費。一名隊友獲得能量。消耗。 |
| 吵架 | 造成傷害，所有玩家各失去 2 點生命。 |
| 和好 | 你和一名隊友回復生命。消耗。 |
| 夾擊 | 造成傷害並給予易傷。 |
| 約定 | 稀有能力。你和一名隊友在每回合開始時獲得格擋。 |
| 兩個共用遺物 | 戰鬥開始時所有玩家獲得格擋；在休息處回血時讓隊友額外回血。 |

## 安裝

在 Steam 工作坊訂閱即可。這個 mod 需要 [BaseLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127)，訂閱時 Steam 會提示一起訂閱。

- 多人連線時，每位玩家都要裝同一個版本。
- 不要另外在遊戲的 `mods` 資料夾放一份手動安裝的版本。
- 有 mod 時遊戲使用另一組存檔，不會動到原本的進度。

只在遊戲 v0.107.1 測試過。

## 從原始碼建置

需要：

- .NET 9 SDK
- MegaDot 4.5.1（Godot 4.5.1 mono），用來匯出 `.pck`
- 已安裝《殺戮尖塔 2》

在 `Directory.Build.props` 設定 `GodotPath`；如果找不到遊戲，再設定 `Sts2Path`。這個檔案存的是本機路徑，所以不進 git。

```bash
dotnet publish SpireDuo.csproj
```

`dotnet build` 會把 DLL 複製到遊戲的 `mods/SpireDuo`。`dotnet publish` 另外匯出含圖片和文字的 `SpireDuo.pck`。

內部 mod id 維持 `SpireDuo`，顯示名稱寫在 `SpireDuo.json`。

## 專案結構

| 路徑 | 內容 |
|---|---|
| `SpireDuoCode/Character` | 角色、卡池、遺物池 |
| `SpireDuoCode/Cards` | 所有卡牌 |
| `SpireDuoCode/Powers` | 睡意、竹子等能力 |
| `SpireDuoCode/Relics` | 起始遺物與共用遺物 |
| `SpireDuo/localization` | `eng`、`zht`、`zhs` 三種語言的文字 |
| `SpireDuo/images` | 卡圖、圖示、角色圖 |
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
