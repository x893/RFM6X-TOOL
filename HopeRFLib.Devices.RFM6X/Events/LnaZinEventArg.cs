using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class LnaZinEventArg : EventArgs
	{
		private LnaZinEnum value;

		public LnaZinEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public LnaZinEventArg(LnaZinEnum value)
		{
			this.value = value;
		}
	}
}