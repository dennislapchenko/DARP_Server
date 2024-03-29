using System;
using System.Collections.Generic;
using RegionServer.Model.Interfaces;
using RegionServer.Model;

namespace RegionServer.Calculators
{
	public class Calculator
	{
		protected List<IFunction> Functions = new List<IFunction>();

		public void AddFunction(IFunction function)
		{
			Functions.Add(function);
			Functions.Sort((x,y) => x.Order.CompareTo(y.Order));
		}

		public void AddFunction(IEnumerable<IFunction> functions)
		{
			foreach(IFunction function in functions)
			{
				Functions.Add(function);
			}
			Functions.Sort((x,y) => x.Order.CompareTo(y.Order));
		}

		public void RemoveFunction(IFunction function)
		{
			Functions.Remove(function);
		}

		public void RemoveFunction(IEnumerable<IFunction> functions)
		{
			foreach(IFunction function in functions)
			{
				Functions.Remove(function);
			}
		}

	    public void ReplaceAllFunctions(List<IFunction> newFunctions)
	    {
	        Functions = newFunctions;
            Functions.Sort((x, y) => x.Order.CompareTo(y.Order));
        }

		public List<IStat> RemoveOwner(CObject owner)
		{
			var modifiedStats = new List<IStat>();

			foreach (var function in Functions)
			{
				if(function.Owner == owner)
				{
					modifiedStats.Add(function.Stat);
					RemoveFunction(function);
				}
			}
			return modifiedStats;
		}

		public void Calculate(Environment env)
		{
			foreach (var function in Functions)
			{
                //DebugUtils.Logp(String.Format("Calculating... Func:{0} pre-calc value:", function.GetType()), env.Value + " " + function.Stat.Name);
				function.Calc(env);
                //DebugUtils.Logp(String.Format("Calculating... Func:{0} post-calc value:", function.GetType()), env.Value + " " + function.Stat.Name);
            }
		}
	}
}

