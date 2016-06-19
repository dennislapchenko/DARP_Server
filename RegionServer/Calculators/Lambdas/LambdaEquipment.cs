using RegionServer.Model.Interfaces;

namespace RegionServer.Calculators.Lambdas
{
	public class LambdaEquipment : ILambda
	{
		private IStat _stat;

		public LambdaEquipment(IStat stat)
		{
			_stat = stat;
		}


		public float Calculate(Environment env)
		{
			float saveValue = env.Value;
			float returnValue = 0;
			env.Value = 0;

			if(env.Character != null)
			{
				foreach(var item in env.Character.Items.Equipment)
				{
					if(item.Value != null)
					{
						env.Value += item.Value.Stats.GetStat(_stat);
					}
				}
			}
			returnValue = env.Value;
			env.Value = saveValue;

			return returnValue;
		}

	}
}

