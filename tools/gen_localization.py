"""gen_localization.py <spire-duo dir>: writes eng / zht / zhs localization. zhs is converted from zht with a character table."""
import json
import pathlib
import sys

loc = pathlib.Path(sys.argv[1]) / "SanaCkoala" / "localization"

D, B, CD = "{Damage:diff()}", "{Block:diff()}", "{CalculatedDamage:diff()}"
EX = "{ExtraDamage:diff()}"
DR, BA = "{DrowsyPower:diff()}", "{BambooPower:diff()}"
CARDS = "{Cards:diff()}"
GB_E, GB_Z = "[gold]Block[/gold]", "[gold]格擋[/gold]"
DRE, DRZ = "[gold]Drowsy[/gold]", "[gold]睡意[/gold]"
BAE, BAZ = "[gold]Bamboo[/gold]", "[gold]竹子[/gold]"
ME, MZ = "[gold]Munch[/gold]", "[gold]啃[/gold]"
card_word = lambda n: "card{Cards:plural:|s}"
CB, CX = "{CalculatedBlock:diff()}", "{CalculationExtra:diff()}"
INC, EN = "{Increase:diff()}", "{Energy:energyIcons()}"

cards = {
    "HOLD_HANDS": ("Hold Hands", f"You and an ally gain {B} {GB_E}.",
                   "牽手", f"你和一名隊友各獲得 {B} 點{GB_Z}。"),
    # Koala
    "STRIKE_KOALA": ("Strike", f"Deal {D} damage.", "打擊", f"造成 {D} 點傷害。"),
    "DEFEND_KOALA": ("Defend", f"Gain {B} {GB_E}.", "防禦", f"獲得 {B} 點{GB_Z}。"),
    "DOZE": ("Doze", f"Gain {B} {GB_E}.\nGain {DR} {DRE}.",
             "打盹", f"獲得 {B} 點{GB_Z}。\n獲得 {DR} 層{DRZ}。"),
    "EUCALYPTUS_SWIPE": ("Eucalyptus Swipe", f"Deal {D} damage to ALL enemies.",
                         "尤加利爪擊", f"對所有敵人造成 {D} 點傷害。"),
    "SLEEPY_HEADBUTT": ("Sleepy Headbutt",
                        f"Deal {D} damage.\nWhenever this card is [gold]Retained[/gold], lower its cost by [blue]1[/blue] until played.",
                        "瞌睡頭槌", f"造成 {D} 點傷害。\n每次被[gold]保留[/gold]時，這張牌在打出前的耗能減少 [blue]1[/blue]。"),
    "CURL_UP": ("Curl Up", f"Gain {B} {GB_E}.\nWhenever this card is [gold]Retained[/gold], increase its {GB_E} by {INC}.",
                "蜷成一團", f"獲得 {B} 點{GB_Z}。\n每次被[gold]保留[/gold]時，這張牌的{GB_Z}增加 {INC} 點。"),
    "YAWN": ("Yawn", f"Gain {DR} {DRE}.\nApply {{WeakPower:diff()}} [gold]Weak[/gold] to ALL enemies.",
             "打哈欠", f"獲得 {DR} 層{DRZ}。\n給予所有敵人 {{WeakPower:diff()}} 層[gold]虛弱[/gold]。"),
    "TREE_HUG": ("Tree Hug", f"Gain {B} {GB_E}.\nDraw {CARDS} card.",
                 "抱樹", f"獲得 {B} 點{GB_Z}。\n抽 {CARDS} 張牌。"),
    "DROWSY_POUNCE": ("Drowsy Pounce", f"Deal {D} damage.\nDraw {CARDS} card.",
                      "迷糊飛撲", f"造成 {D} 點傷害。\n抽 {CARDS} 張牌。"),
    "WAKE_UP_CALL": ("Wake-Up Call",
                     f"Deal {CD} damage.\nDeals {EX} additional damage for each {DRE}.\nRemove all {DRE}.",
                     "起床氣", f"造成 {CD} 點傷害。\n每有 1 層{DRZ}，傷害 +{EX}。\n移除所有{DRZ}。"),
    "SLEEPWALK": ("Sleepwalk", f"Deal {CD} damage.\nDeals {EX} additional damage for each {DRE}.",
                  "夢遊", f"造成 {CD} 點傷害。\n每有 1 層{DRZ}，傷害 +{EX}。"),
    "FOREST_NAP": ("Forest Nap", f"Gain {B} {GB_E}.\nNext turn, gain {EN} and draw {CARDS} additional card.",
                   "森林午睡", f"獲得 {B} 點{GB_Z}。\n下回合獲得 {EN}，並多抽 {CARDS} 張牌。"),
    "STRETCH": ("Stretch", f"Lose up to {DR} {DRE}.\nDraw {CARDS} cards.",
                "伸懶腰", f"失去至多 {DR} 層{DRZ}。\n抽 {CARDS} 張牌。"),
    "THICK_FUR": ("Thick Fur", "Gain {DexterityPower:diff()} [gold]Dexterity[/gold].",
                  "厚毛皮", "獲得 {DexterityPower:diff()} 點[gold]敏捷[/gold]。"),
    "DREAM_POWER": ("Dream Power",
                    f"At the start of your turn, gain {{DreamPowerPower:diff()}} [gold]Strength[/gold] for every [blue]4[/blue] {DRE} you have.",
                    "夢中神力", f"在你的回合開始時，每有 [blue]4[/blue] 層{DRZ}，獲得 {{DreamPowerPower:diff()}} 點[gold]力量[/gold]。"),
    "HIBERNATE": ("Hibernate", f"At the start of your turn, gain {{HibernatePower:diff()}} {DRE}.",
                  "冬眠", f"在你的回合開始時，獲得 {{HibernatePower:diff()}} 層{DRZ}。"),
    "RUDE_AWAKENING": ("Wide Awake", f"Remove all your {DRE}.\nFor every {{Per:diff()}} removed, gain {EN} and draw {CARDS} card.",
                       "大夢初醒", f"移除你所有的{DRZ}。\n每移除 {{Per:diff()}} 層，獲得 {EN} 並抽 {CARDS} 張牌。"),
    "SWEET_DREAMS": ("Sweet Dreams", f"Double your {DRE}.", "美夢成雙", f"你的{DRZ}加倍。"),
    "BACK_TO_SLEEP": ("Back to Sleep", f"Gain {DR} {DRE}.\nIf you had no {DRE}, gain {{Bonus:diff()}} more.",
                      "回籠覺", f"獲得 {DR} 層{DRZ}。\n若原本沒有{DRZ}，額外獲得 {{Bonus:diff()}} 層。"),
    "TAKE_IT_SLOW": ("Take It Slow", f"Deal {D} damage.\nWhenever this card is [gold]Retained[/gold], increase its damage by {INC}.",
                     "慢慢來", f"造成 {D} 點傷害。\n每次被[gold]保留[/gold]時，這張牌的傷害增加 {INC} 點。"),
    "DAYDREAM": ("Daydream", f"Draw {CARDS} {{Cards:plural:card|cards}}.\nWhenever this card is [gold]Retained[/gold], it draws [blue]1[/blue] more.",
                 "白日夢", f"抽 {CARDS} 張牌。\n每次被[gold]保留[/gold]時，多抽 [blue]1[/blue] 張。"),
    "LAZY_AFTERNOON": ("Lazy Afternoon",
                       f"At the end of your turn, gain {{LazyAfternoonPower:diff()}} {DRE} for each card you [gold]Retain[/gold].",
                       "慵懶午後", f"在你的回合結束時，每[gold]保留[/gold] 1 張牌，獲得 {{LazyAfternoonPower:diff()}} 層{DRZ}。"),
    "TUCK_IN": ("Tuck In", f"Gain {CX} {GB_E} for each other card in your hand.{{InCombat:\n(Gain {CB} {GB_E})|}}",
                "蓋好被子", f"手牌中每有 1 張其他牌，獲得 {CX} 點{GB_Z}。{{InCombat:\n（獲得 {CB} 點{GB_Z}）|}}"),
    "BLANKET_FORT": ("Blanket Fort", f"Gain {B} {GB_E}.\n[gold]Retain[/gold] your [gold]Hand[/gold] this turn.",
                     "棉被堡壘", f"獲得 {B} 點{GB_Z}。\n在本回合[gold]保留[/gold]你的[gold]手牌[/gold]。"),
    "SHORT_TEMPER": ("Short Temper", f"Whenever you remove all your {DRE}, deal damage equal to the amount removed to ALL enemies.",
                     "火氣很大", f"每次移除所有{DRZ}時，對所有敵人造成等同移除層數的傷害。"),
    "DREAM_CATCHER": ("Dreamweaver", f"Whenever you gain {DRE}, gain {{DreamCatcherPower:diff()}} more.",
                      "織夢網", f"每次獲得{DRZ}時，額外獲得 {{DreamCatcherPower:diff()}} 層。"),
    "NAP_TOGETHER": ("Nap Together", f"You and an ally gain {DR} {DRE}.",
                     "一起睡午覺", f"你和一名隊友各獲得 {DR} 層{DRZ}。"),
    "TREE_DROP": ("Tree Drop", f"Deal {D} damage to ALL enemies.", "從天而降", f"對所有敵人造成 {D} 點傷害。"),
    "SNORING": ("Snoring", f"At the end of your turn, deal damage equal to your {DRE} to a random enemy.",
                "打呼", f"在你的回合結束時，對隨機一名敵人造成等同{DRZ}層數的傷害。"),
    "PROCRASTINATION": ("Procrastination", "You no longer discard your [gold]Hand[/gold] at the end of your turn.",
                        "拖延症", "回合結束時，不再棄掉你的[gold]手牌[/gold]。"),
    "LAST_MINUTE_RUSH": ("Last-Minute Rush", f"Deal {EX} damage for each other card in your hand.{{InCombat:\n(Deals {CD} damage)|}}",
                         "臨時抱佛腳", f"手牌中每有 1 張其他牌，造成 {EX} 點傷害。{{InCombat:\n（造成 {CD} 點傷害）|}}"),
    "BEDTIME_STORY": ("Bedtime Story", f"Draw {CARDS} cards.\nThey gain [gold]Retain[/gold].",
                      "床邊故事", f"抽 {CARDS} 張牌。\n抽到的牌獲得[gold]保留[/gold]。"),
    # Panda
    "STRIKE_PANDA": ("Strike", f"Deal {D} damage.", "打擊", f"造成 {D} 點傷害。"),
    "DEFEND_PANDA": ("Defend", f"Gain {B} {GB_E}.", "防禦", f"獲得 {B} 點{GB_Z}。"),
    "GATHER_BAMBOO": ("Gather Bamboo", f"Gain {B} {GB_E}.\nGain {BA} {BAE}.",
                      "採竹子", f"獲得 {B} 點{GB_Z}。\n獲得 {BA} 根{BAZ}。"),
    "PAW_SLAP": ("Paw Slap", f"Deal {D} damage.\nGain {BA} {BAE}.",
                 "熊掌拍", f"造成 {D} 點傷害。\n獲得 {BA} 根{BAZ}。"),
    "BEAR_HUG": ("Bear Hug", f"Deal {D} damage.\n{ME} 2: Apply {{WeakPower:diff()}} [gold]Weak[/gold].",
                 "熊抱", f"造成 {D} 點傷害。\n{MZ} 2：給予 {{WeakPower:diff()}} 層[gold]虛弱[/gold]。"),
    "CHOMP": ("Chomp", f"Gain {B} {GB_E}.\n{ME} 2: Gain it again.",
              "啃竹子", f"獲得 {B} 點{GB_Z}。\n{MZ} 2：再獲得一次。"),
    "PLANT_BAMBOO": ("Plant Bamboo", f"Gain {BA} {BAE}.", "種竹子", f"獲得 {BA} 根{BAZ}。"),
    "ROLL_OVER": ("Roll Over", f"Gain {B} {GB_E}.\nGain [blue]1[/blue] [gold]Blur[/gold].",
                  "打滾", f"獲得 {B} 點{GB_Z}。\n獲得 [blue]1[/blue] 層[gold]殘影[/gold]。"),
    "SHARE_BAMBOO": ("Share Bamboo",
                     f"An ally gains {B} {GB_E}.\n{ME} 2: They gain {{StrengthPower:diff()}} [gold]Strength[/gold].",
                     "分你竹子", f"一名隊友獲得 {B} 點{GB_Z}。\n{MZ} 2：隊友獲得 {{StrengthPower:diff()}} 點[gold]力量[/gold]。"),
    "BAMBOO_CHARGE": ("Bamboo Charge", f"Deal {D} damage.\n{ME} 3: Deal it again.",
                      "竹林衝撞", f"造成 {D} 點傷害。\n{MZ} 3：再造成一次。"),
    "FOOD_COMA": ("Food Coma", f"{ME} all your {BAE}.\nGain {{PerBamboo:diff()}} {GB_E} for each one eaten.",
                  "吃飽想睡", f"{MZ}掉所有{BAZ}。\n每啃 1 根獲得 {{PerBamboo:diff()}} 點{GB_Z}。"),
    "DARK_CIRCLES": ("Dark Circles",
                     f"Gain {EN}.\nGain {BA} {BAE}.\nAdd a [gold]Dazed[/gold] into your [gold]Discard Pile[/gold].",
                     "黑眼圈", f"獲得 {EN}。\n獲得 {BA} 根{BAZ}。\n將一張[gold]暈眩[/gold]加入你的[gold]棄牌堆[/gold]。"),
    "TUMBLE": ("Tumble", f"Gain {B} {GB_E}.\n{ME} 1: Draw {CARDS} cards.",
               "翻跟斗", f"獲得 {B} 點{GB_Z}。\n{MZ} 1：抽 {CARDS} 張牌。"),
    "BAMBOO_GROVE": ("Bamboo Grove", f"At the start of your turn, gain {{BambooGrovePower:diff()}} {BAE}.",
                     "竹林", f"在你的回合開始時，獲得 {{BambooGrovePower:diff()}} 根{BAZ}。"),
    "PANDA_FIST": ("Panda Fist", f"Deal {CD} damage.\nDeals {EX} additional damage for each {BAE}.",
                   "熊貓拳", f"造成 {CD} 點傷害。\n每有 1 根{BAZ}，傷害 +{EX}。"),
    "NATIONAL_TREASURE": ("National Treasure",
                          "At the start of your turn, gain {NationalTreasurePower:diff()} [gold]Strength[/gold] and [gold]Dexterity[/gold].",
                          "國寶", "在你的回合開始時，獲得 {NationalTreasurePower:diff()} 點[gold]力量[/gold]和[gold]敏捷[/gold]。"),
    "BAMBOO_FEAST": ("Bamboo Feast", f"Gain {BA} {BAE}.\nDraw {CARDS} cards.",
                     "竹子大餐", f"獲得 {BA} 根{BAZ}。\n抽 {CARDS} 張牌。"),
    "IRON_HIDE": ("Iron Hide", f"Whenever you {ME}, gain {{IronHidePower:diff()}} {GB_E}.",
                  "鐵布衫", f"每次{MZ}竹子時，獲得 {{IronHidePower:diff()}} 點{GB_Z}。"),
    "BRANCH_POKE": ("Branch Poke", f"Deal {D} damage.\nGain {DR} {DRE}.",
                    "樹枝戳戳", f"造成 {D} 點傷害。\n獲得 {DR} 層{DRZ}。"),
    "SLOWPOKE": ("Slowpoke", f"Deal {D} damage.\nApply {{WeakPower:diff()}} [gold]Weak[/gold].",
                 "慢吞吞", f"造成 {D} 點傷害。\n給予 {{WeakPower:diff()}} 層[gold]虛弱[/gold]。"),
    "SNOOZE_BUTTON": ("Snooze Button", f"Gain {DR} {DRE}.\nDraw {CARDS} card.",
                      "再睡五分鐘", f"獲得 {DR} 層{DRZ}。\n抽 {CARDS} 張牌。"),
    "DREAM_SHIELD": ("Dream Shield", f"Gain {GB_E} equal to {{Multiplier:diff()}} times your {DRE}.",
                     "夢境結界", f"獲得等同{DRZ}層數 {{Multiplier:diff()}} 倍的{GB_Z}。"),
    "NIBBLE": ("Nibble", f"Deal {D} damage.\n{ME} 1: Draw {CARDS} card.",
               "咬一口", f"造成 {D} 點傷害。\n{MZ} 1：抽 {CARDS} 張牌。"),
    "ROLY_POLY": ("Roly-Poly", f"Gain {B} {GB_E}.\n{ME} 2: Gain {{DexterityPower:diff()}} [gold]Dexterity[/gold].",
                  "圓滾滾", f"獲得 {B} 點{GB_Z}。\n{MZ} 2：獲得 {{DexterityPower:diff()}} 點[gold]敏捷[/gold]。"),
    "BAMBOO_STICK": ("Bamboo Stick", f"Deal {D} damage.\n{ME} 1: Apply {{VulnerablePower:diff()}} [gold]Vulnerable[/gold].",
                     "竹棍敲", f"造成 {D} 點傷害。\n{MZ} 1：給予 {{VulnerablePower:diff()}} 層[gold]易傷[/gold]。"),
    "BIG_APPETITE": ("Big Appetite", f"Whenever you {ME}, gain {{BigAppetitePower:diff()}} [gold]Strength[/gold].",
                     "大胃王", f"每次{MZ}竹子時，獲得 {{BigAppetitePower:diff()}} 點[gold]力量[/gold]。"),
    "BELLY_BUMP": ("Belly Bump",
                   f"{ME} 1: Gain {B} {GB_E}.\nDeal damage equal to your {GB_E}.{{InCombat:\n(Deals {CD} damage)|}}",
                   "肚皮撞", f"{MZ} 1：獲得 {B} 點{GB_Z}。\n造成等同你{GB_Z}值的傷害。{{InCombat:\n（造成 {CD} 點傷害）|}}"),
    "PAW_COMBO": ("Paw Combo", f"Deal {D} damage {{Repeat:diff()}} times.\n{ME} 1: Hit [blue]1[/blue] more time.",
                  "連環掌", f"造成 {D} 點傷害 {{Repeat:diff()}} 次。\n{MZ} 1：多打 [blue]1[/blue] 次。"),
    "BODY_ROLL": ("Body Roll", f"Deal {D} damage to ALL enemies.\n{ME} 1: Gain {B} {GB_E}.",
                  "滾來滾去", f"對所有敵人造成 {D} 點傷害。\n{MZ} 1：獲得 {B} 點{GB_Z}。"),
    "BAMBOO_WALL": ("Bamboo Wall", f"Gain {CB} {GB_E}.\nGains {CX} additional {GB_E} for each {BAE}.",
                    "竹籬笆", f"獲得 {CB} 點{GB_Z}。\n每有 1 根{BAZ}，{GB_Z} +{CX}。"),
    "DIG_IN": ("Dig In", f"Whenever you {ME}, draw {{DigInPower:diff()}} card.",
               "開動了", f"每次{MZ}竹子時，抽 {{DigInPower:diff()}} 張牌。"),
    "SHELTER": ("Shelter", f"Gain {B} {GB_E}.\nAn ally gains {GB_E} equal to your {GB_E}.",
                "我罩你", f"獲得 {B} 點{GB_Z}。\n一名隊友獲得等同你{GB_Z}值的{GB_Z}。"),
    "HEAVY_PAW": ("Heavy Paw", f"Deal {CD} damage.\nDeals {EX} additional damage for each [gold]Strength[/gold].",
                  "千斤掌", f"造成 {CD} 點傷害。\n每有 1 點[gold]力量[/gold]，傷害 +{EX}。"),
    "BAMBOO_PROTEIN": ("Bamboo Protein", f"Gain {B} {GB_E}.\n{ME} 2: Gain {{StrengthPower:diff()}} [gold]Strength[/gold].",
                       "高蛋白竹子", f"獲得 {B} 點{GB_Z}。\n{MZ} 2：獲得 {{StrengthPower:diff()}} 點[gold]力量[/gold]。"),
    "DEEP_BREATH": ("Deep Breath", f"Double your {GB_E}.", "深呼吸", f"你的{GB_Z}加倍。"),
    "THICK_FAT": ("Thick Fat", f"{GB_E} is no longer removed at the start of your turn.",
                  "厚脂肪", f"{GB_Z}不再在你的回合開始時消失。"),
    "BOTTOMLESS_STOMACH": ("Bottomless Stomach",
                           f"{ME} costs {{BottomlessStomachPower:diff()}} less {BAE} (minimum [blue]1[/blue]).",
                           "無底胃", f"{MZ}少花 {{BottomlessStomachPower:diff()}} 根{BAZ}（至少 [blue]1[/blue] 根）。"),
    "BAMBOO_AVALANCHE": ("Bamboo Avalanche", f"{ME} all your {BAE}.\nDeal {D} damage to a random enemy for each one eaten.",
                         "竹子雪崩", f"{MZ}掉所有{BAZ}。\n每啃 1 根，對隨機一名敵人造成 {D} 點傷害。"),
    "STEAMROLLER": ("Steamroller", f"Deal damage equal to your {GB_E} to ALL enemies.{{InCombat:\n(Deals {CD} damage)|}}",
                    "壓路機", f"對所有敵人造成等同你{GB_Z}值的傷害。{{InCombat:\n（造成 {CD} 點傷害）|}}"),
    # Duo (colorless, multiplayer only)
    "SHARE_A_BITE": ("Share a Bite", "You and an ally each gain {Energy:energyIcons()}.",
                     "分你一口", "你和一名隊友各獲得 {Energy:energyIcons()}。"),
    "QUARREL": ("Quarrel", f"Deal {D} damage.\nEvery player loses {{HpLoss:diff()}} HP.",
                "吵架", f"造成 {D} 點傷害。\n所有玩家各失去 {{HpLoss:diff()}} 點生命。"),
    "MAKE_UP": ("Make Up", "You and an ally heal {Heal:diff()} HP.",
                "和好", "你和一名隊友各回復 {Heal:diff()} 點生命。"),
    "PINCER": ("Assist", f"Deal {D} damage.\nThis turn, an ally's next hit on this enemy deals {{PincerPower:diff()}} extra damage.",
               "助攻", f"造成 {D} 點傷害。\n本回合隊友下一次打中這名敵人時，額外造成 {{PincerPower:diff()}} 點傷害。"),
    "PROMISE": ("Promise", "You and an ally gain {PromisePower:diff()} [gold]Promise[/gold].",
                "約定", "你和一名隊友各獲得 {PromisePower:diff()} 層[gold]約定[/gold]。"),
}

