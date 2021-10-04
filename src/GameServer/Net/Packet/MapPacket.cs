using Server.Common.Constants;
using Server.Common.IO.Packet;
using Server.Common.Net;
using Server.Ghost;
using Server.Ghost.Characters;
using Server.Net;
using System.Collections.Generic;

namespace Server.Packet
{
	public static class MapPacket
	{
		public static void enterMapStart(Client c)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.ENTER_MAP_START))
			{
				var chr = c.Character;
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteShort(chr.MapX);
				plew.WriteShort(chr.MapY);
				plew.WriteShort(chr.PlayerX);
				plew.WriteShort(chr.PlayerY);


				c.Send(plew);

			}
		}

		public static void warpToMap(Client c, Character chr, int CharacterID, short MapX, short MapY, short PositionX,
			short PositionY)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.ENTER_WARP_ACK))
			{
				Dictionary<InventoryType.EquipType, int> equip = InventoryPacket.getEquip(chr);

				int WeaponUpgradeAttack = 0;
				if (equip.ContainsKey(InventoryType.EquipType.Weapon))
				{
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level1 * 10;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level2 * 9;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level3 * 8;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level4 * 7;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level5 * 6;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level6 * 5;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level7 * 4;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level8 * 3;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level9 * 2;
					WeaponUpgradeAttack +=
						chr.Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level10 * 1;
				}

				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);

				plew.WriteInt(CharacterID); // 角色編號

				plew.WriteString(chr.Name, 20);
				plew.WriteString(chr.Title, 20);

				plew.WriteShort(MapX);
				plew.WriteShort(MapY);
				plew.WriteShort(PositionX);
				plew.WriteShort(PositionY);
				plew.WriteHexString("01 03 02 FF FF FF FF FF FF 00 00 00 00 00 FF FF D1 18 8A 00 00 00 00 00 00 00 00 00 00 00 00 00 DD A1 8B 00 CB E6 7B 00 00 00 00 00 3C 87 7A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 54 89 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 32 20 30 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 34 33 65 00 00 00 00 00 00 00 00 35 39 37 32 00 01 01 01 01 01 01 01 01 00 00 00 00 00 BC A6 B5 A1 A9 FE B0 3F 1F 40 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 64 A2 56 01 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 91 2A 00 00 00 00 00 00 00 00 01 01 00 00 00 00 00 00 00 00 3F 01 00 00 00 00 00 00 00 00 00 00 00 56 FF FF FF FF 00 00 00 00 00 00 00 00 FF FF FF FF 01 00 00 00 00 03 00 00 FF FF FF FF");
				//plew.WriteByte(chr.Gender);
				//plew.WriteByte(chr.Level);
				//plew.WriteByte(chr.Class);
				//plew.WriteByte(chr.ClassLevel);
				//plew.WriteByte(255);
				//plew.WriteByte(255);
				//plew.WriteByte(2);
				//plew.WriteByte(1);
				//plew.WriteByte(255);
				//plew.WriteByte(0);
				//plew.WriteByte(0);
				//plew.WriteByte(chr.Guild);
				//plew.WriteByte(0); // 光圈
				//plew.WriteByte(chr.IsHiding == false ? 0 : 1);
				//plew.WriteByte(chr.IsFuring == false ? 0 : chr.FuringType);
				//plew.WriteInt(0); // (byte)
				//plew.WriteInt(chr.Hair); // 頭髮
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Face)
				//	? equip[InventoryType.EquipType.Face]
				//	: 0); // Face
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Face2)
				//	? equip[InventoryType.EquipType.Face2]
				//	: 0); // Face 2
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Hat)
				//	? equip[InventoryType.EquipType.Hat]
				//	: 0); // Hat
				//plew.WriteInt(chr.Eyes); // 眼睛
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Outfit)
				//	? equip[InventoryType.EquipType.Outfit]
				//	: 0); // Eye
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Dress)
				//	? equip[InventoryType.EquipType.Dress]
				//	: 0); // Dress
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Weapon)
				//	? equip[InventoryType.EquipType.Weapon]
				//	: 0); // Weapon
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Mantle)
				//	? equip[InventoryType.EquipType.Mantle]
				//	: 0); // Mantle
				//plew.WriteInt(0);
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Pet)
				//	? equip[InventoryType.EquipType.Pet]
				//	: 0); // 靈物
				//plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Toy)
				//	? equip[InventoryType.EquipType.Toy]
				//	: 0); // 玩物
				//		  // 寵物
				//plew.WriteInt(0); //Weapon AC
				//plew.WriteInt(9000005); //Skin
				//plew.WriteString(chr.Pets.Name((byte)InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Pet),
				//	20); // PetName
				//plew.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF");
				//plew.WriteString("", 20); // ToyName
				//plew.WriteHexString("01 00 00 00 00 00 00 00 DC BF F5 05 00 00 00 00 00 01 01 01 01 01 01 01");
				//plew.WriteShort(0); // (byte)
				//plew.WriteShort(WeaponUpgradeAttack); // 武器 Glow ++
				//plew.WriteShort(0);
				//plew.WriteByte(int.Parse(chr.Client.Title.Split('.')[0]));
				//plew.WriteByte(int.Parse(chr.Client.Title.Split('.')[1]));
				//plew.WriteByte(int.Parse(chr.Client.Title.Split('.')[2]));
				//plew.WriteByte(int.Parse(chr.Client.Title.Split('.')[3]));
				//plew.WriteByte(chr.IP.GetAddressBytes()[0]);
				//plew.WriteByte(chr.IP.GetAddressBytes()[1]);
				//plew.WriteByte(chr.IP.GetAddressBytes()[2]);
				//plew.WriteByte(chr.IP.GetAddressBytes()[3]);

				//plew.WriteHexString("1F 40"); // Port
				//plew.WriteShort(0); // (byte)
				//plew.WriteShort(0);
				//plew.WriteHexString("FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 6C EC 00 00 00 00 00 00 00 00 01 01 ED F5 00 00 00 00 00 00 6E EC 00 00 00 00 00 00 00 00 00 00 00 EC FF FF FF FF 00 00 00 00 00 00 00 00 FF FF FF FF 01 00 00 00 00 00 1A 03 FF FF FF FF");


			//	c.SendCrypto(plew);

			}
		}

		public static void removeUser(Client c, int CharacterID)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.LEAVE_WARP_ACK))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(CharacterID); // 玩家ID
				c.Send(plew);
			}
		}

		public static void createUser(Client c, Map Map)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.USER_CREATE))
			{
				var chr = Map.Characters;
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(Map.GetMapCharactersTotal()); // 玩家數量
				for (int i = 0; i < Map.GetMapCharactersTotal(); i++)
				{
					Dictionary<InventoryType.EquipType, int> equip = null;
					try
					{
						equip = InventoryPacket.getEquip(chr[i]);
					}
					catch
					{
						equip = null;
					}

					int WeaponUpgradeAttack = 0;
					if (equip.ContainsKey(InventoryType.EquipType.Weapon))
					{
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level1 *
							10;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level2 *
							9;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level3 *
							8;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level4 *
							7;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level5 *
							6;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level6 *
							5;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level7 *
							4;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level8 *
							3;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level9 *
							2;
						WeaponUpgradeAttack +=
							chr[i].Items[InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Weapon].Level10 *
							1;
					}

					plew.WriteInt(chr[i].CharacterID); // 玩家ID(-1)
					plew.WriteString(chr[i].Name, 20); // 玩家名稱
					plew.WriteString(chr[i].Title, 20); // 玩家稱號
					plew.WriteShort(chr[i].PlayerX); // 玩家 PositionX
					plew.WriteShort(chr[i].PlayerY); // 玩家 PositionY
					plew.WriteByte(chr[i].Gender); // 性別(1)
					plew.WriteByte(chr[i].Level); // 等級
					plew.WriteByte(chr[i].Class); // 職業
					plew.WriteByte(chr[i].ClassLevel);
					plew.WriteByte(chr[i].Guild);
					plew.WriteByte(0); // 光圈
					plew.WriteByte(0); // 隱形
					plew.WriteByte(chr[i].Shop != null ? 1 : 0);
					plew.WriteInt(chr[i].IsFuring == true ? chr[i].FuringType : 0);
					plew.WriteInt(chr[i].Hair); // 頭髮[Hair]
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Face)
						? equip[InventoryType.EquipType.Face]
						: 0); // 臉上[Face]
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Face2)
						? equip[InventoryType.EquipType.Face2]
						: 0); // 臉下[Face2]
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Hat)
						? equip[InventoryType.EquipType.Hat]
						: 0); // 頭部[Hat]
					plew.WriteInt(chr[i].Eyes); // 眼睛[Eyes]
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Outfit)
						? equip[InventoryType.EquipType.Outfit]
						: 0); // 衣服[Outfit]
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Dress)
						? equip[InventoryType.EquipType.Dress]
						: 0); // 服裝[Dress]
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Weapon)
						? equip[InventoryType.EquipType.Weapon]
						: 0); // 武器[Weapon]
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Mantle)
						? equip[InventoryType.EquipType.Mantle]
						: 0); // 披風[Mantle]
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Pet)
						? equip[InventoryType.EquipType.Pet]
						: 0); // 靈物[Pet]
							  //plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.HairAcc) ? equip[InventoryType.EquipType.HairAcc] : 0);
					plew.WriteInt(equip.ContainsKey(InventoryType.EquipType.Toy)
						? equip[InventoryType.EquipType.Toy]
						: 0); // 玩物[Toy]

					// 寵物
					plew.WriteString(
						chr[i].Pets.Name((byte)InventoryType.ItemType.Equip, (byte)InventoryType.EquipType.Pet),
						20); // PetName
					plew.WriteInt(chr[i].Pets.Level((byte)InventoryType.ItemType.Equip,
						(byte)InventoryType.EquipType.Pet));
					plew.WriteInt(chr[i].Pets.Hp((byte)InventoryType.ItemType.Equip,
						(byte)InventoryType.EquipType.Pet));
					plew.WriteInt(chr[i].Pets.Mp((byte)InventoryType.ItemType.Equip,
						(byte)InventoryType.EquipType.Pet));
					plew.WriteInt(chr[i].Pets.Exp((byte)InventoryType.ItemType.Equip,
						(byte)InventoryType.EquipType.Pet));
					plew.WriteInt(0);

					// 玩物
					plew.WriteString("", 20); // ToyName
					plew.WriteInt(0); // ToyLevel
					plew.WriteInt(0); // ToyHP
					plew.WriteInt(0); // ToyMaxMP

					plew.WriteShort(WeaponUpgradeAttack); // 武器 Glow ++
					plew.WriteShort(0);

					// 遠端IP位置
					plew.WriteByte(int.Parse(chr[i].Client.Title.Split('.')[0]));
					plew.WriteByte(int.Parse(chr[i].Client.Title.Split('.')[1]));
					plew.WriteByte(int.Parse(chr[i].Client.Title.Split('.')[2]));
					plew.WriteByte(int.Parse(chr[i].Client.Title.Split('.')[3]));

					// 遠端虛擬IP位置
					plew.WriteByte(chr[i].IP.GetAddressBytes()[0]);
					plew.WriteByte(chr[i].IP.GetAddressBytes()[1]);
					plew.WriteByte(chr[i].IP.GetAddressBytes()[2]);
					plew.WriteByte(chr[i].IP.GetAddressBytes()[3]);

					plew.WriteHexString("1F 40");
					// 個人商店
					plew.WriteString(chr[i].Shop != null ? chr[i].Shop.Name : "", 40); // 個人商店名稱

					plew.WriteShort(0);
					plew.WriteShort(-1);
					plew.WriteShort(0);
					plew.WriteInt(0);
					plew.WriteInt(0);
					plew.WriteInt(-1);

					plew.WriteString("", 20);

					plew.WriteByte(0);
					plew.WriteByte(0); // Like Warp On Player Effect
					plew.WriteByte(0);
					plew.WriteByte(0); // 泡泡效果
					plew.WriteByte(0); // 泡泡效果
					plew.WriteByte(0);
					plew.WriteShort(0);
					plew.WriteShort(chr[i].CharacterID); // 玩家ID [Map Number]
					plew.WriteByte(-1);
					plew.WriteByte(0);
					plew.WriteByte(0);
					plew.WriteByte(0);
					plew.WriteShort(0);
					//Log.Inform("(Other) CharacterID = {0} 遠端IP = {1}.{2}.{3}.{4} , 虛擬IP = {5}.{6}.{7}.{8}", chr[i].CharacterID, int.Parse(chr[i].Client.Title.Split('.')[0]), int.Parse(chr[i].Client.Title.Split('.')[1]), int.Parse(chr[i].Client.Title.Split('.')[2]), int.Parse(chr[i].Client.Title.Split('.')[3]), chr[i].IP.GetAddressBytes()[0], chr[i].IP.GetAddressBytes()[1], chr[i].IP.GetAddressBytes()[2], chr[i].IP.GetAddressBytes()[3]);
				}

				c.Send(plew);
			}
		}

		public static void userDead(Client c)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.PLAYER_DEAD_ACK))
			{
				var chr = c.Character;
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(chr.CharacterID); // 玩家ID
				plew.WriteShort(chr.MapX);
				plew.WriteShort(chr.MapY);
				plew.WriteShort(chr.PlayerX); // 玩家 PositionX
				plew.WriteShort(chr.PlayerY); // 玩家 PositionY

				plew.WriteShort(chr.MapX);
				plew.WriteShort(chr.MapY);
				plew.WriteShort(chr.PlayerX); // 玩家 PositionX
				plew.WriteShort(chr.PlayerY); // 玩家 PositionY

				c.Send(plew);
			}
		}

		public static void warpToMapAuth(Client c, bool isAvailableMap, short mapX, short mapY, short positionX,
			short positionY)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CAN_WARP_ACK))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(isAvailableMap ? 1 : 0);
				plew.WriteShort(mapX);
				plew.WriteShort(mapY);
				plew.WriteShort(positionX);
				plew.WriteShort(positionY);

				plew.WriteShort(mapX);
				plew.WriteShort(mapY);
				plew.WriteShort(positionX);
				plew.WriteShort(positionY);

				c.Send(plew);
			}
		}

		public static void MonsterDrop(Client c, Monster Monster)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.MON_DROP_ITEM))
			{
				var chr = c.Character;
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				for (int i = 0; i < 7; i++)
				{
					plew.WriteInt(i < Monster.Drops.Count ? Monster.Drops[i].ID : 0);
				}

				for (int i = 0; i < 7; i++)
				{
					plew.WriteInt(i < Monster.Drops.Count ? Monster.Drops[i].ItemID : 0);
				}

				for (int i = 0; i < 7; i++)
				{
					plew.WriteShort(i < Monster.Drops.Count ? Monster.Drops[i].PositionX : 0);
				}

				for (int i = 0; i < 7; i++)
				{
					plew.WriteShort(i < Monster.Drops.Count ? Monster.Drops[i].PositionY : 0);
				}

				plew.WriteInt(chr.CharacterID);
				for (int i = 0; i < 7; i++)
				{
					plew.WriteByte(0);
				}

				plew.WriteByte(0);
				for (int i = 0; i < 7; i++)
				{
					plew.WriteInt(i < Monster.Drops.Count ? Monster.Drops[i].Quantity : 0);
				}

				c.Send(plew);
			}
		}
	}
}
