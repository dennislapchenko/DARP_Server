namespace RegionServer.Model.Effects.Definitions
{
    public struct StatBonus
    {
        public int AdditiveBonus { get; private set; }
        public float MultiplicativeBonus { get; private set; }

        public StatBonus(int add, float mult)
        {
            AdditiveBonus = add;
            MultiplicativeBonus = mult;
        }

        public static StatBonus New(int add, float mult)
        {
            return new StatBonus(add, mult);
        }

        public void Flip()
        {
            AdditiveBonus *= -1;
            MultiplicativeBonus *= -1.0f;
        }
    }
}
