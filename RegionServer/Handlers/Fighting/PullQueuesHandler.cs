using MMO.Photon.Server;
using MMO.Framework;
using MMO.Photon.Application;
using ComplexServerCommon;
using System.Collections.Generic;
using Photon.SocketServer;
using RegionServer.Model.Fighting;

namespace RegionServer.Handlers
{
	public class PullQueuesHandler : PhotonServerHandler
	{
		private FightManager _fightManager;
		public PullQueuesHandler(PhotonApplication application, FightManager fightManager) : base(application)
		{
			_fightManager = fightManager;
		}

		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int) MessageSubCode.PullQueue; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var para = new Dictionary<byte, object>()
			{
				{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
				{(byte)ClientParameterCode.SubOperationCode, message.Parameters[(byte)ClientParameterCode.SubOperationCode]},
				{(byte)ClientParameterCode.Object, SerializeUtil.Serialize(_fightManager.GetAllQueues())},
			};

			serverPeer.SendEvent(new EventData(message.Code) {Parameters = para}, new SendParameters());

			return true;
		}
	}
}

