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
using SubServerCommon;

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

