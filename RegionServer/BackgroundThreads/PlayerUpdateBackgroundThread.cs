using MMO.Framework;
using System.Diagnostics;
using RegionServer.Model;
using ExitGames.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using RegionServer.Model.ServerEvents;


namespace RegionServer.BackgroundThreads
{
	public class PlayerUpdateBackgroundThread : IBackgroundThread
	{
		private const double UPDATE_SPEED = 100;

		public Region Region {get; set;}
		private bool isRunning = false;
		protected static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public PlayerUpdateBackgroundThread(Region region)
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
					if (timer.Elapsed < TimeSpan.FromMilliseconds(UPDATE_SPEED))
					{
						if (Region.NumPlayers <= 0)
						{
							Thread.Sleep(1000);
							timer.Restart();
						}
						if((int)UPDATE_SPEED - timer.Elapsed.Milliseconds > 0)
						{
							Thread.Sleep((int)UPDATE_SPEED - timer.Elapsed.Milliseconds);
						}
						continue;
					}
					var updateTime = timer.Elapsed;
					timer.Restart();
					Update(updateTime);

				}
				catch (Exception e)
				{
					Log.ErrorFormat(string.Format("Exception happened in PlayerUpdateBackgroundThread.Run - {0}", e.StackTrace));
				}
			}
		}

		void Update(TimeSpan elapsed)
		{
			//Parallel.ForEach(Region.AllPlayers.Values.Where(p => p.Physics.Dirty && p is CPlayerInstance).Cast<CPlayerInstance>(), SendUpdate);
		}

		public void SendUpdate(CPlayerInstance instance)
		{
//			if( instance != null && instance.Physics.Dirty)
//			{
//				instance.BroadcastMessage(new MoveToLocation(instance));
//				instance.Physics.Dirty = false;
//				//Log.DebugFormat("Sending MoveToLocation to {0}", instance.Name);
//			}
		}

		public void Stop()
		{
			isRunning = false;
		}
	}
}