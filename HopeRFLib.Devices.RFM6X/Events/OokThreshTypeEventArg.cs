using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class OokThreshTypeEventArg : EventArgs
	{
		private OokThreshTypeEnum value;

		public OokThreshTypeEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public OokThreshTypeEventArg(OokThreshTypeEnum value)
		{
			this.value = value;
		}
	}
}