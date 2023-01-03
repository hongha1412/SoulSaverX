using Server.Accounts;
using Server.Common.IO;
using Server.Common.IO.Packet;
using Server.Common.Net;
using Server.Common.Utilities;
using Server.Interoperability;
using System;
using System.Net.Sockets;

namespace Server.Ghost
{
	public sealed class Client : Session
	{
		public byte WorldID { get; private set; }
		public byte GameID { get; private set; }
		public Account Account { get; private set; }
		public string[] MacAddresses { get; private set; }
		public long SessionID { get; private set; }
		public int RetryLoginCount { get; set; }

		public World World
		{
			get { return LoginServer.Worlds[this.WorldID]; }
			set { this.WorldID = value.ID; }
		}

		public InteroperabilityClient Game
		{
			get { return this.World[this.GameID]; }
			set { this.GameID = value.InternalID; }
		}

		public Client(Socket socket)
			: base(socket)
		{
			this.SessionID = Randomizer.NextLong();
			this.RetryLoginCount = 0;
		}

		protected override void Register()
		{
			LoginServer.Clients.Add(this);

			Log.Inform("Accepted connection from {0}.", this.Title);
		}

		protected override void Unregister()
		{
			if (this.Account != null)
			{
				this.Account.LoggedIn = 0;

				this.Account.Save();
			}

			LoginServer.Clients.Remove(this);

			Log.Inform("Lost connection from {0}.", this.Title);
		}


		protected override void Dispatch(InPacket inPacket)
		{
			if (inPacket.OperationCode != (ushort)ClientOpcode.LOGIN_SERVER) return;

			inPacket.ReadUShort(); // Original length
			var header = (LoginClientOpcode)inPacket.ReadByte(); // Read header

#if DEBUG
			Log.Hex("Received (0x{0:X2}) packet from {1}: ", inPacket.Content, header, this.Title);
#endif

			switch (header)
			{
				case LoginClientOpcode.LOGIN_REQ:
					LoginHandler.HandleLoginRequest(inPacket, this);
					break;
				case LoginClientOpcode.SERVERLIST_REQ:
					LoginHandler.HandleServerListRequest(inPacket, this);
					break;
				case LoginClientOpcode.GAME_REQ:
					LoginHandler.HandleGameRequest(inPacket, this);
					break;
				case LoginClientOpcode.WORLD_REQ:
					LoginHandler.HandleWorldRequest(inPacket, this);
					break;
				case LoginClientOpcode.GAME_VERSIONINFO_REQ:
					LoginHandler.HandlePatchVersionRequest(inPacket, this);
					break;
				case LoginClientOpcode.SUBPASSWORD_REQ:
					LoginHandler.HandleSubPasswordRequest(inPacket, this);
					break;
			}
		}

		public void SetAccount(Account Account)
		{
			this.Account = Account;
		}
	}
}
