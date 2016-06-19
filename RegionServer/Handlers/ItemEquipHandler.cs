using MMO.Photon.Server;
using MMO.Framework;
using MMO.Photon.Application;
using ComplexServerCommon;
using RegionServer.Model;
using RegionServer.Operations;

namespace RegionServer.Handlers
{
	public class ItemEquipHandler : PhotonServerHandler
	{
		public ItemEquipHandler(PhotonApplication application) : base(application) {}

		public override MessageType Type { get { return MessageType.Request; } 	}
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int)MessageSubCode.EquipItem; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			Log.DebugFormat("receiving equipItem request");

			var operation = new ItemEquipOperation(serverPeer.Protocol, message);
			if(!operation.IsValid)
			{
				Log.DebugFormat("Invalid operation for Equip Item");
			}

			var instance = Util.GetCPlayerInstance(Server, message);
			var items = instance.Items;

			items.EquipItem(operation.InventorySlot);
			instance.Stats.RefreshCurrentHealth();
			instance.BroadcastUserInfo();

			return true;
		}
	}
}

