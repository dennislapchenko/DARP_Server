
namespace RegionServer.Model.Interfaces
{
    public interface IItemStatHolder
    {
        void SetStat<T>(T stat, float value) where T : class, IStat;
    }
}
