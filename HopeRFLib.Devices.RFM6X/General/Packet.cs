using SemtechLib.Devices.SX1231.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SemtechLib.Devices.SX1231.General
{
	public class Packet : INotifyPropertyChanged
	{
		private readonly byte[] PayloadMinLength = new byte[8] { 1, 1, 2, 2, 1, 1, 2, 2 };
		private readonly byte[] PayloadMaxLength = new byte[8] { 66, 66, 66, 66, 64, 65, 65, 50 };
		private readonly byte[] MessageMaxLength = new byte[8] { 66, 65, 65, 64, 64, 64, 64, 48 };
		private readonly byte[] MessageLengthOffset = new byte[8] { 0, 1, 1, 2, 0, 1, 1, 2 };
		private OperatingModeEnum mode = OperatingModeEnum.Stdby;
		private int preambleSize = 3;
		private bool syncOn = true;
		private byte syncSize = (byte)4;
		private byte[] syncValue = new byte[4] { 105, 129, 126, 150 };
		private PacketFormatEnum packetFormat = PacketFormatEnum.Variable;
		private bool crcOn = true;
		private bool crcIbmOn = true;
		private byte payloadLength = (byte)66;
		private byte payloadLengthRx = (byte)66;
		private bool txStartCondition = true;
		private byte fifoThreshold = (byte)15;
		private bool autoRxRestartOn = true;
		private bool aesOn = true;
		private byte[] aesKey = new byte[16];
		private byte[] message = new byte[0];
		private Decimal rssi = new Decimal(1275, 0, 0, true, (byte)1);
		private const ushort PolynomeCcitt = (ushort)4129;
		private const ushort PolynomeIbm = (ushort)32773;
		public const int MaxFifoSize = 66;
		private FifoFillConditionEnum fifoFillCondition;
		private byte syncTol;
		private DcFreeEnum dcFree;
		private bool crcAutoClearOff;
		private AddressFilteringEnum addressFiltering;
		private byte nodeAddress;
		private byte nodeAddressRx;
		private byte broadcastAddress;
		private EnterConditionEnum enterCondition;
		private ExitConditionEnum exitCondition;
		private IntermediateModeEnum intermediateMode;
		private int interPacketRxDelay;
		private bool logEnabled;
		private byte maxLengthIndex;

		public OperatingModeEnum Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		public int PreambleSize
		{
			get
			{
				return this.preambleSize;
			}
			set
			{
				this.preambleSize = value;
				this.OnPropertyChanged("PreambleSize");
			}
		}

		public bool SyncOn
		{
			get
			{
				return this.syncOn;
			}
			set
			{
				this.syncOn = value;
				this.OnPropertyChanged("SyncOn");
			}
		}

		public FifoFillConditionEnum FifoFillCondition
		{
			get
			{
				return this.fifoFillCondition;
			}
			set
			{
				this.fifoFillCondition = value;
				this.OnPropertyChanged("FifoFillCondition");
			}
		}

		public byte SyncSize
		{
			get
			{
				return this.syncSize;
			}
			set
			{
				this.syncSize = value;
				Array.Resize<byte>(ref this.syncValue, (int)this.syncSize);
				this.OnPropertyChanged("SyncSize");
			}
		}

		public byte SyncTol
		{
			get
			{
				return this.syncTol;
			}
			set
			{
				this.syncTol = value;
				this.OnPropertyChanged("SyncTol");
			}
		}

		public byte[] SyncValue
		{
			get
			{
				return this.syncValue;
			}
			set
			{
				this.syncValue = value;
				this.OnPropertyChanged("SyncValue");
			}
		}

		public PacketFormatEnum PacketFormat
		{
			get
			{
				return this.packetFormat;
			}
			set
			{
				this.packetFormat = value;
				this.OnPropertyChanged("PacketFormat");
				this.OnPropertyChanged("Crc");
			}
		}

		public DcFreeEnum DcFree
		{
			get
			{
				return this.dcFree;
			}
			set
			{
				this.dcFree = value;
				this.OnPropertyChanged("DcFree");
			}
		}

		public bool CrcOn
		{
			get
			{
				return this.crcOn;
			}
			set
			{
				this.crcOn = value;
				this.OnPropertyChanged("CrcOn");
				this.OnPropertyChanged("Crc");
			}
		}

		public bool CrcAutoClearOff
		{
			get
			{
				return this.crcAutoClearOff;
			}
			set
			{
				this.crcAutoClearOff = value;
				this.OnPropertyChanged("CrcAutoClearOff");
			}
		}

		public AddressFilteringEnum AddressFiltering
		{
			get
			{
				return this.addressFiltering;
			}
			set
			{
				this.addressFiltering = value;
				this.OnPropertyChanged("AddressFiltering");
				this.OnPropertyChanged("PayloadLength");
				this.OnPropertyChanged("MessageLength");
				this.OnPropertyChanged("Crc");
			}
		}

		public bool CrcIbmOn
		{
			get
			{
				return this.crcIbmOn;
			}
			set
			{
				this.crcIbmOn = value;
				this.OnPropertyChanged("CrcIbmOn");
				this.OnPropertyChanged("Crc");
			}
		}

		public byte PayloadLength
		{
			get
			{
				if (this.Mode == OperatingModeEnum.Rx)
					return this.payloadLengthRx;
				byte num = (byte)0;
				if (this.PacketFormat == PacketFormatEnum.Variable)
					++num;
				if (this.AddressFiltering != AddressFilteringEnum.OFF)
					++num;
				return (byte)((uint)num + (uint)(byte)this.Message.Length);
			}
			set
			{
				if (this.Mode == OperatingModeEnum.Rx)
					this.payloadLengthRx = value;
				else
					this.payloadLength = value;
				this.OnPropertyChanged("PayloadLength");
			}
		}

		public byte NodeAddress
		{
			get
			{
				return this.nodeAddress;
			}
			set
			{
				this.nodeAddress = value;
				this.OnPropertyChanged("NodeAddress");
				this.OnPropertyChanged("Crc");
			}
		}

		public byte NodeAddressRx
		{
			get
			{
				return this.nodeAddressRx;
			}
			set
			{
				this.nodeAddressRx = value;
				this.OnPropertyChanged("NodeAddressRx");
			}
		}

		public byte BroadcastAddress
		{
			get
			{
				return this.broadcastAddress;
			}
			set
			{
				this.broadcastAddress = value;
				this.OnPropertyChanged("BroadcastAddress");
			}
		}

		public EnterConditionEnum EnterCondition
		{
			get
			{
				return this.enterCondition;
			}
			set
			{
				this.enterCondition = value;
				this.OnPropertyChanged("EnterCondition");
			}
		}

		public ExitConditionEnum ExitCondition
		{
			get
			{
				return this.exitCondition;
			}
			set
			{
				this.exitCondition = value;
				this.OnPropertyChanged("ExitCondition");
			}
		}

		public IntermediateModeEnum IntermediateMode
		{
			get
			{
				return this.intermediateMode;
			}
			set
			{
				this.intermediateMode = value;
				this.OnPropertyChanged("IntermediateMode");
			}
		}

		public bool TxStartCondition
		{
			get
			{
				return this.txStartCondition;
			}
			set
			{
				this.txStartCondition = value;
				this.OnPropertyChanged("TxStartCondition");
			}
		}

		public byte FifoThreshold
		{
			get
			{
				return this.fifoThreshold;
			}
			set
			{
				this.fifoThreshold = value;
				this.OnPropertyChanged("FifoThreshold");
			}
		}

		public int InterPacketRxDelay
		{
			get
			{
				return this.interPacketRxDelay;
			}
			set
			{
				this.interPacketRxDelay = value;
				this.OnPropertyChanged("InterPacketRxDelay");
			}
		}

		public bool AutoRxRestartOn
		{
			get
			{
				return this.autoRxRestartOn;
			}
			set
			{
				this.autoRxRestartOn = value;
				this.OnPropertyChanged("AutoRxRestartOn");
			}
		}

		public bool AesOn
		{
			get
			{
				return this.aesOn;
			}
			set
			{
				this.aesOn = value;
				this.OnPropertyChanged("AesOn");
			}
		}

		public byte[] AesKey
		{
			get
			{
				return this.aesKey;
			}
			set
			{
				this.aesKey = value;
				this.OnPropertyChanged("AesKey");
			}
		}

		public byte MessageLength
		{
			get
			{
				byte num = (byte)0;
				if (this.AddressFiltering != AddressFilteringEnum.OFF)
					++num;
				return (byte)((uint)num + (uint)(byte)this.Message.Length);
			}
		}

		public byte[] Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
				this.OnPropertyChanged("Message");
				this.OnPropertyChanged("PayloadLength");
				this.OnPropertyChanged("MessageLength");
				this.OnPropertyChanged("Crc");
			}
		}

		public ushort Crc
		{
			get
			{
				byte[] array = new byte[0];
				int num1 = 0;
				int num2 = 0;
				if (this.PacketFormat == PacketFormatEnum.Variable)
				{
					Array.Resize<byte>(ref array, ++num1);
					array[num2++] = this.MessageLength;
				}
				if (this.AddressFiltering != AddressFilteringEnum.OFF)
				{
					Array.Resize<byte>(ref array, ++num1);
					array[num2++] = this.Mode != OperatingModeEnum.Rx ? this.NodeAddress : this.NodeAddressRx;
				}
				int newSize = num1 + this.Message.Length;
				Array.Resize<byte>(ref array, newSize);
				for (int index = 0; index < this.Message.Length; ++index)
					array[num2 + index] = this.Message[index];
				return this.ComputeCrc(array);
			}
		}

		public Decimal Rssi
		{
			get
			{
				return this.rssi;
			}
			set
			{
				this.rssi = value;
				this.OnPropertyChanged("Rssi");
			}
		}

		public bool LogEnabled
		{
			get
			{
				return this.logEnabled;
			}
			set
			{
				this.logEnabled = value;
				this.OnPropertyChanged("LogEnabled");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void UpdatePayloadLengthMaxMin()
		{
			this.maxLengthIndex = this.AesOn ? (byte)4 : (byte)0;
			this.maxLengthIndex |= this.PacketFormat == PacketFormatEnum.Variable ? (byte)2 : (byte)0;
			this.maxLengthIndex |= this.AddressFiltering != AddressFilteringEnum.OFF ? (byte)1 : (byte)0;
			if (this.Message.Length <= (int)this.MessageMaxLength[(int)this.maxLengthIndex])
				return;
			Array.Resize<byte>(ref this.message, (int)this.MessageMaxLength[(int)this.maxLengthIndex]);
			this.OnPropertyChanged("Message");
			this.OnPropertyChanged("MessageLength");
			this.OnPropertyChanged("PayloadLength");
			this.OnPropertyChanged("Crc");
		}

		private ushort ComputeCcittCrc(ushort crc, byte data)
		{
			for (int index = 0; index < 8; ++index)
			{
				if ((((int)crc & 32768) >> 8 ^ (int)data & 128) != 0)
				{
					crc <<= 1;
					crc ^= (ushort)4129;
				}
				else
					crc <<= 1;
				data <<= 1;
			}
			return crc;
		}

		private ushort ComputeIbmCrc(ushort crc, byte data)
		{
			for (int index = 0; index < 8; ++index)
			{
				if ((((int)crc & 32768) >> 8 ^ (int)data & 128) != 0)
				{
					crc <<= 1;
					crc ^= (ushort)32773;
				}
				else
					crc <<= 1;
				data <<= 1;
			}
			return crc;
		}

		public ushort ComputeCrc(byte[] packet)
		{
			ushort crc = !this.crcIbmOn ? (ushort)7439 : ushort.MaxValue;
			for (int index = 0; index < packet.Length; ++index)
				crc = !this.crcIbmOn ? this.ComputeCcittCrc(crc, packet[index]) : this.ComputeIbmCrc(crc, packet[index]);
			if (this.crcIbmOn)
				return crc;
			else
				return (ushort)(~crc);
		}

		public byte[] ToArray()
		{
			List<byte> list = new List<byte>();
			if (this.PacketFormat == PacketFormatEnum.Variable)
				list.Add(this.MessageLength);
			if (this.AddressFiltering != AddressFilteringEnum.OFF)
				list.Add(this.NodeAddress);
			for (int index = 0; index < this.Message.Length; ++index)
				list.Add(this.Message[index]);
			return list.ToArray();
		}

		public void SetSaveData(string data)
		{
			string[] strArray1 = data.Split(new char[1]
      {
        ';'
      });
			if (strArray1.Length != 5)
				return;
			string[] strArray2 = strArray1[4].Split(new char[1]
      {
        ','
      });
			if (this.message != null)
				Array.Resize<byte>(ref this.message, strArray2.Length);
			else
				this.message = new byte[strArray2.Length];
			for (int index = 0; index < strArray2.Length; ++index)
			{
				if (strArray2[index].Length != 0)
					this.message[index] = Convert.ToByte(strArray2[index], 16);
			}
			this.OnPropertyChanged("Message");
			this.OnPropertyChanged("MessageLength");
			this.OnPropertyChanged("PayloadLength");
			this.OnPropertyChanged("Crc");
			this.UpdatePayloadLengthMaxMin();
		}

		public string GetSaveData()
		{
			string str = (this.Mode == OperatingModeEnum.Tx ? true.ToString() : false.ToString()) + ";" + (this.AddressFiltering != AddressFilteringEnum.OFF ? true.ToString() : false.ToString()) + ";" + this.payloadLength.ToString() + ";" + this.nodeAddress.ToString() + ";";
			if (this.message != null && this.message.Length != 0)
			{
				int index;
				for (index = 0; index < this.message.Length - 1; ++index)
					str = str + this.message[index].ToString("X02") + ",";
				str = str + this.message[index].ToString("X02");
			}
			return str;
		}

		private void OnPropertyChanged(string propName)
		{
			if (this.PropertyChanged == null)
				return;
			this.PropertyChanged((object)this, new PropertyChangedEventArgs(propName));
		}
	}
}