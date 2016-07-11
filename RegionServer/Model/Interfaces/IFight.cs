using ComplexServerCommon.Enums;
using System.Collections.Generic;
using System;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Interfaces
{
	public interface IFight
	{
		Guid FightId {get;set;}
		string Creator {get;set;}
		FightState fightState {get;set;}
		FightType Type {get;set;}
		int TeamSize {get;set;}
		float Timeout {get;set;}
		bool Sanguinary {get;set;}
		bool BotsWelcome { get; set; }
        Position FightLocation { get; set; }
		Dictionary<int, CCharacter> TeamBlue {get; set;}
		Dictionary<int, CCharacter> TeamRed {get; set;}
		//Dicitonary<int, IMove> Moves {get;set;}

		bool addPlayer(int team, CCharacter player);
		bool addPlayer(CCharacter player);
		bool RemovePlayer(CCharacter player);
		FightQueueListItem GetQueueInfo();

	}
}

