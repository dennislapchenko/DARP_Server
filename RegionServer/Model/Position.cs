
using ComplexServerCommon.MessageObjects;
using ComplexServerCommon.Enums;

namespace RegionServer.Model
{
	public class Position
	{
		private LocationType LocationHighlevel;
		private LocationType LocationLowlevel;

		public Position()
			: this (LocationType.CITY, LocationType.CITY)
		{ }

		public Position(LocationType highLevel)
            : this (highLevel, highLevel)
        { }

		public Position(LocationType highLevel, LocationType lowLevel)
		{
			this.LocationHighlevel = highLevel;
			this.LocationLowlevel = lowLevel;
		}

		public void setPosition(LocationType high, LocationType low)
		{
			LocationHighlevel = high;
			LocationLowlevel = low;
		}

		public LocationType[] getPosition()
		{
			return new LocationType[] {LocationHighlevel, LocationLowlevel};
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", LocationHighlevel, LocationLowlevel);
		}

		public static implicit operator PositionData(Position pos)
		{
			return new PositionData(pos.LocationHighlevel, pos.LocationLowlevel);
		}
	 
		public static implicit operator Position(PositionData position)
		{
			return new Position(position.LocationHighlevel, position.LocationLowlevel);
		}

		public string Serialize()
		{
			return ComplexServerCommon.SerializeUtil.Serialize<PositionData>((PositionData)this);
		}

		public static PositionData Deserialize(string value)
		{
			return ComplexServerCommon.SerializeUtil.Deserialize<PositionData>(value);
		}


	}
}

