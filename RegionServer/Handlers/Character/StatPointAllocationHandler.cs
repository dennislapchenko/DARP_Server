﻿using System.Collections.Generic;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using MMO.Framework;
using MMO.Photon.Application;
using MMO.Photon.Server;
using Photon.SocketServer;
using RegionServer.Model.CharacterDatas;
using RegionServer.Model.Interfaces;
using RegionServer.Model.Stats;
using RegionServer.Model.Stats.BaseStats;
using RegionServer.Model.Stats.PrimaryStats;

namespace RegionServer.Handlers.Character
{
    public class StatPointAllocationHandler : PhotonServerHandler
    {
        private const string CLASSNAME = "StatPointAllocationHandler";

        public StatPointAllocationHandler(PhotonApplication application) : base(application)
        {
        }

        public override MessageType Type { get { return MessageType.Request;} }
        public override byte Code { get { return (byte) ClientOperationCode.Region; } }
        public override int? SubCode { get { return (int)MessageSubCode.StatAllocation; } }

        protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
        {
            if (!message.Parameters.ContainsKey((byte) ClientParameterCode.Object))
            {
                DebugUtils.Logp(DebugUtils.Level.ERROR, CLASSNAME, "OnHandleMessage", "No StatAllocationData object passed. Exiting handler.");
                return true;
            }

            var para = new Dictionary<byte, object>
                        {
                            {(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
                            {(byte)ClientParameterCode.SubOperationCode, message.Parameters[(byte)ClientParameterCode.SubOperationCode]}
                        };

            var instance = Util.GetCPlayerInstance(Server, message);

            var statAllocData = SerializeUtil.Deserialize<StatAllocationData>(message.Parameters[(byte) ClientParameterCode.Object]);

            if (statAllocData.ResetPoints)
            {
                resetAllAllocatedStatPoints(instance);
            }
            else
            {
                foreach (var stat in statAllocData.Allocations)
                {
                    ((StatHolder)instance.Stats).SetStatByID(stat.Key, stat.Value);
                    instance.GetCharData<GeneralStats>().TotalAllocatedStats += stat.Value;
                    instance.Stats.AddToStat<StatPoints>(-stat.Value);
                }
            }

            var debugMessage = statAllocData.ResetPoints ? "Stats successfully reset!" : "Stats successfully added!";
            para.Add((byte)ClientParameterCode.Object, SerializeUtil.Serialize(instance.Stats.GetMainStatsForEnemy()));
            para.Add((byte)ClientParameterCode.StatsToAllocate, instance.Stats.GetStat<StatPoints>());

            serverPeer.SendOperationResponse(new OperationResponse(message.Code)
                                                                {
                                                                    ReturnCode = (int)ErrorCode.OK,
                                                                    DebugMessage = debugMessage,
                                                                    Parameters = para
                                                                }, new SendParameters());
            return true;
        }

        private void resetAllAllocatedStatPoints(ICharacter instance)
        {
            var stats = instance.Stats;

            stats.SetStat<Strength>(5);
            stats.SetStat<Dexterity>(5);
            stats.SetStat<Instinct>(5);
            stats.SetStat<Stamina>(5);

            stats.SetStat<StatPoints>(instance.GetCharData<GeneralStats>().TotalAllocatedStats);
            instance.GetCharData<GeneralStats>().TotalAllocatedStats = 0;
        }
    }
}
