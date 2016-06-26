using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RegionServer.Model.DataKeepers
{
	public class FightInformation
	{
		public DateTime QueueCreatedTime { get; set; }
		public DateTime FightStartedTime { get; set; }
		public DateTime FightEndedTime { get; set; }
		public float FightDuration { get; set; }
		public int MovesExchanged { get; set; }
		public string TeamSizes { get; set; }
		public string TeamRedNames { get; set; }
		public string TeamBlueNames { get; set; }
		public int HighestDamagePlayer { get; set; }
		public int LowestDamagePlayer { get; set; }

		private Stopwatch fightTimer;

		public FightInformation()
		{
			QueueCreatedTime = DateTime.Now;
		}

		public void FightStarted()
		{
			FightStartedTime = DateTime.Now;
			fightTimer = new Stopwatch();
			fightTimer.Start();
		}

		public void FightEnded()
		{
			FightEndedTime = DateTime.Now;
			FightDuration = fightTimer.ElapsedMilliseconds;
			fightTimer.Stop();
		}

	}
}