MUNCH_E = f"{ME} N: if you have N {BAE}, spend them for the bonus."
powers = {
    "DROWSY_POWER": ("Drowsy", f"At the end of your turn, gain {GB_E} equal to your Drowsy.",
                     "At the end of your turn, gain [blue]{Amount}[/blue] [gold]Block[/gold].",
                     "睡意", f"在你的回合結束時，獲得等同睡意層數的{GB_Z}。",
                     "在你的回合結束時，獲得 [blue]{Amount}[/blue] 點[gold]格擋[/gold]。"),
    "BAMBOO_POWER": ("Bamboo", f"Cards that {ME} spend Bamboo for a bonus effect.",
                     f"You have [blue]{{Amount}}[/blue] Bamboo. Cards that {ME} spend it for a bonus effect.",
                     "竹子", f"帶有{MZ}的卡牌會吃掉竹子，觸發額外效果。",
                     f"你有 [blue]{{Amount}}[/blue] 根竹子。帶有{MZ}的卡牌會吃掉竹子，觸發額外效果。"),
    "HIBERNATE_POWER": ("Hibernate", f"At the start of your turn, gain {DRE}.",
                        f"At the start of your turn, gain [blue]{{Amount}}[/blue] {DRE}.",
                        "冬眠", f"在你的回合開始時，獲得{DRZ}。",
                        f"在你的回合開始時，獲得 [blue]{{Amount}}[/blue] 層{DRZ}。"),
    "BAMBOO_GROVE_POWER": ("Bamboo Grove", f"At the start of your turn, gain {BAE}.",
                           f"At the start of your turn, gain [blue]{{Amount}}[/blue] {BAE}.",
                           "竹林", f"在你的回合開始時，獲得{BAZ}。",
                           f"在你的回合開始時，獲得 [blue]{{Amount}}[/blue] 根{BAZ}。"),
    "BIG_APPETITE_POWER": ("Big Appetite", f"Whenever you {ME}, gain [gold]Strength[/gold].",
                           f"Whenever you {ME}, gain [blue]{{Amount}}[/blue] [gold]Strength[/gold].",
                           "大胃王", f"每次{MZ}竹子時，獲得[gold]力量[/gold]。",
                           f"每次{MZ}竹子時，獲得 [blue]{{Amount}}[/blue] 點[gold]力量[/gold]。"),
    "IRON_HIDE_POWER": ("Iron Hide", f"Whenever you {ME}, gain {GB_E}.",
                        f"Whenever you {ME}, gain [blue]{{Amount}}[/blue] {GB_E}.",
                        "鐵布衫", f"每次{MZ}竹子時，獲得{GB_Z}。",
                        f"每次{MZ}竹子時，獲得 [blue]{{Amount}}[/blue] 點{GB_Z}。"),
    "PROMISE_POWER": ("Promise", f"At the start of your turn, gain {GB_E}.",
                      f"At the start of your turn, gain [blue]{{Amount}}[/blue] {GB_E}.",
                      "約定", f"在你的回合開始時，獲得{GB_Z}。",
                      f"在你的回合開始時，獲得 [blue]{{Amount}}[/blue] 點{GB_Z}。"),
    "SNORING_POWER": ("Snoring", f"At the end of your turn, deal damage equal to your {DRE} to a random enemy.",
                      f"At the end of your turn, deal damage equal to your {DRE} to a random enemy "
                      f"[blue]{{Amount}}[/blue] {{Amount:plural:time|times}}.",
                      "打呼", f"在你的回合結束時，對隨機一名敵人造成等同{DRZ}層數的傷害。",
                      f"在你的回合結束時，對隨機敵人造成等同{DRZ}層數的傷害，共 [blue]{{Amount}}[/blue] 次。"),
    "LAZY_AFTERNOON_POWER": ("Lazy Afternoon", f"At the end of your turn, gain {DRE} for each card you [gold]Retain[/gold].",
                             f"At the end of your turn, gain [blue]{{Amount}}[/blue] {DRE} for each card you [gold]Retain[/gold].",
                             "慵懶午後", f"在你的回合結束時，每[gold]保留[/gold] 1 張牌就獲得{DRZ}。",
                             f"在你的回合結束時，每[gold]保留[/gold] 1 張牌，獲得 [blue]{{Amount}}[/blue] 層{DRZ}。"),
    "SHORT_TEMPER_POWER": ("Short Temper", f"Whenever you remove all your {DRE}, ALL enemies take damage based on the amount removed.",
                           f"Whenever you remove all your {DRE}, ALL enemies take [blue]{{Amount}}[/blue] damage for each one removed.",
                           "火氣很大", f"每次移除所有{DRZ}時，依移除的層數對所有敵人造成傷害。",
                           f"每次移除所有{DRZ}時，每移除 1 層，對所有敵人造成 [blue]{{Amount}}[/blue] 點傷害。"),
    "DREAM_CATCHER_POWER": ("Dreamweaver", f"Whenever you gain {DRE}, gain more.",
                            f"Whenever you gain {DRE}, gain [blue]{{Amount}}[/blue] more.",
                            "織夢網", f"每次獲得{DRZ}時，額外多獲得一些。",
                            f"每次獲得{DRZ}時，額外獲得 [blue]{{Amount}}[/blue] 層。"),
    "PROCRASTINATION_POWER": ("Procrastination", "You no longer discard your [gold]Hand[/gold] at the end of your turn.",
                              "You no longer discard your [gold]Hand[/gold] at the end of your turn.",
                              "拖延症", "回合結束時，不再棄掉你的[gold]手牌[/gold]。",
                              "回合結束時，不再棄掉你的[gold]手牌[/gold]。"),
    "DREAM_POWER_POWER": ("Dream Power", f"At the start of your turn, gain [gold]Strength[/gold] for every [blue]4[/blue] {DRE}.",
                          f"At the start of your turn, gain [blue]{{Amount}}[/blue] [gold]Strength[/gold] for every [blue]4[/blue] {DRE}.",
                          "夢中神力", f"在你的回合開始時，每有 [blue]4[/blue] 層{DRZ}就獲得[gold]力量[/gold]。",
                          f"在你的回合開始時，每有 [blue]4[/blue] 層{DRZ}，獲得 [blue]{{Amount}}[/blue] 點[gold]力量[/gold]。"),
    "DIG_IN_POWER": ("Dig In", f"Whenever you {ME}, draw cards.",
                     f"Whenever you {ME}, draw [blue]{{Amount}}[/blue] {{Amount:plural:card|cards}}.",
                     "開動了", f"每次{MZ}竹子時，抽牌。",
                     f"每次{MZ}竹子時，抽 [blue]{{Amount}}[/blue] 張牌。"),
    "NATIONAL_TREASURE_POWER": ("National Treasure", "At the start of your turn, gain [gold]Strength[/gold] and [gold]Dexterity[/gold].",
                                "At the start of your turn, gain [blue]{Amount}[/blue] [gold]Strength[/gold] and [gold]Dexterity[/gold].",
                                "國寶", "在你的回合開始時，獲得[gold]力量[/gold]和[gold]敏捷[/gold]。",
                                "在你的回合開始時，獲得 [blue]{Amount}[/blue] 點[gold]力量[/gold]和[gold]敏捷[/gold]。"),
    "PINCER_POWER": ("Opening", "This turn, an ally's next hit on this enemy deals extra damage.",
                     "This turn, an ally's next hit on this enemy deals [blue]{Amount}[/blue] extra damage.",
                     "破綻", "本回合隊友下一次打中這名敵人時，額外造成傷害。",
                     "本回合隊友下一次打中這名敵人時，額外造成 [blue]{Amount}[/blue] 點傷害。"),
    "BOTTOMLESS_STOMACH_POWER": ("Bottomless Stomach", f"{ME} costs less {BAE} (minimum [blue]1[/blue]).",
                                 f"{ME} costs [blue]{{Amount}}[/blue] less {BAE} (minimum [blue]1[/blue]).",
                                 "無底胃", f"{MZ}少花一些{BAZ}（至少 [blue]1[/blue] 根）。",
                                 f"{MZ}少花 [blue]{{Amount}}[/blue] 根{BAZ}（至少 [blue]1[/blue] 根）。"),
}

