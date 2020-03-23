using Server.Accounts;
using Server.Common;
using Server.Common.Constants;
using Server.Common.Data;
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

            c.SetAccount(new Account(c));
            try
            {
                /*
                    Load Username
                */
                c.Account.Load(username);

                /*
                    Check Password [IsAlphaNumeric / Equals]
                */
                if (!password.IsAlphaNumeric() || !password.Equals(c.Account.Password))
                {
                    Log.Error("[Login_Req] Username: {0} | Password: {1}", username, password);
                    LoginPacket.Login_Ack(c, ServerState.LoginState.PASSWORD_ERROR);
                    c.RetryLoginCount += 1;

                }
                
                /*
                    if RetryLoginCount >= 3
                */
                if (c.RetryLoginCount >= 3) 
                {
                    c.Dispose();
                }

                /*
                    Check Banned status
                */
                switch (c.Account.Banned)
                {
                    case 1:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.LOGIN_FAILED);
                        break;
                    case 7:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.BUG_LOCK);
                        break;
                    case 8:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.BILLING_LOCK);
                        break;
                    case 10:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.SPAM_LOCK);
                        break;
                    case 11:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.BUG_LOCK);
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
                        string isMaster = c.Account.Master == 1 ? "Master" : "non-Master";
                        Log.Success("[Login_Req] Username: {0} | Password: {1} |({2})", c.Account.Username, c.Account.Password, isMaster);
                        LoginPacket.Login_Ack(c, ServerState.LoginState.OK);
                        c.Account.LoggedIn = 1;
                        break;
                }

            }
            catch (NoAccountException e)
            {
                Log.Error("[Login_Req] Username: {0} | Password: {1} ({2})", username, password, e.Message);
                /*
                    Check ServerConstants.AUTO_REGISTRATION == True [ Register new account ]
                    Check ServerConstants.AUTO_REGISTRATION == False [ Return ServerState.LoginState.NO_USERNAME]
                
                */
                if (ServerConstants.AUTO_REGISTRATION)
                {
                    if (username.Length < 5 || password.Length < 5) 
                    {
                        LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);
                    }

                    Account account = new Account(c);
                    account.Username = username;
                    account.Password = password;
                    account.Creation = DateTime.Now;
                    /*
                        Dont hardcode it ? 
                    */
                    account.LoggedIn = 0;
                    account.Banned = 0;
                    account.Master = 0;
                    account.GamePoints = 0;
                    account.GiftPoints = 0;
                    account.BonusPoints = 0;
                    account.Save();
                    // Add Login success?
                    return;
                } else {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);
                    return;
                }
            }
        }

        /*

        */
        public static void ServerList_Req(InPacket lea, Client c)
        {
            /* Check If player use WPE (Winsock Packet Edior) to Skip Error Msg */
            switch (c.Account.Banned) 
            {
                case 1:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 29:
                    c.Dispose();
                    break;
            }

            LoginPacket.ServerList_Ack(c);
        }
        
        /*

        */
        public static void PatchVersion_Req(InPacket lea, Client c)
        {
            LoginPacket.GameVersionInfoAck(c);
        }

        /*

        */
        public static void Game_Req(InPacket lea, Client c)
        {
            LoginPacket.Game_Ack(c, ServerState.ChannelState.OK);
        }

        /*

        */
        public static void World_Req(InPacket lea, Client c)
        {
            LoginPacket.World_Ack(c);
        }

        /*
        
        */
        public static void TWOFACTOR_REQ(InPacket lea, Client c)
        {

            int isSubPassword = c.Account.TwoFA;
			string Password = lea.ReadString();
			string ConfrimPassword = lea.ReadString();
			string AccountSubPassword = c.Account.TwoFactorPassword;
			switch (isSubPassword)
            {

                case 0:
                    if (ServerConstants.DEBUG_MODE)
                    {
                        Log.Debug("2FA Request From Client: Password: {0}  ConfrimPassword: {1}", Password, ConfrimPassword);
                    }
                    LoginPacket.SubPassError(c);
                    break;
                case 1:
                    if ( AccountSubPassword == Password )
                    {
                        LoginPacket.SubPassLoginOK(c);
                    } else {
                        LoginPacket.SubPassLoginWrong(c);
                    }
                    break;
            }
        }
    }
}
