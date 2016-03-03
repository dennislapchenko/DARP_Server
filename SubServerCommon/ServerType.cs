using System;
namespace SubServerCommon
{
	[Flags]
	public enum ServerType
	{
		Proxy = 0x1,
		Login = 0x2,
		Chat = 0x4,
		Region = 0x8
	}
}

