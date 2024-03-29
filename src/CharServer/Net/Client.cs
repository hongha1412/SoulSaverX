using Server.Accounts;
using Server.Common.IO;
using Server.Common.IO.Packet;
using Server.Common.Net;
using System;
using System.Net.Sockets;

namespace Server.Ghost
{
	public sealed class Client : Session
	{
		public Account Account { get; private set; }
		public byte WorldID { get; private set; }
		public byte GameID { get; private set; }
		public int RetryLoginCount { get; set; }

		public Client(Socket socket) : base(socket)
		{
			this.RetryLoginCount = 0;
		}

		protected override void Register()
		{
			CharServer.Clients.Add(this);
		}

		protected override void Unregister()
		{
			if (this.Account != null)
			{
				this.Account.LoggedIn = 0;

				this.Account.Save();
			}

			// TODO: Save character.
			CharServer.Clients.Remove(this);
		}

		public bool IsServerAlive
		{
			get { return CharServer.IsAlive; }
		}

		protected override void Dispatch(InPacket inPacket)
		{
			try
			{
#if DEBUG
				Log.Hex("<<< Received (0x{0:X2}) packet from {1}: ", inPacket.Content, inPacket.OperationCode, this.Title);
#endif

				if (inPacket.OperationCode == (ushort)ClientOpcode.SERVER)
				{
					short Header = inPacket.ReadShort(); // Read header
					inPacket.ReadInt(); // Original length + CRC
					inPacket.ReadInt();
#if DEBUG
					Log.Debug("^^^--- <<< Received opcode (0x{0:X}): ", Header);
#endif

					switch ((ClientOpcode)Header)
					{
						case ClientOpcode.MYCHAR_INFO_REQ:
							Log.Debug("Send >> CharHandler.MyChar_Info_Req");
							CharHandler.MyCharInfoReq(inPacket, this);
							break;
						case ClientOpcode.CREATE_MYCHAR_REQ:
							Log.Debug("Send >> CharHandler.Create_MyChar_Req");
							CharHandler.Create_MyChar_Req(inPacket, this);
							break;
						case ClientOpcode.CHECK_SAMENAME_REQ:
							Log.Debug("Send >> CharHandler.Check_SameName_Req");
							CharHandler.CheckSameNameReq(inPacket, this);
							break;
						case ClientOpcode.DELETE_MYCHAR_REQ:
							Log.Debug("Send >> CharHandler.Delete_MyChar_Req");
							CharHandler.Delete_MyChar_Req(inPacket, this);
							break;
						case ClientOpcode.CREATE_PREVIEW_REQ:
							Log.Debug("Send >> CharHandler.Create_Preview_Req");
							CharHandler.Create_Preview_Req(inPacket, this);
							break;
						case ClientOpcode.CHAR_PAGE2_REQ:
							Log.Debug("Send >> CharHandler.Char_page2_preview ");
							CharHandler.Char_page2_preview(inPacket, this);
							break;
					}
				}
			}
			catch (HackException e)
			{
				Log.Warn("Hack from {0}: \n{1}", this.Account.Username, e.ToString());
			}
			catch (Exception e)
			{
				Log.Error("Unhandled exception from {0}: \n{1}", this.Title, e.ToString());
			}
		}

		public void SetAccount(Account Account)
		{
			this.Account = Account;
		}
	}
}
