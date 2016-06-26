using System;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class PositionData
	{
        /*  Highlevel location  - where in the outer sense the character is
         *  Lowlevel location   - which exact location it is
         *      i.e:    CITY - Magadan
         *              SHOP - Venruki the Cock's
         *              FIGHT - Magadan Pits
         */
        public LocationType LocationHighlevel { get; set; }
        public LocationType LocationLowlevel { get; set; }
		
		public PositionData()
			: this (LocationType.CITY, LocationType.CITY)
		{}

        public PositionData(LocationType highLevel)
            : this (highLevel, highLevel)
        {}

	    public PositionData(LocationType highLevel, LocationType lowLevel)
	    {
	        this.LocationHighlevel = highLevel;
	        this.LocationLowlevel = highLevel;
	    }

		public override string ToString()
		{
			return string.Format("Location: {0} - {1}", LocationHighlevel, LocationLowlevel);
		}
	}
}

