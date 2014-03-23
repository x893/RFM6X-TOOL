using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class FifoFillConditionEventArg : EventArgs
	{
		private FifoFillConditionEnum value;

		public FifoFillConditionEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public FifoFillConditionEventArg(FifoFillConditionEnum value)
		{
			this.value = value;
		}
	}
}