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
				plew.WriteByte(chr.Gender);
				plew.WriteByte(chr.Level);
				plew.WriteByte(chr.Class);
				plew.WriteByte(chr.ClassLevel);
				plew.WriteByte(255);
				plew.WriteByte(255);
				plew.WriteByte(2);
				plew.WriteByte(1);
				plew.WriteByte(255);
				plew.WriteByte(chr.Guild);
				plew.WriteByte(0);
				plew.WriteByte(0);
				plew.WriteInt(9000005);

				plew.WriteShort(chr.MaxHp);
				plew.WriteShort(chr.Hp);
				plew.WriteShort(chr.MaxMp);
				plew.WriteInt(chr.Mp);
				plew.WriteLong(GameConstants.getExpNeededForLevel(chr.Level));
				plew.WriteLong(chr.Exp);
				plew.WriteShort(0);//ชื่อเสียง
				plew.WriteShort(chr.Rank);
				plew.WriteShort(chr.
					 MaxFury); // 憤怒值(Max)
				plew.WriteShort(chr.Fury); // 憤怒值
				plew.WriteByte(chr.JumpHeight);
				plew.WriteByte(chr.JumpHeight); // 跳躍高度
				plew.WriteShort(chr.Str); // 力量
				plew.WriteShort(chr.Dex); // 精力
				plew.WriteShort(chr.Vit); // 氣力
				plew.WriteShort(chr.Int); // 智力
				plew.WriteShort(chr.MaxAttack); // 攻擊力(Max)
				plew.WriteShort(chr.Attack); // 攻擊力(Min)
				plew.WriteShort(chr.MaxMagic); // 魔攻力(Max)
				plew.WriteShort(chr.Magic); // 魔攻力(Min)
				plew.WriteShort(chr.Defense); // 防禦力
				plew.WriteByte(0); // 攻擊速度 [Speed]
				plew.WriteByte(0); // 攻擊距離
						  //plew.WriteShort(chr.Avoid); // 迴避率
						  //plew.WriteShort(chr.AbilityBonus); // 能力上升值
						  //plew.WriteShort(chr.SkillBonus); // 技能上升值
						  //plew.WriteShort(chr.UpgradeStr); // 力量+
						  //plew.WriteShort(chr.UpgradeDex); // 敏捷+
						  //plew.WriteShort(chr.UpgradeVit); // 氣力+
						  //plew.WriteShort(chr.UpgradeInt); // 智力+
						  //plew.WriteShort(chr.UpgradeAttack); // 攻擊力+
						  //plew.WriteShort(chr.UpgradeMagic); // 魔攻力+
						  //plew.WriteShort(chr.UpgradeDefense); // 防禦力+
						  //plew.WriteShort(0);
						  //plew.WriteShort(0); // Not read
				plew.WriteHexString("00 00 00 00 81 00 00 00 00 00 00 00 05 E6 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
				plew.WriteShort(1000);
				plew.WriteHexString("00 00 0A 00 00 00 00 00 00 00");
				plew.WriteHexString("00 00 00 00");
				plew.WriteHexString("00 00 00 00");
				plew.WriteHexString("00 00 00 00");
				plew.WriteHexString("FF FF FF FF");

				c.Send(plew);
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
