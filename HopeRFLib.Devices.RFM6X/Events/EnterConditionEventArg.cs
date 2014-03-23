using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class EnterConditionEventArg : EventArgs
	{
		private EnterConditionEnum value;

		public EnterConditionEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public EnterConditionEventArg(EnterConditionEnum value)
		{
			this.value = value;
		}
	}
}