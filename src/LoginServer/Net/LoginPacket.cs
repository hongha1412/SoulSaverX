using Server.Common.Constants;
using Server.Common.IO.Packet;
using Server.Common.Net;
using Server.Interoperability;

namespace Server.Ghost
{
	public static class LoginPacket
	{
		public static void SendGameVersionInfo(Client client)
		{
			using (var packet = new OutPacket(LoginServerOpcode.PATCH_ACK))
			{
				packet.WriteInt(LoginServer.PatchVer);
				packet.WriteInt(0);
				packet.WriteString(LoginServer.PatchDownloadUrl);
				packet.WriteString("live/");
				client.Send(packet);
			}
		}



		/* NetCafe
         * 會員於特約網咖連線
         */
		public static void SendLoginResponse(Client client, ServerState.LoginState state, short encryptKey = 0, bool netCafe = false)
		{
			using (var packet = new OutPacket(LoginServerOpcode.LOGIN_ACK))
			{
				packet.WriteByte(state == ServerState.LoginState.LOGIN_SERVER_DEAD ? (byte)30 : (byte)0);
				packet.WriteByte(0);
				packet.WriteByte(client.Account.Master);
				packet.WriteByte(client.Account.isTwoFactor);
				packet.WriteBytes(new byte[10]);
				client.Send(packet);
			}
		}


		public static void SendServerListResponse(Client client)
		{
			using (var packet = new OutPacket(LoginServerOpcode.SERVERLIST_ACK))
			{
				packet.WriteBytes(new byte[13]);
				packet.WriteInt(LoginServer.Worlds.Count); // Number of servers
				foreach (var world in LoginServer.Worlds)
				{
					packet.WriteShort(world.ID); // Server order
					packet.WriteInt(world.Channel); // Number of channels

					for (int i = 0; i < 8; i++)
					{
						packet.WriteShort(i + 1);
						packet.WriteShort(i + 1);
						packet.WriteString(ServerConstants.SERVER_IP);
						packet.WriteInt(15101);
						packet.WriteInt(i < world.Count ? world[i].LoadProportion : 0); // Number of players
						packet.WriteInt(ServerConstants.CHANNEL_LOAD); // Maximum number of channels
						packet.WriteInt(2); // Type of seal
						packet.WriteInt(0);
						packet.WriteByte(1); // Channel open
						packet.WriteInt(15199);
					}
				}

				client.Send(packet);
			}
		}



		public static void Game_Ack(Client c, ServerState.ChannelState state)
		{
			using (var plew = new OutPacket(LoginServerOpcode.GAME_ACK))
			{
				plew.WriteByte((byte)state);
				plew.WriteString(ServerConstants.SERVER_IP);
				plew.WriteInt(15101);
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
