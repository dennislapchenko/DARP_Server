using System;
using MMO.Framework;
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
    public class BotQueenMoveDeployBackgroundThread : IBackgroundThread
    {
        public static readonly string CLASSNAME = "BotQueenMoveDeployBackgroundThread";

        private static readonly int UPDATE_SPEED = 4300; //15000sec

        public FightManager FightManager { get; set; }
        private bool isRunning { get; set; }

        protected static readonly ILogger Log = LogManager.GetCurrentClassLogger();


        public BotQueenMoveDeployBackgroundThread(FightManager fightManager, NPCFactory npcFactory)
        {
            FightManager = fightManager;
        }

        public void Setup()
        {
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
            Parallel.ForEach(FightManager.getAllBotFights().Where(f => f.fightState == FightState.ENGAGED), MoveBots);
        }

        private void MoveBots(Fight fight)
        {
            const string METHODNAME = "MoveBots";
            try
            {
                foreach (var bot in fight.getBots.Values.Where(bot => !bot.IsDead))
                {
                    if (bot.Target != null)
                    {
                        bot.makeAMove();
                    }
                }
            }
            catch (Exception e)
            {
                DebugUtils.Logp(DebugUtils.Level.WARNING, CLASSNAME, METHODNAME, e.Message + e.StackTrace);
            }
   
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}