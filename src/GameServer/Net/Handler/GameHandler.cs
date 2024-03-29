using Server.Common.Constants;
using Server.Common.Data;
using Server.Common.IO;
using Server.Common.IO.Packet;
using Server.Ghost;
using Server.Ghost.Accounts;
using Server.Ghost.Characters;
using Server.Ghost.Provider;
using Server.Net;
using Server.Packet;
using System;
using System.Collections.Generic;
using System.Net;

namespace Server.Handler
{
	public static class GameHandler
	{


		public static void Game_Log_Req(InPacket lea, Client gc)
		{

			string[] data = lea.ReadString(50).Split(new[] { (char)0x20 }, StringSplitOptions.None);
			int encryptKey = int.Parse(data[1]);


			string username = data[2];
			string password = data[4];
			int selectCharacter = lea.ReadByte();
			IPAddress hostid = lea.ReadIPAddress();
			gc.SetAccount(new Account(gc));

			try
			{
				gc.Account.Load(username);

				if (!password.Equals(password))
				{
					gc.Dispose();
					Log.Error("Login Fail!");
				}
				else
				{
					gc.Account.Characters = new List<Character>();
					foreach (dynamic datum in new Datums("Characters").PopulateWith("id", "accountId = '{0}' ORDER BY position ASC", gc.Account.ID))
					{
						Character character = new Character(datum.id, gc);
						character.Load(false);
						character.IP = hostid;
						gc.Account.Characters.Add(character);
					}
					gc.SetCharacter(gc.Account.Characters[selectCharacter]);
				}
			}
			catch (NoAccountException)
			{
				gc.Dispose();
				Log.Error("Login Fail!");
			}
			Character chr = gc.Character;
			chr.CharacterID = gc.CharacterID;
			StatusPacket.UpdateHpMp(gc, 0, 0, 0, 0);
			GamePacket.ServerInfoEvent(gc);
			GamePacket.ServerBuffEventStatus(gc);

			GamePacket.Cus2(gc);
			GamePacket.Cus3(gc);
			MapPacket.enterMapStart(gc);
			CashShopPacket.MgameCash(gc);
			CashShopPacket.GuiHonCash(gc);
			GamePacket.Cus31(gc);

			System.Threading.Thread.Sleep(1500);

			GamePacket.Cus4(gc);
			GamePacket.Cus5(gc);
			StatusPacket.getStatusInfo(gc);
		}



