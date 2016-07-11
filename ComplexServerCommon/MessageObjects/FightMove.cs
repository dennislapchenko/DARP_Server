using ComplexServerCommon.Enums;
using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class FightMove
	{
		//public Guid? PeerId {get;set;}
        public int PeerObjectId;
        public HitSpot AttackSpot;
        public List<HitSpot> BlockSpots;
        public int? SkillId;
        public int TargetObjectId;
		//public Guid? TargetPeerId {get;set;}

		public override string ToString()
		{
			return string.Format("[FightMove: PeerObjectId={0}, AttackSpot={1}, BlockSpots={2}, SkillId={3}, TargetObjectId={4}]", PeerObjectId, AttackSpot, BlockSpots, SkillId, TargetObjectId);
		}
	}
}

