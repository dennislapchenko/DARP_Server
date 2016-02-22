using System;
namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class CharacterListItem
	{
		public int Id {get; set;}
		public string Name {get; set;}
		public int Level {get; set;}
		public string Class {get; set;}
		public string Sex {get; set;}
	}
}

