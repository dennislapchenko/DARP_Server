
using System.Collections.Generic;
using RegionServer.Model.ServerEvents;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Interfaces
{
	public interface ICharacter
	{
		IObject Target {get; set;}
		void SwitchCurrentFightTarget();

		bool IsDead {get;}
		bool Moving {get; set;}
		Position Destination {get; set;}

		IList<ICharacter> StatusListeners {get;} //anyone who has this character targeted = will have characters things updated (hp etc)

		IStatHolder Stats {get;}
		IItemHolder Items {get;}
		GeneralStats GenStats {get; }
		Fight CurrentFight {get;}

		void BroadcastMessage(ServerPacket packet); //take and send a message to everything that can receive inside character known list (HP updates)
		void SendMessage(string text); //message to THIS class (Will wrap a packet and send it using SendPacket());

		void Teleport(Position pos);

		bool Die();

		void CalculateRewards(ICharacter killer);

		void BroadcastStatusUpdate(); //update to all status listeners
		void UpdateAndBroadcastStatus(int broadcastType); //overridden by other chars (used by player chars) (type: differentiate stats, or everything)
		void SendStateToPlayer(IObject owner);//used by Login function, if anything exists in world can be moved for player 
	}
}

