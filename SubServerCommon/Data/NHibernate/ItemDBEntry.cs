namespace SubServerCommon.Data.NHibernate
{
	public class ItemDBEntry
	{
		public virtual string Name {get; set;}
		public virtual int ItemId {get; set;}
		public virtual int Type {get; set;}
		public virtual int Slot {get; set;}
		public virtual int Value {get; set;}
		public virtual int Equippable {get; set;}
		public virtual string AddedStats {get; set;}
		public virtual int LevelReq {get;set;}
		public virtual int minDamageValue {get; set;}
		public virtual int maxDamageValue {get; set;}
		public virtual int strengthValue {get; set;}
		public virtual int dexterityValue {get; set;}
		public virtual int instinctValue {get; set;}
		public virtual int staminaValue {get; set;}
		public virtual int criticalHitChanceValue {get; set;}
		public virtual int criticalDamageValue {get; set;}
		public virtual int dodgeChanceValue {get; set;}
		public virtual int counterAttackChanceValue {get; set;}
		public virtual int healthRegenValue {get; set;}
		//public virtual string Stats {get; set;}
	}
}

