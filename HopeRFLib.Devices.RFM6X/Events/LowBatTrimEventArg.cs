using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class LowBatTrimEventArg : EventArgs
	{
		private LowBatTrimEnum value;

		public LowBatTrimEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public LowBatTrimEventArg(LowBatTrimEnum value)
		{
			this.value = value;
		}
	}
}