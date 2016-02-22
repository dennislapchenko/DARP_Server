
using System;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class ChatItem
	{
		public string TellPlayer {get; set;}
		public string Text {get;set;}
		public ChatType Type {get; set;}
		public string ByPlayer {get; set;}
	}	
}
