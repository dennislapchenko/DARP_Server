using System;
using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using RegionServer.Model;


namespace RegionServer.Handlers
{
	public class PlayerInGameHandler : PhotonServerHandler
	{
		public PlayerInGameHandler(PhotonApplication application) : base(application)
		{		
		}

		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int)MessageSubCode.PlayerInGame; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var instance = Util.GetCPlayerInstance(Server, message);
			instance.Spawn();
			instance.BroadcastUserInfo();

			Log.DebugFormat("Character {0} joined Region Server.", instance.Name);
			//Notify guild members that someone logged in

			//Notify friend list that someone logged in
			return true;
		}

	}
}

