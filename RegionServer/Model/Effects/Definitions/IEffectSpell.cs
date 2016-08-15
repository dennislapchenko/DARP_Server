namespace RegionServer.Model.Effects.Definitions
{
    public interface IEffectSpell : IEffect
    {
        byte UnlockLevel { get; }
    }
}
