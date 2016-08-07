namespace RegionServer.Model.Effects.Definitions
{
    public interface IEffectSpell : IEffect
    {
        string Name { get; }
        string Description { get; }
        byte UnlockLevel { get; }
    }
}
