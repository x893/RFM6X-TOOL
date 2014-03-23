using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class OokAverageThreshFiltEventArg : EventArgs
	{
		private OokAverageThreshFiltEnum value;

		public OokAverageThreshFiltEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public OokAverageThreshFiltEventArg(OokAverageThreshFiltEnum value)
		{
			this.value = value;
		}
	}
}