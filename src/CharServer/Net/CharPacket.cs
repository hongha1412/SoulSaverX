using MySqlX.XDevAPI.Common;
using Server.Characters;
using Server.Common.Constants;
using Server.Common.IO.Packet;
using Server.Common.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Ghost
{
	public static class CharPacket
	{
		public static void MyCharInfoAck(Client gc, List<Character> chars)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.MYCHAR_INFO_ACK))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(chars.Count);

				for (int i = 0; i < 4; i++)
				{
					var character = chars.FirstOrDefault(chr => chr.Position == (i + 1));
					getCharactersData(plew, character);
				}
				// Send opcode:: 0x9 have fixed packet length of 1464, so it must send with total 1464 bytes
				// Add empty padding for null data with closing header [01 00 00 00 01 00 00 00]
				plew.WriteHexString(
					"00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00");

				gc.Send(plew);
			}
		}

		/*
*         * 0 = name in use
          * 1 = This name can be used
          * 2 = Unable to create a new character, please purchase the character expansion item first, and create up to 4 characters at the same time.
          * else = unknown error
         */
		public static void SendCheckSameNameAck(Client client, int result)
		{
			using (var packet = new OutPacket(ServerOpcode.CHECK_SAMENAME_ACK))
			{
				packet.WriteInt(0); // length + CRC
				packet.WriteInt(0);
				packet.WriteInt(result);

				client.Send(packet);
			}
		}


		/*
          * -2 = Unable to create a new character. Please purchase a character expansion item first, and create up to 4 characters at the same time.
          * -1 = name in use
          * 0 = can't create a role now, please wait
          * 1 = Created successfully
          * 2 = Created successfully
          * 3 = Created successfully
          * 4 = Created successfully
          * else = unknown error
          */
		public static void Create_MyChar_Ack(Client gc, int position)
		{
			// Original : 05 01 0B 00 14 00 24 01 00 00 00 00 01 01 9D FF 01 00 00 00 
			// Error    : 05 01 0B 00 14 00 24 01 00 00 00 00 FF 00 8A 00 00 00 00 00 

			using (OutPacket plew = new OutPacket(ServerOpcode.CREATE_MYCHAR_ACK))
			{
				// [[05 01] [0B 00]] [14 00 24 01] [00 00 00 00] [01 01 9D FF 01 00] [00 00]
				plew.WriteInt(0); // length + CRC [14 00 24 01]
				plew.WriteInt(0); // [00 00 00 00] 

				plew.WriteHexString("01 01 9D FF"); // dont know what

				plew.WriteInt(position); // maybe wrong

				// plew.WriteInt(0); 


				gc.Send(plew);
			}
		}

		/*
          * -2 = unknown error
          * -1 = char deletion failed
          *  1 = char deleted successfully
          *  2 = char deleted successfully
          *  3 = char deleted successfully
          *  4 = char deleted successfully
          *  else = can be deleted after 1 hour of creation
          */
		public static void Delete_MyChar_Ack(Client gc, int position)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.DELETE_MYCHAR_ACK))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(position);

				gc.Send(plew);
			}
		}

		public static void getCharactersData(OutPacket plew, Character chr)
		{
			if (chr != null)
			{
				var CharName = Encoding.Default.GetBytes(chr.Name);
				var Chartitle = Encoding.Default.GetBytes(chr.Title);
			}

			plew.WriteString(chr != null ? chr.Name : "", 20); // name
			plew.WriteString(chr != null ? chr.Title : "", 20); // title                                   
																//  plew.WriteHexString(chr != null ? "01" : "00"); // ?
			plew.WriteHexString("01"); // Empty Slot Enable
			plew.WriteByte(chr != null ? chr.Gender : 0); // Gender
			plew.WriteByte(chr != null ? chr.Level : 0);
			plew.WriteByte(chr != null ? chr.Job : 0);
			plew.WriteByte(chr != null ? chr.Class : 0);
			plew.WriteByte(chr != null ? chr.ClassLevel : 0);
			plew.WriteByte(chr != null ? chr.ClassLevel : 0);

			if (chr != null)
			{
				plew.WriteHexString("FF 00 00 01 00 01 00 01 00 01 00 00 00"); // ?
			}
			else
			{
				plew.WriteHexString("01 00 00 00 00 00 00 00 00 00 00 00 00"); // ?


			}

			Dictionary<InventoryType.EquipType, int> equip = getEquip(chr);
			plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Weapon)
				? equip[InventoryType.EquipType.Weapon]
				: 0);
			plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Outfit)
				? equip[InventoryType.EquipType.Outfit]
				: 0);
			plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Dress) ? equip[InventoryType.EquipType.Dress] : 0);
			plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Face) ? equip[InventoryType.EquipType.Face] : 0);
			plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Face2) ? equip[InventoryType.EquipType.Face2] : 0);
			plew.WriteInt(chr != null ? chr.Eyes : 0);
			plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Hat) ? equip[InventoryType.EquipType.Hat] : 0);
			plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Mantle)
				? equip[InventoryType.EquipType.Mantle]
				: 0);
			plew.WriteInt(chr != null ? chr.Hair : 0);
			plew.WriteInt(chr != null ? 0 : 0); // Hair Accessories
			plew.WriteInt(chr != null ? 0 : 0); // Unknow
			plew.WriteInt(chr != null ? 9000005 : 0); //skin
			plew.WriteByte(chr != null ? chr.Position : 0);
			plew.WriteHexString("00 00 00 01 00 00 00 00 00 00 00");
		}

		public static Dictionary<InventoryType.EquipType, int> getEquip(Character chr)
		{
			Dictionary<InventoryType.EquipType, int> equip = new Dictionary<InventoryType.EquipType, int>();
			if (chr != null)
			{
				foreach (Item im in chr.Items)
				{
					if (im.Type != (byte)InventoryType.ItemType.Equip)
						continue;
					switch (im.Slot)
					{
						case (byte)InventoryType.EquipType.Weapon:
							equip.Add(InventoryType.EquipType.Weapon, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Outfit:
							equip.Add(InventoryType.EquipType.Outfit, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Ring:
							equip.Add(InventoryType.EquipType.Ring, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Necklace:
							equip.Add(InventoryType.EquipType.Necklace, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Mantle:
							equip.Add(InventoryType.EquipType.Mantle, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Seal:
							equip.Add(InventoryType.EquipType.Seal, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Hat:
							equip.Add(InventoryType.EquipType.Hat, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Hair:
							equip.Add(InventoryType.EquipType.Hair, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Eyes:
							equip.Add(InventoryType.EquipType.Eyes, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Face:
							equip.Add(InventoryType.EquipType.Face, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Pet:
							equip.Add(InventoryType.EquipType.Pet, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Dress:
							equip.Add(InventoryType.EquipType.Dress, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Face2:
							equip.Add(InventoryType.EquipType.Face2, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Earing:
							equip.Add(InventoryType.EquipType.Earing, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.HairAcc:
							equip.Add(InventoryType.EquipType.HairAcc, im.ItemID);
							break;
						case (byte)InventoryType.EquipType.Toy:
							equip.Add(InventoryType.EquipType.Toy, im.ItemID);
							break;
					}
				}

				return equip;
			}

			return new Dictionary<InventoryType.EquipType, int>();
		}

		public static void Create_Preview_Ack(Client gc, int position)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CREATE_PREVIEW_ACK))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteHexString("01 00 58 03 01 00 00 00");
				gc.Send(plew);
			}
		}
		public static void Char_page2_preview_ack(Client gc)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_PAGE2_ACK))
			{
				plew.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 ");
				gc.Send(plew);
			}
		}
	}
}
