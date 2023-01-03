using Server.Common.Constants;
using Server.Common.IO.Packet;
using Server.Common.Net;
using Server.Ghost.Characters;
using Server.Ghost.Provider;
using Server.Net;
using System.Collections.Generic;

namespace Server.Packet
{
	public static class StatusPacket
	{
		public static void getStatusInfo(Client c)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_ALL))
			{
				var chr = c.Character;
				Dictionary<InventoryType.EquipType, int> equip = InventoryPacket.getEquip(chr);
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteString(chr.Name, 20);
				plew.WriteString(chr.Title, 20);
				plew.WriteBytes(chr.Gender);
				plew.WriteBytes(chr.Level);
				plew.WriteBytes(chr.Class);


				plew.WriteHexString("FF FF FF FF FF FF 00 00 00 45 54 89 00 56 00 00 00 56 00 00 00 3A 00 00 00 3A 00 00 00 41 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B0 04 3C 00 03 03 03 00 03 00 03 00 03 00 14 00 11 00 04 00 04 00 0C 00 09 00 04 00 04 00 00 00 10 00 00 00 03 01 05 25 08 00 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 6C 25 00 00 00 00 E8 03 00 00 0A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF");
				c.SendCrypto(plew);
			}
		}

		public static void UpdateHpMp(Client c, int updateHp, short updateMp, short updateFury, short updateMaxFury)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_HPSP))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteShort(updateHp);
				plew.WriteShort(updateMp);
				plew.WriteShort(updateFury);
				plew.WriteShort(updateMaxFury);
				c.Send(plew);
			}
		}

		public static void UpdateExp(Client c)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_LVEXP))
			{
				var chr = c.Character;
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(chr.Level);
				plew.WriteInt(chr.Exp);
				plew.WriteInt(0);
				plew.WriteInt(0);
				plew.WriteInt(0);
				c.Send(plew);
			}
		}

		public static void LevelUp(Client c, Character chr, int level)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_LEVELUP))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(chr.CharacterID);
				plew.WriteInt(level);
				c.Send(plew);
			}
		}

		public static void UpdateFame(Client c, int fame)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_FAME))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(fame);
				c.Send(plew);
			}
		}

		public static void UpdateStat(Client c)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_STATUP_ACK))
			{
				var chr = c.Character;
				Dictionary<InventoryType.EquipType, int> equip = InventoryPacket.getEquip(chr);
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteShort(chr.MaxHp);
				plew.WriteShort(chr.MaxMp);
				plew.WriteShort(chr.Str);
				plew.WriteShort(chr.Dex);
				plew.WriteShort(chr.Vit);
				plew.WriteShort(chr.Int);
				plew.WriteShort(chr.MaxAttack);
				plew.WriteShort(chr.Attack);
				plew.WriteShort(chr.MaxMagic);
				plew.WriteShort(chr.Magic);
				plew.WriteShort(chr.Defense);
				plew.WriteByte(equip.ContainsKey(InventoryType.EquipType.Weapon)
					? ItemFactory.weaponData[equip[InventoryType.EquipType.Weapon]].Speed
					: 0); // 攻擊速度 [Speed]
				plew.WriteByte(equip.ContainsKey(InventoryType.EquipType.Weapon)
					? ItemFactory.weaponData[equip[InventoryType.EquipType.Weapon]].AttackRange
					: 0); // 攻擊距離
				plew.WriteShort(chr.Avoid);
				plew.WriteShort(chr.AbilityBonus);
				plew.WriteShort(chr.SkillBonus);
				plew.WriteShort(chr.UpgradeStr);
				plew.WriteShort(chr.UpgradeDex);
				plew.WriteShort(chr.UpgradeVit);
				plew.WriteShort(chr.UpgradeInt);
				plew.WriteShort(chr.UpgradeAttack);
				plew.WriteShort(chr.UpgradeMagic);
				plew.WriteShort(chr.UpgradeDefense);
				c.Send(plew);
			}
		}

		public static void Hide(Client c, Character chr, int Active)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_HIDE))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteShort(chr.CharacterID);
				plew.WriteShort(Active);
				c.Send(plew);
			}
		}

		public static void Fury(Client c, Character chr, int Type)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.CHAR_USERSP_ACK))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteShort(chr.CharacterID);
				plew.WriteShort(Type);
				plew.WriteInt(0);
				c.Send(plew);
			}
		}


		public static void MarkState(Client c)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.MARK_STATE))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);

				plew.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 71 56");
				c.Send(plew);
			}
		}


		public static void GuihonmanItem(Client c)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.AVATAR_ITEM))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);

				plew.WriteHexString("FF 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0E 41 0A");
				c.Send(plew);
			}
		}

	}
}
