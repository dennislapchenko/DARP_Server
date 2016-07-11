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
using RegionServer.Model.Fighting;
using RegionServer.Model.NPC;

namespace RegionServer.BackgroundThreads
{
	public class BotQueenBackgroundThread : IBackgroundThread
	{
		public static readonly string CLASSNAME = "BotQueenBackgroundThread";

		private static readonly int UPDATE_SPEED = 5000; //15000sec

		public FightManager FightManager { get; set; }
		private bool isRunning { get; set; }
        protected static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private NPCFactory _NPCFactory { get; }

		private ConcurrentDictionary<int, CCharacter> Bots { get; set; }

		public BotQueenBackgroundThread(FightManager fightManager, NPCFactory npcFactory)
		{
			FightManager = fightManager;
		    _NPCFactory = npcFactory;
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
			Parallel.ForEach(FightManager.getAllFights().Where(f => f.fightState == FightState.QUEUE && f.BotsWelcome && !f.isFull), DeployBots);
            Parallel.ForEach(FightManager.getAllFights().Where(f => f.fightState == FightState.ENGAGED && f.isBotsBothSides()), MoveBots);
		}

		private void DeployBots(Fight fight)
		{
		    const string METHODNAME = "DeployBots";

		    try
		    {
		        byte level = (byte) RngUtil.intRange(fight.getLowestLevel(), fight.getHighestLevel());
		        var bot = _NPCFactory.createFightBot(level);

                bot.joinQueue(fight);

		        Bots.TryAdd(bot.ObjectId, bot);
		        Log.DebugFormat("{0} - {1} shoving {2} into a queue {3}", CLASSNAME, METHODNAME, bot, fight);
		    }
		    catch (Exception e)
		    {
		        DebugUtils.Logp(DebugUtils.Level.ERROR, CLASSNAME, METHODNAME, String.Format("e.MSG: {0}\n e.Stack: {1}", e.Message, e.StackTrace));
		    }
		}

	    private void MoveBots(Fight fight)
	    {
	        const string METHODNAME = "MoveBots";

	        foreach (var bot in fight.getBots)
	        {
	            var botAsTarget = bot.Value.Target as CBotInstance;
	            if (botAsTarget != null)
	            {
	                bot.Value.makeAMove(botAsTarget.ObjectId);
	            }
	        }
	    }

		public void Stop()
		{
			isRunning = false;
		}
	}
}