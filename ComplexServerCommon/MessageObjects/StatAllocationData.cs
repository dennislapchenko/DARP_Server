using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
    public class StatAllocationData
    {
        public Dictionary<int, int> Allocations; //stat id + points to allocate
        public bool ResetAllPoints;
    }
}
