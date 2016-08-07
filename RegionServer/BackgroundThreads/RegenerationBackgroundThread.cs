using System;
using MMO.Framework;
using RegionServer.Model;
using ExitGames.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using RegionServer.Model.ServerEvents;
using RegionServer.Model.Stats;

namespace RegionServer.BackgroundThreads
{
	public class RegenerationBackgroundThread : IBackgroundThread
	{
		private const int UPDATE_SPEED = 5000; //5sec

		public Region Region {get;set;}
		private bool isRunning {get;set;}
		protected static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public RegenerationBackgroundThread(Region region)
		{
			Region = region;
		}

		public void Setup()
		{
		}

		public void Run(object threadContext)
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();
			isRunning = true;

			while(isRunning)
			{
				try
				{	//UPDATE_SPEED is the time that has to pass in order to run the thread process
					if(timer.Elapsed < TimeSpan.FromMilliseconds((double)UPDATE_SPEED))
					{
						if(Region.NumPlayers <= 0)
						{
							Thread.Sleep(1000);
							timer.Restart();
						}
						if(UPDATE_SPEED - timer.Elapsed.Milliseconds > 0)
						{
							Thread.Sleep(UPDATE_SPEED - timer.Elapsed.Milliseconds);
						}
						continue;
					}
					var updateTime = timer.Elapsed;
					timer.Restart();
					Update(updateTime); //process
				}
				catch( Exception e)
				{
					Log.ErrorFormat(string.Format("Exception happened in Regen Background Thread - {0}", e.StackTrace));
				}
			}
		}

		void Update(TimeSpan elapsed)
		{
			Parallel.ForEach(Region.AllPlayers.Values.Where(p => ((CPlayerInstance)p).Stats.Dirty && p is CPlayerInstance).Cast<CPlayerInstance>(), SendUpdate);
		}

		public void SendUpdate(CPlayerInstance instance)
		{
			if(instance != null && instance.Stats.Dirty) //Stats.Dirty becomes true when current health is below maximum health
			{
				var newHealth = instance.Stats.RegenHealth(); //adds HP5Packet value to players current health
				instance.SendPacket(new HP5Packet(instance, newHealth)); //sends new health values to client
				//Log.DebugFormat("Sending HP5Packet update to {0} ({1}/{2})", instance.Name, instance.Stats.GetStat<CurrHealth>(), instance.Stats.GetStat<MaxHealth>());
			}
		}

		public void Stop()
		{
			isRunning = false;
		}
	}
}