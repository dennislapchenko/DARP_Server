
using System.Collections.Generic;
using RegionServer.Model.ServerEvents;

namespace RegionServer.Model.Interfaces
{
	public interface ICharacter
	{
		IObject Target {get; set;}
		int TargetId {get;}
		bool IsTeleporting {get;}
		bool IsDead {get;}
		Position Destination {get; set;}
		IList<ICharacter> StatusListeners {get;} //anyone who has this character targeted = will have characters things updated (hp etc)
		IStatHolder Stats {get;}

		void BroadcastMessage(ServerPacket packet); //take and send a message to everything that can receive inside character known list (HP updates)
		void SendMessage(string text); //message to THIS class (Will wrap a packet and send it using SendPacket());

		void Teleport(Position pos);
		void Teleport(float x, float y, float z, short heading);
		void Teleport(float x, float y, float z);
		void Teleport(ITeleportType teleportType); //tp to gates, summon stones
		bool Die(ICharacter killer); //generate loot, give exp (bool is for certain NPCs or chars to NOT die)
		void StopMove(Position pos);
		void CalculateRewards(ICharacter killer);
		void BroadcastStatusUpdate(); //update to all status listeners
		void UpdateAndBroadcastStatus(int broadcastType); //overridden by other chars (used by player chars) (type: differentiate stats, or everything)
		void SendStateToPlayer(IObject owner);//used by Login function, if anything exists in world can be moved for player 






	}
}

