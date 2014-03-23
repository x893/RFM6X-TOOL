using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class ListenCriteriaEventArg : EventArgs
	{
		private ListenCriteriaEnum value;

		public ListenCriteriaEnum Value
		{
			get
			{
				return this.value;
			}
		}

		public ListenCriteriaEventArg(ListenCriteriaEnum value)
		{
			this.value = value;
		}
	}
}