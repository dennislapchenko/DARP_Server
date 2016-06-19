using MMO.Framework;
using System;

namespace SubServerCommon.Data.ClientData
{
	public class CharacterData : IClientData
	{
		public int? CharacterId {get; set;}
		public int? UserId {get; set;}


		//Life-time stats
		public int? Achievements {get; set;}

		public int? TotalGoldAcquired {get; set;}

		public int? TotalDamageDone {get; set;}
		public int? TotalDamageTaken {get; set;}
		public int? TotalCritsDealt {get; set;}
		public int? TotalDodges {get; set;}
		public int? TotalParries {get; set;}
		public int? TotalCounterAttacks {get; set;}
	}
}

