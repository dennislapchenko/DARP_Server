using System;
using System.Collections.Concurrent;
using MMO.Framework;
using RegionServer.Model;
using ExitGames.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ComplexServerCommon.Enums;
using FluentNHibernate.Utils;
using RegionServer.Model.Interfaces;
using RegionServer.Model.Items;
using RegionServer.Model.KnownList;
using RegionServer.Model.Stats;

namespace RegionServer.BackgroundThreads
{
	public class BotQueenBackgroundThread : IBackgroundThread
	{
		public static readonly string CLASSNAME = "BotQueenBackgroundThread";

		private const int UPDATE_SPEED = 5000; //15000sec

		public FightManager FightManager { get; set; }
		private bool isRunning { get; set; }
		public CBotInstance ProtoBot { get; set; }
		private ConcurrentDictionary<int, CCharacter> Bots { get; set; }
		protected static readonly ILogger Log = LogManager.GetCurrentClassLogger();

		public BotQueenBackgroundThread(FightManager fightManager, Region region, IStatHolder freshStats)
		{
			FightManager = fightManager;
			ProtoBot = new CBotInstance(region, new CharacterKnownList(), freshStats, new ItemHolder(), new GeneralStats());
		}

		public void Setup()
		{
			Bots = new ConcurrentDictionary<int, CCharacter>();
		}

		public void Run(object threadContext)
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();
			isRunning = true;

			while (isRunning)
			{
				try
				{   //UPDATE_SPEED is the time that has to pass in order to run the thread process
					if (timer.Elapsed < TimeSpan.FromMilliseconds((double)UPDATE_SPEED))
					{
						if (FightManager.getAllFightsCount() <= 0)
						{
							Thread.Sleep(1000);
							timer.Restart();
						}
						if (UPDATE_SPEED - timer.Elapsed.Milliseconds > 0)
						{
							Thread.Sleep(UPDATE_SPEED - timer.Elapsed.Milliseconds);
						}
						continue;
					}
					var updateTime = timer.Elapsed;
					timer.Restart();
					Update(updateTime); //process
				}
				catch (Exception e)
				{
					Log.ErrorFormat(string.Format("Exception happened in {0} - {1}", CLASSNAME, e.StackTrace));
				}
			}
		}

		void Update(TimeSpan elapsed)
		{
			Parallel.ForEach(FightManager.getAllFights().Where(f => f.State == FightState.QUEUE && f.BotsWelcome).Cast<Fight>(), DeployBots);
		}

		public void DeployBots(Fight fight)
		{
			string METHODNAME = "DeployBots";
			Log.DebugFormat(String.Format("{0} - {1} : to {2}", CLASSNAME, METHODNAME, fight));
			//var charToEmulate = fight.getAllParticipants().FirstOrDefault();
			if (fight.getNumPlayers() < fight.TeamSize*2)
			{
				var bot = ProtoBot.copy();
				bot.configureBot((byte)new Random().Next(fight.getLowestLevel(), fight.getHighestLevel()+1));
				bot.joinQueue(fight);

				Bots.TryAdd(bot.ObjectId, bot);
				Log.DebugFormat("{0} - {1} shoving {2} into a queue {3}", CLASSNAME, METHODNAME, bot, fight);
			}
		}

		public void Stop()
		{
			isRunning = false;
		}
	}
}