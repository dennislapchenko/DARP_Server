using System;
using ComplexServerCommon.Enums;
using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class FightMove
	{
		//public Guid? PeerId {get;set;}
		public int PeerObjectId {get;set;}
		public HitSpot AttackSpot{get;set;}
		public List<HitSpot> BlockSpots{get;set;}
		public int? SkillId {get;set;}
		public int TargetObjectId {get;set;}
		//public Guid? TargetPeerId {get;set;}

		public override string ToString()
		{
			return string.Format("[FightMove: PeerObjectId={0}, AttackSpot={1}, BlockSpots={2}, SkillId={3}, TargetObjectId={4}]", PeerObjectId, AttackSpot, BlockSpots, SkillId, TargetObjectId);
		}
	}
}

