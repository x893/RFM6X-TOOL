using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class RfPaSwitchSelEventArg : EventArgs
	{
		private RfPaSwitchSelEnum value;

		public RfPaSwitchSelEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public RfPaSwitchSelEventArg(RfPaSwitchSelEnum value)
		{
			this.value = value;
		}
	}
}