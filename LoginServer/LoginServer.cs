using System;
using MMO.Photon.Application;
using System.Net;
using Autofac;
using ComplexServerCommon;
using SubServerCommon;
using MMO.Photon.Server;
using Photon.SocketServer;
using SubServerCommon.Operations;
using SubServerCommon.Data;
using System.Xml.Serialization;
using System.IO;
using ComplexServer;
using LoginServer.Handlers;
using System.Reflection;
using System.Collections.Generic;
using MMO.Photon.Client;
using SubServerCommon.Data.ClientData;
using MMO.Framework;
using SubServerCommon.Handlers; 


namespace LoginServer
{
	public class LoginServer : PhotonApplication
	{
		private readonly IPAddress _publicIPAddress = IPAddress.Parse("127.0.0.1");
		private readonly IPEndPoint _masterEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4520);

		protected override void RegisterContainerObjects (ContainerBuilder builder)
		{
			builder.RegisterType<ErrorEventForwardHandler>().As<DefaultEventHandler>().SingleInstance();
			builder.RegisterType<ErrorRequestForwardHandler>().As<DefaultRequestHandler>().SingleInstance();
			builder.RegisterType<ErrorResponseForwardHandler>().As<DefaultResponseHandler>().SingleInstance();
			builder.RegisterType<SubServerConnectionCollection >().As<PhotonConnectionCollection>().SingleInstance();
			builder.RegisterInstance(this).As<PhotonApplication>().SingleInstance();
			builder.RegisterType<SubServerClientPeer>();
			builder.RegisterType<CharacterData>().As<IClientData>();

			//Add Handlers
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(GetType())).Where(t => t.Name.EndsWith("Handler")).As<PhotonServerHandler>().SingleInstance();
		}

		protected override void ResolveParameters (IContainer container)
		{
		}

		public override void Register (PhotonServerPeer peer)
		{
			var registerSubServerOperation = 
				new RegisterSubServerData()
											{
												GameServerAddress = PublicIpAddress.ToString(),
												TcpPort = TcpPort,
												UdpPort = UdpPort,
												ServerId = ServerId,
												ServerType = ServerType,
												ApplicationName = ApplicationName
											};
			peer.SendOperationRequest(new OperationRequest((byte)ServerOperationCode.RegisterSubServer,
			                  new RegisterSubServer() {RegisterSubServerOperation = SerializeUtil.Serialize(registerSubServerOperation)}), new SendParameters());
		}

		public override byte SubCodeParameterCode {
			get {
				return (byte) ClientParameterCode.SubOperationCode;
			}
		}

		public override IPEndPoint MasterEndPoint {
			get {
				return _masterEndPoint;
			}
		}

		public override int? TcpPort {
			get {
				return 4531;
			}
		}

		public override int? UdpPort {
			get {
				return 5056;
			}
		}

		public override IPAddress PublicIpAddress {
			get {
				return _publicIPAddress;
			}
		}

		public override int ServerType {
			get {
				return (int)SubServerCommon.ServerType.Login;
			}
		}

		protected override int ConnectRetryIntervalSeconds {
			get {
				return 14;
			}
		}

		protected override bool ConnectsToMaster {
			get {
				return true;
			}
		}

	}
}

