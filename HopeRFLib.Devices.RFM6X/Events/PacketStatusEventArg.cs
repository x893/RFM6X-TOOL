using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class PacketStatusEventArg : EventArgs
	{
		private int number;
		private int max;

		public int Number
		{
			get
			{
				return this.number;
			}
		}

		public int Max
		{
			get
			{
				return this.max;
			}
		}

		public PacketStatusEventArg(int number, int max)
		{
			this.number = number;
			this.max = max;
		}
	}
}