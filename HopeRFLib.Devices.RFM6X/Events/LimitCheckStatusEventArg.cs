using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class LimitCheckStatusEventArg : EventArgs
	{
		private LimitCheckStatusEnum status;
		private string message;

		public LimitCheckStatusEnum Status
		{
			get
			{
				return this.status;
			}
		}

		public string Message
		{
			get
			{
				return this.message;
			}
		}

		public LimitCheckStatusEventArg(LimitCheckStatusEnum status, string message)
		{
			this.status = status;
			this.message = message;
		}
	}
}