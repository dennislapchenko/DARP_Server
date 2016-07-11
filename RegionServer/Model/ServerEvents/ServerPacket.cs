using System;
using MMO.Photon.Application;
using System.Collections.Generic;
using ComplexServerCommon;
using ExitGames.Logging;


namespace RegionServer.Model.ServerEvents
{
	public class ServerPacket : PhotonEvent
	{
		protected static ILogger Log = LogManager.GetCurrentClassLogger();

		public bool SendToSelf {get; set;}

		public ServerPacket (ClientEventCode code, MessageSubCode subCode, bool sendToSelf = true) : base ((byte)code, (int?)subCode, new Dictionary<byte, object>())
		{
			SendToSelf = sendToSelf;
			AddParameter(subCode, ClientParameterCode.SubOperationCode);

		}


		public void AddParameter<T>(T obj, ClientParameterCode code)
		{
			if(Parameters.ContainsKey((byte)code))
			{
				Parameters[(byte)code] = obj;
			}
			else
			{
				Parameters.Add((byte)code, obj);
			}
		}

		public void AddSerializedParameter<T>(T obj, ClientParameterCode code, bool binary = true)
		{
			if(Parameters.ContainsKey((byte)code))
			{
				Parameters[(byte)code] = SerializeUtil.Serialize(obj);
			}
			else
			{
				Parameters.Add((byte)code, SerializeUtil.Serialize(obj));
			}
		}
	}
}

