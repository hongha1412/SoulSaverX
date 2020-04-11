using Server.Common.Constants;
using Server.Common.IO;
using Server.Common.IO.Packet;
using Server.Common.Net;
using Server.Interoperability;

namespace Server.Ghost
{
    public static class LoginPacket
    {
        public static void GameVersionInfoAck(Client c)
        {
            using (var plew = new OutPacket())
            {
                plew.WriteHexString("AA 55 2F 00 11"); // Packet Header
                plew.WriteInt(2020041106); // Patch Version YYYYMMDD VV Eg 2020031501
                plew.WriteHexString("00 00 00 00");
                plew.WriteString("http://patch.ghostonline.xyz/");
                plew.WriteString("test/");
                plew.WriteHexString("55 AA"); //END Packet
                c.SendCustom(plew);
            }
        }



        /* NetCafe
         * 會員於特約網咖連線
         */
        public static void Login_Ack(Client c, ServerState.LoginState state, short encryptKey = 0, bool netCafe = false)
        {
            using (var plew = new OutPacket(LoginServerOpcode.LOGIN_ACK))
            {

                int LoginCase;
                if(c.Account.Master == 1 && c.Account.TwoFA == 1)
                {
                    LoginCase = 1;
                }else if(c.Account.Master == 0 && c.Account.TwoFA == 1)
                {
                    LoginCase = 2;
                }
                else if (c.Account.Master == 1 && c.Account.TwoFA == 0)
                {
                    LoginCase = 3;
                }
                else if (c.Account.Master == 0 && c.Account.TwoFA == 0)
                {
                    LoginCase = 4;
                }else
                {

                    LoginCase = 5;
                }

                plew.WriteByte((byte)state);
                plew.WriteHexString("00");
                // plew.WriteInt(isGM); // Send GM Status
                switch (LoginCase)
                {
                    case 1:
                        plew.WriteHexString("01 01");
                        break;
                    case 2:
                        plew.WriteHexString("00 01");
                        break;
                    case 3:
                        plew.WriteHexString("01 00");
                        break;
                    case 4:
                        plew.WriteHexString("00 00");
                        break;
                    case 5:
                        plew.WriteHexString("00 00");
                        break;
                    default:
                        break;
                }
                plew.WriteBytes(new byte[]
                    {0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00, 0x00, 0x00});
                //plew.WriteBool(netCafe);
                //plew.WriteShort(encryptKey);
                c.Send(plew);
            }
        }

        public static void ServerList_Ack(Client c)
        {
            using (var plew = new OutPacket(LoginServerOpcode.SERVERLIST_ACK))
            {
				for (int i = 0; i < 13; i++)
				{
					plew.WriteByte(0xFF);
				}
				plew.WriteInt(LoginServer.Worlds.Count); // Number of servers
				foreach (World world in LoginServer.Worlds)
				{
					plew.WriteShort(world.ID); // Server order
					plew.WriteInt(world.Channel); // Number of channels

					for (int i = 0; i < 8; i++)
					{
						plew.WriteShort(i + 1);
						plew.WriteShort(i + 1);
						plew.WriteString(ServerConstants.SERVER_IP);
						plew.WriteInt(15101 + i);
						plew.WriteInt(i < world.Count ? world[i].LoadProportion : 0); // Number of players
						plew.WriteInt(ServerConstants.CHANNEL_LOAD); // Maximum number of channels
						plew.WriteInt(1); // Type of seal
						plew.WriteInt(0);
						plew.WriteByte(i < world.Count ? 1 : 4); // Channel open
						plew.WriteInt(15199);
					}
				}

				c.Send(plew);
            }
        }

        public static void Game_Ack(Client c, ServerState.ChannelState state)
        {
            using (var plew = new OutPacket(LoginServerOpcode.GAME_ACK))
            {
                plew.WriteByte((byte)state);
                plew.WriteString(ServerConstants.SERVER_IP);
                plew.WriteInt(15101 + c.World.ID);
                plew.WriteInt(ServerConstants.UDP_PORT);

                c.Send(plew);
            }
        }

        public static void SubPassError(Client c)
        {
            using(var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
            {
                plew.WriteHexString("00 00 00 00 00 00 00 00");
                c.Send(plew);
            }
        }

        public static void SubPassLoginOK(Client c)
        {
            using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
            {
                plew.WriteHexString("01 00 00 00 01 00 00 00");
                c.Send(plew);
            }
        }
        public static void SubPassLoginWrong(Client c)
        {
            using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
            {
                plew.WriteHexString("01 00 00 00 00 00 00 00");
                c.Send(plew);
            }
        }
        public static void SubPassAddOK(Client c)
        {
            using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
            {
                plew.WriteHexString("00 00 00 00 01 00 00 00");
                c.Send(plew);
            }
        }
        public static void World_Ack(Client c)
        {
            using (var plew = new OutPacket(LoginServerOpcode.WORLD_ACK))
            {
                try
                {
                    plew.WriteString("127.0.0.1"); // 219.83.162.27
                    plew.WriteString("15010");
                    plew.WriteString("127.0.0.1"); // 219.83.162.27
                    plew.WriteString("15111");
                    //plew.WriteBytes(new byte[] { 0x00, 0x00, 0x00, 0xBB, 0x19, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                }
                catch
                {
                }

                c.Send(plew);
            }
        }
    }
}
