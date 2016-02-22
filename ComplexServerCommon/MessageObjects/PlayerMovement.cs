using System;
namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class PlayerMovement
	{
		public int ObjectId {get; set;}
		public int Facing {get; set;}
		public bool Walk {get; set;}
		public bool Duck {get; set;}
		public bool Moving {get; set;}
		public float Forward {get; set;} //which direction is forward
		public float Right {get; set;} //+ is right, - is left, 0 is straight
		public bool Jump {get; set;}

	}
}

