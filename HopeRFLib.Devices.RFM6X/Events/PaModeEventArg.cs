using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class PaModeEventArg : EventArgs
	{
		private PaModeEnum value;

		public PaModeEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public PaModeEventArg(PaModeEnum value)
		{
			this.value = value;
		}
	}
}