using ComplexServerCommon.MessageObjects.Enums;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class ExchangeProfile
    {
        public int objectId;
        public MoveOutcome outcome;
        public int damage;
        public int totalDamage;

		public ExchangeProfile()
		{
		}
	}
}

