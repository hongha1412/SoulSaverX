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

			//int encryptKey = int.Parse(data[1]);
			string username = data[2];
			string password = data[4];

			gc.SetAccount(new Account(gc));




			try
			{
				Log.Debug("MyChar_Info_Req {0} ", "gc.Account.Load");
				gc.Account.Load(username);
				int AccountStatus = gc.Account.Banned;
				string AccountPassword = gc.Account.Password;


#if DEBUG
				Log.Debug("[LOG] Login Check has been bypass from DEBUG MODE");
#else
	if (!password.Equals(AccountPassword))
				{
					gc.Dispose();
				}

				if(AccountStatus > 1)
				{
					gc.Dispose();
				}
#endif



				gc.Account.Characters = new List<Character>();
				foreach (dynamic datum in new Datums("Characters").PopulateWith("id",
					"accountId = '{0}' && worldId = '{1}' ORDER BY position ASC", gc.Account.ID, gc.WorldID))
				{
					Log.Debug("MyChar_Info_Req -> datum.id {0} -> username {1}", datum.id, gc.Account.Username);

					Character character = new Character((int)datum.id, gc);
					character.Load(false);
					gc.Account.Characters.Add(character);

				}

				CharPacket.MyChar_Info_Ack(gc, gc.Account.Characters);
				Log.Success("Login Success! Username: {0}", username);

				Log.Debug("Password = {0}", password);
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





			/*
            Private
            [05 01 ] [0A 00 38 00] [47 01 01 00] 00 00  Header
            4E 69 6E 6A 61 35 35 35 35  CharacterName
            00 00 00 00 00 00 00 00 00 00 00  Empty
            01 E7 19 00  // Unknow
            DD A1 8B 00 // Eye = 
            D1 18 8A 00 // hair =
            3C 87 7A 00 // weapon 8030012
            01 00 7D 00 // Outfit 8192001
            02 00 00 00 
             * 
             */
			Log.Inform(">> Create Character");
			Log.Debug(">> Character Name: {0} Gender : {1} Value1 : {2} Value2 : {3} Value3 : {4}", name, gender, value1, value2, value3);
			Log.Debug(">> Character ITEM : Eye : {0} Hair: {1} Weapon: {2} Outfit: {3} Job: {4}", eyes, hair, weapon, outfit, job);


			var account_id = gc.Account.ID;
			if (gender != 1 && gender != 2)
			{
				account_id = 0;
				gc.Dispose();
			}

			// Hack Check
			int DefaultCharacterLevel = 1;
			if (job == 6 || job == 7 || job == 8 || job == 9)
			{
				DefaultCharacterLevel = 60;
			}

			if (job < 1 || job > 11)
			{
				job = 0;
				account_id = 0;
				gc.Dispose();
			}
			if (job == 3 && outfit != 8130011 && outfit != 8130012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 3 && weapon != 8060012 && weapon != 8050012)

			{
				weapon = 8060012;
				account_id = 0;
				gc.Dispose();
			}

			if (job == 1 && outfit != 8110011 && outfit != 8110012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 2 && outfit != 8120011 && outfit != 8120012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 4 && outfit != 8140011 && outfit != 8140012)
			{
				account_id = 0;
				gc.Dispose();
			}

			if (job == 4 && outfit != 8140011 && outfit != 8140012)
			{
				account_id = 0;
				gc.Dispose();
			}

			if (job == 5 && outfit != 8160011 && outfit != 8160012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 6 && outfit != 18110601 && outfit != 18110602)
			{
				account_id = 0;
				gc.Dispose();
			}

			if (job == 7 && outfit != 18120601 && outfit != 18120602)
			{
				account_id = 0;
				gc.Dispose();
			}

			if (job == 8 && outfit != 18130601 && outfit != 18130602)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 10 && outfit != 18160601 && outfit != 18160602)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 11 && outfit != 8150011 && outfit != 8150012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 1 && weapon != 8010012 && weapon != 8020012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 2 && weapon != 8030012 && weapon != 8040012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 4 && weapon != 8070012 && weapon != 8080012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 5 && weapon != 7930012 && weapon != 7940012)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 6 && weapon != 18020601 && weapon != 18040601)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 7 && weapon != 18030601 && weapon != 18080601)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 8 && weapon != 18050601 && weapon != 18060601)
			{
				account_id = 0;
				gc.Dispose();
			}

			if (job == 10 && weapon != 17940601 && weapon != 18010601)
			{
				account_id = 0;
				gc.Dispose();
			}
			if (job == 11 && weapon != 7910012)
			{
				account_id = 0;
				gc.Dispose();
			}

			Character chr = new Character();

			chr.AccountID = account_id;
			chr.WorldID = gc.WorldID;
			chr.Name = name;
			chr.Title = "";
			chr.Level = (byte)DefaultCharacterLevel;
			chr.Class = 0;
			chr.ClassLevel = 0xFF;
			chr.Guild = 0xFF;
			chr.Gender = (byte)gender;
			chr.Job = job;
			chr.Eyes = eyes;
			chr.Hair = hair;
			chr.MapX = 0;
			chr.MapY = 0;
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

			Log.Debug("[Create_MyChar_Req] total char = '{0}'", gc.Account.Characters.Count);

			if ((gc.Account.Characters.Count + 1) <= 4)
			{
				Log.Debug("[Create_MyChar_Req] trying to save '{0}' to DB", name);

				chr.Save();
				gc.Account.Characters.Insert(pos - 1, chr);
				pos = (chr.Position << 8) + 1;

				Log.Debug("[Create_MyChar_Req] save status is: '{0}'", pos);
			}
			else if (Database.Exists("Characters", "name = '{0}'", name))
			{
				Log.Debug("[Create_MyChar_Req] user exist: '{0}'", name);
				pos = -1;
			}
			else if ((gc.Account.Characters.Count + 1) > 4)
			{
				Log.Debug("[Create_MyChar_Req] user maxed: {0} - total 4/4: '{0}'", name);
				pos = -2;
			}
			else
			{
				Log.Debug("[Create_MyChar_Req] unknown error during save: '{0}'", name);
				pos = 0;
			}

			Log.Debug("Send Create_MyChar_Ack: '{0}'", name);
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


		//Preview Character 2019-08-25 14:54
		public static void Create_Preview_Req(InPacket lea, Client gc)
		{
			// [Original]: [05 01] [0B 00] [14 00 24 01] [01 00] 00 00 01 00 58 03 01 00 00 00 
			int unknown1 = lea.ReadInt(); // 01 00

			CharPacket.Create_Preview_Ack(gc, unknown1);
		}

		// Preview Page 2 2019-08-29 11:3 [GMT+7]
		public static void Char_page2_preview(InPacket lea, Client gc)
		{
			int unknown1 = lea.ReadInt();
			CharPacket.Char_page2_preview_ack(gc);
		}
	}
}
