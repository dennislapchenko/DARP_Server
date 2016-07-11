
using MMO.Photon.Server;
using MMO.Photon.Application;
using ComplexServerCommon;
using Photon.SocketServer;
using System.Collections.Generic;
using LoginServer.Operations;
using System;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;
using System.Security.Cryptography;
using System.Text;
using MMO.Framework;

namespace LoginServer.Handlers
{
	public class ComplexServerRegisterRequestHandler : PhotonServerHandler //PhotonClientHandler if SIMPLE SERVER
	{
		public ComplexServerRegisterRequestHandler(PhotonApplication application) : base(application)
		{
		}

		#region implemented abstract members of PhotonServerHandler

		public override MessageType Type {
			get {
				return MessageType.Request;
			}
		}
		
		public override byte Code {
			get {
				return (byte) ClientOperationCode.Login;
			}
		}
		
		public override int? SubCode {
			get {
				return (int) MessageSubCode.Register;
			}
		}

		protected override bool OnHandleMessage (IMessage message, PhotonServerPeer serverPeer)
		{
			var para = new Dictionary<byte, object>()
			{
				{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
				{(byte)ClientParameterCode.SubOperationCode, MessageSubCode.Register},
			};

			var operation = new RegisterSecurely(serverPeer.Protocol, message);
			if (!operation.IsValid)
			{
				serverPeer.SendOperationResponse(new OperationResponse(message.Code)
				    {ReturnCode = (int)ErrorCode.OperationInvalid, DebugMessage = operation.GetErrorMessage()}	,new SendParameters());
			return true;
			}

		
			if (operation.UserName == "" || operation.Email == "" || operation.Password == "")
			{
				serverPeer.SendOperationResponse(new OperationResponse(message.Code, para)
				                                 {	ReturnCode = (int)ErrorCode.OperationInvalid,
													DebugMessage = "All fields are required!" }, new SendParameters());
				return true;
			}

		try
		{
			using(var session = NHibernateHelper.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					Log.Debug("About too look for user account");
					var userList = session.QueryOver<User>().Where(u => u.UserName == operation.UserName).List();
					if (userList.Count > 0)
					{
						Log.DebugFormat("Found account name already in use");
						transaction.Commit();
						serverPeer.SendOperationResponse(new OperationResponse(message.Code, para) {
							ReturnCode = (int)ErrorCode.UserNameInUse,
							DebugMessage = "Account name already in use, please use another"
						}, new SendParameters());
						return true;
					}

					string salt = Guid.NewGuid().ToString().Replace("-", "");
					//Log.DebugFormat("Created salt {0}", salt);
					User newUser = new User() 
					            {
						            Email = operation.Email,
						            UserName = operation.UserName,
						            Password = 
						            BitConverter.ToString(SHA1CryptoServiceProvider.Create().ComputeHash(Encoding.UTF8.GetBytes(salt + operation.Password))).Replace("-", ""),
						            Salt = salt,
						            Algorithm = "sha1",
						            Created = DateTime.Now,
						            Updated = DateTime.Now
					            };
					session.Save(newUser);
					Log.Debug("Saved new user " + operation.UserName);
					transaction.Commit();
				}
				using (var transaction = session.BeginTransaction())
				{
					Log.DebugFormat("Looking up newly created user");
					var userList = session.QueryOver<User>().Where(u => u.UserName == operation.UserName).List();
					if (userList.Count > 0)
					{
						Log.DebugFormat("Creating Profile");
						UserProfile profile = new UserProfile() { CharacterSlots = 3, UserId = userList[0]};
						session.Save(profile);
						Log.DebugFormat("Saved profile");
						transaction.Commit();
						serverPeer.SendOperationResponse(new OperationResponse(message.Code, para)
                        { ReturnCode = (byte) ClientReturnCode.UserCreated}, new SendParameters());
					}
				}
			}
		}
		catch (Exception e)
		{
			Log.Error("Error Occured", e);
			serverPeer.SendOperationResponse(new OperationResponse(message.Code, para)
			                                 {	ReturnCode = (int)ErrorCode.UserNameInUse,
												DebugMessage = e.ToString() },
												new SendParameters());	
			}
			return true;
		}

		#endregion
	}
}

