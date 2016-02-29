using System;

namespace ComplexServerCommon.MessageObjects
{
	[Flags]
	public enum MoveDirection
	{
		None = 0,
		Forward = 1,
		Backward = 2,
		Left = 4,
		Right = 8,
		ForwardLeft = 5,
		ForwardRight = 9,
		BackwardLeft = 6,
		BackwardRight = 10
	}
}

