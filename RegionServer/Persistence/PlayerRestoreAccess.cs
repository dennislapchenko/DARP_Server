using System.Linq;
using RegionServer.Model;
using RegionServer.Model.CharacterDatas;
using RegionServer.Model.Constants;
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
						//player.Stats.SetStat<Level>(character.Level); is set in Stats
                        
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

					    var GenStats = player.GetCharData<GeneralStats>();
                        if (!string.IsNullOrEmpty(character.GenStats))
						{
							GenStats.DeserializeStats(character.GenStats);
							((ItemHolder) player.Items).SetInventorySlots(GenStats.InventorySlots);
						}
						else
						{
						    GenStats.Experience = ExperienceConstants.LEVEL_0;
						    GenStats.NextLevelExperience = ExperienceConstants.LEVEL_1;
							GenStats.Battles = 0;
							GenStats.Win = 0;
							GenStats.Loss = 0;
							GenStats.Tie = 0;
							GenStats.Gold = 0;
							GenStats.Skulls = 0;
							GenStats.InventorySlots = 20;
							((ItemHolder)player.Items).SetInventorySlots(GenStats.InventorySlots);
						}

                        player.GetCharData<EloKeeper>().UpdateElo(character.Elo);

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
