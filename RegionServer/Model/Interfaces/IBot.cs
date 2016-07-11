namespace RegionServer.Model.Interfaces
{
	public interface IBot
	{
		void joinQueue(IFight fight);
		void makeAMove(int targetId);
		void configureBot(byte level);
	}
}

