﻿using System;
using System.Runtime.InteropServices;

namespace SemtechLib.Devices.SX1231.Events
{
	[ComVisible(true)]
	[Serializable]
	public delegate void ListenCriteriaEventHandler(object sender, ListenCriteriaEventArg e);
}