relics = {
    "EUCALYPTUS_LEAF": ("Eucalyptus Leaf", f"At the start of each combat, gain [blue]{{DrowsyPower}}[/blue] {DRE}.",
                        "Koalas sleep up to twenty hours a day. This is why.",
                        "尤加利葉", f"在每場戰鬥開始時，獲得 [blue]{{DrowsyPower}}[/blue] 層{DRZ}。",
                        "無尾熊一天睡二十個小時，都是它害的。"),
    "BAMBOO_BASKET": ("Bamboo Basket", f"At the start of each combat, gain [blue]{{BambooPower}}[/blue] {BAE}.",
                      "Always packed. Always shared.",
                      "竹籃", f"在每場戰鬥開始時，獲得 [blue]{{BambooPower}}[/blue] 根{BAZ}。",
                      "永遠裝滿，永遠分你一半。"),
    "COUPLE_RINGS": ("Couple Rings", f"At the start of each combat, every player gains [blue]{{Block}}[/blue] {GB_E}.",
                     "One for you, one for me.",
                     "情侶對戒", f"在每場戰鬥開始時，所有玩家各獲得 [blue]{{Block}}[/blue] 點{GB_Z}。",
                     "一只給你，一只給我。"),
    "SPARE_KEY": ("Spare Key", "Whenever you heal at a Rest Site, every other player heals [blue]{Heal}[/blue] HP.",
                  "Come in, the door's always open.",
                  "備用鑰匙", "在休息處回復生命時，其他玩家各回復 [blue]{Heal}[/blue] 點生命。",
                  "直接進來，門永遠為你開著。"),
    "FLUFFY_PILLOW": ("Fluffy Pillow", f"At the end of your turn, gain [blue]{{Block}}[/blue] {GB_E} for each card you [gold]Retain[/gold].",
                      "Still warm.",
                      "蓬鬆枕頭", f"在你的回合結束時，每[gold]保留[/gold] 1 張牌，獲得 [blue]{{Block}}[/blue] 點{GB_Z}。",
                      "還是暖的。"),
    "ALARM_CLOCK": ("Alarm Clock", f"Whenever you remove all your {DRE}, draw [blue]{{Cards}}[/blue] cards.",
                    "It rings every morning. Nobody gets up.",
                    "鬧鐘", f"每次移除所有{DRZ}時，抽 [blue]{{Cards}}[/blue] 張牌。",
                    "每天早上都響，從來沒人起床。"),
    "NIGHT_CAP": ("Night Cap", f"At the start of your turn, gain [blue]{{DrowsyPower}}[/blue] {DRE}.",
                  "Fashion is optional. Sleep is not.",
                  "睡帽", f"在你的回合開始時，獲得 [blue]{{DrowsyPower}}[/blue] 層{DRZ}。",
                  "時尚可以不要，覺不能不睡。"),
    "BODY_PILLOW": ("Body Pillow", f"Whenever you gain {DRE}, gain that much {GB_E}.",
                    "Hug it tight and drift off.",
                    "抱枕", f"每次獲得{DRZ}時，獲得等量的{GB_Z}。",
                    "抱緊就睡著了。"),
    "CHOPSTICKS": ("Chopsticks", f"Whenever you can't pay for a {ME}, gain [blue]{{BambooPower}}[/blue] {BAE}.",
                   "Two sticks, endless snacks.",
                   "筷子", f"每次竹子不夠{MZ}時，獲得 [blue]{{BambooPower}}[/blue] 根{BAZ}。",
                   "兩根棒子，吃不完的點心。"),
    "RICE_BOWL": ("Rice Bowl",
                  f"At the end of your turn, if you have [blue]{{Threshold}}[/blue] or more {BAE}, gain [blue]{{Block}}[/blue] {GB_E}.",
                  "There's always room for more.",
                  "飯碗", f"在你的回合結束時，若有至少 [blue]{{Threshold}}[/blue] 根{BAZ}，獲得 [blue]{{Block}}[/blue] 點{GB_Z}。",
                  "永遠還裝得下。"),
    "GOLDEN_BAMBOO": ("Golden Bamboo", f"At the start of your turn, gain [blue]{{BambooPower}}[/blue] {BAE}.",
                      "Too pretty to eat. Eaten anyway.",
                      "金竹子", f"在你的回合開始時，獲得 [blue]{{BambooPower}}[/blue] 根{BAZ}。",
                      "漂亮到捨不得吃，結果還是吃了。"),
    "LUNCH_BOX": ("Lunch Box", f"At the start of each combat, gain [blue]{{BambooPower}}[/blue] {BAE}.",
                  "Packed the night before.",
                  "便當盒", f"在每場戰鬥開始時，獲得 [blue]{{BambooPower}}[/blue] 根{BAZ}。",
                  "前一晚就裝好了。"),
    # Starter relics upgraded by Touch of Orobas
    "EUCALYPTUS_CROWN": ("Eucalyptus Crown", f"At the start of each combat, gain [blue]{{DrowsyPower}}[/blue] {DRE}.",
                         "Heavy is the head that naps.",
                         "尤加利花冠", f"在每場戰鬥開始時，獲得 [blue]{{DrowsyPower}}[/blue] 層{DRZ}。",
                         "頭越重，睡得越沉。"),
    "BAMBOO_HAMPER": ("Bamboo Hamper", f"At the start of each combat, gain [blue]{{BambooPower}}[/blue] {BAE}.",
                      "Packed for two. Empty by noon.",
                      "野餐竹籃", f"在每場戰鬥開始時，獲得 [blue]{{BambooPower}}[/blue] 根{BAZ}。",
                      "裝了兩人份，中午前就見底。"),
}

