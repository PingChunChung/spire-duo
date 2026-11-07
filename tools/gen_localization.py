"""gen_localization.py <spire-duo dir>: writes eng / zht / zhs localization. zhs is converted from zht with a character table."""
import json
import pathlib
import sys

loc = pathlib.Path(sys.argv[1]) / "SpireDuo" / "localization"

D, B, CD = "{Damage:diff()}", "{Block:diff()}", "{CalculatedDamage:diff()}"
EX = "{ExtraDamage:diff()}"
DR, BA = "{DrowsyPower:diff()}", "{BambooPower:diff()}"
CARDS = "{Cards:diff()}"
GB_E, GB_Z = "[gold]Block[/gold]", "[gold]格擋[/gold]"
DRE, DRZ = "[gold]Drowsy[/gold]", "[gold]睡意[/gold]"
BAE, BAZ = "[gold]Bamboo[/gold]", "[gold]竹子[/gold]"
ME, MZ = "[gold]Munch[/gold]", "[gold]啃[/gold]"
card_word = lambda n: "card{Cards:plural:|s}"

cards = {
    "HOLD_HANDS": ("Hold Hands", f"You and an ally gain {B} {GB_E}.",
                   "牽手", f"你和一名隊友各獲得 {B} 點{GB_Z}。"),
    # Koala
    "STRIKE_KOALA": ("Strike", f"Deal {D} damage.", "打擊", f"造成 {D} 點傷害。"),
    "DEFEND_KOALA": ("Defend", f"Gain {B} {GB_E}.", "防禦", f"獲得 {B} 點{GB_Z}。"),
    "DOZE": ("Doze", f"Gain {B} {GB_E}.\nGain {DR} {DRE}.",
             "打盹", f"獲得 {B} 點{GB_Z}。\n獲得 {DR} 層{DRZ}。"),
    "EUCALYPTUS_SWIPE": ("Eucalyptus Swipe", f"Deal {D} damage.", "尤加利爪擊", f"造成 {D} 點傷害。"),
    "SLEEPY_HEADBUTT": ("Sleepy Headbutt", f"Deal {D} damage.\nGain {DR} {DRE}.",
                        "瞌睡頭槌", f"造成 {D} 點傷害。\n獲得 {DR} 層{DRZ}。"),
    "CURL_UP": ("Curl Up", f"Gain {B} {GB_E}.\nGain {DR} {DRE}.",
                "蜷成一團", f"獲得 {B} 點{GB_Z}。\n獲得 {DR} 層{DRZ}。"),
    "YAWN": ("Yawn", f"Gain {DR} {DRE}.", "打哈欠", f"獲得 {DR} 層{DRZ}。"),
    "TREE_HUG": ("Tree Hug", f"Gain {B} {GB_E}.\nDraw {CARDS} card.",
                 "抱樹", f"獲得 {B} 點{GB_Z}。\n抽 {CARDS} 張牌。"),
    "DROWSY_POUNCE": ("Drowsy Pounce", f"Deal {D} damage.\nDraw {CARDS} card.",
                      "迷糊飛撲", f"造成 {D} 點傷害。\n抽 {CARDS} 張牌。"),
    "WAKE_UP_CALL": ("Wake-Up Call",
                     f"Deal {CD} damage.\nDeals {EX} additional damage for each {DRE}.\nRemove all {DRE}.",
                     "起床氣", f"造成 {CD} 點傷害。\n每有 1 層{DRZ}，傷害 +{EX}。\n移除所有{DRZ}。"),
    "SLEEPWALK": ("Sleepwalk", f"Deal {CD} damage.\nDeals {EX} additional damage for each {DRE}.",
                  "夢遊", f"造成 {CD} 點傷害。\n每有 1 層{DRZ}，傷害 +{EX}。"),
    "FOREST_NAP": ("Forest Nap", f"Gain {B} {GB_E}.\nGain {DR} {DRE}.",
                   "森林午睡", f"獲得 {B} 點{GB_Z}。\n獲得 {DR} 層{DRZ}。"),
    "STRETCH": ("Stretch", f"Lose up to {DR} {DRE}.\nDraw {CARDS} cards.",
                "伸懶腰", f"失去至多 {DR} 層{DRZ}。\n抽 {CARDS} 張牌。"),
    "THICK_FUR": ("Thick Fur", "Gain {DexterityPower:diff()} [gold]Dexterity[/gold].",
                  "厚毛皮", "獲得 {DexterityPower:diff()} 點[gold]敏捷[/gold]。"),
    "DREAM_POWER": ("Dream Power", "Gain {StrengthPower:diff()} [gold]Strength[/gold].",
                    "夢中神力", "獲得 {StrengthPower:diff()} 點[gold]力量[/gold]。"),
    "HIBERNATE": ("Hibernate", f"At the start of your turn, gain {{HibernatePower:diff()}} {DRE}.",
                  "冬眠", f"在你的回合開始時，獲得 {{HibernatePower:diff()}} 層{DRZ}。"),
    "RUDE_AWAKENING": ("Rude Awakening",
                       f"Deal {CD} damage to ALL enemies.\nDeals {EX} additional damage for each {DRE}.\nRemove all {DRE}.",
                       "大夢初醒", f"對所有敵人造成 {CD} 點傷害。\n每有 1 層{DRZ}，傷害 +{EX}。\n移除所有{DRZ}。"),
    "SWEET_DREAMS": ("Sweet Dreams", f"Double your {DRE}.", "美夢成雙", f"你的{DRZ}加倍。"),
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
    "ROLL_OVER": ("Roll Over", f"Gain {B} {GB_E}.\nGain {BA} {BAE}.",
                  "打滾", f"獲得 {B} 點{GB_Z}。\n獲得 {BA} 根{BAZ}。"),
    "SHARE_BAMBOO": ("Share Bamboo",
                     f"An ally gains {B} {GB_E}.\n{ME} 2: They gain {{StrengthPower:diff()}} [gold]Strength[/gold].",
                     "分你竹子", f"一名隊友獲得 {B} 點{GB_Z}。\n{MZ} 2：隊友獲得 {{StrengthPower:diff()}} 點[gold]力量[/gold]。"),
    "BAMBOO_CHARGE": ("Bamboo Charge", f"Deal {D} damage.\n{ME} 3: Deal it again.",
                      "竹林衝撞", f"造成 {D} 點傷害。\n{MZ} 3：再造成一次。"),
    "FOOD_COMA": ("Food Coma", f"{ME} all your {BAE}.\nGain {{PerBamboo:diff()}} {GB_E} for each one eaten.",
                  "吃飽想睡", f"{MZ}掉所有{BAZ}。\n每啃 1 根獲得 {{PerBamboo:diff()}} 點{GB_Z}。"),
    "DARK_CIRCLES": ("Dark Circles", f"Deal {D} damage.\nApply {{VulnerablePower:diff()}} [gold]Vulnerable[/gold].",
                     "黑眼圈", f"造成 {D} 點傷害。\n給予 {{VulnerablePower:diff()}} 層[gold]易傷[/gold]。"),
    "TUMBLE": ("Tumble", f"Gain {B} {GB_E}.\n{ME} 1: Draw {CARDS} cards.",
               "翻跟斗", f"獲得 {B} 點{GB_Z}。\n{MZ} 1：抽 {CARDS} 張牌。"),
    "BAMBOO_GROVE": ("Bamboo Grove", f"At the start of your turn, gain {{BambooGrovePower:diff()}} {BAE}.",
                     "竹林", f"在你的回合開始時，獲得 {{BambooGrovePower:diff()}} 根{BAZ}。"),
    "PANDA_FIST": ("Panda Fist", f"Deal {CD} damage.\nDeals {EX} additional damage for each {BAE}.",
                   "熊貓拳", f"造成 {CD} 點傷害。\n每有 1 根{BAZ}，傷害 +{EX}。"),
    "NATIONAL_TREASURE": ("National Treasure",
                          "Gain {StrengthPower:diff()} [gold]Strength[/gold] and {DexterityPower:diff()} [gold]Dexterity[/gold].",
                          "國寶", "獲得 {StrengthPower:diff()} 點[gold]力量[/gold]和 {DexterityPower:diff()} 點[gold]敏捷[/gold]。"),
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
    # Duo (colorless, multiplayer only)
    "SHARE_A_BITE": ("Share a Bite", "An ally gains {Energy:energyIcons()}.",
                     "分你一口", "一名隊友獲得 {Energy:energyIcons()}。"),
    "QUARREL": ("Quarrel", f"Deal {D} damage.\nEvery player loses {{HpLoss:diff()}} HP.",
                "吵架", f"造成 {D} 點傷害。\n所有玩家各失去 {{HpLoss:diff()}} 點生命。"),
    "MAKE_UP": ("Make Up", "You and an ally heal {Heal:diff()} HP.",
                "和好", "你和一名隊友各回復 {Heal:diff()} 點生命。"),
    "PINCER": ("Pincer", f"Deal {D} damage.\nApply {{VulnerablePower:diff()}} [gold]Vulnerable[/gold].",
               "夾擊", f"造成 {D} 點傷害。\n給予 {{VulnerablePower:diff()}} 層[gold]易傷[/gold]。"),
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
relic_loc = localized(relics, ("title", "description", "flavor"))
characters["zhs"] = {k: zhs(v) for k, v in characters["zht"].items()}

for lang in ("eng", "zht", "zhs"):
    d = loc / lang
    (d / "cards.json").write_text(json.dumps(card_loc[lang], ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    merge(d / "powers.json", power_loc[lang])
    merge(d / "relics.json", relic_loc[lang])
    merge(d / "characters.json", characters[lang])
merge(loc / "eng" / "ancients.json", ancients)

leftover = {ch for v in list(card_loc["zhs"].values()) + list(power_loc["zhs"].values()) + list(relic_loc["zhs"].values())
            for ch in v if "一" <= ch <= "鿿" and ch not in T2S.values() and ch not in T2S}
print("unmapped chars:", "".join(sorted(leftover)))
