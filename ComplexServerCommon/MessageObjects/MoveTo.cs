namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class MoveTo
	{
		public MoveTo()
		{
			CurrentPosition = new PositionData();
			Destination = new PositionData();
		}

        public PositionData CurrentPosition;
        public PositionData Destination;

        public bool Moving;
        public MoveDirection Direction;
	}
}

