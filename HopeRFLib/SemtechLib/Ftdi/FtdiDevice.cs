using System;

namespace SemtechLib.Ftdi
{
	public class FtdiDevice : IDisposable
	{
		public enum MpsseProtocol
		{
			SPI,
			I2C,
		}
		public event EventHandler Opened;
		public event EventHandler Closed;

		private Mpsse portA;
		private IoPort portB;

		public bool IsOpen
		{
			get
			{
				if (portA.IsOpen)
					return portB.IsOpen;
				else
					return false;
			}
		}

		public Mpsse PortA
		{
			get { return portA; }
			set { portA = value; }
		}

		public IoPort PortB
		{
			get { return portB; }
			set { portB = value; }
		}

		public FtdiDevice(FtdiDevice.MpsseProtocol protocol)
		{
			switch (protocol)
			{
				case FtdiDevice.MpsseProtocol.SPI:
					portA = (Mpsse)new MpsseSPI("A");
					break;
				case FtdiDevice.MpsseProtocol.I2C:
					portA = (Mpsse)new MpsseI2C("A");
					break;
			}
			portB = new IoPort("B");
			portA.Opened += new EventHandler(ports_Opened);
			portA.Closed += new EventHandler(ports_Closed);
			portB.Opened += new EventHandler(ports_Opened);
			portB.Closed += new EventHandler(ports_Closed);
		}

		private void OnOpened()
		{
			if (Opened == null)
				return;
			Opened((object)this, EventArgs.Empty);
		}

		private void OnClosed()
		{
			if (Closed == null)
				return;
			Closed((object)this, EventArgs.Empty);
		}

		private void ports_Opened(object sender, EventArgs e)
		{
			if (!IsOpen)
				return;
			OnOpened();
		}

		private void ports_Closed(object sender, EventArgs e)
		{
			Close();
		}

		public bool Open(string name)
		{
			if (!portA.Open(name) || !portB.Open(name))
				return false;
			OnOpened();
			return true;
		}

		public bool Close()
		{
			if (portA.IsOpen)
				portA.Close();
			if (portB.IsOpen)
				portB.Close();
			OnClosed();
			return true;
		}

		public void Dispose()
		{
			if (!IsOpen)
				return;
			Close();
		}
	}
}