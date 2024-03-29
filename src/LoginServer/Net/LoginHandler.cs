using Server.Accounts;
using Server.Common;
using Server.Common.IO;
using Server.Common.IO.Packet;

namespace Server.Ghost
{
	public static class LoginHandler
	{
		public static void HandleLoginRequest(InPacket lea, Client c)
		{

			string username = lea.ReadString();

			string password = lea.ReadString();
			short passwordKey = lea.ReadShort();
			Log.Debug("Password Key : {0}", passwordKey);
			Log.Debug("Password = {0}", password);
			if (username.IsAlphaNumeric() == false)
			{
				LoginPacket.SendLoginResponse(c, ServerState.LoginState.PASSWORD_ERROR);
				return;
			}
			c.SetAccount(new Account(c));

			try
			{
				c.Account.Load(username.ToLower());
				if (LoginServer.IsMaintenance)
				{
					LoginPacket.SendLoginResponse(c, ServerState.LoginState.LOGIN_SERVER_DEAD);
					return;

				}
				else
				{



					if (c.RetryLoginCount >= 3)
					{
						c.Dispose();
					}
					if (password.IsAlphaNumeric() == false)
					{
						LoginPacket.SendLoginResponse(c, ServerState.LoginState.PASSWORD_ERROR);
						Log.Error("Login Fail!");
						c.RetryLoginCount += 1;
					}
					if (!password.Equals(c.Account.Password))
					{
						LoginPacket.SendLoginResponse(c, ServerState.LoginState.PASSWORD_ERROR);
						Log.Error("Login Fail!");
						Log.Inform("Retry Count: {0}", c.RetryLoginCount);
						c.RetryLoginCount += 1;
					}
					else if (c.Account.Banned > 1)
					{
						int LoginErrorCode = c.Account.Banned;
						switch (LoginErrorCode)
						{
							case 1:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.LOGIN_FAILED); //Account Lock
								break;
							case 7:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.BUG_LOCK); // Hack Ban
								break;
							case 8:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.BILLING_LOCK); // Billing Lock
								break;
							case 10:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.SPAM_LOCK); //SPAM  LOCK
								break;
							case 11:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.BUG_LOCK);  //Temp Banned
								break;
							case 12:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.USER_LOCK);
								break;
							case 13:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.NO_USERNAME);
								break;
							case 16:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.UNKNOWN_LOGIN_ERROR);
								break;
							case 17:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.ID_BLOCK_BUGPLAY2);
								break;
							case 29:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.HACK_LOCK);
								break;
							case 31:
								LoginPacket.SendLoginResponse(c, ServerState.LoginState.ID_BLOCK_NONE_ACTIVATION);
								break;
							default:
								return;
						}
					}
					else
					{
						int isMaster = c.Account.Master;
						LoginPacket.SendLoginResponse(c, ServerState.LoginState.OK);

						c.Account.LoggedIn = 1;

						Log.Success("Login Success! Username: {0}", username);
					}

				}

			}
			catch (NoAccountException)
			{

				LoginPacket.SendLoginResponse(c, ServerState.LoginState.ID_BLOCK_NONE_ACTIVATION);
				switch (1)
				{
					case 1:
						LoginPacket.SendLoginResponse(c, ServerState.LoginState.NO_USERNAME);
						break;
				}


				//if (ServerConstants.AUTO_REGISTRATION == true)
				//{
				//    if (username.Length < 5 || password.Length < 5)
				//        LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);


				//    //account.Username = username.ToLower();
				//    //account.Password = password;
				//    //account.Creation = DateTime.Now;
				//    //account.LoggedIn = 0;
				//    //account.Banned = 0;
				//    //account.Master = 0;
				//    //account.GamePoints = 0;
				//    //account.GiftPoints = 0;
				//    //account.BonusPoints = 0;

				//c.Account.Save();
				//    LoginPacket.Login_Ack(c, ServerState.LoginState.USER_LOCK);
				//    return;
				//}



			}
		}

		public static void HandleServerListRequest(InPacket lea, Client c)
		{
			if (c.Account.Banned > 1)
			{
				c.Dispose();
			}
			LoginPacket.SendServerListResponse(c);
		}

		public static void HandlePatchVersionRequest(InPacket packet, Client client)
		{
			LoginPacket.SendGameVersionInfo(client);
		}


		public static void HandleGameRequest(InPacket lea, Client c)
		{
			LoginPacket.Game_Ack(c, ServerState.ChannelState.OK);
		}

		public static void HandleWorldRequest(InPacket lea, Client c)
		{
			LoginPacket.World_Ack(c);
		}
		public static void HandleSubPasswordRequest(InPacket lea, Client c)
		{
			lea.ReadInt();
			string Password = lea.ReadString();
			string ConfrimPassword = lea.ReadString();
			Log.Debug("Account : {0}", c.Account.Username.ToString());
			Log.Debug("SubPassowrd Request Password: {0} Confrim : {1} ", Password, ConfrimPassword);
			int isSubPassword = c.Account.isTwoFactor;
			if (isSubPassword == 0)
			{

				if (Password != ConfrimPassword)
				{
					LoginPacket.SubPassError(c);
				}
				else
				{
					c.SetAccount(c.Account);
					c.Account.isTwoFactor = 1;
					c.Account.TwoFactorPassword = Password;
					c.Account.Save();
					LoginPacket.SubPassLoginOK(c);
				}

			}
			if (isSubPassword == 1)
			{
				string AccountSubPassword = c.Account.TwoFactorPassword;
				if (AccountSubPassword == Password)
				{
					LoginPacket.SubPassLoginOK(c);
				}
				else
				{
					LoginPacket.SubPassLoginWrong(c);
				}
			}



		}
	}
}
