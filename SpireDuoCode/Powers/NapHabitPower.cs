namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner plays a Skill, they gain Drowsy.</summary>
public sealed class NapHabitPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Skill) return;
        Flash();
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner, Amount, Owner, null);
    }
}
