using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class DcFreeEventArg : EventArgs
	{
		private DcFreeEnum value;

		public DcFreeEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public DcFreeEventArg(DcFreeEnum value)
		{
			this.value = value;
		}
	}
}