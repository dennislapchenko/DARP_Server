using MMO.Photon.Application;
using ComplexServerCommon;
using RegionServer.Model;
using System.Collections.Generic;
using Photon.SocketServer;
using MMO.Photon.Server;
using MMO.Framework;
using System;
using RegionServer.Model.ServerEvents;
using ComplexServerCommon.Enums;

namespace RegionServer.Handlers
{
	public class JoinQueueHandler : PhotonServerHandler
	{
		private FightManager _fightManager;

		public JoinQueueHandler(PhotonApplication application, FightManager fightManager) : base(application)
		{
			_fightManager = fightManager;
		}

		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int) MessageSubCode.JoinQueue; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
//			var para = new Dictionary<byte, object>()
//			{
//				{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
//			};
//
			var instance = Util.GetCPlayerInstance(Server, message);
			Guid fightId = Guid.Parse((string)message.Parameters[(byte)ClientParameterCode.FightID]);

			Fight fight = _fightManager.GetFight(fightId) as Fight;
			bool success = false;

			if(fight.NumPlayers() < fight.TeamSize*2 && fight.State == FightState.QUEUE)
			{
				if(fight.TeamRed.Count < fight.TeamSize && fight.TeamBlue.Count < fight.TeamSize)
				{
					success = fight.AddPlayer(new Random().Next(1,3), instance);
				}
				else
				{
					if(fight.TeamRed.Count < fight.TeamSize)
					{
						success = fight.AddPlayer(1, instance); //else add to red
					}
					else
					{
						success = fight.AddPlayer(2, instance); //if red full - add to blue
					}
				}
			}
			if(success)
			{
				var fightParticipants = new FightQueueParticipants(fight);
				var pulledQueues = new PulledQueues(_fightManager);
				foreach(var character in fight.Players.Values)
				{
					character.SendPacket(pulledQueues);
					character.SendPacket(fightParticipants);
				}
			}
			return true;
		}
	}
}