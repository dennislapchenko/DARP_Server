using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;

namespace SubServerCommon.Handlers
{
	public class ErrorRequestForwardHandler : DefaultRequestHandler
	{
		public ErrorRequestForwardHandler(PhotonApplication application) : base(application)
		{
		}
		
		public override MessageType Type {
			get {
				return MessageType.Request;
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
			Log.ErrorFormat("No existing Request Handler {0} - {1}", message.Code, message.SubCode);
			return true;
		}

	}
}

