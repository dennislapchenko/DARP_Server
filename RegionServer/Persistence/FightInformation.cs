using System;
using System.Diagnostics;
using System.Globalization;
using ComplexServerCommon;
using ComplexServerCommon.Enums;
using NHibernate.Exceptions;
using RegionServer.Model.Fighting;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;

namespace RegionServer.Model.DataKeepers
{
	public class FightInformation
	{
	    private const string CLASSNAME = "FightInformation";

		public DateTime QueueCreatedTime { get; set; }
		public DateTime FightStartedTime { get; set; }
		public DateTime FightEndedTime { get; set; }
        public string FightId { get; set; }
        public FightType FightType { get; set; }
        public int TeamSize { get; set; }
        public string Location { get; set; }
		public TimeSpan FightDuration { get; set; }
		public int MovesExchanged { get; set; }
		public string TeamRedNames { get; set; }
		public string TeamBlueNames { get; set; }
		public string HighestDamagePlayer { get; set; }
		public string LowestDamagePlayer { get; set; }
        public string Winner { get; set; }

		private Stopwatch fightTimer;
	    private readonly Fight _fight;

		public FightInformation(Fight fight)
		{
		    _fight = fight;
		    FightType = _fight.Type;
		    TeamSize = _fight.TeamSize;
		    Location = _fight.FightLocation.ToString();

		    FightId = _fight.FightId.ToString().Replace("-", "");

		    TeamRedNames = "";
		    TeamBlueNames = "";

		    QueueCreatedTime = DateTime.Now.UpToSeconds();
		}

		public void FightStarted()
		{
		    FightStartedTime = DateTime.Now.UpToSeconds();
			fightTimer = new Stopwatch();
			fightTimer.Start();

		    foreach (var player in _fight.TeamRed.Values)
		    {
		        TeamRedNames += player.Name + ", ";
		    }
            foreach (var player in _fight.TeamBlue.Values)
            {
                TeamBlueNames += player.Name + ", ";
            }

		    if (_fight.TeamSize == 1)
		    {
		        TeamRedNames = TeamRedNames.Replace(", ", "");
                TeamBlueNames = TeamBlueNames.Replace(", ", "");

            }
        }

        public void FightEnded()
        {
            FightEndedTime = DateTime.Now.UpToSeconds();
            FightDuration = TimeSpan.FromMilliseconds(fightTimer.ElapsedMilliseconds);
			fightTimer.Stop();

            setLowestDamage();
            setHighestDamage();
		    MovesExchanged = _fight.ExchangeCount;
		    Winner = _fight.Winner;

            UpdateElo();
        }

	    private void UpdateElo()
	    {
	        switch (_fight.Type)
	        {
                case (FightType.SINGLE):
	                break;
                case (FightType.GROUP):
	                break;
                default:
	                break;
	        }
	    }

	    private void setHighestDamage()
	    {
	        var damage = 0;
	        string name = "";
	        foreach (var entry in _fight.CharFightData)
	        {
	            if (entry.Value.TotalDamage > damage)
	            {
	                damage = entry.Value.TotalDamage;
	                name = entry.Key.Name;
	            }
	        }
	        HighestDamagePlayer = String.Format("{0}:{1}", name, damage);
	    }

        private void setLowestDamage()
        {
            var damage = 999999999;
            string name = "";
            foreach (var entry in _fight.CharFightData)
            {
                if (entry.Value.TotalDamage < damage)
                {
                    damage = entry.Value.TotalDamage;
                    name = entry.Key.Name;
                }
            }
            LowestDamagePlayer = String.Format("{0}:{1}", name, damage);
        }

	    public void store()
	    {
            FightEnded();
            storeInDB();
	    }

	    private void storeInDB()
	    {
	        const string METHODNAME = "storeInDB";
	        try
	        {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var fightEntry = session.QueryOver<FightHistory>().Where(x => x.FightId == FightId).SingleOrDefault();
                        if (fightEntry != null) throw new SqlParseException("fight id already exists");

                        FightHistory newFightEntry = new FightHistory()
                                                                {
                                                                    QueueCreatedTime = QueueCreatedTime.UpToSeconds(),
                                                                    FightStartedTime = FightStartedTime.UpToSeconds(),
                                                                    FightEndedTime = FightEndedTime.UpToSeconds(),
                                                                    FightId = FightId,
                                                                    FightType = FightType.ToString(),
                                                                    TeamSize = TeamSize,
                                                                    Location = Location,
                                                                    FightDuration = FightDuration.ToString("g"),
                                                                    MovesExchanged = MovesExchanged,
                                                                    TeamRedNames = TeamRedNames,
                                                                    TeamBlueNames = TeamBlueNames,
                                                                    LowestDamagePlayer = LowestDamagePlayer,
                                                                    HighestDamagePlayer = HighestDamagePlayer,
                                                                    Winner = Winner
                                                               };
   

                        session.Save(newFightEntry);
                        transaction.Commit();
                    }
                }
            }
	        catch (Exception e)
	        {
                DebugUtils.Logp(DebugUtils.Level.WARNING, CLASSNAME, METHODNAME, "saving fight to history failed with: " + e.Message);
	        }

	    }

    }
}
