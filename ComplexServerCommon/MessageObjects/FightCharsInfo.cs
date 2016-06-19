using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class FightCharsInfo
	{
		public FightType Type {get;set;}
		public bool Sanguinity {get;set;}
		public List<CharInfo> RedInfo {get;set;}
		public List<CharInfo> BlueInfo {get;set;}
		public string UsersTeam {get;set;}
	}
}

