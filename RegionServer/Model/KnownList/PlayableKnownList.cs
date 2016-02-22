using System;
using RegionServer.Model.Interfaces;
using System.Threading.Tasks;


namespace RegionServer.Model.KnownList
{
	public class PlayableKnownList : CharacterKnownList
	{
		public override void FindObjects()
		{
			Parallel.ForEach(Owner.Region.VisibleObjects, obj => 
											                	{
																	if (obj != Owner)
																	{
																		AddKnownObject(obj);
																		if (obj is ICharacter)
																		{
																				obj.KnownList.AddKnownObject(Owner);
																		}
																	}
																});
		}
	}
}

