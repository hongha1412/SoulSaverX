namespace Server.Common.IO.Packet
{
	public abstract class PacketBase
	{
		public static LogLevel LogLevel { get; set; }

		public abstract int Position { get; set; }

		public abstract byte[] Content { get; }


	}
}