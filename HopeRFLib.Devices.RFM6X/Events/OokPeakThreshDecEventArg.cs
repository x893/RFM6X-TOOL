using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class OokPeakThreshDecEventArg : EventArgs
	{
		private OokPeakThreshDecEnum value;

		public OokPeakThreshDecEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public OokPeakThreshDecEventArg(OokPeakThreshDecEnum value)
		{
			this.value = value;
		}
	}
}