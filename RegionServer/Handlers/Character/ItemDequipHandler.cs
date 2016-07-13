using MMO.Photon.Server;
using MMO.Framework;
using MMO.Photon.Application;
using ComplexServerCommon;
using RegionServer.Operations;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Handlers.Character
{
	public class ItemDequipHandler : PhotonServerHandler
	{
		public ItemDequipHandler(PhotonApplication application) : base(application)
		{}

		public override MessageType Type { get { return MessageType.Request; } }

		public override byte Code
		{
			get
			{
				return (byte)ClientOperationCode.Region;
			}
		}

		public override int? SubCode
		{
			get
			{
				return (int)MessageSubCode.DequipItem;
			}
		}

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			Log.DebugFormat("receiving DequipItem request");

			var operation = new ItemDequipOperation(serverPeer.Protocol, message);
			if(!operation.IsValid)
			{
				Log.DebugFormat("Invalid operation for Dequip Item");
			}
			var instance = Util.GetCPlayerInstance(Server, message);
			var items = instance.Items;

			items.DequipItem((ItemSlot)operation.EquipmentSlot);
			instance.Stats.RefreshCurrentHealth();

			instance.BroadcastUserInfo();

			return true;
		}
	}
}
