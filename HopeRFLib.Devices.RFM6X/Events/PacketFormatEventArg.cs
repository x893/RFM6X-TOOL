using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class PacketFormatEventArg : EventArgs
	{
		private PacketFormatEnum value;

		public PacketFormatEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public PacketFormatEventArg(PacketFormatEnum value)
		{
			this.value = value;
		}
	}
}