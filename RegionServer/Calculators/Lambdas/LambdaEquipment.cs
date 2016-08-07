using RegionServer.Model.Interfaces;

namespace RegionServer.Calculators.Lambdas
{
	public class LambdaEquipment : ILambda
	{
		private readonly IStat _stat;

		public LambdaEquipment(IStat stat)
		{
			this._stat = stat;
		}


		public float Calculate(Environment env)
		{
			float saveValue = env.Value;
			float returnValue = 0;
			env.Value = 0;

			if(env.Character != null)
			{
				foreach(var item in env.Character.Items.Equipment.Values)
				{
				    if (item.Stats.Stats.ContainsKey(_stat.GetType()))
				    {
				        env.Value += item.Stats.GetStatBase(_stat);
				    }
				}
			}
			returnValue = env.Value;
			env.Value = saveValue;

			return returnValue;
		}

	}
}

