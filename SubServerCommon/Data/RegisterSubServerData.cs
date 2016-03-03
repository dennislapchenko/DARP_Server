using System;
namespace SubServerCommon.Data
{
	public class RegisterSubServerData
	{
		public string GameServerAddress {get; set;}
		public Guid? ServerId {get; set;}
		public int? TcpPort {get; set;}
		public int? UdpPort {get; set;}
		public int ServerType  {get; set;}
		public string ApplicationName  {get; set;}

	}
}

