using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using RegionServer.Operations;
using RegionServer.Model;
using Photon.SocketServer;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using System;
using RegionServer.Model.ServerEvents;

namespace RegionServer.Handlers
{
	public class CreateQueueHandler : PhotonServerHandler
	{
		private static readonly string CLASSNAME = "CreateQueueHandler";

		private FightManager _fightManager;
		public CreateQueueHandler(PhotonApplication application, FightManager fightManager)
			: base (application)
		{
			_fightManager = fightManager;
		}

		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int) MessageSubCode.CreateQueue; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var para = new Dictionary<byte, object>()
						{
							{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
							{(byte)ClientParameterCode.SubOperationCode, MessageSubCode.PullQueue},
						};
				
			var operation = new CreateQueueOperation(serverPeer.Protocol, message);
			if(!operation.IsValid)
			{
				serverPeer.SendOperationResponse(new OperationResponse
					(message.Code, para) {ReturnCode = (int)ErrorCode.OperationInvalid, DebugMessage = "Create queue operation invalid"}, new SendParameters());
			}

			var instance = Util.GetCPlayerInstance(Server, message);

			var queueItemRequest = Xml.Deserialize<FightQueueListItem>(operation.fightInit);
			queueItemRequest.Creator = instance.Name;

			Fight newFight = instance.CurrentFight;
			bool wasAllowedNewFight = false;
			if(instance.CurrentFight == null)
			{
				newFight = _fightManager.AddFight(queueItemRequest);
				wasAllowedNewFight = newFight.addPlayer(1, instance); //1 - red, 2 - blue teams
			}

			//send response with refreshed queue list

			if(wasAllowedNewFight)
			{
				instance.SendPacket(new PulledQueues(_fightManager));
				foreach(var player in newFight.Players.Values)
				{
					player.SendPacket(new FightQueueParticipants(newFight));
					Log.Debug(CLASSNAME + " - OnHandleMessage:: sending queue info back to client");
				}
			}
			else
			{
				serverPeer.SendOperationResponse(new OperationResponse(message.Code)
				{
					ReturnCode = (int)ErrorCode.AlreadyInFight,
					DebugMessage = "Already queued/engaged in a fight",
					Parameters = para,
				}, new SendParameters());
				Log.Debug(CLASSNAME + " - OnHandleMessage:: was not allowed a new fight");

			}
			return true;
		}

	}
}