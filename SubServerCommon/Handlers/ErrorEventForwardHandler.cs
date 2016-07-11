using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;


namespace SubServerCommon.Handlers
{
	public class ErrorEventForwardHandler : DefaultEventHandler
	{
		public ErrorEventForwardHandler(PhotonApplication application) : base(application)
		{
		}

		public override MessageType Type {
			get {
				return MessageType.Async;
			}
		}
		
		public override byte Code {
			get {
				return (byte)(ClientOperationCode.Chat | ClientOperationCode.Region | ClientOperationCode.Login);
			}
		}

		public override int? SubCode {
			get {
				return null;
			}
		}

		protected override bool OnHandleMessage (IMessage message, PhotonServerPeer serverPeer)
		{
			Log.ErrorFormat("No existing Event Handler. Code {0}. SubCode {1}", ((ServerEventCode)message.Code).ToString(), ((MessageSubCode)message.SubCode).ToString());
			return true;
		}
	}
}