potions = {
    "WARM_MILK": ("Warm Milk", f"Gain [blue]{{DrowsyPower}}[/blue] {DRE}.",
                  "熱牛奶", f"獲得 [blue]{{DrowsyPower}}[/blue] 層{DRZ}。"),
    "STRONG_COFFEE": ("Strong Coffee",
                      f"Remove all your {DRE}.\nDeal [blue]{{PerDrowsy}}[/blue] damage to ALL enemies for each one removed.",
                      "濃縮咖啡", f"移除你所有的{DRZ}。\n每移除 1 層，對所有敵人造成 [blue]{{PerDrowsy}}[/blue] 點傷害。"),
    "BAMBOO_JUICE": ("Bamboo Juice", f"Gain [blue]{{BambooPower}}[/blue] {BAE}.",
                     "竹子汁", f"獲得 [blue]{{BambooPower}}[/blue] 根{BAZ}。"),
    "HONEY_JAR": ("Honey Jar", f"Gain [blue]{{Block}}[/blue] {GB_E}.\nGain [blue]1[/blue] [gold]Blur[/gold].",
                  "蜂蜜罐", f"獲得 [blue]{{Block}}[/blue] 點{GB_Z}。\n獲得 [blue]1[/blue] 層[gold]殘影[/gold]。"),
}

