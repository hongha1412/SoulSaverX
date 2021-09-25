using Server.Common.Constants;
using Server.Common.IO.Packet;
using Server.Common.Net;
using Server.Interoperability;

namespace Server.Ghost
{
	public static class LoginPacket
	{
		public static void GameVersionInfoAck(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.PATCH_ACK))
			{
				plew.WriteInt(LoginServer.PatchVer); // Patch Version from patch.dat
				plew.WriteInt(0);
				plew.WriteString(LoginServer.PatchDownloadUrl);
				plew.WriteString("live/");
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
				plew.WriteByte(c.Account.isTwoFactor);
				plew.WriteBytes(new byte[]
					{0x00,0x00,0x00,0x00,0x00,0x00,0x00, 0x00, 0x00, 0x00});
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
						plew.WriteInt(2); // Type of seal
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
			using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
			{
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteShort(0);
				c.Send(plew);
			}
		}

		public static void SubPassLoginOK(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
			{
				plew.WriteShort(256);
				plew.WriteShort(0);
				plew.WriteShort(256);
				plew.WriteShort(0);
				c.Send(plew);
			}
		}
		public static void SubPassLoginWrong(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
			{
				plew.WriteShort(256);
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteShort(0);
				c.Send(plew);
			}
		}
		public static void SubPassAddOK(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.SubPasswordACK))
			{
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteShort(256);
				plew.WriteShort(0);
				c.Send(plew);
			}
		}
		public static void World_Ack(Client c)
		{
			using (var plew = new OutPacket(LoginServerOpcode.WORLD_ACK))
			{

				plew.WriteString("127.0.0.1"); //Character Server
				plew.WriteString("15010");
				plew.WriteString("127.0.0.1"); //Messenger Server
				plew.WriteString("15111");
				c.Send(plew);
			}
		}
	}
}
