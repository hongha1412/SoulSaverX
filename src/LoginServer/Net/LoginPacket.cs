using Server.Common.Constants;
using Server.Common.IO.Packet;
using Server.Common.Net;

namespace Server.Ghost
{
	public static class LoginPacket
	{
		public static void GameVersionInfoAck(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.PATCH_ACK))
			{
				plew.WriteInt(2021070701); // Patch Version from patch.dat
				plew.WriteHexString("00 00 00 00");
				plew.WriteString("http://127.0.0.1:8080/ghostsoul/");
				plew.WriteString("test/");
				c.Send(plew);
			}
		}



		/* NetCafe
         * 會員於特約網咖連線
         */
		public static void Login_Ack(Client c, ServerState.LoginState state, short encryptKey = 0, bool netCafe = false)
		{
			using (var plew = new OutPacket(LoginServerOpcode.LOGIN_ACK))
			{

#if DEBUG
				plew.WriteByte(0); //bypass password check in debug mode
#else
					plew.WriteByte((byte)state);
#endif
				plew.WriteByte(0);
				plew.WriteByte(c.Account.Master);
				plew.WriteByte(c.Account.TwoFA);
				plew.WriteBytes(new byte[]
					{0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00, 0x00, 0x00});
				c.Send(plew);
			}
		}

		public static void ServerList_Ack(Client c)
		{

			using (var plew = new OutPacket(LoginServerOpcode.SERVERLIST_ACK))
			{


				plew.WriteBytes(new byte[]
			{
								0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x01,
				0x00, 0x01, 0x00, 0x0E, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x30, 0x30,
				0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x31, 0xFD, 0x3A, 0x00, 0x00, 0x83,
				0x00, 0x00, 0x00, 0x2C, 0x01, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x01, 0x5F, 0x3B, 0x00, 0x00, 0x02, 0x00, 0x02, 0x00,
				0x0E, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x30,
				0x30, 0x2E, 0x30, 0x31, 0xFE, 0x3A, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00,
				0x64, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x01, 0x5E, 0x3B, 0x00, 0x00, 0x03, 0x00, 0x03, 0x00, 0x0E, 0x00, 0x31,
				0x32, 0x37, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30,
				0x31, 0xFD, 0x3A, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00,
				0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x5F, 0x3B,
				0x00, 0x00, 0x04, 0x00, 0x04, 0x00, 0x0E, 0x00, 0x31, 0x32, 0x37, 0x2E,
				0x30, 0x30, 0x30, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x31, 0xFE, 0x3A,
				0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x0C, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x5E, 0x3B, 0x00, 0x00, 0x05,
				0x00, 0x05, 0x00, 0x0E, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x30, 0x30,
				0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x31, 0xFF, 0x3A, 0x00, 0x00, 0x05,
				0x00, 0x00, 0x00, 0x2C, 0x01, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x01, 0x5D, 0x3B, 0x00, 0x00, 0x06, 0x00, 0x06, 0x00,
				0x0E, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x30,
				0x30, 0x2E, 0x30, 0x31, 0xFD, 0x3A, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00,
				0x2C, 0x01, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x01, 0x5F, 0x3B, 0x00, 0x00, 0x07, 0x00, 0x07, 0x00, 0x0E, 0x00, 0x31,
				0x32, 0x37, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30,
				0x31, 0xFE, 0x3A, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00,
				0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x5E, 0x3B,
				0x00, 0x00, 0x08, 0x00, 0x08, 0x00, 0x0E, 0x00, 0x31, 0x32, 0x37, 0x2E,
				0x30, 0x30, 0x30, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x31, 0xFF, 0x3A,
				0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x0C, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x5D, 0x3B, 0x00, 0x00, 0x09,
				0x00, 0x09, 0x00, 0x0F, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x30, 0x30,
				0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30, 0x30, 0x31, 0xFE, 0x3A, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x90, 0x01, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x5E, 0x3B, 0x00, 0x00, 0x0A, 0x00, 0x0A,
				0x00, 0x0D, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x30, 0x30, 0x2E, 0x30,
				0x30, 0x30, 0x2E, 0x31, 0xFE, 0x3A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x90, 0x01, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x5E, 0x3B, 0x00, 0x00, 0x0B, 0x00, 0x0B, 0x00, 0x09, 0x00, 0x31,
				0x32, 0x37, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x31, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x90, 0x01, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x0C,
				0x00, 0x09, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x31,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x90, 0x01, 0x00, 0x00,
				0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
				0x00, 0x0D, 0x00, 0x0D, 0x00, 0x09, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30,
				0x2E, 0x30, 0x2E, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x64, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x02, 0x00, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x0E, 0x00, 0x09, 0x00, 0x31,
				0x32, 0x37, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x31, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x0F,
				0x00, 0x09, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x31,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00,
				0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
				0x00, 0x10, 0x00, 0x10, 0x00, 0x09, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30,
				0x2E, 0x30, 0x2E, 0x31, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x64, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x02, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x11, 0x00, 0x09, 0x00, 0x31,
				0x32, 0x37, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x31, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x12, 0x00, 0x12,
				0x00, 0x09, 0x00, 0x31, 0x32, 0x37, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x31,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00,
				0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
				0x00

			});
				//        {
				//for (int i = 0; i < 13; i++)
				//{
				//	plew.WriteByte(0xFF);
				//}
				//plew.WriteInt(LoginServer.Worlds.Count); // Number of servers
				//foreach (World world in LoginServer.Worlds)
				//{
				//	plew.WriteShort(world.ID); // Server order
				//	plew.WriteInt(world.Channel); // Number of channels

				//	for (int i = 0; i < 8; i++)
				//	{
				//		plew.WriteShort(i + 1);
				//		plew.WriteShort(i + 1);
				//		plew.WriteString(ServerConstants.SERVER_IP);
				//		plew.WriteInt(15101 + i);
				//		plew.WriteInt(i < world.Count ? world[i].LoadProportion : 0); // Number of players
				//		plew.WriteInt(ServerConstants.CHANNEL_LOAD); // Maximum number of channels
				//		plew.WriteInt(1); // Type of seal
				//		plew.WriteInt(0);
				//		plew.WriteByte(i < world.Count ? 1 : 4); // Channel open
				//		plew.WriteInt(15199);
				//	}
				//}

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
			using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
			{
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				c.Send(plew);
			}
		}

		public static void SubPassLoginOK(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
			{
				plew.WriteByte(1);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(1);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				c.Send(plew);
			}
		}
		public static void SubPassLoginWrong(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
			{
				plew.WriteByte(1);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				c.Send(plew);
			}
		}
		public static void SubPassAddOK(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
			{
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(1);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteByte(0);
				c.Send(plew);
			}
		}
		public static void World_Ack(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.WORLD_ACK))
			{

				plew.WriteString("127.0.0.1"); // 219.83.162.27
				plew.WriteString("15010");
				plew.WriteString("127.0.0.1"); // 219.83.162.27
				plew.WriteString("15111");
				c.Send(plew);
			}
		}
	}
}
