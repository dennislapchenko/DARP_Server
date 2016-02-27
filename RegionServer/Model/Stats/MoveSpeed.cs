
using RegionServer.Model.Interfaces;


namespace RegionServer.Model.Stats
{
	public class MoveSpeed : IStat
	{
		public MoveSpeed()
		{
		}

		#region IStat implementation
		public string Name { get { return "Move Speed"; } }
		public bool IsBaseStat { get { return true; } }
		public bool IsNonZero { get { return false; } }
		public float BaseValue {get { return 6f; } set {} }
		#endregion
	}
}

