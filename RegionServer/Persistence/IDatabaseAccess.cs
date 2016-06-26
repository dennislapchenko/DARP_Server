using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegionServer.Persistence
{
    interface IDatabaseAccess
    {
        void execute();
    }
}
