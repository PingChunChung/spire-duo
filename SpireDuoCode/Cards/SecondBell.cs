using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SecondBell() : BlackCatCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{ protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await FootprintStatePower.UnlockAll(c, Owner.Creature, this); if (IsUpgraded) await CardPileCmd.Draw(c, 2, Owner); } protected override void OnUpgrade() { } }
