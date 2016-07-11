using MMO.Photon.Application;
using ComplexServerCommon;
using MMO.Photon.Server;
using MMO.Framework;
using System;
using RegionServer.Model.Fighting;

namespace RegionServer.Handlers
{
	public class JoinQueueHandler : PhotonServerHandler
	{
		private FightManager _fightManager;

		public JoinQueueHandler(PhotonApplication application, FightManager fightManager) : base(application)
		{
			_fightManager = fightManager;
		}

		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int) MessageSubCode.JoinQueue; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
//			var para = new Dictionary<byte, object>()
//			{
//				{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
//			};
//
			var instance = Util.GetCPlayerInstance(Server, message);
			Guid fightId = Guid.Parse((string)message.Parameters[(byte)ClientParameterCode.FightID]);

			Fight fight = _fightManager.GetFight(fightId) as Fight;

			fight.addPlayer(instance);
			return true;
		}
	}
}