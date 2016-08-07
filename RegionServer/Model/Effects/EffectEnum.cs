namespace RegionServer.Model.Effects
{
    public enum EffectEnum : byte
    {
        HEAL = 0,
        HEALTH = 4,
        DAMAGE = 6,
        INJURY = 7,

        HEAL_OVER_TURN = 1,
        THORNS = 2,
        TELEPORT_TO_WOODS = 5,
    }
}
