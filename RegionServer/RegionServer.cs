using MMO.Photon.Application;
using System.Net;
using Autofac;
using ComplexServerCommon;
using SubServerCommon;
using MMO.Photon.Server;
using Photon.SocketServer;
using SubServerCommon.Operations;
using SubServerCommon.Data;
using System.Reflection;
using MMO.Photon.Client;
using SubServerCommon.Handlers;
using RegionServer.Model;
using RegionServer.Model.Effects;
using RegionServer.Model.KnownList;
using RegionServer.Model.Fighting;
using RegionServer.Model.Items;
using RegionServer.Model.NPC;
using RegionServer.Model.Stats;


namespace RegionServer
{
	public class RegionServer : PhotonApplication
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
			builder.RegisterType<CPlayerInstance>();
		    builder.RegisterType<CBotInstance>();
		    builder.RegisterType<Item>();
			builder.RegisterType<Region>().SingleInstance();
			builder.RegisterType<PlayerKnownList>();
		    builder.RegisterType<CharacterKnownList>();
		    builder.RegisterType<StatHolder>().AsImplementedInterfaces().AsSelf();
		    builder.RegisterType<EffectHolder>().AsSelf();
		    builder.RegisterType<NPCFactory>();
			builder.RegisterType<ItemDBCache>().SingleInstance();
		    builder.RegisterType<EffectCache>().SingleInstance();
			builder.RegisterType<FightManager>().SingleInstance();
			
			//Registering Assemblies
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(GetType())).Where(t => t.Name.EndsWith("Handler")).As<PhotonServerHandler>().SingleInstance();
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(GetType())).AsImplementedInterfaces(); //BGThreads, CPlayerInstance + others
			
		}
		
		protected override void ResolveParameters (IContainer container)
		{
		}
		
		public override void Register (PhotonServerPeer peer)
		{
			var registerSubServerOperation = new RegisterSubServerData()
			                                        {
				                                        GameServerAddress = PublicIpAddress.ToString(),
				                                        TcpPort = TcpPort,
				                                        UdpPort = UdpPort,
				                                        ServerId = ServerId,
				                                        ServerType = ServerType,
				                                        ApplicationName = ApplicationName
			                                        };

			Log.DebugFormat("[RegionServer] Server Register Request Sent");
			peer.SendOperationRequest(new OperationRequest((byte)ServerOperationCode.RegisterSubServer,
			                                               new RegisterSubServer() {RegisterSubServerOperation = ComplexServerCommon.SerializeUtil.Serialize(registerSubServerOperation)}), new SendParameters());
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
				return 4533;
			}
		}
		
		public override int? UdpPort {
			get {
				return 5058;
			}
		}
		
		public override IPAddress PublicIpAddress {
			get {
				return _publicIPAddress;
			}
		}
		
		public override int ServerType {
			get {
				return (int)SubServerCommon.ServerType.Region;
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
