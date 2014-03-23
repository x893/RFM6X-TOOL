using SemtechLib.Controls;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Events;
using SemtechLib.General.Events;
using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class CommonViewControl : UserControl, INotifyDocumentationChanged
	{
		public event DecimalEventHandler FrequencyXoChanged;
		public event BooleanEventHandler SequencerChanged;
		public event BooleanEventHandler ListenModeChanged;
		public event EventHandler ListenModeAbortChanged;
		public event DataModeEventHandler DataModeChanged;
		public event ModulationTypeEventHandler ModulationTypeChanged;
		public event ByteEventHandler ModulationShapingChanged;
		public event DecimalEventHandler BitRateChanged;
		public event DecimalEventHandler FdevChanged;
		public event DecimalEventHandler FrequencyRfChanged;
		public event EventHandler RcCalibrationChanged;
		public event BooleanEventHandler LowBatOnChanged;
		public event LowBatTrimEventHandler LowBatTrimChanged;
		public event ListenResolEventHandler ListenResolIdleChanged;
		public event ListenResolEventHandler ListenResolRxChanged;
		public event ListenCriteriaEventHandler ListenCriteriaChanged;
		public event ListenEndEventHandler ListenEndChanged;
		public event DecimalEventHandler ListenCoefIdleChanged;
		public event DecimalEventHandler ListenCoefRxChanged;
		public event DocumentationChangedEventHandler DocumentationChanged;

		private Version version = new Version(2, 4);
		private Decimal bitRate = new Decimal(4800);
		private IContainer components;
		private Button btnRcCalibration;
		private Led ledLowBatMonitor;
		private Led ledRcCalibration;
		private NumericUpDownEx nudFrequencyXo;
		private ComboBox cBoxLowBatTrim;
		private NumericUpDownEx nudBitRate;
		private NumericUpDownEx nudFrequencyRf;
		private NumericUpDownEx nudFdev;
		private Panel panel4;
		private RadioButton rBtnLowBatOff;
		private RadioButton rBtnLowBatOn;
		private Label label1;
		private Panel panel2;
		private RadioButton rBtnModulationTypeOok;
		private RadioButton rBtnModulationTypeFsk;
		private Panel panel3;
		private RadioButton rBtnModulationShaping11;
		private RadioButton rBtnModulationShaping10;
		private RadioButton rBtnModulationShaping01;
		private RadioButton rBtnModulationShapingOff;
		private Label label5;
		private Panel panel1;
		private RadioButton rBtnContinousBitSyncOff;
		private RadioButton rBtnContinousBitSyncOn;
		private RadioButton rBtnPacketHandler;
		private Label label7;
		private Label label6;
		private Label label10;
		private Label label9;
		private Label label14;
		private Label label16;
		private Label lblRcOscillatorCalStat;
		private Label lblRcOscillatorCal;
		private Label label15;
		private Label label13;
		private Label label17;
		private Label label11;
		private Label label18;
		private Label label8;
		private Label label12;
		private RadioButton rBtnSequencerOff;
		private RadioButton rBtnSequencerOn;
		private Label label19;
		private Panel panel5;
		private Label label20;
		private Panel panel6;
		private RadioButton rBtnListenModeOff;
		private RadioButton rBtnListenModeOn;
		private Button btnListenModeAbort;
		private Label label21;
		private ComboBox cBoxListenResolIdle;
		private Label label22;
		private Panel panel7;
		private RadioButton rBtnListenCriteria1;
		private RadioButton rBtnListenCriteria0;
		private Label label23;
		private Label label24;
		private ComboBox cBoxListenEnd;
		private NumericUpDownEx nudListenCoefIdle;
		private NumericUpDownEx nudListenCoefRx;
		private Label label25;
		private Label label26;
		private Label label27;
		private Label label28;
		private ErrorProvider errorProvider;
		private ComboBox cBoxListenResolRx;
		private Label label30;
		private Label lblListenResolRx;
		private GroupBoxEx gBoxOscillators;
		private GroupBoxEx gBoxModulation;
		private GroupBoxEx gBoxBitSyncDataMode;
		private GroupBoxEx gBoxGeneral;
		private GroupBoxEx gBoxListenMode;
		private GroupBoxEx gBoxBatteryManagement;

		public Decimal FrequencyXo
		{
			get
			{
				return nudFrequencyXo.Value;
			}
			set
			{
				nudFrequencyXo.ValueChanged -= new EventHandler(nudFrequencyXo_ValueChanged);
				nudFrequencyXo.Value = value;
				nudFrequencyXo.ValueChanged += new EventHandler(nudFrequencyXo_ValueChanged);
			}
		}

		public Decimal FrequencyStep
		{
			get
			{
				return nudFrequencyRf.Increment;
			}
			set
			{
				nudFrequencyRf.Increment = value;
				nudFdev.Increment = value;
			}
		}

		public Version Version
		{
			get
			{
				return version;
			}
			set
			{
				version = value;
				if (value <= new Version(2, 1))
					cBoxListenResolRx.Enabled = false;
				else
					cBoxListenResolRx.Enabled = true;
			}
		}

		public bool Sequencer
		{
			get
			{
				return rBtnSequencerOn.Checked;
			}
			set
			{
				rBtnSequencerOn.CheckedChanged -= new EventHandler(rBtnSequencer_CheckedChanged);
				rBtnSequencerOff.CheckedChanged -= new EventHandler(rBtnSequencer_CheckedChanged);
				if (value)
				{
					rBtnSequencerOn.Checked = true;
					rBtnSequencerOff.Checked = false;
				}
				else
				{
					rBtnSequencerOn.Checked = false;
					rBtnSequencerOff.Checked = true;
				}
				rBtnSequencerOn.CheckedChanged += new EventHandler(rBtnSequencer_CheckedChanged);
				rBtnSequencerOff.CheckedChanged += new EventHandler(rBtnSequencer_CheckedChanged);
			}
		}

		public bool ListenMode
		{
			get
			{
				return rBtnListenModeOn.Checked;
			}
			set
			{
				rBtnListenModeOn.CheckedChanged -= new EventHandler(rBtnListenMode_CheckedChanged);
				rBtnListenModeOff.CheckedChanged -= new EventHandler(rBtnListenMode_CheckedChanged);
				if (value)
				{
					btnListenModeAbort.Enabled = true;
					rBtnListenModeOn.Checked = true;
					rBtnListenModeOff.Checked = false;
				}
				else
				{
					btnListenModeAbort.Enabled = false;
					rBtnListenModeOn.Checked = false;
					rBtnListenModeOff.Checked = true;
				}
				rBtnListenModeOn.CheckedChanged += new EventHandler(rBtnListenMode_CheckedChanged);
				rBtnListenModeOff.CheckedChanged += new EventHandler(rBtnListenMode_CheckedChanged);
			}
		}

		public DataModeEnum DataMode
		{
			get
			{
				if (rBtnPacketHandler.Checked)
					return DataModeEnum.Packet;
				if (rBtnContinousBitSyncOn.Checked)
					return DataModeEnum.ContinuousBitSync;
				return rBtnContinousBitSyncOff.Checked ? DataModeEnum.Continuous : DataModeEnum.Reserved;
			}
			set
			{
				rBtnPacketHandler.CheckedChanged -= new EventHandler(rBtnDataMode_CheckedChanged);
				rBtnContinousBitSyncOn.CheckedChanged -= new EventHandler(rBtnDataMode_CheckedChanged);
				rBtnContinousBitSyncOff.CheckedChanged -= new EventHandler(rBtnDataMode_CheckedChanged);
				switch (value)
				{
					case DataModeEnum.Packet:
						rBtnPacketHandler.Checked = true;
						rBtnContinousBitSyncOn.Checked = false;
						rBtnContinousBitSyncOff.Checked = false;
						break;
					case DataModeEnum.ContinuousBitSync:
						rBtnPacketHandler.Checked = false;
						rBtnContinousBitSyncOn.Checked = true;
						rBtnContinousBitSyncOff.Checked = false;
						break;
					case DataModeEnum.Continuous:
						rBtnPacketHandler.Checked = false;
						rBtnContinousBitSyncOn.Checked = false;
						rBtnContinousBitSyncOff.Checked = true;
						break;
				}
				rBtnPacketHandler.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
				rBtnContinousBitSyncOn.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
				rBtnContinousBitSyncOff.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
			}
		}

		public ModulationTypeEnum ModulationType
		{
			get
			{
				return rBtnModulationTypeFsk.Checked || !rBtnModulationTypeOok.Checked ? ModulationTypeEnum.FSK : ModulationTypeEnum.OOK;
			}
			set
			{
				rBtnModulationTypeFsk.CheckedChanged -= new EventHandler(rBtnModulationType_CheckedChanged);
				rBtnModulationTypeOok.CheckedChanged -= new EventHandler(rBtnModulationType_CheckedChanged);
				switch (value)
				{
					case ModulationTypeEnum.FSK:
						rBtnModulationTypeFsk.Checked = true;
						rBtnModulationTypeOok.Checked = false;
						rBtnModulationShapingOff.Text = "OFF";
						rBtnModulationShaping01.Text = "Gaussian filter, BT = 1.0";
						rBtnModulationShaping10.Text = "Gaussian filter, BT = 0.5";
						rBtnModulationShaping11.Text = "Gaussian filter, BT = 0.3";
						rBtnModulationShaping11.Visible = true;
						break;
					case ModulationTypeEnum.OOK:
						rBtnModulationTypeFsk.Checked = false;
						rBtnModulationTypeOok.Checked = true;
						rBtnModulationShapingOff.Text = "OFF";
						rBtnModulationShaping01.Text = "Filtering with fCutOff = BR";
						rBtnModulationShaping10.Text = "Filtering with fCutOff = 2 * BR";
						rBtnModulationShaping11.Text = "Unused";
						rBtnModulationShaping11.Visible = false;
						break;
				}
				rBtnModulationTypeFsk.CheckedChanged += new EventHandler(rBtnModulationType_CheckedChanged);
				rBtnModulationTypeOok.CheckedChanged += new EventHandler(rBtnModulationType_CheckedChanged);
			}
		}

		public byte ModulationShaping
		{
			get
			{
				if (rBtnModulationShapingOff.Checked)
					return (byte)0;
				if (rBtnModulationShaping01.Checked)
					return (byte)1;
				return rBtnModulationShaping10.Checked ? (byte)2 : (byte)3;
			}
			set
			{
				rBtnModulationShapingOff.CheckedChanged -= new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping01.CheckedChanged -= new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping10.CheckedChanged -= new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping11.CheckedChanged -= new EventHandler(rBtnModulationShaping_CheckedChanged);
				switch (value)
				{
					case (byte)0:
						rBtnModulationShapingOff.Checked = true;
						rBtnModulationShaping01.Checked = false;
						rBtnModulationShaping10.Checked = false;
						rBtnModulationShaping11.Checked = false;
						break;
					case (byte)1:
						rBtnModulationShapingOff.Checked = false;
						rBtnModulationShaping01.Checked = true;
						rBtnModulationShaping10.Checked = false;
						rBtnModulationShaping11.Checked = false;
						break;
					case (byte)2:
						rBtnModulationShapingOff.Checked = false;
						rBtnModulationShaping01.Checked = false;
						rBtnModulationShaping10.Checked = true;
						rBtnModulationShaping11.Checked = false;
						break;
					case (byte)3:
						rBtnModulationShapingOff.Checked = false;
						rBtnModulationShaping01.Checked = false;
						rBtnModulationShaping10.Checked = false;
						rBtnModulationShaping11.Checked = true;
						break;
				}
				rBtnModulationShapingOff.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping01.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping10.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
				rBtnModulationShaping11.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
			}
		}

		public Decimal BitRate
		{
			get
			{
				return bitRate;
			}
			set
			{
				try
				{
					nudBitRate.ValueChanged -= new EventHandler(nudBitRate_ValueChanged);
					bitRate = Math.Round(FrequencyXo / (Decimal)(ushort)Math.Round(FrequencyXo / value, MidpointRounding.AwayFromZero), MidpointRounding.AwayFromZero);
					nudBitRate.Value = bitRate;
				}
				catch (Exception)
				{
					nudBitRate.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudBitRate.ValueChanged += new EventHandler(nudBitRate_ValueChanged);
				}
			}
		}

		public Decimal Fdev
		{
			get
			{
				return nudFdev.Value;
			}
			set
			{
				try
				{
					nudFdev.ValueChanged -= new EventHandler(nudFdev_ValueChanged);
					nudFdev.Value = (Decimal)(ushort)Math.Round(value / FrequencyStep, MidpointRounding.AwayFromZero) * FrequencyStep;
				}
				catch (Exception)
				{
					nudFdev.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudFdev.ValueChanged += new EventHandler(nudFdev_ValueChanged);
				}
			}
		}

		public Decimal FrequencyRf
		{
			get
			{
				return nudFrequencyRf.Value;
			}
			set
			{
				try
				{
					nudFrequencyRf.ValueChanged -= new EventHandler(nudFrequencyRf_ValueChanged);
					nudFrequencyRf.Value = (Decimal)(uint)Math.Round(value / FrequencyStep, MidpointRounding.AwayFromZero) * FrequencyStep;
				}
				catch (Exception)
				{
					nudFrequencyRf.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudFrequencyRf.ValueChanged += new EventHandler(nudFrequencyRf_ValueChanged);
				}
			}
		}

		public bool RcCalDone
		{
			get
			{
				return ledRcCalibration.Checked;
			}
			set
			{
				ledRcCalibration.Checked = value;
			}
		}

		public bool LowBatMonitor
		{
			get
			{
				return ledLowBatMonitor.Checked;
			}
			set
			{
				ledLowBatMonitor.Checked = value;
			}
		}

		public bool LowBatOn
		{
			get
			{
				return rBtnLowBatOn.Checked;
			}
			set
			{
				rBtnLowBatOn.CheckedChanged -= new EventHandler(rBtnLowBatOn_CheckedChanged);
				rBtnLowBatOff.CheckedChanged -= new EventHandler(rBtnLowBatOn_CheckedChanged);
				if (value)
				{
					rBtnLowBatOn.Checked = true;
					rBtnLowBatOff.Checked = false;
				}
				else
				{
					rBtnLowBatOn.Checked = false;
					rBtnLowBatOff.Checked = true;
				}
				rBtnLowBatOn.CheckedChanged += new EventHandler(rBtnLowBatOn_CheckedChanged);
				rBtnLowBatOff.CheckedChanged += new EventHandler(rBtnLowBatOn_CheckedChanged);
			}
		}

		public LowBatTrimEnum LowBatTrim
		{
			get
			{
				return (LowBatTrimEnum)cBoxLowBatTrim.SelectedIndex;
			}
			set
			{
				cBoxLowBatTrim.SelectedIndexChanged -= new EventHandler(cBoxLowBatTrim_SelectedIndexChanged);
				cBoxLowBatTrim.SelectedIndex = (int)value;
				cBoxLowBatTrim.SelectedIndexChanged += new EventHandler(cBoxLowBatTrim_SelectedIndexChanged);
			}
		}

		public ListenResolEnum ListenResolIdle
		{
			get
			{
				return (ListenResolEnum)cBoxListenResolIdle.SelectedIndex;
			}
			set
			{
				cBoxListenResolIdle.SelectedIndexChanged -= new EventHandler(cBoxListenResolIdle_SelectedIndexChanged);
				cBoxListenResolIdle.SelectedIndex = (int)value;
				switch (value)
				{
					case ListenResolEnum.Res000064:
						nudListenCoefIdle.ValueChanged -= new EventHandler(nudListenCoefIdle_ValueChanged);
						nudListenCoefIdle.Maximum = new Decimal(16320, 0, 0, false, (byte)3);
						nudListenCoefIdle.Increment = new Decimal(64, 0, 0, false, (byte)3);
						nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
						break;
					case ListenResolEnum.Res004100:
						nudListenCoefIdle.ValueChanged -= new EventHandler(nudListenCoefIdle_ValueChanged);
						nudListenCoefIdle.Maximum = new Decimal(10455, 0, 0, false, (byte)1);
						nudListenCoefIdle.Increment = new Decimal(41, 0, 0, false, (byte)1);
						nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
						break;
					case ListenResolEnum.Res262000:
						nudListenCoefIdle.ValueChanged -= new EventHandler(nudListenCoefIdle_ValueChanged);
						nudListenCoefIdle.Maximum = new Decimal(66810);
						nudListenCoefIdle.Increment = new Decimal(262);
						nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
						break;
				}
				cBoxListenResolIdle.SelectedIndexChanged += new EventHandler(cBoxListenResolIdle_SelectedIndexChanged);
			}
		}

		public ListenResolEnum ListenResolRx
		{
			get
			{
				return (ListenResolEnum)cBoxListenResolRx.SelectedIndex;
			}
			set
			{
				try
				{
					cBoxListenResolRx.SelectedIndexChanged -= new EventHandler(cBoxListenResolRx_SelectedIndexChanged);
					cBoxListenResolRx.SelectedIndex = (int)value;
					switch (value)
					{
						case ListenResolEnum.Res000064:
							nudListenCoefRx.ValueChanged -= new EventHandler(nudListenCoefRx_ValueChanged);
							nudListenCoefRx.Maximum = new Decimal(16320, 0, 0, false, (byte)3);
							nudListenCoefRx.Increment = new Decimal(64, 0, 0, false, (byte)3);
							nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
							break;
						case ListenResolEnum.Res004100:
							nudListenCoefRx.ValueChanged -= new EventHandler(nudListenCoefRx_ValueChanged);
							nudListenCoefRx.Maximum = new Decimal(10455, 0, 0, false, (byte)1);
							nudListenCoefRx.Increment = new Decimal(41, 0, 0, false, (byte)1);
							nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
							break;
						case ListenResolEnum.Res262000:
							nudListenCoefRx.ValueChanged -= new EventHandler(nudListenCoefRx_ValueChanged);
							nudListenCoefRx.Maximum = new Decimal(66810);
							nudListenCoefRx.Increment = new Decimal(262);
							nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
							break;
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					cBoxListenResolRx.SelectedIndexChanged += new EventHandler(cBoxListenResolRx_SelectedIndexChanged);
				}
			}
		}

		public ListenCriteriaEnum ListenCriteria
		{
			get
			{
				return rBtnListenCriteria0.Checked ? ListenCriteriaEnum.RssiThresh : ListenCriteriaEnum.RssiThreshSyncAddress;
			}
			set
			{
				rBtnListenCriteria0.CheckedChanged -= new EventHandler(rBtnListenCriteria_CheckedChanged);
				rBtnListenCriteria1.CheckedChanged -= new EventHandler(rBtnListenCriteria_CheckedChanged);
				switch (value)
				{
					case ListenCriteriaEnum.RssiThresh:
						rBtnListenCriteria0.Checked = true;
						rBtnListenCriteria1.Checked = false;
						break;
					case ListenCriteriaEnum.RssiThreshSyncAddress:
						rBtnListenCriteria0.Checked = false;
						rBtnListenCriteria1.Checked = true;
						break;
				}
				rBtnListenCriteria0.CheckedChanged += new EventHandler(rBtnListenCriteria_CheckedChanged);
				rBtnListenCriteria1.CheckedChanged += new EventHandler(rBtnListenCriteria_CheckedChanged);
			}
		}

		public ListenEndEnum ListenEnd
		{
			get
			{
				return (ListenEndEnum)cBoxListenEnd.SelectedIndex;
			}
			set
			{
				try
				{
					cBoxListenEnd.SelectedIndexChanged -= new EventHandler(cBoxListenEnd_SelectedIndexChanged);
					if (value == ListenEndEnum.Reserved)
						cBoxListenEnd.SelectedIndex = -1;
					else
						cBoxListenEnd.SelectedIndex = (int)value;
				}
				catch (Exception) { }
				finally
				{
					cBoxListenEnd.SelectedIndexChanged += new EventHandler(cBoxListenEnd_SelectedIndexChanged);
				}
			}
		}

		public Decimal ListenCoefIdle
		{
			get
			{
				return nudListenCoefIdle.Value;
			}
			set
			{
				try
				{
					nudListenCoefIdle.ValueChanged -= new EventHandler(nudListenCoefIdle_ValueChanged);
					nudListenCoefIdle.Value = value;
				}
				catch { }
				finally
				{
					nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
				}
			}
		}

		public Decimal ListenCoefRx
		{
			get
			{
				return nudListenCoefRx.Value;
			}
			set
			{
				try
				{
					nudListenCoefRx.ValueChanged -= new EventHandler(nudListenCoefRx_ValueChanged);
					nudListenCoefRx.Value = value;
				}
				catch { }
				finally
				{
					nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
				}
			}
		}

		public CommonViewControl()
		{
			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = (IContainer)new Container();
			btnRcCalibration = new Button();
			cBoxLowBatTrim = new ComboBox();
			panel4 = new Panel();
			rBtnLowBatOff = new RadioButton();
			rBtnLowBatOn = new RadioButton();
			label1 = new Label();
			panel2 = new Panel();
			rBtnModulationTypeOok = new RadioButton();
			rBtnModulationTypeFsk = new RadioButton();
			panel3 = new Panel();
			rBtnModulationShaping11 = new RadioButton();
			rBtnModulationShaping10 = new RadioButton();
			rBtnModulationShaping01 = new RadioButton();
			rBtnModulationShapingOff = new RadioButton();
			label5 = new Label();
			panel1 = new Panel();
			rBtnContinousBitSyncOff = new RadioButton();
			rBtnContinousBitSyncOn = new RadioButton();
			rBtnPacketHandler = new RadioButton();
			label7 = new Label();
			label6 = new Label();
			label10 = new Label();
			label9 = new Label();
			label14 = new Label();
			label16 = new Label();
			lblRcOscillatorCalStat = new Label();
			lblRcOscillatorCal = new Label();
			label15 = new Label();
			label13 = new Label();
			label17 = new Label();
			label11 = new Label();
			label18 = new Label();
			label8 = new Label();
			label12 = new Label();
			rBtnSequencerOff = new RadioButton();
			rBtnSequencerOn = new RadioButton();
			label19 = new Label();
			panel5 = new Panel();
			label20 = new Label();
			panel6 = new Panel();
			rBtnListenModeOff = new RadioButton();
			rBtnListenModeOn = new RadioButton();
			btnListenModeAbort = new Button();
			label21 = new Label();
			cBoxListenResolIdle = new ComboBox();
			label22 = new Label();
			panel7 = new Panel();
			rBtnListenCriteria1 = new RadioButton();
			rBtnListenCriteria0 = new RadioButton();
			label23 = new Label();
			label24 = new Label();
			cBoxListenEnd = new ComboBox();
			label25 = new Label();
			label26 = new Label();
			label27 = new Label();
			label28 = new Label();
			errorProvider = new ErrorProvider(components);
			nudBitRate = new NumericUpDownEx();
			nudFdev = new NumericUpDownEx();
			nudFrequencyRf = new NumericUpDownEx();
			lblListenResolRx = new Label();
			label30 = new Label();
			cBoxListenResolRx = new ComboBox();
			gBoxGeneral = new GroupBoxEx();
			gBoxBitSyncDataMode = new GroupBoxEx();
			gBoxModulation = new GroupBoxEx();
			gBoxOscillators = new GroupBoxEx();
			nudFrequencyXo = new NumericUpDownEx();
			ledRcCalibration = new Led();
			gBoxBatteryManagement = new GroupBoxEx();
			ledLowBatMonitor = new Led();
			gBoxListenMode = new GroupBoxEx();
			nudListenCoefRx = new NumericUpDownEx();
			nudListenCoefIdle = new NumericUpDownEx();
			panel4.SuspendLayout();
			panel2.SuspendLayout();
			panel3.SuspendLayout();
			panel1.SuspendLayout();
			panel5.SuspendLayout();
			panel6.SuspendLayout();
			panel7.SuspendLayout();
			nudFdev.BeginInit();
			nudFrequencyRf.BeginInit();
			gBoxGeneral.SuspendLayout();
			gBoxBitSyncDataMode.SuspendLayout();
			gBoxModulation.SuspendLayout();
			gBoxOscillators.SuspendLayout();
			nudFrequencyXo.BeginInit();
			gBoxBatteryManagement.SuspendLayout();
			gBoxListenMode.SuspendLayout();
			nudListenCoefRx.BeginInit();
			nudListenCoefIdle.BeginInit();
			SuspendLayout();
			btnRcCalibration.Location = new Point(164, 47);
			btnRcCalibration.Name = "btnRcCalibration";
			btnRcCalibration.Size = new Size(75, 21);
			btnRcCalibration.TabIndex = 4;
			btnRcCalibration.Text = "Calibrate";
			btnRcCalibration.UseVisualStyleBackColor = true;
			btnRcCalibration.Click += new EventHandler(btnRcCalibration_Click);
			cBoxLowBatTrim.DropDownStyle = ComboBoxStyle.DropDownList;
			cBoxLowBatTrim.FormattingEnabled = true;
			cBoxLowBatTrim.Items.AddRange(new object[8] { "1.695", "1.764", "1.835", "1.905", "1.976", "2.045", "2.116", "2.185" });
			cBoxLowBatTrim.Location = new Point(166, 42);
			cBoxLowBatTrim.Name = "cBoxLowBatTrim";
			cBoxLowBatTrim.Size = new Size(124, 20);
			cBoxLowBatTrim.TabIndex = 3;
			cBoxLowBatTrim.SelectedIndexChanged += new EventHandler(cBoxLowBatTrim_SelectedIndexChanged);
			panel4.AutoSize = true;
			panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel4.Controls.Add((Control)rBtnLowBatOff);
			panel4.Controls.Add((Control)rBtnLowBatOn);
			panel4.Location = new Point(166, 18);
			panel4.Name = "panel4";
			panel4.Size = new Size(98, 19);
			panel4.TabIndex = 1;
			rBtnLowBatOff.AutoSize = true;
			rBtnLowBatOff.Location = new Point(54, 3);
			rBtnLowBatOff.Margin = new Padding(3, 0, 3, 0);
			rBtnLowBatOff.Name = "rBtnLowBatOff";
			rBtnLowBatOff.Size = new Size(41, 16);
			rBtnLowBatOff.TabIndex = 1;
			rBtnLowBatOff.Text = "OFF";
			rBtnLowBatOff.UseVisualStyleBackColor = true;
			rBtnLowBatOn.AutoSize = true;
			rBtnLowBatOn.Checked = true;
			rBtnLowBatOn.Location = new Point(3, 3);
			rBtnLowBatOn.Margin = new Padding(3, 0, 3, 0);
			rBtnLowBatOn.Name = "rBtnLowBatOn";
			rBtnLowBatOn.Size = new Size(35, 16);
			rBtnLowBatOn.TabIndex = 0;
			rBtnLowBatOn.TabStop = true;
			rBtnLowBatOn.Text = "ON";
			rBtnLowBatOn.UseVisualStyleBackColor = true;
			label1.AutoSize = true;
			label1.Location = new Point(6, 21);
			label1.Name = "label1";
			label1.Size = new Size(83, 12);
			label1.TabIndex = 0;
			label1.Text = "XO Frequency:";
			panel2.AutoSize = true;
			panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel2.Controls.Add((Control)rBtnModulationTypeOok);
			panel2.Controls.Add((Control)rBtnModulationTypeFsk);
			panel2.Location = new Point(164, 18);
			panel2.Name = "panel2";
			panel2.Size = new Size(98, 22);
			panel2.TabIndex = 1;
			rBtnModulationTypeOok.AutoSize = true;
			rBtnModulationTypeOok.Location = new Point(54, 3);
			rBtnModulationTypeOok.Name = "rBtnModulationTypeOok";
			rBtnModulationTypeOok.Size = new Size(41, 16);
			rBtnModulationTypeOok.TabIndex = 1;
			rBtnModulationTypeOok.Text = "OOK";
			rBtnModulationTypeOok.UseVisualStyleBackColor = true;
			rBtnModulationTypeOok.CheckedChanged += new EventHandler(rBtnModulationType_CheckedChanged);
			rBtnModulationTypeFsk.AutoSize = true;
			rBtnModulationTypeFsk.Checked = true;
			rBtnModulationTypeFsk.Location = new Point(3, 3);
			rBtnModulationTypeFsk.Name = "rBtnModulationTypeFsk";
			rBtnModulationTypeFsk.Size = new Size(41, 16);
			rBtnModulationTypeFsk.TabIndex = 0;
			rBtnModulationTypeFsk.TabStop = true;
			rBtnModulationTypeFsk.Text = "FSK";
			rBtnModulationTypeFsk.UseVisualStyleBackColor = true;
			rBtnModulationTypeFsk.CheckedChanged += new EventHandler(rBtnModulationType_CheckedChanged);
			panel3.AutoSize = true;
			panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel3.Controls.Add((Control)rBtnModulationShaping11);
			panel3.Controls.Add((Control)rBtnModulationShaping10);
			panel3.Controls.Add((Control)rBtnModulationShaping01);
			panel3.Controls.Add((Control)rBtnModulationShapingOff);
			panel3.Location = new Point(164, 44);
			panel3.Name = "panel3";
			panel3.Size = new Size(179, 85);
			panel3.TabIndex = 3;
			rBtnModulationShaping11.AutoSize = true;
			rBtnModulationShaping11.Location = new Point(3, 66);
			rBtnModulationShaping11.Name = "rBtnModulationShaping11";
			rBtnModulationShaping11.Size = new Size(173, 16);
			rBtnModulationShaping11.TabIndex = 3;
			rBtnModulationShaping11.Text = "Gaussian filter, BT = 0.3";
			rBtnModulationShaping11.UseVisualStyleBackColor = true;
			rBtnModulationShaping11.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
			rBtnModulationShaping10.AutoSize = true;
			rBtnModulationShaping10.Location = new Point(3, 45);
			rBtnModulationShaping10.Name = "rBtnModulationShaping10";
			rBtnModulationShaping10.Size = new Size(173, 16);
			rBtnModulationShaping10.TabIndex = 2;
			rBtnModulationShaping10.Text = "Gaussian filter, BT = 0.5";
			rBtnModulationShaping10.UseVisualStyleBackColor = true;
			rBtnModulationShaping10.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
			rBtnModulationShaping01.AutoSize = true;
			rBtnModulationShaping01.Location = new Point(3, 24);
			rBtnModulationShaping01.Name = "rBtnModulationShaping01";
			rBtnModulationShaping01.Size = new Size(173, 16);
			rBtnModulationShaping01.TabIndex = 1;
			rBtnModulationShaping01.Text = "Gaussian filter, BT = 1.0";
			rBtnModulationShaping01.UseVisualStyleBackColor = true;
			rBtnModulationShaping01.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
			rBtnModulationShapingOff.AutoSize = true;
			rBtnModulationShapingOff.Checked = true;
			rBtnModulationShapingOff.Location = new Point(3, 3);
			rBtnModulationShapingOff.Name = "rBtnModulationShapingOff";
			rBtnModulationShapingOff.Size = new Size(41, 16);
			rBtnModulationShapingOff.TabIndex = 0;
			rBtnModulationShapingOff.TabStop = true;
			rBtnModulationShapingOff.Text = "OFF";
			rBtnModulationShapingOff.UseVisualStyleBackColor = true;
			rBtnModulationShapingOff.CheckedChanged += new EventHandler(rBtnModulationShaping_CheckedChanged);
			label5.AutoSize = true;
			label5.Location = new Point(6, 22);
			label5.Name = "label5";
			label5.Size = new Size(71, 12);
			label5.TabIndex = 0;
			label5.Text = "Modulation:";
			panel1.AutoSize = true;
			panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel1.Controls.Add((Control)rBtnContinousBitSyncOff);
			panel1.Controls.Add((Control)rBtnContinousBitSyncOn);
			panel1.Controls.Add((Control)rBtnPacketHandler);
			panel1.Location = new Point(164, 18);
			panel1.Name = "panel1";
			panel1.Size = new Size(143, 64);
			panel1.TabIndex = 0;
			rBtnContinousBitSyncOff.AutoSize = true;
			rBtnContinousBitSyncOff.Location = new Point(3, 45);
			rBtnContinousBitSyncOff.Name = "rBtnContinousBitSyncOff";
			rBtnContinousBitSyncOff.Size = new Size(107, 16);
			rBtnContinousBitSyncOff.TabIndex = 2;
			rBtnContinousBitSyncOff.Text = "OFF- Continous";
			rBtnContinousBitSyncOff.UseVisualStyleBackColor = true;
			rBtnContinousBitSyncOff.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
			rBtnContinousBitSyncOn.AutoSize = true;
			rBtnContinousBitSyncOn.Location = new Point(3, 24);
			rBtnContinousBitSyncOn.Name = "rBtnContinousBitSyncOn";
			rBtnContinousBitSyncOn.Size = new Size(107, 16);
			rBtnContinousBitSyncOn.TabIndex = 1;
			rBtnContinousBitSyncOn.Text = "ON - Continous";
			rBtnContinousBitSyncOn.UseVisualStyleBackColor = true;
			rBtnContinousBitSyncOn.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
			rBtnPacketHandler.AutoSize = true;
			rBtnPacketHandler.Checked = true;
			rBtnPacketHandler.Location = new Point(3, 3);
			rBtnPacketHandler.Name = "rBtnPacketHandler";
			rBtnPacketHandler.Size = new Size(137, 16);
			rBtnPacketHandler.TabIndex = 0;
			rBtnPacketHandler.TabStop = true;
			rBtnPacketHandler.Text = "ON - Packet handler";
			rBtnPacketHandler.UseVisualStyleBackColor = true;
			rBtnPacketHandler.CheckedChanged += new EventHandler(rBtnDataMode_CheckedChanged);
			label7.AutoSize = true;
			label7.Location = new Point(6, 45);
			label7.Name = "label7";
			label7.Size = new Size(53, 12);
			label7.TabIndex = 3;
			label7.Text = "Bitrate:";
			label6.AutoSize = true;
			label6.Location = new Point(6, 49);
			label6.Name = "label6";
			label6.Size = new Size(119, 12);
			label6.TabIndex = 2;
			label6.Text = "Modulation shaping:";
			label10.AutoSize = true;
			label10.Location = new Point(6, 69);
			label10.Name = "label10";
			label10.Size = new Size(35, 12);
			label10.TabIndex = 6;
			label10.Text = "Fdev:";
			label9.AutoSize = true;
			label9.Location = new Point(294, 21);
			label9.Name = "label9";
			label9.Size = new Size(17, 12);
			label9.TabIndex = 2;
			label9.Text = "Hz";
			label14.AutoSize = true;
			label14.Location = new Point(6, 21);
			label14.Name = "label14";
			label14.Size = new Size(83, 12);
			label14.TabIndex = 0;
			label14.Text = "RF frequency:";
			label16.AutoSize = true;
			label16.Location = new Point(296, 45);
			label16.Name = "label16";
			label16.Size = new Size(11, 12);
			label16.TabIndex = 4;
			label16.Text = "V";
			lblRcOscillatorCalStat.AutoSize = true;
			lblRcOscillatorCalStat.Location = new Point(6, 75);
			lblRcOscillatorCalStat.Name = "lblRcOscillatorCalStat";
			lblRcOscillatorCalStat.Size = new Size(203, 12);
			lblRcOscillatorCalStat.TabIndex = 5;
			lblRcOscillatorCalStat.Text = "RC oscillator calibration status:";
			lblRcOscillatorCal.AutoSize = true;
			lblRcOscillatorCal.Location = new Point(6, 52);
			lblRcOscillatorCal.Name = "lblRcOscillatorCal";
			lblRcOscillatorCal.Size = new Size(161, 12);
			lblRcOscillatorCal.TabIndex = 3;
			lblRcOscillatorCal.Text = "RC oscillator calibration:";
			label15.AutoSize = true;
			label15.Location = new Point(8, 22);
			label15.Name = "label15";
			label15.Size = new Size(131, 12);
			label15.TabIndex = 0;
			label15.Text = "Low battery detector:";
			label13.AutoSize = true;
			label13.Location = new Point(294, 21);
			label13.Name = "label13";
			label13.Size = new Size(17, 12);
			label13.TabIndex = 2;
			label13.Text = "Hz";
			label17.AutoSize = true;
			label17.Location = new Point(8, 44);
			label17.Name = "label17";
			label17.Size = new Size(167, 12);
			label17.TabIndex = 2;
			label17.Text = "Low battery threshold trim:";
			label11.AutoSize = true;
			label11.Location = new Point(294, 69);
			label11.Name = "label11";
			label11.Size = new Size(17, 12);
			label11.TabIndex = 9;
			label11.Text = "Hz";
			label18.AutoSize = true;
			label18.Location = new Point(8, 67);
			label18.Name = "label18";
			label18.Size = new Size(137, 12);
			label18.TabIndex = 5;
			label18.Text = "Low battery indicator:";
			label8.AutoSize = true;
			label8.Location = new Point(294, 45);
			label8.Name = "label8";
			label8.Size = new Size(23, 12);
			label8.TabIndex = 5;
			label8.Text = "bps";
			label12.AutoSize = true;
			label12.Location = new Point(137, 69);
			label12.Name = "label12";
			label12.Size = new Size(23, 12);
			label12.TabIndex = 7;
			label12.Text = "+/-";
			rBtnSequencerOff.AutoSize = true;
			rBtnSequencerOff.Location = new Point(50, 0);
			rBtnSequencerOff.Margin = new Padding(3, 0, 3, 0);
			rBtnSequencerOff.Name = "rBtnSequencerOff";
			rBtnSequencerOff.Size = new Size(41, 16);
			rBtnSequencerOff.TabIndex = 1;
			rBtnSequencerOff.TabStop = true;
			rBtnSequencerOff.Text = "OFF";
			rBtnSequencerOff.UseVisualStyleBackColor = true;
			rBtnSequencerOff.CheckedChanged += new EventHandler(rBtnSequencer_CheckedChanged);
			rBtnSequencerOn.AutoSize = true;
			rBtnSequencerOn.Location = new Point(3, 0);
			rBtnSequencerOn.Margin = new Padding(3, 0, 3, 0);
			rBtnSequencerOn.Name = "rBtnSequencerOn";
			rBtnSequencerOn.Size = new Size(35, 16);
			rBtnSequencerOn.TabIndex = 0;
			rBtnSequencerOn.TabStop = true;
			rBtnSequencerOn.Text = "ON";
			rBtnSequencerOn.UseVisualStyleBackColor = true;
			rBtnSequencerOn.CheckedChanged += new EventHandler(rBtnSequencer_CheckedChanged);
			label19.AutoSize = true;
			label19.Location = new Point(7, 91);
			label19.Name = "label19";
			label19.Size = new Size(65, 12);
			label19.TabIndex = 10;
			label19.Text = "Sequencer:";
			panel5.AutoSize = true;
			panel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel5.Controls.Add((Control)rBtnSequencerOff);
			panel5.Controls.Add((Control)rBtnSequencerOn);
			panel5.Location = new Point(164, 90);
			panel5.Name = "panel5";
			panel5.Size = new Size(94, 16);
			panel5.TabIndex = 11;
			label20.AutoSize = true;
			label20.Location = new Point(8, 80);
			label20.Name = "label20";
			label20.Size = new Size(77, 12);
			label20.TabIndex = 0;
			label20.Text = "Listen mode:";
			panel6.AutoSize = true;
			panel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel6.Controls.Add((Control)rBtnListenModeOff);
			panel6.Controls.Add((Control)rBtnListenModeOn);
			panel6.Location = new Point(165, 78);
			panel6.Name = "panel6";
			panel6.Size = new Size(94, 16);
			panel6.TabIndex = 1;
			rBtnListenModeOff.AutoSize = true;
			rBtnListenModeOff.Location = new Point(50, 0);
			rBtnListenModeOff.Margin = new Padding(3, 0, 3, 0);
			rBtnListenModeOff.Name = "rBtnListenModeOff";
			rBtnListenModeOff.Size = new Size(41, 16);
			rBtnListenModeOff.TabIndex = 1;
			rBtnListenModeOff.TabStop = true;
			rBtnListenModeOff.Text = "OFF";
			rBtnListenModeOff.UseVisualStyleBackColor = true;
			rBtnListenModeOff.CheckedChanged += new EventHandler(rBtnListenMode_CheckedChanged);
			rBtnListenModeOn.AutoSize = true;
			rBtnListenModeOn.Location = new Point(3, 0);
			rBtnListenModeOn.Margin = new Padding(3, 0, 3, 0);
			rBtnListenModeOn.Name = "rBtnListenModeOn";
			rBtnListenModeOn.Size = new Size(35, 16);
			rBtnListenModeOn.TabIndex = 0;
			rBtnListenModeOn.TabStop = true;
			rBtnListenModeOn.Text = "ON";
			rBtnListenModeOn.UseVisualStyleBackColor = true;
			rBtnListenModeOn.CheckedChanged += new EventHandler(rBtnListenMode_CheckedChanged);
			btnListenModeAbort.Enabled = false;
			btnListenModeAbort.Location = new Point(269, 76);
			btnListenModeAbort.Name = "btnListenModeAbort";
			btnListenModeAbort.Size = new Size(75, 21);
			btnListenModeAbort.TabIndex = 2;
			btnListenModeAbort.Text = "Abort";
			btnListenModeAbort.UseVisualStyleBackColor = true;
			btnListenModeAbort.Visible = false;
			btnListenModeAbort.Click += new EventHandler(btnListenModeAbort_Click);
			label21.AutoSize = true;
			label21.Location = new Point(8, 105);
			label21.Name = "label21";
			label21.Size = new Size(143, 12);
			label21.TabIndex = 3;
			label21.Text = "Listen resolution idle:";
			cBoxListenResolIdle.DropDownStyle = ComboBoxStyle.DropDownList;
			cBoxListenResolIdle.FormattingEnabled = true;
			cBoxListenResolIdle.Items.AddRange(new object[3]
      {
        (object) "64",
        (object) "4'100",
        (object) "262'000"
      });
			cBoxListenResolIdle.Location = new Point(165, 100);
			cBoxListenResolIdle.Name = "cBoxListenResolIdle";
			cBoxListenResolIdle.Size = new Size(124, 20);
			cBoxListenResolIdle.TabIndex = 4;
			cBoxListenResolIdle.SelectedIndexChanged += new EventHandler(cBoxListenResolIdle_SelectedIndexChanged);
			label22.AutoSize = true;
			label22.Location = new Point(295, 103);
			label22.Name = "label22";
			label22.Size = new Size(17, 12);
			label22.TabIndex = 5;
			label22.Text = "µs";
			panel7.AutoSize = true;
			panel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel7.BackColor = Color.Transparent;
			panel7.Controls.Add((Control)rBtnListenCriteria1);
			panel7.Controls.Add((Control)rBtnListenCriteria0);
			panel7.Location = new Point(165, 150);
			panel7.Name = "panel7";
			panel7.Size = new Size(257, 43);
			panel7.TabIndex = 10;
			rBtnListenCriteria1.AutoSize = true;
			rBtnListenCriteria1.Location = new Point(3, 24);
			rBtnListenCriteria1.Name = "rBtnListenCriteria1";
			rBtnListenCriteria1.Size = new Size(251, 16);
			rBtnListenCriteria1.TabIndex = 1;
			rBtnListenCriteria1.Text = "> RssiThreshold && SyncAddress detected";
			rBtnListenCriteria1.UseVisualStyleBackColor = true;
			rBtnListenCriteria1.CheckedChanged += new EventHandler(rBtnListenCriteria_CheckedChanged);
			rBtnListenCriteria0.AutoSize = true;
			rBtnListenCriteria0.Checked = true;
			rBtnListenCriteria0.Location = new Point(3, 3);
			rBtnListenCriteria0.Name = "rBtnListenCriteria0";
			rBtnListenCriteria0.Size = new Size(113, 16);
			rBtnListenCriteria0.TabIndex = 0;
			rBtnListenCriteria0.TabStop = true;
			rBtnListenCriteria0.Text = "> RssiThreshold";
			rBtnListenCriteria0.UseVisualStyleBackColor = true;
			rBtnListenCriteria0.CheckedChanged += new EventHandler(rBtnListenCriteria_CheckedChanged);
			label23.AutoSize = true;
			label23.Location = new Point(8, 154);
			label23.Name = "label23";
			label23.Size = new Size(101, 12);
			label23.TabIndex = 9;
			label23.Text = "Listen criteria:";
			label24.AutoSize = true;
			label24.Location = new Point(8, 200);
			label24.Name = "label24";
			label24.Size = new Size(71, 12);
			label24.TabIndex = 11;
			label24.Text = "Listen end:";
			cBoxListenEnd.DropDownStyle = ComboBoxStyle.DropDownList;
			cBoxListenEnd.FormattingEnabled = true;
			cBoxListenEnd.Items.AddRange(new object[3]
      {
        (object) "Rx",
        (object) "Rx & Mode after IRQ",
        (object) "Rx & Idle after IRQ"
      });
			cBoxListenEnd.Location = new Point(165, 198);
			cBoxListenEnd.Name = "cBoxListenEnd";
			cBoxListenEnd.Size = new Size(124, 20);
			cBoxListenEnd.TabIndex = 12;
			cBoxListenEnd.SelectedIndexChanged += new EventHandler(cBoxListenEnd_SelectedIndexChanged);
			label25.AutoSize = true;
			label25.Location = new Point(295, 226);
			label25.Name = "label25";
			label25.Size = new Size(17, 12);
			label25.TabIndex = 15;
			label25.Text = "ms";
			label26.AutoSize = true;
			label26.Location = new Point(295, 250);
			label26.Name = "label26";
			label26.Size = new Size(17, 12);
			label26.TabIndex = 18;
			label26.Text = "ms";
			label27.AutoSize = true;
			label27.Location = new Point(8, 226);
			label27.Name = "label27";
			label27.Size = new Size(107, 12);
			label27.TabIndex = 13;
			label27.Text = "Listen idle time:";
			label28.AutoSize = true;
			label28.Location = new Point(8, 249);
			label28.Name = "label28";
			label28.Size = new Size(95, 12);
			label28.TabIndex = 16;
			label28.Text = "Listen Rx time:";
			errorProvider.ContainerControl = (ContainerControl)this;
			errorProvider.SetIconPadding((Control)nudBitRate, 30);
			nudBitRate.Location = new Point(164, 42);
			int[] bits1 = new int[4];
			bits1[0] = 603774;
			Decimal num1 = new Decimal(bits1);
			nudBitRate.Maximum = num1;
			int[] bits2 = new int[4];
			bits2[0] = 600;
			Decimal num2 = new Decimal(bits2);
			nudBitRate.Minimum = num2;
			nudBitRate.Name = "nudBitRate";
			nudBitRate.Size = new Size(124, 21);
			nudBitRate.TabIndex = 4;
			nudBitRate.ThousandsSeparator = true;
			int[] bits3 = new int[4];
			bits3[0] = 4800;
			Decimal num3 = new Decimal(bits3);
			nudBitRate.Value = num3;
			nudBitRate.ValueChanged += new EventHandler(nudBitRate_ValueChanged);
			errorProvider.SetIconPadding((Control)nudFdev, 30);
			int[] bits4 = new int[4];
			bits4[0] = 61;
			Decimal num4 = new Decimal(bits4);
			nudFdev.Increment = num4;
			nudFdev.Location = new Point(164, 66);
			int[] bits5 = new int[4];
			bits5[0] = 300000;
			Decimal num5 = new Decimal(bits5);
			nudFdev.Maximum = num5;
			nudFdev.Name = "nudFdev";
			nudFdev.Size = new Size(124, 21);
			nudFdev.TabIndex = 8;
			nudFdev.ThousandsSeparator = true;
			int[] bits6 = new int[4];
			bits6[0] = 5005;
			Decimal num6 = new Decimal(bits6);
			nudFdev.Value = num6;
			nudFdev.ValueChanged += new EventHandler(nudFdev_ValueChanged);
			errorProvider.SetIconPadding((Control)nudFrequencyRf, 30);
			int[] bits7 = new int[4];
			bits7[0] = 61;
			Decimal num7 = new Decimal(bits7);
			nudFrequencyRf.Increment = num7;
			nudFrequencyRf.Location = new Point(164, 18);
			int[] bits8 = new int[4];
			bits8[0] = 1020000000;
			Decimal num8 = new Decimal(bits8);
			nudFrequencyRf.Maximum = num8;
			int[] bits9 = new int[4];
			bits9[0] = 290000000;
			Decimal num9 = new Decimal(bits9);
			nudFrequencyRf.Minimum = num9;
			nudFrequencyRf.Name = "nudFrequencyRf";
			nudFrequencyRf.Size = new Size(124, 21);
			nudFrequencyRf.TabIndex = 1;
			nudFrequencyRf.ThousandsSeparator = true;
			int[] bits10 = new int[4];
			bits10[0] = 915000000;
			Decimal num10 = new Decimal(bits10);
			nudFrequencyRf.Value = num10;
			nudFrequencyRf.ValueChanged += new EventHandler(nudFrequencyRf_ValueChanged);
			lblListenResolRx.AutoSize = true;
			lblListenResolRx.Location = new Point(8, 130);
			lblListenResolRx.Name = "lblListenResolRx";
			lblListenResolRx.Size = new Size(131, 12);
			lblListenResolRx.TabIndex = 6;
			lblListenResolRx.Text = "Listen resolution Rx:";
			label30.AutoSize = true;
			label30.Location = new Point(295, 128);
			label30.Name = "label30";
			label30.Size = new Size(17, 12);
			label30.TabIndex = 8;
			label30.Text = "µs";
			cBoxListenResolRx.DropDownStyle = ComboBoxStyle.DropDownList;
			cBoxListenResolRx.FormattingEnabled = true;
			cBoxListenResolRx.Items.AddRange(new object[3]
      {
        (object) "64",
        (object) "4'100",
        (object) "262'000"
      });
			cBoxListenResolRx.Location = new Point(165, 125);
			cBoxListenResolRx.Name = "cBoxListenResolRx";
			cBoxListenResolRx.Size = new Size(124, 20);
			cBoxListenResolRx.TabIndex = 7;
			cBoxListenResolRx.SelectedIndexChanged += new EventHandler(cBoxListenResolRx_SelectedIndexChanged);
			gBoxGeneral.Controls.Add((Control)nudBitRate);
			gBoxGeneral.Controls.Add((Control)label12);
			gBoxGeneral.Controls.Add((Control)label8);
			gBoxGeneral.Controls.Add((Control)label11);
			gBoxGeneral.Controls.Add((Control)label13);
			gBoxGeneral.Controls.Add((Control)label14);
			gBoxGeneral.Controls.Add((Control)panel5);
			gBoxGeneral.Controls.Add((Control)label19);
			gBoxGeneral.Controls.Add((Control)label10);
			gBoxGeneral.Controls.Add((Control)label7);
			gBoxGeneral.Controls.Add((Control)nudFdev);
			gBoxGeneral.Controls.Add((Control)nudFrequencyRf);
			gBoxGeneral.Location = new Point(16, 8);
			gBoxGeneral.Name = "gBoxGeneral";
			gBoxGeneral.Size = new Size(355, 113);
			gBoxGeneral.TabIndex = 0;
			gBoxGeneral.TabStop = false;
			gBoxGeneral.Text = "General";
			gBoxGeneral.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxGeneral.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxBitSyncDataMode.Controls.Add((Control)panel1);
			gBoxBitSyncDataMode.Location = new Point(16, 126);
			gBoxBitSyncDataMode.Name = "gBoxBitSyncDataMode";
			gBoxBitSyncDataMode.Size = new Size(355, 84);
			gBoxBitSyncDataMode.TabIndex = 1;
			gBoxBitSyncDataMode.TabStop = false;
			gBoxBitSyncDataMode.Text = "Bit synchronizer / data mode";
			gBoxBitSyncDataMode.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxBitSyncDataMode.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxModulation.Controls.Add((Control)panel2);
			gBoxModulation.Controls.Add((Control)label6);
			gBoxModulation.Controls.Add((Control)label5);
			gBoxModulation.Controls.Add((Control)panel3);
			gBoxModulation.Location = new Point(16, 216);
			gBoxModulation.Name = "gBoxModulation";
			gBoxModulation.Size = new Size(355, 132);
			gBoxModulation.TabIndex = 2;
			gBoxModulation.TabStop = false;
			gBoxModulation.Text = "Modulation";
			gBoxModulation.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxModulation.MouseLeave += new EventHandler(control_MouseLeave);
			gBoxOscillators.Controls.Add((Control)nudFrequencyXo);
			gBoxOscillators.Controls.Add((Control)label9);
			gBoxOscillators.Controls.Add((Control)btnRcCalibration);
			gBoxOscillators.Controls.Add((Control)label1);
			gBoxOscillators.Controls.Add((Control)lblRcOscillatorCal);
			gBoxOscillators.Controls.Add((Control)lblRcOscillatorCalStat);
			gBoxOscillators.Controls.Add((Control)ledRcCalibration);
			gBoxOscillators.Location = new Point(16, 354);
			gBoxOscillators.Name = "gBoxOscillators";
			gBoxOscillators.Size = new Size(355, 92);
			gBoxOscillators.TabIndex = 3;
			gBoxOscillators.TabStop = false;
			gBoxOscillators.Text = "Oscillators";
			gBoxOscillators.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxOscillators.MouseLeave += new EventHandler(control_MouseLeave);
			nudFrequencyXo.Location = new Point(164, 18);
			int[] bits11 = new int[4];
			bits11[0] = 32000000;
			Decimal num11 = new Decimal(bits11);
			nudFrequencyXo.Maximum = num11;
			int[] bits12 = new int[4];
			bits12[0] = 26000000;
			Decimal num12 = new Decimal(bits12);
			nudFrequencyXo.Minimum = num12;
			nudFrequencyXo.Name = "nudFrequencyXo";
			nudFrequencyXo.Size = new Size(124, 21);
			nudFrequencyXo.TabIndex = 1;
			nudFrequencyXo.ThousandsSeparator = true;
			int[] bits13 = new int[4];
			bits13[0] = 32000000;
			Decimal num13 = new Decimal(bits13);
			nudFrequencyXo.Value = num13;
			nudFrequencyXo.ValueChanged += new EventHandler(nudFrequencyXo_ValueChanged);
			ledRcCalibration.BackColor = Color.Transparent;
			ledRcCalibration.LedColor = Color.Green;
			ledRcCalibration.LedSize = new Size(11, 11);
			ledRcCalibration.Location = new Point(164, 74);
			ledRcCalibration.Name = "ledRcCalibration";
			ledRcCalibration.Size = new Size(15, 14);
			ledRcCalibration.TabIndex = 6;
			ledRcCalibration.Text = "led1";
			gBoxBatteryManagement.Controls.Add((Control)panel4);
			gBoxBatteryManagement.Controls.Add((Control)label18);
			gBoxBatteryManagement.Controls.Add((Control)label17);
			gBoxBatteryManagement.Controls.Add((Control)label15);
			gBoxBatteryManagement.Controls.Add((Control)label16);
			gBoxBatteryManagement.Controls.Add((Control)cBoxLowBatTrim);
			gBoxBatteryManagement.Controls.Add((Control)ledLowBatMonitor);
			gBoxBatteryManagement.Location = new Point(377, 354);
			gBoxBatteryManagement.Name = "gBoxBatteryManagement";
			gBoxBatteryManagement.Size = new Size(405, 92);
			gBoxBatteryManagement.TabIndex = 5;
			gBoxBatteryManagement.TabStop = false;
			gBoxBatteryManagement.Text = "Battery management";
			gBoxBatteryManagement.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxBatteryManagement.MouseLeave += new EventHandler(control_MouseLeave);
			ledLowBatMonitor.BackColor = Color.Transparent;
			ledLowBatMonitor.LedColor = Color.Green;
			ledLowBatMonitor.LedSize = new Size(11, 11);
			ledLowBatMonitor.Location = new Point(166, 66);
			ledLowBatMonitor.Name = "ledLowBatMonitor";
			ledLowBatMonitor.Size = new Size(15, 14);
			ledLowBatMonitor.TabIndex = 6;
			ledLowBatMonitor.Text = "Low battery";
			gBoxListenMode.Controls.Add((Control)panel6);
			gBoxListenMode.Controls.Add((Control)label20);
			gBoxListenMode.Controls.Add((Control)label21);
			gBoxListenMode.Controls.Add((Control)label23);
			gBoxListenMode.Controls.Add((Control)lblListenResolRx);
			gBoxListenMode.Controls.Add((Control)label24);
			gBoxListenMode.Controls.Add((Control)label27);
			gBoxListenMode.Controls.Add((Control)label28);
			gBoxListenMode.Controls.Add((Control)btnListenModeAbort);
			gBoxListenMode.Controls.Add((Control)label22);
			gBoxListenMode.Controls.Add((Control)cBoxListenEnd);
			gBoxListenMode.Controls.Add((Control)label25);
			gBoxListenMode.Controls.Add((Control)cBoxListenResolRx);
			gBoxListenMode.Controls.Add((Control)label30);
			gBoxListenMode.Controls.Add((Control)cBoxListenResolIdle);
			gBoxListenMode.Controls.Add((Control)label26);
			gBoxListenMode.Controls.Add((Control)nudListenCoefRx);
			gBoxListenMode.Controls.Add((Control)panel7);
			gBoxListenMode.Controls.Add((Control)nudListenCoefIdle);
			gBoxListenMode.Location = new Point(377, 8);
			gBoxListenMode.Name = "gBoxListenMode";
			gBoxListenMode.Size = new Size(405, 340);
			gBoxListenMode.TabIndex = 4;
			gBoxListenMode.TabStop = false;
			gBoxListenMode.Text = "Listen mode";
			gBoxListenMode.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxListenMode.MouseLeave += new EventHandler(control_MouseLeave);
			nudListenCoefRx.DecimalPlaces = 3;
			nudListenCoefRx.Increment = new Decimal(new int[4]
      {
        41,
        0,
        0,
        65536
      });
			nudListenCoefRx.Location = new Point(165, 246);
			nudListenCoefRx.Maximum = new Decimal(new int[4]
      {
        10455,
        0,
        0,
        65536
      });
			nudListenCoefRx.Name = "nudListenCoefRx";
			nudListenCoefRx.Size = new Size(124, 21);
			nudListenCoefRx.TabIndex = 17;
			nudListenCoefRx.ThousandsSeparator = true;
			nudListenCoefRx.Value = new Decimal(new int[4]
      {
        1312,
        0,
        0,
        65536
      });
			nudListenCoefRx.ValueChanged += new EventHandler(nudListenCoefRx_ValueChanged);
			nudListenCoefIdle.DecimalPlaces = 3;
			nudListenCoefIdle.Increment = new Decimal(new int[4]
      {
        41,
        0,
        0,
        65536
      });
			nudListenCoefIdle.Location = new Point(165, 222);
			nudListenCoefIdle.Maximum = new Decimal(new int[4]
      {
        10455,
        0,
        0,
        65536
      });
			nudListenCoefIdle.Name = "nudListenCoefIdle";
			nudListenCoefIdle.Size = new Size(124, 21);
			nudListenCoefIdle.TabIndex = 14;
			nudListenCoefIdle.ThousandsSeparator = true;
			nudListenCoefIdle.Value = new Decimal(new int[4]
      {
        10045,
        0,
        0,
        65536
      });
			nudListenCoefIdle.ValueChanged += new EventHandler(nudListenCoefIdle_ValueChanged);
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add((Control)gBoxListenMode);
			Controls.Add((Control)gBoxBatteryManagement);
			Controls.Add((Control)gBoxOscillators);
			Controls.Add((Control)gBoxModulation);
			Controls.Add((Control)gBoxBitSyncDataMode);
			Controls.Add((Control)gBoxGeneral);
			Name = "CommonViewControl";
			Size = new Size(799, 455);
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panel5.ResumeLayout(false);
			panel5.PerformLayout();
			panel6.ResumeLayout(false);
			panel6.PerformLayout();
			panel7.ResumeLayout(false);
			panel7.PerformLayout();
			nudBitRate.EndInit();
			nudFdev.EndInit();
			nudFrequencyRf.EndInit();
			gBoxGeneral.ResumeLayout(false);
			gBoxGeneral.PerformLayout();
			gBoxBitSyncDataMode.ResumeLayout(false);
			gBoxBitSyncDataMode.PerformLayout();
			gBoxModulation.ResumeLayout(false);
			gBoxModulation.PerformLayout();
			gBoxOscillators.ResumeLayout(false);
			gBoxOscillators.PerformLayout();
			nudFrequencyXo.EndInit();
			gBoxBatteryManagement.ResumeLayout(false);
			gBoxBatteryManagement.PerformLayout();
			gBoxListenMode.ResumeLayout(false);
			gBoxListenMode.PerformLayout();
			nudListenCoefRx.EndInit();
			nudListenCoefIdle.EndInit();
			ResumeLayout(false);
		}

		private void OnFrequencyXoChanged(Decimal value)
		{
			if (FrequencyXoChanged == null)
				return;
			FrequencyXoChanged((object)this, new DecimalEventArg(value));
		}

		private void OnSequencerChanged(bool value)
		{
			if (SequencerChanged == null)
				return;
			SequencerChanged((object)this, new BooleanEventArg(value));
		}

		private void OnListenModeChanged(bool value)
		{
			if (ListenModeChanged == null)
				return;
			ListenModeChanged((object)this, new BooleanEventArg(value));
		}

		private void OnListenModeAbortChanged()
		{
			if (ListenModeAbortChanged == null)
				return;
			ListenModeAbortChanged((object)this, EventArgs.Empty);
		}

		private void OnDataModeChanged(DataModeEnum value)
		{
			if (DataModeChanged == null)
				return;
			DataModeChanged((object)this, new DataModeEventArg(value));
		}

		private void OnModulationTypeChanged(ModulationTypeEnum value)
		{
			if (ModulationTypeChanged == null)
				return;
			ModulationTypeChanged((object)this, new ModulationTypeEventArg(value));
		}

		private void OnModulationShapingChanged(byte value)
		{
			if (ModulationShapingChanged == null)
				return;
			ModulationShapingChanged((object)this, new ByteEventArg(value));
		}

		private void OnBitRateChanged(Decimal value)
		{
			if (BitRateChanged == null)
				return;
			BitRateChanged((object)this, new DecimalEventArg(value));
		}

		private void OnFdevChanged(Decimal value)
		{
			if (FdevChanged == null)
				return;
			FdevChanged((object)this, new DecimalEventArg(value));
		}

		private void OnFrequencyRfChanged(Decimal value)
		{
			if (FrequencyRfChanged == null)
				return;
			FrequencyRfChanged((object)this, new DecimalEventArg(value));
		}

		private void OnRcCalibrationChanged()
		{
			if (RcCalibrationChanged == null)
				return;
			RcCalibrationChanged((object)this, EventArgs.Empty);
		}

		private void OnLowBatOnChanged(bool value)
		{
			if (LowBatOnChanged == null)
				return;
			LowBatOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnLowBatTrimChanged(LowBatTrimEnum value)
		{
			if (LowBatTrimChanged == null)
				return;
			LowBatTrimChanged((object)this, new LowBatTrimEventArg(value));
		}

		private void OnListenResolIdleChanged(ListenResolEnum value)
		{
			if (ListenResolIdleChanged == null)
				return;
			ListenResolIdleChanged((object)this, new ListenResolEventArg(value));
		}

		private void OnListenResolRxChanged(ListenResolEnum value)
		{
			if (ListenResolRxChanged == null)
				return;
			ListenResolRxChanged((object)this, new ListenResolEventArg(value));
		}

		private void OnListenCriteriaChanged(ListenCriteriaEnum value)
		{
			if (ListenCriteriaChanged == null)
				return;
			ListenCriteriaChanged((object)this, new ListenCriteriaEventArg(value));
		}

		private void OnListenEndChanged(ListenEndEnum value)
		{
			if (ListenEndChanged == null)
				return;
			ListenEndChanged((object)this, new ListenEndEventArg(value));
		}

		private void OnListenCoefIdleChanged(Decimal value)
		{
			if (ListenCoefIdleChanged == null)
				return;
			ListenCoefIdleChanged((object)this, new DecimalEventArg(value));
		}

		private void OnListenCoefRxChanged(Decimal value)
		{
			if (ListenCoefRxChanged == null)
				return;
			ListenCoefRxChanged((object)this, new DecimalEventArg(value));
		}

		public void UpdateBitRateLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					nudBitRate.BackColor = SystemColors.Window;
					break;
				case LimitCheckStatusEnum.OUT_OF_RANGE:
					nudBitRate.BackColor = ControlPaint.LightLight(Color.Orange);
					break;
				case LimitCheckStatusEnum.ERROR:
					nudBitRate.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			errorProvider.SetError((Control)nudBitRate, message);
		}

		public void UpdateFdevLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					nudFdev.BackColor = SystemColors.Window;
					break;
				case LimitCheckStatusEnum.OUT_OF_RANGE:
					nudFdev.BackColor = ControlPaint.LightLight(Color.Orange);
					break;
				case LimitCheckStatusEnum.ERROR:
					nudFdev.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			errorProvider.SetError((Control)nudFdev, message);
		}

		public void UpdateFrequencyRfLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					nudFrequencyRf.BackColor = SystemColors.Window;
					break;
				case LimitCheckStatusEnum.OUT_OF_RANGE:
					nudFrequencyRf.BackColor = ControlPaint.LightLight(Color.Orange);
					break;
				case LimitCheckStatusEnum.ERROR:
					nudFrequencyRf.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			errorProvider.SetError((Control)nudFrequencyRf, message);
		}

		private void nudFrequencyXo_ValueChanged(object sender, EventArgs e)
		{
			FrequencyXo = nudFrequencyXo.Value;
			OnFrequencyXoChanged(FrequencyXo);
		}

		private void rBtnSequencer_CheckedChanged(object sender, EventArgs e)
		{
			Sequencer = rBtnSequencerOn.Checked;
			OnSequencerChanged(Sequencer);
		}

		private void rBtnListenMode_CheckedChanged(object sender, EventArgs e)
		{
			ListenMode = rBtnListenModeOn.Checked;
			OnListenModeChanged(ListenMode);
		}

		private void btnListenModeAbort_Click(object sender, EventArgs e)
		{
			OnListenModeAbortChanged();
		}

		private void rBtnDataMode_CheckedChanged(object sender, EventArgs e)
		{
			DataMode = !rBtnPacketHandler.Checked ? (!rBtnContinousBitSyncOn.Checked ? (!rBtnContinousBitSyncOff.Checked ? DataModeEnum.Reserved : DataModeEnum.Continuous) : DataModeEnum.ContinuousBitSync) : DataModeEnum.Packet;
			OnDataModeChanged(DataMode);
		}

		private void rBtnModulationType_CheckedChanged(object sender, EventArgs e)
		{
			ModulationType = !rBtnModulationTypeFsk.Checked ? (!rBtnModulationTypeOok.Checked ? ModulationTypeEnum.Reserved : ModulationTypeEnum.OOK) : ModulationTypeEnum.FSK;
			OnModulationTypeChanged(ModulationType);
		}

		private void rBtnModulationShaping_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnModulationShapingOff.Checked)
				ModulationShaping = (byte)0;
			else if (rBtnModulationShaping01.Checked)
				ModulationShaping = (byte)1;
			else if (rBtnModulationShaping10.Checked)
				ModulationShaping = (byte)2;
			else if (rBtnModulationShaping11.Checked)
				ModulationShaping = (byte)3;
			OnModulationShapingChanged(ModulationShaping);
		}

		private void nudBitRate_ValueChanged(object sender, EventArgs e)
		{
			int num1 = (int)Math.Round(FrequencyXo / BitRate, MidpointRounding.AwayFromZero);
			int num2 = (int)Math.Round(FrequencyXo / nudBitRate.Value, MidpointRounding.AwayFromZero);
			int num3 = (int)(nudBitRate.Value - BitRate);
			nudBitRate.ValueChanged -= new EventHandler(nudBitRate_ValueChanged);
			if (num3 >= -1 && num3 <= 1)
				nudBitRate.Value = Math.Round(FrequencyXo / (Decimal)(num2 - num3), MidpointRounding.AwayFromZero);
			else
				nudBitRate.Value = Math.Round(FrequencyXo / (Decimal)num2, MidpointRounding.AwayFromZero);
			nudBitRate.ValueChanged += new EventHandler(nudBitRate_ValueChanged);
			BitRate = nudBitRate.Value;
			OnBitRateChanged(BitRate);
		}

		private void nudFdev_ValueChanged(object sender, EventArgs e)
		{
			Fdev = nudFdev.Value;
			OnFdevChanged(Fdev);
		}

		private void nudFrequencyRf_ValueChanged(object sender, EventArgs e)
		{
			FrequencyRf = nudFrequencyRf.Value;
			OnFrequencyRfChanged(FrequencyRf);
		}

		private void btnRcCalibration_Click(object sender, EventArgs e)
		{
			OnRcCalibrationChanged();
		}

		private void rBtnLowBatOn_CheckedChanged(object sender, EventArgs e)
		{
			LowBatOn = rBtnLowBatOn.Checked;
			OnLowBatOnChanged(LowBatOn);
		}

		private void cBoxLowBatTrim_SelectedIndexChanged(object sender, EventArgs e)
		{
			LowBatTrim = (LowBatTrimEnum)cBoxLowBatTrim.SelectedIndex;
			OnLowBatTrimChanged(LowBatTrim);
		}

		private void cBoxListenResolIdle_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListenResolIdle = (ListenResolEnum)cBoxListenResolIdle.SelectedIndex;
			OnListenResolIdleChanged(ListenResolIdle);
		}

		private void cBoxListenResolRx_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListenResolRx = (ListenResolEnum)cBoxListenResolRx.SelectedIndex;
			OnListenResolRxChanged(ListenResolRx);
		}

		private void rBtnListenCriteria_CheckedChanged(object sender, EventArgs e)
		{
			ListenCriteria = rBtnListenCriteria0.Checked ? ListenCriteriaEnum.RssiThresh : ListenCriteriaEnum.RssiThreshSyncAddress;
			OnListenCriteriaChanged(ListenCriteria);
		}

		private void cBoxListenEnd_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListenEnd = (ListenEndEnum)cBoxListenEnd.SelectedIndex;
			OnListenEndChanged(ListenEnd);
		}

		private void nudListenCoefIdle_ValueChanged(object sender, EventArgs e)
		{
			ListenCoefIdle = nudListenCoefIdle.Value;
			OnListenCoefIdleChanged(ListenCoefIdle);
		}

		private void nudListenCoefRx_ValueChanged(object sender, EventArgs e)
		{
			ListenCoefRx = nudListenCoefRx.Value;
			OnListenCoefRxChanged(ListenCoefRx);
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == gBoxGeneral)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "General"));
			else if (sender == gBoxBitSyncDataMode)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Bit synchronizer Data mode"));
			else if (sender == gBoxModulation)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Modulation"));
			else if (sender == gBoxOscillators)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Oscillators"));
			else if (sender == gBoxListenMode)
			{
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Listen mode"));
			}
			else
			{
				if (sender != gBoxBatteryManagement)
					return;
				OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Battery management"));
			}
		}

		private void control_MouseLeave(object sender, EventArgs e)
		{
			OnDocumentationChanged(new DocumentationChangedEventArgs(".", "Overview"));
		}

		private void OnDocumentationChanged(DocumentationChangedEventArgs e)
		{
			if (DocumentationChanged != null)
				DocumentationChanged(this, e);
		}
	}
}