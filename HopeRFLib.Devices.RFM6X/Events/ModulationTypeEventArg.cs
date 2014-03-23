using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class ModulationTypeEventArg : EventArgs
	{
		private ModulationTypeEnum value;

		public ModulationTypeEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public ModulationTypeEventArg(ModulationTypeEnum value)
		{
			this.value = value;
		}
	}
}