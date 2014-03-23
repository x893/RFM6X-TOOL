using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class PaRampEventArg : EventArgs
	{
		private PaRampEnum value;

		public PaRampEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public PaRampEventArg(PaRampEnum value)
		{
			this.value = value;
		}
	}
}