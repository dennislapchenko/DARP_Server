using RegionServer.Model.Interfaces;
using System.Collections.Concurrent;
using System;
using ExitGames.Logging;
using ComplexServerCommon.Enums;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using RegionServer.Model.ServerEvents;

namespace RegionServer.Model.Fighting
{
	public class FightManager
	{
		private static readonly string CLASSNAME = "FightManager";

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
			var newFightId = Guid.NewGuid();
			var newFight = new Fight(newFightId, newFightInfo.Creator, newFightInfo.Type, newFightInfo.TeamSize, newFightInfo.Timeout, newFightInfo.Sanguinary);

			var success = Fights.TryAdd(newFight.FightId, newFight);
			if(success)
			{
//				Log.DebugFormat("new fight: creator {0}, type {1}, teamSize {2}, red: {3}, blue: {4}", newFight.Value.Creator, newFight.Value.Type.ToString(), newFight.Value.TeamSize, newFight.Value.
				return Fights[newFightId];
			}
			else
			{
				return null;
			}
		}

	    public Fight AddFight(int teamSize, FightType fightType = FightType.SINGLE)
	    {
            var newFightId = Guid.NewGuid();
            var newFight = new Fight(newFightId, "Mock", fightType, teamSize, 15, true);

            var success = Fights.TryAdd(newFight.FightId, newFight);
            if (success)
            {
                return Fights[newFightId];
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
				foreach(var player in fight.getPlayers.Values)
				{
					player.CurrentFight = null;
				}
				Fight removedFight;
				return Fights.TryRemove(fight.FightId, out removedFight);
			}
			return false;
		}

		public List<FightQueueListItem> GetAllQueues() //argument for fights in which state to Get
		{
			var result = new List<FightQueueListItem>();

			foreach(var fight in Fights.Where(f => f.Value.fightState == FightState.QUEUE))
			{
				result.Add(fight.Value.GetQueueInfo());
			}

			if(result != null)
			{
				return result;
			}

			return new List<FightQueueListItem>();
		}


		public bool LeaveQueue(CPlayerInstance instance)
		{
			var targetFight = instance.CurrentFight;
			if(targetFight != null)
			{
				switch(targetFight.fightState)
				{
				case(FightState.QUEUE):
						targetFight.RemovePlayer(instance);
						TrySetNewCreatorOrRemoveFight(targetFight);
						break;
				case(FightState.ENGAGED): //action on other players upon ones leave from engaged fight
						//will send 'load lobby scene event' for now
						foreach(var p in targetFight.getPlayers.Values)
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
			if(queue.getNumPlayers() > 0)
			{
				queue.Creator = queue.getPlayers.FirstOrDefault().Value.Name;
				return true;
			}
			else
			{
				RemoveFight(queue);
				return true;
			}
		}

		public List<Fight> getAllFights()
		{
			return Fights.Values.ToList();
		}

		public int getAllFightsCount()
		{
			return Fights.Count;
		}
	}
}

