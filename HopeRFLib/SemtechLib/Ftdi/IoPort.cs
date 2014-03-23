using FTD2XX_NET;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Ftdi
{
	public class IoPort : FtdiIoPort
	{
		public override event FtdiIoPort.IoChangedEventHandler Io0Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io1Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io2Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io3Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io4Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io5Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io6Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io7Changed;

		public override byte PortDir
		{
			get
			{
				return portDir;
			}
			set
			{
				portDir = value;
				int num = (int)SetBitMode(portDir, (byte)1);
			}
		}

		public override byte PortValue
		{
			get
			{
				return portValue;
			}
			set
			{
				portValue = value;
				txBuffer.Add(portValue);
				txBuffer.Add(portValue);
				SendBytes();
			}
		}

		public IoPort(string device)
		{
			Device = device;
			portDir = (byte)0;
			portValue = (byte)0;
		}

		private void OnIo0Changed(bool state)
		{
			if (Io0Changed == null)
				return;
			Io0Changed((object)this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo1Changed(bool state)
		{
			if (Io1Changed == null)
				return;
			Io1Changed((object)this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo2Changed(bool state)
		{
			if (Io2Changed == null)
				return;
			Io2Changed((object)this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo3Changed(bool state)
		{
			if (Io3Changed == null)
				return;
			Io3Changed((object)this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo4Changed(bool state)
		{
			if (Io4Changed == null)
				return;
			Io4Changed((object)this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo5Changed(bool state)
		{
			if (Io5Changed == null)
				return;
			Io5Changed((object)this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo6Changed(bool state)
		{
			if (Io6Changed == null)
				return;
			Io6Changed((object)this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo7Changed(bool state)
		{
			if (Io7Changed == null)
				return;
			Io7Changed((object)this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		public override bool Init(uint frequency)
		{
			lock (syncThread)
			{
				ftStatus = SetBaudRate(frequency);
				if (ftStatus != FTDI.FT_STATUS.FT_OK)
				{
					isInitialized = false;
					return false;
				}
				else
				{
					ftStatus = SetBitMode((byte)0, (byte)1);
					if (ftStatus != FTDI.FT_STATUS.FT_OK)
					{
						isInitialized = false;
						return false;
					}
					else
					{
						readThreadContinue = true;
						readThread = new Thread(new ThreadStart(ReadThread));
						readThread.Start();
						isInitialized = true;
						return true;
					}
				}
			}
		}

		public override bool SendBytes()
		{
			lock (syncThread)
			{
				byte[] local_0 = new byte[txBuffer.Count];
				uint local_1 = 0U;
				txBuffer.CopyTo(local_0);
				ftStatus = Write(local_0, local_0.Length, ref local_1);
				txBuffer.Clear();
				return ftStatus == FTDI.FT_STATUS.FT_OK;
			}
		}

		public override bool ReadBytes(out byte[] rxBuffer, uint bitCount)
		{
			lock (syncThread)
				throw new NotImplementedException();
		}

		private new void ReadThread()
		{
			byte BitMode = (byte)0;
			while (readThreadContinue)
			{
				if (!isInitialized)
				{
					Application.DoEvents();
					Thread.Sleep(10);
				}
				else
				{
					lock (syncThread)
						ftStatus = GetPinStates(ref BitMode);
					if (ftStatus == FTDI.FT_STATUS.FT_OK)
					{
						if (((int)BitMode & 128) == 128)
							OnIo7Changed(true);
						else
							OnIo7Changed(false);
						if (((int)BitMode & 64) == 64)
							OnIo6Changed(true);
						else
							OnIo6Changed(false);
						if (((int)BitMode & 32) == 32)
							OnIo5Changed(true);
						else
							OnIo5Changed(false);
						if (((int)BitMode & 16) == 16)
							OnIo4Changed(true);
						else
							OnIo4Changed(false);
						if (((int)BitMode & 8) == 8)
							OnIo3Changed(true);
						else
							OnIo3Changed(false);
						if (((int)BitMode & 4) == 4)
							OnIo2Changed(true);
						else
							OnIo2Changed(false);
						if (((int)BitMode & 2) == 2)
							OnIo1Changed(true);
						else
							OnIo1Changed(false);
						if (((int)BitMode & 1) == 1)
							OnIo0Changed(true);
						else
							OnIo0Changed(false);
					}
					else
					{
						lock (syncThread)
							Close();
					}
					Thread.Sleep(0);
				}
			}
		}
	}
}