using System;
namespace RegionServer.Model.Interfaces
{
	public interface IPlayerListener
	{
		event Action<IPlayer> OnAddPlayer;
		event Action<IPlayer> OnRemovePlayer;

	}
}

