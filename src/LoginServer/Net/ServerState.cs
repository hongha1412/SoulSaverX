namespace Server.Ghost
{
	public class ServerState
	{
		public enum LoginState
		{
			OK = 0,
			LOGIN_FAILED = 1,
			PASSWORD_ERROR = 2,
			BUG_LOCK = 7,
			LOCK_5 = 5,
			LOCK_6 = 6,
			BILLING_LOCK = 8,
			FUCK_LOCK = 9,
			SPAM_LOCK = 10,
			TMPE_LOCK = 11,
			USER_LOCK = 12,
			NO_USERNAME = 13,
			PASSWORD_ERR = 14,
			PASSWORD_ERR2 = 15,
			UNKNOWN_LOGIN_ERROR = 16,
			ID_BLOCK_BUGPLAY2 = 17,
			BLOCK_BUGPLAY2 = 23,
			HACK_LOCK = 29,
			LOGIN_SERVER_DEAD = 30,
			ID_BLOCK_NONE_ACTIVATION = 31,
			LOGIN_FAIL33 = 33,
			COUNTRY_RESTRICT = 37,
			VPN_NOT_ALLOWED = 38
		}

		public enum ChannelState
		{
			OK = 0,
			OTHER = 1,
			CONNECTED_WAITE = 4,
			YEARS_OLD = 28
		}
	}
}
