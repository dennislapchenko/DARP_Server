using RegionServer.Model.Interfaces;

namespace RegionServer.Calculators
{
	public class Environment
	{
		public ICharacter Character {get; set;}
		public ICharacter Target {get; set;}
		public float Value {get; set;}

	}
}

