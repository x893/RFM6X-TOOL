using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class ExitConditionEventArg : EventArgs
	{
		private ExitConditionEnum value;

		public ExitConditionEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public ExitConditionEventArg(ExitConditionEnum value)
		{
			this.value = value;
		}
	}
}