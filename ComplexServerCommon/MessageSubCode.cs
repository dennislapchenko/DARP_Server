namespace ComplexServerCommon
{
	public enum MessageSubCode
	{

		//Login Server Code
		Register,
		Login,
		ListCharacters,
		SelectCharacter,
		CreateCharacter,
		Logout,

		//Chat Server Code
		Chat,

		//Region Server Code
		PlayerInGame,
		DeleteObject,
		LoadIngameScene,
		RegenUpdates,

		//items
		EquipItem,
		DequipItem,
		UseItem,
		RemoveItem,

		//fight
		PullQueue,
		CreateQueue,
		JoinQueue,
		FightQueueParticipants,
		FightParticipants,
		LeaveQueue,
		ReadyPrompt,
		PlayerReadyQueue,
		StartFight,
		UserFightInfo,
		PlayerInFight,
		SendMove,
		SwitchTarget,
		FightUpdate,
		FinishFight,
	

		//position & general stats
		UserInfo,
		Statistics,
		CharInfo,
		StatusUpdate,
		MoveToLocation,
		PlayerMovement,
		TeleportToLocation,

        //character
        StatAllocation,
	}
}

