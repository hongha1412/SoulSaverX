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
                bool login_status = true;
                switch (c.Account.Banned)
                {
                    case 1:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.LOGIN_FAILED);
                        login_status = false;
                        break;
                    case 7:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.BUG_LOCK);
                        login_status = false;
                        break;
                    case 8:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.BILLING_LOCK);
                        login_status = false;
                        break;
                    case 10:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.SPAM_LOCK);
                        login_status = false;
                        break;
                    case 11:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.BUG_LOCK);
                        login_status = false;
                        break;
                    case 12:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.USER_LOCK);
                        login_status = false;
                        break;
                    case 13:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);
                        login_status = false;
                        break;
                    case 16:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.UNKNOWN_LOGIN_ERROR);
                        login_status = false;
                        break;
                    case 17:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.ID_BLOCK_BUGPLAY2);
                        login_status = false;
                        break;
                    case 29:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.HACK_LOCK);
                        login_status = false;
                        break;
                    case 31:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.ID_BLOCK_NONE_ACTIVATION);
                        login_status = false;
                        break;
                }

                /*
                    Login Success
                */
                if (login_status)
                {
                    string isMaster = c.Account.Master ? "Master" : "User";
                    LoginPacket.Login_Ack(c, ServerState.LoginState.OK);
                    Log.Success("[Login_Req] Username: {0} | Password: {1} |({2})", c.Account.Username, c.Account.password, isMaster);
                    c.Account.LoggedIn = 1;
                }
            }
            catch (NoAccountException)
            {
                /*
                // TODO !
				int hardcode_switch = 2;
                switch (hardcode_switch)
                {
                    case 1:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);
                        break;
                    case 2:
                        LoginPacket.Login_Ack(c, ServerState.LoginState.PASSWORD_ERROR);
                        break;
                }
                */
                
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
                } else {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);
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

            switch (isSubPassword)
            {

                case 0:
                    string Password = lea.ReadString();
                    string ConfrimPassword = lea.ReadString();
                    if (ServerConstants.DEBUG_MODE)
                    {
                        Log.Debug("2FA Request From Client: Password: {0}  ConfrimPassword: {1}", Password, ConfrimPassword);
                    }
                    LoginPacket.SubPassError(c);
                    break;
                case 1:
                    string Password = lea.ReadString();
                    string AccountSubPassword = c.Account.TwoFactorPassword;
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