characters = {
    "eng": {
        "SPIREDUO-KOALA.description": "A sleepy marsupial. Builds up [gold]Drowsy[/gold] for Block, then wakes up all at once.",
        "SPIREDUO-PANDA.title": "The Panda",
        "SPIREDUO-PANDA.titleObject": "the Panda",
        "SPIREDUO-PANDA.description": "A sturdy bear with a basket of [gold]Bamboo[/gold], always ready to share a bite.",
        "SPIREDUO-PANDA.pronounObject": "them",
        "SPIREDUO-PANDA.possessiveAdjective": "their",
        "SPIREDUO-PANDA.pronounPossessive": "theirs",
        "SPIREDUO-PANDA.pronounSubject": "they",
        "SPIREDUO-PANDA.goldMonologue": "Can gold buy bamboo?",
        "SPIREDUO-PANDA.eventDeathPrevention": "Not before dinner.",
        "SPIREDUO-PANDA.aromaPrinciple": "Bamboo",
        "SPIREDUO-PANDA.cardsModifierTitle": "Panda Cards",
        "SPIREDUO-PANDA.cardsModifierDescription": "Panda cards will appear in rewards.",
        "SPIREDUO-PANDA.banter.alive.endTurnPing": "Your turn!",
        "SPIREDUO-PANDA.banter.dead.endTurnPing": "...save me some bamboo",
    },
    "zht": {
        "SPIREDUO-KOALA.description": "愛睡的有袋動物。累積[gold]睡意[/gold]換取格擋，再一口氣醒來爆發。",
        "SPIREDUO-PANDA.title": "熊貓",
        "SPIREDUO-PANDA.titleObject": "熊貓",
        "SPIREDUO-PANDA.description": "揹著一籃[gold]竹子[/gold]的結實熊貓，隨時準備分你一口。",
    },
}
ancients = {
    "THE_ARCHITECT.talk.SPIREDUO-PANDA.0-0r.char": "Is there bamboo up here?",
    "THE_ARCHITECT.talk.SPIREDUO-PANDA.0-0r.next": "Continue",
    "THE_ARCHITECT.talk.SPIREDUO-PANDA.0-1r.ancient": "You climbed all this way for a snack?",
    "THE_ARCHITECT.talk.SPIREDUO-PANDA.0-attack": "Attack",
}
# The -attack keys are parsed as an ArchitectAttackers name, not shown, so they stay English-only.
ancients_zht = {
    "THE_ARCHITECT.talk.SPIREDUO-KOALA.0-0r.char": "這裡就是塔頂了嗎？",
    "THE_ARCHITECT.talk.SPIREDUO-KOALA.0-0r.next": "繼續",
    "THE_ARCHITECT.talk.SPIREDUO-KOALA.0-1r.ancient": "你爬了這麼高，就只是為了睡午覺？",
    "THE_ARCHITECT.talk.SPIREDUO-PANDA.0-0r.char": "上面有竹子嗎？",
    "THE_ARCHITECT.talk.SPIREDUO-PANDA.0-0r.next": "繼續",
    "THE_ARCHITECT.talk.SPIREDUO-PANDA.0-1r.ancient": "你爬了這麼高，就只是為了找點心？",
}

