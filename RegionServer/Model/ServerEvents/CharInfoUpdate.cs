using System;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using System.Linq;


namespace RegionServer.Model.ServerEvents
{
	public class CharInfoUpdate : ServerPacket
	{
		public CharInfoUpdate(CPlayerInstance player) : base(ClientEventCode.ServerPacket, MessageSubCode.CharInfo, false)
		{
			AddParameter(player.ObjectId, ClientParameterCode.ObjectId);
			AddCharInfo(player);
		}

		public void AddCharInfo(CPlayerInstance player)
		{
			var equipment = player.Items.Equipment.ToDictionary(item => (int)item.Key, item => (ItemData)item.Value);
			CharInfo info = new CharInfo() 
			{
				Position = player.Position,
				Name = player.Name,

				//stats
				GenStats = player.GenStats,
				Stats = player.Stats.GetMainStatsForEnemy(),
				EquipmentKeys = equipment.Keys.ToArray(),
				EquipmentValues = equipment.Values.ToArray(),

				//race, sex, class, title, guild
				//effects - pvp flag, debuffs, buffs
				//movement speed for smoothing/calculation
				//action/emote walk, run, sit
			};
			info.GenStats.Gold = -1;
			info.GenStats.Skulls = -1;
			info.GenStats.InventorySlots = -1;
			AddSerializedParameter(info, ClientParameterCode.Object, false);
		}
	}
}

