using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class ClockOutEventArg : EventArgs
	{
		private ClockOutEnum value;

		public ClockOutEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public ClockOutEventArg(ClockOutEnum value)
		{
			this.value = value;
		}
	}
}