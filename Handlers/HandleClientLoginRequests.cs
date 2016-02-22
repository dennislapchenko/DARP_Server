//
//  HandleClientLoginRequests.cs
//
//  Author:
//       Dennis Lapchenko <>
//
//  Copyright (c) 2015 Dennis Lapchenko
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using MMO.Photon.Client;
using MMO.Photon.Application;
using ComplexServerCommon;
using Photon.SocketServer;
using SubServerCommon;
using SubServerCommon.Data.ClientData;
using MMO.Framework;


namespace ComplexServer.Handlers
{
	public class HandleClientLoginRequests : PhotonClientHandler
	{
		public HandleClientLoginRequests (PhotonApplication application) : base(application)
		{
		}

		#region implemented abstract members of PhotonClientHandler

		public override MessageType Type {
			get {
				return MessageType.Async | MessageType.Request | MessageType.Response;
			}
		}
		public override byte Code {
			get {
				return (byte) ClientOperationCode.Login;
			}
		}
		public override int? SubCode {
			get {
				return null;
			}
		}
		protected override bool OnHandleMessage (IMessage message, PhotonClientPeer peer)
		{
			Log.DebugFormat("Handling Client Login Request");
			message.Parameters.Remove((byte)ClientParameterCode.PeerId);
			message.Parameters.Add((byte) ClientParameterCode.PeerId, peer.PeerId.ToByteArray());
			message.Parameters.Remove((byte) ClientParameterCode.UserId);
			if (peer.ClientData<CharacterData>().UserId.HasValue)
			{
				message.Parameters.Add( (byte)ClientParameterCode.UserId, peer.ClientData<CharacterData>().UserId);
			}
			if(peer.ClientData<CharacterData>().CharacterId.HasValue)
			{
				message.Parameters.Remove((byte)ClientParameterCode.CharacterId);
				message.Parameters.Add((byte)ClientParameterCode.CharacterId, peer.ClientData<CharacterData>().CharacterId);
			}
			var operationRequest = new OperationRequest(message.Code, message.Parameters);
			switch (message.Code)
			{
			case (byte)ClientOperationCode.Login:
				if (Server.ConnectionCollection<PhotonConnectionCollection>() != null)
				{
					Server.ConnectionCollection<PhotonConnectionCollection>().GetServerByType((int)ServerType.Login).SendOperationRequest(operationRequest, new SendParameters());

				}
				break;
			default:
				Log.DebugFormat("Invalid Operation Code - Expecting Login");
				break;
			}
			return true;
		}
		#endregion
	}
}
