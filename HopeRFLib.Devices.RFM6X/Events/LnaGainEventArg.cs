using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class LnaGainEventArg : EventArgs
	{
		private LnaGainEnum value;

		public LnaGainEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public LnaGainEventArg(LnaGainEnum value)
		{
			this.value = value;
		}
	}
}