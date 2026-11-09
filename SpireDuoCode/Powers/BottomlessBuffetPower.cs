namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Marks the first Munches in combat as cheaper; BambooPower also reads its Strength payoff.</summary>
public sealed class BottomlessBuffetPower : SpireDuoPower
{
    private sealed class Data { public int Used; }
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override object InitInternalData() => new Data();
    internal bool TryDiscountMunch()
    {
        var data = GetInternalData<Data>();
        if (data.Used >= Amount) return false;
        data.Used++;
        Flash();
        return true;
    }
}
