using System;
using RegionServer.Model.Interfaces;
using RegionServer.Model;
using MMO.Framework;
using ComplexServerCommon;
using MMO.Photon.Application;
using SubServerCommon.Data.ClientData;


namespace RegionServer
{
	public class Util
	{
		private static PhotonApplication Server {get;set;}

		public Util(PhotonApplication application)
		{
			Server = application;
		}

        public static double MapToRange(float input, float input_start, float input_end, float output_start, float output_end)
        {
            var slope = (output_end - output_start) / (input_end - input_start);
            return output_start + slope * (input - input_start);
        }

        public static bool IsInShortRange(int distanceToForgetObject, IObject obj1, IObject obj2, bool includeZAxis)
		{
			if(obj1 == null || obj2 == null)
			{
				return false;
			}

			if(distanceToForgetObject == -1)
			{
				return false;
			}

			var obj1Loc = obj1.Position.getPosition();
			var obj2Loc = obj2.Position.getPosition();

			if (obj1Loc[0] == obj2Loc[0] && obj1Loc[1] == obj2Loc[1])
				return true;
			else
				return false;
		}

		public static CPlayerInstance GetCPlayerInstance(PhotonApplication server, IMessage message)
		{
			if(message.Parameters.ContainsKey((byte)ClientParameterCode.PeerId))
			{
				var peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);
				var clients = server.ConnectionCollection<SubServerConnectionCollection>().Clients;
				return clients[peerId].ClientData<CPlayerInstance>();
			}
			return null;
		}

		public static CharacterData GetCharacterData(PhotonApplication server, IMessage message)
		{
			if(message.Parameters.ContainsKey((byte)ClientParameterCode.PeerId))
			{
				var peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);
				var clients = server.ConnectionCollection<SubServerConnectionCollection>().Clients;
				return clients[peerId].ClientData<CharacterData>();
			}
			return null;
		}
	}
}

