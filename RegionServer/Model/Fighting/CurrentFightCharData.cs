using ComplexServerCommon.MessageObjects;
using ComplexServerCommon.Enums;

namespace RegionServer.Model.Fighting
{
	public struct CurrentFightCharData
	{
		public FightTeam Team {get;set;}
		public int TotalDamage {get;set;}
		public bool Injuried {get;set;}
		public FightWinLossTie? WLT {get; set;}

        public short NumHits { get; set; }
        public short NumCrits { get; set; }
        public short NumDodges { get; set; }
        public short NumBlocks { get; set; }
        public short NumBlockCrits { get; set; }

        public override string ToString()
		{
			return string.Format("[CurrentFightCharData: Team={0}, TotalDamage={1}, Injuried={2}, WLT={3}]", Team, TotalDamage, Injuried, WLT.ToString());
		}
	}
}

