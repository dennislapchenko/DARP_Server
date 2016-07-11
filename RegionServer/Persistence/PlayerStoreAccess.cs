using System;
using RegionServer.Model;
using RegionServer.Model.Stats;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;

namespace RegionServer.Persistence
{
    class PlayerStoreAccess : IDatabaseAccess
    {
	    private readonly string CLASSNAME = "PlayerStoreAccess";

        private CPlayerInstance player;

        public PlayerStoreAccess(CPlayerInstance playerInstance)
        {
	        player = playerInstance;
        }

        public void execute()
        {
            const string METHODNAME = "execute";
			try
			{
				using (var session = NHibernateHelper.OpenSession())
				{
					using (var transaction = session.BeginTransaction())
					{
						var user = session.QueryOver<User>().Where(u => u.Id == player.UserID).SingleOrDefault();
						var character = session.QueryOver<ComplexCharacter>().Where(cc => cc.UserId == user && cc.Name == player.Name).SingleOrDefault();
						//player.UserID = user.Id;
						//player.CharacterID = character.Id;

						character.Level = (int)player.Stats.GetStat<Level>();

						string position = player.Position.Serialize();
						character.Position = position;

						// Store stats
						character.GenStats = player.GenStats.SerializeStats();
						character.Stats = player.Stats.SerializeStats();

						//Store items
						character.Items = player.Items.SerializeItems();

						session.Save(character);
						transaction.Commit();
					}
				}
			}
			catch (Exception e)
			{
				DebugUtils.Logp(DebugUtils.Level.ERROR, CLASSNAME, METHODNAME, "Failed to Store player = " + e.Message);
			}
		}
    }
}
