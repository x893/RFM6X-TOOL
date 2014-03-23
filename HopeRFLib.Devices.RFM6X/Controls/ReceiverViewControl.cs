using SemtechLib.Controls;
using SemtechLib.Devices.SX1231;
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
	public class ReceiverViewControl : UserControl, INotifyDocumentationChanged
	{
		private Decimal frequencyXo = new Decimal(32000000);
		private bool rssiAutoThresh = true;
		private Decimal bitRate = new Decimal(4800);
		private Version version = new Version(2, 4);
		private int agcReference = -80;
		private Decimal dccFreq = new Decimal(414);
		private Decimal rxBw = new Decimal(10417);
		private Decimal afcDccFreq = new Decimal(497);
		private Decimal afcRxBw = new Decimal(50000);
		private Decimal ookPeakThreshStep = new Decimal(5, 0, 0, false, (byte)1);
		private Decimal afcValue = new Decimal(0, 0, 0, false, (byte)1);
		private Decimal feiValue = new Decimal(0, 0, 0, false, (byte)1);
		private Decimal rssiValue = new Decimal(1275, 0, 0, true, (byte)1);
		private IContainer components;
		private ErrorProvider errorProvider;
		private Label suffixOOKfixed;
		private Label suffixOOKstep;
		private Label label3;
		private Label suffixAFCRxBw;
		private Label suffixAFCDCC;
		private Label suffixRxBw;
		private Label suffixDCC;
		private Label lblOokFixed;
		private Label lblOokCutoff;
		private Label lblOokDec;
		private Label lblOokStep;
		private Label lblOokType;
		private Label label2;
		private Label label1;
		private Label label8;
		private Label lblAfcRxBw;
		private Label lblAfcDcc;
		private Label lblRxBw;
		private Label lblDcc;
		private Label label5;
		private Label label4;
		private Label lblAGC;
		private NumericUpDownEx nudRxFilterBw;
		private NumericUpDownEx nudRxFilterBwAfc;
		private Panel panel2;
		private RadioButton rBtnAgcAutoRefOff;
		private RadioButton rBtnAgcAutoRefOn;
		private NumericUpDownEx nudAgcRefLevel;
		private NumericUpDownEx nudAgcSnrMargin;
		private NumericUpDown nudAgcStep5;
		private NumericUpDown nudAgcStep4;
		private NumericUpDown nudAgcStep3;
		private NumericUpDown nudAgcStep2;
		private NumericUpDown nudAgcStep1;
		private Label label33;
		private Label label32;
		private Label label31;
		private Label label30;
		private Label label29;
		private Label label28;
		private Label label27;
		private Label label26;
		private Label label25;
		private Label label24;
		private Panel panel4;
		private RadioButton rBtnLnaLowPowerOff;
		private RadioButton rBtnLnaLowPowerOn;
		private Panel panel5;
		private RadioButton rBtnLnaGainAutoOff;
		private RadioButton rBtnLnaGainAutoOn;
		private Panel panel3;
		private RadioButton rBtnLnaZin200;
		private RadioButton rBtnLnaZin50;
		private NumericUpDownEx nudDccFreq;
		private NumericUpDownEx nudAfcDccFreq;
		private Label lblAgcThresh5;
		private Label lblAgcThresh4;
		private Label lblAgcThresh3;
		private Label lblAgcThresh2;
		private Label lblAgcThresh1;
		private Label lblLnaGain6;
		private Label lblLnaGain5;
		private Label lblLnaGain4;
		private Label lblLnaGain3;
		private Label lblLnaGain2;
		private Label lblLnaGain1;
		private Label lblAgcReference;
		private Panel panel6;
		private RadioButton rBtnLnaGain1;
		private RadioButton rBtnLnaGain2;
		private RadioButton rBtnLnaGain3;
		private RadioButton rBtnLnaGain4;
		private RadioButton rBtnLnaGain5;
		private RadioButton rBtnLnaGain6;
		private Label label47;
		private Label label53;
		private Label label52;
		private Label label51;
		private Label label50;
		private Label label49;
		private Label label48;
		private Label label54;
		private Label label55;
		private Label lblRssiValue;
		private Label label56;
		private Label label13;
		private Label label16;
		private NumericUpDownEx nudRssiThresh;
		private Panel panel1;
		private RadioButton rBtnRssiAutoThreshOff;
		private RadioButton rBtnRssiAutoThreshOn;
		private Label label6;
		private ComboBox cBoxOokThreshType;
		private NumericUpDownEx nudOokPeakThreshStep;
		private ComboBox cBoxOokAverageThreshFilt;
		private ComboBox cBoxOokPeakThreshDec;
		private NumericUpDownEx nudOokFixedThresh;
		private Label label7;
		private Panel panel7;
		private RadioButton rBtnFastRxOff;
		private RadioButton rBtnFastRxOn;
		private Label label10;
		private Led ledFeiDone;
		private Label lblFeiValue;
		private Button btnAfcClear;
		private Button btnAfcStart;
		private Button btnRssiRead;
		private Label lblAfcValue;
		private Panel panel9;
		private RadioButton rBtnAfcAutoOff;
		private RadioButton rBtnAfcAutoOn;
		private Panel panel8;
		private RadioButton rBtnAfcAutoClearOff;
		private RadioButton rBtnAfcAutoClearOn;
		private Label label20;
		private Label label19;
		private Led ledRssiDone;
		private Button btnFeiRead;
		private Label label21;
		private Label label22;
		private Label label12;
		private Led ledAfcDone;
		private NumericUpDownEx nudTimeoutRssiThresh;
		private NumericUpDownEx nudTimeoutRxStart;
		private Label label15;
		private Label label11;
		private Label label14;
		private Label label9;
		private GroupBoxEx gBoxRssi;
		private GroupBoxEx gBoxAfcFei;
		private GroupBoxEx gBoxOok;
		private GroupBoxEx gBoxAfcBw;
		private GroupBoxEx gBoxRxBw;
		private GroupBoxEx gBoxLna;
		private GroupBoxEx gBoxAgc;
		private Label label17;
		private Label label18;
		private Label lblAfcLowBeta;
		private Panel pnlAfcLowBeta;
		private RadioButton rBtnAfcLowBetaOff;
		private RadioButton rBtnAfcLowBetaOn;
		private NumericUpDownEx nudLowBetaAfcOffset;
		private Label lblLowBetaAfcOffset;
		private Label lblLowBetaAfcOfssetUnit;
		private Label lblSensitivityBoost;
		private Panel pnlSensitivityBoost;
		private RadioButton rBtnSensitivityBoostOff;
		private RadioButton rBtnSensitivityBoostOn;
		private GroupBoxEx gBoxLnaSensitivity;
		private Panel pnlRssiPhase;
		private RadioButton rBtnRssiPhaseManual;
		private RadioButton rBtnRssiPhaseAuto;
		private Label label23;
		private Button btnRestartRx;
		private GroupBoxEx gBoxDagc;
		private Label label34;
		private Panel panel11;
		private RadioButton rBtnDagcOff;
		private RadioButton rBtnDagcOn;
		private DataModeEnum dataMode;
		private ModulationTypeEnum modulationType;
		private int agcThresh1;
		private int agcThresh2;
		private int agcThresh3;
		private int agcThresh4;
		private int agcThresh5;
		private bool feiDone;
		private bool afcDone;
		private bool rssiDone;
		private Decimal lowBetaAfcOffset;

		public Decimal FrequencyXo
		{
			get
			{
				return this.frequencyXo;
			}
			set
			{
				this.frequencyXo = value;
			}
		}

		public bool RssiAutoThresh
		{
			get
			{
				return this.rBtnRssiAutoThreshOn.Checked;
			}
			set
			{
				this.rBtnRssiAutoThreshOn.CheckedChanged -= new EventHandler(this.rBtnRssiAutoThreshOn_CheckedChanged);
				this.rBtnRssiAutoThreshOff.CheckedChanged -= new EventHandler(this.rBtnRssiAutoThreshOn_CheckedChanged);
				if (value)
				{
					this.rBtnRssiAutoThreshOn.Checked = true;
					this.rBtnRssiAutoThreshOff.Checked = false;
					this.nudRssiThresh.Enabled = false;
				}
				else
				{
					this.rBtnRssiAutoThreshOn.Checked = false;
					this.rBtnRssiAutoThreshOff.Checked = true;
					this.nudRssiThresh.Enabled = true;
				}
				this.rssiAutoThresh = value;
				this.rBtnRssiAutoThreshOn.CheckedChanged += new EventHandler(this.rBtnRssiAutoThreshOn_CheckedChanged);
				this.rBtnRssiAutoThreshOff.CheckedChanged += new EventHandler(this.rBtnRssiAutoThreshOn_CheckedChanged);
			}
		}

		public DataModeEnum DataMode
		{
			get
			{
				return this.dataMode;
			}
			set
			{
				this.dataMode = value;
				if (!(this.Version < new Version(2, 3)))
					return;
				if (this.AutoRxRestartOn || this.DataMode == DataModeEnum.Packet && !this.AutoRxRestartOn)
					this.btnRestartRx.Enabled = false;
				else
					this.btnRestartRx.Enabled = true;
			}
		}

		public ModulationTypeEnum ModulationType
		{
			get
			{
				return this.modulationType;
			}
			set
			{
				this.modulationType = value;
			}
		}

		public Decimal BitRate
		{
			get
			{
				return this.bitRate;
			}
			set
			{
				if (!(this.bitRate != value))
					return;
				int num1 = (int)this.OokAverageThreshFilt;
				this.cBoxOokAverageThreshFilt.Items.Clear();
				int num2 = 32;
				while (num2 >= 2)
				{
					if (num2 != 16)
						this.cBoxOokAverageThreshFilt.Items.Add((object)Math.Round(value / (Decimal)((double)num2 * Math.PI)).ToString());
					num2 /= 2;
				}
				this.OokAverageThreshFilt = (OokAverageThreshFiltEnum)num1;
				try
				{
					this.nudTimeoutRxStart.ValueChanged -= new EventHandler(this.nudTimeoutRxStart_ValueChanged);
					Decimal num3 = (Decimal)(uint)Math.Round(this.nudTimeoutRxStart.Value / new Decimal(1000) / new Decimal(16) / this.bitRate, MidpointRounding.AwayFromZero);
					this.nudTimeoutRxStart.Maximum = new Decimal((int)byte.MaxValue) * new Decimal(16) / value * new Decimal(1000);
					this.nudTimeoutRxStart.Increment = this.nudTimeoutRxStart.Maximum / new Decimal((int)byte.MaxValue);
					this.nudTimeoutRxStart.Value = num3 * new Decimal(16) / value * new Decimal(1000);
				}
				catch
				{
				}
				finally
				{
					this.nudTimeoutRxStart.ValueChanged += new EventHandler(this.nudTimeoutRxStart_ValueChanged);
				}
				try
				{
					this.nudTimeoutRssiThresh.ValueChanged -= new EventHandler(this.nudTimeoutRssiThresh_ValueChanged);
					Decimal num3 = (Decimal)(uint)Math.Round(this.nudTimeoutRssiThresh.Value / new Decimal(1000) / new Decimal(16) / this.bitRate, MidpointRounding.AwayFromZero);
					this.nudTimeoutRssiThresh.Maximum = new Decimal((int)byte.MaxValue) * new Decimal(16) / value * new Decimal(1000);
					this.nudTimeoutRssiThresh.Increment = this.nudTimeoutRssiThresh.Maximum / new Decimal((int)byte.MaxValue);
					this.nudTimeoutRssiThresh.Value = num3 * new Decimal(16) / value * new Decimal(1000);
				}
				catch
				{
				}
				finally
				{
					this.nudTimeoutRssiThresh.ValueChanged += new EventHandler(this.nudTimeoutRssiThresh_ValueChanged);
				}
				this.bitRate = value;
			}
		}

		public bool AfcLowBetaOn
		{
			get
			{
				return this.rBtnAfcLowBetaOn.Checked;
			}
			set
			{
				this.rBtnAfcLowBetaOn.CheckedChanged -= new EventHandler(this.rBtnAfcLowBeta_CheckedChanged);
				this.rBtnAfcLowBetaOff.CheckedChanged -= new EventHandler(this.rBtnAfcLowBeta_CheckedChanged);
				if (value)
				{
					this.rBtnAfcLowBetaOn.Checked = true;
					this.rBtnAfcLowBetaOff.Checked = false;
				}
				else
				{
					this.rBtnAfcLowBetaOn.Checked = false;
					this.rBtnAfcLowBetaOff.Checked = true;
				}
				this.rBtnAfcLowBetaOn.CheckedChanged += new EventHandler(this.rBtnAfcLowBeta_CheckedChanged);
				this.rBtnAfcLowBetaOff.CheckedChanged += new EventHandler(this.rBtnAfcLowBeta_CheckedChanged);
			}
		}

		public Version Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
				if (value <= new Version(2, 1))
				{
					this.gBoxAgc.Visible = true;
					this.gBoxDagc.Visible = false;
					this.lblAfcLowBeta.Visible = false;
					this.pnlAfcLowBeta.Visible = false;
					this.lblLowBetaAfcOffset.Visible = false;
					this.nudLowBetaAfcOffset.Visible = false;
					this.lblLowBetaAfcOfssetUnit.Visible = false;
					this.label21.Visible = true;
					this.panel7.Visible = true;
					this.label4.Visible = true;
					this.panel4.Visible = true;
				}
				else
				{
					this.gBoxAgc.Visible = false;
					this.gBoxDagc.Visible = false;
					this.lblAfcLowBeta.Visible = true;
					this.pnlAfcLowBeta.Visible = true;
					this.lblLowBetaAfcOffset.Visible = true;
					this.nudLowBetaAfcOffset.Visible = true;
					this.lblLowBetaAfcOfssetUnit.Visible = true;
					this.label21.Visible = false;
					this.panel7.Visible = false;
					this.label4.Visible = false;
					this.panel4.Visible = false;
					if (!(value >= new Version(2, 3)))
						return;
					this.gBoxDagc.Visible = true;
				}
			}
		}

		public int AgcReference
		{
			get
			{
				return this.agcReference;
			}
			set
			{
				this.agcReference = value;
				this.lblAgcReference.Text = value.ToString();
				if (!this.rssiAutoThresh)
					return;
				this.RssiThresh = (Decimal)value;
			}
		}

		public int AgcThresh1
		{
			get
			{
				return this.agcThresh1;
			}
			set
			{
				this.agcThresh1 = value;
				this.lblAgcThresh1.Text = value.ToString();
			}
		}

		public int AgcThresh2
		{
			get
			{
				return this.agcThresh2;
			}
			set
			{
				this.agcThresh2 = value;
				this.lblAgcThresh2.Text = value.ToString();
			}
		}

		public int AgcThresh3
		{
			get
			{
				return this.agcThresh3;
			}
			set
			{
				this.agcThresh3 = value;
				this.lblAgcThresh3.Text = value.ToString();
			}
		}

		public int AgcThresh4
		{
			get
			{
				return this.agcThresh4;
			}
			set
			{
				this.agcThresh4 = value;
				this.lblAgcThresh4.Text = value.ToString();
			}
		}

		public int AgcThresh5
		{
			get
			{
				return this.agcThresh5;
			}
			set
			{
				this.agcThresh5 = value;
				this.lblAgcThresh5.Text = value.ToString();
			}
		}

		public Decimal DccFreqMin
		{
			get
			{
				return this.nudDccFreq.Minimum;
			}
			set
			{
				this.nudDccFreq.Minimum = value;
			}
		}

		public Decimal DccFreqMax
		{
			get
			{
				return this.nudDccFreq.Maximum;
			}
			set
			{
				this.nudDccFreq.Maximum = value;
			}
		}

		public Decimal RxBwMin
		{
			get
			{
				return this.nudRxFilterBw.Minimum;
			}
			set
			{
				this.nudRxFilterBw.Minimum = value;
			}
		}

		public Decimal RxBwMax
		{
			get
			{
				return this.nudRxFilterBw.Maximum;
			}
			set
			{
				this.nudRxFilterBw.Maximum = value;
			}
		}

		public Decimal AfcDccFreqMin
		{
			get
			{
				return this.nudAfcDccFreq.Minimum;
			}
			set
			{
				this.nudAfcDccFreq.Minimum = value;
			}
		}

		public Decimal AfcDccFreqMax
		{
			get
			{
				return this.nudAfcDccFreq.Maximum;
			}
			set
			{
				this.nudAfcDccFreq.Maximum = value;
			}
		}

		public Decimal AfcRxBwMin
		{
			get
			{
				return this.nudRxFilterBwAfc.Minimum;
			}
			set
			{
				this.nudRxFilterBwAfc.Minimum = value;
			}
		}

		public Decimal AfcRxBwMax
		{
			get
			{
				return this.nudRxFilterBwAfc.Maximum;
			}
			set
			{
				this.nudRxFilterBwAfc.Maximum = value;
			}
		}

		public bool AgcAutoRefOn
		{
			get
			{
				return this.rBtnAgcAutoRefOn.Checked;
			}
			set
			{
				this.rBtnAgcAutoRefOn.CheckedChanged -= new EventHandler(this.rBtnAgcAutoRef_CheckedChanged);
				this.rBtnAgcAutoRefOff.CheckedChanged -= new EventHandler(this.rBtnAgcAutoRef_CheckedChanged);
				if (value)
				{
					this.rBtnAgcAutoRefOn.Checked = true;
					this.rBtnAgcAutoRefOff.Checked = false;
					this.nudAgcSnrMargin.Enabled = true;
					this.nudAgcRefLevel.Enabled = false;
				}
				else
				{
					this.rBtnAgcAutoRefOn.Checked = false;
					this.rBtnAgcAutoRefOff.Checked = true;
					this.nudAgcSnrMargin.Enabled = false;
					this.nudAgcRefLevel.Enabled = true;
				}
				this.rBtnAgcAutoRefOn.CheckedChanged += new EventHandler(this.rBtnAgcAutoRef_CheckedChanged);
				this.rBtnAgcAutoRefOff.CheckedChanged += new EventHandler(this.rBtnAgcAutoRef_CheckedChanged);
			}
		}

		public int AgcRefLevel
		{
			get
			{
				return (int)this.nudAgcRefLevel.Value;
			}
			set
			{
				try
				{
					this.nudAgcRefLevel.ValueChanged -= new EventHandler(this.nudAgcRefLevel_ValueChanged);
					this.nudAgcRefLevel.Value = (Decimal)value;
					this.nudAgcRefLevel.ValueChanged += new EventHandler(this.nudAgcRefLevel_ValueChanged);
				}
				catch (Exception)
				{
					this.nudAgcRefLevel.ValueChanged += new EventHandler(this.nudAgcRefLevel_ValueChanged);
				}
			}
		}

		public byte AgcSnrMargin
		{
			get
			{
				return (byte)this.nudAgcSnrMargin.Value;
			}
			set
			{
				try
				{
					this.nudAgcSnrMargin.ValueChanged -= new EventHandler(this.nudAgcSnrMargin_ValueChanged);
					this.nudAgcSnrMargin.Value = (Decimal)value;
					this.nudAgcSnrMargin.ValueChanged += new EventHandler(this.nudAgcSnrMargin_ValueChanged);
				}
				catch (Exception)
				{
					this.nudAgcSnrMargin.ValueChanged += new EventHandler(this.nudAgcSnrMargin_ValueChanged);
				}
			}
		}

		public byte AgcStep1
		{
			get
			{
				return (byte)this.nudAgcStep1.Value;
			}
			set
			{
				try
				{
					this.nudAgcStep1.ValueChanged -= new EventHandler(this.nudAgcStep_ValueChanged);
					this.nudAgcStep1.Value = (Decimal)value;
					this.nudAgcStep1.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					this.nudAgcStep1.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
			}
		}

		public byte AgcStep2
		{
			get
			{
				return (byte)this.nudAgcStep2.Value;
			}
			set
			{
				try
				{
					this.nudAgcStep2.ValueChanged -= new EventHandler(this.nudAgcStep_ValueChanged);
					this.nudAgcStep2.Value = (Decimal)value;
					this.nudAgcStep2.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					this.nudAgcStep2.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
			}
		}

		public byte AgcStep3
		{
			get
			{
				return (byte)this.nudAgcStep3.Value;
			}
			set
			{
				try
				{
					this.nudAgcStep3.ValueChanged -= new EventHandler(this.nudAgcStep_ValueChanged);
					this.nudAgcStep3.Value = (Decimal)value;
					this.nudAgcStep3.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					this.nudAgcStep3.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
			}
		}

		public byte AgcStep4
		{
			get
			{
				return (byte)this.nudAgcStep4.Value;
			}
			set
			{
				try
				{
					this.nudAgcStep4.ValueChanged -= new EventHandler(this.nudAgcStep_ValueChanged);
					this.nudAgcStep4.Value = (Decimal)value;
					this.nudAgcStep4.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					this.nudAgcStep4.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
			}
		}

		public byte AgcStep5
		{
			get
			{
				return (byte)this.nudAgcStep5.Value;
			}
			set
			{
				try
				{
					this.nudAgcStep5.ValueChanged -= new EventHandler(this.nudAgcStep_ValueChanged);
					this.nudAgcStep5.Value = (Decimal)value;
					this.nudAgcStep5.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
				catch (Exception)
				{
					this.nudAgcStep5.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
				}
			}
		}

		public LnaZinEnum LnaZin
		{
			get
			{
				return this.rBtnLnaZin50.Checked ? LnaZinEnum.ZIN_50 : LnaZinEnum.ZIN_200;
			}
			set
			{
				this.rBtnLnaZin50.CheckedChanged -= new EventHandler(this.rBtnLnaZin_CheckedChanged);
				this.rBtnLnaZin200.CheckedChanged -= new EventHandler(this.rBtnLnaZin_CheckedChanged);
				if (value == LnaZinEnum.ZIN_50)
				{
					this.rBtnLnaZin50.Checked = true;
					this.rBtnLnaZin200.Checked = false;
				}
				else
				{
					this.rBtnLnaZin50.Checked = false;
					this.rBtnLnaZin200.Checked = true;
				}
				this.rBtnLnaZin50.CheckedChanged += new EventHandler(this.rBtnLnaZin_CheckedChanged);
				this.rBtnLnaZin200.CheckedChanged += new EventHandler(this.rBtnLnaZin_CheckedChanged);
			}
		}

		public bool LnaLowPowerOn
		{
			get
			{
				return this.rBtnLnaLowPowerOn.Checked;
			}
			set
			{
				this.rBtnLnaLowPowerOn.CheckedChanged -= new EventHandler(this.rBtnLnaLowPower_CheckedChanged);
				this.rBtnLnaLowPowerOff.CheckedChanged -= new EventHandler(this.rBtnLnaLowPower_CheckedChanged);
				if (value)
				{
					this.rBtnLnaLowPowerOn.Checked = true;
					this.rBtnLnaLowPowerOff.Checked = false;
				}
				else
				{
					this.rBtnLnaLowPowerOn.Checked = false;
					this.rBtnLnaLowPowerOff.Checked = true;
				}
				this.rBtnLnaLowPowerOn.CheckedChanged += new EventHandler(this.rBtnLnaLowPower_CheckedChanged);
				this.rBtnLnaLowPowerOff.CheckedChanged += new EventHandler(this.rBtnLnaLowPower_CheckedChanged);
			}
		}

		public LnaGainEnum LnaCurrentGain
		{
			private get
			{
				if (this.rBtnLnaGainAutoOn.Checked)
					return LnaGainEnum.AGC;
				if (this.rBtnLnaGain1.Checked)
					return LnaGainEnum.G1;
				if (this.rBtnLnaGain2.Checked)
					return LnaGainEnum.G2;
				if (this.rBtnLnaGain3.Checked)
					return LnaGainEnum.G3;
				if (this.rBtnLnaGain4.Checked)
					return LnaGainEnum.G4;
				if (this.rBtnLnaGain5.Checked)
					return LnaGainEnum.G5;
				return this.rBtnLnaGain6.Checked ? LnaGainEnum.G6 : LnaGainEnum.AGC;
			}
			set
			{
				switch (value)
				{
					case LnaGainEnum.G1:
						this.lblLnaGain1.BackColor = Color.LightSteelBlue;
						this.lblLnaGain2.BackColor = Color.Transparent;
						this.lblLnaGain3.BackColor = Color.Transparent;
						this.lblLnaGain4.BackColor = Color.Transparent;
						this.lblLnaGain5.BackColor = Color.Transparent;
						this.lblLnaGain6.BackColor = Color.Transparent;
						break;
					case LnaGainEnum.G2:
						this.lblLnaGain1.BackColor = Color.Transparent;
						this.lblLnaGain2.BackColor = Color.LightSteelBlue;
						this.lblLnaGain3.BackColor = Color.Transparent;
						this.lblLnaGain4.BackColor = Color.Transparent;
						this.lblLnaGain5.BackColor = Color.Transparent;
						this.lblLnaGain6.BackColor = Color.Transparent;
						break;
					case LnaGainEnum.G3:
						this.lblLnaGain1.BackColor = Color.Transparent;
						this.lblLnaGain2.BackColor = Color.Transparent;
						this.lblLnaGain3.BackColor = Color.LightSteelBlue;
						this.lblLnaGain4.BackColor = Color.Transparent;
						this.lblLnaGain5.BackColor = Color.Transparent;
						this.lblLnaGain6.BackColor = Color.Transparent;
						break;
					case LnaGainEnum.G4:
						this.lblLnaGain1.BackColor = Color.Transparent;
						this.lblLnaGain2.BackColor = Color.Transparent;
						this.lblLnaGain3.BackColor = Color.Transparent;
						this.lblLnaGain4.BackColor = Color.LightSteelBlue;
						this.lblLnaGain5.BackColor = Color.Transparent;
						this.lblLnaGain6.BackColor = Color.Transparent;
						break;
					case LnaGainEnum.G5:
						this.lblLnaGain1.BackColor = Color.Transparent;
						this.lblLnaGain2.BackColor = Color.Transparent;
						this.lblLnaGain3.BackColor = Color.Transparent;
						this.lblLnaGain4.BackColor = Color.Transparent;
						this.lblLnaGain5.BackColor = Color.LightSteelBlue;
						this.lblLnaGain6.BackColor = Color.Transparent;
						break;
					case LnaGainEnum.G6:
						this.lblLnaGain1.BackColor = Color.Transparent;
						this.lblLnaGain2.BackColor = Color.Transparent;
						this.lblLnaGain3.BackColor = Color.Transparent;
						this.lblLnaGain4.BackColor = Color.Transparent;
						this.lblLnaGain5.BackColor = Color.Transparent;
						this.lblLnaGain6.BackColor = Color.LightSteelBlue;
						break;
				}
			}
		}

		public LnaGainEnum LnaGainSelect
		{
			private get
			{
				if (this.rBtnLnaGainAutoOn.Checked)
					return LnaGainEnum.AGC;
				if (this.rBtnLnaGain1.Checked)
					return LnaGainEnum.G1;
				if (this.rBtnLnaGain2.Checked)
					return LnaGainEnum.G2;
				if (this.rBtnLnaGain3.Checked)
					return LnaGainEnum.G3;
				if (this.rBtnLnaGain4.Checked)
					return LnaGainEnum.G4;
				if (this.rBtnLnaGain5.Checked)
					return LnaGainEnum.G5;
				return this.rBtnLnaGain6.Checked ? LnaGainEnum.G6 : LnaGainEnum.AGC;
			}
			set
			{
				this.rBtnLnaGain1.CheckedChanged -= new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain2.CheckedChanged -= new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain3.CheckedChanged -= new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain4.CheckedChanged -= new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain5.CheckedChanged -= new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain6.CheckedChanged -= new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGainAutoOn.CheckedChanged -= new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGainAutoOff.CheckedChanged -= new EventHandler(this.rBtnLnaGain_CheckedChanged);
				switch (value)
				{
					case LnaGainEnum.AGC:
						this.rBtnLnaGainAutoOn.Checked = true;
						this.rBtnLnaGainAutoOff.Checked = false;
						this.rBtnLnaGain1.Checked = true;
						this.rBtnLnaGain2.Checked = false;
						this.rBtnLnaGain3.Checked = false;
						this.rBtnLnaGain4.Checked = false;
						this.rBtnLnaGain5.Checked = false;
						this.rBtnLnaGain6.Checked = false;
						this.rBtnLnaGain1.Enabled = false;
						this.rBtnLnaGain2.Enabled = false;
						this.rBtnLnaGain3.Enabled = false;
						this.rBtnLnaGain4.Enabled = false;
						this.rBtnLnaGain5.Enabled = false;
						this.rBtnLnaGain6.Enabled = false;
						break;
					case LnaGainEnum.G1:
						this.rBtnLnaGainAutoOn.Checked = false;
						this.rBtnLnaGainAutoOff.Checked = true;
						this.rBtnLnaGain1.Checked = true;
						this.rBtnLnaGain2.Checked = false;
						this.rBtnLnaGain3.Checked = false;
						this.rBtnLnaGain4.Checked = false;
						this.rBtnLnaGain5.Checked = false;
						this.rBtnLnaGain6.Checked = false;
						this.rBtnLnaGain1.Enabled = true;
						this.rBtnLnaGain2.Enabled = true;
						this.rBtnLnaGain3.Enabled = true;
						this.rBtnLnaGain4.Enabled = true;
						this.rBtnLnaGain5.Enabled = true;
						this.rBtnLnaGain6.Enabled = true;
						break;
					case LnaGainEnum.G2:
						this.rBtnLnaGainAutoOn.Checked = false;
						this.rBtnLnaGainAutoOff.Checked = true;
						this.rBtnLnaGain1.Checked = false;
						this.rBtnLnaGain2.Checked = true;
						this.rBtnLnaGain3.Checked = false;
						this.rBtnLnaGain4.Checked = false;
						this.rBtnLnaGain5.Checked = false;
						this.rBtnLnaGain6.Checked = false;
						this.rBtnLnaGain1.Enabled = true;
						this.rBtnLnaGain2.Enabled = true;
						this.rBtnLnaGain3.Enabled = true;
						this.rBtnLnaGain4.Enabled = true;
						this.rBtnLnaGain5.Enabled = true;
						this.rBtnLnaGain6.Enabled = true;
						break;
					case LnaGainEnum.G3:
						this.rBtnLnaGainAutoOn.Checked = false;
						this.rBtnLnaGainAutoOff.Checked = true;
						this.rBtnLnaGain1.Checked = false;
						this.rBtnLnaGain2.Checked = false;
						this.rBtnLnaGain3.Checked = true;
						this.rBtnLnaGain4.Checked = false;
						this.rBtnLnaGain5.Checked = false;
						this.rBtnLnaGain6.Checked = false;
						this.rBtnLnaGain1.Enabled = true;
						this.rBtnLnaGain2.Enabled = true;
						this.rBtnLnaGain3.Enabled = true;
						this.rBtnLnaGain4.Enabled = true;
						this.rBtnLnaGain5.Enabled = true;
						this.rBtnLnaGain6.Enabled = true;
						break;
					case LnaGainEnum.G4:
						this.rBtnLnaGainAutoOn.Checked = false;
						this.rBtnLnaGainAutoOff.Checked = true;
						this.rBtnLnaGain1.Checked = false;
						this.rBtnLnaGain2.Checked = false;
						this.rBtnLnaGain3.Checked = false;
						this.rBtnLnaGain4.Checked = true;
						this.rBtnLnaGain5.Checked = false;
						this.rBtnLnaGain6.Checked = false;
						this.rBtnLnaGain1.Enabled = true;
						this.rBtnLnaGain2.Enabled = true;
						this.rBtnLnaGain3.Enabled = true;
						this.rBtnLnaGain4.Enabled = true;
						this.rBtnLnaGain5.Enabled = true;
						this.rBtnLnaGain6.Enabled = true;
						break;
					case LnaGainEnum.G5:
						this.rBtnLnaGainAutoOn.Checked = false;
						this.rBtnLnaGainAutoOff.Checked = true;
						this.rBtnLnaGain1.Checked = false;
						this.rBtnLnaGain2.Checked = false;
						this.rBtnLnaGain3.Checked = false;
						this.rBtnLnaGain4.Checked = false;
						this.rBtnLnaGain5.Checked = true;
						this.rBtnLnaGain6.Checked = false;
						this.rBtnLnaGain1.Enabled = true;
						this.rBtnLnaGain2.Enabled = true;
						this.rBtnLnaGain3.Enabled = true;
						this.rBtnLnaGain4.Enabled = true;
						this.rBtnLnaGain5.Enabled = true;
						this.rBtnLnaGain6.Enabled = true;
						break;
					case LnaGainEnum.G6:
						this.rBtnLnaGainAutoOn.Checked = false;
						this.rBtnLnaGainAutoOff.Checked = true;
						this.rBtnLnaGain1.Checked = false;
						this.rBtnLnaGain2.Checked = false;
						this.rBtnLnaGain3.Checked = false;
						this.rBtnLnaGain4.Checked = false;
						this.rBtnLnaGain5.Checked = false;
						this.rBtnLnaGain6.Checked = true;
						this.rBtnLnaGain1.Enabled = true;
						this.rBtnLnaGain2.Enabled = true;
						this.rBtnLnaGain3.Enabled = true;
						this.rBtnLnaGain4.Enabled = true;
						this.rBtnLnaGain5.Enabled = true;
						this.rBtnLnaGain6.Enabled = true;
						break;
				}
				this.rBtnLnaGain1.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain2.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain3.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain4.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain5.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGain6.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGainAutoOn.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
				this.rBtnLnaGainAutoOff.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			}
		}

		public Decimal DccFreq
		{
			get
			{
				return this.dccFreq;
			}
			set
			{
				try
				{
					this.nudDccFreq.ValueChanged -= new EventHandler(this.nudDccFreq_ValueChanged);
					this.dccFreq = new Decimal(40, 0, 0, false, (byte)1) * this.RxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * (Decimal)Math.Pow(2.0, (double)((int)(byte)(Math.Log10((double)(new Decimal(40, 0, 0, false, (byte)1) * this.RxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * value)) / Math.Log10(2.0) - 2.0) + 2));
					this.nudDccFreq.Value = this.dccFreq;
					this.nudDccFreq.ValueChanged += new EventHandler(this.nudDccFreq_ValueChanged);
				}
				catch (Exception)
				{
					this.nudDccFreq.ValueChanged += new EventHandler(this.nudDccFreq_ValueChanged);
				}
			}
		}

		public Decimal RxBw
		{
			get
			{
				return this.rxBw;
			}
			set
			{
				try
				{
					this.nudRxFilterBw.ValueChanged -= new EventHandler(this.nudRxFilterBw_ValueChanged);
					int mant = 0;
					int exp = 0;
					SX1231.ComputeRxBwMantExp(this.frequencyXo, this.ModulationType, value, ref mant, ref exp);
					this.rxBw = SX1231.ComputeRxBw(this.frequencyXo, this.ModulationType, mant, exp);
					this.nudRxFilterBw.Value = this.rxBw;
					this.nudRxFilterBw.ValueChanged += new EventHandler(this.nudRxFilterBw_ValueChanged);
				}
				catch (Exception)
				{
					this.nudRxFilterBw.ValueChanged += new EventHandler(this.nudRxFilterBw_ValueChanged);
				}
			}
		}

		public Decimal AfcDccFreq
		{
			get
			{
				return this.afcDccFreq;
			}
			set
			{
				try
				{
					this.nudAfcDccFreq.ValueChanged -= new EventHandler(this.nudAfcDccFreq_ValueChanged);
					this.afcDccFreq = new Decimal(40, 0, 0, false, (byte)1) * this.AfcRxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * (Decimal)Math.Pow(2.0, (double)((int)(byte)(Math.Log10((double)(new Decimal(40, 0, 0, false, (byte)1) * this.AfcRxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * value)) / Math.Log10(2.0) - 2.0) + 2));
					this.nudAfcDccFreq.Value = this.afcDccFreq;
					this.nudAfcDccFreq.ValueChanged += new EventHandler(this.nudAfcDccFreq_ValueChanged);
				}
				catch (Exception)
				{
					this.nudAfcDccFreq.ValueChanged += new EventHandler(this.nudAfcDccFreq_ValueChanged);
				}
			}
		}

		public Decimal AfcRxBw
		{
			get
			{
				return this.afcRxBw;
			}
			set
			{
				try
				{
					this.nudRxFilterBwAfc.ValueChanged -= new EventHandler(this.nudRxFilterBwAfc_ValueChanged);
					int mant = 0;
					int exp = 0;
					SX1231.ComputeRxBwMantExp(this.frequencyXo, this.ModulationType, value, ref mant, ref exp);
					this.afcRxBw = SX1231.ComputeRxBw(this.frequencyXo, this.ModulationType, mant, exp);
					this.nudRxFilterBwAfc.Value = this.afcRxBw;
					this.nudRxFilterBwAfc.ValueChanged += new EventHandler(this.nudRxFilterBwAfc_ValueChanged);
				}
				catch (Exception)
				{
					this.nudRxFilterBwAfc.ValueChanged += new EventHandler(this.nudRxFilterBwAfc_ValueChanged);
				}
			}
		}

		public OokThreshTypeEnum OokThreshType
		{
			get
			{
				return (OokThreshTypeEnum)this.cBoxOokThreshType.SelectedIndex;
			}
			set
			{
				this.cBoxOokThreshType.SelectedIndexChanged -= new EventHandler(this.cBoxOokThreshType_SelectedIndexChanged);
				switch (value)
				{
					case OokThreshTypeEnum.Fixed:
						this.cBoxOokThreshType.SelectedIndex = 0;
						this.nudOokPeakThreshStep.Enabled = false;
						this.cBoxOokPeakThreshDec.Enabled = false;
						this.cBoxOokAverageThreshFilt.Enabled = false;
						this.nudOokFixedThresh.Enabled = true;
						break;
					case OokThreshTypeEnum.Peak:
						this.cBoxOokThreshType.SelectedIndex = 1;
						this.nudOokPeakThreshStep.Enabled = true;
						this.cBoxOokPeakThreshDec.Enabled = true;
						this.cBoxOokAverageThreshFilt.Enabled = false;
						this.nudOokFixedThresh.Enabled = true;
						break;
					case OokThreshTypeEnum.Average:
						this.cBoxOokThreshType.SelectedIndex = 2;
						this.nudOokPeakThreshStep.Enabled = false;
						this.cBoxOokPeakThreshDec.Enabled = false;
						this.cBoxOokAverageThreshFilt.Enabled = true;
						this.nudOokFixedThresh.Enabled = false;
						break;
					default:
						this.cBoxOokThreshType.SelectedIndex = -1;
						break;
				}
				this.cBoxOokThreshType.SelectedIndexChanged += new EventHandler(this.cBoxOokThreshType_SelectedIndexChanged);
			}
		}

		public Decimal OokPeakThreshStep
		{
			get
			{
				return this.ookPeakThreshStep;
			}
			set
			{
				try
				{
					this.nudOokPeakThreshStep.ValueChanged -= new EventHandler(this.nudOokPeakThreshStep_ValueChanged);
					Decimal[] array = new Decimal[8]
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
					int index = Array.IndexOf<Decimal>(array, value);
					this.ookPeakThreshStep = array[index];
					this.nudOokPeakThreshStep.Value = this.ookPeakThreshStep;
					this.nudOokPeakThreshStep.ValueChanged += new EventHandler(this.nudOokPeakThreshStep_ValueChanged);
				}
				catch (Exception)
				{
					this.nudOokPeakThreshStep.ValueChanged += new EventHandler(this.nudOokPeakThreshStep_ValueChanged);
				}
			}
		}

		public OokPeakThreshDecEnum OokPeakThreshDec
		{
			get
			{
				return (OokPeakThreshDecEnum)this.cBoxOokPeakThreshDec.SelectedIndex;
			}
			set
			{
				this.cBoxOokPeakThreshDec.SelectedIndexChanged -= new EventHandler(this.cBoxOokPeakThreshDec_SelectedIndexChanged);
				this.cBoxOokPeakThreshDec.SelectedIndex = (int)value;
				this.cBoxOokPeakThreshDec.SelectedIndexChanged += new EventHandler(this.cBoxOokPeakThreshDec_SelectedIndexChanged);
			}
		}

		public OokAverageThreshFiltEnum OokAverageThreshFilt
		{
			get
			{
				return (OokAverageThreshFiltEnum)this.cBoxOokAverageThreshFilt.SelectedIndex;
			}
			set
			{
				this.cBoxOokAverageThreshFilt.SelectedIndexChanged -= new EventHandler(this.cBoxOokAverageThreshFilt_SelectedIndexChanged);
				this.cBoxOokAverageThreshFilt.SelectedIndex = (int)value;
				this.cBoxOokAverageThreshFilt.SelectedIndexChanged += new EventHandler(this.cBoxOokAverageThreshFilt_SelectedIndexChanged);
			}
		}

		public byte OokFixedThresh
		{
			get
			{
				return (byte)this.nudOokFixedThresh.Value;
			}
			set
			{
				try
				{
					this.nudOokFixedThresh.ValueChanged -= new EventHandler(this.nudOokFixedThresh_ValueChanged);
					this.nudOokFixedThresh.Value = (Decimal)value;
					this.nudOokFixedThresh.ValueChanged += new EventHandler(this.nudOokFixedThresh_ValueChanged);
				}
				catch (Exception)
				{
					this.nudOokFixedThresh.ValueChanged += new EventHandler(this.nudOokFixedThresh_ValueChanged);
				}
			}
		}

		public bool FeiDone
		{
			get
			{
				return this.feiDone;
			}
			set
			{
				this.feiDone = value;
				this.ledFeiDone.Checked = value;
			}
		}

		public bool AfcDone
		{
			get
			{
				return this.afcDone;
			}
			set
			{
				this.afcDone = value;
				this.ledAfcDone.Checked = value;
			}
		}

		public bool AfcAutoClearOn
		{
			get
			{
				return this.rBtnAfcAutoClearOn.Checked;
			}
			set
			{
				this.rBtnAfcAutoClearOn.CheckedChanged -= new EventHandler(this.rBtnAfcAutoClearOn_CheckedChanged);
				this.rBtnAfcAutoClearOff.CheckedChanged -= new EventHandler(this.rBtnAfcAutoClearOn_CheckedChanged);
				if (value)
				{
					this.rBtnAfcAutoClearOn.Checked = true;
					this.rBtnAfcAutoClearOff.Checked = false;
				}
				else
				{
					this.rBtnAfcAutoClearOn.Checked = false;
					this.rBtnAfcAutoClearOff.Checked = true;
				}
				this.rBtnAfcAutoClearOn.CheckedChanged += new EventHandler(this.rBtnAfcAutoClearOn_CheckedChanged);
				this.rBtnAfcAutoClearOff.CheckedChanged += new EventHandler(this.rBtnAfcAutoClearOn_CheckedChanged);
			}
		}

		public bool AfcAutoOn
		{
			get
			{
				return this.rBtnAfcAutoOn.Checked;
			}
			set
			{
				this.rBtnAfcAutoOn.CheckedChanged -= new EventHandler(this.rBtnAfcAutoOn_CheckedChanged);
				this.rBtnAfcAutoOff.CheckedChanged -= new EventHandler(this.rBtnAfcAutoOn_CheckedChanged);
				if (value)
				{
					this.rBtnAfcAutoOn.Checked = true;
					this.rBtnAfcAutoOff.Checked = false;
				}
				else
				{
					this.rBtnAfcAutoOn.Checked = false;
					this.rBtnAfcAutoOff.Checked = true;
				}
				this.rBtnAfcAutoOn.CheckedChanged += new EventHandler(this.rBtnAfcAutoOn_CheckedChanged);
				this.rBtnAfcAutoOff.CheckedChanged += new EventHandler(this.rBtnAfcAutoOn_CheckedChanged);
			}
		}

		public Decimal AfcValue
		{
			get
			{
				return this.afcValue;
			}
			set
			{
				this.afcValue = value;
				this.lblAfcValue.Text = this.afcValue.ToString("N0");
			}
		}

		public Decimal FeiValue
		{
			get
			{
				return this.feiValue;
			}
			set
			{
				this.feiValue = value;
				this.lblFeiValue.Text = this.feiValue.ToString("N0");
			}
		}

		public bool FastRx
		{
			get
			{
				return this.rBtnFastRxOn.Checked;
			}
			set
			{
				this.rBtnFastRxOn.CheckedChanged -= new EventHandler(this.rBtnFastRx_CheckedChanged);
				this.rBtnFastRxOff.CheckedChanged -= new EventHandler(this.rBtnFastRx_CheckedChanged);
				if (value)
				{
					this.rBtnFastRxOn.Checked = true;
					this.rBtnFastRxOff.Checked = false;
				}
				else
				{
					this.rBtnFastRxOn.Checked = false;
					this.rBtnFastRxOff.Checked = true;
				}
				this.rBtnFastRxOn.CheckedChanged += new EventHandler(this.rBtnFastRx_CheckedChanged);
				this.rBtnFastRxOff.CheckedChanged += new EventHandler(this.rBtnFastRx_CheckedChanged);
			}
		}

		public bool RssiDone
		{
			get
			{
				return this.rssiDone;
			}
			set
			{
				this.rssiDone = value;
				this.ledRssiDone.Checked = value;
			}
		}

		public Decimal RssiValue
		{
			get
			{
				return this.rssiValue;
			}
			set
			{
				this.rssiValue = value;
				this.lblRssiValue.Text = value.ToString("###.0");
			}
		}

		public Decimal RssiThresh
		{
			get
			{
				return this.nudRssiThresh.Value;
			}
			set
			{
				try
				{
					this.nudRssiThresh.ValueChanged -= new EventHandler(this.nudRssiThresh_ValueChanged);
					if (!this.rssiAutoThresh)
						this.nudRssiThresh.Value = value;
					else if (this.AgcReference < -127)
						this.nudRssiThresh.Value = new Decimal(1275, 0, 0, true, (byte)1);
					else
						this.nudRssiThresh.Value = (Decimal)(this.AgcReference - (int)this.AgcSnrMargin);
					this.nudRssiThresh.ValueChanged += new EventHandler(this.nudRssiThresh_ValueChanged);
				}
				catch (Exception)
				{
					this.nudRssiThresh.ValueChanged += new EventHandler(this.nudRssiThresh_ValueChanged);
				}
			}
		}

		public Decimal TimeoutRxStart
		{
			get
			{
				return this.nudTimeoutRxStart.Value;
			}
			set
			{
				try
				{
					this.nudTimeoutRxStart.ValueChanged -= new EventHandler(this.nudTimeoutRxStart_ValueChanged);
					this.nudTimeoutRxStart.Value = (Decimal)(uint)Math.Round(value / new Decimal(1000) / new Decimal(16) / this.BitRate, MidpointRounding.AwayFromZero) * new Decimal(16) / this.BitRate * new Decimal(1000);
					this.nudTimeoutRxStart.ValueChanged += new EventHandler(this.nudTimeoutRxStart_ValueChanged);
				}
				catch (Exception)
				{
					this.nudTimeoutRxStart.ValueChanged += new EventHandler(this.nudTimeoutRxStart_ValueChanged);
				}
			}
		}

		public Decimal TimeoutRssiThresh
		{
			get
			{
				return this.nudTimeoutRssiThresh.Value;
			}
			set
			{
				try
				{
					this.nudTimeoutRssiThresh.ValueChanged -= new EventHandler(this.nudTimeoutRssiThresh_ValueChanged);
					this.nudTimeoutRssiThresh.Value = (Decimal)(uint)Math.Round(value / new Decimal(1000) / new Decimal(16) / this.BitRate, MidpointRounding.AwayFromZero) * new Decimal(16) / this.BitRate * new Decimal(1000);
					this.nudTimeoutRssiThresh.ValueChanged += new EventHandler(this.nudTimeoutRssiThresh_ValueChanged);
				}
				catch (Exception)
				{
					this.nudTimeoutRssiThresh.ValueChanged += new EventHandler(this.nudTimeoutRssiThresh_ValueChanged);
				}
			}
		}

		public bool AutoRxRestartOn
		{
			get
			{
				return this.rBtnRssiPhaseAuto.Checked;
			}
			set
			{
				this.rBtnRssiPhaseAuto.Checked = value;
				this.rBtnRssiPhaseManual.Checked = !value;
				if (!(this.Version < new Version(2, 3)))
					return;
				if (value || this.DataMode == DataModeEnum.Packet && !value)
					this.btnRestartRx.Enabled = false;
				else
					this.btnRestartRx.Enabled = true;
			}
		}

		public bool SensitivityBoostOn
		{
			get
			{
				return this.rBtnSensitivityBoostOn.Checked;
			}
			set
			{
				this.rBtnSensitivityBoostOn.CheckedChanged -= new EventHandler(this.rBtnSensitivityBoost_CheckedChanged);
				this.rBtnSensitivityBoostOff.CheckedChanged -= new EventHandler(this.rBtnSensitivityBoost_CheckedChanged);
				if (value)
				{
					this.rBtnSensitivityBoostOn.Checked = true;
					this.rBtnSensitivityBoostOff.Checked = false;
				}
				else
				{
					this.rBtnSensitivityBoostOn.Checked = false;
					this.rBtnSensitivityBoostOff.Checked = true;
				}
				this.rBtnSensitivityBoostOn.CheckedChanged += new EventHandler(this.rBtnSensitivityBoost_CheckedChanged);
				this.rBtnSensitivityBoostOff.CheckedChanged += new EventHandler(this.rBtnSensitivityBoost_CheckedChanged);
			}
		}

		public Decimal LowBetaAfcOffset
		{
			get
			{
				return this.lowBetaAfcOffset;
			}
			set
			{
				try
				{
					this.nudLowBetaAfcOffset.ValueChanged -= new EventHandler(this.nudLowBetaAfcOffset_ValueChanged);
					this.lowBetaAfcOffset = (Decimal)(sbyte)(value / new Decimal(4880, 0, 0, false, (byte)1)) * new Decimal(4880, 0, 0, false, (byte)1);
					this.nudLowBetaAfcOffset.Value = this.lowBetaAfcOffset;
				}
				catch (Exception)
				{
				}
				finally
				{
					this.nudLowBetaAfcOffset.ValueChanged += new EventHandler(this.nudLowBetaAfcOffset_ValueChanged);
				}
			}
		}

		public bool DagcOn
		{
			get
			{
				return this.rBtnDagcOn.Checked;
			}
			set
			{
				this.rBtnDagcOn.CheckedChanged -= new EventHandler(this.rBtnDagc_CheckedChanged);
				this.rBtnDagcOff.CheckedChanged -= new EventHandler(this.rBtnDagc_CheckedChanged);
				if (value)
				{
					this.rBtnDagcOn.Checked = true;
					this.rBtnDagcOff.Checked = false;
				}
				else
				{
					this.rBtnDagcOn.Checked = false;
					this.rBtnDagcOff.Checked = true;
				}
				this.rBtnDagcOn.CheckedChanged += new EventHandler(this.rBtnDagc_CheckedChanged);
				this.rBtnDagcOff.CheckedChanged += new EventHandler(this.rBtnDagc_CheckedChanged);
			}
		}

		public event BooleanEventHandler AfcLowBetaOnChanged;

		public event DecimalEventHandler LowBetaAfcOffsetChanged;

		public event BooleanEventHandler SensitivityBoostOnChanged;

		public event BooleanEventHandler RssiAutoThreshChanged;

		public event BooleanEventHandler DagcOnChanged;

		public event BooleanEventHandler AgcAutoRefChanged;

		public event ByteEventHandler AgcSnrMarginChanged;

		public event Int32EventHandler AgcRefLevelChanged;

		public event AgcStepEventHandler AgcStepChanged;

		public event LnaZinEventHandler LnaZinChanged;

		public event BooleanEventHandler LnaLowPowerOnChanged;

		public event LnaGainEventHandler LnaGainChanged;

		public event DecimalEventHandler DccFreqChanged;

		public event DecimalEventHandler RxBwChanged;

		public event DecimalEventHandler AfcDccFreqChanged;

		public event DecimalEventHandler AfcRxBwChanged;

		public event OokThreshTypeEventHandler OokThreshTypeChanged;

		public event DecimalEventHandler OokPeakThreshStepChanged;

		public event OokPeakThreshDecEventHandler OokPeakThreshDecChanged;

		public event OokAverageThreshFiltEventHandler OokAverageThreshFiltChanged;

		public event ByteEventHandler OokFixedThreshChanged;

		public event EventHandler FeiStartChanged;

		public event BooleanEventHandler AfcAutoClearOnChanged;

		public event BooleanEventHandler AfcAutoOnChanged;

		public event EventHandler AfcClearChanged;

		public event EventHandler AfcStartChanged;

		public event BooleanEventHandler FastRxChanged;

		public event DecimalEventHandler RssiThreshChanged;

		public event EventHandler RssiStartChanged;

		public event DecimalEventHandler TimeoutRxStartChanged;

		public event DecimalEventHandler TimeoutRssiThreshChanged;

		public event EventHandler RestartRxChanged;

		public event BooleanEventHandler AutoRxRestartOnChanged;

		public event DocumentationChangedEventHandler DocumentationChanged;

		public ReceiverViewControl()
		{
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
				this.components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = (IContainer)new Container();
			this.errorProvider = new ErrorProvider(this.components);
			this.nudRxFilterBw = new NumericUpDownEx();
			this.gBoxDagc = new GroupBoxEx();
			this.label34 = new Label();
			this.panel11 = new Panel();
			this.rBtnDagcOff = new RadioButton();
			this.rBtnDagcOn = new RadioButton();
			this.gBoxLnaSensitivity = new GroupBoxEx();
			this.panel3 = new Panel();
			this.rBtnLnaZin200 = new RadioButton();
			this.rBtnLnaZin50 = new RadioButton();
			this.lblSensitivityBoost = new Label();
			this.lblAGC = new Label();
			this.pnlSensitivityBoost = new Panel();
			this.rBtnSensitivityBoostOff = new RadioButton();
			this.rBtnSensitivityBoostOn = new RadioButton();
			this.label4 = new Label();
			this.label7 = new Label();
			this.panel4 = new Panel();
			this.rBtnLnaLowPowerOff = new RadioButton();
			this.rBtnLnaLowPowerOn = new RadioButton();
			this.gBoxAgc = new GroupBoxEx();
			this.panel2 = new Panel();
			this.rBtnAgcAutoRefOff = new RadioButton();
			this.rBtnAgcAutoRefOn = new RadioButton();
			this.label5 = new Label();
			this.label8 = new Label();
			this.label24 = new Label();
			this.label25 = new Label();
			this.label26 = new Label();
			this.label27 = new Label();
			this.label28 = new Label();
			this.label1 = new Label();
			this.label2 = new Label();
			this.label3 = new Label();
			this.label29 = new Label();
			this.label30 = new Label();
			this.label31 = new Label();
			this.label32 = new Label();
			this.label33 = new Label();
			this.nudAgcStep5 = new NumericUpDown();
			this.nudAgcSnrMargin = new NumericUpDownEx();
			this.nudAgcStep4 = new NumericUpDown();
			this.nudAgcRefLevel = new NumericUpDownEx();
			this.nudAgcStep3 = new NumericUpDown();
			this.nudAgcStep1 = new NumericUpDown();
			this.nudAgcStep2 = new NumericUpDown();
			this.gBoxRssi = new GroupBoxEx();
			this.pnlRssiPhase = new Panel();
			this.rBtnRssiPhaseManual = new RadioButton();
			this.rBtnRssiPhaseAuto = new RadioButton();
			this.label23 = new Label();
			this.btnRestartRx = new Button();
			this.panel7 = new Panel();
			this.rBtnFastRxOff = new RadioButton();
			this.rBtnFastRxOn = new RadioButton();
			this.label21 = new Label();
			this.btnRssiRead = new Button();
			this.label17 = new Label();
			this.label54 = new Label();
			this.label55 = new Label();
			this.label56 = new Label();
			this.lblRssiValue = new Label();
			this.nudRssiThresh = new NumericUpDownEx();
			this.ledRssiDone = new Led();
			this.panel1 = new Panel();
			this.rBtnRssiAutoThreshOff = new RadioButton();
			this.rBtnRssiAutoThreshOn = new RadioButton();
			this.label6 = new Label();
			this.nudTimeoutRxStart = new NumericUpDownEx();
			this.label9 = new Label();
			this.label14 = new Label();
			this.label11 = new Label();
			this.label15 = new Label();
			this.nudTimeoutRssiThresh = new NumericUpDownEx();
			this.gBoxAfcFei = new GroupBoxEx();
			this.nudLowBetaAfcOffset = new NumericUpDownEx();
			this.lblLowBetaAfcOffset = new Label();
			this.lblAfcLowBeta = new Label();
			this.label19 = new Label();
			this.lblLowBetaAfcOfssetUnit = new Label();
			this.label20 = new Label();
			this.pnlAfcLowBeta = new Panel();
			this.rBtnAfcLowBetaOff = new RadioButton();
			this.rBtnAfcLowBetaOn = new RadioButton();
			this.btnFeiRead = new Button();
			this.panel8 = new Panel();
			this.rBtnAfcAutoClearOff = new RadioButton();
			this.rBtnAfcAutoClearOn = new RadioButton();
			this.ledFeiDone = new Led();
			this.panel9 = new Panel();
			this.rBtnAfcAutoOff = new RadioButton();
			this.rBtnAfcAutoOn = new RadioButton();
			this.lblFeiValue = new Label();
			this.label12 = new Label();
			this.label18 = new Label();
			this.label10 = new Label();
			this.btnAfcClear = new Button();
			this.btnAfcStart = new Button();
			this.ledAfcDone = new Led();
			this.lblAfcValue = new Label();
			this.label22 = new Label();
			this.gBoxOok = new GroupBoxEx();
			this.cBoxOokThreshType = new ComboBox();
			this.lblOokType = new Label();
			this.lblOokStep = new Label();
			this.lblOokDec = new Label();
			this.lblOokCutoff = new Label();
			this.lblOokFixed = new Label();
			this.suffixOOKstep = new Label();
			this.suffixOOKfixed = new Label();
			this.nudOokPeakThreshStep = new NumericUpDownEx();
			this.nudOokFixedThresh = new NumericUpDownEx();
			this.cBoxOokPeakThreshDec = new ComboBox();
			this.cBoxOokAverageThreshFilt = new ComboBox();
			this.gBoxAfcBw = new GroupBoxEx();
			this.nudAfcDccFreq = new NumericUpDownEx();
			this.lblAfcDcc = new Label();
			this.lblAfcRxBw = new Label();
			this.suffixAFCDCC = new Label();
			this.suffixAFCRxBw = new Label();
			this.nudRxFilterBwAfc = new NumericUpDownEx();
			this.gBoxRxBw = new GroupBoxEx();
			this.nudDccFreq = new NumericUpDownEx();
			this.lblDcc = new Label();
			this.lblRxBw = new Label();
			this.suffixDCC = new Label();
			this.suffixRxBw = new Label();
			this.gBoxLna = new GroupBoxEx();
			this.panel5 = new Panel();
			this.rBtnLnaGainAutoOff = new RadioButton();
			this.rBtnLnaGainAutoOn = new RadioButton();
			this.label13 = new Label();
			this.label16 = new Label();
			this.lblAgcReference = new Label();
			this.label48 = new Label();
			this.label49 = new Label();
			this.label50 = new Label();
			this.label51 = new Label();
			this.label52 = new Label();
			this.lblLnaGain1 = new Label();
			this.label53 = new Label();
			this.panel6 = new Panel();
			this.rBtnLnaGain1 = new RadioButton();
			this.rBtnLnaGain2 = new RadioButton();
			this.rBtnLnaGain3 = new RadioButton();
			this.rBtnLnaGain4 = new RadioButton();
			this.rBtnLnaGain5 = new RadioButton();
			this.rBtnLnaGain6 = new RadioButton();
			this.lblLnaGain2 = new Label();
			this.lblLnaGain3 = new Label();
			this.lblLnaGain4 = new Label();
			this.lblLnaGain5 = new Label();
			this.lblLnaGain6 = new Label();
			this.lblAgcThresh1 = new Label();
			this.lblAgcThresh2 = new Label();
			this.lblAgcThresh3 = new Label();
			this.lblAgcThresh4 = new Label();
			this.lblAgcThresh5 = new Label();
			this.label47 = new Label();
			this.nudRxFilterBw.BeginInit();
			this.gBoxDagc.SuspendLayout();
			this.panel11.SuspendLayout();
			this.gBoxLnaSensitivity.SuspendLayout();
			this.panel3.SuspendLayout();
			this.pnlSensitivityBoost.SuspendLayout();
			this.panel4.SuspendLayout();
			this.gBoxAgc.SuspendLayout();
			this.panel2.SuspendLayout();
			this.nudAgcStep5.BeginInit();
			this.nudAgcSnrMargin.BeginInit();
			this.nudAgcStep4.BeginInit();
			this.nudAgcRefLevel.BeginInit();
			this.nudAgcStep3.BeginInit();
			this.nudAgcStep1.BeginInit();
			this.nudAgcStep2.BeginInit();
			this.gBoxRssi.SuspendLayout();
			this.pnlRssiPhase.SuspendLayout();
			this.panel7.SuspendLayout();
			this.nudRssiThresh.BeginInit();
			this.panel1.SuspendLayout();
			this.nudTimeoutRxStart.BeginInit();
			this.nudTimeoutRssiThresh.BeginInit();
			this.gBoxAfcFei.SuspendLayout();
			this.nudLowBetaAfcOffset.BeginInit();
			this.pnlAfcLowBeta.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel9.SuspendLayout();
			this.gBoxOok.SuspendLayout();
			this.nudOokPeakThreshStep.BeginInit();
			this.nudOokFixedThresh.BeginInit();
			this.gBoxAfcBw.SuspendLayout();
			this.nudAfcDccFreq.BeginInit();
			this.nudRxFilterBwAfc.BeginInit();
			this.gBoxRxBw.SuspendLayout();
			this.nudDccFreq.BeginInit();
			this.gBoxLna.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel6.SuspendLayout();
			this.SuspendLayout();
			this.errorProvider.ContainerControl = (ContainerControl)this;
			this.errorProvider.SetIconPadding((Control)this.nudRxFilterBw, 30);
			this.nudRxFilterBw.Location = new Point(109, 50);
			int[] bits1 = new int[4];
			bits1[0] = 500000;
			Decimal num1 = new Decimal(bits1);
			this.nudRxFilterBw.Maximum = num1;
			int[] bits2 = new int[4];
			bits2[0] = 3906;
			Decimal num2 = new Decimal(bits2);
			this.nudRxFilterBw.Minimum = num2;
			this.nudRxFilterBw.Name = "nudRxFilterBw";
			this.nudRxFilterBw.Size = new Size(124, 21);
			this.nudRxFilterBw.TabIndex = 4;
			this.nudRxFilterBw.ThousandsSeparator = true;
			int[] bits3 = new int[4];
			bits3[0] = 10417;
			Decimal num3 = new Decimal(bits3);
			this.nudRxFilterBw.Value = num3;
			this.nudRxFilterBw.ValueChanged += new EventHandler(this.nudRxFilterBw_ValueChanged);
			this.gBoxDagc.Controls.Add((Control)this.label34);
			this.gBoxDagc.Controls.Add((Control)this.panel11);
			this.gBoxDagc.Location = new Point(585, 112);
			this.gBoxDagc.Name = "gBoxDagc";
			this.gBoxDagc.Size = new Size(211, 46);
			this.gBoxDagc.TabIndex = 5;
			this.gBoxDagc.TabStop = false;
			this.gBoxDagc.Text = "DAGC";
			this.gBoxDagc.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxDagc.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label34.AutoSize = true;
			this.label34.Location = new Point(11, 19);
			this.label34.Name = "label34";
			this.label34.Size = new Size(35, 12);
			this.label34.TabIndex = 3;
			this.label34.Text = "DAGC:";
			this.panel11.AutoSize = true;
			this.panel11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel11.Controls.Add((Control)this.rBtnDagcOff);
			this.panel11.Controls.Add((Control)this.rBtnDagcOn);
			this.panel11.Location = new Point(106, 18);
			this.panel11.Name = "panel11";
			this.panel11.Size = new Size(89, 16);
			this.panel11.TabIndex = 4;
			this.rBtnDagcOff.AutoSize = true;
			this.rBtnDagcOff.Location = new Point(45, 0);
			this.rBtnDagcOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnDagcOff.Name = "rBtnDagcOff";
			this.rBtnDagcOff.Size = new Size(41, 16);
			this.rBtnDagcOff.TabIndex = 1;
			this.rBtnDagcOff.Text = "OFF";
			this.rBtnDagcOff.UseVisualStyleBackColor = true;
			this.rBtnDagcOff.CheckedChanged += new EventHandler(this.rBtnDagc_CheckedChanged);
			this.rBtnDagcOn.AutoSize = true;
			this.rBtnDagcOn.Checked = true;
			this.rBtnDagcOn.Location = new Point(3, 0);
			this.rBtnDagcOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnDagcOn.Name = "rBtnDagcOn";
			this.rBtnDagcOn.Size = new Size(35, 16);
			this.rBtnDagcOn.TabIndex = 0;
			this.rBtnDagcOn.TabStop = true;
			this.rBtnDagcOn.Text = "ON";
			this.rBtnDagcOn.UseVisualStyleBackColor = true;
			this.rBtnDagcOn.CheckedChanged += new EventHandler(this.rBtnDagc_CheckedChanged);
			this.gBoxLnaSensitivity.Controls.Add((Control)this.panel3);
			this.gBoxLnaSensitivity.Controls.Add((Control)this.lblSensitivityBoost);
			this.gBoxLnaSensitivity.Controls.Add((Control)this.lblAGC);
			this.gBoxLnaSensitivity.Controls.Add((Control)this.pnlSensitivityBoost);
			this.gBoxLnaSensitivity.Controls.Add((Control)this.label4);
			this.gBoxLnaSensitivity.Controls.Add((Control)this.label7);
			this.gBoxLnaSensitivity.Controls.Add((Control)this.panel4);
			this.gBoxLnaSensitivity.Location = new Point(585, 3);
			this.gBoxLnaSensitivity.Name = "gBoxLnaSensitivity";
			this.gBoxLnaSensitivity.Size = new Size(211, 103);
			this.gBoxLnaSensitivity.TabIndex = 5;
			this.gBoxLnaSensitivity.TabStop = false;
			this.gBoxLnaSensitivity.Text = "Lna sensitivity";
			this.gBoxLnaSensitivity.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxLnaSensitivity.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.panel3.AutoSize = true;
			this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel3.Controls.Add((Control)this.rBtnLnaZin200);
			this.panel3.Controls.Add((Control)this.rBtnLnaZin50);
			this.panel3.Location = new Point(106, 18);
			this.panel3.Name = "panel3";
			this.panel3.Size = new Size(47, 32);
			this.panel3.TabIndex = 1;
			this.rBtnLnaZin200.AutoSize = true;
			this.rBtnLnaZin200.Location = new Point(3, 16);
			this.rBtnLnaZin200.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaZin200.Name = "rBtnLnaZin200";
			this.rBtnLnaZin200.Size = new Size(41, 16);
			this.rBtnLnaZin200.TabIndex = 1;
			this.rBtnLnaZin200.Text = "200";
			this.rBtnLnaZin200.UseVisualStyleBackColor = true;
			this.rBtnLnaZin200.CheckedChanged += new EventHandler(this.rBtnLnaZin_CheckedChanged);
			this.rBtnLnaZin50.AutoSize = true;
			this.rBtnLnaZin50.Checked = true;
			this.rBtnLnaZin50.Location = new Point(3, 0);
			this.rBtnLnaZin50.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaZin50.Name = "rBtnLnaZin50";
			this.rBtnLnaZin50.Size = new Size(35, 16);
			this.rBtnLnaZin50.TabIndex = 0;
			this.rBtnLnaZin50.TabStop = true;
			this.rBtnLnaZin50.Text = "50";
			this.rBtnLnaZin50.UseVisualStyleBackColor = true;
			this.rBtnLnaZin50.CheckedChanged += new EventHandler(this.rBtnLnaZin_CheckedChanged);
			this.lblSensitivityBoost.AutoSize = true;
			this.lblSensitivityBoost.Location = new Point(11, 56);
			this.lblSensitivityBoost.Name = "lblSensitivityBoost";
			this.lblSensitivityBoost.Size = new Size(113, 12);
			this.lblSensitivityBoost.TabIndex = 3;
			this.lblSensitivityBoost.Text = "Sensitivity boost:";
			this.lblAGC.AutoSize = true;
			this.lblAGC.Location = new Point(11, 28);
			this.lblAGC.Name = "lblAGC";
			this.lblAGC.Size = new Size(101, 12);
			this.lblAGC.TabIndex = 0;
			this.lblAGC.Text = "Input impedance:";
			this.pnlSensitivityBoost.AutoSize = true;
			this.pnlSensitivityBoost.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlSensitivityBoost.Controls.Add((Control)this.rBtnSensitivityBoostOff);
			this.pnlSensitivityBoost.Controls.Add((Control)this.rBtnSensitivityBoostOn);
			this.pnlSensitivityBoost.Location = new Point(106, 54);
			this.pnlSensitivityBoost.Name = "pnlSensitivityBoost";
			this.pnlSensitivityBoost.Size = new Size(89, 16);
			this.pnlSensitivityBoost.TabIndex = 4;
			this.rBtnSensitivityBoostOff.AutoSize = true;
			this.rBtnSensitivityBoostOff.Location = new Point(45, 0);
			this.rBtnSensitivityBoostOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnSensitivityBoostOff.Name = "rBtnSensitivityBoostOff";
			this.rBtnSensitivityBoostOff.Size = new Size(41, 16);
			this.rBtnSensitivityBoostOff.TabIndex = 1;
			this.rBtnSensitivityBoostOff.Text = "OFF";
			this.rBtnSensitivityBoostOff.UseVisualStyleBackColor = true;
			this.rBtnSensitivityBoostOff.CheckedChanged += new EventHandler(this.rBtnSensitivityBoost_CheckedChanged);
			this.rBtnSensitivityBoostOn.AutoSize = true;
			this.rBtnSensitivityBoostOn.Checked = true;
			this.rBtnSensitivityBoostOn.Location = new Point(3, 0);
			this.rBtnSensitivityBoostOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnSensitivityBoostOn.Name = "rBtnSensitivityBoostOn";
			this.rBtnSensitivityBoostOn.Size = new Size(35, 16);
			this.rBtnSensitivityBoostOn.TabIndex = 0;
			this.rBtnSensitivityBoostOn.TabStop = true;
			this.rBtnSensitivityBoostOn.Text = "ON";
			this.rBtnSensitivityBoostOn.UseVisualStyleBackColor = true;
			this.rBtnSensitivityBoostOn.CheckedChanged += new EventHandler(this.rBtnSensitivityBoost_CheckedChanged);
			this.label4.AutoSize = true;
			this.label4.Location = new Point(11, 79);
			this.label4.Name = "label4";
			this.label4.Size = new Size(101, 12);
			this.label4.TabIndex = 6;
			this.label4.Text = "Mixer low-power:";
			this.label7.AutoSize = true;
			this.label7.BackColor = Color.Transparent;
			this.label7.Location = new Point(161, 28);
			this.label7.Name = "label7";
			this.label7.Size = new Size(29, 12);
			this.label7.TabIndex = 2;
			this.label7.Text = "ohms";
			this.panel4.AutoSize = true;
			this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel4.Controls.Add((Control)this.rBtnLnaLowPowerOff);
			this.panel4.Controls.Add((Control)this.rBtnLnaLowPowerOn);
			this.panel4.Location = new Point(106, 78);
			this.panel4.Name = "panel4";
			this.panel4.Size = new Size(89, 16);
			this.panel4.TabIndex = 5;
			this.rBtnLnaLowPowerOff.AutoSize = true;
			this.rBtnLnaLowPowerOff.BackColor = Color.Transparent;
			this.rBtnLnaLowPowerOff.Location = new Point(45, 0);
			this.rBtnLnaLowPowerOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaLowPowerOff.Name = "rBtnLnaLowPowerOff";
			this.rBtnLnaLowPowerOff.Size = new Size(41, 16);
			this.rBtnLnaLowPowerOff.TabIndex = 1;
			this.rBtnLnaLowPowerOff.Text = "OFF";
			this.rBtnLnaLowPowerOff.UseVisualStyleBackColor = false;
			this.rBtnLnaLowPowerOff.CheckedChanged += new EventHandler(this.rBtnLnaLowPower_CheckedChanged);
			this.rBtnLnaLowPowerOn.AutoSize = true;
			this.rBtnLnaLowPowerOn.BackColor = Color.Transparent;
			this.rBtnLnaLowPowerOn.Checked = true;
			this.rBtnLnaLowPowerOn.Location = new Point(3, 0);
			this.rBtnLnaLowPowerOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaLowPowerOn.Name = "rBtnLnaLowPowerOn";
			this.rBtnLnaLowPowerOn.Size = new Size(35, 16);
			this.rBtnLnaLowPowerOn.TabIndex = 0;
			this.rBtnLnaLowPowerOn.TabStop = true;
			this.rBtnLnaLowPowerOn.Text = "ON";
			this.rBtnLnaLowPowerOn.UseVisualStyleBackColor = false;
			this.rBtnLnaLowPowerOn.CheckedChanged += new EventHandler(this.rBtnLnaLowPower_CheckedChanged);
			this.gBoxAgc.Controls.Add((Control)this.panel2);
			this.gBoxAgc.Controls.Add((Control)this.label5);
			this.gBoxAgc.Controls.Add((Control)this.label8);
			this.gBoxAgc.Controls.Add((Control)this.label24);
			this.gBoxAgc.Controls.Add((Control)this.label25);
			this.gBoxAgc.Controls.Add((Control)this.label26);
			this.gBoxAgc.Controls.Add((Control)this.label27);
			this.gBoxAgc.Controls.Add((Control)this.label28);
			this.gBoxAgc.Controls.Add((Control)this.label1);
			this.gBoxAgc.Controls.Add((Control)this.label2);
			this.gBoxAgc.Controls.Add((Control)this.label3);
			this.gBoxAgc.Controls.Add((Control)this.label29);
			this.gBoxAgc.Controls.Add((Control)this.label30);
			this.gBoxAgc.Controls.Add((Control)this.label31);
			this.gBoxAgc.Controls.Add((Control)this.label32);
			this.gBoxAgc.Controls.Add((Control)this.label33);
			this.gBoxAgc.Controls.Add((Control)this.nudAgcStep5);
			this.gBoxAgc.Controls.Add((Control)this.nudAgcSnrMargin);
			this.gBoxAgc.Controls.Add((Control)this.nudAgcStep4);
			this.gBoxAgc.Controls.Add((Control)this.nudAgcRefLevel);
			this.gBoxAgc.Controls.Add((Control)this.nudAgcStep3);
			this.gBoxAgc.Controls.Add((Control)this.nudAgcStep1);
			this.gBoxAgc.Controls.Add((Control)this.nudAgcStep2);
			this.gBoxAgc.Location = new Point(585, 112);
			this.gBoxAgc.Name = "gBoxAgc";
			this.gBoxAgc.Size = new Size(211, 232);
			this.gBoxAgc.TabIndex = 6;
			this.gBoxAgc.TabStop = false;
			this.gBoxAgc.Text = "AGC";
			this.gBoxAgc.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxAgc.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.panel2.AutoSize = true;
			this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel2.Controls.Add((Control)this.rBtnAgcAutoRefOff);
			this.panel2.Controls.Add((Control)this.rBtnAgcAutoRefOn);
			this.panel2.Location = new Point(110, 21);
			this.panel2.Name = "panel2";
			this.panel2.Size = new Size(47, 32);
			this.panel2.TabIndex = 1;
			this.rBtnAgcAutoRefOff.AutoSize = true;
			this.rBtnAgcAutoRefOff.Location = new Point(3, 16);
			this.rBtnAgcAutoRefOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAgcAutoRefOff.Name = "rBtnAgcAutoRefOff";
			this.rBtnAgcAutoRefOff.Size = new Size(41, 16);
			this.rBtnAgcAutoRefOff.TabIndex = 1;
			this.rBtnAgcAutoRefOff.Text = "OFF";
			this.rBtnAgcAutoRefOff.UseVisualStyleBackColor = true;
			this.rBtnAgcAutoRefOff.CheckedChanged += new EventHandler(this.rBtnAgcAutoRef_CheckedChanged);
			this.rBtnAgcAutoRefOn.AutoSize = true;
			this.rBtnAgcAutoRefOn.Checked = true;
			this.rBtnAgcAutoRefOn.Location = new Point(3, 0);
			this.rBtnAgcAutoRefOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAgcAutoRefOn.Name = "rBtnAgcAutoRefOn";
			this.rBtnAgcAutoRefOn.Size = new Size(35, 16);
			this.rBtnAgcAutoRefOn.TabIndex = 0;
			this.rBtnAgcAutoRefOn.TabStop = true;
			this.rBtnAgcAutoRefOn.Text = "ON";
			this.rBtnAgcAutoRefOn.UseVisualStyleBackColor = true;
			this.rBtnAgcAutoRefOn.CheckedChanged += new EventHandler(this.rBtnAgcAutoRef_CheckedChanged);
			this.label5.AutoSize = true;
			this.label5.Location = new Point(15, 31);
			this.label5.Name = "label5";
			this.label5.Size = new Size(95, 12);
			this.label5.TabIndex = 0;
			this.label5.Text = "Auto reference:";
			this.label8.AutoSize = true;
			this.label8.BackColor = Color.Transparent;
			this.label8.Location = new Point(15, 60);
			this.label8.Name = "label8";
			this.label8.Size = new Size(101, 12);
			this.label8.TabIndex = 2;
			this.label8.Text = "Reference Level:";
			this.label24.AutoSize = true;
			this.label24.BackColor = Color.Transparent;
			this.label24.Location = new Point(15, 108);
			this.label24.Name = "label24";
			this.label24.Size = new Size(107, 12);
			this.label24.TabIndex = 8;
			this.label24.Text = "Threshold step 1:";
			this.label25.AutoSize = true;
			this.label25.BackColor = Color.Transparent;
			this.label25.Location = new Point(15, 132);
			this.label25.Name = "label25";
			this.label25.Size = new Size(107, 12);
			this.label25.TabIndex = 11;
			this.label25.Text = "Threshold step 2:";
			this.label26.AutoSize = true;
			this.label26.BackColor = Color.Transparent;
			this.label26.Location = new Point(15, 156);
			this.label26.Name = "label26";
			this.label26.Size = new Size(107, 12);
			this.label26.TabIndex = 14;
			this.label26.Text = "Threshold step 3:";
			this.label27.AutoSize = true;
			this.label27.BackColor = Color.Transparent;
			this.label27.Location = new Point(15, 180);
			this.label27.Name = "label27";
			this.label27.Size = new Size(107, 12);
			this.label27.TabIndex = 17;
			this.label27.Text = "Threshold step 4:";
			this.label28.AutoSize = true;
			this.label28.BackColor = Color.Transparent;
			this.label28.Location = new Point(15, 204);
			this.label28.Name = "label28";
			this.label28.Size = new Size(107, 12);
			this.label28.TabIndex = 20;
			this.label28.Text = "Threshold step 5:";
			this.label1.AutoSize = true;
			this.label1.BackColor = Color.Transparent;
			this.label1.Location = new Point(167, 60);
			this.label1.Name = "label1";
			this.label1.Size = new Size(23, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "dBm";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(15, 86);
			this.label2.Name = "label2";
			this.label2.Size = new Size(71, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "SNR margin:";
			this.label3.AutoSize = true;
			this.label3.BackColor = Color.Transparent;
			this.label3.Location = new Point(167, 85);
			this.label3.Name = "label3";
			this.label3.Size = new Size(17, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "dB";
			this.label29.AutoSize = true;
			this.label29.BackColor = Color.Transparent;
			this.label29.Location = new Point(167, 109);
			this.label29.Name = "label29";
			this.label29.Size = new Size(17, 12);
			this.label29.TabIndex = 10;
			this.label29.Text = "dB";
			this.label30.AutoSize = true;
			this.label30.BackColor = Color.Transparent;
			this.label30.Location = new Point(167, 133);
			this.label30.Name = "label30";
			this.label30.Size = new Size(17, 12);
			this.label30.TabIndex = 13;
			this.label30.Text = "dB";
			this.label31.AutoSize = true;
			this.label31.BackColor = Color.Transparent;
			this.label31.Location = new Point(167, 157);
			this.label31.Name = "label31";
			this.label31.Size = new Size(17, 12);
			this.label31.TabIndex = 16;
			this.label31.Text = "dB";
			this.label32.AutoSize = true;
			this.label32.BackColor = Color.Transparent;
			this.label32.Location = new Point(167, 181);
			this.label32.Name = "label32";
			this.label32.Size = new Size(17, 12);
			this.label32.TabIndex = 19;
			this.label32.Text = "dB";
			this.label33.AutoSize = true;
			this.label33.BackColor = Color.Transparent;
			this.label33.Location = new Point(167, 205);
			this.label33.Name = "label33";
			this.label33.Size = new Size(17, 12);
			this.label33.TabIndex = 22;
			this.label33.Text = "dB";
			this.nudAgcStep5.Location = new Point(110, 202);
			int[] bits4 = new int[4];
			bits4[0] = 15;
			Decimal num4 = new Decimal(bits4);
			this.nudAgcStep5.Maximum = num4;
			this.nudAgcStep5.Name = "nudAgcStep5";
			this.nudAgcStep5.Size = new Size(51, 21);
			this.nudAgcStep5.TabIndex = 21;
			int[] bits5 = new int[4];
			bits5[0] = 11;
			Decimal num5 = new Decimal(bits5);
			this.nudAgcStep5.Value = num5;
			this.nudAgcStep5.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
			this.nudAgcSnrMargin.Location = new Point(110, 82);
			int[] bits6 = new int[4];
			bits6[0] = 7;
			Decimal num6 = new Decimal(bits6);
			this.nudAgcSnrMargin.Maximum = num6;
			this.nudAgcSnrMargin.Name = "nudAgcSnrMargin";
			this.nudAgcSnrMargin.Size = new Size(51, 21);
			this.nudAgcSnrMargin.TabIndex = 6;
			this.nudAgcSnrMargin.ThousandsSeparator = true;
			int[] bits7 = new int[4];
			bits7[0] = 5;
			Decimal num7 = new Decimal(bits7);
			this.nudAgcSnrMargin.Value = num7;
			this.nudAgcSnrMargin.ValueChanged += new EventHandler(this.nudAgcSnrMargin_ValueChanged);
			this.nudAgcStep4.Location = new Point(110, 178);
			int[] bits8 = new int[4];
			bits8[0] = 15;
			Decimal num8 = new Decimal(bits8);
			this.nudAgcStep4.Maximum = num8;
			this.nudAgcStep4.Name = "nudAgcStep4";
			this.nudAgcStep4.Size = new Size(51, 21);
			this.nudAgcStep4.TabIndex = 18;
			int[] bits9 = new int[4];
			bits9[0] = 9;
			Decimal num9 = new Decimal(bits9);
			this.nudAgcStep4.Value = num9;
			this.nudAgcStep4.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
			this.nudAgcRefLevel.Location = new Point(110, 58);
			this.nudAgcRefLevel.Maximum = new Decimal(new int[4]
      {
        80,
        0,
        0,
        int.MinValue
      });
			this.nudAgcRefLevel.Minimum = new Decimal(new int[4]
      {
        143,
        0,
        0,
        int.MinValue
      });
			this.nudAgcRefLevel.Name = "nudAgcRefLevel";
			this.nudAgcRefLevel.Size = new Size(51, 21);
			this.nudAgcRefLevel.TabIndex = 3;
			this.nudAgcRefLevel.ThousandsSeparator = true;
			this.nudAgcRefLevel.Value = new Decimal(new int[4]
      {
        80,
        0,
        0,
        int.MinValue
      });
			this.nudAgcRefLevel.ValueChanged += new EventHandler(this.nudAgcRefLevel_ValueChanged);
			this.nudAgcStep3.Location = new Point(110, 154);
			int[] bits10 = new int[4];
			bits10[0] = 15;
			Decimal num10 = new Decimal(bits10);
			this.nudAgcStep3.Maximum = num10;
			this.nudAgcStep3.Name = "nudAgcStep3";
			this.nudAgcStep3.Size = new Size(51, 21);
			this.nudAgcStep3.TabIndex = 15;
			int[] bits11 = new int[4];
			bits11[0] = 11;
			Decimal num11 = new Decimal(bits11);
			this.nudAgcStep3.Value = num11;
			this.nudAgcStep3.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
			this.nudAgcStep1.Location = new Point(110, 106);
			int[] bits12 = new int[4];
			bits12[0] = 31;
			Decimal num12 = new Decimal(bits12);
			this.nudAgcStep1.Maximum = num12;
			this.nudAgcStep1.Name = "nudAgcStep1";
			this.nudAgcStep1.Size = new Size(51, 21);
			this.nudAgcStep1.TabIndex = 9;
			int[] bits13 = new int[4];
			bits13[0] = 16;
			Decimal num13 = new Decimal(bits13);
			this.nudAgcStep1.Value = num13;
			this.nudAgcStep1.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
			this.nudAgcStep2.Location = new Point(110, 130);
			int[] bits14 = new int[4];
			bits14[0] = 15;
			Decimal num14 = new Decimal(bits14);
			this.nudAgcStep2.Maximum = num14;
			this.nudAgcStep2.Name = "nudAgcStep2";
			this.nudAgcStep2.Size = new Size(51, 21);
			this.nudAgcStep2.TabIndex = 12;
			int[] bits15 = new int[4];
			bits15[0] = 7;
			Decimal num15 = new Decimal(bits15);
			this.nudAgcStep2.Value = num15;
			this.nudAgcStep2.ValueChanged += new EventHandler(this.nudAgcStep_ValueChanged);
			this.gBoxRssi.Controls.Add((Control)this.pnlRssiPhase);
			this.gBoxRssi.Controls.Add((Control)this.label23);
			this.gBoxRssi.Controls.Add((Control)this.btnRestartRx);
			this.gBoxRssi.Controls.Add((Control)this.panel7);
			this.gBoxRssi.Controls.Add((Control)this.label21);
			this.gBoxRssi.Controls.Add((Control)this.btnRssiRead);
			this.gBoxRssi.Controls.Add((Control)this.label17);
			this.gBoxRssi.Controls.Add((Control)this.label54);
			this.gBoxRssi.Controls.Add((Control)this.label55);
			this.gBoxRssi.Controls.Add((Control)this.label56);
			this.gBoxRssi.Controls.Add((Control)this.lblRssiValue);
			this.gBoxRssi.Controls.Add((Control)this.nudRssiThresh);
			this.gBoxRssi.Controls.Add((Control)this.ledRssiDone);
			this.gBoxRssi.Controls.Add((Control)this.panel1);
			this.gBoxRssi.Controls.Add((Control)this.label6);
			this.gBoxRssi.Controls.Add((Control)this.nudTimeoutRxStart);
			this.gBoxRssi.Controls.Add((Control)this.label9);
			this.gBoxRssi.Controls.Add((Control)this.label14);
			this.gBoxRssi.Controls.Add((Control)this.label11);
			this.gBoxRssi.Controls.Add((Control)this.label15);
			this.gBoxRssi.Controls.Add((Control)this.nudTimeoutRssiThresh);
			this.gBoxRssi.Location = new Point(294, 163);
			this.gBoxRssi.Name = "gBoxRssi";
			this.gBoxRssi.Size = new Size(285, 180);
			this.gBoxRssi.TabIndex = 4;
			this.gBoxRssi.TabStop = false;
			this.gBoxRssi.Text = "RSSI";
			this.gBoxRssi.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxRssi.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.pnlRssiPhase.AutoSize = true;
			this.pnlRssiPhase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlRssiPhase.Controls.Add((Control)this.rBtnRssiPhaseManual);
			this.pnlRssiPhase.Controls.Add((Control)this.rBtnRssiPhaseAuto);
			this.pnlRssiPhase.Location = new Point(133, 155);
			this.pnlRssiPhase.Margin = new Padding(3, 2, 3, 2);
			this.pnlRssiPhase.Name = "pnlRssiPhase";
			this.pnlRssiPhase.Size = new Size(118, 19);
			this.pnlRssiPhase.TabIndex = 20;
			this.rBtnRssiPhaseManual.AutoSize = true;
			this.rBtnRssiPhaseManual.Location = new Point(56, 3);
			this.rBtnRssiPhaseManual.Margin = new Padding(3, 0, 3, 0);
			this.rBtnRssiPhaseManual.Name = "rBtnRssiPhaseManual";
			this.rBtnRssiPhaseManual.Size = new Size(59, 16);
			this.rBtnRssiPhaseManual.TabIndex = 1;
			this.rBtnRssiPhaseManual.Text = "Manual";
			this.rBtnRssiPhaseManual.UseVisualStyleBackColor = true;
			this.rBtnRssiPhaseManual.CheckedChanged += new EventHandler(this.rBtnRssiPhaseManual_CheckedChanged);
			this.rBtnRssiPhaseAuto.AutoSize = true;
			this.rBtnRssiPhaseAuto.Checked = true;
			this.rBtnRssiPhaseAuto.Location = new Point(3, 3);
			this.rBtnRssiPhaseAuto.Margin = new Padding(3, 0, 3, 0);
			this.rBtnRssiPhaseAuto.Name = "rBtnRssiPhaseAuto";
			this.rBtnRssiPhaseAuto.Size = new Size(47, 16);
			this.rBtnRssiPhaseAuto.TabIndex = 0;
			this.rBtnRssiPhaseAuto.TabStop = true;
			this.rBtnRssiPhaseAuto.Text = "Auto";
			this.rBtnRssiPhaseAuto.UseVisualStyleBackColor = true;
			this.rBtnRssiPhaseAuto.CheckedChanged += new EventHandler(this.rBtnRssiPhaseAuto_CheckedChanged);
			this.label23.AutoSize = true;
			this.label23.Location = new Point(6, 159);
			this.label23.Name = "label23";
			this.label23.Size = new Size(41, 12);
			this.label23.TabIndex = 18;
			this.label23.Text = "Phase:";
			this.label23.TextAlign = ContentAlignment.MiddleLeft;
			this.btnRestartRx.Location = new Point(60, 154);
			this.btnRestartRx.Name = "btnRestartRx";
			this.btnRestartRx.Size = new Size(67, 21);
			this.btnRestartRx.TabIndex = 19;
			this.btnRestartRx.Text = "Restart Rx";
			this.btnRestartRx.UseVisualStyleBackColor = true;
			this.btnRestartRx.Click += new EventHandler(this.btnRestartRx_Click);
			this.panel7.AutoSize = true;
			this.panel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel7.Controls.Add((Control)this.rBtnFastRxOff);
			this.panel7.Controls.Add((Control)this.rBtnFastRxOn);
			this.panel7.Location = new Point(133, 18);
			this.panel7.Name = "panel7";
			this.panel7.Size = new Size(94, 16);
			this.panel7.TabIndex = 1;
			this.rBtnFastRxOff.AutoSize = true;
			this.rBtnFastRxOff.Location = new Point(50, 0);
			this.rBtnFastRxOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnFastRxOff.Name = "rBtnFastRxOff";
			this.rBtnFastRxOff.Size = new Size(41, 16);
			this.rBtnFastRxOff.TabIndex = 1;
			this.rBtnFastRxOff.Text = "OFF";
			this.rBtnFastRxOff.UseVisualStyleBackColor = true;
			this.rBtnFastRxOn.AutoSize = true;
			this.rBtnFastRxOn.Checked = true;
			this.rBtnFastRxOn.Location = new Point(3, 0);
			this.rBtnFastRxOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnFastRxOn.Name = "rBtnFastRxOn";
			this.rBtnFastRxOn.Size = new Size(35, 16);
			this.rBtnFastRxOn.TabIndex = 0;
			this.rBtnFastRxOn.TabStop = true;
			this.rBtnFastRxOn.Text = "ON";
			this.rBtnFastRxOn.UseVisualStyleBackColor = true;
			this.rBtnFastRxOn.CheckedChanged += new EventHandler(this.rBtnFastRx_CheckedChanged);
			this.label21.AutoSize = true;
			this.label21.Location = new Point(3, 19);
			this.label21.Name = "label21";
			this.label21.Size = new Size(95, 12);
			this.label21.TabIndex = 0;
			this.label21.Text = "Fast Rx wakeup:";
			this.btnRssiRead.Location = new Point(86, 131);
			this.btnRssiRead.Name = "btnRssiRead";
			this.btnRssiRead.Size = new Size(41, 21);
			this.btnRssiRead.TabIndex = 14;
			this.btnRssiRead.Text = "Read";
			this.btnRssiRead.UseVisualStyleBackColor = true;
			this.btnRssiRead.Visible = false;
			this.btnRssiRead.Click += new EventHandler(this.btnRssiStart_Click);
			this.label17.AutoSize = true;
			this.label17.BackColor = Color.Transparent;
			this.label17.Location = new Point((int)byte.MaxValue, 112);
			this.label17.Name = "label17";
			this.label17.Size = new Size(23, 12);
			this.label17.TabIndex = 12;
			this.label17.Text = "dBm";
			this.label17.TextAlign = ContentAlignment.MiddleCenter;
			this.label54.AutoSize = true;
			this.label54.BackColor = Color.Transparent;
			this.label54.Location = new Point((int)byte.MaxValue, 136);
			this.label54.Name = "label54";
			this.label54.Size = new Size(23, 12);
			this.label54.TabIndex = 17;
			this.label54.Text = "dBm";
			this.label54.TextAlign = ContentAlignment.MiddleCenter;
			this.label55.AutoSize = true;
			this.label55.BackColor = Color.Transparent;
			this.label55.Location = new Point(3, 112);
			this.label55.Margin = new Padding(0);
			this.label55.Name = "label55";
			this.label55.Size = new Size(65, 12);
			this.label55.TabIndex = 10;
			this.label55.Text = "Threshold:";
			this.label55.TextAlign = ContentAlignment.MiddleCenter;
			this.label56.AutoSize = true;
			this.label56.BackColor = Color.Transparent;
			this.label56.Location = new Point(3, 136);
			this.label56.Margin = new Padding(0);
			this.label56.Name = "label56";
			this.label56.Size = new Size(41, 12);
			this.label56.TabIndex = 13;
			this.label56.Text = "Value:";
			this.label56.TextAlign = ContentAlignment.MiddleCenter;
			this.lblRssiValue.BackColor = Color.Transparent;
			this.lblRssiValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblRssiValue.Location = new Point(133, 132);
			this.lblRssiValue.Margin = new Padding(3);
			this.lblRssiValue.Name = "lblRssiValue";
			this.lblRssiValue.Size = new Size(98, 18);
			this.lblRssiValue.TabIndex = 15;
			this.lblRssiValue.Text = "0";
			this.lblRssiValue.TextAlign = ContentAlignment.MiddleCenter;
			this.nudRssiThresh.DecimalPlaces = 1;
			this.nudRssiThresh.Enabled = false;
			this.nudRssiThresh.Increment = new Decimal(new int[4]
      {
        5,
        0,
        0,
        65536
      });
			this.nudRssiThresh.Location = new Point(133, 108);
			this.nudRssiThresh.Maximum = new Decimal(new int[4]);
			this.nudRssiThresh.Minimum = new Decimal(new int[4]
      {
        1275,
        0,
        0,
        -2147418112
      });
			this.nudRssiThresh.Name = "nudRssiThresh";
			this.nudRssiThresh.Size = new Size(98, 21);
			this.nudRssiThresh.TabIndex = 11;
			this.nudRssiThresh.ThousandsSeparator = true;
			this.nudRssiThresh.Value = new Decimal(new int[4]
      {
        80,
        0,
        0,
        int.MinValue
      });
			this.nudRssiThresh.ValueChanged += new EventHandler(this.nudRssiThresh_ValueChanged);
			this.ledRssiDone.BackColor = Color.Transparent;
			this.ledRssiDone.LedColor = Color.Green;
			this.ledRssiDone.LedSize = new Size(11, 11);
			this.ledRssiDone.Location = new Point(234, 135);
			this.ledRssiDone.Name = "ledRssiDone";
			this.ledRssiDone.Size = new Size(15, 14);
			this.ledRssiDone.TabIndex = 16;
			this.ledRssiDone.Text = "led1";
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add((Control)this.rBtnRssiAutoThreshOff);
			this.panel1.Controls.Add((Control)this.rBtnRssiAutoThreshOn);
			this.panel1.Location = new Point(133, 87);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(94, 16);
			this.panel1.TabIndex = 9;
			this.rBtnRssiAutoThreshOff.AutoSize = true;
			this.rBtnRssiAutoThreshOff.Location = new Point(50, 0);
			this.rBtnRssiAutoThreshOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnRssiAutoThreshOff.Name = "rBtnRssiAutoThreshOff";
			this.rBtnRssiAutoThreshOff.Size = new Size(41, 16);
			this.rBtnRssiAutoThreshOff.TabIndex = 1;
			this.rBtnRssiAutoThreshOff.Text = "OFF";
			this.rBtnRssiAutoThreshOff.UseVisualStyleBackColor = true;
			this.rBtnRssiAutoThreshOff.CheckedChanged += new EventHandler(this.rBtnRssiAutoThreshOn_CheckedChanged);
			this.rBtnRssiAutoThreshOn.AutoSize = true;
			this.rBtnRssiAutoThreshOn.Checked = true;
			this.rBtnRssiAutoThreshOn.Location = new Point(3, 0);
			this.rBtnRssiAutoThreshOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnRssiAutoThreshOn.Name = "rBtnRssiAutoThreshOn";
			this.rBtnRssiAutoThreshOn.Size = new Size(35, 16);
			this.rBtnRssiAutoThreshOn.TabIndex = 0;
			this.rBtnRssiAutoThreshOn.TabStop = true;
			this.rBtnRssiAutoThreshOn.Text = "ON";
			this.rBtnRssiAutoThreshOn.UseVisualStyleBackColor = true;
			this.rBtnRssiAutoThreshOn.CheckedChanged += new EventHandler(this.rBtnRssiAutoThreshOn_CheckedChanged);
			this.label6.AutoSize = true;
			this.label6.Location = new Point(3, 89);
			this.label6.Name = "label6";
			this.label6.Size = new Size(95, 12);
			this.label6.TabIndex = 8;
			this.label6.Text = "Auto threshold:";
			this.nudTimeoutRxStart.Location = new Point(133, 39);
			int[] bits16 = new int[4];
			bits16[0] = 850;
			Decimal num16 = new Decimal(bits16);
			this.nudTimeoutRxStart.Maximum = num16;
			this.nudTimeoutRxStart.Name = "nudTimeoutRxStart";
			this.nudTimeoutRxStart.Size = new Size(98, 21);
			this.nudTimeoutRxStart.TabIndex = 3;
			this.nudTimeoutRxStart.ThousandsSeparator = true;
			this.nudTimeoutRxStart.ValueChanged += new EventHandler(this.nudTimeoutRxStart_ValueChanged);
			this.label9.AutoSize = true;
			this.label9.Location = new Point(3, 42);
			this.label9.Name = "label9";
			this.label9.Size = new Size(107, 12);
			this.label9.TabIndex = 2;
			this.label9.Text = "Timeout Rx start:";
			this.label14.AutoSize = true;
			this.label14.Location = new Point(3, 66);
			this.label14.Name = "label14";
			this.label14.Size = new Size(113, 12);
			this.label14.TabIndex = 5;
			this.label14.Text = "Timeout threshold:";
			this.label11.AutoSize = true;
			this.label11.Location = new Point((int)byte.MaxValue, 42);
			this.label11.Name = "label11";
			this.label11.Size = new Size(17, 12);
			this.label11.TabIndex = 4;
			this.label11.Text = "ms";
			this.label15.AutoSize = true;
			this.label15.Location = new Point((int)byte.MaxValue, 66);
			this.label15.Name = "label15";
			this.label15.Size = new Size(17, 12);
			this.label15.TabIndex = 7;
			this.label15.Text = "ms";
			this.nudTimeoutRssiThresh.Location = new Point(133, 63);
			int[] bits17 = new int[4];
			bits17[0] = 850;
			Decimal num17 = new Decimal(bits17);
			this.nudTimeoutRssiThresh.Maximum = num17;
			this.nudTimeoutRssiThresh.Name = "nudTimeoutRssiThresh";
			this.nudTimeoutRssiThresh.Size = new Size(98, 21);
			this.nudTimeoutRssiThresh.TabIndex = 6;
			this.nudTimeoutRssiThresh.ThousandsSeparator = true;
			this.nudTimeoutRssiThresh.ValueChanged += new EventHandler(this.nudTimeoutRssiThresh_ValueChanged);
			this.gBoxAfcFei.Controls.Add((Control)this.nudLowBetaAfcOffset);
			this.gBoxAfcFei.Controls.Add((Control)this.lblLowBetaAfcOffset);
			this.gBoxAfcFei.Controls.Add((Control)this.lblAfcLowBeta);
			this.gBoxAfcFei.Controls.Add((Control)this.label19);
			this.gBoxAfcFei.Controls.Add((Control)this.lblLowBetaAfcOfssetUnit);
			this.gBoxAfcFei.Controls.Add((Control)this.label20);
			this.gBoxAfcFei.Controls.Add((Control)this.pnlAfcLowBeta);
			this.gBoxAfcFei.Controls.Add((Control)this.btnFeiRead);
			this.gBoxAfcFei.Controls.Add((Control)this.panel8);
			this.gBoxAfcFei.Controls.Add((Control)this.ledFeiDone);
			this.gBoxAfcFei.Controls.Add((Control)this.panel9);
			this.gBoxAfcFei.Controls.Add((Control)this.lblFeiValue);
			this.gBoxAfcFei.Controls.Add((Control)this.label12);
			this.gBoxAfcFei.Controls.Add((Control)this.label18);
			this.gBoxAfcFei.Controls.Add((Control)this.label10);
			this.gBoxAfcFei.Controls.Add((Control)this.btnAfcClear);
			this.gBoxAfcFei.Controls.Add((Control)this.btnAfcStart);
			this.gBoxAfcFei.Controls.Add((Control)this.ledAfcDone);
			this.gBoxAfcFei.Controls.Add((Control)this.lblAfcValue);
			this.gBoxAfcFei.Controls.Add((Control)this.label22);
			this.gBoxAfcFei.Location = new Point(294, 3);
			this.gBoxAfcFei.Name = "gBoxAfcFei";
			this.gBoxAfcFei.Size = new Size(285, 155);
			this.gBoxAfcFei.TabIndex = 3;
			this.gBoxAfcFei.TabStop = false;
			this.gBoxAfcFei.Text = "AFC / FEI";
			this.gBoxAfcFei.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxAfcFei.MouseLeave += new EventHandler(this.control_MouseLeave);
			int[] bits18 = new int[4];
			bits18[0] = 488;
			Decimal num18 = new Decimal(bits18);
			this.nudLowBetaAfcOffset.Increment = num18;
			this.nudLowBetaAfcOffset.Location = new Point(133, 39);
			int[] bits19 = new int[4];
			bits19[0] = 61976;
			Decimal num19 = new Decimal(bits19);
			this.nudLowBetaAfcOffset.Maximum = num19;
			this.nudLowBetaAfcOffset.Minimum = new Decimal(new int[4]
      {
        62464,
        0,
        0,
        int.MinValue
      });
			this.nudLowBetaAfcOffset.Name = "nudLowBetaAfcOffset";
			this.nudLowBetaAfcOffset.Size = new Size(98, 21);
			this.nudLowBetaAfcOffset.TabIndex = 2;
			this.nudLowBetaAfcOffset.ThousandsSeparator = true;
			this.nudLowBetaAfcOffset.ValueChanged += new EventHandler(this.nudLowBetaAfcOffset_ValueChanged);
			this.lblLowBetaAfcOffset.AutoSize = true;
			this.lblLowBetaAfcOffset.Location = new Point(3, 41);
			this.lblLowBetaAfcOffset.Name = "lblLowBetaAfcOffset";
			this.lblLowBetaAfcOffset.Size = new Size(125, 12);
			this.lblLowBetaAfcOffset.TabIndex = 3;
			this.lblLowBetaAfcOffset.Text = "AFC low beta offset:";
			this.lblAfcLowBeta.AutoSize = true;
			this.lblAfcLowBeta.Location = new Point(3, 19);
			this.lblAfcLowBeta.Name = "lblAfcLowBeta";
			this.lblAfcLowBeta.Size = new Size(83, 12);
			this.lblAfcLowBeta.TabIndex = 0;
			this.lblAfcLowBeta.Text = "AFC low beta:";
			this.label19.AutoSize = true;
			this.label19.Location = new Point(3, 65);
			this.label19.Name = "label19";
			this.label19.Size = new Size(95, 12);
			this.label19.TabIndex = 5;
			this.label19.Text = "AFC auto clear:";
			this.lblLowBetaAfcOfssetUnit.AutoSize = true;
			this.lblLowBetaAfcOfssetUnit.Location = new Point((int)byte.MaxValue, 42);
			this.lblLowBetaAfcOfssetUnit.Name = "lblLowBetaAfcOfssetUnit";
			this.lblLowBetaAfcOfssetUnit.Size = new Size(17, 12);
			this.lblLowBetaAfcOfssetUnit.TabIndex = 4;
			this.lblLowBetaAfcOfssetUnit.Text = "Hz";
			this.label20.AutoSize = true;
			this.label20.Location = new Point(3, 86);
			this.label20.Name = "label20";
			this.label20.Size = new Size(59, 12);
			this.label20.TabIndex = 8;
			this.label20.Text = "AFC auto:";
			this.pnlAfcLowBeta.AutoSize = true;
			this.pnlAfcLowBeta.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlAfcLowBeta.Controls.Add((Control)this.rBtnAfcLowBetaOff);
			this.pnlAfcLowBeta.Controls.Add((Control)this.rBtnAfcLowBetaOn);
			this.pnlAfcLowBeta.Location = new Point(133, 18);
			this.pnlAfcLowBeta.Name = "pnlAfcLowBeta";
			this.pnlAfcLowBeta.Size = new Size(94, 16);
			this.pnlAfcLowBeta.TabIndex = 1;
			this.rBtnAfcLowBetaOff.AutoSize = true;
			this.rBtnAfcLowBetaOff.Location = new Point(50, 0);
			this.rBtnAfcLowBetaOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAfcLowBetaOff.Name = "rBtnAfcLowBetaOff";
			this.rBtnAfcLowBetaOff.Size = new Size(41, 16);
			this.rBtnAfcLowBetaOff.TabIndex = 1;
			this.rBtnAfcLowBetaOff.Text = "OFF";
			this.rBtnAfcLowBetaOff.UseVisualStyleBackColor = true;
			this.rBtnAfcLowBetaOff.CheckedChanged += new EventHandler(this.rBtnAfcLowBeta_CheckedChanged);
			this.rBtnAfcLowBetaOn.AutoSize = true;
			this.rBtnAfcLowBetaOn.Checked = true;
			this.rBtnAfcLowBetaOn.Location = new Point(3, 0);
			this.rBtnAfcLowBetaOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAfcLowBetaOn.Name = "rBtnAfcLowBetaOn";
			this.rBtnAfcLowBetaOn.Size = new Size(35, 16);
			this.rBtnAfcLowBetaOn.TabIndex = 0;
			this.rBtnAfcLowBetaOn.TabStop = true;
			this.rBtnAfcLowBetaOn.Text = "ON";
			this.rBtnAfcLowBetaOn.UseVisualStyleBackColor = true;
			this.rBtnAfcLowBetaOn.CheckedChanged += new EventHandler(this.rBtnAfcLowBeta_CheckedChanged);
			this.btnFeiRead.Location = new Point(86, 128);
			this.btnFeiRead.Name = "btnFeiRead";
			this.btnFeiRead.Size = new Size(41, 21);
			this.btnFeiRead.TabIndex = 16;
			this.btnFeiRead.Text = "Read";
			this.btnFeiRead.UseVisualStyleBackColor = true;
			this.btnFeiRead.Click += new EventHandler(this.btnFeiStart_Click);
			this.panel8.AutoSize = true;
			this.panel8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel8.Controls.Add((Control)this.rBtnAfcAutoClearOff);
			this.panel8.Controls.Add((Control)this.rBtnAfcAutoClearOn);
			this.panel8.Location = new Point(133, 63);
			this.panel8.Name = "panel8";
			this.panel8.Size = new Size(94, 16);
			this.panel8.TabIndex = 6;
			this.rBtnAfcAutoClearOff.AutoSize = true;
			this.rBtnAfcAutoClearOff.Location = new Point(50, 0);
			this.rBtnAfcAutoClearOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAfcAutoClearOff.Name = "rBtnAfcAutoClearOff";
			this.rBtnAfcAutoClearOff.Size = new Size(41, 16);
			this.rBtnAfcAutoClearOff.TabIndex = 1;
			this.rBtnAfcAutoClearOff.Text = "OFF";
			this.rBtnAfcAutoClearOff.UseVisualStyleBackColor = true;
			this.rBtnAfcAutoClearOff.CheckedChanged += new EventHandler(this.rBtnAfcAutoClearOn_CheckedChanged);
			this.rBtnAfcAutoClearOn.AutoSize = true;
			this.rBtnAfcAutoClearOn.Checked = true;
			this.rBtnAfcAutoClearOn.Location = new Point(3, 0);
			this.rBtnAfcAutoClearOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAfcAutoClearOn.Name = "rBtnAfcAutoClearOn";
			this.rBtnAfcAutoClearOn.Size = new Size(35, 16);
			this.rBtnAfcAutoClearOn.TabIndex = 0;
			this.rBtnAfcAutoClearOn.TabStop = true;
			this.rBtnAfcAutoClearOn.Text = "ON";
			this.rBtnAfcAutoClearOn.UseVisualStyleBackColor = true;
			this.rBtnAfcAutoClearOn.CheckedChanged += new EventHandler(this.rBtnAfcAutoClearOn_CheckedChanged);
			this.ledFeiDone.BackColor = Color.Transparent;
			this.ledFeiDone.LedColor = Color.Green;
			this.ledFeiDone.LedSize = new Size(11, 11);
			this.ledFeiDone.Location = new Point(234, 132);
			this.ledFeiDone.Name = "ledFeiDone";
			this.ledFeiDone.Size = new Size(15, 14);
			this.ledFeiDone.TabIndex = 18;
			this.ledFeiDone.Text = "led1";
			this.panel9.AutoSize = true;
			this.panel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel9.Controls.Add((Control)this.rBtnAfcAutoOff);
			this.panel9.Controls.Add((Control)this.rBtnAfcAutoOn);
			this.panel9.Location = new Point(133, 84);
			this.panel9.Name = "panel9";
			this.panel9.Size = new Size(94, 16);
			this.panel9.TabIndex = 7;
			this.rBtnAfcAutoOff.AutoSize = true;
			this.rBtnAfcAutoOff.Location = new Point(50, 0);
			this.rBtnAfcAutoOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAfcAutoOff.Name = "rBtnAfcAutoOff";
			this.rBtnAfcAutoOff.Size = new Size(41, 16);
			this.rBtnAfcAutoOff.TabIndex = 1;
			this.rBtnAfcAutoOff.Text = "OFF";
			this.rBtnAfcAutoOff.UseVisualStyleBackColor = true;
			this.rBtnAfcAutoOff.CheckedChanged += new EventHandler(this.rBtnAfcAutoOn_CheckedChanged);
			this.rBtnAfcAutoOn.AutoSize = true;
			this.rBtnAfcAutoOn.Checked = true;
			this.rBtnAfcAutoOn.Location = new Point(3, 0);
			this.rBtnAfcAutoOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAfcAutoOn.Name = "rBtnAfcAutoOn";
			this.rBtnAfcAutoOn.Size = new Size(35, 16);
			this.rBtnAfcAutoOn.TabIndex = 0;
			this.rBtnAfcAutoOn.TabStop = true;
			this.rBtnAfcAutoOn.Text = "ON";
			this.rBtnAfcAutoOn.UseVisualStyleBackColor = true;
			this.rBtnAfcAutoOn.CheckedChanged += new EventHandler(this.rBtnAfcAutoOn_CheckedChanged);
			this.lblFeiValue.BackColor = Color.Transparent;
			this.lblFeiValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblFeiValue.Location = new Point(133, 129);
			this.lblFeiValue.Margin = new Padding(3);
			this.lblFeiValue.Name = "lblFeiValue";
			this.lblFeiValue.Size = new Size(98, 18);
			this.lblFeiValue.TabIndex = 17;
			this.lblFeiValue.Text = "0";
			this.lblFeiValue.TextAlign = ContentAlignment.MiddleLeft;
			this.label12.AutoSize = true;
			this.label12.BackColor = Color.Transparent;
			this.label12.Location = new Point(3, 133);
			this.label12.Name = "label12";
			this.label12.Size = new Size(29, 12);
			this.label12.TabIndex = 15;
			this.label12.Text = "FEI:";
			this.label12.TextAlign = ContentAlignment.MiddleCenter;
			this.label18.AutoSize = true;
			this.label18.Location = new Point((int)byte.MaxValue, 109);
			this.label18.Name = "label18";
			this.label18.Size = new Size(17, 12);
			this.label18.TabIndex = 14;
			this.label18.Text = "Hz";
			this.label10.AutoSize = true;
			this.label10.Location = new Point((int)byte.MaxValue, 133);
			this.label10.Name = "label10";
			this.label10.Size = new Size(17, 12);
			this.label10.TabIndex = 19;
			this.label10.Text = "Hz";
			this.btnAfcClear.Location = new Point(86, 104);
			this.btnAfcClear.Name = "btnAfcClear";
			this.btnAfcClear.Size = new Size(41, 21);
			this.btnAfcClear.TabIndex = 11;
			this.btnAfcClear.Text = "Clear";
			this.btnAfcClear.UseVisualStyleBackColor = true;
			this.btnAfcClear.Click += new EventHandler(this.btnAfcClear_Click);
			this.btnAfcStart.Location = new Point(39, 104);
			this.btnAfcStart.Name = "btnAfcStart";
			this.btnAfcStart.Size = new Size(41, 21);
			this.btnAfcStart.TabIndex = 10;
			this.btnAfcStart.Text = "Start";
			this.btnAfcStart.UseVisualStyleBackColor = true;
			this.btnAfcStart.Click += new EventHandler(this.btnAfcStart_Click);
			this.ledAfcDone.BackColor = Color.Transparent;
			this.ledAfcDone.LedColor = Color.Green;
			this.ledAfcDone.LedSize = new Size(11, 11);
			this.ledAfcDone.Location = new Point(234, 108);
			this.ledAfcDone.Name = "ledAfcDone";
			this.ledAfcDone.Size = new Size(15, 14);
			this.ledAfcDone.TabIndex = 13;
			this.ledAfcDone.Text = "led1";
			this.lblAfcValue.BackColor = Color.Transparent;
			this.lblAfcValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAfcValue.Location = new Point(133, 105);
			this.lblAfcValue.Margin = new Padding(3);
			this.lblAfcValue.Name = "lblAfcValue";
			this.lblAfcValue.Size = new Size(98, 18);
			this.lblAfcValue.TabIndex = 12;
			this.lblAfcValue.Text = "0";
			this.lblAfcValue.TextAlign = ContentAlignment.MiddleLeft;
			this.label22.AutoSize = true;
			this.label22.BackColor = Color.Transparent;
			this.label22.Location = new Point(3, 109);
			this.label22.Name = "label22";
			this.label22.Size = new Size(29, 12);
			this.label22.TabIndex = 9;
			this.label22.Text = "AFC:";
			this.label22.TextAlign = ContentAlignment.MiddleCenter;
			this.gBoxOok.Controls.Add((Control)this.cBoxOokThreshType);
			this.gBoxOok.Controls.Add((Control)this.lblOokType);
			this.gBoxOok.Controls.Add((Control)this.lblOokStep);
			this.gBoxOok.Controls.Add((Control)this.lblOokDec);
			this.gBoxOok.Controls.Add((Control)this.lblOokCutoff);
			this.gBoxOok.Controls.Add((Control)this.lblOokFixed);
			this.gBoxOok.Controls.Add((Control)this.suffixOOKstep);
			this.gBoxOok.Controls.Add((Control)this.suffixOOKfixed);
			this.gBoxOok.Controls.Add((Control)this.nudOokPeakThreshStep);
			this.gBoxOok.Controls.Add((Control)this.nudOokFixedThresh);
			this.gBoxOok.Controls.Add((Control)this.cBoxOokPeakThreshDec);
			this.gBoxOok.Controls.Add((Control)this.cBoxOokAverageThreshFilt);
			this.gBoxOok.Location = new Point(3, 182);
			this.gBoxOok.Name = "gBoxOok";
			this.gBoxOok.Size = new Size(285, 162);
			this.gBoxOok.TabIndex = 2;
			this.gBoxOok.TabStop = false;
			this.gBoxOok.Text = "OOK";
			this.gBoxOok.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxOok.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.cBoxOokThreshType.FormattingEnabled = true;
			this.cBoxOokThreshType.Items.AddRange(new object[3]
      {
        (object) "Fixed",
        (object) "Peak",
        (object) "Average"
      });
			this.cBoxOokThreshType.Location = new Point(109, 28);
			this.cBoxOokThreshType.Name = "cBoxOokThreshType";
			this.cBoxOokThreshType.Size = new Size(124, 20);
			this.cBoxOokThreshType.TabIndex = 1;
			this.cBoxOokThreshType.SelectedIndexChanged += new EventHandler(this.cBoxOokThreshType_SelectedIndexChanged);
			this.lblOokType.AutoSize = true;
			this.lblOokType.Location = new Point(6, 31);
			this.lblOokType.Name = "lblOokType";
			this.lblOokType.Size = new Size(95, 12);
			this.lblOokType.TabIndex = 0;
			this.lblOokType.Text = "Threshold type:";
			this.lblOokStep.AutoSize = true;
			this.lblOokStep.Location = new Point(6, 55);
			this.lblOokStep.Name = "lblOokStep";
			this.lblOokStep.Size = new Size(125, 12);
			this.lblOokStep.TabIndex = 2;
			this.lblOokStep.Text = "Peak threshold step:";
			this.lblOokDec.AutoSize = true;
			this.lblOokDec.Location = new Point(6, 79);
			this.lblOokDec.Name = "lblOokDec";
			this.lblOokDec.Size = new Size(119, 12);
			this.lblOokDec.TabIndex = 5;
			this.lblOokDec.Text = "Peak threshold dec:";
			this.lblOokCutoff.AutoSize = true;
			this.lblOokCutoff.Location = new Point(6, 103);
			this.lblOokCutoff.Name = "lblOokCutoff";
			this.lblOokCutoff.Size = new Size(131, 12);
			this.lblOokCutoff.TabIndex = 7;
			this.lblOokCutoff.Text = "Avg threshold cutoff:";
			this.lblOokFixed.AutoSize = true;
			this.lblOokFixed.Location = new Point(6, (int)sbyte.MaxValue);
			this.lblOokFixed.Name = "lblOokFixed";
			this.lblOokFixed.Size = new Size(101, 12);
			this.lblOokFixed.TabIndex = 9;
			this.lblOokFixed.Text = "Fixed threshold:";
			this.suffixOOKstep.AutoSize = true;
			this.suffixOOKstep.BackColor = Color.Transparent;
			this.suffixOOKstep.Location = new Point(239, 55);
			this.suffixOOKstep.Name = "suffixOOKstep";
			this.suffixOOKstep.Size = new Size(17, 12);
			this.suffixOOKstep.TabIndex = 4;
			this.suffixOOKstep.Text = "dB";
			this.suffixOOKfixed.AutoSize = true;
			this.suffixOOKfixed.BackColor = Color.Transparent;
			this.suffixOOKfixed.Location = new Point(239, (int)sbyte.MaxValue);
			this.suffixOOKfixed.Name = "suffixOOKfixed";
			this.suffixOOKfixed.Size = new Size(17, 12);
			this.suffixOOKfixed.TabIndex = 11;
			this.suffixOOKfixed.Text = "dB";
			this.nudOokPeakThreshStep.DecimalPlaces = 1;
			this.nudOokPeakThreshStep.Increment = new Decimal(new int[4]
      {
        5,
        0,
        0,
        65536
      });
			this.nudOokPeakThreshStep.Location = new Point(109, 52);
			this.nudOokPeakThreshStep.Maximum = new Decimal(new int[4]
      {
        60,
        0,
        0,
        65536
      });
			this.nudOokPeakThreshStep.Minimum = new Decimal(new int[4]
      {
        5,
        0,
        0,
        65536
      });
			this.nudOokPeakThreshStep.Name = "nudOokPeakThreshStep";
			this.nudOokPeakThreshStep.Size = new Size(124, 21);
			this.nudOokPeakThreshStep.TabIndex = 3;
			this.nudOokPeakThreshStep.ThousandsSeparator = true;
			this.nudOokPeakThreshStep.Value = new Decimal(new int[4]
      {
        5,
        0,
        0,
        65536
      });
			this.nudOokPeakThreshStep.ValueChanged += new EventHandler(this.nudOokPeakThreshStep_ValueChanged);
			this.nudOokPeakThreshStep.Validating += new CancelEventHandler(this.nudOokPeakThreshStep_Validating);
			this.nudOokFixedThresh.Location = new Point(109, 124);
			int[] bits20 = new int[4];
			bits20[0] = (int)byte.MaxValue;
			Decimal num20 = new Decimal(bits20);
			this.nudOokFixedThresh.Maximum = num20;
			this.nudOokFixedThresh.Name = "nudOokFixedThresh";
			this.nudOokFixedThresh.Size = new Size(124, 21);
			this.nudOokFixedThresh.TabIndex = 10;
			this.nudOokFixedThresh.ThousandsSeparator = true;
			int[] bits21 = new int[4];
			bits21[0] = 6;
			Decimal num21 = new Decimal(bits21);
			this.nudOokFixedThresh.Value = num21;
			this.nudOokFixedThresh.ValueChanged += new EventHandler(this.nudOokFixedThresh_ValueChanged);
			this.cBoxOokPeakThreshDec.FormattingEnabled = true;
			this.cBoxOokPeakThreshDec.Items.AddRange(new object[8]
      {
        (object) "Once per chip",
        (object) "Once every 2 chips",
        (object) "Once every 4 chips",
        (object) "Once every 8 chips",
        (object) "2 times per chip",
        (object) "4 times per chip",
        (object) "8 times per chip",
        (object) "16 times per chip"
      });
			this.cBoxOokPeakThreshDec.Location = new Point(109, 76);
			this.cBoxOokPeakThreshDec.Name = "cBoxOokPeakThreshDec";
			this.cBoxOokPeakThreshDec.Size = new Size(124, 20);
			this.cBoxOokPeakThreshDec.TabIndex = 6;
			this.cBoxOokPeakThreshDec.SelectedIndexChanged += new EventHandler(this.cBoxOokPeakThreshDec_SelectedIndexChanged);
			this.cBoxOokAverageThreshFilt.FormattingEnabled = true;
			this.cBoxOokAverageThreshFilt.Items.AddRange(new object[4]
      {
        (object) "Bitrate / 32π",
        (object) "Bitrate / 8π",
        (object) "Bitrate / 4π",
        (object) "Bitrate / 2π"
      });
			this.cBoxOokAverageThreshFilt.Location = new Point(109, 100);
			this.cBoxOokAverageThreshFilt.Name = "cBoxOokAverageThreshFilt";
			this.cBoxOokAverageThreshFilt.Size = new Size(124, 20);
			this.cBoxOokAverageThreshFilt.TabIndex = 8;
			this.cBoxOokAverageThreshFilt.SelectedIndexChanged += new EventHandler(this.cBoxOokAverageThreshFilt_SelectedIndexChanged);
			this.gBoxAfcBw.Controls.Add((Control)this.nudAfcDccFreq);
			this.gBoxAfcBw.Controls.Add((Control)this.lblAfcDcc);
			this.gBoxAfcBw.Controls.Add((Control)this.lblAfcRxBw);
			this.gBoxAfcBw.Controls.Add((Control)this.suffixAFCDCC);
			this.gBoxAfcBw.Controls.Add((Control)this.suffixAFCRxBw);
			this.gBoxAfcBw.Controls.Add((Control)this.nudRxFilterBwAfc);
			this.gBoxAfcBw.Location = new Point(3, 92);
			this.gBoxAfcBw.Name = "gBoxAfcBw";
			this.gBoxAfcBw.Size = new Size(285, 84);
			this.gBoxAfcBw.TabIndex = 1;
			this.gBoxAfcBw.TabStop = false;
			this.gBoxAfcBw.Text = "AFC bandwidth";
			this.gBoxAfcBw.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxAfcBw.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudAfcDccFreq.Location = new Point(109, 26);
			int[] bits22 = new int[4];
			bits22[0] = 1657;
			Decimal num22 = new Decimal(bits22);
			this.nudAfcDccFreq.Maximum = num22;
			int[] bits23 = new int[4];
			bits23[0] = 12;
			Decimal num23 = new Decimal(bits23);
			this.nudAfcDccFreq.Minimum = num23;
			this.nudAfcDccFreq.Name = "nudAfcDccFreq";
			this.nudAfcDccFreq.Size = new Size(124, 21);
			this.nudAfcDccFreq.TabIndex = 1;
			this.nudAfcDccFreq.ThousandsSeparator = true;
			int[] bits24 = new int[4];
			bits24[0] = 497;
			Decimal num24 = new Decimal(bits24);
			this.nudAfcDccFreq.Value = num24;
			this.nudAfcDccFreq.ValueChanged += new EventHandler(this.nudAfcDccFreq_ValueChanged);
			this.lblAfcDcc.AutoSize = true;
			this.lblAfcDcc.Location = new Point(6, 28);
			this.lblAfcDcc.Name = "lblAfcDcc";
			this.lblAfcDcc.Size = new Size(89, 12);
			this.lblAfcDcc.TabIndex = 0;
			this.lblAfcDcc.Text = "DCC frequency:";
			this.lblAfcRxBw.AutoSize = true;
			this.lblAfcRxBw.Location = new Point(6, 53);
			this.lblAfcRxBw.Name = "lblAfcRxBw";
			this.lblAfcRxBw.Size = new Size(125, 12);
			this.lblAfcRxBw.TabIndex = 3;
			this.lblAfcRxBw.Text = "Rx filter bandwidth:";
			this.suffixAFCDCC.AutoSize = true;
			this.suffixAFCDCC.Location = new Point(239, 30);
			this.suffixAFCDCC.Name = "suffixAFCDCC";
			this.suffixAFCDCC.Size = new Size(17, 12);
			this.suffixAFCDCC.TabIndex = 2;
			this.suffixAFCDCC.Text = "Hz";
			this.suffixAFCRxBw.AutoSize = true;
			this.suffixAFCRxBw.Location = new Point(239, 54);
			this.suffixAFCRxBw.Name = "suffixAFCRxBw";
			this.suffixAFCRxBw.Size = new Size(17, 12);
			this.suffixAFCRxBw.TabIndex = 5;
			this.suffixAFCRxBw.Text = "Hz";
			this.nudRxFilterBwAfc.Location = new Point(109, 51);
			int[] bits25 = new int[4];
			bits25[0] = 400000;
			Decimal num25 = new Decimal(bits25);
			this.nudRxFilterBwAfc.Maximum = num25;
			int[] bits26 = new int[4];
			bits26[0] = 3125;
			Decimal num26 = new Decimal(bits26);
			this.nudRxFilterBwAfc.Minimum = num26;
			this.nudRxFilterBwAfc.Name = "nudRxFilterBwAfc";
			this.nudRxFilterBwAfc.Size = new Size(124, 21);
			this.nudRxFilterBwAfc.TabIndex = 4;
			this.nudRxFilterBwAfc.ThousandsSeparator = true;
			int[] bits27 = new int[4];
			bits27[0] = 50000;
			Decimal num27 = new Decimal(bits27);
			this.nudRxFilterBwAfc.Value = num27;
			this.nudRxFilterBwAfc.ValueChanged += new EventHandler(this.nudRxFilterBwAfc_ValueChanged);
			this.gBoxRxBw.Controls.Add((Control)this.nudDccFreq);
			this.gBoxRxBw.Controls.Add((Control)this.lblDcc);
			this.gBoxRxBw.Controls.Add((Control)this.lblRxBw);
			this.gBoxRxBw.Controls.Add((Control)this.suffixDCC);
			this.gBoxRxBw.Controls.Add((Control)this.suffixRxBw);
			this.gBoxRxBw.Controls.Add((Control)this.nudRxFilterBw);
			this.gBoxRxBw.Location = new Point(3, 3);
			this.gBoxRxBw.Name = "gBoxRxBw";
			this.gBoxRxBw.Size = new Size(285, 84);
			this.gBoxRxBw.TabIndex = 0;
			this.gBoxRxBw.TabStop = false;
			this.gBoxRxBw.Text = "Rx bandwidth";
			this.gBoxRxBw.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxRxBw.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudDccFreq.Location = new Point(109, 28);
			int[] bits28 = new int[4];
			bits28[0] = 1657;
			Decimal num28 = new Decimal(bits28);
			this.nudDccFreq.Maximum = num28;
			int[] bits29 = new int[4];
			bits29[0] = 12;
			Decimal num29 = new Decimal(bits29);
			this.nudDccFreq.Minimum = num29;
			this.nudDccFreq.Name = "nudDccFreq";
			this.nudDccFreq.Size = new Size(124, 21);
			this.nudDccFreq.TabIndex = 1;
			this.nudDccFreq.ThousandsSeparator = true;
			int[] bits30 = new int[4];
			bits30[0] = 414;
			Decimal num30 = new Decimal(bits30);
			this.nudDccFreq.Value = num30;
			this.nudDccFreq.ValueChanged += new EventHandler(this.nudDccFreq_ValueChanged);
			this.lblDcc.AutoSize = true;
			this.lblDcc.Location = new Point(6, 29);
			this.lblDcc.Name = "lblDcc";
			this.lblDcc.Size = new Size(89, 12);
			this.lblDcc.TabIndex = 0;
			this.lblDcc.Text = "DCC frequency:";
			this.lblRxBw.AutoSize = true;
			this.lblRxBw.Location = new Point(6, 53);
			this.lblRxBw.Name = "lblRxBw";
			this.lblRxBw.Size = new Size(125, 12);
			this.lblRxBw.TabIndex = 3;
			this.lblRxBw.Text = "Rx filter bandwidth:";
			this.suffixDCC.AutoSize = true;
			this.suffixDCC.Location = new Point(239, 31);
			this.suffixDCC.Name = "suffixDCC";
			this.suffixDCC.Size = new Size(17, 12);
			this.suffixDCC.TabIndex = 2;
			this.suffixDCC.Text = "Hz";
			this.suffixRxBw.AutoSize = true;
			this.suffixRxBw.Location = new Point(239, 54);
			this.suffixRxBw.Name = "suffixRxBw";
			this.suffixRxBw.Size = new Size(17, 12);
			this.suffixRxBw.TabIndex = 5;
			this.suffixRxBw.Text = "Hz";
			this.gBoxLna.Controls.Add((Control)this.panel5);
			this.gBoxLna.Controls.Add((Control)this.label13);
			this.gBoxLna.Controls.Add((Control)this.label16);
			this.gBoxLna.Controls.Add((Control)this.lblAgcReference);
			this.gBoxLna.Controls.Add((Control)this.label48);
			this.gBoxLna.Controls.Add((Control)this.label49);
			this.gBoxLna.Controls.Add((Control)this.label50);
			this.gBoxLna.Controls.Add((Control)this.label51);
			this.gBoxLna.Controls.Add((Control)this.label52);
			this.gBoxLna.Controls.Add((Control)this.lblLnaGain1);
			this.gBoxLna.Controls.Add((Control)this.label53);
			this.gBoxLna.Controls.Add((Control)this.panel6);
			this.gBoxLna.Controls.Add((Control)this.lblLnaGain2);
			this.gBoxLna.Controls.Add((Control)this.lblLnaGain3);
			this.gBoxLna.Controls.Add((Control)this.lblLnaGain4);
			this.gBoxLna.Controls.Add((Control)this.lblLnaGain5);
			this.gBoxLna.Controls.Add((Control)this.lblLnaGain6);
			this.gBoxLna.Controls.Add((Control)this.lblAgcThresh1);
			this.gBoxLna.Controls.Add((Control)this.lblAgcThresh2);
			this.gBoxLna.Controls.Add((Control)this.lblAgcThresh3);
			this.gBoxLna.Controls.Add((Control)this.lblAgcThresh4);
			this.gBoxLna.Controls.Add((Control)this.lblAgcThresh5);
			this.gBoxLna.Controls.Add((Control)this.label47);
			this.gBoxLna.Location = new Point(3, 349);
			this.gBoxLna.Name = "gBoxLna";
			this.gBoxLna.Size = new Size(793, 103);
			this.gBoxLna.TabIndex = 7;
			this.gBoxLna.TabStop = false;
			this.gBoxLna.Text = "Lna gain";
			this.gBoxLna.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxLna.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.panel5.AutoSize = true;
			this.panel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel5.Controls.Add((Control)this.rBtnLnaGainAutoOff);
			this.panel5.Controls.Add((Control)this.rBtnLnaGainAutoOn);
			this.panel5.Location = new Point(54, 75);
			this.panel5.Name = "panel5";
			this.panel5.Size = new Size(118, 16);
			this.panel5.TabIndex = 21;
			this.rBtnLnaGainAutoOff.AutoSize = true;
			this.rBtnLnaGainAutoOff.Location = new Point(56, 0);
			this.rBtnLnaGainAutoOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaGainAutoOff.Name = "rBtnLnaGainAutoOff";
			this.rBtnLnaGainAutoOff.Size = new Size(59, 16);
			this.rBtnLnaGainAutoOff.TabIndex = 1;
			this.rBtnLnaGainAutoOff.Text = "Manual";
			this.rBtnLnaGainAutoOff.UseVisualStyleBackColor = true;
			this.rBtnLnaGainAutoOff.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			this.rBtnLnaGainAutoOn.AutoSize = true;
			this.rBtnLnaGainAutoOn.Checked = true;
			this.rBtnLnaGainAutoOn.Location = new Point(3, 0);
			this.rBtnLnaGainAutoOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaGainAutoOn.Name = "rBtnLnaGainAutoOn";
			this.rBtnLnaGainAutoOn.Size = new Size(47, 16);
			this.rBtnLnaGainAutoOn.TabIndex = 0;
			this.rBtnLnaGainAutoOn.TabStop = true;
			this.rBtnLnaGainAutoOn.Text = "Auto";
			this.rBtnLnaGainAutoOn.UseVisualStyleBackColor = true;
			this.rBtnLnaGainAutoOn.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			this.label13.BackColor = Color.Transparent;
			this.label13.Location = new Point(79, 37);
			this.label13.Name = "label13";
			this.label13.Size = new Size(42, 12);
			this.label13.TabIndex = 6;
			this.label13.Text = "AGC";
			this.label13.TextAlign = ContentAlignment.MiddleCenter;
			this.label16.BackColor = Color.Transparent;
			this.label16.Location = new Point(19, 75);
			this.label16.Margin = new Padding(0, 0, 0, 3);
			this.label16.Name = "label16";
			this.label16.Size = new Size(32, 16);
			this.label16.TabIndex = 20;
			this.label16.Text = "Gain:";
			this.label16.TextAlign = ContentAlignment.MiddleLeft;
			this.lblAgcReference.BackColor = Color.Transparent;
			this.lblAgcReference.Location = new Point(110, 37);
			this.lblAgcReference.Margin = new Padding(0, 0, 0, 3);
			this.lblAgcReference.Name = "lblAgcReference";
			this.lblAgcReference.Size = new Size(100, 12);
			this.lblAgcReference.TabIndex = 7;
			this.lblAgcReference.Text = "-80";
			this.lblAgcReference.TextAlign = ContentAlignment.MiddleCenter;
			this.label48.BackColor = Color.Transparent;
			this.label48.Location = new Point(110, 22);
			this.label48.Margin = new Padding(0, 0, 0, 3);
			this.label48.Name = "label48";
			this.label48.Size = new Size(100, 12);
			this.label48.TabIndex = 0;
			this.label48.Text = "Reference";
			this.label48.TextAlign = ContentAlignment.MiddleCenter;
			this.label49.BackColor = Color.Transparent;
			this.label49.Location = new Point(210, 22);
			this.label49.Margin = new Padding(0, 0, 0, 3);
			this.label49.Name = "label49";
			this.label49.Size = new Size(100, 12);
			this.label49.TabIndex = 1;
			this.label49.Text = "Threshold 1";
			this.label49.TextAlign = ContentAlignment.MiddleCenter;
			this.label50.BackColor = Color.Transparent;
			this.label50.Location = new Point(310, 22);
			this.label50.Margin = new Padding(0, 0, 0, 3);
			this.label50.Name = "label50";
			this.label50.Size = new Size(100, 12);
			this.label50.TabIndex = 2;
			this.label50.Text = "Threshold 2";
			this.label50.TextAlign = ContentAlignment.MiddleCenter;
			this.label51.BackColor = Color.Transparent;
			this.label51.Location = new Point(410, 22);
			this.label51.Margin = new Padding(0, 0, 0, 3);
			this.label51.Name = "label51";
			this.label51.Size = new Size(100, 12);
			this.label51.TabIndex = 3;
			this.label51.Text = "Threshold 3";
			this.label51.TextAlign = ContentAlignment.MiddleCenter;
			this.label52.BackColor = Color.Transparent;
			this.label52.Location = new Point(510, 22);
			this.label52.Margin = new Padding(0, 0, 0, 3);
			this.label52.Name = "label52";
			this.label52.Size = new Size(100, 12);
			this.label52.TabIndex = 4;
			this.label52.Text = "Threshold 4";
			this.label52.TextAlign = ContentAlignment.MiddleCenter;
			this.lblLnaGain1.BackColor = Color.LightSteelBlue;
			this.lblLnaGain1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLnaGain1.Location = new Point(160, 52);
			this.lblLnaGain1.Margin = new Padding(0, 0, 0, 3);
			this.lblLnaGain1.Name = "lblLnaGain1";
			this.lblLnaGain1.Size = new Size(100, 18);
			this.lblLnaGain1.TabIndex = 14;
			this.lblLnaGain1.Text = "G1";
			this.lblLnaGain1.TextAlign = ContentAlignment.MiddleCenter;
			this.label53.BackColor = Color.Transparent;
			this.label53.Location = new Point(610, 22);
			this.label53.Margin = new Padding(0, 0, 0, 3);
			this.label53.Name = "label53";
			this.label53.Size = new Size(100, 12);
			this.label53.TabIndex = 5;
			this.label53.Text = "Threshold 5";
			this.label53.TextAlign = ContentAlignment.MiddleCenter;
			this.panel6.AutoSize = true;
			this.panel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel6.Controls.Add((Control)this.rBtnLnaGain1);
			this.panel6.Controls.Add((Control)this.rBtnLnaGain2);
			this.panel6.Controls.Add((Control)this.rBtnLnaGain3);
			this.panel6.Controls.Add((Control)this.rBtnLnaGain4);
			this.panel6.Controls.Add((Control)this.rBtnLnaGain5);
			this.panel6.Controls.Add((Control)this.rBtnLnaGain6);
			this.panel6.Location = new Point(201, 77);
			this.panel6.Name = "panel6";
			this.panel6.Size = new Size(521, 13);
			this.panel6.TabIndex = 22;
			this.rBtnLnaGain1.AutoSize = true;
			this.rBtnLnaGain1.Checked = true;
			this.rBtnLnaGain1.Location = new Point(3, 0);
			this.rBtnLnaGain1.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaGain1.Name = "rBtnLnaGain1";
			this.rBtnLnaGain1.Size = new Size(14, 13);
			this.rBtnLnaGain1.TabIndex = 0;
			this.rBtnLnaGain1.TabStop = true;
			this.rBtnLnaGain1.UseVisualStyleBackColor = true;
			this.rBtnLnaGain1.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			this.rBtnLnaGain2.AutoSize = true;
			this.rBtnLnaGain2.Location = new Point(102, 0);
			this.rBtnLnaGain2.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaGain2.Name = "rBtnLnaGain2";
			this.rBtnLnaGain2.Size = new Size(14, 13);
			this.rBtnLnaGain2.TabIndex = 1;
			this.rBtnLnaGain2.UseVisualStyleBackColor = true;
			this.rBtnLnaGain2.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			this.rBtnLnaGain3.AutoSize = true;
			this.rBtnLnaGain3.Location = new Point(203, 0);
			this.rBtnLnaGain3.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaGain3.Name = "rBtnLnaGain3";
			this.rBtnLnaGain3.Size = new Size(14, 13);
			this.rBtnLnaGain3.TabIndex = 2;
			this.rBtnLnaGain3.UseVisualStyleBackColor = true;
			this.rBtnLnaGain3.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			this.rBtnLnaGain4.AutoSize = true;
			this.rBtnLnaGain4.Location = new Point(303, 0);
			this.rBtnLnaGain4.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaGain4.Name = "rBtnLnaGain4";
			this.rBtnLnaGain4.Size = new Size(14, 13);
			this.rBtnLnaGain4.TabIndex = 3;
			this.rBtnLnaGain4.UseVisualStyleBackColor = true;
			this.rBtnLnaGain4.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			this.rBtnLnaGain5.AutoSize = true;
			this.rBtnLnaGain5.Location = new Point(404, 0);
			this.rBtnLnaGain5.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaGain5.Name = "rBtnLnaGain5";
			this.rBtnLnaGain5.Size = new Size(14, 13);
			this.rBtnLnaGain5.TabIndex = 4;
			this.rBtnLnaGain5.UseVisualStyleBackColor = true;
			this.rBtnLnaGain5.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			this.rBtnLnaGain6.AutoSize = true;
			this.rBtnLnaGain6.Location = new Point(504, 0);
			this.rBtnLnaGain6.Margin = new Padding(3, 0, 3, 0);
			this.rBtnLnaGain6.Name = "rBtnLnaGain6";
			this.rBtnLnaGain6.Size = new Size(14, 13);
			this.rBtnLnaGain6.TabIndex = 5;
			this.rBtnLnaGain6.UseVisualStyleBackColor = true;
			this.rBtnLnaGain6.CheckedChanged += new EventHandler(this.rBtnLnaGain_CheckedChanged);
			this.lblLnaGain2.BackColor = Color.Transparent;
			this.lblLnaGain2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLnaGain2.Location = new Point(260, 52);
			this.lblLnaGain2.Margin = new Padding(0, 0, 0, 3);
			this.lblLnaGain2.Name = "lblLnaGain2";
			this.lblLnaGain2.Size = new Size(100, 18);
			this.lblLnaGain2.TabIndex = 15;
			this.lblLnaGain2.Text = "G2";
			this.lblLnaGain2.TextAlign = ContentAlignment.MiddleCenter;
			this.lblLnaGain3.BackColor = Color.Transparent;
			this.lblLnaGain3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLnaGain3.Location = new Point(360, 52);
			this.lblLnaGain3.Margin = new Padding(0, 0, 0, 3);
			this.lblLnaGain3.Name = "lblLnaGain3";
			this.lblLnaGain3.Size = new Size(100, 18);
			this.lblLnaGain3.TabIndex = 16;
			this.lblLnaGain3.Text = "G3";
			this.lblLnaGain3.TextAlign = ContentAlignment.MiddleCenter;
			this.lblLnaGain4.BackColor = Color.Transparent;
			this.lblLnaGain4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLnaGain4.Location = new Point(460, 52);
			this.lblLnaGain4.Margin = new Padding(0, 0, 0, 3);
			this.lblLnaGain4.Name = "lblLnaGain4";
			this.lblLnaGain4.Size = new Size(100, 18);
			this.lblLnaGain4.TabIndex = 17;
			this.lblLnaGain4.Text = "G4";
			this.lblLnaGain4.TextAlign = ContentAlignment.MiddleCenter;
			this.lblLnaGain5.BackColor = Color.Transparent;
			this.lblLnaGain5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLnaGain5.Location = new Point(560, 52);
			this.lblLnaGain5.Margin = new Padding(0, 0, 0, 3);
			this.lblLnaGain5.Name = "lblLnaGain5";
			this.lblLnaGain5.Size = new Size(100, 18);
			this.lblLnaGain5.TabIndex = 18;
			this.lblLnaGain5.Text = "G5";
			this.lblLnaGain5.TextAlign = ContentAlignment.MiddleCenter;
			this.lblLnaGain6.BackColor = Color.Transparent;
			this.lblLnaGain6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLnaGain6.Location = new Point(660, 52);
			this.lblLnaGain6.Margin = new Padding(0, 0, 0, 3);
			this.lblLnaGain6.Name = "lblLnaGain6";
			this.lblLnaGain6.Size = new Size(100, 18);
			this.lblLnaGain6.TabIndex = 19;
			this.lblLnaGain6.Text = "G6";
			this.lblLnaGain6.TextAlign = ContentAlignment.MiddleCenter;
			this.lblAgcThresh1.BackColor = Color.Transparent;
			this.lblAgcThresh1.Location = new Point(210, 37);
			this.lblAgcThresh1.Margin = new Padding(0, 0, 0, 3);
			this.lblAgcThresh1.Name = "lblAgcThresh1";
			this.lblAgcThresh1.Size = new Size(100, 12);
			this.lblAgcThresh1.TabIndex = 8;
			this.lblAgcThresh1.Text = "0";
			this.lblAgcThresh1.TextAlign = ContentAlignment.MiddleCenter;
			this.lblAgcThresh2.BackColor = Color.Transparent;
			this.lblAgcThresh2.Location = new Point(310, 37);
			this.lblAgcThresh2.Margin = new Padding(0, 0, 0, 3);
			this.lblAgcThresh2.Name = "lblAgcThresh2";
			this.lblAgcThresh2.Size = new Size(100, 12);
			this.lblAgcThresh2.TabIndex = 9;
			this.lblAgcThresh2.Text = "0";
			this.lblAgcThresh2.TextAlign = ContentAlignment.MiddleCenter;
			this.lblAgcThresh3.BackColor = Color.Transparent;
			this.lblAgcThresh3.Location = new Point(410, 37);
			this.lblAgcThresh3.Margin = new Padding(0, 0, 0, 3);
			this.lblAgcThresh3.Name = "lblAgcThresh3";
			this.lblAgcThresh3.Size = new Size(100, 12);
			this.lblAgcThresh3.TabIndex = 10;
			this.lblAgcThresh3.Text = "0";
			this.lblAgcThresh3.TextAlign = ContentAlignment.MiddleCenter;
			this.lblAgcThresh4.BackColor = Color.Transparent;
			this.lblAgcThresh4.Location = new Point(510, 37);
			this.lblAgcThresh4.Margin = new Padding(0, 0, 0, 3);
			this.lblAgcThresh4.Name = "lblAgcThresh4";
			this.lblAgcThresh4.Size = new Size(100, 12);
			this.lblAgcThresh4.TabIndex = 11;
			this.lblAgcThresh4.Text = "0";
			this.lblAgcThresh4.TextAlign = ContentAlignment.MiddleCenter;
			this.lblAgcThresh5.BackColor = Color.Transparent;
			this.lblAgcThresh5.Location = new Point(610, 37);
			this.lblAgcThresh5.Margin = new Padding(0, 0, 0, 3);
			this.lblAgcThresh5.Name = "lblAgcThresh5";
			this.lblAgcThresh5.Size = new Size(100, 12);
			this.lblAgcThresh5.TabIndex = 12;
			this.lblAgcThresh5.Text = "0";
			this.lblAgcThresh5.TextAlign = ContentAlignment.MiddleCenter;
			this.label47.AutoSize = true;
			this.label47.BackColor = Color.Transparent;
			this.label47.Location = new Point(709, 37);
			this.label47.Margin = new Padding(0);
			this.label47.Name = "label47";
			this.label47.Size = new Size(77, 12);
			this.label47.TabIndex = 13;
			this.label47.Text = "-> Pin [dBm]";
			this.label47.TextAlign = ContentAlignment.MiddleLeft;
			this.AutoScaleDimensions = new SizeF(6f, 12f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add((Control)this.gBoxDagc);
			this.Controls.Add((Control)this.gBoxLnaSensitivity);
			this.Controls.Add((Control)this.gBoxAgc);
			this.Controls.Add((Control)this.gBoxRssi);
			this.Controls.Add((Control)this.gBoxAfcFei);
			this.Controls.Add((Control)this.gBoxOok);
			this.Controls.Add((Control)this.gBoxAfcBw);
			this.Controls.Add((Control)this.gBoxRxBw);
			this.Controls.Add((Control)this.gBoxLna);
			this.Name = "ReceiverViewControl";
			this.Size = new Size(799, 455);
			this.nudRxFilterBw.EndInit();
			this.gBoxDagc.ResumeLayout(false);
			this.gBoxDagc.PerformLayout();
			this.panel11.ResumeLayout(false);
			this.panel11.PerformLayout();
			this.gBoxLnaSensitivity.ResumeLayout(false);
			this.gBoxLnaSensitivity.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.pnlSensitivityBoost.ResumeLayout(false);
			this.pnlSensitivityBoost.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.gBoxAgc.ResumeLayout(false);
			this.gBoxAgc.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.nudAgcStep5.EndInit();
			this.nudAgcSnrMargin.EndInit();
			this.nudAgcStep4.EndInit();
			this.nudAgcRefLevel.EndInit();
			this.nudAgcStep3.EndInit();
			this.nudAgcStep1.EndInit();
			this.nudAgcStep2.EndInit();
			this.gBoxRssi.ResumeLayout(false);
			this.gBoxRssi.PerformLayout();
			this.pnlRssiPhase.ResumeLayout(false);
			this.pnlRssiPhase.PerformLayout();
			this.panel7.ResumeLayout(false);
			this.panel7.PerformLayout();
			this.nudRssiThresh.EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.nudTimeoutRxStart.EndInit();
			this.nudTimeoutRssiThresh.EndInit();
			this.gBoxAfcFei.ResumeLayout(false);
			this.gBoxAfcFei.PerformLayout();
			this.nudLowBetaAfcOffset.EndInit();
			this.pnlAfcLowBeta.ResumeLayout(false);
			this.pnlAfcLowBeta.PerformLayout();
			this.panel8.ResumeLayout(false);
			this.panel8.PerformLayout();
			this.panel9.ResumeLayout(false);
			this.panel9.PerformLayout();
			this.gBoxOok.ResumeLayout(false);
			this.gBoxOok.PerformLayout();
			this.nudOokPeakThreshStep.EndInit();
			this.nudOokFixedThresh.EndInit();
			this.gBoxAfcBw.ResumeLayout(false);
			this.gBoxAfcBw.PerformLayout();
			this.nudAfcDccFreq.EndInit();
			this.nudRxFilterBwAfc.EndInit();
			this.gBoxRxBw.ResumeLayout(false);
			this.gBoxRxBw.PerformLayout();
			this.nudDccFreq.EndInit();
			this.gBoxLna.ResumeLayout(false);
			this.gBoxLna.PerformLayout();
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			this.panel6.ResumeLayout(false);
			this.panel6.PerformLayout();
			this.ResumeLayout(false);
		}

		private void OnAfcLowBetaOnChanged(bool value)
		{
			if (this.AfcLowBetaOnChanged == null)
				return;
			this.AfcLowBetaOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnLowBetaAfcOffsetChanged(Decimal value)
		{
			if (this.LowBetaAfcOffsetChanged == null)
				return;
			this.LowBetaAfcOffsetChanged((object)this, new DecimalEventArg(value));
		}

		private void OnSensitivityBoostOnChanged(bool value)
		{
			if (this.SensitivityBoostOnChanged == null)
				return;
			this.SensitivityBoostOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnRssiAutoThreshChanged(bool value)
		{
			if (this.RssiAutoThreshChanged == null)
				return;
			this.RssiAutoThreshChanged((object)this, new BooleanEventArg(value));
		}

		private void OnDagcOnChanged(bool value)
		{
			if (this.DagcOnChanged == null)
				return;
			this.DagcOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnAgcAutoRefChanged(bool value)
		{
			if (this.AgcAutoRefChanged == null)
				return;
			this.AgcAutoRefChanged((object)this, new BooleanEventArg(value));
		}

		private void OnAgcSnrMarginChanged(byte value)
		{
			if (this.AgcSnrMarginChanged == null)
				return;
			this.AgcSnrMarginChanged((object)this, new ByteEventArg(value));
		}

		private void OnAgcRefLevelChanged(int value)
		{
			if (this.AgcRefLevelChanged == null)
				return;
			this.AgcRefLevelChanged((object)this, new Int32EventArg(value));
		}

		private void OnAgcStepChanged(byte id, byte value)
		{
			if (this.AgcStepChanged == null)
				return;
			this.AgcStepChanged((object)this, new AgcStepEventArg(id, value));
		}

		private void OnLnaZinChanged(LnaZinEnum value)
		{
			if (this.LnaZinChanged == null)
				return;
			this.LnaZinChanged((object)this, new LnaZinEventArg(value));
		}

		private void OnLnaLowPowerOnChanged(bool value)
		{
			if (this.LnaLowPowerOnChanged == null)
				return;
			this.LnaLowPowerOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnLnaGainChanged(LnaGainEnum value)
		{
			if (this.LnaGainChanged == null)
				return;
			this.LnaGainChanged((object)this, new LnaGainEventArg(value));
		}

		private void OnDccFreqChanged(Decimal value)
		{
			if (this.DccFreqChanged == null)
				return;
			this.DccFreqChanged((object)this, new DecimalEventArg(value));
		}

		private void OnRxBwChanged(Decimal value)
		{
			if (this.RxBwChanged == null)
				return;
			this.RxBwChanged((object)this, new DecimalEventArg(value));
		}

		private void OnAfcDccFreqChanged(Decimal value)
		{
			if (this.AfcDccFreqChanged == null)
				return;
			this.AfcDccFreqChanged((object)this, new DecimalEventArg(value));
		}

		private void OnAfcRxBwChanged(Decimal value)
		{
			if (this.AfcRxBwChanged == null)
				return;
			this.AfcRxBwChanged((object)this, new DecimalEventArg(value));
		}

		private void OnOokThreshTypeChanged(OokThreshTypeEnum value)
		{
			if (this.OokThreshTypeChanged == null)
				return;
			this.OokThreshTypeChanged((object)this, new OokThreshTypeEventArg(value));
		}

		private void OnOokPeakThreshStepChanged(Decimal value)
		{
			if (this.OokPeakThreshStepChanged == null)
				return;
			this.OokPeakThreshStepChanged((object)this, new DecimalEventArg(value));
		}

		private void OnOokPeakThreshDecChanged(OokPeakThreshDecEnum value)
		{
			if (this.OokPeakThreshDecChanged == null)
				return;
			this.OokPeakThreshDecChanged((object)this, new OokPeakThreshDecEventArg(value));
		}

		private void OnOokAverageThreshFiltChanged(OokAverageThreshFiltEnum value)
		{
			if (this.OokAverageThreshFiltChanged == null)
				return;
			this.OokAverageThreshFiltChanged((object)this, new OokAverageThreshFiltEventArg(value));
		}

		private void OnOokFixedThreshChanged(byte value)
		{
			if (this.OokFixedThreshChanged == null)
				return;
			this.OokFixedThreshChanged((object)this, new ByteEventArg(value));
		}

		private void OnFeiStartChanged()
		{
			if (this.FeiStartChanged == null)
				return;
			this.FeiStartChanged((object)this, EventArgs.Empty);
		}

		private void OnAfcAutoClearOnChanged(bool value)
		{
			if (this.AfcAutoClearOnChanged == null)
				return;
			this.AfcAutoClearOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnAfcAutoOnChanged(bool value)
		{
			if (this.AfcAutoOnChanged == null)
				return;
			this.AfcAutoOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnAfcClearChanged()
		{
			if (this.AfcClearChanged == null)
				return;
			this.AfcClearChanged((object)this, EventArgs.Empty);
		}

		private void OnAfcStartChanged()
		{
			if (this.AfcStartChanged == null)
				return;
			this.AfcStartChanged((object)this, EventArgs.Empty);
		}

		private void OnFastRxChanged(bool value)
		{
			if (this.FastRxChanged == null)
				return;
			this.FastRxChanged((object)this, new BooleanEventArg(value));
		}

		private void OnRssiThreshChanged(Decimal value)
		{
			if (this.RssiThreshChanged == null)
				return;
			this.RssiThreshChanged((object)this, new DecimalEventArg(value));
		}

		private void OnRssiStartChanged()
		{
			if (this.RssiStartChanged == null)
				return;
			this.RssiStartChanged((object)this, EventArgs.Empty);
		}

		private void OnTimeoutRxStartChanged(Decimal value)
		{
			if (this.TimeoutRxStartChanged == null)
				return;
			this.TimeoutRxStartChanged((object)this, new DecimalEventArg(value));
		}

		private void OnTimeoutRssiThreshChanged(Decimal value)
		{
			if (this.TimeoutRssiThreshChanged == null)
				return;
			this.TimeoutRssiThreshChanged((object)this, new DecimalEventArg(value));
		}

		private void OnRestartRxChanged()
		{
			if (this.RestartRxChanged == null)
				return;
			this.RestartRxChanged((object)this, EventArgs.Empty);
		}

		private void OnAutoRxRestartOnChanged(bool value)
		{
			if (this.AutoRxRestartOnChanged == null)
				return;
			this.AutoRxRestartOnChanged((object)this, new BooleanEventArg(value));
		}

		public void UpdateRxBwLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					this.nudRxFilterBw.BackColor = SystemColors.Window;
					break;
				case LimitCheckStatusEnum.OUT_OF_RANGE:
					this.nudRxFilterBw.BackColor = ControlPaint.LightLight(Color.Orange);
					break;
				case LimitCheckStatusEnum.ERROR:
					this.nudRxFilterBw.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			this.errorProvider.SetError((Control)this.nudRxFilterBw, message);
		}

		private void rBtnAfcLowBeta_CheckedChanged(object sender, EventArgs e)
		{
			this.AfcLowBetaOn = this.rBtnAfcLowBetaOn.Checked;
			this.OnAfcLowBetaOnChanged(this.AfcLowBetaOn);
		}

		private void nudLowBetaAfcOffset_ValueChanged(object sender, EventArgs e)
		{
			int num1 = (int)Math.Round(this.LowBetaAfcOffset / new Decimal(4880, 0, 0, false, (byte)1), MidpointRounding.AwayFromZero);
			int num2 = (int)Math.Round(this.nudLowBetaAfcOffset.Value / new Decimal(4880, 0, 0, false, (byte)1), MidpointRounding.AwayFromZero);
			int num3 = (int)(this.nudLowBetaAfcOffset.Value - this.LowBetaAfcOffset);
			this.nudLowBetaAfcOffset.ValueChanged -= new EventHandler(this.nudLowBetaAfcOffset_ValueChanged);
			if (num3 >= -1 && num3 <= 1)
				this.nudLowBetaAfcOffset.Value = (Decimal)((num2 - num3) * 488);
			else
				this.nudLowBetaAfcOffset.Value = (Decimal)(num2 * 488);
			this.nudLowBetaAfcOffset.ValueChanged += new EventHandler(this.nudLowBetaAfcOffset_ValueChanged);
			this.LowBetaAfcOffset = this.nudLowBetaAfcOffset.Value;
			this.OnLowBetaAfcOffsetChanged(this.LowBetaAfcOffset);
		}

		private void rBtnSensitivityBoost_CheckedChanged(object sender, EventArgs e)
		{
			this.SensitivityBoostOn = this.rBtnSensitivityBoostOn.Checked;
			this.OnSensitivityBoostOnChanged(this.SensitivityBoostOn);
		}

		private void rBtnRssiAutoThreshOn_CheckedChanged(object sender, EventArgs e)
		{
			this.RssiThresh = this.nudRssiThresh.Value;
			this.OnRssiThreshChanged(this.RssiThresh);
			this.RssiAutoThresh = this.rBtnRssiAutoThreshOn.Checked;
			this.OnRssiAutoThreshChanged(this.RssiAutoThresh);
		}

		private void rBtnDagc_CheckedChanged(object sender, EventArgs e)
		{
			this.DagcOn = this.rBtnDagcOn.Checked;
			this.OnDagcOnChanged(this.DagcOn);
		}

		private void rBtnAgcAutoRef_CheckedChanged(object sender, EventArgs e)
		{
			this.AgcAutoRefOn = this.rBtnAgcAutoRefOn.Checked;
			this.OnAgcAutoRefChanged(this.AgcAutoRefOn);
		}

		private void nudAgcSnrMargin_ValueChanged(object sender, EventArgs e)
		{
			this.AgcSnrMargin = (byte)this.nudAgcSnrMargin.Value;
			this.OnAgcSnrMarginChanged(this.AgcSnrMargin);
		}

		private void nudAgcRefLevel_ValueChanged(object sender, EventArgs e)
		{
			this.AgcRefLevel = (int)this.nudAgcRefLevel.Value;
			this.OnAgcRefLevelChanged(this.AgcRefLevel);
		}

		private void nudAgcStep_ValueChanged(object sender, EventArgs e)
		{
			byte num = (byte)0;
			byte id = (byte)0;
			if (sender == this.nudAgcStep1)
			{
				num = this.AgcStep1 = (byte)this.nudAgcStep1.Value;
				id = (byte)1;
			}
			else if (sender == this.nudAgcStep2)
			{
				num = this.AgcStep2 = (byte)this.nudAgcStep2.Value;
				id = (byte)2;
			}
			else if (sender == this.nudAgcStep3)
			{
				num = this.AgcStep3 = (byte)this.nudAgcStep3.Value;
				id = (byte)3;
			}
			else if (sender == this.nudAgcStep4)
			{
				num = this.AgcStep4 = (byte)this.nudAgcStep4.Value;
				id = (byte)4;
			}
			else if (sender == this.nudAgcStep5)
			{
				num = this.AgcStep5 = (byte)this.nudAgcStep5.Value;
				id = (byte)5;
			}
			this.OnAgcStepChanged(id, num);
		}

		private void rBtnLnaZin_CheckedChanged(object sender, EventArgs e)
		{
			this.LnaZin = this.rBtnLnaZin50.Checked ? LnaZinEnum.ZIN_50 : LnaZinEnum.ZIN_200;
			this.OnLnaZinChanged(this.LnaZin);
		}

		private void rBtnLnaLowPower_CheckedChanged(object sender, EventArgs e)
		{
			this.LnaLowPowerOn = this.rBtnLnaLowPowerOn.Checked;
			this.OnLnaLowPowerOnChanged(this.LnaLowPowerOn);
		}

		private void rBtnLnaGain_CheckedChanged(object sender, EventArgs e)
		{
			this.LnaGainSelect = !this.rBtnLnaGainAutoOn.Checked ? (!this.rBtnLnaGain1.Checked ? (!this.rBtnLnaGain2.Checked ? (!this.rBtnLnaGain3.Checked ? (!this.rBtnLnaGain4.Checked ? (!this.rBtnLnaGain5.Checked ? (!this.rBtnLnaGain6.Checked ? LnaGainEnum.AGC : LnaGainEnum.G6) : LnaGainEnum.G5) : LnaGainEnum.G4) : LnaGainEnum.G3) : LnaGainEnum.G2) : LnaGainEnum.G1) : LnaGainEnum.AGC;
			this.OnLnaGainChanged(this.LnaGainSelect);
		}

		private void nudDccFreq_ValueChanged(object sender, EventArgs e)
		{
			int num1 = (int)(Math.Log10((double)(new Decimal(40, 0, 0, false, (byte)1) * this.RxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * this.DccFreq)) / Math.Log10(2.0) - 2.0);
			int num2 = (int)(Math.Log10((double)(new Decimal(40, 0, 0, false, (byte)1) * this.RxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * this.nudDccFreq.Value)) / Math.Log10(2.0) - 2.0);
			int num3 = (int)(this.nudDccFreq.Value - this.DccFreq);
			if (num3 >= -1 && num3 <= 1 && num1 == num2)
			{
				this.nudDccFreq.ValueChanged -= new EventHandler(this.nudDccFreq_ValueChanged);
				this.nudDccFreq.Value = new Decimal(40, 0, 0, false, (byte)1) * this.RxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * (Decimal)Math.Pow(2.0, (double)(num2 - num3 + 2));
				this.nudDccFreq.ValueChanged += new EventHandler(this.nudDccFreq_ValueChanged);
			}
			this.DccFreq = this.nudDccFreq.Value;
			this.OnDccFreqChanged(this.DccFreq);
		}

		private void nudRxFilterBw_ValueChanged(object sender, EventArgs e)
		{
			Decimal[] rxBwFreqTable = SX1231.ComputeRxBwFreqTable(this.frequencyXo, this.modulationType);
			int num1 = (int)(this.nudRxFilterBw.Value - this.RxBw);
			int index;
			if (num1 >= -1 && num1 <= 1)
			{
				index = Array.IndexOf<Decimal>(rxBwFreqTable, this.RxBw) - num1;
			}
			else
			{
				int mant = 0;
				int exp = 0;
				Decimal num2 = new Decimal(0);
				SX1231.ComputeRxBwMantExp(this.frequencyXo, this.ModulationType, this.nudRxFilterBw.Value, ref mant, ref exp);
				Decimal rxBw = SX1231.ComputeRxBw(this.frequencyXo, this.ModulationType, mant, exp);
				index = Array.IndexOf<Decimal>(rxBwFreqTable, rxBw);
			}
			this.nudRxFilterBw.ValueChanged -= new EventHandler(this.nudRxFilterBw_ValueChanged);
			this.nudRxFilterBw.Value = rxBwFreqTable[index];
			this.nudRxFilterBw.ValueChanged += new EventHandler(this.nudRxFilterBw_ValueChanged);
			this.RxBw = this.nudRxFilterBw.Value;
			this.OnRxBwChanged(this.RxBw);
		}

		private void nudAfcDccFreq_ValueChanged(object sender, EventArgs e)
		{
			int num1 = (int)(Math.Log10((double)(new Decimal(40, 0, 0, false, (byte)1) * this.AfcRxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * this.AfcDccFreq)) / Math.Log10(2.0) - 2.0);
			int num2 = (int)(Math.Log10((double)(new Decimal(40, 0, 0, false, (byte)1) * this.AfcRxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * this.nudAfcDccFreq.Value)) / Math.Log10(2.0) - 2.0);
			int num3 = (int)(this.nudAfcDccFreq.Value - this.AfcDccFreq);
			if (num3 >= -1 && num3 <= 1 && num1 == num2)
			{
				this.nudAfcDccFreq.ValueChanged -= new EventHandler(this.nudAfcDccFreq_ValueChanged);
				this.nudAfcDccFreq.Value = new Decimal(40, 0, 0, false, (byte)1) * this.AfcRxBw / new Decimal(340449852, 1462918, 0, false, (byte)15) * (Decimal)Math.Pow(2.0, (double)(num2 - num3 + 2));
				this.nudAfcDccFreq.ValueChanged += new EventHandler(this.nudAfcDccFreq_ValueChanged);
			}
			this.AfcDccFreq = this.nudAfcDccFreq.Value;
			this.OnAfcDccFreqChanged(this.AfcDccFreq);
		}

		private void nudRxFilterBwAfc_ValueChanged(object sender, EventArgs e)
		{
			Decimal[] rxBwFreqTable = SX1231.ComputeRxBwFreqTable(this.frequencyXo, this.modulationType);
			int num1 = (int)(this.nudRxFilterBwAfc.Value - this.AfcRxBw);
			int index;
			if (num1 >= -1 && num1 <= 1)
			{
				index = Array.IndexOf<Decimal>(rxBwFreqTable, this.AfcRxBw) - num1;
			}
			else
			{
				int mant = 0;
				int exp = 0;
				Decimal num2 = new Decimal(0);
				SX1231.ComputeRxBwMantExp(this.frequencyXo, this.ModulationType, this.nudRxFilterBwAfc.Value, ref mant, ref exp);
				Decimal rxBw = SX1231.ComputeRxBw(this.frequencyXo, this.ModulationType, mant, exp);
				index = Array.IndexOf<Decimal>(rxBwFreqTable, rxBw);
			}
			this.nudRxFilterBwAfc.ValueChanged -= new EventHandler(this.nudRxFilterBwAfc_ValueChanged);
			this.nudRxFilterBwAfc.Value = rxBwFreqTable[index];
			this.nudRxFilterBwAfc.ValueChanged += new EventHandler(this.nudRxFilterBwAfc_ValueChanged);
			this.AfcRxBw = this.nudRxFilterBwAfc.Value;
			this.OnAfcRxBwChanged(this.AfcRxBw);
		}

		private void cBoxOokThreshType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.OokThreshType = (OokThreshTypeEnum)this.cBoxOokThreshType.SelectedIndex;
			this.OnOokThreshTypeChanged(this.OokThreshType);
		}

		private void nudOokPeakThreshStep_Validating(object sender, CancelEventArgs e)
		{
			int num = this.nudOokPeakThreshStep.Value < new Decimal(2) ? 1 : 0;
		}

		private void nudOokPeakThreshStep_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				this.nudOokPeakThreshStep.ValueChanged -= new EventHandler(this.nudOokPeakThreshStep_ValueChanged);
				Decimal[] array = new Decimal[8]
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
				int index1 = 0;
				Decimal num1 = this.nudOokPeakThreshStep.Value - this.OokPeakThreshStep;
				Decimal num2 = new Decimal(10000000);
				for (int index2 = 0; index2 < 8; ++index2)
				{
					if (Math.Abs(this.nudOokPeakThreshStep.Value - array[index2]) < num2)
					{
						num2 = Math.Abs(this.nudOokPeakThreshStep.Value - array[index2]);
						index1 = index2;
					}
				}
				if (num2 / Math.Abs(num1) == new Decimal(1) && num1 >= new Decimal(5, 0, 0, false, (byte)1))
				{
					if (num1 > new Decimal(0))
					{
						NumericUpDownEx numericUpDownEx = this.nudOokPeakThreshStep;
						Decimal num3 = numericUpDownEx.Value + this.nudOokPeakThreshStep.Increment;
						numericUpDownEx.Value = num3;
					}
					else
					{
						NumericUpDownEx numericUpDownEx = this.nudOokPeakThreshStep;
						Decimal num3 = numericUpDownEx.Value - this.nudOokPeakThreshStep.Increment;
						numericUpDownEx.Value = num3;
					}
					index1 = Array.IndexOf<Decimal>(array, this.nudOokPeakThreshStep.Value);
				}
				this.nudOokPeakThreshStep.Value = array[index1];
				this.nudOokPeakThreshStep.ValueChanged += new EventHandler(this.nudOokPeakThreshStep_ValueChanged);
				this.OokPeakThreshStep = this.nudOokPeakThreshStep.Value;
				this.OnOokPeakThreshStepChanged(this.OokPeakThreshStep);
			}
			catch
			{
				this.nudOokPeakThreshStep.ValueChanged += new EventHandler(this.nudOokPeakThreshStep_ValueChanged);
			}
		}

		private void cBoxOokPeakThreshDec_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.OokPeakThreshDec = (OokPeakThreshDecEnum)this.cBoxOokPeakThreshDec.SelectedIndex;
			this.OnOokPeakThreshDecChanged(this.OokPeakThreshDec);
		}

		private void cBoxOokAverageThreshFilt_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.OokAverageThreshFilt = (OokAverageThreshFiltEnum)this.cBoxOokAverageThreshFilt.SelectedIndex;
			this.OnOokAverageThreshFiltChanged(this.OokAverageThreshFilt);
		}

		private void nudOokFixedThresh_ValueChanged(object sender, EventArgs e)
		{
			this.OokFixedThresh = (byte)this.nudOokFixedThresh.Value;
			this.OnOokFixedThreshChanged(this.OokFixedThresh);
		}

		private void btnFeiStart_Click(object sender, EventArgs e)
		{
			this.OnFeiStartChanged();
		}

		private void rBtnAfcAutoClearOn_CheckedChanged(object sender, EventArgs e)
		{
			this.AfcAutoClearOn = this.rBtnAfcAutoClearOn.Checked;
			this.OnAfcAutoClearOnChanged(this.AfcAutoClearOn);
		}

		private void rBtnAfcAutoOn_CheckedChanged(object sender, EventArgs e)
		{
			this.AfcAutoOn = this.rBtnAfcAutoOn.Checked;
			this.OnAfcAutoOnChanged(this.AfcAutoOn);
		}

		private void btnAfcClear_Click(object sender, EventArgs e)
		{
			this.OnAfcClearChanged();
		}

		private void btnAfcStart_Click(object sender, EventArgs e)
		{
			this.OnAfcStartChanged();
		}

		private void rBtnFastRx_CheckedChanged(object sender, EventArgs e)
		{
			this.FastRx = this.rBtnFastRxOn.Checked;
			this.OnFastRxChanged(this.FastRx);
		}

		private void btnRssiStart_Click(object sender, EventArgs e)
		{
			this.OnRssiStartChanged();
		}

		private void nudRssiThresh_ValueChanged(object sender, EventArgs e)
		{
			this.RssiThresh = this.nudRssiThresh.Value;
			this.OnRssiThreshChanged(this.RssiThresh);
		}

		private void nudTimeoutRxStart_ValueChanged(object sender, EventArgs e)
		{
			this.TimeoutRxStart = this.nudTimeoutRxStart.Value;
			this.OnTimeoutRxStartChanged(this.TimeoutRxStart);
		}

		private void nudTimeoutRssiThresh_ValueChanged(object sender, EventArgs e)
		{
			this.TimeoutRssiThresh = this.nudTimeoutRssiThresh.Value;
			this.OnTimeoutRssiThreshChanged(this.TimeoutRssiThresh);
		}

		private void rBtnRssiPhaseAuto_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnRssiPhaseAuto.Checked)
				return;
			this.AutoRxRestartOn = this.rBtnRssiPhaseAuto.Checked;
			this.OnAutoRxRestartOnChanged(this.AutoRxRestartOn);
		}

		private void rBtnRssiPhaseManual_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnRssiPhaseManual.Checked)
				return;
			this.AutoRxRestartOn = this.rBtnRssiPhaseAuto.Checked;
			this.OnAutoRxRestartOnChanged(this.AutoRxRestartOn);
		}

		private void btnRestartRx_Click(object sender, EventArgs e)
		{
			this.OnRestartRxChanged();
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == this.gBoxRxBw)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Rx bandwidth"));
			else if (sender == this.gBoxAfcBw)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Afc bandwidth"));
			else if (sender == this.gBoxOok)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Ook"));
			else if (sender == this.gBoxAfcFei)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Afc Fei"));
			else if (sender == this.gBoxRssi)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Rssi"));
			else if (sender == this.gBoxLna)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Lna"));
			else if (sender == this.gBoxLnaSensitivity)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Lna sensitivity"));
			else if (sender == this.gBoxAgc)
			{
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Agc"));
			}
			else
			{
				if (sender != this.gBoxDagc)
					return;
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Receiver", "Dagc"));
			}
		}

		private void control_MouseLeave(object sender, EventArgs e)
		{
			this.OnDocumentationChanged(new DocumentationChangedEventArgs(".", "Overview"));
		}

		private void OnDocumentationChanged(DocumentationChangedEventArgs e)
		{
			if (this.DocumentationChanged == null)
				return;
			this.DocumentationChanged((object)this, e);
		}
	}
}