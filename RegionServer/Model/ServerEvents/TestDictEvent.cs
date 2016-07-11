using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Stats;

namespace RegionServer.Model.ServerEvents
{
    public class TestDictEvent : ServerPacket
    {
        public TestDictEvent(CCharacter player) : base(ClientEventCode.ServerPacket, MessageSubCode.SendMove)
        {
            var testDictObj = new TestDictObj()
            {
                Equipment = player.Items.Equipment.ToDictionary(k => k.Key, v => (ItemData)v.Value),
                Inventory = player.Items.Inventory.ToDictionary(k => k.Key, v => (ItemData)v.Value),
                Stats = player.Stats.Stats.ToDictionary(k => k.Value.Name, v => player.Stats.GetStat(v.Value)),
                GenStats = player.GenStats
            };

            AddSerializedParameter(testDictObj, ClientParameterCode.Object);
        }
    }
}
