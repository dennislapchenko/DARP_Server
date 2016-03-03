using System;
using System.Linq;
using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using System.Collections.Generic;
using Photon.SocketServer;
using LoginServer.Operations;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;
using SubServerCommon.Data.ClientData;


namespace LoginServer.Handlers
{
	public class LoginServerSelectCharacterHandler : PhotonServerHandler
	{
		public LoginServerSelectCharacterHandler(PhotonApplication application) : base (application)
		{
		}

		public override MessageType Type
		{
			get { return MessageType.Request; }
		}
		public override byte Code
		{
			get { return (byte)ClientOperationCode.Login; }
		}
		public override int? SubCode
		{
			get { return (int) MessageSubCode.SelectCharacter; }
		}

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var para = new Dictionary<byte, object> 
						{
							{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
							{(byte)ClientParameterCode.SubOperationCode, message.Parameters[(byte)ClientParameterCode.SubOperationCode]}
						};
			var operation = new SelectCharacter(serverPeer.Protocol, message);
			if(!operation.IsValid)
			{
				Log.Error(operation.GetErrorMessage());
				serverPeer.SendOperationResponse(new OperationResponse(message.Code) 
			                             	    { 	ReturnCode = (int)ErrorCode.OperationInvalid,
													DebugMessage = operation.GetErrorMessage(),
													Parameters = para
												}, new SendParameters() );
				return true;
			}

			try 
			{
				using (var session = NHibernateHelper.OpenSession())
				{
					using (var transaction = session.BeginTransaction())
					{
						var user = session.QueryOver<User>().Where(u => u.Id == operation.UserId).List().FirstOrDefault();
						if(user != null)
						{
							Log.DebugFormat("Found user {0}", user.UserName);
						}
						var character = session.QueryOver<ComplexCharacter>().Where(cc => cc.UserId == user).And(cc => cc.Id == operation.CharacterId).List().FirstOrDefault();
						transaction.Commit();

						if(character == null)
						{
							serverPeer.SendOperationResponse(new OperationResponse(message.Code) { ReturnCode = (int)ErrorCode.InvalidCharacter, DebugMessage = "Invalid character.", Parameters = para}, new SendParameters());
							return true;
						}
						else
						{
							Log.DebugFormat("Found character {0}", character.Name);
							para.Add((byte)ClientParameterCode.CharacterId, character.Id);
							serverPeer.SendOperationResponse(new OperationResponse(message.Code) 
							                                { 
																ReturnCode = (byte)ErrorCode.OK, 
																Parameters = para
															}, new SendParameters()	);
						}
					}
				}
				return true;
			}
			catch(Exception e)
			{
				Log.Error(e);
				serverPeer.SendOperationResponse(new OperationResponse(message.Code) { ReturnCode = (int)ErrorCode.InvalidCharacter, DebugMessage = e.ToString(), Parameters = para}, new SendParameters());
			}
			return true;
		}
	}
}

