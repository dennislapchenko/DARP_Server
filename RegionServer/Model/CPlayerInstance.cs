using System;
using System.Collections.Generic;
using RegionServer.Model.Interfaces;
using MMO.Photon.Client;
using MMO.Photon.Server;
using RegionServer.Model.ServerEvents;
using ComplexServerCommon;
using Photon.SocketServer;
using RegionServer.Model.KnownList;
using MMO.Framework;
using RegionServer.Model.CharacterDatas;
using RegionServer.Model.Effects;
using RegionServer.Model.Fighting;
using RegionServer.Model.Stats;
using RegionServer.Persistence;

namespace RegionServer.Model
{
	public class CPlayerInstance : CPlayable, IPlayer, IClientData
	{


		public CPlayerInstance(FightManager fightManager, Region region, PlayerKnownList objectKnownList, 
            StatHolder stats, IItemHolder items, EffectHolder effects, IEnumerable<ICharacterData> characterDatas) 
			: base (region, objectKnownList, stats, items, effects, characterDatas)
		{
			Destination = new Position();
			FightManager = fightManager;

		}

		public SubServerClientPeer Client {get; set;}
		public PhotonServerPeer ServerPeer {get; set;}
		public FightManager FightManager {get;set;}
		public int? UserID {get; set;}
		public int? CharacterID {get; set;}


        public bool isNPC { get { return false; } }

        public override Position Position
		{ 
			get	
			{	
				return base.Position;	
			}
			set	
			{	
				base.Position = value;		
			}		
		}

		public override bool Moving
		{
			get
			{
				return base.Moving;
			}
			set
			{
				base.Moving = value;
			}
		}

		public new PlayerKnownList KnownList
		{
			get	{ return ObjectKnownList as PlayerKnownList; }
			set	{ ObjectKnownList = value; }
		}

		public override void BroadcastMessage(ServerPacket packet)
		{
			if (packet.SendToSelf)
			{
				SendPacket(packet);
			}

			foreach (CPlayerInstance player in KnownList.KnownPlayers.Values)
			{
				player.SendPacket(packet);
			}
		}

		public override void SendPacket(ServerPacket packet)
		{
			if (Client != null)
			{
				packet.AddParameter(Client.PeerId.ToByteArray(), ClientParameterCode.PeerId);
				ServerPeer.SendEvent(new EventData(packet.Code, packet.Parameters), new SendParameters());
			}
		}

		public override void SendInfo(IObject obj)
		{
			obj.SendPacket(new CharInfoUpdatePacket(this));
		}

		public void BroadcastUserInfo()
		{
			SendPacket(new UserInfoUpdatePacket(this));
			BroadcastMessage(new CharInfoUpdatePacket(this));
		}

		public override void DeleteMe()
		{
			CleanUp();
			Store();
			base.DeleteMe();
		}

		public void CleanUp()
		{
			//abort attacks
			//remove created queues
			FightManager.LeaveQueue(this);

			//remove temp items
			//remove LFG
			//Stop some timers
			//stop crafting

			Target = null;

			//stop temp buffs
			//decay from server
			Decay();
			//unsummon pets
			//notify guild/friends of logoff
			//cancel trading
		}

		// Save full character info to database from the passed reference
		public void Store()
		{
			PlayerStoreAccess storeAccess = new PlayerStoreAccess(this);
			storeAccess.execute();
		}

		// Restore full character info from database to the passed reference
		public void Restore()
		{
			PlayerRestoreAccess restoreAccess = new PlayerRestoreAccess(this);
			restoreAccess.execute();
		}
	}
}