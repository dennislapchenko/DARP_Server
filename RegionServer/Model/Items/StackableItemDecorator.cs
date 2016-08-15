using RegionServer.DataHelperObjects;
using RegionServer.Model.Interfaces;

namespace RegionServer.Model.Items
{
    public struct StackableItemDecorator
    {
        public IItem Item;
        public CappedByte Stack;

        public IItem GetItem()
        {
            return Stack.DecrValue() ? Item : null;
        }

        public static StackableItemDecorator Init(IItem item, byte currentStack)
        {
            return new StackableItemDecorator() {Item = item, Stack = CappedByte.Init(currentStack, item.MaxStack)};
        }
    }
}
