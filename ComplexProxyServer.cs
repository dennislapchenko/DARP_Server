//
//  ComplexProxyServer.cs
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
using MMO.Photon.Application;
using ComplexServerCommon;
using Autofac;
using MMO.Photon.Server;
using ComplexServer.Handlers;
using MMO.Framework;
using MMO.Photon.Client;
using SubServerCommon.Data.ClientData;
using System.Net;

namespace ComplexServer
{
	public class ComplexProxyServer : PhotonApplication
	{
		#region implemented abstract members of PhotonApplication

		protected override void RegisterContainerObjects (ContainerBuilder builder)
		{
			builder.RegisterType<ComplexConnectionCollection>().As<PhotonConnectionCollection>().SingleInstance();
			builder.RegisterInstance(this).As<PhotonApplication>().SingleInstance();
			builder.RegisterType<EventForwardHandler>().As<DefaultEventHandler>().SingleInstance();
			builder.RegisterType<RequestForwardHandler>().As<DefaultRequestHandler>().SingleInstance();
			builder.RegisterType<ResponseForwardHandler>().As<DefaultResponseHandler>().SingleInstance();
			builder.RegisterType<HandleServerRegistration>().As<PhotonServerHandler>().SingleInstance();
			builder.RegisterType<CharacterData>().As<IClientData>();

			//Add Handlers
			builder.RegisterType<HandleClientLoginRequests>().As<PhotonClientHandler>().SingleInstance();
			builder.RegisterType<HandleClientChatRequests>().As<PhotonClientHandler>().SingleInstance();
			builder.RegisterType<HandleClientRegionRequests>().As<PhotonClientHandler>().SingleInstance();
			builder.RegisterType<LoginResponseHandler>().As<PhotonServerHandler>().SingleInstance();
			builder.RegisterType<SelectCharacterResponseHandler>().As<PhotonServerHandler>().SingleInstance();

		}

		protected override void ResolveParameters (IContainer container)
		{
		}

		public override void Register (PhotonServerPeer peer)
		{
		}

		public override byte SubCodeParameterCode {
			get {
				return (byte)ClientParameterCode.SubOperationCode;
			}
		}

		public override IPEndPoint MasterEndPoint {
			get {
				throw new NotImplementedException ();
			}
		}

		public override int? TcpPort {
			get {
				throw new NotImplementedException ();
			}
		}

		public override int? UdpPort {
			get {
				throw new NotImplementedException ();
			}
		}

		public override IPAddress PublicIpAddress {
			get {
				throw new NotImplementedException ();
			}
		}

		public override int ServerType {
			get {
				throw new NotImplementedException ();
			}
		}

		protected override int ConnectRetryIntervalSeconds {
			get {
				throw new NotImplementedException ();
			}
		}

		protected override bool ConnectsToMaster {
			get {
				return false;
			}
		}

		#endregion


	}
}

