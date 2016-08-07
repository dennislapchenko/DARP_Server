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
		public virtual int LevelReq {get;set;}

        public virtual string Stats { get; set; }
	}
}