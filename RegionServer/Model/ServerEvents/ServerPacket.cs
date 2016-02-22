using System;
using MMO.Photon.Application;
using System.Collections.Generic;
using ComplexServerCommon;


namespace RegionServer.Model.ServerEvents
{
	public class ServerPacket : PhotonEvent
	{
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
				Parameters[(byte)code] = Xml.Serialize(obj);
			}
			else
			{
				Parameters.Add((byte)code, Xml.Serialize(obj));
			}
		}
	}
}

