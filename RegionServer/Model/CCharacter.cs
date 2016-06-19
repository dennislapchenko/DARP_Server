
using System;
using System.Linq;
using System.Collections;
using RegionServer.Model.KnownList;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Model.ServerEvents;
using ComplexServerCommon.MessageObjects;


namespace RegionServer.Model
{
	public class CCharacter : CObject, ICharacter
	{
		public CCharacter(Region region, CharacterKnownList objectKnownList, IStatHolder stats, IItemHolder items) : base(region, objectKnownList)
		{
			Stats = stats;
			Stats.Character = this;
			Items = items;
			Items.Character = this;
			StatusListeners = new List<ICharacter>();
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
			var hostileTeam = CurrentFight.CharFightData[this as CPlayerInstance].Team == FightTeam.Red ? CurrentFight.TeamBlue : CurrentFight.TeamRed;
			var aliveTargets = hostileTeam.Values.Where(p => !p.IsDead).ToList();
			var cplayer = this as CPlayerInstance;
			if(cplayer != null)
			{
				cplayer.Client.Log.DebugFormat("found hostile team: {0}. {1}/{2} players alive", hostileTeam, aliveTargets.Count, hostileTeam.Values.Count);
			}

			if(!CurrentFight.Moves.Any())
			{
				Target = aliveTargets.FirstOrDefault();
				if(cplayer != null)
				{
					cplayer.Client.Log.DebugFormat("({0}){1} has targeted ({2}){3} - (Moves were empty)", cplayer.ObjectId, cplayer.Name, cplayer.TargetId, cplayer.Target.Name);
				}
				return;
			}

			foreach(var enemy in aliveTargets)
			{
				var matchingMove = CurrentFight.Moves.FirstOrDefault(n => n.PeerObjectId == ObjectId && n.TargetObjectId == enemy.ObjectId);

				if(matchingMove == null)
				{
					Target = enemy;
					if(cplayer != null)
					{
						cplayer.Client.Log.DebugFormat("{0} has targeted ({1}){2}", cplayer.Name, cplayer.TargetId, cplayer.Target.Name);
					}
					return;
				}
				else
				{
					if(cplayer != null)
					{
						cplayer.Client.Log.DebugFormat("found existing move in a foreach enemy list\n" + matchingMove.ToString());
					}
				}
			}
		}
//			}
//			else
//			{
//				//switch NPC's target
//			}

		public bool IsTeleporting {get; private set;}
		public bool IsDead {get; set;}
		public Position Destination {get; set;}
		public virtual MoveDirection Direction {get; set;}
		public virtual bool Moving {get; set;}
		public int Facing {get; set;}
		public IList<ICharacter> StatusListeners {get; private set;}
		public delegate void DeathListener(ICharacter killer);

		public IStatHolder Stats {get; set;}
		public IItemHolder Items {get; protected set;}
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
			Teleport(pos.X, pos.Y, pos.Z, pos.Heading);
		}

		public void Teleport(float x, float y, float z, short heading)
		{
			StopMove(null);

			IsTeleporting = true;
			Target = null;

			BroadcastMessage(new TeleportToLocation(this, x, y, z, heading));

			Decay();
			Position.XYZ(x, y, z);
			Position.Heading = heading;
		}

		public void Teleport(float x, float y, float z)
		{
			Teleport(x, y, z, Position.Heading);
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
			StopMove(null);
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
			StopMove(null);
			//Effects.StopAllEffectsThroughDeath();
			//CalculateRewards(killer);
			BroadcastStatusUpdate();
			//Region.OnDeath(this);

			return true;
		}

		public void StopMove(Position pos)
		{
			if(pos != null)
			{
				Destination = pos;
				Position = pos;
			}
			else
			{
				Destination = Position;
			}
			BroadcastMessage(new StopMove(this));
		}

		public virtual void CalculateRewards(ICharacter killer)
		{
		}

		public void BroadcastStatusUpdate()
		{
			//foreach (var statusListener in StatusListeners)
			foreach(var player in CurrentFight.Players)
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

	}
}