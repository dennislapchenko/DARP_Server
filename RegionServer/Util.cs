using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Items;
using System.Linq;
using RegionServer.Model;
using MMO.Framework;
using ComplexServerCommon;
using MMO.Photon.Application;
using MMO.Photon.Server;
using ComplexServerCommon.MessageObjects.Enums;
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

			float dx = obj1.Position.X - obj2.Position.X;
			float dy = obj1.Position.Y - obj2.Position.Y;
			float dz = obj1.Position.Z - obj2.Position.Z;

			return ((dx*dx) + (dy*dy) + (includeZAxis ? (dz*dz) : 0 )) <= distanceToForgetObject * distanceToForgetObject;
		}

		public static float DegToRad(float degrees)
		{
			return (float)Math.PI * (degrees/180f);
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

		public static List<KeyValuePairS<ItemSlot, ItemData>> ConvertEquipmentForXml(Dictionary<ItemSlot, Item> items)
		{
			var result = new List<KeyValuePairS<ItemSlot, ItemData>>();
			foreach(var item in items)
			{
				result.Add(new KeyValuePairS<ItemSlot, ItemData>(item.Key, (ItemData)item.Value));
			}
			return result;
		}

		public static MoveOutcome CheckAhitB(FightMove A, FightMove B)
		{
			if(A.AttackSpot != B.BlockSpots[0] && A.AttackSpot != B.BlockSpots[1])
			{ 
				return MoveOutcome.Hit; 
			} 
			else 
			{
				return MoveOutcome.Block; 
			}
		}

//		public static void NullDebug(object obj, string debugName)
//		{
//			if(obj == null)
//			{
//				Server.Log.DebugFormat("{0} == NULL", debugName);
//			}
//			else
//			{
//				Server.Log.DebugFormat("{0} != NULL", debugName);
//			}
//		}
	}
}

