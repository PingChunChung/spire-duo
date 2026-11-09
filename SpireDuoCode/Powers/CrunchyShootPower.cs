namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Reduces the next Munch's Bamboo cost, then removes itself.</summary>
public sealed class CrunchyShootPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
