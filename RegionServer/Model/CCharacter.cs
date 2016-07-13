
using System;
using System.Linq;
using RegionServer.Model.KnownList;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using ComplexServerCommon.Enums;
using RegionServer.Model.ServerEvents;
using ComplexServerCommon.MessageObjects;
using ExitGames.Logging;
using FluentNHibernate.Conventions;
using RegionServer.Model.Fighting;
using RegionServer.Model.Stats;


namespace RegionServer.Model
{
	public abstract class CCharacter : CObject, ICharacter
	{
		protected readonly ILogger Log = LogManager.GetCurrentClassLogger();

		protected CCharacter(Region region, CharacterKnownList objectKnownList, IStatHolder stats, IItemHolder items, GeneralStats genStats) : base(region, objectKnownList)
		{
			Stats = stats;
			Stats.Character = this;
			Items = items;
			Items.Character = this;
			GenStats = genStats;
			StatusListeners = new List<ICharacter>();
		}

		protected CCharacter()
		{
			
		}

		public new CharacterKnownList KnownList //new means the type can be changed
		{
			get { return ObjectKnownList as CharacterKnownList; }
		}

		private IObject _target;
		public IObject Target 
		{
			get { return _target; } 
			set 
			{
				if (value != null && !value.IsVisible)
				{
					//value = null;
				}
				
				if (value != null && value != _target)
				{
					KnownList.AddKnownObject(value);
					value.KnownList.AddKnownObject(value);
					_target = value;
				}
			}
		}
		public int TargetId 
		{
			get 
			{ 
				if (Target != null)
				{
					return Target.ObjectId;
				}
				return -1;
			}
		}

		public void SwitchCurrentFightTarget()
		{
			var hostileTeam = CurrentFight.getPlayerTeam(this) == FightTeam.Red ? CurrentFight.TeamBlue : CurrentFight.TeamRed;
			var aliveTargets = hostileTeam.Values.Where(p => !p.IsDead).ToList();

			if(!CurrentFight.Moves.Any())
			{
			    Target = aliveTargets[RngUtil.intRange(0, aliveTargets.Count - 1)];
				return;
			}

		    var enemiesWithoutAttacks = (from enemy 
                                         in aliveTargets
                                         let matchingMove = CurrentFight.Moves.Values.FirstOrDefault(n => n.PeerObjectId == ObjectId && n.TargetObjectId == enemy.ObjectId)
                                         where matchingMove == null select enemy).ToList();
		    if (enemiesWithoutAttacks.IsEmpty()) return;
		    Target = enemiesWithoutAttacks[RngUtil.intRange(0, enemiesWithoutAttacks.Count - 1)];
		    Log.DebugFormat("{0} has targeted {1}", ToString(), Target.ToString());
        }

        public bool isNPC { get; }
		public bool IsDead {get; set;}
		public Position Destination {get; set;}

		public virtual bool Moving {get; set;}

		public IList<ICharacter> StatusListeners {get; private set;}
		public delegate void DeathListener(ICharacter killer);

		public IStatHolder Stats {get; set;}
		public IItemHolder Items {get; protected set;}
		public GeneralStats GenStats { get; set; }
		public Fight CurrentFight {get; set;}

		

		public DeathListener DeathListeners; //so spawner can remove it from its list and respawn the NPC after timer
		
		public virtual void BroadcastMessage(ServerPacket packet)
		{
			foreach(CPlayerInstance player in KnownList.KnownPlayers.Values)
			{
				player.SendPacket(packet);
			}
		}

		public virtual void SendMessage(string text)
		{
		}

		public void Teleport(Position pos)
		{
			var locations = pos.getPosition();
			Teleport(locations[0], locations[1]);
		}

		public void Teleport(LocationType high, LocationType low)
		{
			Target = null;

			BroadcastMessage(new TeleportToLocation(this, high, low));

			Decay();
			Position.setPosition(high, low);
		}

		public void Teleport(ITeleportType teleportType)
		{
			Teleport(teleportType.GetNearestTeleportLocation(this));
		}

		public virtual bool Die()
		{
			if(IsDead)
			{
				return false;
			}

			IsDead = true;
			Target = null;
			//BroadcastStatusUpdate();
			return true;
		}

		public virtual bool Resurrect()
		{
			if(!IsDead)
			{
				return false;
			}
			IsDead = false;
			return true;
		}


		public virtual bool Die(ICharacter killer)
		{
			if(IsDead)
			{
				return false;
			}
			//Stats.OnDeath();
			IsDead = true;

			if(DeathListeners != null)
			{
				DeathListeners(killer);
			}

			Target = null;
			//Effects.StopAllEffectsThroughDeath();
			//CalculateRewards(killer);
			BroadcastStatusUpdate();
			//Region.OnDeath(this);

			return true;
		}

		public virtual void CalculateRewards(ICharacter killer)
		{
		}

		public void BroadcastStatusUpdate()
		{
			//foreach (var statusListener in StatusListeners)
			foreach(var player in CurrentFight.getPlayers)
			{
				player.Value.BroadcastMessage(new StatusUpdate(this));
			}
		}
		public virtual void UpdateAndBroadcastStatus(int broadcastType)
		{
		}

		public virtual void SendStateToPlayer(IObject owner)
		{
			owner.SendPacket(new MoveToLocation(this));
		}

		public virtual void DeleteMe() //deregister
		{

		}

		public override string ToString()
		{
			return String.Format("[{0}]{1}", ObjectId, Name);
		}
	}
}