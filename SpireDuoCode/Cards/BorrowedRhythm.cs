using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BorrowedRhythm() : BlackCatCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        if (play.Target?.Player is { } ally && !play.Target.IsDead)
        {
            await PowerCmd.Apply<BorrowedRhythmPower>(context, play.Target, 1, Owner.Creature, this);
            if (IsUpgraded)
                await CardPileCmd.Draw(context, 1, ally);
        }
    }
    protected override void OnUpgrade() { }
}
