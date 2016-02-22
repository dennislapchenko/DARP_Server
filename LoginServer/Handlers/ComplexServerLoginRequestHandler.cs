using MMO.Photon.Server;
using MMO.Photon.Client;
using MMO.Photon.Application;
using LoginServer.Operations;
using Photon.SocketServer;
using System.Collections.Generic;
using ComplexServerCommon;
using SubServerCommon.Data.NHibernate;
using SubServerCommon;
using System;
using System.Security.Cryptography;
using System.Text;
using SubServerCommon.Data.ClientData;

namespace LoginServer.Handlers
{
	public class ComplexServerLoginRequestHandler : PhotonServerHandler
	{
		private readonly SubServerClientPeer.Factory _clientFactory;
	
		public ComplexServerLoginRequestHandler(PhotonApplication application, SubServerClientPeer.Factory clientFactory) : base(application)
		{
			_clientFactory = clientFactory;
		}
		
		#region implemented abstract members of PhotonServerHandler

		public override MMO.Framework.MessageType Type {
			get {
				return MMO.Framework.MessageType.Request;
			}
		}
		
		public override byte Code {
			get {
				return (byte) ClientOperationCode.Login;
			}
		}
		
		public override int? SubCode {
			get {
				return (int) MessageSubCode.Login;
			}
		}

		protected override bool OnHandleMessage (MMO.Framework.IMessage message, PhotonServerPeer serverPeer)
		{
			var operation = new LoginSecurely(serverPeer.Protocol, message);
			if (!operation.IsValid)
			{
				serverPeer.SendOperationResponse(new OperationResponse(message.Code, new Dictionary<byte, object> {{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]}})
				           	{	
								ReturnCode = (int)ErrorCode.OperationInvalid,
								DebugMessage = operation.GetErrorMessage() 
							},	new SendParameters());
				
				return true;
			}
			
			if (operation.UserName == "" ||	 operation.Password == "")
			{
				serverPeer.SendOperationResponse(new OperationResponse(message.Code, new Dictionary<byte, object> {{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]}} )
                       	  	{		
								ReturnCode = (int)ErrorCode.UserNamePasswordInvalid,
								DebugMessage = "Username or password is incorrect" 
							},	new SendParameters());
				return true;
			}
			
			try
			{
				using(var session = NHibernateHelper.OpenSession())
				{
					using (var transaction = session.BeginTransaction())
					{
						Log.Debug("About to look for user account " + operation.UserName);
						var userList = session.QueryOver<User>().Where(u => u.UserName == operation.UserName).List();
						if (userList.Count > 0)
						{
							User user = userList[0];
							var hash = BitConverter.ToString(SHA1CryptoServiceProvider.Create()
							                      .ComputeHash(Encoding.UTF8.GetBytes(user.Salt + operation.Password)))
								.Replace("-", "");
							if (String.Equals(hash.Trim(), user.Password.Trim(), StringComparison.OrdinalIgnoreCase))
							{
								LoginServer server = Server as LoginServer;
								if(server != null)
								{
									bool founduser = false;
									foreach (var subServerClientPeer in server.ConnectionCollection<SubServerConnectionCollection>().Clients)
									{
										if(subServerClientPeer.Value.ClientData<CharacterData>().UserId == user.Id)
										{
											founduser = true;
										}
									}

									if(founduser)
									{
										var para = new Dictionary<byte, object> 
										{ 
											{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
											{(byte)ClientParameterCode.SubOperationCode, message.Parameters[(byte)ClientParameterCode.SubOperationCode]},
										};
										serverPeer.SendOperationResponse(new OperationResponse((byte)ClientOperationCode.Login) {Parameters = para, ReturnCode = (short)ErrorCode.UserCurrentlyLoggedIn, DebugMessage = "User is currently logged in"}, new SendParameters());
									}
									else 
									{
										server.ConnectionCollection<SubServerConnectionCollection>().Clients.Add(new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]), _clientFactory(new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId])));
										server.ConnectionCollection<SubServerConnectionCollection>().Clients[new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId])].ClientData<CharacterData>().UserId = user.Id;
										Log.Debug("Login Handler sucessfully found character to log in.");
										var para = new Dictionary<byte, object> 
										{
											{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
											{(byte)ClientParameterCode.SubOperationCode, message.Parameters[(byte)ClientParameterCode.SubOperationCode]},
											{(byte)ClientParameterCode.UserId, user.Id}
										};
										serverPeer.SendOperationResponse(new OperationResponse((byte)ClientOperationCode.Login) {Parameters = para}, new SendParameters());
									}
								}
								return true;
							}
							else
							{
								serverPeer.SendOperationResponse(new OperationResponse(message.Code, new Dictionary<byte, object> {{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]}} )
								                                 {		
																	ReturnCode = (int)ErrorCode.UserNamePasswordInvalid,
																	DebugMessage = "Username or password is incorrect" 
																},	new SendParameters());
								return true;

							}
						}
						else
						{
							Log.DebugFormat("Account name does not exist {0}", operation.UserName);
							transaction.Commit(); //closing transaction
							serverPeer.SendOperationResponse(new OperationResponse(message.Code) 
							    {
									ReturnCode = (int)ErrorCode.UserNamePasswordInvalid,
									DebugMessage = "Username or password is incorrect" 
								}, new SendParameters());
							return true;
						}
					}
				}
			}
			catch (Exception e)
			{
				Log.Error("Error Occured", e);
				serverPeer.SendOperationResponse(new OperationResponse(message.Code, 
				    new Dictionary<byte, object> {{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]}})
                     	{	
							ReturnCode = (int)ErrorCode.UserNameInUse,
							DebugMessage = e.ToString() 	
						}, new SendParameters());	
			}
			return true;
		}

		#endregion
	}
}