# Traditional -> Simplified for every character used above that differs.
T2S = {}
T2S.update({"圓": "圆", "鐘": "钟", "結": "结", "界": "界", "敲": "敲", "咬": "咬", "棍": "棍", "胃": "胃", "王": "王", "戳": "戳", "慢": "慢", "吞": "吞", "來": "来", "著": "着", "牽": "牵", "無": "无", "戰": "战", "發": "发", "擊": "击", "禦": "御", "獲": "获", "層": "层", "隊": "队", "頭": "头", "張": "张", "飛": "飞", "撲": "扑", "氣": "气", "對": "对", "夢": "梦", "遊": "游", "懶": "懒", "醒": "醒", "給": "给", "予": "予", "虛": "虚", "弱": "弱", "飽": "饱", "傷": "伤", "鐵": "铁", "點": "点", "樹": "树", "擋": "挡", "於": "于", "麼": "么", "為": "为", "與": "与", "憂": "忧", "盹": "盹", "睏": "困",
            "臉": "脸", "蜷": "蜷", "團": "团", "伸": "伸", "睡": "睡", "竹": "竹", "啃": "啃", "掉": "掉",
            "吃": "吃", "想": "想", "冬": "冬", "眠": "眠", "林": "林", "拳": "拳", "布": "布", "衫": "衫",
            "嗎": "吗", "個": "个", "葉": "叶", "尤": "尤", "加": "加", "利": "利", "易": "易", "誰": "谁",
            "雙": "双", "倍": "倍", "移": "移", "除": "除", "所": "所", "有": "有", "多": "多", "失": "失",
            "去": "去", "至": "至", "抽": "抽", "抱": "抱", "迷": "迷", "糊": "糊", "瞌": "瞌", "欠": "欠",
            "哈": "哈", "打": "打", "防": "防", "造": "造", "成": "成", "敵": "敌", "人": "人", "大": "大",
            "初": "初", "美": "美", "神": "神", "力": "力", "量": "量", "毛": "毛", "皮": "皮", "森": "森",
            "午": "午", "實": "实", "結實": "结实", "動": "动", "物": "物", "愛": "爱", "換": "换", "取": "取",
            "積": "积", "累": "累", "爆": "爆", "口": "口", "再": "再", "隨": "随", "準": "准", "備": "备",
            "分": "分", "你": "你", "一": "一", "半": "半", "永": "永", "遠": "远", "滿": "满", "裝": "装",
            "天": "天", "二": "二", "十": "十", "小": "小", "都": "都", "是": "是", "它": "它", "害": "害",
            "的": "的", "在": "在", "場": "场", "鬥": "斗", "開": "开", "始": "始", "時": "时", "等": "等",
            "同": "同", "數": "数", "結": "结", "束": "束", "合": "合", "回": "回", "根": "根", "觸": "触",
            "效": "效", "果": "果", "外": "外", "額": "额", "帶": "带", "會": "会", "牌": "牌", "卡": "卡",
            "次": "次", "每": "每", "和": "和", "國": "国", "寶": "宝", "餐": "餐", "子": "子", "衝": "冲",
            "撞": "撞", "眼": "眼", "圈": "圈", "黑": "黑", "跟": "跟", "斗": "斗", "翻": "翻", "滾": "滚",
            "種": "种", "採": "采", "拍": "拍", "掌": "掌", "熊": "熊", "貓": "猫", "揹": "背", "籃": "篮", "隊": "队", "獲": "获", "傷": "伤", "給": "给", "對": "对", "戒": "戒", "鑰": "钥", "匙": "匙", "備": "备", "處": "处", "復": "复", "開": "开", "門": "门", "遠": "远", "為": "为", "進": "进", "約": "约", "定": "定", "吵": "吵", "架": "架", "夾": "夹", "擊": "击", "失": "失", "去": "去", "命": "命", "玩": "玩", "家": "家", "所": "所", "各": "各", "侶": "侣", "情": "情", "只": "只", "來": "来", "著": "着", "息": "息", "休": "休", "時": "时", "場": "场", "戰": "战", "鬥": "斗", "點": "点", "層": "层", "易": "易"})
