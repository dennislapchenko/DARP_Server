using System.Linq;
using RegionServer.Model;
using RegionServer.Model.Items;
using RegionServer.Model.Stats;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;

namespace RegionServer.Persistence
{
    class PlayerRestoreAccess : IDatabaseAccess
    {
	    private readonly string CLASSNAME = "PlayerRestoreAccess";

        private CPlayerInstance player;

        public PlayerRestoreAccess(CPlayerInstance playerInstance)
        {
	        player = playerInstance;
        }

        public void execute()
        {
			using (var session = NHibernateHelper.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					var character = session.QueryOver<ComplexCharacter>().Where(cc => cc.Id == player.ObjectId).List().FirstOrDefault();
					if (character != null)
					{
						transaction.Commit(); //close connection
						player.Name = character.Name;

						//Appearance

						//Level
						player.Stats.SetStat<Level>(character.Level);

						//Position
						if (!string.IsNullOrEmpty(character.Position))
						{
							player.Position = Position.Deserialize(character.Position);
						}
						else
						{
							player.Position = new Position();
						}

						//Guild
						//Titles
						//Timers

						if (!string.IsNullOrEmpty(character.GenStats))
						{
							player.GenStats.DeserializeStats(character.GenStats);
							player.GenStats.Name = character.Name;
							((ItemHolder) player.Items).SetInventorySlots(player.GenStats.InventorySlots);
						}
						else
						{
							player.GenStats.Name = character.Name;
							player.GenStats.Experience = 0;
							player.GenStats.Battles = 0;
							player.GenStats.Win = 0;
							player.GenStats.Loss = 0;
							player.GenStats.Tie = 0;
							player.GenStats.Gold = 0;
							player.GenStats.Skulls = 0;
							player.GenStats.InventorySlots = 20;
							((ItemHolder)player.Items).SetInventorySlots(player.GenStats.InventorySlots);
						}

						if (!string.IsNullOrEmpty(character.Stats))
						{
							player.Stats.DeserializeStats(character.Stats);
						}


						//equipment
						if (!string.IsNullOrEmpty(character.Items))
						{
							player.Items.DeserializeItems(character.Items);
						}
						else
						{
							player.Client.Log.DebugFormat("{0}", player.Items.AddItem(1));
							player.Client.Log.DebugFormat("{0}", player.Items.AddItem(4));
							player.Client.Log.DebugFormat("{0}", player.Items.AddItem(5));
							player.Client.Log.DebugFormat("{0}", player.Items.AddItem(6));
							player.Items.EquipItem(1);
							player.Items.EquipItem(2);
							player.Items.EquipItem(2);
							player.Client.Log.DebugFormat("{0}", player.Items.AddItem(2));
							player.Client.Log.DebugFormat("{0}", player.Items.AddItem(3));
							player.Client.Log.DebugFormat("{0}", player.Items.AddItem(7));
							player.Client.Log.DebugFormat("{0}", player.Items.AddItem(2));
						}
						player.Stats.SetStat<CurrHealth>(player.Stats.GetStat<MaxHealth>());
					}
					else
					{
						transaction.Commit();
						player.Client.Log.FatalFormat("{0} - Should not reach - Character not found in database", CLASSNAME);
					}

				}
			}
		}
    }
}
