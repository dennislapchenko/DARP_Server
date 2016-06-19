using RegionServer.Model.Interfaces;
using System.Collections.Concurrent;
using System;
using ExitGames.Logging;
using ComplexServerCommon.Enums;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using System.Linq;
using RegionServer.Model.ServerEvents;
namespace RegionServer.Model
{
	public class FightManager
	{
		private ConcurrentDictionary<Guid, Fight> Fights;
		private readonly Fight.Factory _fightFactory;

		protected ILogger Log = LogManager.GetCurrentClassLogger();

		public FightManager()
		{
			Fights = new ConcurrentDictionary<Guid, Fight>();
		}

		public FightManager(Fight.Factory fightFactory)
		{
			_fightFactory = fightFactory;
			Fights = new ConcurrentDictionary<Guid, Fight>();
		}


		public IFight GetFight(Guid fightId)
		{
			if(Fights.ContainsKey(fightId))
			{
				return Fights[fightId];
			}
			return null;
		}

		public Fight AddFight(FightQueueListItem newFightInfo)
		{
//			foreach(var fight in Fights)
//			{
//				Log.DebugFormat("[{0}] by {1} in {2}", fight.Key, fight.Value.Creator, fight.Value.Type);
//			}
			var fightId = Guid.NewGuid();
			var newFight = new System.Collections.Generic.KeyValuePair<Guid, Fight>(fightId, new Fight(fightId, newFightInfo.Creator, newFightInfo.Type, newFightInfo.TeamSize, newFightInfo.Timeout, newFightInfo.Sanguinary));

			var success = Fights.TryAdd(newFight.Key, newFight.Value);
			if(success)
			{
//				Log.DebugFormat("new fight: creator {0}, type {1}, teamSize {2}, red: {3}, blue: {4}", newFight.Value.Creator, newFight.Value.Type.ToString(), newFight.Value.TeamSize, newFight.Value.
				return Fights[newFight.Key];
			}
			else
			{
				return null;
			}
		}

		public bool RemoveFight(Fight fight)
		{
			if(Fights.ContainsKey(fight.FightId))
			{
				foreach(var player in fight.Players.Values)
				{
					player.CurrentFight = null;
				}

				return Fights.TryRemove(fight.FightId, out fight);
			}
			return false;
		}




		public List<FightQueueListItem> GetAllQueues() //argument for fights in which state to Get
		{
			var result = new List<FightQueueListItem>();

			foreach(var fight in Fights.Where(f => f.Value.State == FightState.QUEUE))
			{
				result.Add(fight.Value.GetQueueInfo());
			}
			if(result != null)
			{
				return result;
			}
			return null;
		}


		public bool LeaveQueue(CPlayerInstance instance)
		{
			var targetFight = instance.CurrentFight;
			if(targetFight != null)
			{
				switch(targetFight.State)
				{
				case(FightState.QUEUE):
						targetFight.RemovePlayer(instance);
						TrySetNewCreatorOrRemoveFight(targetFight);
						break;
				case(FightState.ENGAGED): //action on other players upon ones leave from engaged fight
						//will send 'load lobby scene event' for now
						foreach(var p in targetFight.Players.Values)
						{
							p.CurrentFight = null;
							p.SendPacket(new LoadIngameScene());
						}
						targetFight.RemovePlayer(instance);
						break;
					default:
						break;
				}
			}
			return true;
		}

		private bool TrySetNewCreatorOrRemoveFight(Fight queue)
		{
			if(queue.NumPlayers() > 0)
			{
				queue.Creator = queue.Players.FirstOrDefault().Value.Name;
				return true;
			}
			else
			{
				RemoveFight(queue);
				return true;
			}
		}
	}
}

