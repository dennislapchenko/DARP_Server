using MMO.Photon.Server;
using MMO.Photon.Application;
using RegionServer.Model;
using MMO.Framework;
using ComplexServerCommon;

namespace RegionServer.Handlers.Fighting
{
	public class PlayerReadyQueueHandler : PhotonServerHandler
	{
		public PlayerReadyQueueHandler(PhotonApplication application) : base(application)
		{
		}
		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int) MessageSubCode.PlayerReadyQueue; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var instance = Util.GetCPlayerInstance(Server, message);
			instance.CurrentFight.SetPlayerReady(instance.ObjectId, (bool)message.Parameters[(byte)ClientParameterCode.Object]);

			//send this players "Ready" to other queue participants

			return true;
		}
	}
}

