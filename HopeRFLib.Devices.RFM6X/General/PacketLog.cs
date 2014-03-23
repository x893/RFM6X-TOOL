using SemtechLib.Devices.SX1231;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Events;
using SemtechLib.General.Events;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace SemtechLib.Devices.SX1231.General
{
	public class PacketLog : INotifyPropertyChanged
	{
		private string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		private string fileName = "sx1231-pkt.log";
		private ulong maxSamples = 1000UL;
		private CultureInfo ci = CultureInfo.InvariantCulture;
		private FileStream fileStream;
		private StreamWriter streamWriter;
		private bool state;
		private ulong samples;
		private int packetNumber;
		private int maxPacketNumber;
		private SX1231 sx1231;

		public SX1231 SX1231
		{
			set
			{
				if (this.sx1231 == value)
					return;
				this.sx1231 = value;
				this.sx1231.PropertyChanged += new PropertyChangedEventHandler(this.sx1231_PropertyChanged);
				this.sx1231.PacketHandlerStarted += new EventHandler(this.sx1231_PacketHandlerStarted);
				this.sx1231.PacketHandlerStoped += new EventHandler(this.sx1231_PacketHandlerStoped);
				this.sx1231.PacketHandlerTransmitted += new SX1231.PacketHandlerTransmittedEventHandler(this.sx1231_PacketHandlerTransmitted);
				this.sx1231.PacketHandlerReceived += new SX1231.PacketHandlerReceivedEventHandler(this.sx1231_PacketHandlerReceived);
			}
		}

		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
				this.OnPropertyChanged("Path");
			}
		}

		public string FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;
				this.OnPropertyChanged("FileName");
			}
		}

		public ulong MaxSamples
		{
			get
			{
				return this.maxSamples;
			}
			set
			{
				this.maxSamples = value;
				this.OnPropertyChanged("MaxSamples");
			}
		}

		public event ProgressEventHandler ProgressChanged;

		public event EventHandler Stoped;

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnProgressChanged(ulong progress)
		{
			if (this.ProgressChanged == null)
				return;
			this.ProgressChanged((object)this, new ProgressEventArg(progress));
		}

		private void OnStop()
		{
			if (this.Stoped == null)
				return;
			this.Stoped((object)this, EventArgs.Empty);
		}

		private void GenerateFileHeader()
		{
			string str = "#\tTime\tMode\tRssi\tPkt Max\tPkt #\tPreamble Size\tSync\tLength\tNode Address\tMessage\tCRC";
			this.streamWriter.WriteLine("#\tSX1231 packet log generated the " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString());
			this.streamWriter.WriteLine(str);
		}

		private void Update()
		{
			string str1 = "\t";
			if (this.sx1231 == null || !this.state)
				return;
			if (this.samples < this.maxSamples || (long)this.maxSamples == 0L)
			{
				string str2 = str1 + DateTime.Now.ToString("HH:mm:ss.fff", (IFormatProvider)this.ci) + "\t" + (this.sx1231.Mode == OperatingModeEnum.Tx ? "Tx\t" : (this.sx1231.Mode == OperatingModeEnum.Rx ? "Rx\t" : "\t")) + (this.sx1231.Mode == OperatingModeEnum.Rx ? this.sx1231.Packet.Rssi.ToString("F1") + "\t" : "\t") + this.maxPacketNumber.ToString() + "\t" + this.packetNumber.ToString() + "\t" + this.sx1231.Packet.PreambleSize.ToString() + "\t" + new MaskValidationType(this.sx1231.Packet.SyncValue).StringValue + "\t" + this.sx1231.Packet.MessageLength.ToString("X02") + "\t";
				string str3 = (this.sx1231.Mode != OperatingModeEnum.Rx ? str2 + (this.sx1231.Packet.AddressFiltering != AddressFilteringEnum.OFF ? this.sx1231.Packet.NodeAddress.ToString("X02") : "") : str2 + (this.sx1231.Packet.AddressFiltering != AddressFilteringEnum.OFF ? this.sx1231.Packet.NodeAddressRx.ToString("X02") : "")) + "\t";
				if (this.sx1231.Packet.Message != null && this.sx1231.Packet.Message.Length != 0)
				{
					int index;
					for (index = 0; index < this.sx1231.Packet.Message.Length - 1; ++index)
						str3 = str3 + this.sx1231.Packet.Message[index].ToString("X02") + "-";
					str3 = str3 + this.sx1231.Packet.Message[index].ToString("X02") + "\t";
				}
				this.streamWriter.WriteLine(str3 + (this.sx1231.Packet.CrcOn ? ((int)this.sx1231.Packet.Crc >> 8).ToString("X02") + "-" + ((int)this.sx1231.Packet.Crc & (int)byte.MaxValue).ToString("X02") + "\t" : "\t"));
				if ((long)this.maxSamples != 0L)
				{
					++this.samples;
					this.OnProgressChanged((ulong)((Decimal)this.samples * new Decimal(100) / (Decimal)this.maxSamples));
				}
				else
					this.OnProgressChanged(0UL);
			}
			else
				this.OnStop();
		}

		public void Start()
		{
			try
			{
				this.fileStream = new FileStream(this.path + "\\" + this.fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
				this.streamWriter = new StreamWriter((Stream)this.fileStream, Encoding.ASCII);
				this.GenerateFileHeader();
				this.samples = 0UL;
				this.state = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Stop()
		{
			try
			{
				this.state = false;
				this.streamWriter.Close();
			}
			catch (Exception)
			{
			}
		}

		private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName;
			if ((propertyName = e.PropertyName) == null)
				return;
			int num = propertyName == "RssiValue" ? 1 : 0;
		}

		private void sx1231_PacketHandlerStarted(object sender, EventArgs e)
		{
		}

		private void sx1231_PacketHandlerStoped(object sender, EventArgs e)
		{
		}

		private void sx1231_PacketHandlerTransmitted(object sender, PacketStatusEventArg e)
		{
			this.maxPacketNumber = e.Max;
			this.packetNumber = e.Number;
			this.Update();
		}

		private void sx1231_PacketHandlerReceived(object sender, PacketStatusEventArg e)
		{
			this.maxPacketNumber = e.Max;
			this.packetNumber = e.Number;
			this.Update();
		}

		private void OnPropertyChanged(string propName)
		{
			if (this.PropertyChanged == null)
				return;
			this.PropertyChanged((object)this, new PropertyChangedEventArgs(propName));
		}

		public enum PacketHandlerModeEnum
		{
			IDLE,
			RX,
			TX,
		}
	}
}