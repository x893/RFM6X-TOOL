using FTD2XX_NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SemtechLib.Ftdi
{
	public abstract class FtdiIoPort : FTDI, IDisposable
	{
		public delegate void IoChangedEventHandler(object sender, FtdiIoPort.IoChangedEventArgs e);

		public event EventHandler Opened;
		public event EventHandler Closed;
		public abstract event FtdiIoPort.IoChangedEventHandler Io0Changed;
		public abstract event FtdiIoPort.IoChangedEventHandler Io1Changed;
		public abstract event FtdiIoPort.IoChangedEventHandler Io2Changed;
		public abstract event FtdiIoPort.IoChangedEventHandler Io3Changed;
		public abstract event FtdiIoPort.IoChangedEventHandler Io4Changed;
		public abstract event FtdiIoPort.IoChangedEventHandler Io5Changed;
		public abstract event FtdiIoPort.IoChangedEventHandler Io6Changed;
		public abstract event FtdiIoPort.IoChangedEventHandler Io7Changed;

		protected List<byte> txBuffer = new List<byte>();
		protected object syncThread = new object();
		private string device = "";
		private FTDI.FT_DEVICE_INFO_NODE[] deviceList;
		protected FTDI.FT_STATUS ftStatus;
		protected Thread readThread;
		protected bool readThreadContinue;
		protected bool isInitialized;
		protected FtdiInfo info;
		protected byte portDir;
		protected byte portValue;

		public string Device
		{
			get { return device; }
			set { device = value; }
		}

		public virtual byte PortDir
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public virtual byte PortValue
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
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

		private bool SearchDevice(string name)
		{
			uint devcount = 0U;
			ftStatus = GetNumberOfDevices(ref devcount);
			if (ftStatus == FTDI.FT_STATUS.FT_OK && (int)devcount != 0)
			{
				deviceList = new FTDI.FT_DEVICE_INFO_NODE[devcount];
				ftStatus = GetDeviceList(deviceList);
				if (ftStatus == FTDI.FT_STATUS.FT_OK)
				{
					for (uint index = 0U; index < devcount; ++index)
					{
						string str = ((object)deviceList[index].Description).ToString();
						if (str.Length != 0 && name == str)
						{
							info.DeviceIndex = index;
							info.Flags = deviceList[index].Flags;
							info.Type = ((object)deviceList[index].Type).ToString();
							info.Id = deviceList[index].ID;
							info.LocId = deviceList[index].LocId;
							info.SerialNumber = deviceList[index].SerialNumber;
							info.Description = deviceList[index].Description;
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Open(string name)
		{
			if (SearchDevice(name + " " + device))
			{
				ftStatus = OpenBySerialNumber(info.SerialNumber);
				if (ftStatus == FTDI.FT_STATUS.FT_OK)
				{
					OnOpened();
					return true;
				}
			}
			return false;
		}

		public new bool Close()
		{
			readThreadContinue = false;
			if (IsOpen)
			{
				int num = (int)base.Close();
				OnClosed();
			}
			return true;
		}

		public virtual bool Init(uint frequency)
		{
			throw new NotImplementedException();
		}

		public virtual bool SendBytes()
		{
			throw new NotImplementedException();
		}

		public virtual bool ReadBytes(out byte[] rxBuffer, uint bitCount)
		{
			throw new NotImplementedException();
		}

		public virtual void TxBufferAdd(byte data)
		{
			txBuffer.Add(data);
		}

		public virtual void TxBufferAdd(byte[] data)
		{
			txBuffer.AddRange(Enumerable.AsEnumerable<byte>((IEnumerable<byte>)data));
		}

		protected void ReadThread()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			Close();
		}

		public class IoChangedEventArgs : EventArgs
		{
			private bool m_state;

			public bool Sate
			{
				get
				{
					return m_state;
				}
			}

			public IoChangedEventArgs(bool state)
			{
				m_state = state;
			}
		}
	}
}