		public static void Command_Req(InPacket lea, Client gc)
		{


			string[] cmd = lea.ReadString(60).Split(new[] { (char)0x20 }, StringSplitOptions.None);

			if (gc.Account.Master == 0 || cmd.Length < 1)
				return;
			var chr = gc.Character;
			Character victim = null;

			switch (cmd[0])
			{
				case "//notice":
					if (cmd.Length != 2)
						break;
					foreach (Character all in MapFactory.AllCharacters)
					{
						GamePacket.getNotice(all.Client, 1, cmd[1]);
					}
					break;
				case "//item":
					if (cmd.Length != 2 && cmd.Length != 3)
						break;

					short Quantity = 1;

					if (cmd.Length == 3)
					{
						if (int.Parse(cmd[2]) > 100)
							Quantity = 100;
						else
							Quantity = short.Parse(cmd[2]);
					}

					if (InventoryType.getItemType(int.Parse(cmd[1])) == 1 || InventoryType.getItemType(int.Parse(cmd[1])) == 2)
						Quantity = 1;

					if (InventoryType.getItemType(int.Parse(cmd[1])) == 5)
						return;

					chr.Items.Add(new Item(int.Parse(cmd[1]), InventoryType.getItemType(int.Parse(cmd[1])), chr.Items.GetNextFreeSlot((InventoryType.ItemType)InventoryType.getItemType(int.Parse(cmd[1]))), Quantity));
					InventoryHandler.UpdateInventory(gc, InventoryType.getItemType(int.Parse(cmd[1])));
					break;
				case "//money":
					if (cmd.Length != 2)
						break;
					chr.Money = long.Parse(cmd[1]);
					InventoryPacket.getInvenMoney(gc, chr.Money, long.Parse(cmd[1]));
					break;
				case "//levelup":
					chr.LevelUp();
					break;
				case "//gogo":
					if (cmd.Length != 3)
						break;
					Log.Debug("GoGo Command : Map : {0} Region : {1}", short.Parse(cmd[1]), short.Parse(cmd[2]));
					MapPacket.warpToMapAuth(gc, true, short.Parse(cmd[1]), short.Parse(cmd[2]), -1, -1);
					break;
				case "//hp":
					if (cmd.Length != 2)
						break;

					short Hp = short.Parse(cmd[1]);

					if (Hp > short.MaxValue)
						Hp = short.MaxValue;

					chr.MaxHp = Hp;
					chr.Hp = Hp;
					StatusPacket.getStatusInfo(gc);
					break;
				case "//mp":
					short Mp = short.Parse(cmd[1]);

					if (Mp > short.MaxValue)
						Mp = short.MaxValue;

					chr.MaxMp = Mp;
					chr.Mp = Mp;
					StatusPacket.getStatusInfo(gc);
					break;
				case "//heal":
					chr.Hp = chr.MaxHp;
					chr.Mp = chr.MaxMp;
					chr.Fury = chr.MaxFury;
					StatusPacket.UpdateHpMp(gc, chr.Hp, chr.Mp, chr.Fury, chr.MaxFury);
					break;
				case "//warp":
					if (cmd.Length != 2)
						break;
					foreach (Character find in MapFactory.AllCharacters)
					{
						if (find.Name.Equals(cmd[1]))
						{
							victim = find;
						}
					}
					if (victim != null)
					{
						chr.MapX = victim.MapX;
						chr.MapY = victim.MapY;
						chr.PlayerX = victim.PlayerX;
						chr.PlayerY = victim.PlayerY;
						MapPacket.warpToMapAuth(gc, true, chr.MapX, chr.MapY, chr.PlayerX, chr.PlayerY);
					}
					break;
				case "//ban":
					if (cmd.Length != 2)
						break;
					foreach (Character find in MapFactory.AllCharacters)
					{
						if (find.Name.Equals(cmd[1]))
						{
							victim = find;
						}
					}
					if (victim != null)
					{
						dynamic datum = new Datum("accounts");
						victim.Client.Account.Banned = 7;
						victim.Client.Dispose();
					}
					break;
				case "//save":
					for (int i = 0; i < MapFactory.AllCharacters.Count; i++)
					{
						if (chr.CharacterID == MapFactory.AllCharacters[i].CharacterID)
							continue;
						MapFactory.AllCharacters[i].Client.Dispose();
					}
					//GameServer.IsAlive = false;
					break;
				case "//skillhack":
					break;
				case "//serverinfo":
					GamePacket.NormalNotice(gc, 1, "I: P:15101 U:1 E:1.00 D:1.00 M:1.00 G:1.00");
					break;
				case "//come":

				case "//oxstate":
					break;
				case "//now":
					DateTime now = DateTime.Now;
					string nowtime = string.Format("Server Time Now : [{0}-{1}-{2} {3}:{4}:{5}]", now.Year, now.Month.ToString("00.##"), now.Day.ToString("00.##"), now.Hour.ToString("00.##"), now.Minute.ToString("00.##"), now.Second.ToString("00.##"));
					GamePacket.NormalNotice(gc, 4, nowtime);
					break;
				case "//user":
					break;
				case "//serverdown":
					break;
				case "//test":
					GamePacket.getNotice(gc, 4, "Tes000t");
					break;

				case "//expbuff":
					GamePacket.getNotice(gc, 1, "!@ExpEvent2@!");
					break;
				case "//processlist":
					GamePacket.GmProcessList(gc);
					GamePacket.NormalNotice(gc, 4, "[GM] Process File has saved in game folder.");
					break;
				case "//gameinfo":
					GamePacket.GmGameInfo(gc);
					GamePacket.NormalNotice(gc, 4, "[GM] GAME_INFO has copied to your clipboard."); //[GM] Game Log has copied to your clipboard.
					break;
				case "//maxlevel":
					Log.Inform("COMMAND_REQ NAME: {0}", "name");
					string max1 = string.Format("!@MaxLevel1@!,{0}", chr.Name);
					string max2 = string.Format("!@MaxLevel2@!,{0}", chr.Name);

					GamePacket.getNotice(gc, 3, max1);
					System.Threading.Thread.Sleep(1500);
					GamePacket.getNotice(gc, 3, max2);
					break;
				default:
					break;
			}

			/*
				RcvGuildJoinCheckAck pGjca->result != 0 [%d][%s]
				RcvGuildJoinCheckAck pGuild->GetFaction() != pGjca->faction [%d]
				RcvGuildJoinCheckAck AddJoinWaitPerson() != 0 [%d]
				RcvGuildCreateCheckAck strlen(pGcca->id) <= 0
				RcvGuildCreateCheckAck pSession == NULL�
			    CServerSession::RcvGuildCreateCheckAck(). Result: %d
				CServerSession::RcvGuildCreateCheckAck()
				Munpa creation failed. Result: %d
				RcvGuildJoinCheckAck pGjca->result != 0 [%d][%s]
				RcvGuildJoinCheckAck pGuild->GetFaction() != pGjca->faction [%d]
				RcvGuildJoinCheckAck AddJoinWaitPerson() != 0 [%d]
				RcvGuildCreateCheckAck strlen(pGcca->id) <= 0
				RcvGuildCreateCheckAck pSession == NULL
				CServerSession::RcvGuildCreateCheckAck(). Result: %d0

				CServerSession::RcvGuildCreateCheckAck(). Munpa creation failed. Result: %d
			 */

		}

