using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class ListenEndEventArg : EventArgs
	{
		private ListenEndEnum value;

		public ListenEndEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public ListenEndEventArg(ListenEndEnum value)
		{
			this.value = value;
		}
	}
}