using Server.Characters;
using Server.Common.Data;
using System;

namespace Server
{
	public class Item
	{
		public CharacterItems Parent { get; set; }

		public int ID { get; private set; }
		public int ItemID { get; private set; }
		private short maxPerStack;
		private short quantity;
		public byte IsLocked { get; set; }
		public int Icon { get; set; }
		public int Term { get; set; }
		public byte Type { get; set; }
		public byte Slot { get; set; }

		public bool Assigned { get; set; }

		public Character Character
		{
			get
			{
				try
				{
					return Parent.Parent;
				}
				catch
				{
					return null;
				}
			}
		}

		public short MaxPerStack
		{
			get
			{
				if (maxPerStack == 0) maxPerStack = 100;

				return maxPerStack;
			}
			set => maxPerStack = value;
		}

		public short Quantity
		{
			get => quantity;
			set
			{
				if (value > MaxPerStack)
					throw new ArgumentException("Quantity too high.");
				else
					quantity = value;
			}
		}

		public Item(int itemID, byte type, byte slot, short quantity = 1)
		{
			ItemID = itemID;
			switch (type)
			{
				case 0:
				case 1:
				case 2:
				case 5:
					MaxPerStack = 1;
					break;
				case 3:
				case 4:
				case 6:
					MaxPerStack = 100;
					break;
				case 0x63:
					MaxPerStack = short.MaxValue;
					break;
				default:
					MaxPerStack = 1;
					break;
			}

			Quantity = quantity;
			IsLocked = 0;
			Icon = 0;
			Term = -1;
			Type = type;
			Slot = slot;
		}

		public Item(int itemID, bool IsLocked, byte Icon, int Term, byte type, byte slot, short quantity = 1)
		{
			ItemID = itemID;
			switch (type)
			{
				case 0:
				case 1:
				case 2:
				case 5:
					MaxPerStack = 1;
					break;
				case 3:
				case 4:
				case 6:
					MaxPerStack = 100;
					break;
				case 0x63:
					MaxPerStack = short.MaxValue;
					break;
				default:
					MaxPerStack = 1;
					break;
			}

			Quantity = quantity;
			this.IsLocked = IsLocked ? (byte)1 : (byte)0;
			this.Icon = Icon;
			this.Term = Term;
			Type = type;
			Slot = slot;
		}

		public Item(dynamic datum)
		{
			ID = datum.id;
			Assigned = true;

			ItemID = datum.itemId;
			switch ((byte)datum.type)
			{
				case 0:
				case 1:
				case 2:
				case 5:
					MaxPerStack = 1;
					break;
				case 3:
				case 4:
				case 6:
					MaxPerStack = 100;
					break;
				default:
					MaxPerStack = 1;
					break;
			}

			Quantity = (short)datum.quantity;
			IsLocked = (byte)datum.isLocked;
			Icon = datum.icon;
			Term = datum.term;
			Type = (byte)datum.type;
			Slot = (byte)datum.slot;
		}

		public void Save()
		{
			dynamic datum = new Datum("Items");

			datum.cid = Character.ID;
			datum.itemId = ItemID;
			datum.quantity = Quantity;
			datum.isLocked = IsLocked;
			datum.icon = Icon;
			datum.term = Term;
			datum.type = Type;
			datum.slot = Slot;

			if (Assigned)
			{
				datum.Update("id = '{0}'", ID);
			}
			else
			{
				datum.Insert();

				ID = Database.Fetch("Items", "id",
					"cid = '{0}' && itemId = '{1}' && quantity = '{2}' && isLocked = '{3}' && icon = '{4}' && term = '{5}' && type = '{6}' && slot = '{7}'",
					Character.ID, ItemID, quantity, IsLocked, Icon, Term, Type,
					Slot);

				Assigned = true;
			}
		}

		public void Delete()
		{
			Database.Delete("Items", "id = '{0}'", ID);

			Assigned = false;
		}
	}
}