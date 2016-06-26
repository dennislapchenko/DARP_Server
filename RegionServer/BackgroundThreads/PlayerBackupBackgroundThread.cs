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
	public class PlayerBackupBackgroundThread : IBackgroundThread
	{
		private const int UPDATE_SPEED = 30000; //every 30 sec

		public Region Region {get;set;}
		private bool isRunning {get;set;}
		protected static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public PlayerBackupBackgroundThread(Region region)
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
				{
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
					Update(updateTime);
				}
				catch( Exception e)
				{
					Log.ErrorFormat(string.Format("Exception happened in Regen Background Thread - {0}", e.StackTrace));
				}
			}
		}

		void Update(TimeSpan elapsed)
		{
			Parallel.ForEach(Region.AllPlayers.Values.Where(/*p => p.Stats.AnyNewValues? */ p => p is CPlayerInstance).Cast<CPlayerInstance>(), SendUpdate);
		}

		public void SendUpdate(CPlayerInstance instance)
		{
			if(instance != null /* && instance.Stats.AnyNewValues? */)
			{
				instance.Store();
				//instance.stored = true
				//Log.DebugFormat("Backing info for {0} to the Database", instance.Name);
			}
		}

		public void Stop()
		{
			isRunning = false;
		}
	}
}