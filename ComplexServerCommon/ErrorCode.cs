
namespace ComplexServerCommon
{
	public enum ErrorCode : short
	{
		OperationDenied = -3,
		OperationInvalid = -2,
		InternalServerError = -1,

		OK = 0,
		UserNameInUse = 1,
		UserNamePasswordInvalid = 2,
		UserCurrentlyLoggedIn = 3,
		InvalidCharacter,
		AlreadyInFight
	}
}

