using Server.Ghost.Characters;

namespace Server.Ghost
{
	public class Member
	{
		public Character Character { get; set; }

		public Member(Character chr)
		{
			this.Character = chr;
		}
	}
}