
namespace RegionServer.Model.Interfaces
{
	public interface ITeleportType
	{
		Position GetNearestTeleportLocation(ICharacter character);

	}

}

