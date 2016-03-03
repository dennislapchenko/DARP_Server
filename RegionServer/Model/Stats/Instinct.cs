using RegionServer.Model.Interfaces;


namespace RegionServer.Model.Stats
{
	public class Instinct : IStat
	{
		public Instinct()
		{
		}
		
		#region IStat implementation
		
		public string Name { get { return "Instinct"; } }
		public bool IsBaseStat { get { return true; } }
		public bool IsNonZero { get { return true; } }
		public float BaseValue { get { return 5;} set{} }
		
		#endregion
	}
}