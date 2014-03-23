using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class ListenResolEventArg : EventArgs
	{
		private ListenResolEnum value;

		public ListenResolEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public ListenResolEventArg(ListenResolEnum value)
		{
			this.value = value;
		}
	}
}