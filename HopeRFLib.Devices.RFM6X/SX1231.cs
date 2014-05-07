using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Events;
using SemtechLib.Devices.SX1231.General;
using SemtechLib.Ftdi;
using SemtechLib.General;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231
{
	public class SX1231 : INotifyPropertyChanged, IDisposable
	{
		public delegate void ErrorEventHandler(object sender, SemtechLib.General.Events.ErrorEventArgs e);
		public delegate void LimitCheckStatusChangedEventHandler(object sender, LimitCheckStatusEventArg e);
		public delegate void PacketHandlerTransmittedEventHandler(object sender, PacketStatusEventArg e);
		public delegate void PacketHandlerReceivedEventHandler(object sender, PacketStatusEventArg e);

		public event EventHandler Connected;
		public event EventHandler Disconected;
		public event SX1231.ErrorEventHandler Error;
		public event SX1231.LimitCheckStatusChangedEventHandler FrequencyRfLimitStatusChanged;
		public event SX1231.LimitCheckStatusChangedEventHandler BitRateLimitStatusChanged;
		public event SX1231.LimitCheckStatusChangedEventHandler FdevLimitStatusChanged;
		public event SX1231.LimitCheckStatusChangedEventHandler SyncValueLimitChanged;
		public event EventHandler PacketHandlerStarted;
		public event EventHandler PacketHandlerStoped;
		public event SX1231.PacketHandlerTransmittedEventHandler PacketHandlerTransmitted;
		public event SX1231.PacketHandlerReceivedEventHandler PacketHandlerReceived;
		public event PropertyChangedEventHandler PropertyChanged;

		protected object syncThread = new object();
		private uint spiSpeed = 2000000;
		private Decimal frequencyXo = new Decimal(32000000);
		private Decimal frequencyStep = new Decimal(32000000) / (Decimal)Math.Pow(2.0, 19.0);
		private bool monitor = true;
		private Decimal spectrumFreqSpan = new Decimal(1000000);
		private Decimal spectrumRssiValue = new Decimal(1275, 0, 0, true, (byte)1);
		private bool sequencer = true;
		private OperatingModeEnum mode = OperatingModeEnum.Stdby;
		private Decimal bitRate = new Decimal(4800);
		private Decimal fdev = new Decimal(5000);
		private Decimal frequencyRf = new Decimal(915000000);
		private LowBatTrimEnum lowBatTrim = LowBatTrimEnum.Trim1_835;
		private ListenResolEnum listenResolIdle = ListenResolEnum.Res004100;
		private ListenResolEnum listenResolRx = ListenResolEnum.Res004100;
		private ListenEndEnum listenEnd = ListenEndEnum.RxMode;
		private Decimal listenCoefIdle = new Decimal(10045, 0, 0, false, (byte)1);
		private Decimal listenCoefRx = new Decimal(1312, 0, 0, false, (byte)1);
		private Version version = new Version(0, 0);
		private Decimal outputPower = new Decimal(130, 0, 0, false, (byte)1);
		private PaRampEnum paRamp = PaRampEnum.PaRamp_40;
		private bool ocpOn = true;
		private Decimal ocpTrim = new Decimal(100);
		private bool rssiAutoThresh = true;
		private bool agcAutoRefOn = true;
		private int agcRefLevel = -80;
		private byte agcSnrMargin = (byte)5;
		private byte agcStep1 = (byte)16;
		private byte agcStep2 = (byte)7;
		private byte agcStep3 = (byte)11;
		private byte agcStep4 = (byte)9;
		private byte agcStep5 = (byte)11;
		private LnaZinEnum lnaZin = LnaZinEnum.ZIN_200;
		private bool lnaLowPowerOn = true;
		private LnaGainEnum lnaCurrentGain = LnaGainEnum.G1;
		private LnaGainEnum lnaGainSelect = LnaGainEnum.G1;
		private Decimal dccFreq = new Decimal(414);
		private Decimal rxBw = new Decimal(5208);
		private Decimal afcDccFreq = new Decimal(497);
		private Decimal afcRxBw = new Decimal(25000);
		private OokThreshTypeEnum ookThreshType = OokThreshTypeEnum.Peak;
		private Decimal ookPeakThreshStep = new Decimal(5, 0, 0, false, (byte)1);
		private Decimal[] ookPeakThreshStepTable = new Decimal[8]
    {
      new Decimal(5, 0, 0, false, (byte) 1),
      new Decimal(10, 0, 0, false, (byte) 1),
      new Decimal(15, 0, 0, false, (byte) 1),
      new Decimal(20, 0, 0, false, (byte) 1),
      new Decimal(30, 0, 0, false, (byte) 1),
      new Decimal(40, 0, 0, false, (byte) 1),
      new Decimal(50, 0, 0, false, (byte) 1),
      new Decimal(60, 0, 0, false, (byte) 1)
    };
		private OokAverageThreshFiltEnum ookAverageThreshFilt = OokAverageThreshFiltEnum.COEF_2;
		private byte ookFixedThresh = (byte)6;
		private Decimal afcValue = new Decimal(0, 0, 0, false, (byte)1);
		private Decimal feiValue = new Decimal(0, 0, 0, false, (byte)1);
		private Decimal rssiValue = new Decimal(1275, 0, 0, true, (byte)1);
		private Decimal prevRssiValue = new Decimal(1275, 0, 0, true, (byte)1);
		private ClockOutEnum clockOut = ClockOutEnum.CLOCK_OUT_111;
		private Decimal rssiThresh = new Decimal(-114);
		private Decimal tempValue = new Decimal(1650, 0, 0, false, (byte)1);
		private Decimal tempValueRoom = new Decimal(250, 0, 0, false, (byte)1);
		private Decimal tempValueCal = new Decimal(1650, 0, 0, false, (byte)1);
		private Decimal rfPaRssiValue = new Decimal(1275, 0, 0, true, (byte)1);
		private Decimal rfIoRssiValue = new Decimal(1275, 0, 0, true, (byte)1);
		private byte[] FifoData = new byte[66];
		private bool prevOcpOn = true;
		private Decimal prevOcpTrim = new Decimal(100);
		private const int FR_BAND_1_MAX = 340000000;
		private const int FR_BAND_1_MIN = 290000000;
		private const int FR_BAND_2_MAX = 510000000;
		private const int FR_BAND_2_MIN = 431000000;
		private const int FR_BAND_3_MAX = 1020000000;
		private const int FR_BAND_3_MIN = 862000000;
		private const int BRF_MAX = 300000;
		private const int BRO_MAX = 32768;
		private const int BR_MIN = 600;
		private const int FDA_MAX = 300000;
		private const int FDA_MIN = 600;
		private const int BW_SSB_MAX = 500000;
		private const int NOISE_ABSOLUTE_ZERO = -174;
		private const int NOISE_FIGURE = 7;
		private const int DEMOD_SNR = 8;
		protected bool regUpdateThreadContinue;
		protected Thread regUpdateThread;
		protected int readLock;
		protected int writeLock;
		protected bool restartRx;
		protected bool bitRateFdevCheckDisbale;
		protected bool frequencyRfCheckDisable;
		private FtdiDevice ftdi;
		private string deviceName;
		private bool isOpen;
		private RegisterCollection registers;
		private bool test;
		private OperatingModeEnum prevMode;
		private LnaGainEnum prevLnaGainSelect;
		private ModulationTypeEnum prevModulationType;
		private bool prevRssiAutoThresh;
		private Decimal prevRssiThresh;
		private bool prevMonitorOn;
		private bool spectrumOn;
		private int spectrumFreqId;
		private bool listenMode;
		private DataModeEnum dataMode;
		private ModulationTypeEnum modulationType;
		private byte modulationShaping;
		private bool rcCalDone;
		private bool afcLowBetaOn;
		private bool lowBatMonitor;
		private bool lowBatOn;
		private ListenCriteriaEnum listenCriteria;
		private PaModeEnum paMode;
		private OokPeakThreshDecEnum ookPeakThreshDec;
		private bool feiDone;
		private bool afcDone;
		private bool afcAutoClearOn;
		private bool afcAutoOn;
		private bool fastRx;
		private bool rssiDone;
		private DioMappingEnum dio0Mapping;
		private DioMappingEnum dio1Mapping;
		private DioMappingEnum dio2Mapping;
		private DioMappingEnum dio3Mapping;
		private DioMappingEnum dio4Mapping;
		private DioMappingEnum dio5Mapping;
		private bool modeReady;
		private bool rxReady;
		private bool txReady;
		private bool pllLock;
		private bool rssi;
		private bool timeout;
		private bool autoMode;
		private bool syncAddressMatch;
		private bool fifoFull;
		private bool fifoNotEmpty;
		private bool fifoLevel;
		private bool fifoOverrun;
		private bool packetSent;
		private bool payloadReady;
		private bool crcOk;
		private bool lowBat;
		private Decimal timeoutRxStart;
		private Decimal timeoutRssiThresh;
		private Packet packet;
		private bool tempMeasRunning;
		private bool adcLowPowerOn;
		private bool tempCalDone;
		private bool pa20dBm;
		private bool sensitivityBoostOn;
		private Decimal lowBetaAfcOffset;
		private bool dagcOn;
		private RfPaSwitchSelEnum rfPaSwitchSel;
		private RfPaSwitchSelEnum prevRfPaSwitchSel;
		private int prevRfPaSwitchEnabled;
		private int rfPaSwitchEnabled;
		private int packetNumber;
		private int maxPacketNumber;
		private bool isPacketModeRunning;
		private bool frameTransmitted;
		private bool frameReceived;
		private bool firstTransmit;

		public SX1231()
		{
			PropertyChanged += new PropertyChangedEventHandler(SX1231_PropertyChanged);
			ftdi = new FtdiDevice(FtdiDevice.MpsseProtocol.SPI);
			ftdi.Opened += new EventHandler(ftdi_Opened);
			ftdi.Closed += new EventHandler(ftdi_Closed);
			ftdi.PortB.Io0Changed += new FtdiIoPort.IoChangedEventHandler(sx1231_Dio0Changed);
			ftdi.PortB.Io1Changed += new FtdiIoPort.IoChangedEventHandler(sx1231_Dio1Changed);
			ftdi.PortB.Io2Changed += new FtdiIoPort.IoChangedEventHandler(sx1231_Dio3Changed);
			ftdi.PortB.Io3Changed += new FtdiIoPort.IoChangedEventHandler(sx1231_Dio5Changed);
			ftdi.PortB.Io4Changed += new FtdiIoPort.IoChangedEventHandler(sx1231_Dio2Changed);
			ftdi.PortB.Io5Changed += new FtdiIoPort.IoChangedEventHandler(sx1231_Dio1Changed);
			ftdi.PortA.Io7Changed += new FtdiIoPort.IoChangedEventHandler(sx1231_Dio4Changed);
			PopulateRegisters();
		}

		public string DeviceName
		{
			get { return deviceName; }
		}

		public bool IsOpen
		{
			get
			{
				return isOpen;
			}
			set
			{
				isOpen = value;
				OnPropertyChanged("IsOpen");
			}
		}

		public int SPISpeed
		{
			get
			{
				return (int)spiSpeed;
			}
			set
			{
				spiSpeed = (uint)value;
				OnPropertyChanged("SPISpeed");
			}
		}

		public RegisterCollection Registers
		{
			get
			{
				return registers;
			}
			set
			{
				registers = value;
				OnPropertyChanged("Registers");
			}
		}

		public bool Test
		{
			get { return test; }
			set { test = value; }
		}

		public Decimal FrequencyXo
		{
			get			{				return frequencyXo;			}
			set
			{
				frequencyXo = value;
				FrequencyStep = frequencyXo / (Decimal)Math.Pow(2.0, 19.0);
				FrequencyRf = (Decimal)(registers["RegFrfMsb"].Value << 16 | registers["RegFrfMid"].Value << 8 | registers["RegFrfLsb"].Value) * FrequencyStep;
				Fdev = (Decimal)(registers["RegFdevMsb"].Value << 8 | registers["RegFdevLsb"].Value) * FrequencyStep;
				BitRate = frequencyXo / (Decimal)(registers["RegBitrateMsb"].Value << 8 | registers["RegBitrateLsb"].Value);
				OnPropertyChanged("FrequencyXo");
				int mant1;
				switch ((registers["RegRxBw"].Value & 24U) >> 3)
				{
					case 0U:
						mant1 = 16;
						break;
					case 1U:
						mant1 = 20;
						break;
					case 2U:
						mant1 = 24;
						break;
					default:
						throw new Exception("Invalid RxBwMant parameter");
				}
				RxBw = SX1231.ComputeRxBw(value, modulationType, mant1, (int)registers["RegRxBw"].Value & 7);
				int mant2;
				switch ((registers["RegAfcBw"].Value & 24U) >> 3)
				{
					case 0U:
						mant2 = 16;
						break;
					case 1U:
						mant2 = 20;
						break;
					case 2U:
						mant2 = 24;
						break;
					default:
						throw new Exception("Invalid RxBwMant parameter");
				}
				AfcRxBw = SX1231.ComputeRxBw(value, modulationType, mant2, (int)registers["RegAfcBw"].Value & 7);
				DccFreq = ComputeDccFreq(RxBw, registers["RegRxBw"].Value);
				AfcDccFreq = ComputeDccFreq(AfcRxBw, registers["RegAfcBw"].Value);
			}
		}

		public Decimal FrequencyStep
		{
			get
			{
				return frequencyStep;
			}
			set
			{
				frequencyStep = value;
				OnPropertyChanged("FrequencyStep");
			}
		}

		public Decimal Tbit
		{
			get { return new Decimal(1) / BitRate; }
		}

		public bool Monitor
		{
			get
			{
				lock (syncThread)
					return monitor;
			}
			set
			{
				lock (syncThread)
				{
					monitor = value;
					OnPropertyChanged("Monitor");
				}
			}
		}

		public bool SpectrumOn
		{
			get { return spectrumOn; }
			set
			{
				spectrumOn = value;
				if (spectrumOn)
				{
					RfPaSwitchEnabled = 0;
					prevRssiAutoThresh = RssiAutoThresh;
					RssiAutoThresh = false;
					prevRssiThresh = RssiThresh;
					SetRssiThresh(new Decimal(1275, 0, 0, true, (byte)1));
					prevModulationType = ModulationType;
					SetModulationType(ModulationTypeEnum.OOK);
					prevLnaGainSelect = LnaGainSelect;
					SetLnaGainSelect(LnaGainEnum.G1);
					prevMode = Mode;
					SetOperatingMode(OperatingModeEnum.Rx);
					prevMonitorOn = Monitor;
					Monitor = true;
				}
				else
				{
					SetFrequencyRf(FrequencyRf);
					RfPaSwitchEnabled = prevRfPaSwitchEnabled;
					RssiAutoThresh = prevRssiAutoThresh;
					SetRssiThresh(prevRssiThresh);
					SetModulationType(prevModulationType);
					SetLnaGainSelect(prevLnaGainSelect);
					SetOperatingMode(prevMode);
					Monitor = prevMonitorOn;
				}
				OnPropertyChanged("SpectrumOn");
			}
		}

		public Decimal SpectrumFrequencySpan
		{
			get
			{
				return spectrumFreqSpan;
			}
			set
			{
				spectrumFreqSpan = value;
				OnPropertyChanged("SpectrumFreqSpan");
			}
		}

		public Decimal SpectrumFrequencyMax
		{
			get { return FrequencyRf + SpectrumFrequencySpan / new Decimal(20, 0, 0, false, 1); }
		}

		public Decimal SpectrumFrequencyMin
		{
			get { return FrequencyRf - SpectrumFrequencySpan / new Decimal(20, 0, 0, false, 1); }
		}

		public int SpectrumNbFrequenciesMax
		{
			get { return (int)((SpectrumFrequencyMax - SpectrumFrequencyMin) / SpectrumFrequencyStep); }
		}

		public Decimal SpectrumFrequencyStep
		{
			get { return RxBw / new Decimal(30, 0, 0, false, 1); }
		}

		public int SpectrumFrequencyId
		{
			get
			{
				return spectrumFreqId;
			}
			set
			{
				spectrumFreqId = value;
				OnPropertyChanged("SpectrumFreqId");
			}
		}

		public Decimal SpectrumRssiValue
		{
			get { return spectrumRssiValue; }
		}

		public bool Sequencer
		{
			get { return sequencer; }
			set
			{
				sequencer = value;
				OnPropertyChanged("Sequencer");
			}
		}

		public bool ListenMode
		{
			get { return listenMode; }
			set
			{
				listenMode = value;
				OnPropertyChanged("Listen");
			}
		}

		public OperatingModeEnum Mode
		{
			get { return mode; }
			set
			{
				mode = value;
				OnPropertyChanged("Mode");
			}
		}

		public DataModeEnum DataMode
		{
			get { return dataMode; }
			set
			{
				dataMode = value;
				OnPropertyChanged("DataMode");
			}
		}

		public ModulationTypeEnum ModulationType
		{
			get { return modulationType; }
			set
			{
				modulationType = value;
				OnPropertyChanged("ModulationType");
			}
		}

		public byte ModulationShaping
		{
			get { return modulationShaping; }
			set
			{
				modulationShaping = value;
				OnPropertyChanged("ModulationShaping");
			}
		}

		public Decimal BitRate
		{
			get { return bitRate; }
			set
			{
				bitRate = value;
				BitRateFdevCheck(value, fdev);
				OnPropertyChanged("BitRate");
			}
		}

		public Decimal Fdev
		{
			get { return fdev; }
			set
			{
				fdev = value;
				BitRateFdevCheck(bitRate, value);
				OnPropertyChanged("Fdev");
			}
		}

		public Decimal FrequencyRf
		{
			get { return frequencyRf; }
			set
			{
				frequencyRf = value;
				OnPropertyChanged("FrequencyRf");
				FrequencyRfCheck(value);
			}
		}

		public bool RcCalDone
		{
			get { return rcCalDone; }
		}

		public bool AfcLowBetaOn
		{
			get { return afcLowBetaOn; }
			set
			{
				afcLowBetaOn = value;
				OnPropertyChanged("AfcLowBetaOn");
			}
		}

		public bool LowBatMonitor
		{
			get { return lowBatMonitor; }
		}

		public bool LowBatOn
		{
			get { return lowBatOn; }
			set
			{
				lowBatOn = value;
				OnPropertyChanged("LowBatOn");
			}
		}

		public LowBatTrimEnum LowBatTrim
		{
			get { return lowBatTrim; }
			set
			{
				lowBatTrim = value;
				OnPropertyChanged("LowBatTrim");
			}
		}

		public ListenResolEnum ListenResolIdle
		{
			get { return listenResolIdle; }
			set
			{
				listenResolIdle = value;
				OnPropertyChanged("ListenResolIdle");
				switch (value)
				{
					case ListenResolEnum.Res000064:
						ListenCoefIdle = (Decimal)registers["RegListen2"].Value * new Decimal(64, 0, 0, false, 3);
						break;
					case ListenResolEnum.Res004100:
						ListenCoefIdle = (Decimal)registers["RegListen2"].Value * new Decimal(41, 0, 0, false, 1);
						break;
					case ListenResolEnum.Res262000:
						ListenCoefIdle = (Decimal)registers["RegListen2"].Value * new Decimal(262);
						break;
				}
			}
		}

		public ListenResolEnum ListenResolRx
		{
			get { return listenResolRx; }
			set
			{
				listenResolRx = value;
				OnPropertyChanged("ListenResolRx");
				switch (value)
				{
					case ListenResolEnum.Res000064:
						ListenCoefRx = (Decimal)registers["RegListen3"].Value * new Decimal(64, 0, 0, false, 3);
						break;
					case ListenResolEnum.Res004100:
						ListenCoefRx = (Decimal)registers["RegListen3"].Value * new Decimal(41, 0, 0, false, 1);
						break;
					case ListenResolEnum.Res262000:
						ListenCoefRx = (Decimal)registers["RegListen3"].Value * new Decimal(262);
						break;
				}
			}
		}

		public ListenCriteriaEnum ListenCriteria
		{
			get { return listenCriteria; }
			set
			{
				listenCriteria = value;
				OnPropertyChanged("ListenCriteria");
			}
		}

		public ListenEndEnum ListenEnd
		{
			get { return listenEnd; }
			set
			{
				listenEnd = value;
				OnPropertyChanged("ListenEnd");
			}
		}

		public Decimal ListenCoefIdle
		{
			get { return listenCoefIdle; }
			set
			{
				listenCoefIdle = value;
				OnPropertyChanged("ListenCoefIdle");
			}
		}

		public Decimal ListenCoefRx
		{
			get { return listenCoefRx; }
			set
			{
				listenCoefRx = value;
				OnPropertyChanged("ListenCoefRx");
			}
		}

		public Version Version
		{
			get { return version; }
			set
			{
				if (!(version != value))
					return;
				version = value;
				OnPropertyChanged("Version");
			}
		}

		public PaModeEnum PaMode
		{
			get { return paMode; }
			set
			{
				paMode = value;
				OnPropertyChanged("PaMode");
			}
		}

		public Decimal OutputPower
		{
			get { return outputPower; }
			set
			{
				outputPower = value;
				OnPropertyChanged("OutputPower");
			}
		}

		public PaRampEnum PaRamp
		{
			get { return paRamp; }
			set
			{
				paRamp = value;
				OnPropertyChanged("PaRamp");
			}
		}

		public bool OcpOn
		{
			get { return ocpOn; }
			set
			{
				ocpOn = value;
				OnPropertyChanged("OcpOn");
			}
		}

		public Decimal OcpTrim
		{
			get { return ocpTrim; }
			set
			{
				ocpTrim = value;
				OnPropertyChanged("OcpTrim");
			}
		}

		public bool RssiAutoThresh
		{
			get { return rssiAutoThresh; }
			set
			{
				rssiAutoThresh = value;
				OnPropertyChanged("RssiAutoThresh");
			}
		}

		public int AgcReference
		{
			get
			{
				if (agcAutoRefOn)
					return (int)Math.Round(10.0 * Math.Log10((double)(new Decimal(2) * RxBw)) - 159.0 + (double)AgcSnrMargin, MidpointRounding.AwayFromZero);
				else
					return AgcRefLevel;
			}
		}

		public int AgcThresh1
		{
			get { return AgcReference + (int)agcStep1; }
		}

		public int AgcThresh2
		{
			get { return AgcThresh1 + (int)agcStep2; }
		}

		public int AgcThresh3
		{
			get { return AgcThresh2 + (int)agcStep3; }
		}

		public int AgcThresh4
		{
			get { return AgcThresh3 + (int)agcStep4; }
		}

		public int AgcThresh5
		{
			get { return AgcThresh4 + (int)agcStep5; }
		}

		public Decimal DccFreqMin
		{
			get
			{
				return new Decimal(40, 0, 0, false, 1) * RxBw / new Decimal(340449852, 1462918, 0, false, 15) * (Decimal)Math.Pow(2.0, 9.0);
			}
		}

		public Decimal DccFreqMax
		{
			get
			{
				return new Decimal(40, 0, 0, false, 1) * RxBw / new Decimal(340449852, 1462918, 0, false, 15) * (Decimal)Math.Pow(2.0, 2.0);
			}
		}

		public Decimal RxBwMin
		{
			get
			{
				return ComputeRxBwMin();
			}
		}

		public Decimal RxBwMax
		{
			get
			{
				return ComputeRxBwMax();
			}
		}

		public Decimal AfcDccFreqMin
		{
			get
			{
				return new Decimal(40, 0, 0, false, (byte)1) * AfcRxBw / new Decimal(340449852, 1462918, 0, false, 15) * (Decimal)Math.Pow(2.0, 9.0);
			}
		}

		public Decimal AfcDccFreqMax
		{
			get
			{
				return new Decimal(40, 0, 0, false, (byte)1) * AfcRxBw / new Decimal(340449852, 1462918, 0, false, 15) * (Decimal)Math.Pow(2.0, 2.0);
			}
		}

		public Decimal AfcRxBwMin
		{
			get
			{
				return ComputeRxBwMin();
			}
		}

		public Decimal AfcRxBwMax
		{
			get
			{
				return ComputeRxBwMax();
			}
		}

		public bool AgcAutoRefOn
		{
			get
			{
				return agcAutoRefOn;
			}
			set
			{
				agcAutoRefOn = value;
				OnPropertyChanged("AgcAutoRefOn");
				OnPropertyChanged("AgcReference");
				OnPropertyChanged("AgcThresh1");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public int AgcRefLevel
		{
			get
			{
				return agcRefLevel;
			}
			set
			{
				agcRefLevel = value;
				OnPropertyChanged("AgcRefLevel");
				OnPropertyChanged("AgcReference");
				OnPropertyChanged("AgcThresh1");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcSnrMargin
		{
			get
			{
				return agcSnrMargin;
			}
			set
			{
				agcSnrMargin = value;
				OnPropertyChanged("AgcSnrMargin");
				OnPropertyChanged("AgcReference");
				OnPropertyChanged("AgcThresh1");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep1
		{
			get
			{
				return agcStep1;
			}
			set
			{
				agcStep1 = value;
				OnPropertyChanged("AgcStep1");
				OnPropertyChanged("AgcThresh1");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep2
		{
			get
			{
				return agcStep2;
			}
			set
			{
				agcStep2 = value;
				OnPropertyChanged("AgcStep2");
				OnPropertyChanged("AgcThresh2");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep3
		{
			get
			{
				return agcStep3;
			}
			set
			{
				agcStep3 = value;
				OnPropertyChanged("AgcStep3");
				OnPropertyChanged("AgcThresh3");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep4
		{
			get
			{
				return agcStep4;
			}
			set
			{
				agcStep4 = value;
				OnPropertyChanged("AgcStep4");
				OnPropertyChanged("AgcThresh4");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public byte AgcStep5
		{
			get
			{
				return agcStep5;
			}
			set
			{
				agcStep5 = value;
				OnPropertyChanged("AgcStep5");
				OnPropertyChanged("AgcThresh5");
			}
		}

		public LnaZinEnum LnaZin
		{
			get
			{
				return lnaZin;
			}
			set
			{
				lnaZin = value;
				OnPropertyChanged("LnaZin");
			}
		}

		public bool LnaLowPowerOn
		{
			get
			{
				return lnaLowPowerOn;
			}
			set
			{
				lnaLowPowerOn = value;
				OnPropertyChanged("LnaLowPowerOn");
			}
		}

		public LnaGainEnum LnaCurrentGain
		{
			get
			{
				return lnaCurrentGain;
			}
			set
			{
				lnaCurrentGain = value;
				OnPropertyChanged("LnaCurrentGain");
			}
		}

		public LnaGainEnum LnaGainSelect
		{
			get
			{
				return lnaGainSelect;
			}
			set
			{
				lnaGainSelect = value;
				OnPropertyChanged("LnaGainSelect");
			}
		}

		public Decimal DccFreq
		{
			get
			{
				return dccFreq;
			}
			set
			{
				dccFreq = value;
				OnPropertyChanged("DccFreq");
			}
		}

		public Decimal RxBw
		{
			get
			{
				return rxBw;
			}
			set
			{
				rxBw = value;
				BitRateFdevCheck(bitRate, fdev);
				OnPropertyChanged("RxBw");
			}
		}

		public Decimal AfcDccFreq
		{
			get
			{
				return afcDccFreq;
			}
			set
			{
				afcDccFreq = value;
				OnPropertyChanged("AfcDccFreq");
			}
		}

		public Decimal AfcRxBw
		{
			get
			{
				return afcRxBw;
			}
			set
			{
				afcRxBw = value;
				OnPropertyChanged("AfcRxBw");
			}
		}

		public OokThreshTypeEnum OokThreshType
		{
			get
			{
				return ookThreshType;
			}
			set
			{
				ookThreshType = value;
				OnPropertyChanged("OokThreshType");
			}
		}

		public Decimal OokPeakThreshStep
		{
			get
			{
				return ookPeakThreshStep;
			}
			set
			{
				ookPeakThreshStep = value;
				OnPropertyChanged("OokPeakThreshStep");
			}
		}

		public Decimal[] OoPeakThreshStepTable
		{
			get
			{
				return ookPeakThreshStepTable;
			}
		}

		public OokPeakThreshDecEnum OokPeakThreshDec
		{
			get
			{
				return ookPeakThreshDec;
			}
			set
			{
				ookPeakThreshDec = value;
				OnPropertyChanged("OokPeakThreshDec");
			}
		}

		public OokAverageThreshFiltEnum OokAverageThreshFilt
		{
			get
			{
				return ookAverageThreshFilt;
			}
			set
			{
				ookAverageThreshFilt = value;
				OnPropertyChanged("OokAverageThreshFilt");
			}
		}

		public byte OokFixedThresh
		{
			get
			{
				return ookFixedThresh;
			}
			set
			{
				ookFixedThresh = value;
				OnPropertyChanged("ookFixedThresh");
			}
		}

		public bool FeiDone
		{
			get
			{
				return feiDone;
			}
		}

		public bool AfcDone
		{
			get
			{
				return afcDone;
			}
		}

		public bool AfcAutoClearOn
		{
			get
			{
				return afcAutoClearOn;
			}
			set
			{
				afcAutoClearOn = value;
				OnPropertyChanged("AfcAutoClearOn");
			}
		}

		public bool AfcAutoOn
		{
			get
			{
				return afcAutoOn;
			}
			set
			{
				afcAutoOn = value;
				OnPropertyChanged("AfcAutoOn");
			}
		}

		public Decimal AfcValue
		{
			get
			{
				return afcValue;
			}
			set
			{
				afcValue = value;
				OnPropertyChanged("AfcValue");
			}
		}

		public Decimal FeiValue
		{
			get
			{
				return feiValue;
			}
			set
			{
				feiValue = value;
				OnPropertyChanged("FeiValue");
			}
		}

		public bool FastRx
		{
			get
			{
				return fastRx;
			}
			set
			{
				fastRx = value;
				OnPropertyChanged("FastRx");
			}
		}

		public bool RssiDone
		{
			get { return rssiDone; }
		}

		public Decimal RssiValue
		{
			get { return rssiValue; }
		}

		public DioMappingEnum Dio0Mapping
		{
			get { return dio0Mapping; }
			set
			{
				dio0Mapping = value;
				OnPropertyChanged("Dio0Mapping");
			}
		}

		public DioMappingEnum Dio1Mapping
		{
			get { return dio1Mapping; }
			set
			{
				dio1Mapping = value;
				OnPropertyChanged("Dio1Mapping");
			}
		}

		public DioMappingEnum Dio2Mapping
		{
			get { return dio2Mapping; }
			set
			{
				dio2Mapping = value;
				OnPropertyChanged("Dio2Mapping");
			}
		}

		public DioMappingEnum Dio3Mapping
		{
			get { return dio3Mapping; }
			set
			{
				dio3Mapping = value;
				OnPropertyChanged("Dio3Mapping");
			}
		}

		public DioMappingEnum Dio4Mapping
		{
			get { return dio4Mapping; }
			set
			{
				dio4Mapping = value;
				OnPropertyChanged("Dio4Mapping");
			}
		}

		public DioMappingEnum Dio5Mapping
		{
			get { return dio5Mapping; }
			set
			{
				dio5Mapping = value;
				OnPropertyChanged("Dio5Mapping");
			}
		}

		public ClockOutEnum ClockOut
		{
			get { return clockOut; }
			set
			{
				clockOut = value;
				OnPropertyChanged("ClockOut");
			}
		}

		public bool ModeReady
		{
			get { return modeReady; }
		}

		public bool RxReady
		{
			get { return rxReady; }
		}

		public bool TxReady
		{
			get { return txReady; }
		}

		public bool PllLock
		{
			get { return pllLock; }
		}

		public bool Rssi
		{
			get { return rssi; }
		}

		public bool Timeout
		{
			get { return timeout; }
		}

		public bool AutoMode
		{
			get { return autoMode; }
		}

		public bool SyncAddressMatch
		{
			get { return syncAddressMatch; }
		}

		public bool FifoFull
		{
			get { return fifoFull; }
		}

		public bool FifoNotEmpty
		{
			get { return fifoNotEmpty; }
		}

		public bool FifoLevel
		{
			get { return fifoLevel; }
		}

		public bool FifoOverrun
		{
			get { return fifoOverrun; }
		}

		public bool PacketSent
		{
			get { return packetSent; }
		}

		public bool PayloadReady
		{
			get { return payloadReady; }
		}

		public bool CrcOk
		{
			get { return crcOk; }
		}

		public bool LowBat
		{
			get { return lowBat; }
		}

		public Decimal RssiThresh
		{
			get { return rssiThresh; }
			set
			{
				rssiThresh = value;
				OnPropertyChanged("RssiThresh");
			}
		}

		public Decimal TimeoutRxStart
		{
			get { return timeoutRxStart; }
			set
			{
				timeoutRxStart = value;
				OnPropertyChanged("TimeoutRxStart");
			}
		}

		public Decimal TimeoutRssiThresh
		{
			get { return timeoutRssiThresh; }
			set
			{
				timeoutRssiThresh = value;
				OnPropertyChanged("TimeoutRssiThresh");
			}
		}

		public Packet Packet
		{
			get { return packet; }
			set
			{
				packet = value;
				packet.PropertyChanged += new PropertyChangedEventHandler(packet_PropertyChanged);
				OnPropertyChanged("Packet");
			}
		}

		public bool TempMeasRunning
		{
			get
			{
				return tempMeasRunning;
			}
		}

		public bool AdcLowPowerOn
		{
			get
			{
				return adcLowPowerOn;
			}
			set
			{
				adcLowPowerOn = value;
				OnPropertyChanged("AdcLowPowerOn");
			}
		}

		public Decimal TempValue
		{
			get
			{
				return tempValue;
			}
		}

		public Decimal TempValueRoom
		{
			get
			{
				return tempValueRoom;
			}
			set
			{
				tempValueRoom = value;
				OnPropertyChanged("TempValueRoom");
			}
		}

		public Decimal TempValueCal
		{
			get
			{
				return tempValueCal;
			}
			set
			{
				tempValueCal = value;
			}
		}

		public bool TempCalDone
		{
			get
			{
				return tempCalDone;
			}
			set
			{
				tempCalDone = value;
				OnPropertyChanged("TempCalDone");
			}
		}

		public bool Pa20dBm
		{
			get
			{
				return pa20dBm;
			}
			set
			{
				pa20dBm = value;
				OnPropertyChanged("Pa20dBm");
			}
		}

		public bool SensitivityBoostOn
		{
			get
			{
				return sensitivityBoostOn;
			}
			set
			{
				sensitivityBoostOn = value;
				OnPropertyChanged("SensitivityBoostOn");
			}
		}

		public Decimal LowBetaAfcOffset
		{
			get
			{
				return lowBetaAfcOffset;
			}
			set
			{
				lowBetaAfcOffset = value;
				OnPropertyChanged("LowBetaAfcOffset");
			}
		}

		public bool DagcOn
		{
			get
			{
				return dagcOn;
			}
			set
			{
				dagcOn = value;
				OnPropertyChanged("DagcOn");
			}
		}

		public RfPaSwitchSelEnum RfPaSwitchSel
		{
			get
			{
				return rfPaSwitchSel;
			}
			set
			{
				lock (syncThread)
				{
					try
					{
						rfPaSwitchSel = value;
						switch (value)
						{
							case RfPaSwitchSelEnum.RF_IO_RFIO:
								ftdi.PortA.PortAcValue &= (byte)254;
								break;
							case RfPaSwitchSelEnum.RF_IO_PA_BOOST:
								ftdi.PortA.PortAcValue |= (byte)1;
								break;
						}
						ftdi.PortA.SendBytes();
						OnPropertyChanged("RfPaSwitchSel");
					}
					catch (Exception exception_0)
					{
						OnError(1, exception_0.Message);
					}
				}
			}
		}

		public int RfPaSwitchEnabled
		{
			get
			{
				return rfPaSwitchEnabled;
			}
			set
			{
				lock (syncThread)
				{
					try
					{
						prevRfPaSwitchEnabled = rfPaSwitchEnabled;
						rfPaSwitchEnabled = value;
						if (prevRfPaSwitchEnabled != rfPaSwitchEnabled)
						{
							if (rfPaSwitchEnabled == 2)
								prevRfPaSwitchSel = rfPaSwitchSel;
							else
								rfPaSwitchSel = prevRfPaSwitchSel;
						}
						ftdi.PortA.PortAcDir = rfPaSwitchEnabled == 0 ? (byte)0 : (byte)1;
						ftdi.PortA.SendBytes();
						OnPropertyChanged("RfPaSwitchEnabled");
						OnPropertyChanged("RfPaSwitchSel");
					}
					catch (Exception exception_0)
					{
						OnError(1, exception_0.Message);
					}
				}
			}
		}

		public Decimal RfPaRssiValue
		{
			get
			{
				return rfPaRssiValue;
			}
		}

		public Decimal RfIoRssiValue
		{
			get
			{
				return rfIoRssiValue;
			}
		}

		private void PopulateRegisters()
		{
			if (IsOpen)
			{
				byte data = 0;
				if (!Read(16, ref data))
					throw new Exception("Unable to read register RegVersion");
				if (!Read(16, ref data))
					throw new Exception("Unable to read register RegVersion");
				Version = new Version((int)data >> 4, (int)data & 0x0F);
			}
			registers = new RegisterCollection();

			registers.Add(new Register("RegFifo", 0, 0, true, true));
			registers.Add(new Register("RegOpMode", 1, 4, false, true));

			int num14 = (int)2;
			int num15 = 1;
			byte num16 = (byte)(num14 + num15);
			int num17 = 0;
			int num18 = 0;
			int num19 = 1;
			Register register3 = new Register("RegDataModul", (uint)num14, (uint)num17, num18 != 0, num19 != 0);
			registers.Add(register3);

			string name4 = "RegBitrateMsb";
			int num20 = (int)num16;
			int num21 = 1;
			byte num22 = (byte)(num20 + num21);
			int num23 = 26;
			int num24 = 0;
			int num25 = 1;
			Register register4 = new Register(name4, (uint)num20, (uint)num23, num24 != 0, num25 != 0);
			registers.Add(register4);

			string name5 = "RegBitrateLsb";
			int num26 = (int)num22;
			int num27 = 1;
			byte num28 = (byte)(num26 + num27);
			int num29 = 11;
			int num30 = 0;
			int num31 = 1;
			Register register5 = new Register(name5, (uint)num26, (uint)num29, num30 != 0, num31 != 0);
			registers.Add(register5);

			string name6 = "RegFdevMsb";
			int num32 = (int)num28;
			int num33 = 1;
			byte num34 = (byte)(num32 + num33);
			int num35 = 0;
			int num36 = 0;
			int num37 = 1;
			Register register6 = new Register(name6, (uint)num32, (uint)num35, num36 != 0, num37 != 0);
			registers.Add(register6);

			string name7 = "RegFdevLsb";
			int num38 = (int)num34;
			int num39 = 1;
			byte num40 = (byte)(num38 + num39);
			int num41 = 82;
			int num42 = 0;
			int num43 = 1;
			Register register7 = new Register(name7, (uint)num38, (uint)num41, num42 != 0, num43 != 0);
			registers.Add(register7);

			string name8 = "RegFrfMsb";
			int num44 = (int)num40;
			int num45 = 1;
			byte num46 = (byte)(num44 + num45);
			int num47 = 228;
			int num48 = 0;
			int num49 = 1;
			Register register8 = new Register(name8, (uint)num44, (uint)num47, num48 != 0, num49 != 0);
			registers.Add(register8);

			string name9 = "RegFrfMid";
			int num50 = (int)num46;
			int num51 = 1;
			byte num52 = (byte)(num50 + num51);
			int num53 = 192;
			int num54 = 0;
			int num55 = 1;
			Register register9 = new Register(name9, (uint)num50, (uint)num53, num54 != 0, num55 != 0);
			registers.Add(register9);

			string name10 = "RegFrfLsb";
			int num56 = (int)num52;
			int num57 = 1;
			byte num58 = (byte)(num56 + num57);
			int num59 = 0;
			int num60 = 0;
			int num61 = 1;
			Register register10 = new Register(name10, (uint)num56, (uint)num59, num60 != 0, num61 != 0);
			registers.Add(register10);

			string name11 = "RegOsc1";
			int num62 = (int)num58;
			int num63 = 1;
			byte num64 = (byte)(num62 + num63);
			int num65 = 65;
			int num66 = 0;
			int num67 = 1;
			Register register11 = new Register(name11, (uint)num62, (uint)num65, num66 != 0, num67 != 0);
			registers.Add(register11);

			byte num68;
			if (Version <= new Version(2, 1))
			{
				string name12 = "RegOsc2";
				int num69 = (int)num64;
				int num70 = 1;
				num68 = (byte)(num69 + num70);
				int num71 = 64;
				int num72 = 0;
				int num73 = 1;
				Register register12 = new Register(name12, (uint)num69, (uint)num71, num72 != 0, num73 != 0);
				registers.Add(register12);
			}
			else
			{
				string name12 = "RegAfcCtrl";
				int num69 = (int)num64;
				int num70 = 1;
				num68 = (byte)(num69 + num70);
				int num71 = 0;
				int num72 = 0;
				int num73 = 1;
				Register register12 = new Register(name12, (uint)num69, (uint)num71, num72 != 0, num73 != 0);
				registers.Add(register12);
			}

			string name13 = "RegLowBat";
			int num74 = (int)num68;
			int num75 = 1;
			byte num76 = (byte)(num74 + num75);
			int num77 = 2;
			int num78 = 0;
			int num79 = 1;
			Register register13 = new Register(name13, (uint)num74, (uint)num77, num78 != 0, num79 != 0);
			registers.Add(register13);

			string name14 = "RegListen1";
			int num80 = (int)num76;
			int num81 = 1;
			byte num82 = (byte)(num80 + num81);
			int num83 = 146;
			int num84 = 0;
			int num85 = 1;
			Register register14 = new Register(name14, (uint)num80, (uint)num83, num84 != 0, num85 != 0);
			registers.Add(register14);

			string name15 = "RegListen2";
			int num86 = (int)num82;
			int num87 = 1;
			byte num88 = (byte)(num86 + num87);
			int num89 = 245;
			int num90 = 0;
			int num91 = 1;
			Register register15 = new Register(name15, (uint)num86, (uint)num89, num90 != 0, num91 != 0);
			registers.Add(register15);

			string name16 = "RegListen3";
			int num92 = (int)num88;
			int num93 = 1;
			byte num94 = (byte)(num92 + num93);
			int num95 = 32;
			int num96 = 0;
			int num97 = 1;
			Register register16 = new Register(name16, (uint)num92, (uint)num95, num96 != 0, num97 != 0);
			registers.Add(register16);

			string name17 = "RegVersion";
			int num98 = (int)num94;
			int num99 = 1;
			byte num100 = (byte)(num98 + num99);
			int num101 = 36;
			int num102 = 1;
			int num103 = 1;
			Register register17 = new Register(name17, (uint)num98, (uint)num101, num102 != 0, num103 != 0);
			registers.Add(register17);

			string name18 = "RegPaLevel";
			int num104 = (int)num100;
			int num105 = 1;
			byte num106 = (byte)(num104 + num105);
			int num107 = 159;
			int num108 = 0;
			int num109 = 1;
			Register register18 = new Register(name18, (uint)num104, (uint)num107, num108 != 0, num109 != 0);
			registers.Add(register18);

			string name19 = "RegPaRamp";
			int num110 = (int)num106;
			int num111 = 1;
			byte num112 = (byte)(num110 + num111);
			int num113 = 9;
			int num114 = 0;
			int num115 = 1;
			Register register19 = new Register(name19, (uint)num110, (uint)num113, num114 != 0, num115 != 0);
			registers.Add(register19);

			string name20 = "RegOcp";
			int num116 = (int)num112;
			int num117 = 1;
			byte num118 = (byte)(num116 + num117);
			int num119 = 26;
			int num120 = 0;
			int num121 = 1;
			Register register20 = new Register(name20, (uint)num116, (uint)num119, num120 != 0, num121 != 0);
			registers.Add(register20);
			byte num122;
			if (Version <= new Version(2, 1))
			{
				string name12 = "RegAgcRef";
				int num69 = (int)num118;
				int num70 = 1;
				byte num71 = (byte)(num69 + num70);
				int num72 = 64;
				int num73 = 0;
				int num123 = 1;
				Register register12 = new Register(name12, (uint)num69, (uint)num72, num73 != 0, num123 != 0);
				registers.Add(register12);

				string name21 = "RegAgcThresh1";
				int num124 = (int)num71;
				int num125 = 1;
				byte num126 = (byte)(num124 + num125);
				int num127 = 176;
				int num128 = 0;
				int num129 = 1;
				Register register21 = new Register(name21, (uint)num124, (uint)num127, num128 != 0, num129 != 0);
				registers.Add(register21);

				string name22 = "RegAgcThresh2";
				int num130 = (int)num126;
				int num131 = 1;
				byte num132 = (byte)(num130 + num131);
				int num133 = 123;
				int num134 = 0;
				int num135 = 1;
				Register register22 = new Register(name22, (uint)num130, (uint)num133, num134 != 0, num135 != 0);
				registers.Add(register22);

				string name23 = "RegAgcThresh3";
				int num136 = (int)num132;
				int num137 = 1;
				num122 = (byte)(num136 + num137);
				int num138 = 155;
				int num139 = 0;
				int num140 = 1;
				Register register23 = new Register(name23, (uint)num136, (uint)num138, num139 != 0, num140 != 0);
				registers.Add(register23);
			}
			else
			{
				string name12 = "Reserved14";
				int num69 = (int)num118;
				int num70 = 1;
				byte num71 = (byte)(num69 + num70);
				int num72 = 64;
				int num73 = 0;
				int num123 = 1;
				Register register12 = new Register(name12, (uint)num69, (uint)num72, num73 != 0, num123 != 0);
				registers.Add(register12);

				string name21 = "Reserved15";
				int num124 = (int)num71;
				int num125 = 1;
				byte num126 = (byte)(num124 + num125);
				int num127 = 176;
				int num128 = 0;
				int num129 = 1;
				Register register21 = new Register(name21, (uint)num124, (uint)num127, num128 != 0, num129 != 0);
				registers.Add(register21);

				string name22 = "Reserved16";
				int num130 = (int)num126;
				int num131 = 1;
				byte num132 = (byte)(num130 + num131);
				int num133 = 123;
				int num134 = 0;
				int num135 = 1;
				Register register22 = new Register(name22, (uint)num130, (uint)num133, num134 != 0, num135 != 0);
				registers.Add(register22);

				string name23 = "Reserved17";
				int num136 = (int)num132;
				int num137 = 1;
				num122 = (byte)(num136 + num137);
				int num138 = 155;
				int num139 = 0;
				int num140 = 1;
				Register register23 = new Register(name23, (uint)num136, (uint)num138, num139 != 0, num140 != 0);
				registers.Add(register23);
			}

			string name24 = "RegLna";
			int num141 = (int)num122;
			int num142 = 1;
			byte num143 = (byte)(num141 + num142);
			int num144 = 136;
			int num145 = 0;
			int num146 = 1;
			Register register24 = new Register(name24, (uint)num141, (uint)num144, num145 != 0, num146 != 0);
			registers.Add(register24);

			string name25 = "RegRxBw";
			int num147 = (int)num143;
			int num148 = 1;
			byte num149 = (byte)(num147 + num148);
			int num150 = 85;
			int num151 = 0;
			int num152 = 1;
			Register register25 = new Register(name25, (uint)num147, (uint)num150, num151 != 0, num152 != 0);
			registers.Add(register25);

			string name26 = "RegAfcBw";
			int num153 = (int)num149;
			int num154 = 1;
			byte num155 = (byte)(num153 + num154);
			int num156 = 139;
			int num157 = 0;
			int num158 = 1;
			Register register26 = new Register(name26, (uint)num153, (uint)num156, num157 != 0, num158 != 0);
			registers.Add(register26);

			string name27 = "RegOokPeak";
			int num159 = (int)num155;
			int num160 = 1;
			byte num161 = (byte)(num159 + num160);
			int num162 = 64;
			int num163 = 0;
			int num164 = 1;
			Register register27 = new Register(name27, (uint)num159, (uint)num162, num163 != 0, num164 != 0);
			registers.Add(register27);

			string name28 = "RegOokAvg";
			int num165 = (int)num161;
			int num166 = 1;
			byte num167 = (byte)(num165 + num166);
			int num168 = 128;
			int num169 = 0;
			int num170 = 1;
			Register register28 = new Register(name28, (uint)num165, (uint)num168, num169 != 0, num170 != 0);
			registers.Add(register28);

			string name29 = "RegOokFix";
			int num171 = (int)num167;
			int num172 = 1;
			byte num173 = (byte)(num171 + num172);
			int num174 = 6;
			int num175 = 0;
			int num176 = 1;
			Register register29 = new Register(name29, (uint)num171, (uint)num174, num175 != 0, num176 != 0);
			registers.Add(register29);

			string name30 = "RegAfcFei";
			int num177 = (int)num173;
			int num178 = 1;
			byte num179 = (byte)(num177 + num178);
			int num180 = 16;
			int num181 = 0;
			int num182 = 1;
			Register register30 = new Register(name30, (uint)num177, (uint)num180, num181 != 0, num182 != 0);
			registers.Add(register30);

			string name31 = "RegAfcMsb";
			int num183 = (int)num179;
			int num184 = 1;
			byte num185 = (byte)(num183 + num184);
			int num186 = 0;
			int num187 = 1;
			int num188 = 1;
			Register register31 = new Register(name31, (uint)num183, (uint)num186, num187 != 0, num188 != 0);
			registers.Add(register31);

			string name32 = "RegAfcLsb";
			int num189 = (int)num185;
			int num190 = 1;
			byte num191 = (byte)(num189 + num190);
			int num192 = 0;
			int num193 = 1;
			int num194 = 1;
			Register register32 = new Register(name32, (uint)num189, (uint)num192, num193 != 0, num194 != 0);
			registers.Add(register32);

			string name33 = "RegFeiMsb";
			int num195 = (int)num191;
			int num196 = 1;
			byte num197 = (byte)(num195 + num196);
			int num198 = 0;
			int num199 = 1;
			int num200 = 1;
			Register register33 = new Register(name33, (uint)num195, (uint)num198, num199 != 0, num200 != 0);
			registers.Add(register33);

			string name34 = "RegFeiLsb";
			int num201 = (int)num197;
			int num202 = 1;
			byte num203 = (byte)(num201 + num202);
			int num204 = 0;
			int num205 = 1;
			int num206 = 1;
			Register register34 = new Register(name34, (uint)num201, (uint)num204, num205 != 0, num206 != 0);
			registers.Add(register34);

			string name35 = "RegRssiConfig";
			int num207 = (int)num203;
			int num208 = 1;
			byte num209 = (byte)(num207 + num208);
			int num210 = 2;
			int num211 = 1;
			int num212 = 1;
			Register register35 = new Register(name35, (uint)num207, (uint)num210, num211 != 0, num212 != 0);
			registers.Add(register35);

			string name36 = "RegRssiValue";
			int num213 = (int)num209;
			int num214 = 1;
			byte num215 = (byte)(num213 + num214);
			int num216 = (int)byte.MaxValue;
			int num217 = 1;
			int num218 = 1;
			Register register36 = new Register(name36, (uint)num213, (uint)num216, num217 != 0, num218 != 0);
			registers.Add(register36);

			string name37 = "RegDioMapping1";
			int num219 = (int)num215;
			int num220 = 1;
			byte num221 = (byte)(num219 + num220);
			int num222 = 0;
			int num223 = 0;
			int num224 = 1;
			Register register37 = new Register(name37, (uint)num219, (uint)num222, num223 != 0, num224 != 0);
			registers.Add(register37);

			string name38 = "RegDioMapping2";
			int num225 = (int)num221;
			int num226 = 1;
			byte num227 = (byte)(num225 + num226);
			int num228 = 7;
			int num229 = 0;
			int num230 = 1;
			Register register38 = new Register(name38, (uint)num225, (uint)num228, num229 != 0, num230 != 0);
			registers.Add(register38);

			string name39 = "RegIrqFlags1";
			int num231 = (int)num227;
			int num232 = 1;
			byte num233 = (byte)(num231 + num232);
			int num234 = 128;
			int num235 = 1;
			int num236 = 1;
			Register register39 = new Register(name39, (uint)num231, (uint)num234, num235 != 0, num236 != 0);
			registers.Add(register39);

			string name40 = "RegIrqFlags2";
			int num237 = (int)num233;
			int num238 = 1;
			byte num239 = (byte)(num237 + num238);
			int num240 = 0;
			int num241 = 1;
			int num242 = 1;
			Register register40 = new Register(name40, (uint)num237, (uint)num240, num241 != 0, num242 != 0);
			registers.Add(register40);

			string name41 = "RegRssiThresh";
			int num243 = (int)num239;
			int num244 = 1;
			byte num245 = (byte)(num243 + num244);
			int num246 = 228;
			int num247 = 0;
			int num248 = 1;
			Register register41 = new Register(name41, (uint)num243, (uint)num246, num247 != 0, num248 != 0);
			registers.Add(register41);

			byte num251 = (byte)(num245 + 1);
			registers.Add(new Register("RegRxTimeout1", num245, 0, false, true));

			byte num257 = (byte)(num251 + 1);
			registers.Add(new Register("RegRxTimeout2", (uint)num251, 0, false, true));

			byte num263 = (byte)(num257 + 1);
			registers.Add(new Register("RegPreambleMsb", num257, 0, false, true));

			byte num269 = (byte)(num263 + 1);
			registers.Add(new Register("RegPreambleLsb", num263, 3, false, true));

			byte num275 = (byte)(num269 + 1);
			registers.Add(new Register("RegSyncConfig", num269, 152, false, true));

			byte num281 = (byte)(num275 + 1);
			registers.Add(new Register("RegSyncValue1", num275, 0, false, true));

			string name48 = "RegSyncValue2";
			int num285 = (int)num281;
			int num286 = 1;
			byte num287 = (byte)(num285 + num286);
			int num288 = 0;
			int num289 = 0;
			int num290 = 1;
			Register register48 = new Register(name48, (uint)num285, (uint)num288, num289 != 0, num290 != 0);
			registers.Add(register48);

			string name49 = "RegSyncValue3";
			int num291 = (int)num287;
			int num292 = 1;
			byte num293 = (byte)(num291 + num292);
			int num294 = 0;
			int num295 = 0;
			int num296 = 1;
			Register register49 = new Register(name49, (uint)num291, (uint)num294, num295 != 0, num296 != 0);
			registers.Add(register49);

			string name50 = "RegSyncValue4";
			int num297 = (int)num293;
			int num298 = 1;
			byte num299 = (byte)(num297 + num298);
			int num300 = 0;
			int num301 = 0;
			int num302 = 1;
			Register register50 = new Register(name50, (uint)num297, (uint)num300, num301 != 0, num302 != 0);
			registers.Add(register50);

			string name51 = "RegSyncValue5";
			int num303 = (int)num299;
			int num304 = 1;
			byte num305 = (byte)(num303 + num304);
			int num306 = 0;
			int num307 = 0;
			int num308 = 1;
			Register register51 = new Register(name51, (uint)num303, (uint)num306, num307 != 0, num308 != 0);
			registers.Add(register51);

			string name52 = "RegSyncValue6";
			int num309 = (int)num305;
			int num310 = 1;
			byte num311 = (byte)(num309 + num310);
			int num312 = 0;
			int num313 = 0;
			int num314 = 1;
			Register register52 = new Register(name52, (uint)num309, (uint)num312, num313 != 0, num314 != 0);
			registers.Add(register52);

			string name53 = "RegSyncValue7";
			int num315 = (int)num311;
			int num316 = 1;
			byte num317 = (byte)(num315 + num316);
			int num318 = 0;
			int num319 = 0;
			int num320 = 1;
			Register register53 = new Register(name53, (uint)num315, (uint)num318, num319 != 0, num320 != 0);
			registers.Add(register53);

			string name54 = "RegSyncValue8";
			int num321 = (int)num317;
			int num322 = 1;
			byte num323 = (byte)(num321 + num322);
			int num324 = 0;
			int num325 = 0;
			int num326 = 1;
			Register register54 = new Register(name54, (uint)num321, (uint)num324, num325 != 0, num326 != 0);
			registers.Add(register54);

			string name55 = "RegPacketConfig1";
			int num327 = (int)num323;
			int num328 = 1;
			byte num329 = (byte)(num327 + num328);
			int num330 = 16;
			int num331 = 0;
			int num332 = 1;
			Register register55 = new Register(name55, (uint)num327, (uint)num330, num331 != 0, num332 != 0);
			registers.Add(register55);

			string name56 = "RegPayloadLength";
			int num333 = (int)num329;
			int num334 = 1;
			byte num335 = (byte)(num333 + num334);
			int num336 = 64;
			int num337 = 0;
			int num338 = 1;
			Register register56 = new Register(name56, (uint)num333, (uint)num336, num337 != 0, num338 != 0);
			registers.Add(register56);

			string name57 = "RegNodeAdrs";
			int num339 = (int)num335;
			int num340 = 1;
			byte num341 = (byte)(num339 + num340);
			int num342 = 0;
			int num343 = 0;
			int num344 = 1;
			Register register57 = new Register(name57, (uint)num339, (uint)num342, num343 != 0, num344 != 0);
			registers.Add(register57);

			string name58 = "RegBroadcastAdrs";
			int num345 = (int)num341;
			int num346 = 1;
			byte num347 = (byte)(num345 + num346);
			int num348 = 0;
			int num349 = 0;
			int num350 = 1;
			Register register58 = new Register(name58, (uint)num345, (uint)num348, num349 != 0, num350 != 0);
			registers.Add(register58);

			string name59 = "RegAutoModes";
			int num351 = (int)num347;
			int num352 = 1;
			byte num353 = (byte)(num351 + num352);
			int num354 = 0;
			int num355 = 0;
			int num356 = 1;
			Register register59 = new Register(name59, (uint)num351, (uint)num354, num355 != 0, num356 != 0);
			registers.Add(register59);

			string name60 = "RegFifoThresh";
			int num357 = (int)num353;
			int num358 = 1;
			byte num359 = (byte)(num357 + num358);
			int num360 = 143;
			int num361 = 0;
			int num362 = 1;
			Register register60 = new Register(name60, (uint)num357, (uint)num360, num361 != 0, num362 != 0);
			registers.Add(register60);

			string name61 = "RegPacketConfig2";
			int num363 = (int)num359;
			int num364 = 1;
			byte num365 = (byte)(num363 + num364);
			int num366 = 2;
			int num367 = 0;
			int num368 = 1;
			Register register61 = new Register(name61, (uint)num363, (uint)num366, num367 != 0, num368 != 0);
			registers.Add(register61);

			string name62 = "RegAesKey1";
			int num369 = (int)num365;
			int num370 = 1;
			byte num371 = (byte)(num369 + num370);
			int num372 = 0;
			int num373 = 0;
			int num374 = 1;
			Register register62 = new Register(name62, (uint)num369, (uint)num372, num373 != 0, num374 != 0);
			registers.Add(register62);

			string name63 = "RegAesKey2";
			int num375 = (int)num371;
			int num376 = 1;
			byte num377 = (byte)(num375 + num376);
			int num378 = 0;
			int num379 = 0;
			int num380 = 1;
			Register register63 = new Register(name63, (uint)num375, (uint)num378, num379 != 0, num380 != 0);
			registers.Add(register63);

			string name64 = "RegAesKey3";
			int num381 = (int)num377;
			int num382 = 1;
			byte num383 = (byte)(num381 + num382);
			int num384 = 0;
			int num385 = 0;
			int num386 = 1;
			Register register64 = new Register(name64, (uint)num381, (uint)num384, num385 != 0, num386 != 0);
			registers.Add(register64);

			string name65 = "RegAesKey4";
			int num387 = (int)num383;
			int num388 = 1;
			byte num389 = (byte)(num387 + num388);
			int num390 = 0;
			int num391 = 0;
			int num392 = 1;
			Register register65 = new Register(name65, (uint)num387, (uint)num390, num391 != 0, num392 != 0);
			registers.Add(register65);

			string name66 = "RegAesKey5";
			int num393 = (int)num389;
			int num394 = 1;
			byte num395 = (byte)(num393 + num394);
			int num396 = 0;
			int num397 = 0;
			int num398 = 1;
			Register register66 = new Register(name66, (uint)num393, (uint)num396, num397 != 0, num398 != 0);
			registers.Add(register66);

			string name67 = "RegAesKey6";
			int num399 = (int)num395;
			int num400 = 1;
			byte num401 = (byte)(num399 + num400);
			int num402 = 0;
			int num403 = 0;
			int num404 = 1;
			Register register67 = new Register(name67, (uint)num399, (uint)num402, num403 != 0, num404 != 0);
			registers.Add(register67);

			string name68 = "RegAesKey7";
			int num405 = (int)num401;
			int num406 = 1;
			byte num407 = (byte)(num405 + num406);
			int num408 = 0;
			int num409 = 0;
			int num410 = 1;
			Register register68 = new Register(name68, (uint)num405, (uint)num408, num409 != 0, num410 != 0);
			registers.Add(register68);

			string name69 = "RegAesKey8";
			int num411 = (int)num407;
			int num412 = 1;
			byte num413 = (byte)(num411 + num412);
			int num414 = 0;
			int num415 = 0;
			int num416 = 1;
			Register register69 = new Register(name69, (uint)num411, (uint)num414, num415 != 0, num416 != 0);
			registers.Add(register69);

			string name70 = "RegAesKey9";
			int num417 = (int)num413;
			int num418 = 1;
			byte num419 = (byte)(num417 + num418);
			int num420 = 0;
			int num421 = 0;
			int num422 = 1;
			Register register70 = new Register(name70, (uint)num417, (uint)num420, num421 != 0, num422 != 0);
			registers.Add(register70);

			string name71 = "RegAesKey10";
			int num423 = (int)num419;
			int num424 = 1;
			byte num425 = (byte)(num423 + num424);
			int num426 = 0;
			int num427 = 0;
			int num428 = 1;
			Register register71 = new Register(name71, (uint)num423, (uint)num426, num427 != 0, num428 != 0);
			registers.Add(register71);

			string name72 = "RegAesKey11";
			int num429 = (int)num425;
			int num430 = 1;
			byte num431 = (byte)(num429 + num430);
			int num432 = 0;
			int num433 = 0;
			int num434 = 1;
			Register register72 = new Register(name72, (uint)num429, (uint)num432, num433 != 0, num434 != 0);
			registers.Add(register72);

			string name73 = "RegAesKey12";
			int num435 = (int)num431;
			int num436 = 1;
			byte num437 = (byte)(num435 + num436);
			int num438 = 0;
			int num439 = 0;
			int num440 = 1;
			Register register73 = new Register(name73, (uint)num435, (uint)num438, num439 != 0, num440 != 0);
			registers.Add(register73);

			string name74 = "RegAesKey13";
			int num441 = (int)num437;
			int num442 = 1;
			byte num443 = (byte)(num441 + num442);
			int num444 = 0;
			int num445 = 0;
			int num446 = 1;
			Register register74 = new Register(name74, (uint)num441, (uint)num444, num445 != 0, num446 != 0);
			registers.Add(register74);

			string name75 = "RegAesKey14";
			int num447 = (int)num443;
			int num448 = 1;
			byte num449 = (byte)(num447 + num448);
			int num450 = 0;
			int num451 = 0;
			int num452 = 1;
			Register register75 = new Register(name75, (uint)num447, (uint)num450, num451 != 0, num452 != 0);
			registers.Add(register75);

			string name76 = "RegAesKey15";
			int num453 = (int)num449;
			int num454 = 1;
			byte num455 = (byte)(num453 + num454);
			int num456 = 0;
			int num457 = 0;
			int num458 = 1;
			Register register76 = new Register(name76, (uint)num453, (uint)num456, num457 != 0, num458 != 0);
			registers.Add(register76);

			string name77 = "RegAesKey16";
			int num459 = (int)num455;
			int num460 = 1;
			byte num461 = (byte)(num459 + num460);
			int num462 = 0;
			int num463 = 0;
			int num464 = 1;
			Register register77 = new Register(name77, (uint)num459, (uint)num462, num463 != 0, num464 != 0);
			registers.Add(register77);

			string name78 = "RegTemp1";
			int num465 = (int)num461;
			int num466 = 1;
			byte num467 = (byte)(num465 + num466);
			int num468 = 1;
			int num469 = 1;
			int num470 = 1;
			Register register78 = new Register(name78, (uint)num465, (uint)num468, num469 != 0, num470 != 0);
			registers.Add(register78);

			string name79 = "RegTemp2";
			int num471 = (int)num467;
			int num472 = 1;
			byte num473 = (byte)(num471 + num472);
			int num474 = 0;
			int num475 = 1;
			int num476 = 1;
			Register register79 = new Register(name79, (uint)num471, (uint)num474, num475 != 0, num476 != 0);
			registers.Add(register79);
			if (Version > new Version(2, 1))
			{
				registers.Add(new Register("RegTestLna", 88U, 27U, false, true));
				if (Version >= new Version(2, 4))
				{
					registers.Add(new Register("RegTestPa1", 90U, 85U, false, true));
					registers.Add(new Register("RegTestPa2", 92U, 112U, false, true));
				}
				if (Version >= new Version(2, 3))
					registers.Add(new Register("RegTestDagc", 111U, 48U, false, true));
				registers.Add(new Register("RegTestAfc", 113U, 0U, false, true));
			}
			foreach (Register register12 in registers)
				register12.PropertyChanged += new PropertyChangedEventHandler(registers_PropertyChanged);
			Packet = new Packet();
		}

		private void UpdateSyncValue()
		{
			int num = (int)registers["RegSyncValue1"].Address;
			for (int index = 0; index < packet.SyncValue.Length; ++index)
				packet.SyncValue[index] = (byte)registers[num + index].Value;
			SyncValueCheck(packet.SyncValue);
			OnPropertyChanged("SyncValue");
		}

		private void UpdateAesKey()
		{
			int num = (int)registers["RegAesKey1"].Address;
			for (int index = 0; index < packet.AesKey.Length; ++index)
				packet.AesKey[index] = (byte)registers[num + index].Value;
			OnPropertyChanged("AesKey");
		}

		private void UpdateReceiverData()
		{
			OnPropertyChanged("RxBwMin");
			OnPropertyChanged("RxBwMax");
			switch ((registers["RegRxBw"].Value & 24U) >> 3)
			{
				case 0U:
					rxBw = SX1231.ComputeRxBw(frequencyXo, modulationType, 16, (int)registers["RegRxBw"].Value & 7);
					break;
				case 1U:
					rxBw = SX1231.ComputeRxBw(frequencyXo, modulationType, 20, (int)registers["RegRxBw"].Value & 7);
					break;
				case 2U:
					rxBw = SX1231.ComputeRxBw(frequencyXo, modulationType, 24, (int)registers["RegRxBw"].Value & 7);
					break;
				default:
					throw new Exception("Invalid RxBwMant parameter");
			}
			OnPropertyChanged("RxBw");
			OnPropertyChanged("DccFreqMin");
			OnPropertyChanged("DccFreqMax");
			dccFreq = ComputeDccFreq(RxBw, registers["RegRxBw"].Value);
			OnPropertyChanged("DccFreq");
			OnPropertyChanged("AfcRxBwMin");
			OnPropertyChanged("AfcRxBwMax");
			switch ((registers["RegAfcBw"].Value & 24U) >> 3)
			{
				case 0U:
					afcRxBw = SX1231.ComputeRxBw(frequencyXo, modulationType, 16, (int)registers["RegAfcBw"].Value & 7);
					break;
				case 1U:
					afcRxBw = SX1231.ComputeRxBw(frequencyXo, modulationType, 20, (int)registers["RegAfcBw"].Value & 7);
					break;
				case 2U:
					afcRxBw = SX1231.ComputeRxBw(frequencyXo, modulationType, 24, (int)registers["RegAfcBw"].Value & 7);
					break;
				default:
					throw new Exception("Invalid RxBwMant parameter");
			}
			OnPropertyChanged("AfcRxBw");
			OnPropertyChanged("AfcDccFreqMin");
			OnPropertyChanged("AfcDccFreqMax");
			afcDccFreq = ComputeDccFreq(AfcRxBw, registers["RegAfcBw"].Value);
			OnPropertyChanged("AfcDccFreq");
		}

		private void UpdateRegisterValue(Register r)
		{
			switch (r.Name)
			{
				case "RegOpMode":
					Sequencer = ((int)r.Value & 128) != 128;
					ListenMode = ((int)r.Value & 64) == 64;
					byte num2 = (byte)(r.Value >> 2 & 7U);
					if ((int)num2 > 4)
						num2 = (byte)0;
					Mode = (OperatingModeEnum)num2;
					if (packet.Mode != Mode)
						packet.Mode = Mode;
					if ((int)registers["RegPayloadLength"].Value != (int)packet.PayloadLength)
						registers["RegPayloadLength"].Value = (uint)packet.PayloadLength;
					lock (syncThread)
					{
						SetModeLeds(Mode);
						break;
					}
				case "RegDataModul":
					if (((int)r.Value & 96) == 32)
						r.Value &= 159U;
					if (((int)r.Value & 24) == 16 || ((int)r.Value & 24) == 24)
						r.Value &= 231U;
					DataMode = (DataModeEnum)((int)(r.Value >> 5) & 3);
					ModulationType = (ModulationTypeEnum)((int)(r.Value >> 3) & 3);
					ModulationShaping = (byte)(r.Value & 3U);
					UpdateReceiverData();
					BitRateFdevCheck(bitRate, fdev);
					break;
				case "RegBitrateMsb":
				case "RegBitrateLsb":
					if (((int)registers["RegBitrateMsb"].Value << 8 | (int)registers["RegBitrateLsb"].Value) == 0)
						registers["RegBitrateLsb"].Value = 1U;
					BitRate = frequencyXo / (Decimal)(registers["RegBitrateMsb"].Value << 8 | registers["RegBitrateLsb"].Value);
					break;
				case "RegFdevMsb":
				case "RegFdevLsb":
					Fdev = (Decimal)(registers["RegFdevMsb"].Value << 8 | registers["RegFdevLsb"].Value) * FrequencyStep;
					break;
				case "RegFrfMsb":
				case "RegFrfMid":
				case "RegFrfLsb":
					FrequencyRf = (Decimal)((uint)((int)registers["RegFrfMsb"].Value << 16 | (int)registers["RegFrfMid"].Value << 8) | registers["RegFrfLsb"].Value) * FrequencyStep;
					break;
				case "RegOsc1":
					rcCalDone = ((int)(r.Value >> 6) & 1) == 1;
					OnPropertyChanged("RcCalDone");
					break;
				case "RegOsc2":
				case "RegAfcCtrl":
					if (!(Version > new Version(2, 1)))
						break;
					AfcLowBetaOn = ((int)(r.Value >> 5) & 1) == 1;
					if (!(Version >= new Version(2, 3)))
						break;
					SetDagcOn(DagcOn);
					break;
				case "RegLowBat":
					lowBatMonitor = ((int)(r.Value >> 4) & 1) == 1;
					OnPropertyChanged("LowBatMonitor");
					LowBatOn = ((int)(r.Value >> 3) & 1) == 1;
					LowBatTrim = (LowBatTrimEnum)((int)r.Value & 7);
					break;
				case "RegListen1":
					if (((int)r.Value & 192) == 0)
						r.Value = (uint)(byte)((int)r.Value & 63 | 64);
					if (((int)r.Value & 48) == 0)
						r.Value = (uint)(byte)((int)r.Value & 207 | 16);
					if (((int)r.Value & 6) == 6)
						r.Value = (uint)(byte)((int)r.Value & 249 | 2);
					ListenResolIdle = (ListenResolEnum)((int)(r.Value >> 6) - 1 & 3);
					ListenResolRx = (ListenResolEnum)((int)(r.Value >> 4) - 1 & 3);
					ListenCriteria = (ListenCriteriaEnum)((int)(r.Value >> 3) & 1);
					ListenEnd = (ListenEndEnum)((int)(r.Value >> 1) & 3);
					break;
				case "RegListen2":
					switch (ListenResolIdle)
					{
						case ListenResolEnum.Res000064:
							ListenCoefIdle = (Decimal)r.Value * new Decimal(64, 0, 0, false, (byte)3);
							return;
						case ListenResolEnum.Res004100:
							ListenCoefIdle = (Decimal)r.Value * new Decimal(41, 0, 0, false, (byte)1);
							return;
						case ListenResolEnum.Res262000:
							ListenCoefIdle = (Decimal)r.Value * new Decimal(262);
							return;
						default:
							return;
					}
				case "RegListen3":
					switch (ListenResolRx)
					{
						case ListenResolEnum.Res000064:
							ListenCoefRx = (Decimal)r.Value * new Decimal(64, 0, 0, false, (byte)3);
							return;
						case ListenResolEnum.Res004100:
							ListenCoefRx = (Decimal)r.Value * new Decimal(41, 0, 0, false, (byte)1);
							return;
						case ListenResolEnum.Res262000:
							ListenCoefRx = (Decimal)r.Value * new Decimal(262);
							return;
						default:
							return;
					}
				case "RegVersion":
					Version = new Version((int)(r.Value >> 4), (int)r.Value & 15);
					break;
				case "RegPaLevel":
					if (((int)r.Value & 224) != 128 && ((int)r.Value & 224) != 64 && ((int)r.Value & 224) != 96)
						r.Value = (uint)((int)r.Value & 31 | 128);
					switch (r.Value >> 5 & 7U)
					{
						case 2U:
							PaMode = PaModeEnum.PA1;
							OutputPower = new Decimal(180, 0, 0, true, (byte)1) + (Decimal)(r.Value & 31U);
							return;
						case 3U:
							PaMode = PaModeEnum.PA1_PA2;
							if (!Pa20dBm)
							{
								OutputPower = new Decimal(140, 0, 0, true, (byte)1) + (Decimal)(r.Value & 31U);
								return;
							}
							else
							{
								OutputPower = new Decimal(110, 0, 0, true, (byte)1) + (Decimal)(r.Value & 31U);
								return;
							}
						case 4U:
							PaMode = PaModeEnum.PA0;
							OutputPower = new Decimal(180, 0, 0, true, (byte)1) + (Decimal)(r.Value & 31U);
							return;
						default:
							return;
					}
				case "RegPaRamp":
					PaRamp = (PaRampEnum)((int)r.Value & 15);
					break;
				case "RegOcp":
					OcpOn = ((int)(r.Value >> 4) & 1) == 1;
					OcpTrim = (Decimal)((uint)(45 + 5 * ((int)r.Value & 15)));
					break;
				case "RegAgcRef":
				case "Reserved14":
					AgcAutoRefOn = ((int)(r.Value >> 6) & 1) == 1;
					AgcRefLevel = (int)(-80L - (long)(r.Value & 63U));
					OnPropertyChanged("AgcReference");
					OnPropertyChanged("AgcThresh1");
					OnPropertyChanged("AgcThresh2");
					OnPropertyChanged("AgcThresh3");
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					break;
				case "RegAgcThresh1":
				case "Reserved15":
					AgcSnrMargin = (byte)(r.Value >> 5);
					AgcStep1 = (byte)(r.Value & 31U);
					OnPropertyChanged("AgcReference");
					OnPropertyChanged("AgcThresh1");
					OnPropertyChanged("AgcThresh2");
					OnPropertyChanged("AgcThresh3");
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					break;
				case "RegAgcThresh2":
				case "Reserved16":
					AgcStep2 = (byte)(r.Value >> 4);
					AgcStep3 = (byte)(r.Value & 15U);
					OnPropertyChanged("AgcThresh2");
					OnPropertyChanged("AgcThresh3");
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					break;
				case "RegAgcThresh3":
				case "Reserved17":
					AgcStep4 = (byte)(r.Value >> 4);
					AgcStep5 = (byte)(r.Value & 15U);
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					break;
				case "RegLna":
					LnaZin = ((int)r.Value & 128) == 128 ? LnaZinEnum.ZIN_200 : LnaZinEnum.ZIN_50;
					LnaLowPowerOn = ((int)r.Value & 64) == 64;
					LnaCurrentGain = (LnaGainEnum)((r.Value & 56U) >> 3);
					LnaGainSelect = (LnaGainEnum)((int)r.Value & 7);
					break;
				case "RegRxBw":
					int mant1;
					switch ((r.Value & 24U) >> 3)
					{
						case 0U:
							mant1 = 16;
							break;
						case 1U:
							mant1 = 20;
							break;
						case 2U:
							mant1 = 24;
							break;
						default:
							throw new Exception("Invalid RxBwMant parameter");
					}
					rxBw = SX1231.ComputeRxBw(frequencyXo, modulationType, mant1, (int)r.Value & 7);
					BitRateFdevCheck(bitRate, fdev);
					OnPropertyChanged("DccFreqMin");
					OnPropertyChanged("DccFreqMax");
					OnPropertyChanged("RxBwMin");
					OnPropertyChanged("RxBwMax");
					OnPropertyChanged("RxBw");
					OnPropertyChanged("AgcReference");
					OnPropertyChanged("AgcThresh1");
					OnPropertyChanged("AgcThresh2");
					OnPropertyChanged("AgcThresh3");
					OnPropertyChanged("AgcThresh4");
					OnPropertyChanged("AgcThresh5");
					DccFreq = ComputeDccFreq(rxBw, r.Value);
					break;
				case "RegAfcBw":
					int mant2;
					switch ((r.Value & 24U) >> 3)
					{
						case 0U:
							mant2 = 16;
							break;
						case 1U:
							mant2 = 20;
							break;
						case 2U:
							mant2 = 24;
							break;
						default:
							throw new Exception("Invalid RxBwMant parameter");
					}
					afcRxBw = SX1231.ComputeRxBw(frequencyXo, modulationType, mant2, (int)r.Value & 7);
					OnPropertyChanged("AfcDccFreqMin");
					OnPropertyChanged("AfcDccFreqMax");
					OnPropertyChanged("AfcRxBwMin");
					OnPropertyChanged("AfcRxBwMax");
					OnPropertyChanged("AfcRxBw");
					AfcDccFreq = ComputeDccFreq(afcRxBw, r.Value);
					break;
				case "RegOokPeak":
					OokThreshType = (OokThreshTypeEnum)(r.Value >> 6);
					OokPeakThreshStep = OoPeakThreshStepTable[((r.Value & 56U) >> 3)];
					OokPeakThreshDec = (OokPeakThreshDecEnum)((int)r.Value & 7);
					break;
				case "RegOokAvg":
					OokAverageThreshFilt = (OokAverageThreshFiltEnum)(r.Value >> 6);
					break;
				case "RegOokFix":
					OokFixedThresh = (byte)r.Value;
					break;
				case "RegAfcFei":
					feiDone = ((int)(r.Value >> 6) & 1) == 1;
					OnPropertyChanged("FeiDone");
					afcDone = ((int)(r.Value >> 4) & 1) == 1;
					OnPropertyChanged("AfcDone");
					AfcAutoClearOn = ((int)(r.Value >> 3) & 1) == 1;
					AfcAutoOn = ((int)(r.Value >> 2) & 1) == 1;
					break;
				case "RegAfcMsb":
				case "RegAfcLsb":
					AfcValue = (Decimal)((short)((int)registers["RegAfcMsb"].Value << 8 | (int)registers["RegAfcLsb"].Value)) * FrequencyStep;
					break;
				case "RegFeiMsb":
				case "RegFeiLsb":
					FeiValue = (Decimal)((short)((int)registers["RegFeiMsb"].Value << 8 | (int)registers["RegFeiLsb"].Value)) * FrequencyStep;
					break;
				case "RegRssiConfig":
					FastRx = ((int)(r.Value >> 3) & 1) == 1;
					rssiDone = ((int)(r.Value >> 1) & 1) == 1;
					OnPropertyChanged("RssiDone");
					break;
				case "RegRssiValue":
					prevRssiValue = rssiValue;
					rssiValue = -(Decimal)r.Value / new Decimal(20, 0, 0, false, (byte)1);
					if (RfPaSwitchEnabled != 0)
					{
						if (RfPaSwitchSel == RfPaSwitchSelEnum.RF_IO_RFIO)
						{
							if (RfPaSwitchEnabled == 1)
								rfPaRssiValue = new Decimal(1277, 0, 0, true, (byte)1);
							rfIoRssiValue = rssiValue;
							OnPropertyChanged("RfIoRssiValue");
						}
						else if (RfPaSwitchSel == RfPaSwitchSelEnum.RF_IO_PA_BOOST)
						{
							if (RfPaSwitchEnabled == 1)
								rfIoRssiValue = new Decimal(1277, 0, 0, true, (byte)1);
							rfPaRssiValue = rssiValue;
							OnPropertyChanged("RfPaRssiValue");
						}
					}
					spectrumRssiValue = rssiValue;
					OnPropertyChanged("RssiValue");
					OnPropertyChanged("SpectrumData");
					break;
				case "RegDioMapping1":
					Dio0Mapping = (DioMappingEnum)((int)(r.Value >> 6) & 3);
					Dio1Mapping = (DioMappingEnum)((int)(r.Value >> 4) & 3);
					Dio2Mapping = (DioMappingEnum)((int)(r.Value >> 2) & 3);
					Dio3Mapping = (DioMappingEnum)((int)r.Value & 3);
					break;
				case "RegDioMapping2":
					Dio4Mapping = (DioMappingEnum)((int)(r.Value >> 6) & 3);
					Dio5Mapping = (DioMappingEnum)((int)(r.Value >> 4) & 3);
					ClockOut = (ClockOutEnum)((int)r.Value & 7);
					break;
				case "RegIrqFlags1":
					modeReady = ((int)(r.Value >> 7) & 1) == 1;
					OnPropertyChanged("ModeReady");
					bool flag = ((int)(r.Value >> 6) & 1) == 1;
					if (!rxReady && flag)
						restartRx = true;
					rxReady = flag;
					OnPropertyChanged("RxReady");
					txReady = ((int)(r.Value >> 5) & 1) == 1;
					OnPropertyChanged("TxReady");
					pllLock = ((int)(r.Value >> 4) & 1) == 1;
					OnPropertyChanged("PllLock");
					rssi = ((int)(r.Value >> 3) & 1) == 1;
					OnPropertyChanged("Rssi");
					timeout = ((int)(r.Value >> 2) & 1) == 1;
					OnPropertyChanged("Timeout");
					autoMode = ((int)(r.Value >> 1) & 1) == 1;
					OnPropertyChanged("AutoMode");
					syncAddressMatch = ((int)r.Value & 1) == 1;
					OnPropertyChanged("SyncAddressMatch");
					break;
				case "RegIrqFlags2":
					fifoFull = ((int)(r.Value >> 7) & 1) == 1;
					OnPropertyChanged("FifoFull");
					fifoNotEmpty = ((int)(r.Value >> 6) & 1) == 1;
					OnPropertyChanged("FifoNotEmpty");
					fifoLevel = ((int)(r.Value >> 5) & 1) == 1;
					OnPropertyChanged("FifoLevel");
					fifoOverrun = ((int)(r.Value >> 4) & 1) == 1;
					OnPropertyChanged("FifoOverrun");
					packetSent = ((int)(r.Value >> 3) & 1) == 1;
					OnPropertyChanged("PacketSent");
					payloadReady = ((int)(r.Value >> 2) & 1) == 1;
					OnPropertyChanged("PayloadReady");
					crcOk = ((int)(r.Value >> 1) & 1) == 1;
					OnPropertyChanged("CrcOk");
					lowBat = ((int)r.Value & 1) == 1;
					OnPropertyChanged("LowBat");
					break;
				case "RegRssiThresh":
					RssiThresh = -(Decimal)r.Value / new Decimal(20, 0, 0, false, (byte)1);
					break;
				case "RegRxTimeout1":
					TimeoutRxStart = (Decimal)r.Value * new Decimal(16) * Tbit * new Decimal(1000);
					break;
				case "RegRxTimeout2":
					TimeoutRssiThresh = (Decimal)r.Value * new Decimal(16) * Tbit * new Decimal(1000);
					break;
				case "RegPreambleMsb":
				case "RegPreambleLsb":
					packet.PreambleSize = (int)registers["RegPreambleMsb"].Value << 8 | (int)registers["RegPreambleLsb"].Value;
					break;
				case "RegSyncConfig":
					packet.SyncOn = ((int)(r.Value >> 7) & 1) == 1;
					packet.FifoFillCondition = ((int)(r.Value >> 6) & 1) == 1 ? FifoFillConditionEnum.Allways : FifoFillConditionEnum.OnSyncAddressIrq;
					packet.SyncSize = (byte)(((r.Value & 56U) >> 3) + 1U);
					UpdateSyncValue();
					packet.SyncTol = (byte)(r.Value & 7U);
					break;
				case "RegSyncValue1":
				case "RegSyncValue2":
				case "RegSyncValue3":
				case "RegSyncValue4":
				case "RegSyncValue5":
				case "RegSyncValue6":
				case "RegSyncValue7":
				case "RegSyncValue8":
					UpdateSyncValue();
					break;
				case "RegPacketConfig1":
					packet.PacketFormat = ((int)(r.Value >> 7) & 1) == 1 ? PacketFormatEnum.Variable : PacketFormatEnum.Fixed;
					packet.DcFree = (DcFreeEnum)((int)(r.Value >> 5) & 3);
					packet.CrcOn = ((int)(r.Value >> 4) & 1) == 1;
					packet.CrcAutoClearOff = ((int)(r.Value >> 3) & 1) == 1;
					packet.AddressFiltering = (AddressFilteringEnum)((int)(r.Value >> 1) & 3);
					packet.CrcIbmOn = ((int)r.Value & 1) == 1;
					break;
				case "RegPayloadLength":
					packet.PayloadLength = (byte)r.Value;
					break;
				case "RegNodeAdrs":
					packet.NodeAddress = (byte)r.Value;
					break;
				case "RegBroadcastAdrs":
					packet.BroadcastAddress = (byte)r.Value;
					break;
				case "RegAutoModes":
					packet.EnterCondition = (EnterConditionEnum)((r.Value & 224U) >> 5);
					packet.ExitCondition = (ExitConditionEnum)((r.Value & 28U) >> 2);
					packet.IntermediateMode = (IntermediateModeEnum)((int)r.Value & 3);
					break;
				case "RegFifoThresh":
					packet.TxStartCondition = ((int)(r.Value >> 7) & 1) == 1;
					packet.FifoThreshold = (byte)(r.Value & (uint)sbyte.MaxValue);
					break;
				case "RegPacketConfig2":
					packet.InterPacketRxDelay = (int)(r.Value >> 4);
					packet.AutoRxRestartOn = ((int)(r.Value >> 1) & 1) == 1;
					packet.AesOn = ((int)r.Value & 1) == 1;
					break;
				case "RegAesKey1":
				case "RegAesKey2":
				case "RegAesKey3":
				case "RegAesKey4":
				case "RegAesKey5":
				case "RegAesKey6":
				case "RegAesKey7":
				case "RegAesKey8":
				case "RegAesKey9":
				case "RegAesKey10":
				case "RegAesKey11":
				case "RegAesKey12":
				case "RegAesKey13":
				case "RegAesKey14":
				case "RegAesKey15":
				case "RegAesKey16":
					UpdateAesKey();
					break;
				case "RegTemp1":
					AdcLowPowerOn = ((int)r.Value & 1) == 1;
					break;
				case "RegTemp2":
					tempValue = (Decimal)((int)-(byte)r.Value) + tempValueRoom + tempValueCal;
					OnPropertyChanged("TempValue");
					break;
				case "RegTestLna":
					SensitivityBoostOn = (int)r.Value == 45;
					break;
				case "RegTestPa1":
					Pa20dBm = (int)r.Value == 93 && (int)registers["RegTestPa2"].Value == 124;
					ReadRegister(registers["RegPaLevel"]);
					break;
				case "RegTestPa2":
					Pa20dBm = (int)registers["RegTestPa1"].Value == 93 && (int)r.Value == 124;
					ReadRegister(registers["RegPaLevel"]);
					break;
				case "RegTestDagc":
					DagcOn = ((int)r.Value & 48) == 48 || ((int)r.Value & 32) == 32;
					break;
				case "RegTestAfc":
					LowBetaAfcOffset = (Decimal)((sbyte)r.Value) * new Decimal(4880, 0, 0, false, (byte)1);
					break;
			}
		}

		private Decimal ComputeDccFreq(Decimal bw, uint register)
		{
			return new Decimal(40, 0, 0, false, (byte)1) * bw / new Decimal(340449852, 1462918, 0, false, (byte)15) * (Decimal)Math.Pow(2.0, (double)((register >> 5) + 2U));
		}

		private Decimal ComputeRxBwMin()
		{
			if (ModulationType == ModulationTypeEnum.FSK)
				return FrequencyXo / new Decimal(24) * (Decimal)Math.Pow(2.0, 9.0);
			else
				return FrequencyXo / new Decimal(24) * (Decimal)Math.Pow(2.0, 10.0);
		}

		private Decimal ComputeRxBwMax()
		{
			if (ModulationType == ModulationTypeEnum.FSK)
				return FrequencyXo / new Decimal(16) * (Decimal)Math.Pow(2.0, 2.0);
			else
				return FrequencyXo / new Decimal(16) * (Decimal)Math.Pow(2.0, 3.0);
		}

		public static Decimal ComputeRxBw(Decimal frequencyXo, ModulationTypeEnum mod, int mant, int exp)
		{
			if (mod == ModulationTypeEnum.FSK)
				return frequencyXo / (Decimal)mant * (Decimal)Math.Pow(2.0, (double)(exp + 2));
			else
				return frequencyXo / (Decimal)mant * (Decimal)Math.Pow(2.0, (double)(exp + 3));
		}

		public static void ComputeRxBwMantExp(Decimal frequencyXo, ModulationTypeEnum mod, Decimal value, ref int mant, ref int exp)
		{
			Decimal num1 = new Decimal(0);
			Decimal num2 = new Decimal(10000000);
			for (int index = 0; index < 8; ++index)
			{
				int num3 = 16;
				while (num3 <= 24)
				{
					Decimal num4 = mod != ModulationTypeEnum.FSK ? frequencyXo / (Decimal)num3 * (Decimal)Math.Pow(2.0, (double)(index + 3)) : frequencyXo / (Decimal)num3 * (Decimal)Math.Pow(2.0, (double)(index + 2));
					if (Math.Abs(num4 - value) < num2)
					{
						num2 = Math.Abs(num4 - value);
						mant = num3;
						exp = index;
					}
					num3 += 4;
				}
			}
		}

		public static Decimal[] ComputeRxBwFreqTable(Decimal frequencyXo, ModulationTypeEnum mod)
		{
			Decimal[] numArray = new Decimal[24];
			int num1 = 0;
			for (int index = 0; index < 8; ++index)
			{
				int num2 = 16;
				while (num2 <= 24)
				{
					numArray[num1++] = mod != ModulationTypeEnum.FSK ? frequencyXo / (Decimal)num2 * (Decimal)Math.Pow(2.0, (double)(index + 3)) : frequencyXo / (Decimal)num2 * (Decimal)Math.Pow(2.0, (double)(index + 2));
					num2 += 4;
				}
			}
			return numArray;
		}

		private void BitRateFdevCheck(Decimal bitRate, Decimal fdev)
		{
			Decimal num1 = new Decimal(300000);
			Decimal num2 = new Decimal(600);
			if (bitRateFdevCheckDisbale)
				return;
			if (modulationType == ModulationTypeEnum.OOK)
				num1 = new Decimal(32768);
			if (bitRate < num2 || bitRate > num1)
				OnBitRateLimitStatusChanged(LimitCheckStatusEnum.OUT_OF_RANGE, "The bitrate is out of range.\nThe valid range is [" + num2.ToString() + ", " + num1.ToString() + "]");
			else
				OnBitRateLimitStatusChanged(LimitCheckStatusEnum.OK, "");
			if (modulationType != ModulationTypeEnum.OOK)
			{
				if (fdev < new Decimal(600) || fdev > new Decimal(300000))
					OnFdevLimitStatusChanged(LimitCheckStatusEnum.OUT_OF_RANGE, "The frequency deviation is out of range.\nThe valid range is [" + 600.ToString() + ", " + 300000.ToString() + "]");
				else if (fdev + bitRate / new Decimal(2) > RxBw)
				{
					OnFdevLimitStatusChanged(LimitCheckStatusEnum.ERROR, "The single sided band width has been exceeded.\n Fdev + ( Bitrate / 2 ) > " + RxBw.ToString() + " Hz");
				}
				else
				{
					Decimal num3 = new Decimal(20, 0, 0, false, (byte)1) * fdev / bitRate;
					if (new Decimal(4969, 0, 0, false, (byte)4) <= num3 && num3 <= new Decimal(100, 0, 0, false, (byte)1))
						OnFdevLimitStatusChanged(LimitCheckStatusEnum.OK, "");
					else
						OnFdevLimitStatusChanged(LimitCheckStatusEnum.ERROR, "The modulation index is out of range.\nThe valid range is [0.5, 10]");
				}
			}
			else
				OnFdevLimitStatusChanged(LimitCheckStatusEnum.OK, "");
		}

		private void FrequencyRfCheck(Decimal value)
		{
			if (frequencyRfCheckDisable)
				return;
			if (value < new Decimal(290000000) || value > new Decimal(340000000) && value < new Decimal(431000000) || (value > new Decimal(510000000) && value < new Decimal(862000000) || value > new Decimal(1020000000)))
			{
				string[] strArray = new string[3]
        {
          "[" + 290000000.ToString() + ", " + 340000000.ToString() + "]",
          "[" + 431000000.ToString() + ", " + 510000000.ToString() + "]",
          "[" + 862000000.ToString() + ", " + 1020000000.ToString() + "]"
        };
				OnFrequencyRfLimitStatusChanged(LimitCheckStatusEnum.OUT_OF_RANGE, "The RF frequency is out of range.\nThe valid ranges are:\n" + strArray[0] + "\n" + strArray[1] + "\n" + strArray[2]);
			}
			else
				OnFrequencyRfLimitStatusChanged(LimitCheckStatusEnum.OK, "");
		}

		private void SyncValueCheck(byte[] value)
		{
			int num = 0;
			if (value == null)
				++num;
			else if ((int)value[0] == 0)
				++num;
			if (num != 0)
				OnSyncValueLimitChanged(LimitCheckStatusEnum.ERROR, "First sync word byte must be different of 0!");
			else
				OnSyncValueLimitChanged(LimitCheckStatusEnum.OK, "");
		}

		private void PreambleCheck()
		{
		}

		private void OnConnected()
		{
			if (Connected != null)
				Connected(this, EventArgs.Empty);
		}

		private void OnDisconnected()
		{
			if (Disconected != null)
				Disconected(this, EventArgs.Empty);
		}

		private void OnError(byte status, string message)
		{
			if (Error != null)
				Error(this, new SemtechLib.General.Events.ErrorEventArgs(status, message));
		}

		private void OnFrequencyRfLimitStatusChanged(LimitCheckStatusEnum status, string message)
		{
			if (FrequencyRfLimitStatusChanged != null)
				FrequencyRfLimitStatusChanged(this, new LimitCheckStatusEventArg(status, message));
		}

		private void OnBitRateLimitStatusChanged(LimitCheckStatusEnum status, string message)
		{
			if (BitRateLimitStatusChanged != null)
				BitRateLimitStatusChanged(this, new LimitCheckStatusEventArg(status, message));
		}

		private void OnFdevLimitStatusChanged(LimitCheckStatusEnum status, string message)
		{
			if (FdevLimitStatusChanged != null)
				FdevLimitStatusChanged(this, new LimitCheckStatusEventArg(status, message));
		}

		private void OnSyncValueLimitChanged(LimitCheckStatusEnum status, string message)
		{
			if (SyncValueLimitChanged != null)
				SyncValueLimitChanged(this, new LimitCheckStatusEventArg(status, message));
		}

		public bool Open(string name)
		{
			try
			{
				deviceName = name;
				Close();
				if (ftdi.Open(name))
				{
					if (ftdi.PortA.Init(spiSpeed))
					{
						if (ftdi.PortB.Init(1000000U))
						{
							ftdi.PortA.PortDir = 11;
							ftdi.PortA.PortValue = 14;

							ftdi.PortB.PortDir = 192;
							ftdi.PortB.PortValue = (byte)(test ? 192 : 0);
							ftdi.PortB.SendBytes();
							isOpen = true;
							PopulateRegisters();
							regUpdateThreadContinue = true;
							regUpdateThread = new Thread(new ThreadStart(RegUpdateThread));
							regUpdateThread.Start();
							OnConnected();
							return true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
			return false;
		}

		public bool Close()
		{
			if (isOpen || ftdi != null && ftdi.IsOpen)
			{
				ftdi.Close();
				isOpen = false;
			}
			return true;
		}

		public bool Read(byte address, ref byte data)
		{
			Mpsse portA1 = ftdi.PortA;
			int num1 = (int)(byte)((uint)portA1.PortValue & 247U);
			portA1.PortValue = (byte)num1;
			ftdi.PortA.ScanOut(8, new byte[1]
      {
        (byte) ((uint) address & (uint) sbyte.MaxValue)
      }, 1 != 0);
			ftdi.PortA.ScanIn(8, true);
			Mpsse portA2 = ftdi.PortA;
			int num2 = (int)(byte)((uint)portA2.PortValue | 8U);
			portA2.PortValue = (byte)num2;
			ftdi.PortA.TxBufferAdd((byte)135);
			bool flag = ftdi.PortA.SendBytes();
			byte[] rxBuffer = new byte[1];
			if (!flag || !ftdi.PortA.ReadBytes(out rxBuffer, 8U))
				return false;
			data = rxBuffer[rxBuffer.Length - 1];
			return true;
		}

		public bool Read(byte address, ref byte[] data)
		{
			Mpsse portA1 = ftdi.PortA;
			int num1 = (int)(byte)((uint)portA1.PortValue & 247U);
			portA1.PortValue = (byte)num1;
			ftdi.PortA.ScanOut(8, new byte[1]
      {
        (byte) ((uint) address & (uint) sbyte.MaxValue)
      }, 1 != 0);
			for (int index = 0; index < data.Length; ++index)
				ftdi.PortA.ScanIn(8, true);
			Mpsse portA2 = ftdi.PortA;
			int num2 = (int)(byte)((uint)portA2.PortValue | 8U);
			portA2.PortValue = (byte)num2;
			ftdi.PortA.TxBufferAdd((byte)135);
			bool flag = ftdi.PortA.SendBytes();
			byte[] rxBuffer = new byte[1];
			if (flag && ftdi.PortA.ReadBytes(out rxBuffer, (uint)(data.Length * 8)))
			{
				Array.Copy((Array)rxBuffer, rxBuffer.Length - data.Length, (Array)data, 0, data.Length);
				return true;
			}
			else
			{
				data = (byte[])null;
				return false;
			}
		}

		private bool ReadFifo(ref byte[] data)
		{
			return Read((byte)0, ref data);
		}

		public bool Write(byte address, byte data)
		{
			Mpsse portA1 = ftdi.PortA;
			int num1 = (int)(byte)((uint)portA1.PortValue & 247U);
			portA1.PortValue = (byte)num1;
			ftdi.PortA.ScanOut(8, new byte[1]
      {
        (byte) ((uint) address | 128U)
      }, 1 != 0);
			ftdi.PortA.ScanOut(8, new byte[1]
      {
        data
      }, 1 != 0);
			Mpsse portA2 = ftdi.PortA;
			int num2 = (int)(byte)((uint)portA2.PortValue | 8U);
			portA2.PortValue = (byte)num2;
			ftdi.PortA.TxBufferAdd((byte)135);
			return ftdi.PortA.SendBytes();
		}

		public bool Write(byte address, byte[] data)
		{
			Mpsse portA1 = ftdi.PortA;
			int num1 = (int)(byte)((uint)portA1.PortValue & 247U);
			portA1.PortValue = (byte)num1;
			ftdi.PortA.ScanOut(8, new byte[1]
      {
        (byte) ((uint) address | 128U)
      }, 1 != 0);
			for (int index = 0; index < data.Length; ++index)
				ftdi.PortA.ScanOut(8, new byte[1]
        {
          data[index]
        }, 1 != 0);
			Mpsse portA2 = ftdi.PortA;
			int num2 = (int)(byte)((uint)portA2.PortValue | 8U);
			portA2.PortValue = (byte)num2;
			ftdi.PortA.TxBufferAdd((byte)135);
			return ftdi.PortA.SendBytes();
		}

		public bool WriteFifo(byte[] data)
		{
			return Write((byte)0, data);
		}

		public void SendData(byte[] buffer)
		{
			throw new NotImplementedException();
		}

		private void SetModeLeds(OperatingModeEnum mode)
		{
			if (test)
				return;
			switch (mode)
			{
				case OperatingModeEnum.Tx:
					IoPort portB1 = ftdi.PortB;
					int num1 = (int)(byte)((uint)portB1.PortValue & 63U);
					portB1.PortValue = (byte)num1;
					if (isPacketModeRunning)
					{
						IoPort portB2 = ftdi.PortB;
						int num2 = (int)(byte)((uint)portB2.PortValue | 64U);
						portB2.PortValue = (byte)num2;
						break;
					}
					else
					{
						IoPort portB2 = ftdi.PortB;
						int num2 = (int)(byte)((uint)portB2.PortValue | 192U);
						portB2.PortValue = (byte)num2;
						break;
					}
				case OperatingModeEnum.Rx:
					IoPort portB3 = ftdi.PortB;
					int num3 = (int)(byte)((uint)portB3.PortValue & 63U);
					portB3.PortValue = (byte)num3;
					IoPort portB4 = ftdi.PortB;
					int num4 = (int)(byte)((uint)portB4.PortValue | 128U);
					portB4.PortValue = (byte)num4;
					ftdi.PortB.SendBytes();
					break;
				default:
					IoPort portB5 = ftdi.PortB;
					int num5 = (int)(byte)((uint)portB5.PortValue & 63U);
					portB5.PortValue = (byte)num5;
					break;
			}
			ftdi.PortB.SendBytes();
		}

		private bool WriteRegister(Register r, byte data)
		{
			lock (syncThread)
			{
				try
				{
					++writeLock;
					if (!Write((byte)r.Address, data))
						throw new Exception("Unable to read register: " + r.Name);
					else
						return true;
				}
				catch (Exception exception_0)
				{
					OnError(1, exception_0.Message);
					return false;
				}
				finally
				{
					--writeLock;
				}
			}
		}

		private bool ReadRegister(Register r)
		{
			byte data = 0;
			return ReadRegister(r, ref data);
		}

		private bool ReadRegister(Register r, ref byte data)
		{
			lock (syncThread)
			{
				try
				{
					++readLock;
					if (IsOpen)
					{
						if (!Read((byte)r.Address, ref data))
							throw new Exception("Unable to read register: " + r.Name);
						r.Value = (uint)data;
					}
					else
						UpdateRegisterValue(r);
					return true;
				}
				catch (Exception exception_0)
				{
					OnError(1, exception_0.Message);
					return false;
				}
				finally
				{
					--readLock;
				}
			}
		}

		private void ReadIrqFlags()
		{
			ReadRegister(registers["RegIrqFlags1"]);
			ReadRegister(registers["RegIrqFlags2"]);
		}

		public void Open(ref FileStream stream)
		{
			OnError(0, "-");
			StreamReader streamReader = new StreamReader((Stream)stream, Encoding.ASCII);
			int num1 = 1;
			int num2 = 0;
			string data = "";
			try
			{
				string str;
				while ((str = streamReader.ReadLine()) != null)
				{
					if ((int)str[0] == 35)
					{
						++num1;
					}
					else
					{
						if ((int)str[0] != 82 && (int)str[0] != 80)
							throw new Exception("At line " + num1.ToString() + ": A configuration line must start either by\n\"#\" for comments\nor a\n\"R\" for the register name.");
						string[] strArray = str.Split(new char[1] { '\t' });
						if (strArray.Length != 4)
						{
							if (strArray.Length != 2)
							{
								throw new Exception("At line " + num1.ToString() + ": The number of columns is " + strArray.Length.ToString() + " and it should be 4 or 2.");
							}
							else
							{
								if (!(strArray[0] == "PKT"))
									throw new Exception("At line " + num1.ToString() + ": Invalid Packet.");
								data = strArray[1];
							}
						}
						else
						{
							bool flag = true;
							for (int index = 0; index < registers.Count; ++index)
							{
								if (registers[index].Name == strArray[1])
								{
									flag = false;
									break;
								}
								else
								{
									switch (strArray[1])
									{
										case "RegAgcThres1":
											strArray[1] = "RegAgcThresh1";
											flag = false;
											break;
										case "RegAgcThres2":
											strArray[1] = "RegAgcThresh2";
											flag = false;
											break;
										case "RegAgcThres3":
											strArray[1] = "RegAgcThresh3";
											flag = false;
											break;
									}
									if (Version <= new Version(2, 1))
									{
										switch (strArray[1])
										{
											case "RegAfcCtrl":
												strArray[1] = "RegOsc2";
												flag = false;
												break;
											case "Reserved14":
												strArray[1] = "RegAgcRef";
												flag = false;
												break;
											case "Reserved15":
												strArray[1] = "RegAgcThresh1";
												flag = false;
												break;
											case "Reserved16":
												strArray[1] = "RegAgcThresh2";
												flag = false;
												break;
											case "Reserved17":
												strArray[1] = "RegAgcThresh3";
												flag = false;
												break;
											case "RegTestLna":
												flag = false;
												break;
											case "RegTestAfc":
												flag = false;
												break;
											case "RegTestDagc":
												flag = false;
												break;
										}
									}
									else
									{
										switch (strArray[1])
										{
											case "RegOsc2":
												strArray[1] = "RegAfcCtrl";
												flag = false;
												break;
											case "RegAgcRef":
												strArray[1] = "Reserved14";
												flag = false;
												break;
											case "RegAgcThresh1":
												strArray[1] = "Reserved15";
												flag = false;
												break;
											case "RegAgcThresh2":
												strArray[1] = "Reserved16";
												flag = false;
												break;
											case "RegAgcThresh3":
												strArray[1] = "Reserved17";
												flag = false;
												break;
										}
									}
									if (!flag)
										break;
								}
							}
							if (flag)
								throw new Exception("At line " + num1.ToString() + ": Invalid register name.");
							if (strArray[1] != "RegVersion" && (!(Version <= new Version(2, 1)) || !(strArray[1] == "RegTestLna") && !(strArray[1] == "RegTestAfc") && !(strArray[1] == "RegTestDagc")))
							{
								registers[strArray[1]].Value = (uint)Convert.ToByte(strArray[3], 16);
								++num2;
							}
						}
						++num1;
					}
				}
				packet.SetSaveData(data);
				if (!(Version >= new Version(2, 4)))
					return;
				SetPa20dBm(Pa20dBm);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
			finally
			{
				streamReader.Close();
				if (!IsOpen)
					ReadRegisters();
			}
		}

		public void Save(ref FileStream stream)
		{
			OnError(0, "-");
			StreamWriter streamWriter = new StreamWriter((Stream)stream, Encoding.ASCII);
			try
			{
				streamWriter.WriteLine("#Type\tRegister Name\tAddress[Hex]\tValue[Hex]");
				for (int index = 0; index < registers.Count; ++index)
					streamWriter.WriteLine("REG\t{0}\t0x{1}\t0x{2}", (object)registers[index].Name, (object)registers[index].Address.ToString("X02"), (object)registers[index].Value.ToString("X02"));
				streamWriter.WriteLine("PKT\t{0}", (object)packet.GetSaveData());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				streamWriter.Close();
			}
		}

		public void Reset()
		{
			lock (syncThread)
			{
				try
				{
					bool local_0 = SpectrumOn;
					if (SpectrumOn)
						SpectrumOn = false;
					tempCalDone = false;
					PacketHandlerStop();
					byte local_1 = (byte)32;
					Mpsse temp_14 = ftdi.PortA;
					int temp_18 = (int)(byte)((uint)temp_14.PortDir | (uint)local_1);
					temp_14.PortDir = (byte)temp_18;
					Mpsse temp_21 = ftdi.PortA;
					int temp_25 = (int)(byte)((uint)temp_21.PortValue | (uint)local_1);
					temp_21.PortValue = (byte)temp_25;
					if (!ftdi.PortA.SendBytes())
						throw new Exception("Unable to send bytes over USB device");
					Thread.Sleep(1);
					Mpsse temp_33 = ftdi.PortA;
					int temp_39 = (int)(byte)((uint)temp_33.PortDir & (uint)~local_1);
					temp_33.PortDir = (byte)temp_39;
					Mpsse temp_42 = ftdi.PortA;
					int temp_48 = (int)(byte)((uint)temp_42.PortValue & (uint)~local_1);
					temp_42.PortValue = (byte)temp_48;
					if (!ftdi.PortA.SendBytes())
						throw new Exception("Unable to send bytes over USB device");
					Thread.Sleep(5);
					ReadRegisters();
					if (Version <= new Version(2, 1))
						RcCalStart();
					SetDefaultValues();
					ReadRegisters();
					RfPaSwitchEnabled = 0;
					RfPaSwitchSel = RfPaSwitchSelEnum.RF_IO_RFIO;
					if (!local_0)
						return;
					SpectrumOn = true;
				}
				catch (Exception exception_0)
				{
					OnError(1, exception_0.Message);
				}
			}
		}

		public void SetDefaultValues()
		{
			TempCalDone = false;
			if (IsOpen)
			{
				if (Version <= new Version(2, 1))
				{
					if (!Write((byte)registers["RegListen1"].Address, 162))
						throw new Exception("Unable to write register: " + registers["RegListen1"].Name);
					if (!Write((byte)registers["RegOcp"].Address, 27))
						throw new Exception("Unable to write register: " + registers["RegOcp"].Name);
				}
				if (!Write((byte)registers["RegLna"].Address, new byte[3] { 136, 85, 139 }))
					throw new Exception("Unable to write register: " + registers["RegLna"].Name);
				if (!Write((byte)registers["RegDioMapping2"].Address, (byte)7))
					throw new Exception("Unable to write register: " + registers["RegDioMapping2"].Name);
				if (!Write((byte)registers["RegRssiThresh"].Address, (byte)228))
					throw new Exception("Unable to write register: " + registers["RegRssiThresh"].Name);
				if (!Write((byte)registers["RegSyncValue1"].Address, new byte[8] { 1, 1, 1, 1, 1, 1, 1, 1 }))
					throw new Exception("Unable to write register: " + registers["RegSyncValue1"].Name);
				if (!Write((byte)registers["RegFifoThresh"].Address, (byte)143))
					throw new Exception("Unable to write register: " + registers["RegFifoThresh"].Name);
				if (Version >= new Version(2, 3) && !Write((byte)registers["RegTestDagc"].Address, (byte)48))
					throw new Exception("Unable to write register: " + registers["RegTestDagc"].Name);
				if (Version <= new Version(2, 1) && !Write((byte)110, (byte)12))
					throw new Exception("Unable to write register at address 0x6E ");
			}
			else
			{
				registers["RegLna"].Value = 136U;
				registers["RegRxBw"].Value = 85U;
				registers["RegAfcBw"].Value = 139U;
				registers["RegDioMapping2"].Value = 7U;
				registers["RegRssiThresh"].Value = 228U;
				registers["RegSyncValue1"].Value = 1U;
				registers["RegSyncValue2"].Value = 1U;
				registers["RegSyncValue3"].Value = 1U;
				registers["RegSyncValue4"].Value = 1U;
				registers["RegSyncValue5"].Value = 1U;
				registers["RegSyncValue6"].Value = 1U;
				registers["RegSyncValue7"].Value = 1U;
				registers["RegSyncValue8"].Value = 1U;
				registers["RegFifoThresh"].Value = 143U;
				if (Version >= new Version(2, 3))
					registers["RegTestDagc"].Value = 48U;
				ReadRegisters();
			}
		}

		public void ReadRegisters()
		{
			lock (syncThread)
			{
				try
				{
					++readLock;
					foreach (Register reg in registers)
					{
						if (reg.Address != 0)
							ReadRegister(reg);
					}
				}
				catch (Exception ex)
				{
					OnError(1, ex.Message);
				}
				finally
				{
					--readLock;
				}
			}
		}

		public void WriteRegisters()
		{
			lock (syncThread)
			{
				try
				{
					foreach (Register reg in registers)
					{
						if (reg.Address != 0 && !Write((byte)reg.Address, (byte)reg.Value))
							throw new Exception("Writing register " + reg.Name);
					}
				}
				catch (Exception ex)
				{
					OnError(1, ex.Message);
				}
			}
		}

		public void SetSequencer(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegOpMode"].Value = (registers["RegOpMode"].Value & 127U) | (value ? 0U : 128U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetListenMode(bool value)
		{
			try
			{
				if (Mode == OperatingModeEnum.Sleep)
					SetOperatingMode(OperatingModeEnum.Stdby);
				registers["RegOpMode"].Value = (registers["RegOpMode"].Value & 159U) | (value ? 64U : 32U);
				ReadRegister(registers["RegOpMode"]);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void ListenModeAbort()
		{
			try
			{
				registers["RegOpMode"].Value = ((registers["RegOpMode"].Value & 159U) | 32U);
				ReadRegister(registers["RegOpMode"]);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOperatingMode(OperatingModeEnum value)
		{
			SetOperatingMode(value, false);
		}

		public void SetOperatingMode(OperatingModeEnum value, bool isQuiet)
		{
			try
			{
				byte data = (byte)((registers["RegOpMode"].Value & 227U) | ((uint)value << 2));
				if (!isQuiet)
				{
					registers["RegOpMode"].Value = (uint)data;
				}
				else
				{
					lock (syncThread)
					{
						if (!Write((byte)registers["RegOpMode"].Address, data))
							throw new Exception("Unable to write register " + registers["RegOpMode"].Name);
						if (Mode != OperatingModeEnum.Rx)
							return;
						ReadRegister(registers["RegLna"]);
						ReadRegister(registers["RegFeiMsb"]);
						ReadRegister(registers["RegFeiLsb"]);
						ReadRegister(registers["RegAfcMsb"]);
						ReadRegister(registers["RegAfcLsb"]);
					}
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetDataMode(DataModeEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegDataModul"].Value = (registers["RegDataModul"].Value & 159U) | ((uint)value << 5);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetModulationType(ModulationTypeEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegDataModul"].Value = (registers["RegDataModul"].Value & 231U) | ((uint)value << 3);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetModulationShaping(byte value)
		{
			try
			{
				lock (syncThread)
					registers["RegDataModul"].Value = (registers["RegDataModul"].Value & 252U) | (uint)value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetBitRate(Decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0 = (byte)((long)Math.Round(frequencyXo / value, MidpointRounding.AwayFromZero) >> 8);
					byte local_1 = (byte)(long)Math.Round(frequencyXo / value, MidpointRounding.AwayFromZero);
					bitRateFdevCheckDisbale = true;
					registers["RegBitrateMsb"].Value = (uint)local_0;
					bitRateFdevCheckDisbale = false;
					registers["RegBitrateLsb"].Value = (uint)local_1;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetFdev(Decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte fdevMsb = (byte)((long)(value / frequencyStep) >> 8);
					byte fdevLsb = (byte)(long)(value / frequencyStep);
					bitRateFdevCheckDisbale = true;
					registers["RegFdevMsb"].Value = (uint)fdevMsb;
					bitRateFdevCheckDisbale = false;
					registers["RegFdevLsb"].Value = (uint)fdevLsb;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetFrequencyRf(Decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte frfMsb = (byte)((long)(value / frequencyStep) >> 16);
					byte frfMid = (byte)((long)(value / frequencyStep) >> 8);
					byte frfLsb = (byte)(long)(value / frequencyStep);
					frequencyRfCheckDisable = true;
					registers["RegFrfMsb"].Value = (uint)frfMsb;
					registers["RegFrfMid"].Value = (uint)frfMid;
					frequencyRfCheckDisable = false;
					registers["RegFrfLsb"].Value = (uint)frfLsb;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void RcCalStart()
		{
			lock (syncThread)
			{
				byte osc_value = 0;
				if (Mode == OperatingModeEnum.Stdby)
				{
					if (!Write(0x57, 0x80))
						throw new Exception("Unable to write register at address 0x57.");
					for (int idx = 0; idx < 2; ++idx)
					{
						ReadRegister(registers["RegOsc1"], ref osc_value);
						WriteRegister(registers["RegOsc1"], (byte)((uint)osc_value | 128U));
						DateTime timeout = DateTime.Now;
						bool is_timeout;
						do
						{
							osc_value = 0;
							ReadRegister(registers["RegOsc1"], ref osc_value);
							is_timeout = (DateTime.Now - timeout).TotalMilliseconds >= 1000.0;
						}
						while (((uint)osc_value & 64U) == 0 && !is_timeout);
						if (is_timeout)
							throw new Exception("RC oscillator calibration timeout.");
					}
					if (!Write(0x57, 0))
						throw new Exception("Unable to write register at address 0x57.");
				}
				else
				{
					MessageBox.Show("The chip must be in Standby mode in order to calibrate the RC oscillator!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					throw new Exception("The chip must be in Standby mode in order to calibrate the RC oscillator!");
				}
			}
		}

		public void SetAfcLowBetaOn(bool value)
		{
			if (!(Version > new Version(2, 1)))
				return;
			try
			{
				lock (syncThread)
					registers["RegAfcCtrl"].Value = (registers["RegAfcCtrl"].Value & 223U) | (value ? 32U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetLowBatOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegLowBat"].Value = (registers["RegLowBat"].Value & 247U) | (value ? 8U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetLowBatTrim(LowBatTrimEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegLowBat"].Value = (registers["RegLowBat"].Value & 248U) | (uint)value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetListenResolIdle(ListenResolEnum value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0_2 = (byte)((registers["RegListen1"].Value & 63U) | (((uint)value + 1) << 6));
					if (Version <= new Version(2, 1))
						local_0_2 = (byte)(((uint)local_0_2 & 207U) | (((uint)value + 1) << 4));
					registers["RegListen1"].Value = (uint)local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetListenResolRx(ListenResolEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegListen1"].Value = (registers["RegListen1"].Value & 207U) | (((uint)value + 1) << 4);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetListenCriteria(ListenCriteriaEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegListen1"].Value = (registers["RegListen1"].Value & 247U) | (value == ListenCriteriaEnum.RssiThresh ? 0U : 8U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetListenEnd(ListenEndEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegListen1"].Value = (registers["RegListen1"].Value & 249U) | ((uint)value << 1);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetListenCoefIdle(Decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0 = (byte)registers["RegListen2"].Value;
					switch (ListenResolIdle)
					{
						case ListenResolEnum.Res000064:
							local_0 = (byte)(value / new Decimal(64, 0, 0, false, 3));
							break;
						case ListenResolEnum.Res004100:
							local_0 = (byte)(value / new Decimal(41, 0, 0, false, 1));
							break;
						case ListenResolEnum.Res262000:
							local_0 = (byte)(value / new Decimal(262));
							break;
					}
					registers["RegListen2"].Value = (uint)local_0;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetListenCoefRx(Decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0 = (byte)registers["RegListen3"].Value;
					switch (ListenResolRx)
					{
						case ListenResolEnum.Res000064:
							local_0 = (byte)(value / new Decimal(64, 0, 0, false, 3));
							break;
						case ListenResolEnum.Res004100:
							local_0 = (byte)(value / new Decimal(41, 0, 0, false, 1));
							break;
						case ListenResolEnum.Res262000:
							local_0 = (byte)(value / new Decimal(262));
							break;
					}
					registers["RegListen3"].Value = (uint)local_0;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetPaMode(PaModeEnum value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0_1 = (byte)((uint)(byte)registers["RegPaLevel"].Value & 31U);
					byte local_0_2;
					switch (value)
					{
						case PaModeEnum.PA0:
							local_0_2 = (byte)((uint)local_0_1 | 128U);
							break;
						case PaModeEnum.PA1:
							local_0_2 = (byte)((uint)local_0_1 | 64U);
							break;
						case PaModeEnum.PA1_PA2:
							local_0_2 = (byte)((uint)local_0_1 | 96U);
							break;
						default:
							local_0_2 = (byte)((uint)local_0_1 | 128U);
							break;
					}
					registers["RegPaLevel"].Value = (uint)local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOutputPower(Decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte paLevel = (byte)(registers["RegPaLevel"].Value & 224U);
					switch (PaMode)
					{
						case PaModeEnum.PA0:
							if (value > 13)
								value = 13;
							else if (value < -18)
								value = -18;
							paLevel |= (byte)((uint)(value + 18) & 31U);
							break;
						case PaModeEnum.PA1:
							if (value > 13)
								value = 13;
							else if (value < -2)
								value = -2;
							paLevel |= (byte)((uint)(value + 18) & 31U);
							break;
						case PaModeEnum.PA1_PA2:
							if (!Pa20dBm)
							{
								if (value > 17)
									value = 17;
								else if (value < 2)
									value = 2;
								paLevel |= (byte)((uint)(value + 14) & 31U);
								break;
							}
							else
							{
								if (value > 20)
									value = 20;
								else if (value < 5)
									value = 5;
								paLevel |= (byte)((uint)(value + 11) & 31U);
								break;
							}
					}
					registers["RegPaLevel"].Value = (uint)paLevel;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetPaRamp(PaRampEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegPaRamp"].Value = (registers["RegPaRamp"].Value & 240U) | ((uint)value & 15U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOcpOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegOcp"].Value = (registers["RegOcp"].Value & 239U) | (value ? 16U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOcpTrim(Decimal value)
		{
			try
			{
				lock (syncThread)
					registers["RegOcp"].Value = (registers["RegOcp"].Value & 240U) | ((uint)((value - 45) / 5) & 15U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAgcAutoRefOn(bool value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0_2 = (byte)((
						(!(Version <= new Version(2, 1))
						? registers["Reserved14"].Value
						: registers["RegAgcRef"].Value) & 191U)
						| (value ? 64U : 0U));
					if (Version <= new Version(2, 1))
						registers["RegAgcRef"].Value = (uint)local_0_2;
					else
						registers["Reserved14"].Value = (uint)local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAgcRefLevel(int value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0_2 = (byte)((
						(!(Version <= new Version(2, 1))
						? (uint)(byte)registers["Reserved14"].Value
						: (uint)(byte)registers["RegAgcRef"].Value) & 192U) | ((uint)(-value - 80) & 63U));
					if (Version <= new Version(2, 1))
						registers["RegAgcRef"].Value = (uint)local_0_2;
					else
						registers["Reserved14"].Value = (uint)local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAgcSnrMargin(byte value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0_2 = (byte)((uint)(byte)((!(Version <= new Version(2, 1)) ? (uint)(byte)registers["Reserved15"].Value : (uint)(byte)registers["RegAgcThresh1"].Value) & 31U) | (uint)(byte)(((int)value & 7) << 5));
					if (Version <= new Version(2, 1))
						registers["RegAgcThresh1"].Value = (uint)local_0_2;
					else
						registers["Reserved15"].Value = (uint)local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAgcStep(byte id, byte value)
		{
			try
			{
				lock (syncThread)
				{
					Register local_1;
					switch (id)
					{
						case (byte)1:
							local_1 = !(Version <= new Version(2, 1)) ? registers["Reserved15"] : registers["RegAgcThresh1"];
							break;
						case (byte)2:
						case (byte)3:
							local_1 = !(Version <= new Version(2, 1)) ? registers["Reserved16"] : registers["RegAgcThresh2"];
							break;
						case (byte)4:
						case (byte)5:
							local_1 = !(Version <= new Version(2, 1)) ? registers["Reserved17"] : registers["RegAgcThresh3"];
							break;
						default:
							throw new Exception("Invalid AGC step ID!");
					}
					byte local_0 = (byte)local_1.Value;
					byte local_0_2;
					switch (id)
					{
						case (byte)1:
							local_0_2 = (byte)(((uint)local_0 & 224U) | (uint)value);
							break;
						case (byte)2:
							local_0_2 = (byte)(((uint)local_0 & 15U) | ((uint)value << 4));
							break;
						case (byte)3:
							local_0_2 = (byte)(((uint)local_0 & 240U) | ((uint)value & 15U));
							break;
						case (byte)4:
							local_0_2 = (byte)(((uint)local_0 & 15U) | ((uint)value << 4));
							break;
						case (byte)5:
							local_0_2 = (byte)(((uint)local_0 & 240U) | ((uint)value & 15U));
							break;
						default:
							throw new Exception("Invalid AGC step ID!");
					}
					local_1.Value = (uint)local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetLnaZin(LnaZinEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegLna"].Value = (registers["RegLna"].Value & 127U) | (value == LnaZinEnum.ZIN_200 ? 128U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetLnaLowPowerOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegLna"].Value = (registers["RegLna"].Value & 191U) | (value ? 64U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetLnaGainSelect(LnaGainEnum value)
		{
			try
			{
				lock (syncThread)
				{
					registers["RegLna"].Value = (registers["RegLna"].Value & 248U) | (uint)value;
					if (LnaGainSelect == LnaGainEnum.AGC)
						return;
					ReadRegister(registers["RegLna"]);
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetDccFreq(Decimal value)
		{
			try
			{
				lock (syncThread)
					registers["RegRxBw"].Value = (registers["RegRxBw"].Value & 31U) | ((uint)(int)(Math.Log10((double)(new Decimal(40, 0, 0, false, 1) * RxBw / new Decimal(340449852, 1462918, 0, false, 15) * value)) / Math.Log10(2.0) - 2.0) << 5);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetRxBw(Decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0_1 = (byte)((uint)(byte)registers["RegRxBw"].Value & 224U);
					int local_1 = 0;
					int local_2 = 0;
					SX1231.ComputeRxBwMantExp(frequencyXo, ModulationType, value, ref local_2, ref local_1);
					byte local_0_2;
					switch (local_2)
					{
						case 16:
							local_0_2 = (byte)((uint)local_0_1 | (uint)(byte)(local_1 & 7));
							break;
						case 20:
							local_0_2 = (byte)((uint)local_0_1 | (uint)(byte)(8 | local_1 & 7));
							break;
						case 24:
							local_0_2 = (byte)((uint)local_0_1 | (uint)(byte)(16 | local_1 & 7));
							break;
						default:
							throw new Exception("Invalid RxBwMant parameter");
					}
					registers["RegRxBw"].Value = (uint)local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAfcDccFreq(Decimal value)
		{
			try
			{
				lock (syncThread)
					registers["RegAfcBw"].Value =
						(registers["RegAfcBw"].Value & 31U)
						| ((uint)(Math.Log10((double)(new Decimal(40, 0, 0, false, 1) * AfcRxBw / new Decimal(340449852, 1462918, 0, false, 15) * value)) / Math.Log10(2.0) - 2.0) << 5);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAfcRxBw(Decimal value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0_1 = (byte)((uint)(byte)registers["RegAfcBw"].Value & 224U);
					int local_1 = 0;
					int local_2 = 0;
					SX1231.ComputeRxBwMantExp(frequencyXo, ModulationType, value, ref local_2, ref local_1);
					byte local_0_2;
					switch (local_2)
					{
						case 16:
							local_0_2 = (byte)((uint)local_0_1 | (uint)(byte)(local_1 & 7));
							break;
						case 20:
							local_0_2 = (byte)((uint)local_0_1 | (uint)(byte)(8 | local_1 & 7));
							break;
						case 24:
							local_0_2 = (byte)((uint)local_0_1 | (uint)(byte)(16 | local_1 & 7));
							break;
						default:
							throw new Exception("Invalid RxBwMant parameter");
					}
					registers["RegAfcBw"].Value = (uint)local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOokThreshType(OokThreshTypeEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegOokPeak"].Value = (registers["RegOokPeak"].Value & 63U) | (((uint)value & 3) << 6);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOokPeakThreshStep(Decimal value)
		{
			try
			{
				lock (syncThread)
					registers["RegOokPeak"].Value = (registers["RegOokPeak"].Value & 199U) | (((uint)Array.IndexOf<Decimal>(OoPeakThreshStepTable, value) & 7) << 3);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOokPeakThreshDec(OokPeakThreshDecEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegOokPeak"].Value = (registers["RegOokPeak"].Value & 248U) | ((uint)value & 7U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOokAverageThreshFilt(OokAverageThreshFiltEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegOokAvg"].Value = (registers["RegOokAvg"].Value & 63U) | (((uint)value & 3) << 6);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetOokFixedThresh(byte value)
		{
			try
			{
				lock (syncThread)
					registers["RegOokFix"].Value = (uint)value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetFeiStart()
		{
			lock (syncThread)
			{
				byte local_0 = (byte)0;
				ReadRegister(registers["RegAfcFei"], ref local_0);
				WriteRegister(registers["RegAfcFei"], (byte)((uint)local_0 | 32U));
				DateTime local_1 = DateTime.Now;
				byte local_0_1;
				bool local_3_1;
				do
				{
					local_0_1 = (byte)0;
					ReadRegister(registers["RegAfcFei"], ref local_0_1);
					local_3_1 = (DateTime.Now - local_1).TotalMilliseconds >= 1000.0;
				}
				while ((int)(byte)((uint)local_0_1 & 64U) == 0 && !local_3_1);
				if (local_3_1)
				{
					OnError(1, "FEI read timeout.");
				}
				else
				{
					ReadRegister(registers["RegFeiMsb"]);
					ReadRegister(registers["RegFeiLsb"]);
				}
			}
		}

		public void SetAfcStart()
		{
			lock (syncThread)
			{
				byte local_0 = (byte)0;
				ReadRegister(registers["RegAfcFei"], ref local_0);
				WriteRegister(registers["RegAfcFei"], (byte)((uint)local_0 | 1U));
				DateTime local_1 = DateTime.Now;
				byte local_0_1;
				bool local_3_1;
				do
				{
					local_0_1 = (byte)0;
					ReadRegister(registers["RegAfcFei"], ref local_0_1);
					local_3_1 = (DateTime.Now - local_1).TotalMilliseconds >= 1000.0;
				}
				while ((int)(byte)((uint)local_0_1 & 16U) == 0 && !local_3_1);
				if (local_3_1)
				{
					OnError(1, "AFC read timeout.");
				}
				else
				{
					ReadRegister(registers["RegAfcMsb"]);
					ReadRegister(registers["RegAfcLsb"]);
				}
			}
		}

		public void SetAfcAutoClearOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegAfcFei"].Value = (registers["RegAfcFei"].Value & 247U) | (value ? 8U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAfcAutoOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegAfcFei"].Value = (registers["RegAfcFei"].Value & 251U) | (value ? 4U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAfcClear()
		{
			try
			{
				lock (syncThread)
				{
					registers["RegAfcFei"].Value = registers["RegAfcFei"].Value | 2U;
					ReadRegister(registers["RegAfcMsb"]);
					ReadRegister(registers["RegAfcLsb"]);
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetFastRx(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegAfcFei"].Value = (registers["RegAfcFei"].Value & 247U) | (value ? 8U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetRssiStart()
		{
			lock (syncThread)
			{
				if (Mode != OperatingModeEnum.Rx || !RxReady)
					return;
				byte local_0 = (byte)0;
				ReadRegister(registers["RegRssiConfig"], ref local_0);
				WriteRegister(registers["RegRssiConfig"], (byte)((uint)local_0 | 1U));
				ReadRegister(registers["RegRssiConfig"]);
				ReadRegister(registers["RegRssiValue"]);
			}
		}

		public void SetDioMapping(byte id, DioMappingEnum value)
		{
			try
			{
				lock (syncThread)
				{
					Register local_1;
					switch (id)
					{
						case (byte)0:
						case (byte)1:
						case (byte)2:
						case (byte)3:
							local_1 = registers["RegDioMapping1"];
							break;
						case (byte)4:
						case (byte)5:
							local_1 = registers["RegDioMapping2"];
							break;
						default:
							throw new Exception("Invalid DIO ID!");
					}
					uint local_0 = (uint)(byte)local_1.Value;
					uint local_0_2;
					switch (id)
					{
						case (byte)0:
							local_0_2 = local_0 & 63U | (uint)value << 6;
							break;
						case (byte)1:
							local_0_2 = local_0 & 207U | (uint)value << 4;
							break;
						case (byte)2:
							local_0_2 = local_0 & 243U | (uint)value << 2;
							break;
						case (byte)3:
							local_0_2 = (uint)((DioMappingEnum)(local_0 & 252U) | value & DioMappingEnum.DIO_MAP_11);
							break;
						case (byte)4:
							local_0_2 = local_0 & 63U | (uint)value << 6;
							break;
						case (byte)5:
							local_0_2 = local_0 & 207U | (uint)value << 4;
							break;
						default:
							throw new Exception("Invalid DIO ID!");
					}
					local_1.Value = local_0_2;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetClockOut(ClockOutEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegDioMapping2"].Value = (uint)((ClockOutEnum)(registers["RegDioMapping2"].Value & 248U) | value & ClockOutEnum.CLOCK_OUT_111);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetRssiThresh(Decimal value)
		{
			try
			{
				lock (syncThread)
					registers["RegRssiThresh"].Value = (uint)(-value * new Decimal(2));
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetTimeoutRxStart(Decimal value)
		{
			try
			{
				lock (syncThread)
					registers["RegRxTimeout1"].Value = (uint)Math.Round(value / new Decimal(1000) / new Decimal(16) * Tbit, MidpointRounding.AwayFromZero);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetTimeoutRssiThresh(Decimal value)
		{
			try
			{
				lock (syncThread)
					registers["RegRxTimeout2"].Value = (uint)Math.Round(value / new Decimal(1000) / new Decimal(16) * Tbit, MidpointRounding.AwayFromZero);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		private void PacketHandlerStart()
		{
			lock (syncThread)
			{
				try
				{
					SetModeLeds(OperatingModeEnum.Sleep);
					packetNumber = 0;
					SetDataMode(DataModeEnum.Packet);
					if (Mode == OperatingModeEnum.Tx)
					{
						if ((int)packet.MessageLength == 0)
						{
							int temp_61 = (int)MessageBox.Show("Message must be at least one byte long", "SX1231SKB-PacketHandler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							throw new Exception("Message must be at least one byte long");
						}
						else
						{
							SetDioMapping((byte)0, DioMappingEnum.DIO_MAP_00);
							SetDioMapping((byte)1, DioMappingEnum.DIO_MAP_01);
						}
					}
					else if (Mode == OperatingModeEnum.Rx)
					{
						SetDioMapping((byte)0, DioMappingEnum.DIO_MAP_01);
						SetDioMapping((byte)1, DioMappingEnum.DIO_MAP_10);
					}
					frameTransmitted = false;
					frameReceived = false;
					if (Mode == OperatingModeEnum.Tx)
					{
						SetOperatingMode(OperatingModeEnum.Tx, true);
						firstTransmit = true;
					}
					else
					{
						SetOperatingMode(OperatingModeEnum.Rx, true);
						OnPacketHandlerReceived();
					}
					isPacketModeRunning = true;
					OnPacketHandlerStarted();
				}
				catch (Exception exception_0)
				{
					OnError(1, exception_0.Message);
					PacketHandlerStop();
				}
			}
		}

		private void PacketHandlerStop()
		{
			try
			{
				lock (syncThread)
				{
					isPacketModeRunning = false;
					SetOperatingMode(Mode);
					frameTransmitted = false;
					frameReceived = false;
					firstTransmit = false;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
			finally
			{
				OnPacketHandlerStoped();
			}
		}

		private void PacketHandlerTransmit()
		{
			lock (syncThread)
			{
				try
				{
					SetModeLeds(OperatingModeEnum.Tx);
					if (maxPacketNumber != 0 && packetNumber >= maxPacketNumber || !isPacketModeRunning)
					{
						PacketHandlerStop();
					}
					else
					{
						frameTransmitted = TransmitRfData(packet.ToArray());
						++packetNumber;
					}
				}
				catch (Exception exception_0)
				{
					PacketHandlerStop();
					OnError(1, exception_0.Message);
				}
				finally
				{
					SetModeLeds(OperatingModeEnum.Sleep);
				}
			}
		}

		private void PacketHandlerReceive()
		{
			lock (syncThread)
			{
				try
				{
					SetModeLeds(OperatingModeEnum.Rx);
					byte local_0 = (byte)0;
					ReadRegister(registers["RegRssiValue"], ref local_0);
					packet.Rssi = -(Decimal)local_0 / new Decimal(20, 0, 0, false, (byte)1);
					byte[] local_1;
					frameReceived = ReceiveRfData(out local_1);
					if (packet.PacketFormat == PacketFormatEnum.Fixed)
					{
						if (packet.AddressFiltering != AddressFilteringEnum.OFF)
						{
							packet.NodeAddressRx = local_1[0];
							Array.Copy((Array)local_1, 1, (Array)local_1, 0, local_1.Length - 1);
							Array.Resize<byte>(ref local_1, (int)packet.PayloadLength - 1);
						}
						else
							Array.Resize<byte>(ref local_1, (int)packet.PayloadLength);
					}
					else if (packet.PacketFormat == PacketFormatEnum.Variable)
					{
						int local_2 = (int)local_1[0];
						Array.Copy((Array)local_1, 1, (Array)local_1, 0, local_1.Length - 1);
						Array.Resize<byte>(ref local_1, local_2);
						if (packet.AddressFiltering != AddressFilteringEnum.OFF)
						{
							int local_2_1 = local_2 - 1;
							packet.NodeAddressRx = local_1[0];
							Array.Copy((Array)local_1, 1, (Array)local_1, 0, local_1.Length - 1);
							Array.Resize<byte>(ref local_1, local_2_1);
						}
					}
					packet.Message = local_1;
					++packetNumber;
					OnPacketHandlerReceived();
					if (!isPacketModeRunning)
						PacketHandlerStop();
					SetModeLeds(OperatingModeEnum.Sleep);
				}
				catch (Exception exception_0)
				{
					PacketHandlerStop();
					OnError(1, exception_0.Message);
				}
			}
		}

		private void OnPacketHandlerStarted()
		{
			if (PacketHandlerStarted == null)
				return;
			PacketHandlerStarted((object)this, new EventArgs());
		}

		private void OnPacketHandlerStoped()
		{
			if (PacketHandlerStoped == null)
				return;
			PacketHandlerStoped((object)this, new EventArgs());
		}

		private void OnPacketHandlerTransmitted()
		{
			if (PacketHandlerTransmitted == null)
				return;
			PacketHandlerTransmitted((object)this, new PacketStatusEventArg(packetNumber, maxPacketNumber));
		}

		private void OnPacketHandlerReceived()
		{
			if (PacketHandlerReceived == null)
				return;
			PacketHandlerReceived((object)this, new PacketStatusEventArg(packetNumber, maxPacketNumber));
		}

		public void SetCrcEnable(bool value)
		{
			try
			{
				packet.CrcOn = value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetPreambleSize(int value)
		{
			try
			{
				lock (syncThread)
				{
					registers["RegPreambleMsb"].Value = (uint)value >> 8;
					registers["RegPreambleLsb"].Value = (uint)value & 0xFF;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetSyncOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegSyncConfig"].Value = (registers["RegSyncConfig"].Value & 127U) | (value ? 128U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetFifoFillCondition(FifoFillConditionEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegSyncConfig"].Value = (registers["RegSyncConfig"].Value & 191U) | (value == FifoFillConditionEnum.Allways ? 64U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetSyncSize(byte value)
		{
			try
			{
				lock (syncThread)
					registers["RegSyncConfig"].Value = (registers["RegSyncConfig"].Value & 199U) | (((uint)value - 1 & 7) << 3);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetSyncTol(byte value)
		{
			try
			{
				lock (syncThread)
					registers["RegSyncConfig"].Value = (uint)(byte)((uint)(byte)((uint)(byte)registers["RegSyncConfig"].Value & 248U) | (uint)(byte)((uint)value & 7U));
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetSyncValue(byte[] value)
		{
			try
			{
				lock (syncThread)
				{
					byte local_0 = (byte)registers["RegSyncValue1"].Address;
					for (int local_1 = 0; local_1 < value.Length; ++local_1)
						registers[(int)local_0 + local_1].Value = (uint)value[local_1];
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetPacketFormat(PacketFormatEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig1"].Value = (registers["RegPacketConfig1"].Value & 127U) | (value == PacketFormatEnum.Variable ? 128U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetDcFree(DcFreeEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig1"].Value = (registers["RegPacketConfig1"].Value & 159U) | (uint)(((uint)value & 3) << 5);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetCrcOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig1"].Value = (registers["RegPacketConfig1"].Value & 239U) | (value ? 16U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetCrcAutoClearOff(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig1"].Value = (registers["RegPacketConfig1"].Value & 247U) | (value ? 8U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetCrcIbmOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig1"].Value = (registers["RegPacketConfig1"].Value & 254U) | (value ? 1U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAddressFiltering(AddressFilteringEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig1"].Value = (registers["RegPacketConfig1"].Value & 249U) | (((uint)value & 3) << 1);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetPayloadLength(byte value)
		{
			try
			{
				lock (syncThread)
					registers["RegPayloadLength"].Value = (uint)value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetNodeAddress(byte value)
		{
			try
			{
				lock (syncThread)
					registers["RegNodeAdrs"].Value = (uint)value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetBroadcastAddress(byte value)
		{
			try
			{
				lock (syncThread)
					registers["RegBroadcastAdrs"].Value = (uint)value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetEnterCondition(EnterConditionEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegAutoModes"].Value = (registers["RegAutoModes"].Value & 31U) | (((uint)value & 7U) << 5);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetExitCondition(ExitConditionEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegAutoModes"].Value = (registers["RegAutoModes"].Value & 227U) | (((uint)value & 7U) << 2);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetIntermediateMode(IntermediateModeEnum value)
		{
			try
			{
				lock (syncThread)
					registers["RegAutoModes"].Value = (registers["RegAutoModes"].Value & 252U) | ((uint)value & 3U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetTxStartCondition(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegFifoThresh"].Value = (registers["RegFifoThresh"].Value & 127U) | (value ? 128U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetFifoThreshold(byte value)
		{
			try
			{
				lock (syncThread)
					registers["RegFifoThresh"].Value = (registers["RegFifoThresh"].Value & 128U) | ((uint)value & 127U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetInterPacketRxDelay(int value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig2"].Value = (registers["RegPacketConfig2"].Value & 15U) | ((uint)value << 4);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetRestartRx()
		{
			try
			{
				lock (syncThread)
				{
					registers["RegPacketConfig2"].Value = (registers["RegPacketConfig2"].Value & 251U) | 4U;
					restartRx = true;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAutoRxRestartOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig2"].Value = (registers["RegPacketConfig2"].Value & 253U) | (value ? 2U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAesOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegPacketConfig2"].Value = (registers["RegPacketConfig2"].Value & 254U) | (value ? 1U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetAesKey(byte[] value)
		{
			try
			{
				lock (syncThread)
				{
					byte regAesKey1 = (byte)registers["RegAesKey1"].Address;
					for (int idx = 0; idx < value.Length; ++idx)
						registers[(int)regAesKey1 + idx].Value = (uint)value[idx];
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetMessageLength(int value)
		{
			try
			{
				lock (syncThread)
				{ }
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetMessage(byte[] value)
		{
			try
			{
				lock (syncThread)
					packet.Message = value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetPacketHandlerStartStop(bool value)
		{
			try
			{
				lock (syncThread)
				{
					if (value)
						PacketHandlerStart();
					else
						PacketHandlerStop();
				}
			}
			catch (Exception ex)
			{
				PacketHandlerStop();
				OnError(1, ex.Message);
			}
		}

		public void SetMaxPacketNumber(int value)
		{
			try
			{
				lock (syncThread)
					maxPacketNumber = value;
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetPacketHandlerLogEnable(bool value)
		{
			try
			{
				lock (syncThread)
					packet.LogEnabled = value;
			}
			catch (Exception ex)
			{
				packet.LogEnabled = false;
				OnError(1, ex.Message);
			}
		}

		public bool TransmitRfData(byte[] buffer)
		{
			lock (syncThread)
			{
				try
				{
					SetOperatingMode(OperatingModeEnum.Stdby, true);
					Thread.Sleep(60);
					bool local_0_1 = WriteFifo(buffer);
					SetOperatingMode(OperatingModeEnum.Tx, true);
					return local_0_1;
				}
				catch (Exception exception_0)
				{
					throw exception_0;
				}
			}
		}

		public bool ReceiveRfData(out byte[] buffer)
		{
			lock (syncThread)
			{
				try
				{
					SetOperatingMode(OperatingModeEnum.Stdby, true);
					buffer = FifoData;
					bool local_0_1 = ReadFifo(ref buffer);
					SetOperatingMode(OperatingModeEnum.Rx, true);
					return local_0_1;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		private void packet_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(e.PropertyName);
		}

		public void SetTempMeasStart(bool calibrating)
		{
			lock (syncThread)
			{
				if (!calibrating && !TempCalDone || Mode != OperatingModeEnum.Stdby && Mode != OperatingModeEnum.Fs)
					return;
				byte local_0 = (byte)0;
				ReadRegister(registers["RegTemp1"], ref local_0);
				WriteRegister(registers["RegTemp1"], (byte)((uint)local_0 | 8U));
				int local_1 = 50;
				do
				{
					local_0 = (byte)0;
					ReadRegister(registers["RegTemp1"], ref local_0);
				}
				while ((int)(byte)((uint)local_0 & 4U) == 4 && local_1-- >= 0);
				ReadRegister(registers["RegTemp2"]);
			}
		}

		public void SetAdcLowPowerOn(bool value)
		{
			try
			{
				lock (syncThread)
					registers["RegTemp1"].Value = (registers["RegTemp1"].Value & 254U) | (value ? 1U : 0U);
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetTempCalibrate(Decimal tempRoomValue)
		{
			lock (syncThread)
			{
				TempCalDone = false;
				TempValueRoom = tempRoomValue;
				SetTempMeasStart(true);
				TempValueCal = (Decimal)registers["RegTemp2"].Value;
				SetTempMeasStart(false);
				TempCalDone = true;
			}
		}

		public void SetPa20dBm(bool value)
		{
			try
			{
				lock (syncThread)
				{
					if (value == Pa20dBm)
						return;
					registers["RegTestPa1"].Value = (uint)(value ? 93 : 85);
					registers["RegTestPa2"].Value = (uint)(value ? 124 : 112);
					if (value)
					{
						prevOcpOn = OcpOn;
						prevOcpTrim = OcpTrim;
						SetOcpOn(false);
						SetOcpTrim(120);
						SetPaMode(PaModeEnum.PA1_PA2);
					}
					else
					{
						SetOcpOn(prevOcpOn);
						SetOcpTrim(prevOcpTrim);
					}
					ReadRegister(registers["RegPaLevel"]);
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetSensitivityBoostOn(bool value)
		{
			try
			{
				lock (syncThread)
				{
					registers["RegTestLna"].Value = value ? 45U : 27U;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetLowBetaAfcOffset(Decimal value)
		{
			try
			{
				lock (syncThread)
					registers["RegTestAfc"].Value = (uint)(value / new Decimal(4880, 0, 0, false, 1));
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		public void SetDagcOn(bool value)
		{
			try
			{
				lock (syncThread)
				{
					byte testDagc;
					if (AfcLowBetaOn)
						testDagc = (byte)(value ? 32 : 0);
					else
						testDagc = (byte)(value ? 48 : 0);
					registers["RegTestDagc"].Value = (uint)testDagc;
				}
			}
			catch (Exception ex)
			{
				OnError(1, ex.Message);
			}
		}

		private void registers_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			lock (syncThread)
			{
				if (!(e.PropertyName == "Value"))
					return;
				Register reg = (Register)sender;
				UpdateRegisterValue(reg);
				if (readLock == 0 && !Write((byte)reg.Address, (byte)reg.Value))
					OnError(1, "Unable to write register " + reg.Name);
				if (!(reg.Name == "RegOpMode"))
					return;
				if (Mode == OperatingModeEnum.Rx)
				{
					ReadRegister(registers["RegLna"]);
					ReadRegister(registers["RegFeiMsb"]);
					ReadRegister(registers["RegFeiLsb"]);
					ReadRegister(registers["RegAfcMsb"]);
					ReadRegister(registers["RegAfcLsb"]);
					ReadRegister(registers["RegRssiValue"]);
				}
				ReadIrqFlags();
			}
		}

		private void SX1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "Version":
					PopulateRegisters();
					ReadRegisters();
					break;
			}
		}

		private void ftdi_Opened(object sender, EventArgs e)
		{
			if (isOpen)
				OnConnected();
		}

		private void ftdi_Closed(object sender, EventArgs e)
		{
			spectrumOn = false;
			isOpen = false;
			regUpdateThreadContinue = false;
			OnDisconnected();
			OnError(0, "-");
		}

		private void sx1231_Dio0Changed(object sender, FtdiIoPort.IoChangedEventArgs e)
		{
			lock (syncThread)
			{
				if (!isPacketModeRunning || !e.Sate && !firstTransmit)
					return;
				firstTransmit = false;
				if (Mode == OperatingModeEnum.Tx)
				{
					OnPacketHandlerTransmitted();
					PacketHandlerTransmit();
				}
				else if (Mode == OperatingModeEnum.Rx)
					PacketHandlerReceive();
			}
		}

		private void sx1231_Dio1Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }
		private void sx1231_Dio2Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }
		private void sx1231_Dio3Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }
		private void sx1231_Dio4Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }
		private void sx1231_Dio5Changed(object sender, FtdiIoPort.IoChangedEventArgs e) { }

		private void SpectrumProcess()
		{
			Decimal num = SpectrumFrequencyMin + SpectrumFrequencyStep * (Decimal)SpectrumFrequencyId;
			byte data1 = (byte)((long)(num / frequencyStep) >> 16);
			byte data2 = (byte)((long)(num / frequencyStep) >> 8);
			byte data3 = (byte)(long)(num / frequencyStep);
			if (!Write((byte)registers["RegFrfMsb"].Address, data1))
				OnError(1, "Unable to write register " + registers["RegFrfMsb"].Name);
			if (!Write((byte)registers["RegFrfMid"].Address, data2))
				OnError(1, "Unable to write register " + registers["RegFrfMid"].Name);
			if (!Write((byte)registers["RegFrfLsb"].Address, data3))
				OnError(1, "Unable to write register " + registers["RegFrfLsb"].Name);
			SetRssiStart();
			++SpectrumFrequencyId;
			if (SpectrumFrequencyId >= SpectrumNbFrequenciesMax)
				SpectrumFrequencyId = 0;
		}

		private void RegUpdateThread()
		{
			int num = 0;
			while (regUpdateThreadContinue)
			{
				if (!ftdi.IsOpen)
				{
					Application.DoEvents();
					Thread.Sleep(10);
				}
				else
				{
					try
					{
						lock (syncThread)
						{
							if (!monitor)
							{
								Thread.Sleep(10);
								continue;
							}
							else
							{
								if (num % 10 == 0)
								{
									ReadIrqFlags();
									if (!SpectrumOn)
									{
										if (RfPaSwitchEnabled == 2)
										{
											RfPaSwitchSel = RfPaSwitchSelEnum.RF_IO_RFIO;
											SetRssiStart();
											RfPaSwitchSel = RfPaSwitchSelEnum.RF_IO_PA_BOOST;
											SetRssiStart();
										}
										else
											SetRssiStart();
									}
									else
										SpectrumProcess();
									if (TempCalDone && (Mode == OperatingModeEnum.Stdby || Mode == OperatingModeEnum.Fs))
									{
										tempMeasRunning = false;
										OnPropertyChanged("TempMeasRunning");
									}
								}
								if (num >= 200)
								{
									if (restartRx)
									{
										restartRx = false;
										ReadRegister(registers["RegLna"]);
										ReadRegister(registers["RegFeiMsb"]);
										ReadRegister(registers["RegFeiLsb"]);
										ReadRegister(registers["RegAfcMsb"]);
										ReadRegister(registers["RegAfcLsb"]);
									}
									if (TempCalDone && (Mode == OperatingModeEnum.Stdby || Mode == OperatingModeEnum.Fs))
									{
										tempMeasRunning = true;
										OnPropertyChanged("TempMeasRunning");
									}
									SetTempMeasStart(false);
									num = 0;
								}
							}
						}
					}
					catch
					{
					}
					++num;
					Thread.Sleep(1);
				}
			}
		}

		private void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		public void Dispose()
		{
			Close();
		}
	}
}