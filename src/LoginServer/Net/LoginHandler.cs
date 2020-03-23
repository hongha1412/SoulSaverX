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

            if (username.IsAlphaNumeric() == false)
            {
                LoginPacket.Login_Ack(c, ServerState.LoginState.PASSWORD_ERROR);
                return;
            }

            c.SetAccount(new Account(c));

            try
            {
                c.Account.Load(username);

                if(c.RetryLoginCount >= 3)
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
                else if (c.Account.Banned == 1)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.LOGIN_FAILED); //Account Lock
                }

                
                
                else if (c.Account.Banned == 7)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.BUG_LOCK); // Hack Ban
                }
                else if (c.Account.Banned == 8)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.BILLING_LOCK); // Billing Lock
                }
                else if (c.Account.Banned == 10)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.SPAM_LOCK); //SPAM  LOCK
                }
                else if (c.Account.Banned == 11)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.BUG_LOCK);  //Temp Banned
                }
                else if (c.Account.Banned == 12)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.USER_LOCK);
                }
                else if (c.Account.Banned == 13)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.NO_USERNAME);
                }
                else if (c.Account.Banned == 16)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.UNKNOWN_LOGIN_ERROR);
                }
                else if (c.Account.Banned == 17)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.ID_BLOCK_BUGPLAY2);
                }
                else if (c.Account.Banned == 29)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.HACK_LOCK);
                }
                else if(c.Account.Banned == 31)
                {
                    LoginPacket.Login_Ack(c, ServerState.LoginState.ID_BLOCK_NONE_ACTIVATION);
                }


                //LOGIN_FAILED
                else
                {
                    int isMaster = c.Account.Master;
                    LoginPacket.Login_Ack(c, ServerState.LoginState.OK);
                    
                    c.Account.LoggedIn = 1;
                   
                    Log.Success("Login Success! Username: {0}", username);
                }

                Log.Debug("Password = {0}", password);
            }
            catch (NoAccountException)
            {
                switch (2)
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

                 Account account = new Account(c);
                //    //account.Username = username.ToLower();
                //    //account.Password = password;
                //    //account.Creation = DateTime.Now;
                //    //account.LoggedIn = 0;
                //    //account.Banned = 0;
                //    //account.Master = 0;
                //    //account.GamePoints = 0;
                //    //account.GiftPoints = 0;
                //    //account.BonusPoints = 0;

                   account.Save();
                //    LoginPacket.Login_Ack(c, ServerState.LoginState.USER_LOCK);
                //    return;
                //}


            }
        }

        public static void ServerList_Req(InPacket lea, Client c)
        {
            switch (c.Account.Banned) // Check If player use WPE (Winsock Packet Edior) to Skip Error Msg
            {
                case 1:
                    c.Dispose();
                    break;
                case 7:
                    c.Dispose();
                    break;
                case 8:
                    c.Dispose();
                    break;
                case 9:
                    c.Dispose();
                    break;
                case 10:
                    c.Dispose();
                    break;
                case 11:
                    c.Dispose();
                    break;
                case 12:
                    c.Dispose();
                    break;
                case 13:
                    c.Dispose();
                    break;
                case 29:
                    c.Dispose();
                    break;
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

            int isSubPassword = c.Account.TwoFA;
            if(isSubPassword == 0)
            {
                string Password = lea.ReadString();
                string ConfrimPassword = lea.ReadString();
                Log.Debug("2FA Request From Client: Password: {0}  ConfrimPassword: {1}", Password, ConfrimPassword);
                LoginPacket.SubPassError(c);
            }if(isSubPassword == 1)
            {
                string Password = lea.ReadString();
                string AccountSubPassword = c.Account.TwoFactorPassword;
                if(AccountSubPassword == Password)
                {
                    LoginPacket.SubPassLoginOK(c);
                }else
                {
                    LoginPacket.SubPassLoginWrong(c);
                }
            }
           
            
            
        }
    }
}
