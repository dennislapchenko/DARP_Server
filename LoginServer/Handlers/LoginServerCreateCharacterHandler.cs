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
using ComplexServerCommon.MessageObjects;


namespace LoginServer.Handlers
{
	public class LoginServerCreateCharacterHandler : PhotonServerHandler
	{

        public LoginServerCreateCharacterHandler(PhotonApplication application) : base(application)
		{
		}

		public override MessageType Type
		{
			get	{ return MessageType.Request; }
		}

		public override byte Code
		{
			get { return (byte)ClientOperationCode.Login;}
			
		}

		public override int? SubCode
		{
			get { return (int)MessageSubCode.CreateCharacter;}
		}

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var para = new Dictionary<byte, object>
								{
									{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
									{(byte)ClientParameterCode.SubOperationCode, message.Parameters[(byte)ClientParameterCode.SubOperationCode]}
								};

			var operation = new CreateCharacter(serverPeer.Protocol, message);
			if (!operation.IsValid)
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
						var profile = session.QueryOver<UserProfile>().Where(up => up.UserId == user).List().FirstOrDefault();
						var characters = session.QueryOver<ComplexCharacter>().Where(cc => cc.UserId == user).List();

						if (profile != null && profile.CharacterSlots <= characters.Count)
						{
							serverPeer.SendOperationResponse(new OperationResponse(message.Code) { ReturnCode = (int)ErrorCode.InvalidCharacter, DebugMessage = "No free character slots", Parameters = para}, new SendParameters());
						}
						else
						{
							var createCharacter = SerializeUtil.Deserialize<CharacterCreateDetails>(operation.CharacterCreateDetails);
                            Server.Log.DebugFormat("NEW CHAR: NAME: {0} / SEX: {1} / CLASS: {2}", createCharacter.CharacterName, createCharacter.Sex, createCharacter.CharacterClass);
							var character = session.QueryOver<ComplexCharacter>().Where(cc => cc.Name == createCharacter.CharacterName).List().FirstOrDefault();
							if (character != null)
							{
								transaction.Commit();
								serverPeer.SendOperationResponse(new OperationResponse(message.Code) { ReturnCode = (int)ErrorCode.InvalidCharacter, DebugMessage = "Character name taken", Parameters = para}, new SendParameters());
							}
							else
							{
								var newChar = new ComplexCharacter() 
									{ 	
										UserId = user,
										Name = createCharacter.CharacterName,
										Class = createCharacter.CharacterClass,
										Sex = createCharacter.Sex,
										Level = 0,
									};
								session.Save(newChar);
								transaction.Commit();
								serverPeer.SendOperationResponse(new OperationResponse(message.Code) { ReturnCode = (int)ErrorCode.OK, DebugMessage = "Character successfuly created", Parameters = para}, new SendParameters());
							}
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

