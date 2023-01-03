using Server.Accounts;
using Server.Characters;
using Server.Common.Constants;
using Server.Common.Data;
using Server.Common.IO;
using Server.Common.IO.Packet;
using System;
using System.Collections.Generic;

namespace Server.Ghost
{
	public static class CharHandler
	{
		public static void MyChar_Info_Req(InPacket lea, Client gc)
		{
			string[] data = lea.ReadString(lea.Available).Split(new[] { (char)0x20 }, StringSplitOptions.None);
			string username = data[2];
			string password = data[4];

			gc.SetAccount(new Account(gc));
			try
			{
				gc.Account.Load(username);
				int AccountStatus = gc.Account.Banned;
				string AccountPassword = gc.Account.Password;


				if (!password.Equals(AccountPassword))
				{
					gc.Dispose();
				}

				if (AccountStatus > 1)
				{
					gc.Dispose();
				}


				gc.Account.Characters = new List<Character>();
				foreach (dynamic datum in new Datums("Characters").PopulateWith("id",
					"accountId = '{0}' && worldId = '{1}' ORDER BY position ASC", gc.Account.ID, gc.WorldID))
				{


					Character character = new Character((int)datum.id, gc);
					character.Load(false);
					gc.Account.Characters.Add(character);

				}
				CharPacket.MyChar_Info_Ack(gc, gc.Account.Characters);
				Log.Success("Login Success! Username: {0}", username);
			}
			catch (NoAccountException)
			{
				gc.Dispose();
				Log.Error("Login Fail!");
			}
		}

		public static void Create_MyChar_Req(InPacket lea, Client gc)
		{
			string name = lea.ReadString(20);
			int gender = lea.ReadByte();
			int value1 = lea.ReadByte();
			int value2 = lea.ReadByte();
			int value3 = lea.ReadByte();
			int eyes = lea.ReadInt();
			int hair = lea.ReadInt();
			int weapon = lea.ReadInt();
			int outfit = lea.ReadInt();
			int job = lea.ReadByte();
			int seal = 8510011;



			var account_id = gc.Account.ID;
			if (gender != 1 && gender != 2)
			{
				account_id = 0;
				gc.Dispose();
			}

			// Hack Check
			int DefaultCharacterLevel = 1;
			if (job >= 6 && job < 12 && job != 11)
			{
				DefaultCharacterLevel = 60;
			}

			if (job < 1 || job > 11)
			{
				job = 0;
				account_id = 0;
				gc.Dispose();
			}


			//Update  Winter 2021 -> new jobs == 9


			Character chr = new Character();

			chr.AccountID = account_id;
			chr.WorldID = gc.WorldID;
			chr.Name = name;
			chr.Title = "Kanghoin";
			chr.Level = (byte)DefaultCharacterLevel;
			chr.Class = 0;
			chr.ClassLevel = 0xFF;
			chr.Guild = 0xFF;
			chr.Gender = (byte)gender;
			chr.Job = job;
			chr.Eyes = eyes;
			chr.Hair = hair;
			chr.MapX = 1;
			chr.MapY = 96;
			chr.Str = 3;
			chr.Dex = 3;
			chr.Vit = 3;
			chr.Int = 3;
			chr.Hp = 75;
			chr.MaxHp = 75;
			chr.Mp = 25;
			chr.MaxMp = 25;
			chr.Fury = 0;
			chr.MaxFury = 1200;
			chr.Exp = 0;
			chr.Money = 0;
			chr.Attack = 9;
			chr.MaxAttack = 11;
			chr.Magic = 4;
			chr.MaxMagic = 4;
			chr.Defense = 12;
			chr.JumpHeight = 3;
			chr.JumpLow = 3;



			int pos = 1;
			foreach (Character cc in gc.Account.Characters)
			{
				if (cc.Position != pos)
				{
					break;
				}

				pos++;
			}

			chr.Position = (byte)(pos);

			chr.Items.Add(
				new Item(weapon, (byte)(InventoryType.ItemType.Equip), (byte)InventoryType.EquipType.Weapon));
			chr.Items.Add(
				new Item(outfit, (byte)(InventoryType.ItemType.Equip), (byte)InventoryType.EquipType.Outfit));
			chr.Items.Add(new Item(seal, (byte)(InventoryType.ItemType.Equip), (byte)InventoryType.EquipType.Seal));
			chr.Items.Add(new Item(8810011, (byte)InventoryType.ItemType.Spend3, 0, 10));
			chr.Items.Add(new Item(8820011, (byte)InventoryType.ItemType.Spend3, 1, 10));
			chr.Storages.Add(new Storage(0));
			chr.Skills.Add(new Skill(1, 1, 0, 0));
			chr.Skills.Add(new Skill(2, 1, 0, 1));
			chr.Skills.Add(new Skill(3, 1, 0, 2));
			chr.Skills.Add(new Skill(4, 1, 0, 3));
			chr.Keymap.Add("Z", new Shortcut(1, 0, 0));
			chr.Keymap.Add("X", new Shortcut(4, 0, 3));
			chr.Keymap.Add("C", new Shortcut(3, 0, 2));
			chr.UseSlot.Add((byte)InventoryType.ItemType.Spend3, 0xFF);
			chr.UseSlot.Add((byte)InventoryType.ItemType.Pet5, 0xFF);

			if ((gc.Account.Characters.Count + 1) <= 4)
			{

				chr.Save();
				gc.Account.Characters.Insert(pos - 1, chr);
				pos = (chr.Position << 8) + 1;

			}
			else if (Database.Exists("Characters", "name = '{0}'", name))
			{
				pos = -1;
			}
			else if ((gc.Account.Characters.Count + 1) > 4)
			{
				pos = -2;
			}
			else
			{
				pos = 0;
			}

			CharPacket.Create_MyChar_Ack(gc, pos);
		}

		public static void Check_SameName_Req(InPacket lea, Client gc)
		{
			string name = lea.ReadString(20);
			CharPacket.Check_SameName_Ack(gc, (Database.Exists("Characters", "name = '{0}'", name) ? 0 : 1));
		}

		public static void Delete_MyChar_Req(InPacket lea, Client gc)
		{
			int position = lea.ReadInt();

			gc.Account.Characters[position].Delete();
			gc.Account.Characters.Remove(gc.Account.Characters[position]);
			CharPacket.Delete_MyChar_Ack(gc, position + 1);
		}

		public static void Create_Preview_Req(InPacket lea, Client gc)
		{
			int unknown1 = lea.ReadInt();

			CharPacket.Create_Preview_Ack(gc, unknown1);
		}

		public static void Char_page2_preview(InPacket lea, Client gc)
		{
			int unknown1 = lea.ReadInt();
			CharPacket.Char_page2_preview_ack(gc);
		}
	}
}
