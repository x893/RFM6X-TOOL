using System;
using System.Runtime.InteropServices;

namespace SemtechLib.Devices.SX1231.Events
{
	[ComVisible(true)]
	[Serializable]
	public delegate void AddressFilteringEventHandler(object sender, AddressFilteringEventArg e);
}