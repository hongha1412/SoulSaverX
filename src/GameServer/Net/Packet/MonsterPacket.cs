using Server.Common.IO.Packet;
using Server.Common.Net;
using Server.Ghost;
using Server.Net;
using System.Collections.Generic;

namespace Server.Packet
{
	public static class MonsterPacket
	{
		public static void createAllMonster(Client c, Map Map, List<Monster> Monster)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.MON_ALLCREATE))
			{
				int j = 0;
				for (int i = 0; i < 50; i++)
				{
					if (Map.Monster[i] != null)
					{
						j++;
					}
				}

				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(j); // 怪物數量
				for (int i = 0; i < 50; i++)
				{
					plew.WriteByte(Monster[i] != null ? Monster[i].State : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteByte(Monster[i] != null ? Monster[i].Level : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteByte(Monster[i] != null ? Monster[i].AddEffect : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteByte(Monster[i] != null ? Monster[i].Direction : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteByte(Monster[i] != null ? Monster[i].MoveType : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteByte(Monster[i] != null ? Monster[i].AttackType : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteShort(Monster[i] != null ? Monster[i].OriginalID : -1);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteShort(Monster[i] != null ? Monster[i].PositionX : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteShort(Monster[i] != null ? Monster[i].PositionY : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteInt(0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteInt(0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteShort(Monster[i] != null ? Monster[i].Attack1 : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteShort(Monster[i] != null ? Monster[i].Attack2 : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteShort(Monster[i] != null ? Monster[i].CrashAttack : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteShort(Monster[i] != null ? Monster[i].Defense : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteInt(Monster[i] != null ? Monster[i].MonsterID : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteFloat(Monster[i] != null ? Monster[i].Speed : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteShort(0);
				}


				 plew.WriteHexString("B6 3A 25 32 F1 1C B0 14 9D 36 3E 0A 35 1C A7 49 5E 18 FF 02 1B 29 BA 29 34 4D 8A 1E A0 15 B0 11 B1 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

				for (int i = 0; i < 50; i++)
				{
					plew.WriteInt(Monster[i] != null ? Monster[i].MaxHP : 0);
				}

				for (int i = 0; i < 50; i++)
				{
					plew.WriteInt(Monster[i] != null ? Monster[i].HP : 0);
				}
				plew.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
				c.Send(plew);
			}
		}

		public static void spawnMonster(Client c, Monster Monster, int CharacterID, int Damage, short HitX, short HitY)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.MON_SPAWN))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(Monster.State);
				plew.WriteInt(Monster.OriginalID);
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteInt(Monster.Direction);
				plew.WriteFloat(Monster.Speed);
				plew.WriteShort(Monster.PositionX);
				plew.WriteShort(Monster.PositionY);
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteByte(0);
				//plew.WriteHexString("00 00 00");
				//plew.WriteInt(0);
				//plew.WriteInt(CharacterID);
				//plew.WriteShort(Damage);
				//plew.WriteShort(Monster.Effect); // The effect the monster receives (0: None, 1: Unable to move, 2: Poisoned, 3: Dark, 4: Unknown, 5: Frozen)
				//plew.WriteInt(0);
				//plew.WriteInt(Monster.HP);
				//plew.WriteShort(HitX);
				//plew.WriteShort(HitY);
				plew.WriteHexString("00 7F 03 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 CB 1B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07 00 00 00");
				c.Send(plew);
			}
		}

		public static void regenrMonster(Client c, Monster Monster)
		{
			using (OutPacket plew = new OutPacket(ServerOpcode.MON_REGEN))
			{
				plew.WriteInt(0); // length + CRC
				plew.WriteInt(0);
				plew.WriteInt(Monster.MonsterID);
				plew.WriteByte(Monster.Level);
				plew.WriteByte(Monster.AddEffect);
				plew.WriteByte(Monster.Direction);
				plew.WriteByte(Monster.MoveType);
				plew.WriteShort(Monster.PositionX);
				plew.WriteShort(Monster.PositionY);
				plew.WriteShort(0);
				plew.WriteShort(0);
				plew.WriteInt(Monster.HP);
				plew.WriteShort(Monster.OriginalID);
				plew.WriteShort(Monster.Attack1);
				plew.WriteShort(Monster.Attack2);
				plew.WriteShort(Monster.CrashAttack);
				plew.WriteShort(Monster.Defense);
				plew.WriteShort(Monster.AttackType); // Byte
				plew.WriteHexString("25 0F 00 01 00 00");
				c.Send(plew);
			}
		}
	}
}
