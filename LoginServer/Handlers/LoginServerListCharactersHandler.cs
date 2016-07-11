using System;
using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using LoginServer.Operations;
using Photon.SocketServer;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


namespace LoginServer.Handlers
{
	public class LoginServerListCharactersHandler : PhotonServerHandler
	{
		public LoginServerListCharactersHandler (PhotonApplication application) : base(application)
		{
		}
	
		public override MessageType Type {
			get {
				return MessageType.Request;
			}
		}
		public override byte Code {
			get {
				return (byte)ClientOperationCode.Login;
			}
		}
		public override int? SubCode {
			get {
				return (byte) MessageSubCode.ListCharacters;
			}
		}
		protected override bool OnHandleMessage (IMessage message, PhotonServerPeer serverPeer)
		{
			var operation = new ListCharacters(serverPeer.Protocol, message);
			if (!operation.IsValid)
			{
				Log.DebugFormat("Invalid Operation - {0}", operation.GetErrorMessage());
				serverPeer.SendOperationResponse(
					new OperationResponse(message.Code) 
                    	{
							ReturnCode = (int)ErrorCode.OperationInvalid, 
							DebugMessage = operation.GetErrorMessage()
						}, 
					new SendParameters());
				return true;
			}

			try
			{
				using(var session = NHibernateHelper.OpenSession())
				{
					using (var transaction = session.BeginTransaction())
					{
						var user = session.QueryOver<User>().Where(u => u.Id == operation.UserId).List().FirstOrDefault();
						if (user != null)
						{
							var profile = session.QueryOver<UserProfile>().Where (up => up.UserId == user).List().FirstOrDefault();
							if (profile != null)
							{
								var para = new Dictionary<byte, object>
													{
														{(byte)ClientParameterCode.CharacterSlots, profile.CharacterSlots},
														{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
														{(byte)ClientParameterCode.SubOperationCode, message.Parameters[(byte)ClientParameterCode.SubOperationCode]}
													};
								var characters = session.QueryOver<ComplexCharacter>().Where(cc => cc.UserId == user).List();

								Hashtable characterList = new Hashtable();
								foreach (var complexCharacter in characters)
								{
									characterList.Add(complexCharacter.Id, SerializeUtil.Serialize(complexCharacter.BuilderCharacterListItem()));
								}

								para.Add((byte)ClientParameterCode.CharacterList, characterList);
								
								transaction.Commit();
								serverPeer.SendOperationResponse(new OperationResponse((byte)ClientOperationCode.Login) { Parameters = para }, new SendParameters());
							}
							else
							{
								serverPeer.SendOperationResponse(
									new OperationResponse(message.Code) 
									{ 
									    ReturnCode = (int)ErrorCode.OperationInvalid, 
									    DebugMessage = "Profile not found"
									}, 
									new SendParameters());
							}
						}
						else
						{
							serverPeer.SendOperationResponse(
								new OperationResponse(message.Code) 
								{ 
								    ReturnCode = (int)ErrorCode.OperationInvalid, 
								    DebugMessage = "User not found"
								}, 
								new SendParameters());
						}
					}
				}

			}
			catch (Exception e)
			{
				Log.Error(e.Message);
				serverPeer.SendOperationResponse(
					new OperationResponse(message.Code) 
						{ 
							ReturnCode = (int)ErrorCode.OperationInvalid, 
							DebugMessage = e.ToString()
						}, 
					new SendParameters());
			}
			return true;
		}
	}
}


