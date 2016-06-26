namespace RegionServer.Model.Interfaces
{
	public interface IBot : ICharacter
	{
		void joinQueue(IFight fight);
		void makeAMove();
		void configureBot(byte level);
	}
}

