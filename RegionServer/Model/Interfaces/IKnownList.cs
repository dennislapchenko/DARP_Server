namespace RegionServer.Model.Interfaces
{
	public interface IKnownList
	{
		IObject Owner {get; set;}
		bool AddKnownObject(IObject obj);
		bool KnowsObject(IObject obj);
		void RemoveAllKnownObjects();
		bool RemoveKnownObject(IObject obj);
		void FindObjects();
		void ForgetObjects(bool fullCheck);
		int DistanceToForgetObject(IObject obj);
		int DistanceToWatchObject(IObject obj);

	}
}