		public static void Quick_Slot_Req(InPacket lea, Client gc)
		{
			var chr = gc.Character;
			int KeymapType = lea.ReadShort();
			int KeymapSlot = lea.ReadShort();
			int SkillID = lea.ReadInt();
			int SkillType = lea.ReadShort();
			int SkillSlot = lea.ReadShort();

			string QuickSlotName = "";
			switch (KeymapType)
			{
				case 0:
					switch (KeymapSlot)
					{
						case 0:
							QuickSlotName = "Z";
							break;
						case 1:
							QuickSlotName = "X";
							break;
						case 2:
							QuickSlotName = "C";
							break;
						case 3:
							QuickSlotName = "V";
							break;
						case 4:
							QuickSlotName = "B";
							break;
						case 5:
							QuickSlotName = "N";
							break;
					}
					break;
				case 1:
					switch (KeymapSlot)
					{
						case 0:
							QuickSlotName = "1";
							break;
						case 1:
							QuickSlotName = "2";
							break;
						case 2:
							QuickSlotName = "3";
							break;
						case 3:
							QuickSlotName = "4";
							break;
						case 4:
							QuickSlotName = "5";
							break;
						case 5:
							QuickSlotName = "6";
							break;
					}
					break;
				case 2:
					switch (KeymapSlot)
					{
						case 0:
							QuickSlotName = "Insert";
							break;
						case 1:
							QuickSlotName = "Home";
							break;
						case 2:
							QuickSlotName = "PageUp";
							break;
						case 3:
							QuickSlotName = "Delete";
							break;
						case 4:
							QuickSlotName = "End";
							break;
						case 5:
							QuickSlotName = "PageDown";
							break;
					}
					break;
				case 3:
					switch (KeymapSlot)
					{
						case 0:
							QuickSlotName = "7";
							break;
						case 1:
							QuickSlotName = "8";
							break;
						case 2:
							QuickSlotName = "9";
							break;
						case 3:
							QuickSlotName = "0";
							break;
						case 4:
							QuickSlotName = "-";
							break;
						case 5:
							QuickSlotName = "=";
							break;
					}
					break;
			}
			if (SkillID == -1 && SkillType == -1 && SkillSlot == -1)
			{
				chr.Keymap.Remove(QuickSlotName);
				return;
			}
			chr.Keymap.Remove(QuickSlotName);
			chr.Keymap.Add(QuickSlotName, new Shortcut(SkillID, (byte)SkillType, (byte)SkillSlot));
		}

		public static void GameisActive_Ack(InPacket lea, Client gc)
		{
			lea.ReadInt();
			var PlayerClientVersion = lea.ReadInt();
		}

		public static void GameLoadEac_Ack(InPacket lea, Client gc)
		{
			GamePacket.SendEAC(gc);
		}

		public static void ComeBackEvent_Ack(InPacket lea, Client gc)
		{
			GamePacket.ComeBackEvent(gc);
		}

	}
}
