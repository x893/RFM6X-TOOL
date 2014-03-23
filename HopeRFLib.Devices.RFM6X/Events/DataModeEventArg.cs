using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class DataModeEventArg : EventArgs
	{
		private DataModeEnum value;

		public DataModeEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public DataModeEventArg(DataModeEnum value)
		{
			this.value = value;
		}
	}
}