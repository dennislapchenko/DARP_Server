using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using ExitGames.Logging;

namespace RegionServer.Model.NPC
{
    public class NPCFactory
    {
        protected static ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly CBotInstance.Factory _fightBotFactory;

        public NPCFactory(CBotInstance.Factory fightBotFactory)
        {
            _fightBotFactory = fightBotFactory;
        }

        public CBotInstance createFightBot(byte level)
        {
            var newBot = _fightBotFactory.Invoke(level);
            newBot.configureBot();

            return newBot;
        }
    }
}
