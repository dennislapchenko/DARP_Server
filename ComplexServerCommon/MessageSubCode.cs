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

		//Chat Server Code
		Chat,
		//Region Server Code
		CharInfo,
		DeleteObject,
		MoveToLocation,
		PlayerInGame,
		PlayerMovement,
		StatusUpdate,
		StopMove,
		TeleportToLocation,
		UserInfo
	}
}

