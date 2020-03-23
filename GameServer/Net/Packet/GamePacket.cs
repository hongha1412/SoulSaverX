using Server.Common.Constants;
using Server.Common.IO.Packet;
using Server.Common.Net;
using Server.Ghost.Characters;
using Server.Net;

namespace Server.Packet
{
    public static class GamePacket
    {
        public static byte[] ConvertToHexString(string value)
        {
            var output = new System.Collections.Generic.List<byte>();
            value = value.Replace(" ", "");

            if (value.Length % 2 != 0)
            {
                throw new System.InvalidOperationException("Hex string size is not even.");
            }

            for (int i = 0; i < value.Length; i += 2)
            {
                output.Add((byte.Parse(value.Substring(i, 2), System.Globalization.NumberStyles.HexNumber)));
            }

            return output.ToArray();
        }

        public static void Game_VersionCheck(Client c, int characterID)
        {
            using (OutPacket plew = new OutPacket())
            {
                plew.WriteHexString("28 00 81 00 29 00 D2 00 FC A0 51 99 08");
                plew.WriteHexString("05 01 14 00");
                plew.WriteHexString("28 00 41 01");
                plew.WriteHexString("00 20 00 00");
                plew.WriteInt(characterID);
                plew.WriteInt(ServerConstants.CLIENT_VERSION);
                plew.WriteInt(ServerConstants.UDP_PORT);
                plew.WriteHexString("83 C6 30 03");
                plew.WriteHexString("B8 16 22 C3");
                plew.WriteLong(c.SessionID);
                c.SendCustom(plew);
            }
        }
        public static void Game_LoginStatus(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                plew.WriteHexString("18 00 81 00"); //CRC
                plew.WriteHexString("10 00 A9 00"); //CRC
                plew.WriteHexString("D8 EC 68 E9 08"); //CRC
                plew.WriteHexString("05 01 51 00"); //Header
                plew.WriteHexString("18 00 6E 01"); //
                plew.WriteHexString("00 E0 04 00"); //
                plew.WriteHexString("00 E0 04 00"); //
                plew.WriteHexString("01 00 00"); //




                c.SendCustom(plew);
            }
        }
        public static void Game_ServerTime(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                int CurrentServerYear = System.DateTime.Now.Year;
                int CurrentServerMonth = System.DateTime.Now.Month;
                int CurrentServerDay = System.DateTime.Now.Day;
                int CurrentServerHour = System.DateTime.Now.Hour;
                int CurrentServerMinute = System.DateTime.Now.Minute;

                plew.WriteHexString("05 01 60 01");
                plew.WriteHexString("24 00 89 02");
                plew.WriteHexString("00 00 00 00");
                plew.WriteInt(CurrentServerYear);
                plew.WriteInt(CurrentServerMonth);
                plew.WriteHexString("00 00 00 00");
                plew.WriteInt(CurrentServerDay);
                plew.WriteInt(CurrentServerHour);
                plew.WriteInt(CurrentServerMinute);

                c.SendCustom(plew);
            }
        }



        public static void getNotice(Client c, byte type, string message)
        {
            using (OutPacket plew = new OutPacket(ServerOpcode.NOTICE))
            {
                plew.WriteInt(0); // length + CRC
                plew.WriteInt(0);
                plew.WriteByte(type);
                plew.WriteString(message, 60);
                plew.WriteHexString("00 00 00 00 00 00 00");

                c.Send(plew);
            }
        }


        public static void Game_login_Ack(Client c)
        {
            // Original: 18 00 81 00 10 00 A9 00 D8 0C C4 DC 08 05 01 51 00 18 00 6E 01 00 E0 04 00 01 00 00 
            using (OutPacket plew = new OutPacket())
            {
                // plew.WriteInt(0); // length + CRC
                // plew.WriteInt(0);
                //plew.WriteHexString("18 00 6E 01 00 E0 04 00 01 00 00");

                c.SendRawLock(ConvertToHexString(
                    "18 00 81 00 10 00 A9 00 D8 AC 73 E3 08 05 01 51 00 18 00 6E 01 00 E0 04 00 01 00 00"));
            }
        }

        public static void Game_FristLoad_ACK(Client c)
        {
            // original: 05 01 5F 01 14 00 78 02 00 00 00 00 01 00 00 00 FF FF FF FF 
            using (OutPacket plew = new OutPacket())
            {

                //  c.SendRawLock(ConvertToHexString("05 01 AF 02 10 00 C4 03 00 00 00 00 FF 00 79 56 05 01 5F 01 14 00 78 02 00 00 00 00 00 00 00 00 FF FF FF FF 74 00 81 00 36 00 2B 01 00 00 00 00 08 05 01 2E 03 74 00 A7 04 00 A0 00 00 65 20 07 00 2D E0 0E 03 00 4E 20 17 00 54 20 03 00 57 20 03 02 8F 02 00 60 03 00 17 20 0B 00 00 E0 00 3D E0 1B 00 01 00 00 68 03 81 00 19 00 02 04 00 00 00 00 08 05 01 2D 03 68 03 9A 07 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 3C 00 01 00 00 3C 01 81 00 80 00 3D 02 00 00 00 00 08 05 01 2C 03 3C 01 6D 05 00 40 00 00 FF E0 05 00 03 2B 03 29 07 40 11 0C 01 FF 38 39 3A FF 0B 3B 00 04 05 2E 06 40 10 1A 2C 0A 0C FF 0E 0F 10 11 12 13 02 3F 40 41 0C FF 0D 09 FF 2A 20 21 22 23 24 25 26 40 1E 40 00 06 36 37 FF FF 33 34 35 40 0A E0 3E 00 00 0B E0 11 47 00 0D E0 05 1A 02 1B FF 1C 60 10 04 1E FF 1F 1A 1D 60 09 E0 1E 00 40 D3 05 09 0C 0B 0A 12 13 E0 19 30 02 00 00 00 05 01 65 02 80 00 EA 03 00 00 00 00 FF 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 71 0A 38 00 81 00 10 00 C9 00 00 00 00 00 08 05 01 70 02 38 00 AD 03 00 E0 24 00 01 79 56 C8 00 81 00 75 00 BE 01 00 00 00 00 08 05 01 50 00 C8 00 1D 02 00 20 00 04 74 73 30 30 31 20 07 E0 03 00 07 4B 61 6E 67 68 6F 69 6E E0 03 13 03 01 01 0B FF 60 00 20 14 02 45 54 89 20 05 02 00 00 3E 20 03 40 00 00 1C 20 04 00 0A 20 03 E0 07 00 0A B0 04 00 00 03 03 03 00 07 00 03 20 01 E0 07 00 00 71 A0 32 01 05 1E 80 19 20 2C E0 04 00 01 9C 56 40 0E 02 E8 03 00 E0 0C 5B 03 FF FF FF FF 05 01 63 02 28 00 90 03 00 00 00 00 01 00 01 00 00 00 00 00 00 00 01 00 00 00 00 00 06 00 00 00 08 07 00 00 00 00 00 00 05 01 47 02 10 00 5C 03 00 00 00 00 02 00 00 00 05 01 47 02 10 00 5C 03 00 00 00 00 02 00 00 00 05 01 44 02 2C 00 75 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 02 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 03 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 04 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 05 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 43 02 94 00 DC 03 00 00 00 00 06 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 42 02 1C 00 63 03 00 00 00 00 04 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 05 01 42 02 1C 00 63 03 00 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 46 02 24 00 6F 03 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 46 02 24 00 6F 03 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 5F 01 14 00 78 02 00 00 00 00 00 00 00 00 FF FF FF FF 05 01 B6 02 14 00 CF 03 00 00 00 00 FF FF FF FF 00 00 FF FF 05 01 8E 03 18 00 AB 04 00 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 D0 00 81 00 29 00 7A 01 00 00 00 00 08 05 01 A9 00 D0 00 7E 02 00 20 00 00 01 20 03 40 00 00 04 40 04 01 00 03 20 01 40 00 02 02 00 FF E0 9E 00 03 00 FF FF FF 05 01 A9 00 D0 00 7E 02 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 FF FF FF 24 00 81 00 10 00 B5 00 00 00 00 00 08 05 01 26 03 24 00 4F 04 00 E0 10 00 01 00 00 05 01 8D 03 B0 00 42 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 A3 03 10 00 B8 04 00 00 00 00 00 00 00 00 C8 00 81 00 79 00 C2 01 00 00 00 00 08 05 01 50 00 C8 00 1D 02 00 20 00 04 74 73 30 30 31 20 07 E0 03 00 07 4B 61 6E 67 68 6F 69 6E E0 03 13 03 01 01 0B FF 60 00 07 00 76 56 45 54 89 00 3E 20 1C 40 03 00 1C 20 07 40 03 00 0A 20 07 E0 07 00 07 B0 04 00 00 03 03 03 00 80 01 04 14 00 11 00 04 20 01 02 0C 00 09 20 05 20 1A 01 00 0F 20 30 02 02 01 05 20 05 E0 11 00 02 E8 03 00 E0 0C 5B 03 FF FF FF FF 05 01 63 02 28 00 90 03 00 00 00 00 01 00 01 00 00 00 00 00 00 00 01 00 00 00 00 00 06 00 00 00 08 07 00 00 00 00 00 00 D8 3E 81 00 5E 01 B7 40 00 00 00 00 08 05 01 64 00 D8 3E 41 40 00 20 00 06 7C B2 78 00 FB 5B 7C 20 09 E0 01 00 02 3B DA 81 60 0C 06 D1 18 8A 00 DD A1 8B 60 0B E0 13 00 02 0B 12 7A E0 0C 1E 02 45 54 89 E0 0C 17 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 07 00 00 FF E0 02 00 E0 07 1B E0 1B 00 E0 02 3E E0 1C 00 E0 1B 53 E0 FF 00 E0 FF 00 E0 FF 00 E0 1B 00 E3 1C 84 E0 92 00 E0 1B E3 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 C3 00 EF 92 FA E0 1C 00 E1 C3 8B E0 FF 00 E0 FF 00 E0 DB 00 02 02 41 87 E0 B4 E6 00 01 E0 B4 BD E0 89 00 E1 57 4F E6 1C 54 E0 92 00 E1 89 B1 E0 B5 00 E1 92 EA E0 1C 00 E1 B5 7D E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 59 00 E7 1C 74 E0 FF 00 E0 4A 00 E1 59 E1 E0 FF 00 E0 FF 00 E0 FF 00 E0 75 00 E3 FF F8 E0 FF 00 E0 FF 00 E0 A3 00 00 64 20 AC E0 53 03 E0 B7 00 E1 53 1B E0 5B 5B E0 FF 00 E0 6F 00 E1 5B E3 E0 53 63 E0 FF 00 E0 70 00 2D AF 40 03 E0 03 00 C0 0F 40 07 40 03 E0 17 00 E0 1B 23 E0 FF 00 E0 FF 00 E0 FF 00 E0 9B 00 E4 70 9B E0 9C 00 01 00 00 C4 02 81 00 71 00 B6 03 00 00 00 00 08 05 01 73 00 C4 02 3C 04 00 20 00 05 01 00 02 00 03 00 60 0A E0 00 00 00 01 20 00 80 0C 20 08 A0 09 E0 FF 00 E0 1B 00 00 65 20 24 00 66 20 03 00 68 20 03 E0 13 00 21 5D A0 1E 02 FF FF 69 A0 09 E0 17 00 C0 31 E0 4B 00 04 C4 0B 00 00 C5 20 03 00 C6 20 03 00 C7 20 03 00 C8 20 03 00 C9 20 03 E0 07 00 20 B7 20 00 E0 07 15 E0 63 00 01 00 00 05 01 DB 02 14 00 F4 03 00 00 00 00 00 00 00 00 00 00 00 00 05 01 42 02 1C 00 63 03 00 00 00 00 04 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 CC 08 81 00 2F 00 7C 09 00 00 00 00 08 05 01 79 00 CC 08 4A 0A 00 E0 EA 00 00 30 E0 FF 00 E0 FF 00 E0 CE 00 00 32 E0 CE D7 E0 1E 00 E3 FF E7 E0 FF 00 E0 FF 00 E0 C6 00 01 30 00 D0 00 81 00 29 00 7A 01 00 00 00 00 08 05 01 A9 00 D0 00 7E 02 00 20 00 00 01 20 03 40 00 00 04 40 04 01 00 03 20 01 40 00 02 02 00 FF E0 9E 00 03 00 A7 81 56 D4 1C 81 00 A0 00 F5 1D 00 00 00 00 08 05 01 AA 00 D4 1C 83 1E 00 E0 86 00 00 64 E0 86 8F E0 1F 00 E0 FF B7 E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F 20 00 00 30 80 03 00 30 40 07 E0 35 00 02 FF FF FF E0 0F 40 E1 27 6F 03 FF FF FF FF D4 1C 81 00 A4 00 F9 1D 00 00 00 00 08 05 01 AA 00 D4 1C 83 1E 00 20 00 00 01 20 03 E0 7F 00 00 64 E0 7F 88 E0 26 00 E0 FF B7 E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F 20 00 00 30 80 03 00 30 40 07 E0 35 00 02 FF FF FF E0 0F 40 E1 29 6F 01 00 00 D4 1C 81 00 A4 00 F9 1D 00 00 00 00 08 05 01 AA 00 D4 1C 83 1E 00 20 00 00 02 20 03 E0 7F 00 00 64 E0 7F 88 E0 26 00 E0 FF B7 E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F 20 00 00 30 80 03 00 30 40 07 E0 35 00 02 FF FF FF E0 0F 40 E1 29 6F 01 00 00 D4 1C 81 00 A4 00 F9 1D 00 00 00 00 08 05 01 AA 00 D4 1C 83 1E 00 20 00 00 03 20 03 E0 7F 00 00 64 E0 7F 88 E0 26 00 E0 FF B7 E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F E0 5F 00 E1 FF 6F 20 00 00 30 80 03 00 30 40 07 E0 35 00 02 FF FF FF E0 0F 40 E1 29 6F 01 00 00 05 01 AB 00 14 00 C4 01 00 00 00 00 00 00 00 00 00 00 00 00 14 00 81 00 15 00 AA 00 00 00 00 00 08 05 01 1C 00 14 00 35 01 00 20 00 07 01 00 60 00 7E 08 FD 02 C0 13 81 00 50 00 91 14 00 00 00 00 08 05 01 E6 00 C0 13 AB 15 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 76 00 00 64 20 7F E0 FF 03 E1 2B 07 E0 FF 00 E0 FF 00 E0 65 00 01 00 00 14 00 81 00 15 00 AA 00 00 00 00 00 08 05 01 81 02 14 00 9A 03 00 20 00 00 F6 20 03 03 00 00 81 56 1C 00 81 00 19 00 B6 00 00 00 00 00 08 05 01 E7 02 1C 00 08 04 00 20 00 00 03 20 03 00 01 20 03 80 00 01 00 00 05 01 F2 00 10 00 07 02 00 00 00 00 78 00 00 00 8C 07 81 00 49 00 56 08 00 00 00 00 08 05 01 A5 01 8C 07 36 0A 00 E0 22 00 00 FF 20 00 E0 22 2E 60 00 00 64 60 05 E0 3D 00 20 7E E0 FF 7F E0 20 00 E1 FF 7F E0 3B 00 E2 FF 7F E0 23 00 E2 FF 7F E0 3B 00 E2 FF 7F E0 23 00 E2 2A 7F E7 10 13 E1 4D 7F 01 00 00 40 00 81 00 10 00 D1 00 00 00 00 00 08 05 01 1B 01 40 00 60 02 00 E0 2C 00 01 00 00 BC 04 81 00 20 00 5D 05 00 00 00 00 08 05 01 6D 01 BC 04 2E 07 00 20 00 00 40 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 84 00 01 40 40 CC 01 81 00 24 00 71 02 00 00 00 00 08 05 01 89 01 CC 01 5A 04 00 A0 00 00 FF 20 00 40 0A 20 06 E0 0C 00 40 1B E0 FF 00 E0 83 00 03 FF 40 40 40 F4 03 81 00 1D 00 92 04 00 00 00 00 08 05 01 FD 02 F4 03 F6 07 00 20 00 00 30 E0 FF 00 E0 FF 00 E0 FF 00 E0 C4 00 01 30 00 3C 00 81 00 1C 00 D9 00 00 00 00 00 08 05 01 D4 01 3C 00 15 03 00 E0 16 00 00 FF 20 00 40 22 00 64 20 04 80 03 01 00 00 00 04 81 00 31 00 B2 04 00 00 00 00 08 05 01 68 00 00 04 6D 05 00 20 00 02 02 41 87 20 05 E0 B1 00 00 01 E0 B1 BA E0 8C 00 00 FF E0 B6 00 01 03 FF E2 57 11 01 5D 2A E1 B5 22 01 FF FF 3C 03 81 00 21 00 DE 03 00 00 00 00 08 05 01 69 00 3C 03 AA 04 00 E0 FF 00 E0 42 00 00 FF E0 B6 00 E1 42 0A E0 0C 00 E1 B5 1E 01 FF FF 28 00 81 00 1E 00 C7 00 00 00 00 00 08 05 01 F6 02 28 00 23 04 00 20 00 00 11 20 03 00 24 20 03 00 1D 20 03 E0 05 00 01 00 00 18 00 81 00 15 00 AE 00 00 00 00 00 08 05 01 EE 07 18 00 0B 09 00 20 00 00 01 20 03 80 00 01 00 00 14 00 81 00 10 00 A5 00 00 00 00 00 08 05 01 56 02 14 00 6F 03 00 E0 00 00 01 00 00 04 02 81 00 1C 00 A1 02 00 00 00 00 08 05 01 A0 03 04 02 A9 06 00 20 00 00 30 E0 5A 00 20 66 E0 FF 00 E0 7E 00 01 00 00 05 01 60 01 24 00 89 02 00 00 00 00 E4 07 00 00 02 00 00 00 00 6B 20 31 35 20 57 69 6E 6B 57 69 6E 6B 20 35 "));

                //Opcode = AF
                plew.WriteHexString("05 01 AF 02 10 00");
                plew.WriteHexString("C4 03 00 00 00 00");
                plew.WriteHexString("FF 00 79 56");
                c.SendCustom(plew);
            }
        }

        public static void Game_LOAD_2(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 5F
                plew.WriteHexString("05 01 5F 01");
                plew.WriteHexString("14 00 78 02");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("FF FF FF FF");
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_3(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 2E
                /*  05 01 2E 03 74 00 A7 04 00 A0 00 00 
                 *  65 20 07 00 2D E0 0E 03 00 4E 20 17 
                 *  00 54 20 03 00 57 20 03 02 8F 02 00 
                 *  60 03 00 17 20 0B 00 00 E0 00 3D E0 
                 *  1B 00 01 00 00 68 03 81 00 19 00 02 
                 *  04 00 00 00 00 08 
                 */
                plew.WriteHexString("05 01 2E 03 74 00 A7 04 00 A0 00 00");
                plew.WriteHexString("65 20 07 00 2D E0 0E 03 00 4E 20 17");
                plew.WriteHexString("00 54 20 03 00 57 20 03 02 8F 02 00");
                plew.WriteHexString("60 03 00 17 20 0B 00 00 E0 00 3D E0");
                plew.WriteHexString("1B 00 01 00 00 68 03 81 00 19 00 02");
                plew.WriteHexString("04 00 00 00 00 08");
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_4(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 2D
                /* 05 01 2D 03 68 03 9A 07 00 
                 * E0 FF 00 E0 FF 00 E0 FF 00 
                 * E0 3C 00 01 00 00 3C 01 81 
                 * 00 80 00 3D 02 00 00 00 00 
                 * 08 
                 */
                plew.WriteHexString("05 01 2D 03 68 03 9A 07 00");
                plew.WriteHexString("E0 FF 00 E0 FF 00 E0 FF 00");
                plew.WriteHexString("E0 3C 00 01 00 00 3C 01 81");
                plew.WriteHexString("00 80 00 3D 02 00 00 00 00");
                plew.WriteHexString("08");
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_5(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 2c
                /* 05 01 2C 03 3C 01 6D 05 00 40 00 
                 * 00 FF E0 05 00 03 2B 03 29 07 40 
                 * 11 0C 01 FF 38 39 3A FF 0B 3B 00 
                 * 04 05 2E 06 40 10 1A 2C 0A 0C FF 
                 * 0E 0F 10 11 12 13 02 3F 40 41 0C 
                 * FF 0D 09 FF 2A 20 21 22 23 24 25 
                 * 26 40 1E 40 00 06 36 37 FF FF 33 
                 * 34 35 40 0A E0 3E 00 00 0B E0 11 
                 * 47 00 0D E0 05 1A 02 1B FF 1C 60 
                 * 10 04 1E FF 1F 1A 1D 60 09 E0 1E 
                 * 00 40 D3 05 09 0C 0B 0A 12 13 E0 
                 * 19 30 02 00 00 00 
                 */
                plew.WriteHexString("05 01 2C 03 3C 01 6D 05 00 40 00");
                plew.WriteHexString("00 FF E0 05 00 03 2B 03 29 07 40");
                plew.WriteHexString("11 0C 01 FF 38 39 3A FF 0B 3B 00");
                plew.WriteHexString("04 05 2E 06 40 10 1A 2C 0A 0C FF");
                plew.WriteHexString("0E 0F 10 11 12 13 02 3F 40 41 0C");

                plew.WriteHexString("FF 0D 09 FF 2A 20 21 22 23 24 25");
                plew.WriteHexString("26 40 1E 40 00 06 36 37 FF FF 33");
                plew.WriteHexString("34 35 40 0A E0 3E 00 00 0B E0 11");
                plew.WriteHexString("47 00 0D E0 05 1A 02 1B FF 1C 60");
                plew.WriteHexString("10 04 1E FF 1F 1A 1D 60 09 E0 1E");
                plew.WriteHexString("00 40 D3 05 09 0C 0B 0A 12 13 E0");
                plew.WriteHexString("19 30 02 00 00 00");

                c.SendCustom(plew);

            }
        }

        public static void Game_AvartarJarItem(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 65
                /* 05 01 65 02 
                 * 80 00 EA 03 
                 * 00 00 00 00 
                 * FF 01 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 --> To Begin Load Jar
                 * 19 F2 86 00 ---> 8843801
                 * 7D F2 86 00 ----> 8843901
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 4E 71 0A
                 * 
                 * 38 00 81 00 
                 * 10 00 C9 00 
                 * 00 00 00 00 
                 * 08
                 
                 */
                plew.WriteHexString("05 01 65 02");
                plew.WriteHexString("80 00 EA 03");
                //???
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("FF 01 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                // Load Jar
                plew.WriteHexString("19 F2 86 00");
                plew.WriteHexString("7D F2 86 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");

                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");

                plew.WriteHexString("00 4E 71 0A");
                plew.WriteHexString("38 00 81 00");
                plew.WriteHexString("10 00 C9 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("08");
                c.SendCustom(plew);

            }
        }


        public static void Game_LOAD_7(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 70
                /* 05 01 70 02 
                 * 38 00 AD 03 
                 * 00 E0 24 00 
                 * 01 79 56 C8 
                 * 00 81 00 75 
                 * 00 BE 01 00 
                 * 00 00 00 08
                 */
                plew.WriteHexString("05 01 70 02");
                plew.WriteHexString("38 00 AD 03");
                plew.WriteHexString("01 79 56 C8");
                plew.WriteHexString("00 81 00 75");
                plew.WriteHexString("00 BE 01 00");
                plew.WriteHexString("00 00 00 08");
                c.SendCustom(plew);

            }
        }

        public static void Game_FURY(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 70
                /* 05 01 63 02 
                 * 28 00 90 03 
                 * 00 00 00 00 
                 * 01 00 01 00 
                 * 00 00 00 00 
                 * 00 00 01 00 
                 * 00 00 00 00 
                 * 06 00 00 00 
                 * 08 07 00 00 
                 * 00 00 00 00
                 */
                plew.WriteHexString("05 01 63 02");
                plew.WriteHexString("28 00 90 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("01 00 01 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 01 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");

                plew.WriteHexString("08 07 00 00");
                plew.WriteHexString("00 00 00 00");
                c.SendCustom(plew);

            }
        }

        public static void Game_CHARALL(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 50
                /* 
                 */
                plew.WriteHexString("05 01 50 00 C8 00 1D 02 00 20 00 04 74 73 30 30 31 20 07 E0 03 00 07 4B 61 6E 67 68 6F 69 6E E0 03 13 03 01 01 0B FF 60 00 20 14 02 45 54 89 20 05 02 00 00 3E 20 03 40 00 00 1C 20 04 00 0A 20 03 E0 07 00 0A B0 04 00 00 03 03 03 00 07 00 03 20 01 E0 07 00 00 71 A0 32 01 05 1E 80 19 20 2C E0 04 00 01 9C 56 40 0E 02 E8 03 00 E0 0C 5B 03 FF FF FF FF ");
             
                c.SendCustom(plew);

            }
        }





        public static void Game_LOAD_471(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 47
                /* 05 01 47 02 
                 * 10 00 5C 03 
                 * 00 00 00 00 
                 * 02 00 00 00  
                 */
                plew.WriteHexString("05 01 47 02");
                plew.WriteHexString("10 00 5C 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("02 00 00 00");
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_441(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 44
                /* 05 01 44 02 
                 * 2C 00 75 03 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 */
                plew.WriteHexString("05 01 44 02");
                plew.WriteHexString("2C 00 75 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_431(Client c, int i, int j)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 43 - 1
                /* 05 01 43 02 
                 * 94 00 DC 03 
                 * 00 00 00 00 
                 * 01 00 00 00 //i
                 * 00 00 00 00 //j
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 * 00 00 00 00 
                 */
                plew.WriteHexString("05 01 43 02");
                plew.WriteHexString("2C 00 75 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteInt(i);
                plew.WriteHexString("00 00 00");
                plew.WriteInt(j);
                plew.WriteHexString("00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                c.SendCustom(plew);

            }
        }


        public static void Game_LOAD_421(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 42
                /*
                 05 01 42 02 
                 1C 00 63 03
                 00 00 00 00 
                 04 00 00 00 
                 00 00 00 00
                 01 00 00 00 
                 00 00 00 00 
                 */
                plew.WriteHexString("05 01 42 02");
                plew.WriteHexString("1C 00 63 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("04 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("01 00 00 00");
                plew.WriteHexString("00 00 00 00");
                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_420(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 42
                /*
                 05 01 42 02 
                 1C 00 63 03
                 00 00 00 00 
                 04 00 00 00 
                 00 00 00 00
                 01 00 00 00 
                 00 00 00 00 
                 */
                plew.WriteHexString("05 01 42 02");
                plew.WriteHexString("1C 00 63 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("04 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_461(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 46
                /*
                 05 01 46 02 
                 24 00 6F 03 
                 00 00 00 00 
                 01 00 00 00 
                 00 00 00 00 
                 00 00 00 00 
                 00 00 00 00 
                 00 00 00 00 
                 00 00 00 00 
                 */
                plew.WriteHexString("05 01 46 02");
                plew.WriteHexString("24 00 6F 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("01 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_462(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 46
                /*
                 05 01 46 02 
                 24 00 6F 03 
                 00 00 00 00 
                 02 00 00 00 
                 00 00 00 00 
                 00 00 00 00 
                 00 00 00 00 
                 00 00 00 00 
                 00 00 00 00 
                 */
                plew.WriteHexString("05 01 46 02");
                plew.WriteHexString("24 00 6F 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("01 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_5F(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 5F
                /*
                 05 01 5F 01 
                 14 00 78 02 
                 00 00 00 00 
                 00 00 00 00 
                 FF FF FF FF 
                 */
                plew.WriteHexString("05 01 5F 01");
                plew.WriteHexString("14 00 78 02");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("FF FF FF FF");

                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_B6(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = B6
                /*
                 05 01 B6 02 
                 14 00 CF 03 
                 00 00 00 00 
                 FF FF FF FF 
                 00 00 FF FF 
                 */
                plew.WriteHexString("05 01 B6 02");
                plew.WriteHexString("14 00 CF 03");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("FF FF FF FF");
                plew.WriteHexString("00 00 FF FF");


                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_8E(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 8E
                /*
                  05 01 8E 03 
                  18 00 AB 04 
                  00 00 00 00 
                  08 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  D0 00 81 00 
                  29 00 7A 01 
                  00 00 00 00 
                  08 
                 */
                plew.WriteHexString("05 01 8E 03");
                plew.WriteHexString("18 00 AB 04");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("08 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("D0 00 81 00");
                plew.WriteHexString("29 00 7A 01");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("08");



                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_26(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 26
                /*
                  05 01 26 03 
                  24 00 4F 04 
                  00 E0 10 00 
                  01 00 00
                 */
                plew.WriteHexString("05 01 26 03");
                plew.WriteHexString("24 00 4F 04");
                plew.WriteHexString("00 E0 10 00");
                plew.WriteHexString("01 00 00");
                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_8D(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 8D
                /*
                  05 01 8D 03 
                  B0 00 42 05 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00 
                  00 00 00 00

                 */
                plew.WriteHexString("05 01 8D 03");
                plew.WriteHexString("B0 00 42 05");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                plew.WriteHexString("00 00 00 00");
                c.SendCustom(plew);

            }
        }


        public static void Game_LOAD_50(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 50
                /*
                 
                 */
                plew.WriteHexString("05 01 50 00 C8 00 1D 02 00 20 00 04 74 73 30 30 31 20 07 E0 03 00 07 4B 61 6E 67 68 6F 69 6E E0 03 13 03 01 01 0B FF 60 00 07 00 76 56 45 54 89 00 3E 20 1C 40 03 00 1C 20 07 40 03 00 0A 20 07 E0 07 00 07 B0 04 00 00 03 03 03 00 80 01 04 14 00 11 00 04 20 01 02 0C 00 09 20 05 20 1A 01 00 0F 20 30 02 02 01 05 20 05 E0 11 00 02 E8 03 00 E0 0C 5B 03 FF FF FF FF");
           
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_63(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 63
                /*
                 
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 63 02 28 00 90 03 00 00 00 00 01 00 01 00 00 00 00 00 00 00 01 00 00 00 00 00 06 00 00 00 08 07 00 00 00 00 00 00 D8 3E 81 00 5E 01 B7 40 00 00 00 00 08 ");

                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_64(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 64
                /*
                 
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 64 00 D8 3E 41 40 00 20 00 06 7C B2 78 00 FB 5B 7C 20 09 E0 01 00 02 3B DA 81 60 0C 06 D1 18 8A 00 DD A1 8B 60 0B E0 13 00 02 0B 12 7A E0 0C 1E 02 45 54 89 E0 0C 17 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 07 00 00 FF E0 02 00 E0 07 1B E0 1B 00 E0 02 3E E0 1C 00 E0 1B 53 E0 FF 00 E0 FF 00 E0 FF 00 E0 1B 00 E3 1C 84 E0 92 00 E0 1B E3 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 C3 00 EF 92 FA E0 1C 00 E1 C3 8B E0 FF 00 E0 FF 00 E0 DB 00 02 02 41 87 E0 B4 E6 00 01 E0 B4 BD E0 89 00 E1 57 4F E6 1C 54 E0 92 00 E1 89 B1 E0 B5 00 E1 92 EA E0 1C 00 E1 B5 7D E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 59 00 E7 1C 74 E0 FF 00 E0 4A 00 E1 59 E1 E0 FF 00 E0 FF 00 E0 FF 00 E0 75 00 E3 FF F8 E0 FF 00 E0 FF 00 E0 A3 00 00 64 20 AC E0 53 03 E0 B7 00 E1 53 1B E0 5B 5B E0 FF 00 E0 6F 00 E1 5B E3 E0 53 63 E0 FF 00 E0 70 00 2D AF 40 03 E0 03 00 C0 0F 40 07 40 03 E0 17 00 E0 1B 23 E0 FF 00 E0 FF 00 E0 FF 00 E0 9B 00 E4 70 9B E0 9C 00 01 00 00 C4 02 81 00 71 00 B6 03 00 00 00 00 08");

                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_73(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 73
                /*
                 05 01 73 00 
                 C4 02 3C 04 
                 00 20 00
                 */
                plew.WriteHexString("05 01 73 00");
                plew.WriteHexString("C4 02 3C 04 ");
                plew.WriteHexString("00 20 00");

                c.SendCustom(plew);

            }
        }


        public static void Game_LOAD_02(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 02
                /*
               
                 */
                plew.WriteHexString("05 01 00 02 00 03 00 60 0A E0 00 00 00 01 20 00 80 0C 20 08 A0 09 E0 FF 00 E0 1B 00 00 65 20 24 00 66 20 03 00 68 20 03 E0 13 00 21 5D A0 1E 02 FF FF 69 A0 09 E0 17 00 C0 31 E0 4B 00 04 C4 0B 00 00 C5 20 03 00 C6 20 03 00 C7 20 03 00 C8 20 03 00 C9 20 03 E0 07 00 20 B7 20 00 E0 07 15 E0 63 00 01 00 00 ");
        

                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_DB(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = DB
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 DB 02 14 00 F4 03 00 00 00 00 00 00 00 00 00 00 00 00");

                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_42(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 42
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 42 02 1C 00 63 03 00 00 00 00 04 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 CC 08 81 00 2F 00 7C 09 00 00 00 00 08 ");

                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_79(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 79
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 79 00 CC 08 4A 0A 00 E0 EA 00 00 30 E0 FF 00 E0 FF 00 E0 CE 00 00 32 E0 CE D7 E0 1E 00 E3 FF E7 E0 FF 00 E0 FF 00 E0 C6 00 01 30 00 D0 00 81 00 29 00 7A 01 00 00 00 00 08");

                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_AB(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = AB
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 AB 00 14 00 C4 01 00 00 00 00 00 00 00 00 00 00 00 00 14 00 81 00 15 00 AA 00 00 00 00 00 08 ");

                c.SendCustom(plew);

            }
        }
        public static void Game_LOAD_1C(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 1C
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 AB 00 14 00 C4 01 00 00 00 00 00 00 00 00 00 00 00 00 14 00 81 00 15 00 AA 00 00 00 00 00 08");
                c.SendCustom(plew);

            }
        }

        public static void Game_LOAD_81(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 81
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 81 02 14 00 9A 03 00 20 00 00 F6 20 03 03 00 00 81 56 1C 00 81 00 19 00 B6 00 00 00 00 00 08");
                c.SendCustom(plew);
            }
        }


        public static void Game_LOAD_E7(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = E7
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 E7 02 1C 00 08 04 00 20 00 00 03 20 03 00 01 20 03 80 00 01 00 00 ");
                c.SendCustom(plew);
            }
        }

        public static void Game_LOAD_F2(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = F2
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 F2 00 10 00 07 02 00 00 00 00 78 00 00 00 8C 07 81 00 49 00 56 08 00 00 00 00 08");
                c.SendCustom(plew);
            }
        }
        public static void Game_LOAD_A5(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = A5
                /*
               
                 */
                //plew.WriteHexString("");
                plew.WriteHexString("05 01 A5 01 8C 07 36 0A 00 E0 22 00 00 FF 20 00 E0 22 2E 60 00 00 64 60 05 E0 3D 00 20 7E E0 FF 7F E0 20 00 E1 FF 7F E0 3B 00 E2 FF 7F E0 23 00 E2 FF 7F E0 3B 00 E2 FF 7F E0 23 00 E2 2A 7F E7 10 13 E1 4D 7F 01 00 00 40 00 81 00 10 00 D1 00 00 00 00 00 08 ");
                c.SendCustom(plew);
            }
        }
        public static void Game_LOAD_1B(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 1B
                /*
               
                 */
                plew.WriteHexString("05 01 1B 01 40 00 60 02 00 E0 2C 00 01 00 00 BC 04 81 00 20 00 5D 05 00 00 00 00 08");
              c.SendCustom(plew);
            }
        }
        public static void Game_LOAD_6D(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                //Opcode = 1B
                /*
               
                 */
                plew.WriteHexString("05 01 6D 01 BC 04 2E 07 00 20 00 00 40 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 84 00 01 40 40 CC 01 81 00 24 00 71 02 00 00 00 00 08");
                c.SendCustom(plew);
            }
        }


        public static void Character_info_Ack(Client c)
        {
            // original: 
            using (OutPacket plew = new OutPacket())
            {
                c.SendRawLock(ConvertToHexString(
                    "9C 01 81 00 D1 00 EE 02 78 3F E9 EB 08 05 01 1E 00 9C 01 BF 02 00 20 00 00 0B 20 03 06 67 76 64 67 64 63 76 20 09 E0 01 00 05 BD AD BA FE C8 CB E0 01 0F 40 00 0B 01 00 01 00 C8 00 78 05 01 01 0B FF 60 00 40 14 05 00 E9 EB D1 18 8A 60 09 C0 00 06 DD A1 8B 00 FB 5B 7C 60 0E 02 7C B2 78 60 07 E0 07 00 02 45 54 89 E0 07 12 80 00 02 41 E9 EB 80 08 E0 01 00 40 6B E0 01 0D E0 02 00 02 BC 41 08 C0 0D 05 98 BC 41 00 00 01 A0 00 00 BC 40 15 09 B8 16 22 C3 C0 A8 0A FD 1F 40 40 27 E0 07 43 00 FF E0 0D 51 E0 07 00 E0 04 37 00 01 C0 9B 03 72 34 08 01 20 29 E0 07 53 80 00 E0 01 BF 02 01 01 81 A0 9F 00 2E A0 07 60 00 00 EB E0 03 37 40 0B 40 4B 02 00 E2 81 20 6B 01 FF FF 98 13 81 00 46 00 5F 14 00 00 00 00 08 05 01 4F 00 98 13 EC 14 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 F4 00 01 00 00 08 0A 81 00 84 00 0D 0B 00 00 00 00 08 05 01 42 00 08 0A 4F 0B 00 20 00 00 02 20 03 01 00 01 40 04 E0 23 00 00 01 E0 28 31 E0 29 00 01 01 FF E0 27 33 E0 29 95 E0 29 31 41 2C 00 FF E0 56 00 03 8D 0D 93 0E 20 68 E0 54 00 03 E2 04 22 04 E0 54 60 E0 FF 00 E0 82 00 00 01 20 01 E0 57 00 E0 BF 63 E0 BF 00 02 E7 A1 10 60 03 E0 BD 00 01 C0 3F E0 B7 C7 02 21 15 DE E0 58 C3 00 64 21 24 40 03 E0 B7 00 40 C3 40 03 E0 FF 00 E0 45 00 01 00 00 18 00 81 00 19 00 B2 00 18 41 E9 EB 08 05 01 51 00 18 00 6E 01 00 20 00 00 3E 20 03 00 1C 20 03 03 00 00 B0 04 05 01 44 02 2C 00 75 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 42 02 1C 00 63 03 00 00 00 00 04 8E A5 E7 00 00 00 00 01 00 00 00 00 00 00 00 05 01 4E 02 10 00 63 03 00 00 00 00 00 F0 2E 08 C8 00 81 00 81 00 CA 01 68 40 E9 EB 08 05 01 50 00 C8 00 1D 02 00 20 00 06 67 76 64 67 64 63 76 20 09 E0 01 00 05 BD AD BA FE C8 CB E0 01 0F 40 00 03 01 01 0B FF 60 00 07 00 E9 EB 45 54 89 00 3E 20 14 40 03 00 1C 20 07 40 03 00 0A 20 07 E0 07 00 07 B0 04 00 00 03 03 03 00 80 01 04 14 00 11 00 04 20 01 02 0C 00 09 20 05 04 04 00 F1 F6 0F 20 30 02 02 01 05 20 05 E0 0B 00 01 C2 F5 40 15 02 E8 03 00 E0 0C 5B 03 FF FF FF FF 05 01 63 02 28 00 90 03 00 00 00 00 01 00 01 00 00 00 00 00 00 00 01 00 00 00 00 00 06 00 00 00 08 07 00 00 00 E3 C2 F5 05 01 27 07 10 00 3C 08 00 00 00 00 00 00 00 00 05 01 5C 00 C0 00 21 02 00 00 00 00 0B 00 00 00 D1 18 8A 00 00 00 00 00 00 00 00 00 00 00 00 00 DD A1 8B 00 FB 5B 7C 00 00 00 00 00 7C B2 78 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 54 89 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 01 01 01 01 01 01 01 01 74 00 00 00 00 00 08 00 00 00 00 01 00 00 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 3A 01 0C 96 81 00 25 07 B2 9D 68 8A DE EA 08 05 01 20 01 0C 96 31 98 00 20 00 04 F5 88 8C 00 01 20 07 04 1F 03 00 00 FF 20 00 40 07 40 00 40 13 40 00 00 F6 E0 16 1F 00 F7 E0 16 1F 00 FF A0 1F 01 A2 02 80 5F 40 07 40 00 C0 5F 01 00 89 80 7F 40 17 20 7E E0 08 1F 00 01 E0 16 1F 00 02 E0 16 1F 00 EB A0 7F 00 89 A0 7F 40 07 40 00 C0 7F 00 EC E0 16 1F 00 ED E0 16 1F 00 EE E0 16 1F 00 E1 A0 1F 00 F4 20 70 40 DF 40 07 40 00 20 0E 60 00 00 E2 E0 16 1F 00 E3 E0 16 1F 00 E4 E0 16 1F 00 C6 A0 1F 00 DE 20 70 40 7F 40 07 40 00 20 0E 60 00 00 C5 E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 D0 E0 16 1F 00 CF E0 16 1F 00 CE E0 16 1F 00 CD E0 16 1F 00 DA A0 1F 00 F0 20 F0 40 FF 40 07 40 00 20 0E 60 00 00 D9 E0 16 1F 00 D8 E0 16 1F 00 D7 E0 16 1F 00 BB A0 1F 00 AD 20 70 40 7F 40 07 40 00 20 0E 60 00 00 BA E0 16 1F 00 B9 E0 16 1F 01 77 90 83 3F 00 6E 20 4D 02 10 9B 27 60 07 40 00 C0 5F 00 76 E0 16 1F 00 75 E0 16 1F 00 6D E0 16 1F 00 6C E0 16 1F 00 6B E0 16 1F 00 B1 E0 16 DF 00 B0 E0 16 1F 00 AF E0 16 1F 00 A7 A0 1F 00 CC 21 10 41 7F 40 07 40 00 20 0E 60 00 00 A6 E0 16 1F 00 A5 E0 16 1F 00 9D A0 1F 41 D7 40 5F 40 07 40 00 C0 5F 00 9C E0 16 1F 00 9B E0 16 1F 40 00 C0 4B 40 5F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 EB 1F 02 23 B0 8C 61 FF 00 32 22 04 40 FF 40 07 40 00 20 0E 60 00 00 24 E0 16 1F 00 25 E0 16 1F 00 26 E0 16 1F 00 0F A0 1F 00 F9 20 6D 40 7F 40 07 40 00 C0 7F 00 1B A0 1F 00 DD 20 14 40 1F 40 07 40 00 C0 1F 00 1A E0 16 1F 00 19 E0 16 1F 00 05 A0 1F 00 A5 20 54 40 5F 40 07 40 00 C0 5F 01 FB AF 81 1F 40 17 E0 0B 1F 00 FC E0 16 1F 00 FD E0 16 1F 00 FE E0 16 1F 00 F4 E0 16 1F 00 F3 E0 16 1F 00 F2 E0 16 1F 00 F1 E0 16 1F 00 B8 E0 16 1F 00 B7 E0 16 1F 00 B6 E0 16 1F 00 B5 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 C0 E0 16 1F 00 BF E0 16 1F 00 DD E0 16 1F 00 E9 E0 16 1F 00 E8 E0 16 1F 00 E7 E0 16 1F 00 D5 E0 16 1F 00 D4 E0 16 1F 00 D3 E0 16 1F 00 C9 E0 16 1F 00 AD A0 1F 00 7B 23 14 42 FF 40 07 40 00 C3 1F 00 AC E0 16 1F 00 AB E0 16 1F 40 00 C0 4B 40 5F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 F3 1F 02 29 C4 77 60 FF 00 50 21 14 42 1F 40 07 E0 03 00 01 05 7A 80 1F 00 DD 20 14 02 10 9B 27 60 07 40 00 C1 33 C0 1F 00 3D 20 14 02 90 48 09 60 07 40 00 C0 1F 01 FB 79 80 3F 00 32 20 10 02 10 B5 76 60 07 40 00 20 0E 60 00 C0 1F 00 CA 20 0D 02 10 28 4F 60 07 40 00 E0 07 1F 00 6E 20 14 40 7F 40 07 40 00 E0 07 1F 00 1F 20 14 40 7F 40 07 40 00 C0 1F 01 C9 56 80 7F 00 14 20 10 40 DF 40 07 02 A0 BB 0D E0 00 1F 01 D5 54 80 1F 00 37 20 34 40 1F 40 07 E0 03 00 01 46 53 80 1F 00 87 20 14 40 1F 40 07 E0 03 00 00 45 A0 1F 00 4A 20 14 40 1F 40 07 E0 07 00 20 7A 60 00 40 1F 60 08 E0 06 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 EB 1F 02 BF DE 8C 61 FF 00 EA 22 04 40 FF 40 07 40 00 00 01 20 01 40 00 00 BE E0 16 1F 00 BD E0 16 1F 01 CB DA 80 5F 00 82 A0 5F 40 07 40 00 20 0E 60 00 00 CC E0 16 1F 00 CD E0 16 1F 00 CE E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 A2 A1 1F 40 F7 41 5F 40 07 40 00 C0 FF 00 A1 E0 16 1F 00 A0 E0 16 1F 00 9F E0 16 1F 00 DE A0 1F 00 DD 20 74 02 10 9B 27 60 07 40 00 C0 7F 00 DD E0 16 1F 00 DC E0 16 1F 00 DB E0 16 1F 00 E7 A0 1F 00 4B 20 70 40 FF 40 07 40 00 20 0E 60 00 00 E6 E0 16 1F 00 E5 E0 16 1F 00 CA A0 1F 00 A5 20 4D 40 DF 40 07 40 00 C0 5F 00 C9 E0 16 1F 00 C8 E0 16 1F 00 C7 E0 16 1F 00 AC A0 1F 41 D7 40 DF 40 07 40 00 C0 7F 00 AB E0 16 1F 00 AA E0 16 1F 00 A9 E0 16 1F 00 98 A0 1F 41 57 40 7F 40 07 40 00 C0 7F 00 97 E0 16 1F 00 96 E0 16 1F 00 95 E0 16 1F 00 8E A0 1F 00 70 20 70 40 7F 40 07 40 00 20 0E 60 00 00 8D E0 16 1F 00 8C E0 16 1F 00 8B E0 16 1F 00 D8 A3 7F 40 F7 40 7F 40 07 40 00 C0 7F 00 D7 E0 16 1F 00 D6 E0 16 1F 00 D5 E0 16 1F 00 BA A0 1F 41 F7 40 7F 40 07 40 00 C0 7F 00 B9 E0 16 1F 00 B8 E0 16 1F 00 B7 E0 16 1F 00 B0 A0 1F 41 77 40 7F 40 07 40 00 C0 7F 00 AF E0 16 1F 00 AE E0 16 1F 00 A6 A0 1F 00 32 20 50 40 5F 40 07 40 00 20 0E 60 00 00 A5 E0 16 1F 00 A4 E0 16 1F 00 A3 E0 16 1F 00 FC A1 FF 02 2E 02 00 60 7F 40 07 40 00 C0 7F 00 FB E0 16 1F 00 F9 E0 16 1F 40 00 C0 4B 40 DF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 91 1F 01 00 00 0C 96 81 00 E6 03 73 9A 68 8A DE EA 08 05 01 21 01 0C 96 32 98 00 20 00 04 3B C8 77 00 01 20 07 00 32 20 04 00 FF 20 00 40 07 40 00 20 0E 60 00 00 3A E0 16 1F 00 39 E0 16 1F 00 38 E0 16 1F 00 35 E0 16 1F 00 34 E0 16 1F 00 33 E0 16 1F 00 30 E0 16 1F 00 2F E0 16 1F 00 2E E0 16 1F 00 2D E0 16 1F 00 2C E0 16 1F 00 2B E0 16 1F 00 2A E0 16 1F 00 29 E0 16 1F 00 28 E0 16 1F 00 27 E0 16 1F 00 26 E0 16 1F 00 25 E0 16 1F 00 24 E0 16 1F 00 23 E0 16 1F 00 22 E0 16 1F 00 21 E0 16 1F 00 20 E0 16 1F 00 1F E0 16 1F 00 1E E0 16 1F 00 1D E0 16 1F 00 1C E0 16 1F 00 1B E0 16 1F 00 1A E0 16 1F 00 19 E0 16 1F 00 18 E0 16 1F 00 17 E0 16 1F 00 16 E0 16 1F 00 15 E0 16 1F 00 14 E0 16 1F 00 13 E0 16 1F 00 12 E0 16 1F 00 11 E0 16 1F 40 00 C4 CB 24 DE 00 FF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 93 1F 02 E7 8C 8C 60 9F 00 AD 20 A4 41 BF 40 07 40 00 20 0E 60 00 00 DD A0 1F 00 70 20 10 40 1F 40 07 40 00 20 0E 60 00 00 D3 A0 1F 00 32 20 10 40 1F 40 07 40 00 20 0E 60 00 00 C9 A0 1F 00 FB 20 0D 40 1F 40 07 40 00 C0 1F 00 A1 A0 1F 00 DD 20 14 40 1F 40 07 40 00 C0 1F 00 97 A0 1F 00 B8 20 14 40 1F 40 07 40 00 C0 1F 00 8D A0 1F 00 9F 20 14 40 1F 40 07 40 00 C0 1F 00 83 A0 1F 00 7B 20 14 40 1F 40 07 40 00 C0 1F 40 00 C0 0B 40 1F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 D1 1F 01 00 00 0C FA 81 00 2A 0D B7 07 68 8A DE EA 08 05 01 1F 01 0C FA 30 FC 00 20 00 04 54 F3 86 00 01 20 07 04 D4 03 00 00 FF 20 00 40 07 E0 03 00 00 56 A0 1F 00 EF 20 24 20 1E 00 FF 40 07 60 00 20 0F 40 00 01 3B E7 80 3F 00 14 20 0F 40 1F 40 07 40 00 00 01 20 0F 40 00 02 8D 06 87 60 2A 00 C6 20 0C 40 1F 40 07 40 00 A0 1E 01 00 90 20 1F 00 0A 20 10 00 31 20 03 40 1F 40 07 E0 03 00 01 FF F2 80 5F 00 DA 20 14 40 1F 40 07 E0 03 00 00 46 A0 9F 00 87 20 14 40 1F 40 07 E0 03 00 00 94 20 5F 40 6B 00 3A 20 14 40 1F 40 07 E0 03 00 00 95 A0 1F 00 80 20 14 40 1F 40 07 E0 03 00 00 96 A0 1F 00 02 20 44 40 1F 40 07 E0 03 00 02 F4 5D 87 60 BF 01 57 02 81 3F 40 07 E0 03 00 40 1F 20 3A 01 00 45 20 14 40 3F 40 07 E0 03 00 00 F3 20 3F 40 1F 00 63 20 14 40 1F 40 07 E0 03 00 01 65 F6 80 FF 00 BD 20 14 40 1F 40 07 E0 03 00 00 67 A0 1F 00 9F 20 14 40 1F 40 07 E0 03 00 01 5D EF 80 3F 41 77 40 1F 40 07 E0 03 00 00 76 A0 DF 40 17 E0 0B 1F 00 4B A1 5F 00 62 20 34 40 1F 40 07 E0 03 00 00 4F A0 1F 00 37 20 14 40 1F 40 07 E0 03 00 00 50 A0 1F 00 A9 20 14 40 1F 40 07 E0 03 00 00 F0 A1 DF 41 B7 40 1F 40 07 E0 03 00 00 70 20 BF 00 04 20 10 00 64 21 24 40 1F 40 07 E0 03 00 00 8E A0 BF 00 B0 A2 BF 40 07 E0 03 00 00 FA A0 5F 00 98 A0 1F 40 07 E0 03 00 00 66 A0 5F 00 32 A0 5F 40 07 E0 03 00 00 5C A0 1F 42 D7 40 7F 40 07 E0 03 00 00 52 A0 1F 00 F5 20 14 40 1F 40 07 E0 03 00 00 48 A0 1F 00 D6 20 14 40 1F 40 07 E0 03 00 00 3E A0 1F 00 B8 20 14 40 1F 40 07 E0 03 00 00 34 A0 1F 00 99 20 14 40 1F 40 07 E0 03 00 00 2A A0 1F 00 7B 20 14 40 1F 40 07 E0 03 00 00 20 A0 1F 00 5C 20 14 40 1F 40 07 E0 03 00 00 16 A0 1F 00 3D 20 14 40 1F 40 07 E0 03 00 00 38 A3 DF 41 D7 40 1F 40 07 E0 03 00 00 8E 20 3F 43 BF 00 F0 20 14 40 1F 40 07 E0 03 00 40 1F 21 5A 01 00 23 20 14 40 1F 40 07 E0 03 00 00 FD A1 9F 41 57 40 1F 40 07 E0 03 00 00 FC E0 16 1F 00 FE E0 16 1F 00 45 A2 7F 42 57 40 5F 40 07 E0 03 00 01 CF 31 84 9F 41 F7 40 1F 40 07 40 CF 40 B3 40 00 C0 1F 00 A5 20 0C 40 1F 40 07 00 05 20 0B C0 1F 00 70 22 7F 00 1E 20 0F 00 70 20 10 40 1F 40 07 E0 03 00 40 1F 40 4F 40 37 40 1F 40 07 E0 03 00 00 6B A0 3F 00 3F A0 3F 40 07 E0 03 00 40 1F 40 3F 41 D7 40 3F 40 07 E0 03 00 00 69 20 3F 20 3A 01 00 1D A2 DF 40 07 E0 03 00 00 49 A0 FF 41 57 40 3F 40 07 E0 03 00 00 48 A0 1F 00 EA 20 44 40 1F 40 07 E0 03 00 00 F9 A1 5F 00 DF A4 DF 40 07 E0 03 00 00 F8 A0 1F 00 28 A0 1F 40 07 E0 03 00 04 1C EB 86 00 16 20 10 41 17 40 5F 40 07 E0 03 00 40 1F 40 DF 42 F7 40 1F 40 07 E0 03 00 00 1B A0 3F 40 17 E0 0F 1F 40 3F 42 D7 40 1F 40 07 E0 03 00 00 12 A0 3F 00 4B A0 DF 40 07 E0 03 00 40 1F 40 3F 41 97 40 3F 40 07 E0 03 00 00 11 A0 3F 45 57 40 1F 40 07 E0 03 00 40 1F 40 3F 00 50 20 14 40 1F 40 07 E0 03 00 00 37 A3 3F 41 17 40 1F 40 07 60 00 20 8F 40 00 00 36 A0 1F 00 B9 20 0F 40 1F 40 07 60 00 20 0F 40 00 00 35 A0 1F 00 4F 25 04 40 1F 40 07 E0 03 00 00 2E A0 1F 00 97 A1 FF 40 07 E0 07 00 A0 4A 60 3F E0 07 1B 40 00 E0 FF 1F 41 5F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 B3 1F 04 F4 EF 86 00 05 20 D0 00 3D 20 03 41 DF 40 07 E0 03 00 00 EA E0 16 1F 00 D6 20 1F 5E 7F 00 DD 20 34 40 3F 40 07 E0 03 00 40 1F 3E 3A 01 00 6E 20 14 40 1F 40 07 E0 03 00 00 D5 A0 3F 00 7B 20 14 40 1F 40 07 E0 03 00 40 1F 40 3F 40 97 40 1F 40 07 E0 03 00 00 CC A0 3F 40 77 40 1F 40 07 E0 03 00 40 1F 40 3F 40 77 40 1F 40 07 E0 03 00 00 CB A0 3F 40 77 40 1F 40 07 E0 03 00 40 1F 40 3F 40 77 40 1F 40 07 E0 03 00 00 0F 20 3F 41 FF 00 26 20 04 40 1F 40 07 02 E0 E6 0B 60 13 40 00 00 01 A0 1F 00 F5 20 0C 40 1F 40 07 02 20 12 0A E0 00 1F 02 FC EE 86 60 0B 40 17 C0 1F 02 A0 BB 0D 60 13 C0 00 20 5A 60 00 40 1F 60 08 E0 06 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 AB 1F 02 1A 31 89 61 BF 00 C6 21 D4 02 90 48 09 60 07 40 00 C1 D3 00 1F 20 1F 00 05 20 10 00 96 20 03 40 DF 40 07 E0 03 00 40 1F 00 0B 20 10 00 2C 20 30 40 1F 40 07 E0 03 00 02 D7 91 88 60 4B 00 4E 20 14 40 5F 40 07 40 00 20 2E 60 00 00 D8 E0 16 1F 00 D9 E0 16 1F 01 20 92 E0 01 5F 40 7F C0 5F C0 00 02 25 F2 86 60 6B 02 7A 06 00 60 1F 01 CE 07 80 19 00 01 A0 80 00 8A E0 16 1F 01 A2 2D 81 1F 00 62 20 36 40 5F 40 07 E0 03 00 00 A1 A0 1F 00 44 20 14 40 1F 40 07 E0 03 00 00 89 A0 5F 00 77 A0 7F 40 07 40 00 C0 7F 00 26 E0 16 1F 02 7A 06 87 60 BF 00 BD 20 34 41 3F 40 07 40 00 A0 BE 02 00 10 93 80 FF 00 23 20 14 40 1F 40 07 40 00 C0 1F 00 0F E0 16 1F 00 0E E0 16 1F 00 0D E0 16 1F 00 4F A0 1F 00 6E 20 74 02 10 9B 27 60 07 40 00 C0 DF 00 77 A0 BF 00 EA 20 90 41 1F 40 07 E0 03 00 00 88 E0 16 FF 00 24 E0 16 1F 02 E8 BC 84 60 EB 00 B9 20 5F 40 7F 40 07 40 00 20 0E 60 00 00 E7 E0 16 1F 00 79 A0 1F 00 14 20 30 40 3F 40 07 40 00 20 0E 60 00 00 7F A0 BF 40 DF 40 BF 40 07 E0 03 00 00 7D A0 1F 00 56 20 14 40 1F 40 07 E0 03 00 00 7C A0 1F 00 4A 20 14 40 1F 40 07 E0 03 00 00 7B A0 1F 40 57 40 1F 40 07 60 00 A0 80 00 4C A1 5F 00 DD 20 14 40 9F 40 07 40 00 A0 1E 01 00 4A A0 1F 40 F7 02 10 B5 76 60 07 40 00 E0 07 1F 00 A5 20 14 40 3F 40 07 40 00 C0 1F 00 49 A0 3F 40 F7 40 1F 40 07 40 00 E0 07 1F 00 5C 20 14 42 5F 40 07 40 00 C0 1F 00 5B A3 9F 00 7B 20 14 40 3F 40 07 40 00 E0 07 1F 00 31 20 14 40 3F 40 07 40 00 C0 1F 00 5A E0 16 3F C0 1F 40 37 E0 0B 3F 00 59 E0 16 3F C0 1F E0 0F 3F 00 58 A0 3F 41 77 40 BF 40 07 40 00 C0 9F C0 1F 00 2B 20 14 40 7F 40 07 40 00 C0 1F 00 57 E0 16 3F C0 1F 40 37 E0 0B 3F 00 56 E0 16 3F C0 1F E0 0F 3F 00 55 E0 16 3F C0 1F E0 0F 3F 04 B5 A6 87 00 64 20 D0 42 B7 42 7F 40 07 E0 03 00 40 1F 00 28 20 10 40 3F 40 1F 40 07 E0 03 00 01 D3 45 85 1F 42 37 41 3F 40 07 40 00 C3 FF 00 D2 E0 0F 1F 40 24 20 00 C0 1F 00 50 20 0B 41 1F 40 07 40 00 C0 1F 00 D1 E0 16 3F C0 1F 40 37 E0 0B 3F 00 CF E0 16 3F C0 1F E0 0F 3F 00 CE E0 16 3F C0 1F E0 0F 3F 00 6F E0 0F 3F A0 E0 00 6E E0 0F 1F 41 04 20 00 C0 1F E0 0F 5F 00 6D E0 16 3F C0 1F E0 0F 3F 00 6C E0 16 3F C0 1F E0 0F 3F 00 6B E0 16 3F C0 1F E0 0F 3F 00 6A E0 16 3F C0 1F E0 0F 3F 00 69 E0 16 3F C0 1F E0 0F 3F 00 0B E0 0F 3F A1 80 00 0A E0 0F 1F 21 9B 40 00 C0 1F E0 0F 5F 00 09 E0 16 3F C0 1F E0 0F 3F 00 08 E0 16 3F C0 1F E0 0F 3F 00 07 E0 16 3F C0 1F E0 0F 3F 00 06 E0 16 3F C0 1F E0 0F 3F 01 08 B8 88 9F 00 1F 21 2C 02 90 A5 06 60 07 40 00 C1 3F 00 07 A0 1F 44 57 40 1F 40 07 40 00 C0 1F 00 06 A0 1F E0 0F 3F 00 05 E0 16 1F 00 04 E0 16 1F 00 03 E0 16 1F 00 02 E0 16 1F 00 01 A0 1F 40 B7 E0 0B BF 00 C8 A5 9F 40 BF 45 1F 40 07 40 00 C0 DF 00 C7 E0 16 1F 00 C6 E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 C0 E0 16 1F 00 BF E0 16 1F 00 BD E0 16 1F 00 BC E0 16 1F 00 BA E0 16 1F 00 B9 E0 16 1F 40 00 C1 8B 46 DF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 F3 1F 02 57 B2 87 60 FF 00 2E 21 14 42 1F 00 5C 20 07 E0 03 00 00 56 E0 16 1F 00 55 E0 16 1F 00 52 A0 1F 00 34 20 54 40 5F 00 68 20 07 E0 03 00 00 51 E0 16 1F 00 50 E0 16 1F 00 4F A0 1F E0 0F BF 00 4D A0 1F 00 37 20 74 40 7F 00 6E 20 07 E0 03 00 00 4B A0 1F E0 0F 9F 00 4A A0 1F E0 0F 5F 00 49 E0 16 1F 40 00 E2 F3 5F 40 00 E0 FF FF 42 7F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 03 00 02 3F B6 87 62 1F 00 1C 20 14 41 1F 00 37 20 07 E0 03 00 00 3E E0 16 1F 00 3D E0 16 1F 00 3C A0 1F 00 16 20 54 40 5F 00 2B 20 07 E0 03 00 00 3B E0 16 1F 00 3A A0 1F E0 0F 9F 00 39 E0 16 1F 00 38 A0 1F 00 19 20 74 40 7F 00 31 20 07 E0 03 00 00 37 A0 1F E0 0F 9F 00 36 E0 16 1F 00 35 A0 1F E0 0F 9F 00 34 E0 16 1F 00 33 A0 1F E0 0F 9F 00 32 A0 1F E0 0F 9F 00 31 E0 16 1F 40 00 E3 FF FF 41 FF 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 73 1F 02 21 BA 87 60 7F 00 1C 20 94 41 9F 00 37 20 07 E0 03 00 00 20 E0 16 1F 00 1F A0 1F 00 16 20 34 40 3F 00 2B 20 07 E0 03 00 00 1E A0 1F E0 0F 5F 00 1D E0 16 1F 00 1C A0 1F E0 0F 5F 00 1B A0 1F E0 0F 5F 00 1A E0 16 1F 00 19 A0 1F E0 0F 5F 40 00 E1 73 9F 40 00 E0 FF 7F 42 5F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 31 1F 01 00 00 05 01 38 00 60 00 9D 01 00 00 00 00 01 00 00 A0 01 00 00 00 00 00 00 00 01 6C 84 44 00 00 C0 3F 62 0E 22 04 00 00 00 00 00 00 7F 03 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00"));
            }
        }


        public static void Character_Load_Ack(Client c)
        {
            // Original : 05 01 44 02 2C 00 75 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 05 01 42 02 1C 00 63 03 00 00 00 00 06 F8 28 D6 00 00 00 00 01 00 00 00 00 00 00 00 05 01 4E 02 10 00 63 03 00 00 00 00 00 6C 2E 08 C8 00 81 00 A8 00 F1 01 68 10 C4 DC 08 05 01 50 00 C8 00 1D 02 00 20 00 07 8E D7 B7 D6 D6 AE 8E D7 20 0A E0 00 00 07 C9 D9 BB CA B0 CB D0 C7 E0 00 10 20 00 1F 01 B9 05 09 09 09 00 00 FF 01 C4 DC 45 54 89 00 2B 2F 00 00 EE 27 00 00 8D 38 00 00 F2 33 00 00 04 23 4C AD CA 04 20 27 C0 00 1D 50 16 00 00 10 0E 10 0E 03 03 2B 00 DA 04 2C 00 25 00 A8 2C 93 20 B0 01 B0 01 9D 24 6D 1A 40 07 08 EF F6 6B 26 00 00 03 01 0B 60 32 1C 28 00 5F 01 29 00 22 00 28 06 80 01 93 09 8C 05 80 01 25 F5 E7 25 00 00 E8 03 00 00 0A 60 21 E0 05 00 03 2E 00 00 00 05 01 63 02 28 00 90 03 00 00 00 00 4C 00 35 00 1B 00 1B 00 2A 00 2E 00 ED 8F 00 00 8C CE 00 00 20 1C 00 00 00 E9 25 F5 05 01 5C 00 C0 00 21 02 00 00 00 00 02 02 00 00 A3 18 8A 00 11 EA 84 00 CF 98 8F 00 09 65 83 00 E9 A1 8B 00 3B 8A 7C 00 C1 1C 91 00 C7 2E 79 00 61 9A 81 00 0A 89 8C 00 6D 64 6F 00 FC DE 8C 00 84 11 71 00 45 54 89 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0A 00 00 00 31 3D 00 00 AF 45 01 00 99 06 A1 10 00 00 00 00 09 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0A 00 00 C6 A2 A2 0B D1 89 00 00 97 2B EB 06 00 01 01 01 01 01 01 01 01 00 FF 00 50 01 04 00 00 00 00 00 01 00 74 01 00 00 01 01 01 00 00 00 00 00 00 00 00 00 00 00 0C 96 81 00 25 07 B2 9D 68 7A B9 DC 08 05 01 20 01 0C 96 31 98 00 20 00 04 F5 88 8C 00 01 20 07 04 1F 03 00 00 FF 20 00 40 07 40 00 40 13 40 00 00 F6 E0 16 1F 00 F7 E0 16 1F 00 FF A0 1F 01 A2 02 80 5F 40 07 40 00 C0 5F 01 00 89 80 7F 40 17 20 7E E0 08 1F 00 01 E0 16 1F 00 02 E0 16 1F 00 EB A0 7F 00 89 A0 7F 40 07 40 00 C0 7F 00 EC E0 16 1F 00 ED E0 16 1F 00 EE E0 16 1F 00 E1 A0 1F 00 F4 20 70 40 DF 40 07 40 00 20 0E 60 00 00 E2 E0 16 1F 00 E3 E0 16 1F 00 E4 E0 16 1F 00 C6 A0 1F 00 DE 20 70 40 7F 40 07 40 00 20 0E 60 00 00 C5 E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 D0 E0 16 1F 00 CF E0 16 1F 00 CE E0 16 1F 00 CD E0 16 1F 00 DA A0 1F 00 F0 20 F0 40 FF 40 07 40 00 20 0E 60 00 00 D9 E0 16 1F 00 D8 E0 16 1F 00 D7 E0 16 1F 00 BB A0 1F 00 AD 20 70 40 7F 40 07 40 00 20 0E 60 00 00 BA E0 16 1F 00 B9 E0 16 1F 01 77 90 83 3F 00 6E 20 4D 02 10 9B 27 60 07 40 00 C0 5F 00 76 E0 16 1F 00 75 E0 16 1F 00 6D E0 16 1F 00 6C E0 16 1F 00 6B E0 16 1F 00 B1 E0 16 DF 00 B0 E0 16 1F 00 AF E0 16 1F 00 A7 A0 1F 00 CC 21 10 41 7F 40 07 40 00 20 0E 60 00 00 A6 E0 16 1F 00 A5 E0 16 1F 00 9D A0 1F 41 D7 40 5F 40 07 40 00 C0 5F 00 9C E0 16 1F 00 9B E0 16 1F 40 00 C0 4B 40 5F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 EB 1F 02 23 B0 8C 61 FF 00 32 22 04 40 FF 40 07 40 00 20 0E 60 00 00 24 E0 16 1F 00 25 E0 16 1F 00 26 E0 16 1F 00 0F A0 1F 00 F9 20 6D 40 7F 40 07 40 00 C0 7F 00 1B A0 1F 00 DD 20 14 40 1F 40 07 40 00 C0 1F 00 1A E0 16 1F 00 19 E0 16 1F 00 05 A0 1F 00 A5 20 54 40 5F 40 07 40 00 C0 5F 01 FB AF 81 1F 40 17 E0 0B 1F 00 FC E0 16 1F 00 FD E0 16 1F 00 FE E0 16 1F 00 F4 E0 16 1F 00 F3 E0 16 1F 00 F2 E0 16 1F 00 F1 E0 16 1F 00 B8 E0 16 1F 00 B7 E0 16 1F 00 B6 E0 16 1F 00 B5 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 C0 E0 16 1F 00 BF E0 16 1F 00 DD E0 16 1F 00 E9 E0 16 1F 00 E8 E0 16 1F 00 E7 E0 16 1F 00 D5 E0 16 1F 00 D4 E0 16 1F 00 D3 E0 16 1F 00 C9 E0 16 1F 00 AD A0 1F 00 7B 23 14 42 FF 40 07 40 00 C3 1F 00 AC E0 16 1F 00 AB E0 16 1F 40 00 C0 4B 40 5F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 F3 1F 02 29 C4 77 60 FF 00 50 21 14 42 1F 40 07 E0 03 00 01 05 7A 80 1F 00 DD 20 14 02 10 9B 27 60 07 40 00 C1 33 C0 1F 00 3D 20 14 02 90 48 09 60 07 40 00 C0 1F 01 FB 79 80 3F 00 32 20 10 02 10 B5 76 60 07 40 00 20 0E 60 00 C0 1F 00 CA 20 0D 02 10 28 4F 60 07 40 00 E0 07 1F 00 6E 20 14 40 7F 40 07 40 00 E0 07 1F 00 1F 20 14 40 7F 40 07 40 00 C0 1F 01 C9 56 80 7F 00 14 20 10 40 DF 40 07 02 A0 BB 0D E0 00 1F 01 D5 54 80 1F 00 37 20 34 40 1F 40 07 E0 03 00 01 46 53 80 1F 00 87 20 14 40 1F 40 07 E0 03 00 00 45 A0 1F 00 4A 20 14 40 1F 40 07 E0 07 00 20 7A 60 00 40 1F 60 08 E0 06 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 EB 1F 02 BF DE 8C 61 FF 00 EA 22 04 40 FF 40 07 40 00 00 01 20 01 40 00 00 BE E0 16 1F 00 BD E0 16 1F 01 CB DA 80 5F 00 82 A0 5F 40 07 40 00 20 0E 60 00 00 CC E0 16 1F 00 CD E0 16 1F 00 CE E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 A2 A1 1F 40 F7 41 5F 40 07 40 00 C0 FF 00 A1 E0 16 1F 00 A0 E0 16 1F 00 9F E0 16 1F 00 DE A0 1F 00 DD 20 74 02 10 9B 27 60 07 40 00 C0 7F 00 DD E0 16 1F 00 DC E0 16 1F 00 DB E0 16 1F 00 E7 A0 1F 00 4B 20 70 40 FF 40 07 40 00 20 0E 60 00 00 E6 E0 16 1F 00 E5 E0 16 1F 00 CA A0 1F 00 A5 20 4D 40 DF 40 07 40 00 C0 5F 00 C9 E0 16 1F 00 C8 E0 16 1F 00 C7 E0 16 1F 00 AC A0 1F 41 D7 40 DF 40 07 40 00 C0 7F 00 AB E0 16 1F 00 AA E0 16 1F 00 A9 E0 16 1F 00 98 A0 1F 41 57 40 7F 40 07 40 00 C0 7F 00 97 E0 16 1F 00 96 E0 16 1F 00 95 E0 16 1F 00 8E A0 1F 00 70 20 70 40 7F 40 07 40 00 20 0E 60 00 00 8D E0 16 1F 00 8C E0 16 1F 00 8B E0 16 1F 00 D8 A3 7F 40 F7 40 7F 40 07 40 00 C0 7F 00 D7 E0 16 1F 00 D6 E0 16 1F 00 D5 E0 16 1F 00 BA A0 1F 41 F7 40 7F 40 07 40 00 C0 7F 00 B9 E0 16 1F 00 B8 E0 16 1F 00 B7 E0 16 1F 00 B0 A0 1F 41 77 40 7F 40 07 40 00 C0 7F 00 AF E0 16 1F 00 AE E0 16 1F 00 A6 A0 1F 00 32 20 50 40 5F 40 07 40 00 20 0E 60 00 00 A5 E0 16 1F 00 A4 E0 16 1F 00 A3 E0 16 1F 00 FC A1 FF 02 2E 02 00 60 7F 40 07 40 00 C0 7F 00 FB E0 16 1F 00 F9 E0 16 1F 40 00 C0 4B 40 DF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 91 1F 01 00 00 0C 96 81 00 E6 03 73 9A 68 7A B9 DC 08 05 01 21 01 0C 96 32 98 00 20 00 04 3B C8 77 00 01 20 07 00 32 20 04 00 FF 20 00 40 07 40 00 20 0E 60 00 00 3A E0 16 1F 00 39 E0 16 1F 00 38 E0 16 1F 00 35 E0 16 1F 00 34 E0 16 1F 00 33 E0 16 1F 00 30 E0 16 1F 00 2F E0 16 1F 00 2E E0 16 1F 00 2D E0 16 1F 00 2C E0 16 1F 00 2B E0 16 1F 00 2A E0 16 1F 00 29 E0 16 1F 00 28 E0 16 1F 00 27 E0 16 1F 00 26 E0 16 1F 00 25 E0 16 1F 00 24 E0 16 1F 00 23 E0 16 1F 00 22 E0 16 1F 00 21 E0 16 1F 00 20 E0 16 1F 00 1F E0 16 1F 00 1E E0 16 1F 00 1D E0 16 1F 00 1C E0 16 1F 00 1B E0 16 1F 00 1A E0 16 1F 00 19 E0 16 1F 00 18 E0 16 1F 00 17 E0 16 1F 00 16 E0 16 1F 00 15 E0 16 1F 00 14 E0 16 1F 00 13 E0 16 1F 00 12 E0 16 1F 00 11 E0 16 1F 40 00 C4 CB 24 DE 00 FF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 93 1F 02 E7 8C 8C 60 9F 00 AD 20 A4 41 BF 40 07 40 00 20 0E 60 00 00 DD A0 1F 00 70 20 10 40 1F 40 07 40 00 20 0E 60 00 00 D3 A0 1F 00 32 20 10 40 1F 40 07 40 00 20 0E 60 00 00 C9 A0 1F 00 FB 20 0D 40 1F 40 07 40 00 C0 1F 00 A1 A0 1F 00 DD 20 14 40 1F 40 07 40 00 C0 1F 00 97 A0 1F 00 B8 20 14 40 1F 40 07 40 00 C0 1F 00 8D A0 1F 00 9F 20 14 40 1F 40 07 40 00 C0 1F 00 83 A0 1F 00 7B 20 14 40 1F 40 07 40 00 C0 1F 40 00 C0 0B 40 1F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 D1 1F 01 00 00 0C FA 81 00 2A 0D B7 07 68 7A B9 DC 08 05 01 1F 01 0C FA 30 FC 00 20 00 04 54 F3 86 00 01 20 07 04 D4 03 00 00 FF 20 00 40 07 E0 03 00 00 56 A0 1F 00 EF 20 24 20 1E 00 FF 40 07 60 00 20 0F 40 00 01 3B E7 80 3F 00 14 20 0F 40 1F 40 07 40 00 00 01 20 0F 40 00 02 8D 06 87 60 2A 00 C6 20 0C 40 1F 40 07 40 00 A0 1E 01 00 90 20 1F 00 0A 20 10 00 31 20 03 40 1F 40 07 E0 03 00 01 FF F2 80 5F 00 DA 20 14 40 1F 40 07 E0 03 00 00 46 A0 9F 00 87 20 14 40 1F 40 07 E0 03 00 00 94 20 5F 40 6B 00 3A 20 14 40 1F 40 07 E0 03 00 00 95 A0 1F 00 80 20 14 40 1F 40 07 E0 03 00 00 96 A0 1F 00 02 20 44 40 1F 40 07 E0 03 00 02 F4 5D 87 60 BF 01 57 02 81 3F 40 07 E0 03 00 40 1F 20 3A 01 00 45 20 14 40 3F 40 07 E0 03 00 00 F3 20 3F 40 1F 00 63 20 14 40 1F 40 07 E0 03 00 01 65 F6 80 FF 00 BD 20 14 40 1F 40 07 E0 03 00 00 67 A0 1F 00 9F 20 14 40 1F 40 07 E0 03 00 01 5D EF 80 3F 41 77 40 1F 40 07 E0 03 00 00 76 A0 DF 40 17 E0 0B 1F 00 4B A1 5F 00 62 20 34 40 1F 40 07 E0 03 00 00 4F A0 1F 00 37 20 14 40 1F 40 07 E0 03 00 00 50 A0 1F 00 A9 20 14 40 1F 40 07 E0 03 00 00 F0 A1 DF 41 B7 40 1F 40 07 E0 03 00 00 70 20 BF 00 04 20 10 00 64 21 24 40 1F 40 07 E0 03 00 00 8E A0 BF 00 B0 A2 BF 40 07 E0 03 00 00 FA A0 5F 00 98 A0 1F 40 07 E0 03 00 00 66 A0 5F 00 32 A0 5F 40 07 E0 03 00 00 5C A0 1F 42 D7 40 7F 40 07 E0 03 00 00 52 A0 1F 00 F5 20 14 40 1F 40 07 E0 03 00 00 48 A0 1F 00 D6 20 14 40 1F 40 07 E0 03 00 00 3E A0 1F 00 B8 20 14 40 1F 40 07 E0 03 00 00 34 A0 1F 00 99 20 14 40 1F 40 07 E0 03 00 00 2A A0 1F 00 7B 20 14 40 1F 40 07 E0 03 00 00 20 A0 1F 00 5C 20 14 40 1F 40 07 E0 03 00 00 16 A0 1F 00 3D 20 14 40 1F 40 07 E0 03 00 00 38 A3 DF 41 D7 40 1F 40 07 E0 03 00 00 8E 20 3F 43 BF 00 F0 20 14 40 1F 40 07 E0 03 00 40 1F 21 5A 01 00 23 20 14 40 1F 40 07 E0 03 00 00 FD A1 9F 41 57 40 1F 40 07 E0 03 00 00 FC E0 16 1F 00 FE E0 16 1F 00 45 A2 7F 42 57 40 5F 40 07 E0 03 00 01 CF 31 84 9F 41 F7 40 1F 40 07 40 CF 40 B3 40 00 C0 1F 00 A5 20 0C 40 1F 40 07 00 05 20 0B C0 1F 00 70 22 7F 00 1E 20 0F 00 70 20 10 40 1F 40 07 E0 03 00 40 1F 40 4F 40 37 40 1F 40 07 E0 03 00 00 6B A0 3F 00 3F A0 3F 40 07 E0 03 00 40 1F 40 3F 41 D7 40 3F 40 07 E0 03 00 00 69 20 3F 20 3A 01 00 1D A2 DF 40 07 E0 03 00 00 49 A0 FF 41 57 40 3F 40 07 E0 03 00 00 48 A0 1F 00 EA 20 44 40 1F 40 07 E0 03 00 00 F9 A1 5F 00 DF A4 DF 40 07 E0 03 00 00 F8 A0 1F 00 28 A0 1F 40 07 E0 03 00 04 1C EB 86 00 16 20 10 41 17 40 5F 40 07 E0 03 00 40 1F 40 DF 42 F7 40 1F 40 07 E0 03 00 00 1B A0 3F 40 17 E0 0F 1F 40 3F 42 D7 40 1F 40 07 E0 03 00 00 12 A0 3F 00 4B A0 DF 40 07 E0 03 00 40 1F 40 3F 41 97 40 3F 40 07 E0 03 00 00 11 A0 3F 45 57 40 1F 40 07 E0 03 00 40 1F 40 3F 00 50 20 14 40 1F 40 07 E0 03 00 00 37 A3 3F 41 17 40 1F 40 07 60 00 20 8F 40 00 00 36 A0 1F 00 B9 20 0F 40 1F 40 07 60 00 20 0F 40 00 00 35 A0 1F 00 4F 25 04 40 1F 40 07 E0 03 00 00 2E A0 1F 00 97 A1 FF 40 07 E0 07 00 A0 4A 60 3F E0 07 1B 40 00 E0 FF 1F 41 5F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 B3 1F 04 F4 EF 86 00 05 20 D0 00 3D 20 03 41 DF 40 07 E0 03 00 00 EA E0 16 1F 00 D6 20 1F 5E 7F 00 DD 20 34 40 3F 40 07 E0 03 00 40 1F 3E 3A 01 00 6E 20 14 40 1F 40 07 E0 03 00 00 D5 A0 3F 00 7B 20 14 40 1F 40 07 E0 03 00 40 1F 40 3F 40 97 40 1F 40 07 E0 03 00 00 CC A0 3F 40 77 40 1F 40 07 E0 03 00 40 1F 40 3F 40 77 40 1F 40 07 E0 03 00 00 CB A0 3F 40 77 40 1F 40 07 E0 03 00 40 1F 40 3F 40 77 40 1F 40 07 E0 03 00 00 0F 20 3F 41 FF 00 26 20 04 40 1F 40 07 02 E0 E6 0B 60 13 40 00 00 01 A0 1F 00 F5 20 0C 40 1F 40 07 02 20 12 0A E0 00 1F 02 FC EE 86 60 0B 40 17 C0 1F 02 A0 BB 0D 60 13 C0 00 20 5A 60 00 40 1F 60 08 E0 06 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 AB 1F 02 1A 31 89 61 BF 00 C6 21 D4 02 90 48 09 60 07 40 00 C1 D3 00 1F 20 1F 00 05 20 10 00 96 20 03 40 DF 40 07 E0 03 00 40 1F 00 0B 20 10 00 2C 20 30 40 1F 40 07 E0 03 00 02 D7 91 88 60 4B 00 4E 20 14 40 5F 40 07 40 00 20 2E 60 00 00 D8 E0 16 1F 00 D9 E0 16 1F 01 20 92 E0 01 5F 40 7F C0 5F C0 00 02 25 F2 86 60 6B 02 7A 06 00 60 1F 01 CE 07 80 19 00 01 A0 80 00 8A E0 16 1F 01 A2 2D 81 1F 00 62 20 36 40 5F 40 07 E0 03 00 00 A1 A0 1F 00 44 20 14 40 1F 40 07 E0 03 00 00 89 A0 5F 00 77 A0 7F 40 07 40 00 C0 7F 00 26 E0 16 1F 02 7A 06 87 60 BF 00 BD 20 34 41 3F 40 07 40 00 A0 BE 02 00 10 93 80 FF 00 23 20 14 40 1F 40 07 40 00 C0 1F 00 0F E0 16 1F 00 0E E0 16 1F 00 0D E0 16 1F 00 4F A0 1F 00 6E 20 74 02 10 9B 27 60 07 40 00 C0 DF 00 77 A0 BF 00 EA 20 90 41 1F 40 07 E0 03 00 00 88 E0 16 FF 00 24 E0 16 1F 02 E8 BC 84 60 EB 00 B9 20 5F 40 7F 40 07 40 00 20 0E 60 00 00 E7 E0 16 1F 00 79 A0 1F 00 14 20 30 40 3F 40 07 40 00 20 0E 60 00 00 7F A0 BF 40 DF 40 BF 40 07 E0 03 00 00 7D A0 1F 00 56 20 14 40 1F 40 07 E0 03 00 00 7C A0 1F 00 4A 20 14 40 1F 40 07 E0 03 00 00 7B A0 1F 40 57 40 1F 40 07 60 00 A0 80 00 4C A1 5F 00 DD 20 14 40 9F 40 07 40 00 A0 1E 01 00 4A A0 1F 40 F7 02 10 B5 76 60 07 40 00 E0 07 1F 00 A5 20 14 40 3F 40 07 40 00 C0 1F 00 49 A0 3F 40 F7 40 1F 40 07 40 00 E0 07 1F 00 5C 20 14 42 5F 40 07 40 00 C0 1F 00 5B A3 9F 00 7B 20 14 40 3F 40 07 40 00 E0 07 1F 00 31 20 14 40 3F 40 07 40 00 C0 1F 00 5A E0 16 3F C0 1F 40 37 E0 0B 3F 00 59 E0 16 3F C0 1F E0 0F 3F 00 58 A0 3F 41 77 40 BF 40 07 40 00 C0 9F C0 1F 00 2B 20 14 40 7F 40 07 40 00 C0 1F 00 57 E0 16 3F C0 1F 40 37 E0 0B 3F 00 56 E0 16 3F C0 1F E0 0F 3F 00 55 E0 16 3F C0 1F E0 0F 3F 04 B5 A6 87 00 64 20 D0 42 B7 42 7F 40 07 E0 03 00 40 1F 00 28 20 10 40 3F 40 1F 40 07 E0 03 00 01 D3 45 85 1F 42 37 41 3F 40 07 40 00 C3 FF 00 D2 E0 0F 1F 40 24 20 00 C0 1F 00 50 20 0B 41 1F 40 07 40 00 C0 1F 00 D1 E0 16 3F C0 1F 40 37 E0 0B 3F 00 CF E0 16 3F C0 1F E0 0F 3F 00 CE E0 16 3F C0 1F E0 0F 3F 00 6F E0 0F 3F A0 E0 00 6E E0 0F 1F 41 04 20 00 C0 1F E0 0F 5F 00 6D E0 16 3F C0 1F E0 0F 3F 00 6C E0 16 3F C0 1F E0 0F 3F 00 6B E0 16 3F C0 1F E0 0F 3F 00 6A E0 16 3F C0 1F E0 0F 3F 00 69 E0 16 3F C0 1F E0 0F 3F 00 0B E0 0F 3F A1 80 00 0A E0 0F 1F 21 9B 40 00 C0 1F E0 0F 5F 00 09 E0 16 3F C0 1F E0 0F 3F 00 08 E0 16 3F C0 1F E0 0F 3F 00 07 E0 16 3F C0 1F E0 0F 3F 00 06 E0 16 3F C0 1F E0 0F 3F 01 08 B8 88 9F 00 1F 21 2C 02 90 A5 06 60 07 40 00 C1 3F 00 07 A0 1F 44 57 40 1F 40 07 40 00 C0 1F 00 06 A0 1F E0 0F 3F 00 05 E0 16 1F 00 04 E0 16 1F 00 03 E0 16 1F 00 02 E0 16 1F 00 01 A0 1F 40 B7 E0 0B BF 00 C8 A5 9F 40 BF 45 1F 40 07 40 00 C0 DF 00 C7 E0 16 1F 00 C6 E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 C0 E0 16 1F 00 BF E0 16 1F 00 BD E0 16 1F 00 BC E0 16 1F 00 BA E0 16 1F 00 B9 E0 16 1F 40 00 C1 8B 46 DF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 F3 1F 02 57 B2 87 60 FF 00 2E 21 14 42 1F 00 5C 20 07 E0 03 00 00 56 E0 16 1F 00 55 E0 16 1F 00 52 A0 1F 00 34 20 54 40 5F 00 68 20 07 E0 03 00 00 51 E0 16 1F 00 50 E0 16 1F 00 4F A0 1F E0 0F BF 00 4D A0 1F 00 37 20 74 40 7F 00 6E 20 07 E0 03 00 00 4B A0 1F E0 0F 9F 00 4A A0 1F E0 0F 5F 00 49 E0 16 1F 40 00 E2 F3 5F 40 00 E0 FF FF 42 7F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 03 00 02 3F B6 87 62 1F 00 1C 20 14 41 1F 00 37 20 07 E0 03 00 00 3E E0 16 1F 00 3D E0 16 1F 00 3C A0 1F 00 16 20 54 40 5F 00 2B 20 07 E0 03 00 00 3B E0 16 1F 00 3A A0 1F E0 0F 9F 00 39 E0 16 1F 00 38 A0 1F 00 19 20 74 40 7F 00 31 20 07 E0 03 00 00 37 A0 1F E0 0F 9F 00 36 E0 16 1F 00 35 A0 1F E0 0F 9F 00 34 E0 16 1F 00 33 A0 1F E0 0F 9F 00 32 A0 1F E0 0F 9F 00 31 E0 16 1F 40 00 E3 FF FF 41 FF 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 73 1F 02 21 BA 87 60 7F 00 1C 20 94 41 9F 00 37 20 07 E0 03 00 00 20 E0 16 1F 00 1F A0 1F 00 16 20 34 40 3F 00 2B 20 07 E0 03 00 00 1E A0 1F E0 0F 5F 00 1D E0 16 1F 00 1C A0 1F E0 0F 5F 00 1B A0 1F E0 0F 5F 00 1A E0 16 1F 00 19 A0 1F E0 0F 5F 40 00 E1 73 9F 40 00 E0 FF 7F 42 5F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 31 1F 01 00 00 05 01 38 00 60 00 9D 01 00 00 00 00 00 B8 8B 40 07 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 4F 02 7A 03 00 00 00 00 00 00 3F 08 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 8C B0 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 06 7F 03 09 00 00 00 
            using (OutPacket plew = new OutPacket())
            {
                c.SendRawLock(ConvertToHexString(
                    "9C 01 81 00 D1 00 EE 02 78 3F E9 EB 08 05 01 1E 00 9C 01 BF 02 00 20 00 00 0B 20 03 06 67 76 64 67 64 63 76 20 09 E0 01 00 05 BD AD BA FE C8 CB E0 01 0F 40 00 0B 01 00 01 00 C8 00 78 05 01 01 0B FF 60 00 40 14 05 00 E9 EB D1 18 8A 60 09 C0 00 06 DD A1 8B 00 FB 5B 7C 60 0E 02 7C B2 78 60 07 E0 07 00 02 45 54 89 E0 07 12 80 00 02 41 E9 EB 80 08 E0 01 00 40 6B E0 01 0D E0 02 00 02 BC 41 08 C0 0D 05 98 BC 41 00 00 01 A0 00 00 BC 40 15 09 B8 16 22 C3 C0 A8 0A FD 1F 40 40 27 E0 07 43 00 FF E0 0D 51 E0 07 00 E0 04 37 00 01 C0 9B 03 72 34 08 01 20 29 E0 07 53 80 00 E0 01 BF 02 01 01 81 A0 9F 00 2E A0 07 60 00 00 EB E0 03 37 40 0B 40 4B 02 00 E2 81 20 6B 01 FF FF 98 13 81 00 46 00 5F 14 00 00 00 00 08 05 01 4F 00 98 13 EC 14 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 FF 00 E0 F4 00 01 00 00 08 0A 81 00 84 00 0D 0B 00 00 00 00 08 05 01 42 00 08 0A 4F 0B 00 20 00 00 02 20 03 01 00 01 40 04 E0 23 00 00 01 E0 28 31 E0 29 00 01 01 FF E0 27 33 E0 29 95 E0 29 31 41 2C 00 FF E0 56 00 03 8D 0D 93 0E 20 68 E0 54 00 03 E2 04 22 04 E0 54 60 E0 FF 00 E0 82 00 00 01 20 01 E0 57 00 E0 BF 63 E0 BF 00 02 E7 A1 10 60 03 E0 BD 00 01 C0 3F E0 B7 C7 02 21 15 DE E0 58 C3 00 64 21 24 40 03 E0 B7 00 40 C3 40 03 E0 FF 00 E0 45 00 01 00 00 18 00 81 00 19 00 B2 00 18 41 E9 EB 08 05 01 51 00 18 00 6E 01 00 20 00 00 3E 20 03 00 1C 20 03 03 00 00 B0 04 05 01 44 02 2C 00 75 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 01 42 02 1C 00 63 03 00 00 00 00 04 8E A5 E7 00 00 00 00 01 00 00 00 00 00 00 00 05 01 4E 02 10 00 63 03 00 00 00 00 00 F0 2E 08 C8 00 81 00 81 00 CA 01 68 40 E9 EB 08 05 01 50 00 C8 00 1D 02 00 20 00 06 67 76 64 67 64 63 76 20 09 E0 01 00 05 BD AD BA FE C8 CB E0 01 0F 40 00 03 01 01 0B FF 60 00 07 00 E9 EB 45 54 89 00 3E 20 14 40 03 00 1C 20 07 40 03 00 0A 20 07 E0 07 00 07 B0 04 00 00 03 03 03 00 80 01 04 14 00 11 00 04 20 01 02 0C 00 09 20 05 04 04 00 F1 F6 0F 20 30 02 02 01 05 20 05 E0 0B 00 01 C2 F5 40 15 02 E8 03 00 E0 0C 5B 03 FF FF FF FF 05 01 63 02 28 00 90 03 00 00 00 00 01 00 01 00 00 00 00 00 00 00 01 00 00 00 00 00 06 00 00 00 08 07 00 00 00 E3 C2 F5 05 01 27 07 10 00 3C 08 00 00 00 00 00 00 00 00 05 01 5C 00 C0 00 21 02 00 00 00 00 0B 00 00 00 D1 18 8A 00 00 00 00 00 00 00 00 00 00 00 00 00 DD A1 8B 00 FB 5B 7C 00 00 00 00 00 7C B2 78 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 54 89 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 01 01 01 01 01 01 01 01 74 00 00 00 00 00 08 00 00 00 00 01 00 00 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 3A 01 0C 96 81 00 25 07 B2 9D 68 8A DE EA 08 05 01 20 01 0C 96 31 98 00 20 00 04 F5 88 8C 00 01 20 07 04 1F 03 00 00 FF 20 00 40 07 40 00 40 13 40 00 00 F6 E0 16 1F 00 F7 E0 16 1F 00 FF A0 1F 01 A2 02 80 5F 40 07 40 00 C0 5F 01 00 89 80 7F 40 17 20 7E E0 08 1F 00 01 E0 16 1F 00 02 E0 16 1F 00 EB A0 7F 00 89 A0 7F 40 07 40 00 C0 7F 00 EC E0 16 1F 00 ED E0 16 1F 00 EE E0 16 1F 00 E1 A0 1F 00 F4 20 70 40 DF 40 07 40 00 20 0E 60 00 00 E2 E0 16 1F 00 E3 E0 16 1F 00 E4 E0 16 1F 00 C6 A0 1F 00 DE 20 70 40 7F 40 07 40 00 20 0E 60 00 00 C5 E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 D0 E0 16 1F 00 CF E0 16 1F 00 CE E0 16 1F 00 CD E0 16 1F 00 DA A0 1F 00 F0 20 F0 40 FF 40 07 40 00 20 0E 60 00 00 D9 E0 16 1F 00 D8 E0 16 1F 00 D7 E0 16 1F 00 BB A0 1F 00 AD 20 70 40 7F 40 07 40 00 20 0E 60 00 00 BA E0 16 1F 00 B9 E0 16 1F 01 77 90 83 3F 00 6E 20 4D 02 10 9B 27 60 07 40 00 C0 5F 00 76 E0 16 1F 00 75 E0 16 1F 00 6D E0 16 1F 00 6C E0 16 1F 00 6B E0 16 1F 00 B1 E0 16 DF 00 B0 E0 16 1F 00 AF E0 16 1F 00 A7 A0 1F 00 CC 21 10 41 7F 40 07 40 00 20 0E 60 00 00 A6 E0 16 1F 00 A5 E0 16 1F 00 9D A0 1F 41 D7 40 5F 40 07 40 00 C0 5F 00 9C E0 16 1F 00 9B E0 16 1F 40 00 C0 4B 40 5F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 EB 1F 02 23 B0 8C 61 FF 00 32 22 04 40 FF 40 07 40 00 20 0E 60 00 00 24 E0 16 1F 00 25 E0 16 1F 00 26 E0 16 1F 00 0F A0 1F 00 F9 20 6D 40 7F 40 07 40 00 C0 7F 00 1B A0 1F 00 DD 20 14 40 1F 40 07 40 00 C0 1F 00 1A E0 16 1F 00 19 E0 16 1F 00 05 A0 1F 00 A5 20 54 40 5F 40 07 40 00 C0 5F 01 FB AF 81 1F 40 17 E0 0B 1F 00 FC E0 16 1F 00 FD E0 16 1F 00 FE E0 16 1F 00 F4 E0 16 1F 00 F3 E0 16 1F 00 F2 E0 16 1F 00 F1 E0 16 1F 00 B8 E0 16 1F 00 B7 E0 16 1F 00 B6 E0 16 1F 00 B5 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 C0 E0 16 1F 00 BF E0 16 1F 00 DD E0 16 1F 00 E9 E0 16 1F 00 E8 E0 16 1F 00 E7 E0 16 1F 00 D5 E0 16 1F 00 D4 E0 16 1F 00 D3 E0 16 1F 00 C9 E0 16 1F 00 AD A0 1F 00 7B 23 14 42 FF 40 07 40 00 C3 1F 00 AC E0 16 1F 00 AB E0 16 1F 40 00 C0 4B 40 5F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 F3 1F 02 29 C4 77 60 FF 00 50 21 14 42 1F 40 07 E0 03 00 01 05 7A 80 1F 00 DD 20 14 02 10 9B 27 60 07 40 00 C1 33 C0 1F 00 3D 20 14 02 90 48 09 60 07 40 00 C0 1F 01 FB 79 80 3F 00 32 20 10 02 10 B5 76 60 07 40 00 20 0E 60 00 C0 1F 00 CA 20 0D 02 10 28 4F 60 07 40 00 E0 07 1F 00 6E 20 14 40 7F 40 07 40 00 E0 07 1F 00 1F 20 14 40 7F 40 07 40 00 C0 1F 01 C9 56 80 7F 00 14 20 10 40 DF 40 07 02 A0 BB 0D E0 00 1F 01 D5 54 80 1F 00 37 20 34 40 1F 40 07 E0 03 00 01 46 53 80 1F 00 87 20 14 40 1F 40 07 E0 03 00 00 45 A0 1F 00 4A 20 14 40 1F 40 07 E0 07 00 20 7A 60 00 40 1F 60 08 E0 06 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 EB 1F 02 BF DE 8C 61 FF 00 EA 22 04 40 FF 40 07 40 00 00 01 20 01 40 00 00 BE E0 16 1F 00 BD E0 16 1F 01 CB DA 80 5F 00 82 A0 5F 40 07 40 00 20 0E 60 00 00 CC E0 16 1F 00 CD E0 16 1F 00 CE E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 A2 A1 1F 40 F7 41 5F 40 07 40 00 C0 FF 00 A1 E0 16 1F 00 A0 E0 16 1F 00 9F E0 16 1F 00 DE A0 1F 00 DD 20 74 02 10 9B 27 60 07 40 00 C0 7F 00 DD E0 16 1F 00 DC E0 16 1F 00 DB E0 16 1F 00 E7 A0 1F 00 4B 20 70 40 FF 40 07 40 00 20 0E 60 00 00 E6 E0 16 1F 00 E5 E0 16 1F 00 CA A0 1F 00 A5 20 4D 40 DF 40 07 40 00 C0 5F 00 C9 E0 16 1F 00 C8 E0 16 1F 00 C7 E0 16 1F 00 AC A0 1F 41 D7 40 DF 40 07 40 00 C0 7F 00 AB E0 16 1F 00 AA E0 16 1F 00 A9 E0 16 1F 00 98 A0 1F 41 57 40 7F 40 07 40 00 C0 7F 00 97 E0 16 1F 00 96 E0 16 1F 00 95 E0 16 1F 00 8E A0 1F 00 70 20 70 40 7F 40 07 40 00 20 0E 60 00 00 8D E0 16 1F 00 8C E0 16 1F 00 8B E0 16 1F 00 D8 A3 7F 40 F7 40 7F 40 07 40 00 C0 7F 00 D7 E0 16 1F 00 D6 E0 16 1F 00 D5 E0 16 1F 00 BA A0 1F 41 F7 40 7F 40 07 40 00 C0 7F 00 B9 E0 16 1F 00 B8 E0 16 1F 00 B7 E0 16 1F 00 B0 A0 1F 41 77 40 7F 40 07 40 00 C0 7F 00 AF E0 16 1F 00 AE E0 16 1F 00 A6 A0 1F 00 32 20 50 40 5F 40 07 40 00 20 0E 60 00 00 A5 E0 16 1F 00 A4 E0 16 1F 00 A3 E0 16 1F 00 FC A1 FF 02 2E 02 00 60 7F 40 07 40 00 C0 7F 00 FB E0 16 1F 00 F9 E0 16 1F 40 00 C0 4B 40 DF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 91 1F 01 00 00 0C 96 81 00 E6 03 73 9A 68 8A DE EA 08 05 01 21 01 0C 96 32 98 00 20 00 04 3B C8 77 00 01 20 07 00 32 20 04 00 FF 20 00 40 07 40 00 20 0E 60 00 00 3A E0 16 1F 00 39 E0 16 1F 00 38 E0 16 1F 00 35 E0 16 1F 00 34 E0 16 1F 00 33 E0 16 1F 00 30 E0 16 1F 00 2F E0 16 1F 00 2E E0 16 1F 00 2D E0 16 1F 00 2C E0 16 1F 00 2B E0 16 1F 00 2A E0 16 1F 00 29 E0 16 1F 00 28 E0 16 1F 00 27 E0 16 1F 00 26 E0 16 1F 00 25 E0 16 1F 00 24 E0 16 1F 00 23 E0 16 1F 00 22 E0 16 1F 00 21 E0 16 1F 00 20 E0 16 1F 00 1F E0 16 1F 00 1E E0 16 1F 00 1D E0 16 1F 00 1C E0 16 1F 00 1B E0 16 1F 00 1A E0 16 1F 00 19 E0 16 1F 00 18 E0 16 1F 00 17 E0 16 1F 00 16 E0 16 1F 00 15 E0 16 1F 00 14 E0 16 1F 00 13 E0 16 1F 00 12 E0 16 1F 00 11 E0 16 1F 40 00 C4 CB 24 DE 00 FF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 93 1F 02 E7 8C 8C 60 9F 00 AD 20 A4 41 BF 40 07 40 00 20 0E 60 00 00 DD A0 1F 00 70 20 10 40 1F 40 07 40 00 20 0E 60 00 00 D3 A0 1F 00 32 20 10 40 1F 40 07 40 00 20 0E 60 00 00 C9 A0 1F 00 FB 20 0D 40 1F 40 07 40 00 C0 1F 00 A1 A0 1F 00 DD 20 14 40 1F 40 07 40 00 C0 1F 00 97 A0 1F 00 B8 20 14 40 1F 40 07 40 00 C0 1F 00 8D A0 1F 00 9F 20 14 40 1F 40 07 40 00 C0 1F 00 83 A0 1F 00 7B 20 14 40 1F 40 07 40 00 C0 1F 40 00 C0 0B 40 1F 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 D1 1F 01 00 00 0C FA 81 00 2A 0D B7 07 68 8A DE EA 08 05 01 1F 01 0C FA 30 FC 00 20 00 04 54 F3 86 00 01 20 07 04 D4 03 00 00 FF 20 00 40 07 E0 03 00 00 56 A0 1F 00 EF 20 24 20 1E 00 FF 40 07 60 00 20 0F 40 00 01 3B E7 80 3F 00 14 20 0F 40 1F 40 07 40 00 00 01 20 0F 40 00 02 8D 06 87 60 2A 00 C6 20 0C 40 1F 40 07 40 00 A0 1E 01 00 90 20 1F 00 0A 20 10 00 31 20 03 40 1F 40 07 E0 03 00 01 FF F2 80 5F 00 DA 20 14 40 1F 40 07 E0 03 00 00 46 A0 9F 00 87 20 14 40 1F 40 07 E0 03 00 00 94 20 5F 40 6B 00 3A 20 14 40 1F 40 07 E0 03 00 00 95 A0 1F 00 80 20 14 40 1F 40 07 E0 03 00 00 96 A0 1F 00 02 20 44 40 1F 40 07 E0 03 00 02 F4 5D 87 60 BF 01 57 02 81 3F 40 07 E0 03 00 40 1F 20 3A 01 00 45 20 14 40 3F 40 07 E0 03 00 00 F3 20 3F 40 1F 00 63 20 14 40 1F 40 07 E0 03 00 01 65 F6 80 FF 00 BD 20 14 40 1F 40 07 E0 03 00 00 67 A0 1F 00 9F 20 14 40 1F 40 07 E0 03 00 01 5D EF 80 3F 41 77 40 1F 40 07 E0 03 00 00 76 A0 DF 40 17 E0 0B 1F 00 4B A1 5F 00 62 20 34 40 1F 40 07 E0 03 00 00 4F A0 1F 00 37 20 14 40 1F 40 07 E0 03 00 00 50 A0 1F 00 A9 20 14 40 1F 40 07 E0 03 00 00 F0 A1 DF 41 B7 40 1F 40 07 E0 03 00 00 70 20 BF 00 04 20 10 00 64 21 24 40 1F 40 07 E0 03 00 00 8E A0 BF 00 B0 A2 BF 40 07 E0 03 00 00 FA A0 5F 00 98 A0 1F 40 07 E0 03 00 00 66 A0 5F 00 32 A0 5F 40 07 E0 03 00 00 5C A0 1F 42 D7 40 7F 40 07 E0 03 00 00 52 A0 1F 00 F5 20 14 40 1F 40 07 E0 03 00 00 48 A0 1F 00 D6 20 14 40 1F 40 07 E0 03 00 00 3E A0 1F 00 B8 20 14 40 1F 40 07 E0 03 00 00 34 A0 1F 00 99 20 14 40 1F 40 07 E0 03 00 00 2A A0 1F 00 7B 20 14 40 1F 40 07 E0 03 00 00 20 A0 1F 00 5C 20 14 40 1F 40 07 E0 03 00 00 16 A0 1F 00 3D 20 14 40 1F 40 07 E0 03 00 00 38 A3 DF 41 D7 40 1F 40 07 E0 03 00 00 8E 20 3F 43 BF 00 F0 20 14 40 1F 40 07 E0 03 00 40 1F 21 5A 01 00 23 20 14 40 1F 40 07 E0 03 00 00 FD A1 9F 41 57 40 1F 40 07 E0 03 00 00 FC E0 16 1F 00 FE E0 16 1F 00 45 A2 7F 42 57 40 5F 40 07 E0 03 00 01 CF 31 84 9F 41 F7 40 1F 40 07 40 CF 40 B3 40 00 C0 1F 00 A5 20 0C 40 1F 40 07 00 05 20 0B C0 1F 00 70 22 7F 00 1E 20 0F 00 70 20 10 40 1F 40 07 E0 03 00 40 1F 40 4F 40 37 40 1F 40 07 E0 03 00 00 6B A0 3F 00 3F A0 3F 40 07 E0 03 00 40 1F 40 3F 41 D7 40 3F 40 07 E0 03 00 00 69 20 3F 20 3A 01 00 1D A2 DF 40 07 E0 03 00 00 49 A0 FF 41 57 40 3F 40 07 E0 03 00 00 48 A0 1F 00 EA 20 44 40 1F 40 07 E0 03 00 00 F9 A1 5F 00 DF A4 DF 40 07 E0 03 00 00 F8 A0 1F 00 28 A0 1F 40 07 E0 03 00 04 1C EB 86 00 16 20 10 41 17 40 5F 40 07 E0 03 00 40 1F 40 DF 42 F7 40 1F 40 07 E0 03 00 00 1B A0 3F 40 17 E0 0F 1F 40 3F 42 D7 40 1F 40 07 E0 03 00 00 12 A0 3F 00 4B A0 DF 40 07 E0 03 00 40 1F 40 3F 41 97 40 3F 40 07 E0 03 00 00 11 A0 3F 45 57 40 1F 40 07 E0 03 00 40 1F 40 3F 00 50 20 14 40 1F 40 07 E0 03 00 00 37 A3 3F 41 17 40 1F 40 07 60 00 20 8F 40 00 00 36 A0 1F 00 B9 20 0F 40 1F 40 07 60 00 20 0F 40 00 00 35 A0 1F 00 4F 25 04 40 1F 40 07 E0 03 00 00 2E A0 1F 00 97 A1 FF 40 07 E0 07 00 A0 4A 60 3F E0 07 1B 40 00 E0 FF 1F 41 5F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 B3 1F 04 F4 EF 86 00 05 20 D0 00 3D 20 03 41 DF 40 07 E0 03 00 00 EA E0 16 1F 00 D6 20 1F 5E 7F 00 DD 20 34 40 3F 40 07 E0 03 00 40 1F 3E 3A 01 00 6E 20 14 40 1F 40 07 E0 03 00 00 D5 A0 3F 00 7B 20 14 40 1F 40 07 E0 03 00 40 1F 40 3F 40 97 40 1F 40 07 E0 03 00 00 CC A0 3F 40 77 40 1F 40 07 E0 03 00 40 1F 40 3F 40 77 40 1F 40 07 E0 03 00 00 CB A0 3F 40 77 40 1F 40 07 E0 03 00 40 1F 40 3F 40 77 40 1F 40 07 E0 03 00 00 0F 20 3F 41 FF 00 26 20 04 40 1F 40 07 02 E0 E6 0B 60 13 40 00 00 01 A0 1F 00 F5 20 0C 40 1F 40 07 02 20 12 0A E0 00 1F 02 FC EE 86 60 0B 40 17 C0 1F 02 A0 BB 0D 60 13 C0 00 20 5A 60 00 40 1F 60 08 E0 06 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 AB 1F 02 1A 31 89 61 BF 00 C6 21 D4 02 90 48 09 60 07 40 00 C1 D3 00 1F 20 1F 00 05 20 10 00 96 20 03 40 DF 40 07 E0 03 00 40 1F 00 0B 20 10 00 2C 20 30 40 1F 40 07 E0 03 00 02 D7 91 88 60 4B 00 4E 20 14 40 5F 40 07 40 00 20 2E 60 00 00 D8 E0 16 1F 00 D9 E0 16 1F 01 20 92 E0 01 5F 40 7F C0 5F C0 00 02 25 F2 86 60 6B 02 7A 06 00 60 1F 01 CE 07 80 19 00 01 A0 80 00 8A E0 16 1F 01 A2 2D 81 1F 00 62 20 36 40 5F 40 07 E0 03 00 00 A1 A0 1F 00 44 20 14 40 1F 40 07 E0 03 00 00 89 A0 5F 00 77 A0 7F 40 07 40 00 C0 7F 00 26 E0 16 1F 02 7A 06 87 60 BF 00 BD 20 34 41 3F 40 07 40 00 A0 BE 02 00 10 93 80 FF 00 23 20 14 40 1F 40 07 40 00 C0 1F 00 0F E0 16 1F 00 0E E0 16 1F 00 0D E0 16 1F 00 4F A0 1F 00 6E 20 74 02 10 9B 27 60 07 40 00 C0 DF 00 77 A0 BF 00 EA 20 90 41 1F 40 07 E0 03 00 00 88 E0 16 FF 00 24 E0 16 1F 02 E8 BC 84 60 EB 00 B9 20 5F 40 7F 40 07 40 00 20 0E 60 00 00 E7 E0 16 1F 00 79 A0 1F 00 14 20 30 40 3F 40 07 40 00 20 0E 60 00 00 7F A0 BF 40 DF 40 BF 40 07 E0 03 00 00 7D A0 1F 00 56 20 14 40 1F 40 07 E0 03 00 00 7C A0 1F 00 4A 20 14 40 1F 40 07 E0 03 00 00 7B A0 1F 40 57 40 1F 40 07 60 00 A0 80 00 4C A1 5F 00 DD 20 14 40 9F 40 07 40 00 A0 1E 01 00 4A A0 1F 40 F7 02 10 B5 76 60 07 40 00 E0 07 1F 00 A5 20 14 40 3F 40 07 40 00 C0 1F 00 49 A0 3F 40 F7 40 1F 40 07 40 00 E0 07 1F 00 5C 20 14 42 5F 40 07 40 00 C0 1F 00 5B A3 9F 00 7B 20 14 40 3F 40 07 40 00 E0 07 1F 00 31 20 14 40 3F 40 07 40 00 C0 1F 00 5A E0 16 3F C0 1F 40 37 E0 0B 3F 00 59 E0 16 3F C0 1F E0 0F 3F 00 58 A0 3F 41 77 40 BF 40 07 40 00 C0 9F C0 1F 00 2B 20 14 40 7F 40 07 40 00 C0 1F 00 57 E0 16 3F C0 1F 40 37 E0 0B 3F 00 56 E0 16 3F C0 1F E0 0F 3F 00 55 E0 16 3F C0 1F E0 0F 3F 04 B5 A6 87 00 64 20 D0 42 B7 42 7F 40 07 E0 03 00 40 1F 00 28 20 10 40 3F 40 1F 40 07 E0 03 00 01 D3 45 85 1F 42 37 41 3F 40 07 40 00 C3 FF 00 D2 E0 0F 1F 40 24 20 00 C0 1F 00 50 20 0B 41 1F 40 07 40 00 C0 1F 00 D1 E0 16 3F C0 1F 40 37 E0 0B 3F 00 CF E0 16 3F C0 1F E0 0F 3F 00 CE E0 16 3F C0 1F E0 0F 3F 00 6F E0 0F 3F A0 E0 00 6E E0 0F 1F 41 04 20 00 C0 1F E0 0F 5F 00 6D E0 16 3F C0 1F E0 0F 3F 00 6C E0 16 3F C0 1F E0 0F 3F 00 6B E0 16 3F C0 1F E0 0F 3F 00 6A E0 16 3F C0 1F E0 0F 3F 00 69 E0 16 3F C0 1F E0 0F 3F 00 0B E0 0F 3F A1 80 00 0A E0 0F 1F 21 9B 40 00 C0 1F E0 0F 5F 00 09 E0 16 3F C0 1F E0 0F 3F 00 08 E0 16 3F C0 1F E0 0F 3F 00 07 E0 16 3F C0 1F E0 0F 3F 00 06 E0 16 3F C0 1F E0 0F 3F 01 08 B8 88 9F 00 1F 21 2C 02 90 A5 06 60 07 40 00 C1 3F 00 07 A0 1F 44 57 40 1F 40 07 40 00 C0 1F 00 06 A0 1F E0 0F 3F 00 05 E0 16 1F 00 04 E0 16 1F 00 03 E0 16 1F 00 02 E0 16 1F 00 01 A0 1F 40 B7 E0 0B BF 00 C8 A5 9F 40 BF 45 1F 40 07 40 00 C0 DF 00 C7 E0 16 1F 00 C6 E0 16 1F 00 C4 E0 16 1F 00 C3 E0 16 1F 00 C2 E0 16 1F 00 C1 E0 16 1F 00 C0 E0 16 1F 00 BF E0 16 1F 00 BD E0 16 1F 00 BC E0 16 1F 00 BA E0 16 1F 00 B9 E0 16 1F 40 00 C1 8B 46 DF 40 0F E0 07 00 E0 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 F3 1F 02 57 B2 87 60 FF 00 2E 21 14 42 1F 00 5C 20 07 E0 03 00 00 56 E0 16 1F 00 55 E0 16 1F 00 52 A0 1F 00 34 20 54 40 5F 00 68 20 07 E0 03 00 00 51 E0 16 1F 00 50 E0 16 1F 00 4F A0 1F E0 0F BF 00 4D A0 1F 00 37 20 74 40 7F 00 6E 20 07 E0 03 00 00 4B A0 1F E0 0F 9F 00 4A A0 1F E0 0F 5F 00 49 E0 16 1F 40 00 E2 F3 5F 40 00 E0 FF FF 42 7F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 03 00 02 3F B6 87 62 1F 00 1C 20 14 41 1F 00 37 20 07 E0 03 00 00 3E E0 16 1F 00 3D E0 16 1F 00 3C A0 1F 00 16 20 54 40 5F 00 2B 20 07 E0 03 00 00 3B E0 16 1F 00 3A A0 1F E0 0F 9F 00 39 E0 16 1F 00 38 A0 1F 00 19 20 74 40 7F 00 31 20 07 E0 03 00 00 37 A0 1F E0 0F 9F 00 36 E0 16 1F 00 35 A0 1F E0 0F 9F 00 34 E0 16 1F 00 33 A0 1F E0 0F 9F 00 32 A0 1F E0 0F 9F 00 31 E0 16 1F 40 00 E3 FF FF 41 FF 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 73 1F 02 21 BA 87 60 7F 00 1C 20 94 41 9F 00 37 20 07 E0 03 00 00 20 E0 16 1F 00 1F A0 1F 00 16 20 34 40 3F 00 2B 20 07 E0 03 00 00 1E A0 1F E0 0F 5F 00 1D E0 16 1F 00 1C A0 1F E0 0F 5F 00 1B A0 1F E0 0F 5F 00 1A E0 16 1F 00 19 A0 1F E0 0F 5F 40 00 E1 73 9F 40 00 E0 FF 7F 42 5F 41 0F E0 07 00 E1 FF 1F E1 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 FF 1F E2 FF 1F E0 07 00 E2 31 1F 01 00 00 05 01 38 00 60 00 9D 01 00 00 00 00 01 00 00 A0 01 00 00 00 00 00 00 00 01 6C 84 44 00 00 C0 3F 62 0E 22 04 00 00 00 00 00 00 7F 03 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00  "));
            }
        }

        public static void Map_Load_Ack(Client c)
        {
            // 01 81 00 4B 00 40 02 00 00 00 00 08 05 01 CC 01 74 01 45 04 00 20 00 08 65 50 89 00 74 06 87 00 9C 20 03 E0 13 00 00 01 60 01 E0 05 00 04 80 F4 03 00 FF E0 1A 00 E0 05 35 E0 11 00 40 63 80 00 02 01 01 01 80 08 E0 48 00 00 64 20 51 E0 1B 03 E0 45 00 01 00 00 

            using (OutPacket plew = new OutPacket())
            {
                c.SendRawLock(
                    ConvertToHexString("05 01 9B 01 18 00 B8 02 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 "));
            }
        }

        public static void Map_Load2_Ack(Client c)
        {
            //   05 01 9B 01 18 00 B8 02 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 05 01 38 00 60 00 9D 01 00 00 00 00 01 06 56 40 04 00 00 00 00 00 00 00 01 00 00 00 CD CC 2C 40 F5 0A 75 04 00 00 00 00 00 00 2E 46 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 8C B0 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 09 55 03 09 00 00 00 05 01 AB 03 24 00 D4 04 00 00 00 00 02 00 00 00 19 00 00 00 05 00 00 00 05 00 00 00 19 00 00 00 19 00 00 00 C8 00 81 00 A9 00 F2 01 D8 C0 43 B7 08 05 01 50 00 C8 00 1D 02 00 20 00 07 8E D7 B7 D6 D6 AE 8E D7 20 0A E0 00 00 07 C9 D9 BB CA B0 CB D0 C7 E0 00 10 20 00 1F 01 B9 05 09 09 09 00 00 FF 01 1B 01 45 54 89 00 2B 2F 00 00 EE 27 00 00 8D 38 00 00 F2 33 00 00 04 23 4C AD CA 04 20 27 C0 00 1D 50 16 00 00 10 0E 10 0E 03 03 2B 00 DA 04 2C 00 25 00 E3 2E CE 22 B0 01 B0 01 D8 26 A8 1C 40 07 09 00 00 6B 26 00 00 03 01 0B 08 40 33 1C 28 00 5F 01 29 00 22 00 63 08 80 01 93 09 8C 05 80 01 27 D6 E7 25 00 00 E8 03 00 00 0A 40 20 E0 06 00 03 2E 00 00 00 05 01 63 02 28 00 90 03 00 00 00 00 4C 00 35 00 1B 00 1B 00 2A 00 2E 00 ED 8F 00 00 8C CE 00 00 20 1C 00 00 00 A0 4C 08 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 

            using (OutPacket plew = new OutPacket())
            {
                c.SendRawLock(ConvertToHexString(
                    "05 01 9B 01 18 00 B8 02 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 05 01 38 00 60 00 9D 01 00 00 00 00 01 06 56 40 04 00 00 00 00 00 00 00 01 00 00 00 CD CC 2C 40 F5 0A 75 04 00 00 00 00 00 00 2E 46 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 8C B0 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 09 55 03 09 00 00 00 05 01 AB 03 24 00 D4 04 00 00 00 00 02 00 00 00 19 00 00 00 05 00 00 00 05 00 00 00 19 00 00 00 19 00 00 00 C8 00 81 00 A9 00 F2 01 D8 C0 43 B7 08 05 01 50 00 C8 00 1D 02 00 20 00 07 8E D7 B7 D6 D6 AE 8E D7 20 0A E0 00 00 07 C9 D9 BB CA B0 CB D0 C7 E0 00 10 20 00 1F 01 B9 05 09 09 09 00 00 FF 01 1B 01 45 54 89 00 2B 2F 00 00 EE 27 00 00 8D 38 00 00 F2 33 00 00 04 23 4C AD CA 04 20 27 C0 00 1D 50 16 00 00 10 0E 10 0E 03 03 2B 00 DA 04 2C 00 25 00 E3 2E CE 22 B0 01 B0 01 D8 26 A8 1C 40 07 09 00 00 6B 26 00 00 03 01 0B 08 40 33 1C 28 00 5F 01 29 00 22 00 63 08 80 01 93 09 8C 05 80 01 27 D6 E7 25 00 00 E8 03 00 00 0A 40 20 E0 06 00 03 2E 00 00 00 05 01 63 02 28 00 90 03 00 00 00 00 4C 00 35 00 1B 00 1B 00 2A 00 2E 00 ED 8F 00 00 8C CE 00 00 20 1C 00 00 00 A0 4C 08 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00 05 01 A0 03 04 02 A9 06 00 00 00 00 31 31 31 31 31 31 31 31 31 31 31 31 31 31 30 31 31 31 31 30 31 31 31 31 31 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 31 31 30 30 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 31 30 31 30 30 30 30 31 31 30 31 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 6C 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF 06 00 00 00 06 00 00 00 06 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 E3 04 00 00"));
            }
        }

        public static void Map_Load3_Ack(Client c)
        {
            //05 01 09 01 1C 00 2A 02 00 00 00 00 00 B7 38 06 02 02 00 00 00 00 00 00 00 00 00 00 05 01 38 00 60 00 9D 01 00 00 00 00 00 60 8D 40 03 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 2F 00 58 04 00 00 00 00 00 00 EF F6 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 18 61 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 EA 07 8B 03 09 00 00 00 
            using (OutPacket plew = new OutPacket())
            {
                c.SendRawLock(ConvertToHexString(
                    "05 01 09 01 1C 00 2A 02 00 00 00 00 00 B7 38 06 02 02 00 00 00 00 00 00 00 00 00 00 05 01 38 00 60 00 9D 01 00 00 00 00 00 60 8D 40 03 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 2F 00 58 04 00 00 00 00 00 00 EF F6 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 18 61 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 EA 07 8B 03 09 00 00 00"));
            }
        }

        public static void InvenUseSpendShout(Client c, Character chr, string message)
        {
            using (OutPacket plew = new OutPacket(ServerOpcode.INVEN_USESPEND_SHOUT_ACK))
            {
                plew.WriteInt(0); // length + CRC
                plew.WriteInt(0);
                plew.WriteByte(1); // 頻道
                plew.WriteString(chr.Name, 20);
                plew.WriteString(message, 65);
                plew.WriteShort(1); // 類型
                c.Send(plew);
            }
        }

        public static void Message(Client c, int Message)
        {
            using (OutPacket plew = new OutPacket(ServerOpcode.MESSAGE))
            {
                var chr = c.Character;
                plew.WriteInt(0); // length + CRC
                plew.WriteInt(0);
                plew.WriteInt(31102);
                plew.WriteInt(Message);
                c.Send(plew);
            }
        }

        public static void getQuickSlot(Client c, CharacterKeyMap keymaps)
        {
            using (OutPacket plew = new OutPacket(ServerOpcode.QUICKSLOTALL))
            {
                plew.WriteInt(0); // length + CRC
                plew.WriteInt(0);
                // Z
                plew.WriteInt(keymaps.SkillID("Z")); // 技能ID
                plew.WriteShort(keymaps.Type("Z")); // 技能類型
                plew.WriteShort(keymaps.Slot("Z")); // 技能欄位

                // X
                plew.WriteInt(keymaps.SkillID("X")); // 技能ID
                plew.WriteShort(keymaps.Type("X")); // 技能類型
                plew.WriteShort(keymaps.Slot("X")); // 技能欄位

                // C
                plew.WriteInt(keymaps.SkillID("C")); // 技能ID
                plew.WriteShort(keymaps.Type("C")); // 技能類型
                plew.WriteShort(keymaps.Slot("C")); // 技能欄位

                // V
                plew.WriteInt(keymaps.SkillID("V")); // 技能ID
                plew.WriteShort(keymaps.Type("V")); // 技能類型
                plew.WriteShort(keymaps.Slot("V")); // 技能欄位


                // B
                plew.WriteInt(keymaps.SkillID("B")); // 技能ID
                plew.WriteShort(keymaps.Type("B")); // 技能類型
                plew.WriteShort(keymaps.Slot("B")); // 技能欄位

                // N
                plew.WriteInt(keymaps.SkillID("N")); // 技能ID
                plew.WriteShort(keymaps.Type("N")); // 技能類型
                plew.WriteShort(keymaps.Slot("N")); // 技能欄位

                // ============================================

                // 1
                plew.WriteInt(keymaps.SkillID("1")); // 技能ID
                plew.WriteShort(keymaps.Type("1")); // 技能類型
                plew.WriteShort(keymaps.Slot("1")); // 技能欄位

                // 2
                plew.WriteInt(keymaps.SkillID("2")); // 技能ID
                plew.WriteShort(keymaps.Type("2")); // 技能類型
                plew.WriteShort(keymaps.Slot("2")); // 技能欄位

                // 3
                plew.WriteInt(keymaps.SkillID("3")); // 技能ID
                plew.WriteShort(keymaps.Type("3")); // 技能類型
                plew.WriteShort(keymaps.Slot("3")); // 技能欄位

                // 4
                plew.WriteInt(keymaps.SkillID("4")); // 技能ID
                plew.WriteShort(keymaps.Type("4")); // 技能類型
                plew.WriteShort(keymaps.Slot("4")); // 技能欄位


                // 5
                plew.WriteInt(keymaps.SkillID("5")); // 技能ID
                plew.WriteShort(keymaps.Type("5")); // 技能類型
                plew.WriteShort(keymaps.Slot("5")); // 技能欄位

                // 6
                plew.WriteInt(keymaps.SkillID("6")); // 技能ID
                plew.WriteShort(keymaps.Type("6")); // 技能類型
                plew.WriteShort(keymaps.Slot("6")); // 技能欄位

                // ============================================

                // Insert
                plew.WriteInt(keymaps.SkillID("Insert")); // 技能ID
                plew.WriteShort(keymaps.Type("Insert")); // 技能類型
                plew.WriteShort(keymaps.Slot("Insert")); // 技能欄位

                // Home
                plew.WriteInt(keymaps.SkillID("Home")); // 技能ID
                plew.WriteShort(keymaps.Type("Home")); // 技能類型
                plew.WriteShort(keymaps.Slot("Home")); // 技能欄位

                // PageUp
                plew.WriteInt(keymaps.SkillID("PageUp")); // 技能ID
                plew.WriteShort(keymaps.Type("PageUp")); // 技能類型
                plew.WriteShort(keymaps.Slot("PageUp")); // 技能欄位

                // Delete
                plew.WriteInt(keymaps.SkillID("Delete")); // 技能ID
                plew.WriteShort(keymaps.Type("Delete")); // 技能類型
                plew.WriteShort(keymaps.Slot("Delete")); // 技能欄位


                // End
                plew.WriteInt(keymaps.SkillID("End")); // 技能ID
                plew.WriteShort(keymaps.Type("End")); // 技能類型
                plew.WriteShort(keymaps.Slot("End")); // 技能欄位

                // PageDown
                plew.WriteInt(keymaps.SkillID("PageDown")); // 技能ID
                plew.WriteShort(keymaps.Type("PageDown")); // 技能類型
                plew.WriteShort(keymaps.Slot("PageDown")); // 技能欄位

                // ============================================

                // 7
                plew.WriteInt(keymaps.SkillID("7")); // 技能ID
                plew.WriteShort(keymaps.Type("7")); // 技能類型
                plew.WriteShort(keymaps.Slot("7")); // 技能欄位

                // 8
                plew.WriteInt(keymaps.SkillID("8")); // 技能ID
                plew.WriteShort(keymaps.Type("8")); // 技能類型
                plew.WriteShort(keymaps.Slot("8")); // 技能欄位

                // 9
                plew.WriteInt(keymaps.SkillID("9")); // 技能ID
                plew.WriteShort(keymaps.Type("9")); // 技能類型
                plew.WriteShort(keymaps.Slot("9")); // 技能欄位

                // 0
                plew.WriteInt(keymaps.SkillID("0")); // 技能ID
                plew.WriteShort(keymaps.Type("0")); // 技能類型
                plew.WriteShort(keymaps.Slot("0")); // 技能欄位


                // -
                plew.WriteInt(keymaps.SkillID("-")); // 技能ID
                plew.WriteShort(keymaps.Type("-")); // 技能類型
                plew.WriteShort(keymaps.Slot("-")); // 技能欄位

                // =
                plew.WriteInt(keymaps.SkillID("=")); // 技能ID
                plew.WriteShort(keymaps.Type("=")); // 技能類型
                plew.WriteShort(keymaps.Slot("=")); // 技能欄位
                c.Send(plew);
            }
        }

        public static void FW_DISCOUNTFACTION(Client c)
        {
            using (OutPacket plew = new OutPacket(ServerOpcode.FW_DISCOUNTFACTION))
            {
                plew.WriteInt(0); // length + CRC
                plew.WriteInt(0);
                plew.WriteHexString(
                    "00 00 00 00 64 00 00 00 00 00 00 00 64 00 00 00 00 70 40 00 E8 03 D2 A8 74 A9 00 00 84 D1");

                c.Send(plew);
            }
        }

        public static void PuzzleInfo(Client c)
        {
            using (OutPacket plew = new OutPacket(ServerOpcode.PUZZLE))
            {
                plew.WriteInt(0); // length + CRC
                plew.WriteInt(0);
                plew.WriteHexString(
                    "40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40");
                c.Send(plew);
            }
        }

        public static void PuzzleUpdate(Client c)
        {
            using (OutPacket plew = new OutPacket(ServerOpcode.PUZZLE_UPDATE))
            {
                plew.WriteInt(0); // length + CRC
                plew.WriteInt(0);
                plew.WriteHexString("31 4D 0F 00"); // 怪物ID
                plew.WriteHexString("20 00 43 E4");
                c.Send(plew);
            }
        }

        public static void NoticeWelcome(Client c)
        {
            using (OutPacket plew = new OutPacket())
            {
                c.SendRawLock(ConvertToHexString(
                    "05 01 11 00 4C 00 62 01 00 00 00 00 01 5B 20 53 56 52 5F 53 54 44 20 5D 20 50 72 69 76 61 74 65 20 53 65 72 76 65 72 20 49 73 20 57 6F 72 6B 69 6E 67 20 20 53 56 65 72 20 32 30 32 30 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            }
        }

        //
    }
}
