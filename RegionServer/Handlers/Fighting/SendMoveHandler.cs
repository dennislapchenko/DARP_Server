using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using RegionServer.Model;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Fighting;

namespace RegionServer.Handlers.Fighting
{
	public class SendMoveHandler : PhotonServerHandler
	{

		private Fight _currentFight;
		private CPlayerInstance _instance;



		public SendMoveHandler(PhotonApplication application) : base (application)
		{
		}


		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int)MessageSubCode.SendMove; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			_instance = Util.GetCPlayerInstance(Server, message);
			_currentFight = _instance.CurrentFight;

			var newMove = SerializeUtil.Deserialize<FightMove>(message.Parameters[(byte)ClientParameterCode.Object]);
			newMove.PeerObjectId = _instance.ObjectId;
			newMove.TargetObjectId = _instance.Target.ObjectId;

			_currentFight.AddMoveSendPkg(_instance, newMove);

//			var para = new Dictionary<byte, object>()
//			{
//				{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
//				{(byte)ClientParameterCode.SubOperationCode, MessageSubCode.SwitchTarget},
//				{(byte)ClientParameterCode.ObjectId, _instance.TargetId}
//			};
//			serverPeer.SendOperationResponse(new OperationResponse(message.Code) {Parameters = para}, new SendParameters());
//			Server.Log.DebugFormat("Sending SwitchTarget response for {0} targetting ({1}){2}", _instance.Name, _instance.TargetId, _instance.Target.Name);
			return true;
		}



	}
}