T2S.update({"並": "并", "兩": "两", "壓": "压", "壘": "垒", "夠": "够", "將": "将", "後": "后", "從": "从", "捨": "舍", "暈": "晕", "棄": "弃", "機": "机", "殘": "残", "沒": "没", "減": "减", "濃": "浓", "熱": "热", "環": "环", "當": "当", "籠": "笼", "籬": "篱", "網": "网", "緊": "紧", "縮": "缩", "腳": "脚", "臨": "临", "蓋": "盖", "覺": "觉", "這": "这", "連": "连", "過": "过", "還": "还", "邊": "边", "響": "响", "飯": "饭", "鬆": "松", "鬧": "闹", "織": "织", "綻": "绽"})
T2S.update({"裡": "里", "頂": "顶", "繼": "继", "續": "续", "見": "见", "總": "总"})


def zhs(text):
    return "".join(T2S.get(ch, ch) for ch in text)


def merge(path, entries):
    data = json.loads(path.read_text(encoding="utf-8")) if path.exists() else {}
    data.update(entries)
    path.write_text(json.dumps(data, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")


def localized(table, fields_per_lang):
    out = {"eng": {}, "zht": {}, "zhs": {}}
    for key, values in table.items():
        n = fields_per_lang
        eng, zht = values[:len(values) // 2], values[len(values) // 2:]
        for field, e, z in zip(n, eng, zht):
            out["eng"][f"SPIREDUO-{key}.{field}"] = e
            out["zht"][f"SPIREDUO-{key}.{field}"] = z
            out["zhs"][f"SPIREDUO-{key}.{field}"] = zhs(z)
    return out


card_loc = localized(cards, ("title", "description"))
power_loc = localized(powers, ("title", "description", "smartDescription"))
# Shown instead of smartDescription to players other than the one who applied the power.
remote_powers = {
    "PINCER_POWER": ("This turn, your next hit on this enemy deals [blue]{Amount}[/blue] extra damage.",
                     "本回合你下一次打中這名敵人時，額外造成 [blue]{Amount}[/blue] 點傷害。"),
}
for lang, entries in localized(remote_powers, ("remoteDescription",)).items():
    power_loc[lang].update(entries)
relic_loc = localized(relics, ("title", "description", "flavor"))
potion_loc = localized(potions, ("title", "description"))
characters["zhs"] = {k: zhs(v) for k, v in characters["zht"].items()}

for lang in ("eng", "zht", "zhs"):
    d = loc / lang
    (d / "cards.json").write_text(json.dumps(card_loc[lang], ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    merge(d / "powers.json", power_loc[lang])
    merge(d / "relics.json", relic_loc[lang])
    merge(d / "potions.json", potion_loc[lang])
    merge(d / "characters.json", characters[lang])
merge(loc / "eng" / "ancients.json", ancients)
merge(loc / "zht" / "ancients.json", ancients_zht)
merge(loc / "zhs" / "ancients.json", {k: zhs(v) for k, v in ancients_zht.items()})

leftover = {ch for v in list(card_loc["zhs"].values()) + list(power_loc["zhs"].values()) + list(relic_loc["zhs"].values())
            + list(potion_loc["zhs"].values()) + [zhs(v) for v in ancients_zht.values()]
            for ch in v if "一" <= ch <= "鿿" and ch not in T2S.values() and ch not in T2S}
print("unmapped chars:", "".join(sorted(leftover)))
