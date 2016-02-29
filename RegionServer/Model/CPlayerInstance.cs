using System;
using RegionServer.Model.Interfaces;
using MMO.Photon.Client;
using MMO.Photon.Server;
using ExitGames.Logging;
using RegionServer.Model.ServerEvents;
using ComplexServerCommon;
using Photon.SocketServer;
using RegionServer.Model.KnownList;
using MMO.Framework;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;
using RegionServer.Model.Stats;
using System.Linq;
using ComplexServerCommon.MessageObjects;


namespace RegionServer.Model
{
	public class CPlayerInstance : CPlayable, IPlayer, IClientData
	{

		public CPlayerInstance(Region region, PlayerKnownList objectKnownList, IStatHolder stats, IPhysics physics) : base (region, objectKnownList, stats)
		{
			Physics = physics;
			Physics.MoveSpeed = Stats.GetStat<MoveSpeed>();
			Destination = new Position();
		}


		public SubServerClientPeer Client {get; set;}
		public PhotonServerPeer ServerPeer {get; set;}
		public int? UserID {get; set;}
		public int? CharacterID {get; set;}
		public IPhysics Physics {get; set;}
		
		public override Position Position
		{
			get
			{
				base.Position = Physics.Position;
				return base.Position;
			}
			set
			{
				base.Position = value;
				if(Physics != null)
				{
					Physics.Position = value;
				}
			}
		}

		public override MoveDirection Direction
		{
			get
			{
				base.Direction = Physics.Direction;
				return base.Direction;
			}
			set
			{
				base.Direction = value;
			}
		}
		public override bool Moving
		{
			get
			{
				base.Moving = Physics.Moving;
				return base.Moving;
			}
			set
			{
				base.Moving = value;
			}
		}

		public new PlayerKnownList KnownList
		{
			get	{ return ObjectKnownList as PlayerKnownList; }
			set	{ ObjectKnownList = value; }
		}

		public override void BroadcastMessage(ServerPacket packet)
		{
			if (packet.SendToSelf)
			{
				SendPacket(packet);
			}

			foreach (CPlayerInstance player in KnownList.KnownPlayers.Values)
			{
				player.SendPacket(packet);
			}
		}

		public override void SendPacket(ServerPacket packet)
		{
			if (Client != null)
			{
				packet.AddParameter(Client.PeerId.ToByteArray(), ClientParameterCode.PeerId);
				ServerPeer.SendEvent(new EventData(packet.Code, packet.Parameters), new SendParameters());
			}
		}

		public override void SendInfo(IObject obj)
		{
			obj.SendPacket(new CharInfoUpdate(this));
		}

		public void BroadcastUserInfo()
		{
			SendPacket(new UserInfoUpdate(this));
			BroadcastMessage(new CharInfoUpdate(this));
		}

		public override void DeleteMe()
		{
			CleanUp();
			Store();
			base.DeleteMe();
		}

		public void CleanUp()
		{
			Client.Log.DebugFormat("Logging off");
			//abort attacks
			StopMove(null);
			//remove temp items
			//remove LFG
			//Stop some timers
			//stop crafting

			Target = null;

			//stop temp buffs
			//decay from server
			Decay();
			//unsummon pets
			//notify guild/friends of logoff
			//cancel trading
		}

		public void Store()
		{
			//save character to db

			try
			{
				using(var session = NHibernateHelper.OpenSession())
				{
					using(var transaction = session.BeginTransaction())
					{
						var user = session.QueryOver<User>().Where(u => u.Id == UserID).SingleOrDefault();
						var character = session.QueryOver<ComplexCharacter>().Where(cc => cc.UserId == user && cc.Name == Name).SingleOrDefault();
						character.Level = (int)Stats.GetStat<Level>();
						string position = Position.Serialize();
						character.Position = position;

						// Store stats
						character.Stats = Stats.SerializeStats();
						session.Save(character);

						transaction.Commit();
					}
				}
			}
			catch(Exception e)
			{
				Client.Log.Error(e);
			}
		}

		public void Restore(int objectId)
		{
			using (var session = NHibernateHelper.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					var character = session.QueryOver<ComplexCharacter>().Where(cc => cc.Id == objectId).List().FirstOrDefault();
					if (character != null)
					{
						transaction.Commit();
						ObjectId = objectId;
						Name = character.Name;

						//Appearance

						//Level
						Stats.SetStat<Level>(character.Level);

						//Experience
						//Position
						if(!string.IsNullOrEmpty(character.Position))
						{
							Position = Position.Deserialize(character.Position);
						}
						else
						{
							Position = new Position(0,0,0);
						}

						//Guild
						//Titles
						//Timers

						if(!string.IsNullOrEmpty(character.Stats))
						{
							Stats.DeserializeStats(character.Stats);
						}

						//equipment
						//inventory
						//effects

						//social - guild/friend notify
						Client.Log.DebugFormat("Max HP: {0}", Stats.GetStat<Level5HP>());
					}
					else
					{
						transaction.Commit();
						Client.Log.FatalFormat("[CPlayerInstance] - Should not reach - Character not found in database");
					}
					
				}
			}
		}
	}
}

