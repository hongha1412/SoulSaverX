using Server.Accounts;
using Server.Common;
using Server.Common.Constants;
using Server.Common.IO;
using Server.Common.IO.Packet;
using System;

namespace Server.Ghost
{
	public static class LoginHandler
	{
		public static void Login_Req(InPacket lea, Client c)
		{

			string username = lea.ReadString();

			string password = lea.ReadString();
			short passwordKey = lea.ReadShort();
			Log.Debug("Password Key : {0}", passwordKey);
			Log.Debug("Password = {0}", password);
			if (username.IsAlphaNumeric() == false)
			{
				LoginPacket.Login_Ack(c, ServerState.LoginState.PASSWORD_ERROR);
				return;
			}
			c.SetAccount(new Account(c));

			try
			{
				c.Account.Load(username.ToLower());
				if (LoginServer.IsMaintenance)
				{
					LoginPacket.Login_Ack(c, ServerState.LoginState.LOGIN_SERVER_DEAD);

				}
				else
				{
					


					if (c.RetryLoginCount >= 3)
					{
						c.Dispose();
					}
					if (password.IsAlphaNumeric() == false)
					{
						LoginPacket.Login_Ack(c, ServerState.LoginState.PASSWORD_ERROR);
						Log.Error("Login Fail!");
						c.RetryLoginCount += 1;
					}
					if (!password.Equals(c.Account.Password))
					{
						LoginPacket.Login_Ack(c, ServerState.LoginState.PASSWORD_ERROR);
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
								LoginPacket.Login_Ack(c, ServerState.LoginState.LOGIN_FAILED); //Account Lock
								break;
							case 7:
								LoginPacket.Login_Ack(c, ServerState.LoginState.BUG_LOCK); // Hack Ban
								break;
							case 8:
								LoginPacket.Login_Ack(c, ServerState.LoginState.BILLING_LOCK); // Billing Lock
								break;
							case 10:
								LoginPacket.Login_Ack(c, ServerState.LoginState.SPAM_LOCK); //SPAM  LOCK
								break;
							case 11:
								LoginPacket.Login_Ack(c, ServerState.LoginState.BUG_LOCK);  //Temp Banned
								break;
							case 12:
								LoginPacket.Login_Ack(c, ServerState.LoginState.USER_LOCK);
								break;
							case 13:
								LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);
								break;
							case 16:
								LoginPacket.Login_Ack(c, ServerState.LoginState.UNKNOWN_LOGIN_ERROR);
								break;
							case 17:
								LoginPacket.Login_Ack(c, ServerState.LoginState.ID_BLOCK_BUGPLAY2);
								break;
							case 29:
								LoginPacket.Login_Ack(c, ServerState.LoginState.HACK_LOCK);
								break;
							case 31:
								LoginPacket.Login_Ack(c, ServerState.LoginState.ID_BLOCK_NONE_ACTIVATION);
								break;
							default:
								return;
						}
					}
					else
					{
						int isMaster = c.Account.Master;
						LoginPacket.Login_Ack(c, ServerState.LoginState.OK);

						c.Account.LoggedIn = 1;

						Log.Success("Login Success! Username: {0}", username);
					}

				}

			}
			catch (NoAccountException)
			{
<<<<<<< Updated upstream
				LoginPacket.Login_Ack(c, ServerState.LoginState.ID_BLOCK_NONE_ACTIVATION);
=======
				switch (1)
				{
					case 1:
						LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);
						break;
					case 2:
						LoginPacket.Login_Ack(c, ServerState.LoginState.PASSWORD_ERROR);
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


>>>>>>> Stashed changes
			}
		}

		public static void ServerList_Req(InPacket lea, Client c)
		{
			if (c.Account.Banned > 1)
			{
				c.Dispose();
			}
			LoginPacket.ServerList_Ack(c);
		}

		public static void PatchVersion_Req(InPacket lea, Client c)
		{
			LoginPacket.GameVersionInfoAck(c);
		}

		public static void Game_Req(InPacket lea, Client c)
		{
			LoginPacket.Game_Ack(c, ServerState.ChannelState.OK);
		}

		public static void World_Req(InPacket lea, Client c)
		{
			LoginPacket.World_Ack(c);
		}
		public static void TWOFACTOR_REQ(InPacket lea, Client c)
		{
			lea.ReadInt();
			string Password = lea.ReadString();
			string ConfrimPassword = lea.ReadString();
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
					c.SetAccount(new Account(c));
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
