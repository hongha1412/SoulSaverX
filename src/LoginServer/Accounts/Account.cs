using Server.Common.Data;
using Server.Common.IO;
using Server.Ghost;
using System;
using System.Data;

namespace Server.Accounts
{
	public sealed class Account
	{
		public Client Client { get; private set; }

		public int ID { get; private set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public DateTime Creation { get; set; }
		public int LoggedIn { get; set; }
		public int Banned { get; set; }
		public int Master { get; set; }
		public int isTwoFactor { get; set; }

		public string TwoFactorPassword { get; set; }
		public int GamePoints { get; set; }
		public int GiftPoints { get; set; }
		public int BonusPoints { get; set; }

		public int total_donate { get; set; }
		private bool Assigned { get; set; }
		public int character_slot { get; set; }
		public Account(Client client)
		{
			this.Client = client;
		}

		public void Load(string username)
		{
			dynamic datum = new Datum("accounts");

			try
			{
				datum.Populate("Username = '{0}'", username);
			}
			catch (RowNotInTableException)
			{
				throw new NoAccountException();
			}

			this.ID = datum.id;
			this.Assigned = true;

			this.Username = datum.username;
			this.Password = datum.password;
		//	this.Creation = datum.creation;
			this.LoggedIn = datum.isLoggedIn;
			this.Banned = datum.isBanned;
			this.Master = datum.isMaster;
			this.isTwoFactor = datum.isTwoFactor;
			this.TwoFactorPassword = datum.TwoFactorPassword;
			this.GamePoints = datum.gamePoints;
			this.GiftPoints = datum.giftPoints;
			this.BonusPoints = datum.bonusPoints;
			this.character_slot = datum.character_slot;
		}

		public void Save()
		{
			dynamic datum = new Datum("accounts");

			datum.username = this.Username;
			datum.password = this.Password;
			//datum.creation = this.Creation;
			datum.isLoggedIn = this.LoggedIn;
			datum.isBanned = this.Banned;
			datum.isMaster = this.Master;
			datum.isTwoFactor = this.isTwoFactor;
			datum.TwoFactorPassword = this.TwoFactorPassword;
			datum.gamePoints = this.GamePoints;
			datum.giftPoints = this.GiftPoints;
			datum.bonusPoints = this.BonusPoints;
			if (this.Assigned)
			{
				datum.Update("ID = '{0}'", this.ID);
			}
			else
			{
				datum.Insert();

				this.ID = Database.Fetch("accounts", "ID", "Username = '{0}'", this.Username);
				this.Assigned = true;
			}

			Log.Inform("Saved account '{0}' to database.", this.Username);
		}



	}
}
