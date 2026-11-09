using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SilentHousehold() : BlackCatCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{ public override bool GainsBlock => true; protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7m, ValueProp.Move)]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { var selected = await CardSelectCmd.FromHand(c, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 0), x => x != this, this); foreach (var card in selected) { await CardCmd.Exhaust(c, card); await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p); } } protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2m); }
