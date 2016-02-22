using System;
using RegionServer.Model.Interfaces;
using MMO.Photon.Client;
using MMO.Photon.Server;
using ExitGames.Logging;
using RegionServer.Model.ServerEvents;
using ComplexServerCommon;
using Photon.SocketServer;
using RegionServer.Model.KnownList;
using MMO.Framework;


namespace RegionServer.Model
{
	public class CPlayerInstance : CPlayable, IPlayer, IClientData
	{

		public CPlayerInstance(Region region, PlayerKnownList objectKnownList, IPhysics physics) : base (region, objectKnownList)
		{
			Physics = physics;
			Destination = new Position();
		}


		public SubServerClientPeer Client {get; set;}
		public PhotonServerPeer ServerPeer {get; set;}
		public int? UserID {get; set;}
		public int? CharacterID {get; set;}
		public IPhysics Physics {get; set;}
		
		public override Position Position
		{
			get
			{
				base.Position = Physics.Position;
				return base.Position;
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
			obj.SendPacket(new CharInfoUpdate(this));
		}

		public void BroadcastUserInfo()
		{
			SendPacket(new UserInfoUpdate(this));
			BroadcastMessage(new CharInfoUpdate(this));
		}
	}
}

