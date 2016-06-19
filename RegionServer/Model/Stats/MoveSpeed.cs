
using RegionServer.Model.Interfaces;


namespace RegionServer.Model.Stats
{
	public class MoveSpeed : IStat
	{
		public MoveSpeed()
		{
		}

		public void ConvertToIsOnItem(float value)
		{
			throw new System.NotImplementedException();
		}

		public bool IsOnItem
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		public bool IsNonNegative
		{
			get
			{
				throw new System.NotImplementedException();
			}
		}

		public bool IsForCombat
		{
			get
			{
				throw new System.NotImplementedException();
			}
		}

		public bool IsItemStat
		{
			get
			{
				throw new System.NotImplementedException();
			}
		}

		#region IStat implementation
		public string Name { get { return "Move Speed"; } }
		public bool IsBaseStat { get { return true; } }
		public bool IsNonZero { get { return false; } }
		public float BaseValue {get { return 25f; } set {} }
		#endregion
	}
}

