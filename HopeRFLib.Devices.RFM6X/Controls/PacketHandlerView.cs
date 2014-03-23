using SemtechLib.Controls;
using SemtechLib.Controls.HexBoxCtrl;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Events;
using SemtechLib.Devices.SX1231.General;
using SemtechLib.General.Events;
using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class PacketHandlerView : UserControl, INotifyDocumentationChanged
	{
		private MaskValidationType syncWord = new MaskValidationType("69-81-7E-96");
		private MaskValidationType aesWord = new MaskValidationType("00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00");
		private OperatingModeEnum mode = OperatingModeEnum.Stdby;
		private byte[] syncValue = new byte[4]
    {
      (byte) 105,
      (byte) 129,
      (byte) 126,
      (byte) 150
    };
		private byte[] aesKey = new byte[16];
		private bool inHexPayloadDataChanged;
		private DataModeEnum dataMode;
		private Decimal bitRate;
		private byte[] message;
		private ushort crc;
		private IContainer components;
		private Label label1;
		private NumericUpDownEx nudPreambleSize;
		private Label label2;
		private Label label3;
		private Panel pnlSync;
		private RadioButton rBtnSyncOn;
		private RadioButton rBtnSyncOff;
		private Label label4;
		private Panel pnlFifoFillCondition;
		private RadioButton rBtnFifoFillAlways;
		private RadioButton rBtnFifoFillSyncAddress;
		private Label label5;
		private NumericUpDownEx nudSyncSize;
		private Label label6;
		private Label label7;
		private NumericUpDownEx nudSyncTol;
		private Label label8;
		private Label label9;
		private MaskedTextBox tBoxSyncValue;
		private Label label10;
		private Panel pnlPacketFormat;
		private RadioButton rBtnPacketFormatVariable;
		private RadioButton rBtnPacketFormatFixed;
		private Label label11;
		private NumericUpDownEx nudPayloadLength;
		private Label lblPayloadLength;
		private Label label12;
		private Label label14;
		private ComboBox cBoxEnterCondition;
		private Label label15;
		private ComboBox cBoxExitCondition;
		private Label label16;
		private ComboBox cBoxIntermediateMode;
		private Label label17;
		private Panel pnlAddressInPayload;
		private Label label18;
		private Panel pnlAddressFiltering;
		private RadioButton rBtnAddressFilteringOff;
		private RadioButton rBtnAddressFilteringNode;
		private RadioButton rBtnAddressFilteringNodeBroadcast;
		private Label label19;
		private NumericUpDownEx nudNodeAddress;
		private Label lblNodeAddress;
		private Label label20;
		private NumericUpDownEx nudBroadcastAddress;
		private Label lblBroadcastAddress;
		private Label label21;
		private Panel pnlDcFree;
		private RadioButton rBtnDcFreeOff;
		private RadioButton rBtnDcFreeManchester;
		private RadioButton rBtnDcFreeWhitening;
		private Label label22;
		private Panel pnlCrcCalculation;
		private RadioButton rBtnCrcOn;
		private RadioButton rBtnCrcOff;
		private Label label23;
		private Panel pnlCrcAutoClear;
		private RadioButton rBtnCrcAutoClearOn;
		private RadioButton rBtnCrcAutoClearOff;
		private Label label24;
		private Panel pnlAesEncryption;
		private RadioButton rBtnAesOn;
		private RadioButton rBtnAesOff;
		private Label label25;
		private MaskedTextBox tBoxAesKey;
		private Label label26;
		private Panel pnlTxStart;
		private RadioButton rBtnTxStartFifoLevel;
		private RadioButton rBtnTxStartFifoNotEmpty;
		private Label label27;
		private NumericUpDownEx nudFifoThreshold;
		private Label label28;
		private Label lblInterPacketRxDelayUnit;
		private GroupBoxEx gBoxPacket;
		private TableLayoutPanel tblPacket;
		private Label label29;
		private Label lblPacketPreamble;
		private Label label30;
		private Label lblPacketSyncValue;
		private Label label31;
		private Label lblPacketLength;
		private Label label32;
		private Panel pnlPacketAddr;
		private Label lblPacketAddr;
		private Label label33;
		private Label lblPayload;
		private Label label34;
		private Panel pnlPacketCrc;
		private Label lblPacketCrc;
		private Led ledPacketCrc;
		private PayloadImg imgPacketMessage;
		private GroupBoxEx gBoxMessage;
		private TableLayoutPanel tblPayloadMessage;
		private Label label35;
		private Label label36;
		private HexBox hexBoxPayload;
		private GroupBoxEx gBoxControl;
		private CheckBox cBtnPacketHandlerStartStop;
		private Label lblPacketsNb;
		private TextBox tBoxPacketsNb;
		private Label lblPacketsRepeatValue;
		private TextBox tBoxPacketsRepeatValue;
		private ErrorProvider errorProvider;
		private TableLayoutPanel tableLayoutPanel1;
		private Panel pnlPayloadLength;
		private TableLayoutPanel tableLayoutPanel2;
		private Panel pnlBroadcastAddress;
		private Panel pnlNodeAddress;
		private GroupBoxEx gBoxDeviceStatus;
		private Label lblOperatingMode;
		private Label label37;
		private Label lblBitSynchroniser;
		private Label lblDataMode;
		private Label label38;
		private Label label39;
		private RadioButton rBtnNodeAddressInPayloadNo;
		private RadioButton rBtnNodeAddressInPayloadYes;
		private ComboBox cBoxInterPacketRxDelay;
		private CheckBox cBtnLog;
		private Label label13;
		private Panel pnlCrcPolynom;
		private RadioButton rBtnCrcCcitt;
		private RadioButton rBtnCrcIbm;

		public OperatingModeEnum Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
				if (this.DataMode == DataModeEnum.Packet && (this.mode == OperatingModeEnum.Tx || this.mode == OperatingModeEnum.Rx))
				{
					this.cBtnPacketHandlerStartStop.Enabled = true;
					this.tBoxPacketsNb.Enabled = true;
					this.tBoxPacketsRepeatValue.Enabled = true;
				}
				else
				{
					this.cBtnPacketHandlerStartStop.Enabled = false;
					this.tBoxPacketsNb.Enabled = false;
					this.tBoxPacketsRepeatValue.Enabled = false;
				}
				switch (this.mode)
				{
					case OperatingModeEnum.Sleep:
						this.lblOperatingMode.Text = "Sleep";
						this.nudPayloadLength.Enabled = false;
						this.rBtnNodeAddressInPayloadYes.Enabled = false;
						this.rBtnNodeAddressInPayloadNo.Enabled = false;
						this.lblPacketsNb.Visible = false;
						this.tBoxPacketsNb.Visible = false;
						this.lblPacketsRepeatValue.Visible = false;
						this.tBoxPacketsRepeatValue.Visible = false;
						break;
					case OperatingModeEnum.Stdby:
						this.lblOperatingMode.Text = "Standby";
						this.nudPayloadLength.Enabled = false;
						this.rBtnNodeAddressInPayloadYes.Enabled = false;
						this.rBtnNodeAddressInPayloadNo.Enabled = false;
						this.lblPacketsNb.Visible = false;
						this.tBoxPacketsNb.Visible = false;
						this.lblPacketsRepeatValue.Visible = false;
						this.tBoxPacketsRepeatValue.Visible = false;
						break;
					case OperatingModeEnum.Fs:
						this.lblOperatingMode.Text = "Synthesizer";
						this.nudPayloadLength.Enabled = false;
						this.rBtnNodeAddressInPayloadYes.Enabled = false;
						this.rBtnNodeAddressInPayloadNo.Enabled = false;
						this.lblPacketsNb.Visible = false;
						this.tBoxPacketsNb.Visible = false;
						this.lblPacketsRepeatValue.Visible = false;
						this.tBoxPacketsRepeatValue.Visible = false;
						break;
					case OperatingModeEnum.Tx:
						this.lblOperatingMode.Text = "Transmitter";
						this.nudPayloadLength.Enabled = false;
						this.rBtnNodeAddressInPayloadYes.Enabled = true;
						this.rBtnNodeAddressInPayloadNo.Enabled = true;
						this.lblPacketsNb.Text = "Tx Packets:";
						this.lblPacketsNb.Visible = true;
						this.tBoxPacketsNb.Visible = true;
						this.lblPacketsRepeatValue.Visible = true;
						this.tBoxPacketsRepeatValue.Visible = true;
						break;
					case OperatingModeEnum.Rx:
						this.lblOperatingMode.Text = "Receiver";
						this.nudPayloadLength.Enabled = true;
						this.rBtnNodeAddressInPayloadYes.Enabled = false;
						this.rBtnNodeAddressInPayloadNo.Enabled = false;
						this.lblPacketsNb.Text = "Rx packets:";
						this.lblPacketsNb.Visible = true;
						this.tBoxPacketsNb.Visible = true;
						this.lblPacketsRepeatValue.Visible = false;
						this.tBoxPacketsRepeatValue.Visible = false;
						break;
				}
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
				if (this.DataMode == DataModeEnum.Packet && (this.mode == OperatingModeEnum.Tx || this.mode == OperatingModeEnum.Rx))
				{
					this.cBtnPacketHandlerStartStop.Enabled = true;
					this.tBoxPacketsNb.Enabled = true;
					if (!this.cBtnPacketHandlerStartStop.Checked)
						this.tBoxPacketsRepeatValue.Enabled = true;
				}
				else
				{
					this.cBtnPacketHandlerStartStop.Enabled = false;
					this.tBoxPacketsNb.Enabled = false;
					this.tBoxPacketsRepeatValue.Enabled = false;
				}
				switch (this.dataMode)
				{
					case DataModeEnum.Packet:
						this.lblBitSynchroniser.Text = "ON";
						this.lblDataMode.Text = "Packet";
						break;
					case DataModeEnum.Reserved:
						this.lblBitSynchroniser.Text = "";
						this.lblDataMode.Text = "";
						break;
					case DataModeEnum.ContinuousBitSync:
						this.lblBitSynchroniser.Text = "ON";
						this.lblDataMode.Text = "Continuous";
						break;
					case DataModeEnum.Continuous:
						this.lblBitSynchroniser.Text = "OFF";
						this.lblDataMode.Text = "Continuous";
						break;
				}
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
				try
				{
					int selectedIndex = this.cBoxInterPacketRxDelay.SelectedIndex;
					this.cBoxInterPacketRxDelay.Items.Clear();
					for (int index = 0; index < 12; ++index)
						this.cBoxInterPacketRxDelay.Items.Add((object)(Math.Pow(2.0, (double)index) / (double)value * 1000.0));
					this.cBoxInterPacketRxDelay.Items.Add((object)0.0);
					if (selectedIndex < this.cBoxInterPacketRxDelay.Items.Count)
						this.cBoxInterPacketRxDelay.SelectedIndex = selectedIndex;
					else
						this.cBoxInterPacketRxDelay.SelectedIndex = 0;
				}
				catch
				{
				}
				this.bitRate = value;
			}
		}

		public int PreambleSize
		{
			get
			{
				return (int)this.nudPreambleSize.Value;
			}
			set
			{
				this.nudPreambleSize.Value = (Decimal)value;
				switch (value)
				{
					case 0:
						this.lblPacketPreamble.Text = "";
						break;
					case 1:
						this.lblPacketPreamble.Text = "55";
						break;
					case 2:
						this.lblPacketPreamble.Text = "55-55";
						break;
					case 3:
						this.lblPacketPreamble.Text = "55-55-55";
						break;
					case 4:
						this.lblPacketPreamble.Text = "55-55-55-55";
						break;
					case 5:
						this.lblPacketPreamble.Text = "55-55-55-55-55";
						break;
					default:
						this.lblPacketPreamble.Text = "55-55-55-55-...-55";
						break;
				}
				if (this.nudPreambleSize.Value < new Decimal(2))
				{
					this.nudPreambleSize.BackColor = ControlPaint.LightLight(Color.Red);
					this.errorProvider.SetError((Control)this.nudPreambleSize, "Preamble size must be greater than 12 bits!");
				}
				else
				{
					this.nudPreambleSize.BackColor = SystemColors.Window;
					this.errorProvider.SetError((Control)this.nudPreambleSize, "");
				}
			}
		}

		public bool SyncOn
		{
			get
			{
				return this.rBtnSyncOn.Checked;
			}
			set
			{
				this.rBtnSyncOn.Checked = value;
				this.rBtnSyncOff.Checked = !value;
				this.nudSyncSize.Enabled = value;
				this.nudSyncTol.Enabled = value;
				this.tBoxSyncValue.Enabled = value;
				this.lblPacketSyncValue.Visible = value;
			}
		}

		public FifoFillConditionEnum FifoFillCondition
		{
			get
			{
				return !this.rBtnFifoFillSyncAddress.Checked ? FifoFillConditionEnum.Allways : FifoFillConditionEnum.OnSyncAddressIrq;
			}
			set
			{
				if (value == FifoFillConditionEnum.OnSyncAddressIrq)
					this.rBtnFifoFillSyncAddress.Checked = true;
				else
					this.rBtnFifoFillAlways.Checked = true;
			}
		}

		public byte SyncSize
		{
			get
			{
				return (byte)this.nudSyncSize.Value;
			}
			set
			{
				try
				{
					this.nudSyncSize.Value = (Decimal)value;
					string text = this.tBoxSyncValue.Text;
					switch ((byte)this.nudSyncSize.Value)
					{
						case (byte)1:
							this.tBoxSyncValue.Mask = "&&";
							break;
						case (byte)2:
							this.tBoxSyncValue.Mask = "&&-&&";
							break;
						case (byte)3:
							this.tBoxSyncValue.Mask = "&&-&&-&&";
							break;
						case (byte)4:
							this.tBoxSyncValue.Mask = "&&-&&-&&-&&";
							break;
						case (byte)5:
							this.tBoxSyncValue.Mask = "&&-&&-&&-&&-&&";
							break;
						case (byte)6:
							this.tBoxSyncValue.Mask = "&&-&&-&&-&&-&&-&&";
							break;
						case (byte)7:
							this.tBoxSyncValue.Mask = "&&-&&-&&-&&-&&-&&-&&";
							break;
						case (byte)8:
							this.tBoxSyncValue.Mask = "&&-&&-&&-&&-&&-&&-&&-&&";
							break;
						default:
							throw new Exception("Wrong sync word size!");
					}
					this.tBoxSyncValue.Text = text;
				}
				catch (Exception ex)
				{
					this.OnError((byte)1, ex.Message);
				}
			}
		}

		public byte SyncTol
		{
			get
			{
				return (byte)this.nudSyncTol.Value;
			}
			set
			{
				this.nudSyncTol.Value = (Decimal)value;
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
				try
				{
					this.tBoxSyncValue.TextChanged -= new EventHandler(this.tBoxSyncValue_TextChanged);
					this.tBoxSyncValue.MaskInputRejected -= new MaskInputRejectedEventHandler(this.tBoxSyncValue_MaskInputRejected);
					this.syncWord.ArrayValue = this.syncValue;
					this.lblPacketSyncValue.Text = this.tBoxSyncValue.Text = this.syncWord.StringValue;
				}
				catch (Exception ex)
				{
					this.OnError((byte)1, ex.Message);
				}
				finally
				{
					this.tBoxSyncValue.TextChanged += new EventHandler(this.tBoxSyncValue_TextChanged);
					this.tBoxSyncValue.MaskInputRejected += new MaskInputRejectedEventHandler(this.tBoxSyncValue_MaskInputRejected);
				}
			}
		}

		public PacketFormatEnum PacketFormat
		{
			get
			{
				return this.rBtnPacketFormatVariable.Checked ? PacketFormatEnum.Variable : PacketFormatEnum.Fixed;
			}
			set
			{
				if (this.Mode == OperatingModeEnum.Tx)
					this.nudPayloadLength.Enabled = false;
				else if (this.Mode == OperatingModeEnum.Rx)
					this.nudPayloadLength.Enabled = true;
				else
					this.nudPayloadLength.Enabled = false;
				if (value == PacketFormatEnum.Variable)
				{
					this.lblPacketLength.Visible = true;
					this.rBtnPacketFormatVariable.Checked = true;
				}
				else
				{
					this.lblPacketLength.Visible = false;
					this.rBtnPacketFormatFixed.Checked = true;
				}
			}
		}

		public DcFreeEnum DcFree
		{
			get
			{
				if (this.rBtnDcFreeOff.Checked)
					return DcFreeEnum.OFF;
				if (this.rBtnDcFreeManchester.Checked)
					return DcFreeEnum.Manchester;
				return this.rBtnDcFreeWhitening.Checked ? DcFreeEnum.Whitening : DcFreeEnum.OFF;
			}
			set
			{
				if (value == DcFreeEnum.Manchester)
					this.rBtnDcFreeManchester.Checked = true;
				else if (value == DcFreeEnum.Whitening)
					this.rBtnDcFreeWhitening.Checked = true;
				else
					this.rBtnDcFreeOff.Checked = true;
			}
		}

		public bool CrcOn
		{
			get
			{
				return this.rBtnCrcOn.Checked;
			}
			set
			{
				this.lblPacketCrc.Visible = value;
				this.rBtnCrcOn.Checked = value;
				this.rBtnCrcOff.Checked = !value;
			}
		}

		public bool CrcAutoClearOff
		{
			get
			{
				return this.rBtnCrcAutoClearOff.Checked;
			}
			set
			{
				this.rBtnCrcAutoClearOn.Checked = !value;
				this.rBtnCrcAutoClearOff.Checked = value;
			}
		}

		public bool CrcIbmOn
		{
			get
			{
				return this.rBtnCrcIbm.Checked;
			}
			set
			{
				this.rBtnCrcIbm.CheckedChanged -= new EventHandler(this.rBtnCrcIbm_CheckedChanged);
				this.rBtnCrcCcitt.CheckedChanged -= new EventHandler(this.rBtnCrcIbm_CheckedChanged);
				if (value)
				{
					this.rBtnCrcIbm.Checked = true;
					this.rBtnCrcCcitt.Checked = false;
				}
				else
				{
					this.rBtnCrcIbm.Checked = false;
					this.rBtnCrcCcitt.Checked = true;
				}
				this.rBtnCrcIbm.CheckedChanged += new EventHandler(this.rBtnCrcIbm_CheckedChanged);
				this.rBtnCrcCcitt.CheckedChanged += new EventHandler(this.rBtnCrcIbm_CheckedChanged);
			}
		}

		public AddressFilteringEnum AddressFiltering
		{
			get
			{
				if (this.rBtnAddressFilteringOff.Checked)
					return AddressFilteringEnum.OFF;
				if (this.rBtnAddressFilteringNode.Checked)
					return AddressFilteringEnum.Node;
				return this.rBtnAddressFilteringNodeBroadcast.Checked ? AddressFilteringEnum.NodeBroadcast : AddressFilteringEnum.OFF;
			}
			set
			{
				if (value == AddressFilteringEnum.Node)
				{
					this.rBtnAddressFilteringNode.Checked = true;
					this.lblPacketAddr.Visible = true;
					this.nudNodeAddress.Enabled = true;
					this.lblNodeAddress.Enabled = true;
					this.nudBroadcastAddress.Enabled = false;
					this.lblBroadcastAddress.Enabled = false;
				}
				else if (value == AddressFilteringEnum.NodeBroadcast)
				{
					this.rBtnAddressFilteringNodeBroadcast.Checked = true;
					this.lblPacketAddr.Visible = true;
					this.nudNodeAddress.Enabled = true;
					this.lblNodeAddress.Enabled = true;
					this.nudBroadcastAddress.Enabled = true;
					this.lblBroadcastAddress.Enabled = true;
				}
				else
				{
					this.rBtnAddressFilteringOff.Checked = true;
					this.lblPacketAddr.Visible = false;
					this.nudNodeAddress.Enabled = false;
					this.lblNodeAddress.Enabled = false;
					this.nudBroadcastAddress.Enabled = false;
					this.lblBroadcastAddress.Enabled = false;
				}
			}
		}

		public byte PayloadLength
		{
			get
			{
				return (byte)this.nudPayloadLength.Value;
			}
			set
			{
				this.nudPayloadLength.Value = (Decimal)value;
				this.lblPayloadLength.Text = "0x" + value.ToString("X02");
			}
		}

		public byte NodeAddress
		{
			get
			{
				return (byte)this.nudNodeAddress.Value;
			}
			set
			{
				this.nudNodeAddress.Value = (Decimal)value;
				this.lblPacketAddr.Text = value.ToString("X02");
				this.lblNodeAddress.Text = "0x" + value.ToString("X02");
			}
		}

		public byte NodeAddressRx
		{
			get
			{
				return (byte)0;
			}
			set
			{
				this.lblPacketAddr.Text = value.ToString("X02");
			}
		}

		public byte BroadcastAddress
		{
			get
			{
				return (byte)this.nudBroadcastAddress.Value;
			}
			set
			{
				this.nudBroadcastAddress.Value = (Decimal)value;
				this.lblBroadcastAddress.Text = "0x" + value.ToString("X02");
			}
		}

		public EnterConditionEnum EnterCondition
		{
			get
			{
				return (EnterConditionEnum)this.cBoxEnterCondition.SelectedIndex;
			}
			set
			{
				this.cBoxEnterCondition.SelectedIndex = (int)value;
			}
		}

		public ExitConditionEnum ExitCondition
		{
			get
			{
				return (ExitConditionEnum)this.cBoxExitCondition.SelectedIndex;
			}
			set
			{
				this.cBoxExitCondition.SelectedIndex = (int)value;
			}
		}

		public IntermediateModeEnum IntermediateMode
		{
			get
			{
				return (IntermediateModeEnum)this.cBoxIntermediateMode.SelectedIndex;
			}
			set
			{
				this.cBoxIntermediateMode.SelectedIndex = (int)value;
			}
		}

		public bool TxStartCondition
		{
			get
			{
				return this.rBtnTxStartFifoNotEmpty.Checked;
			}
			set
			{
				this.rBtnTxStartFifoNotEmpty.Checked = value;
				this.rBtnTxStartFifoLevel.Checked = !value;
			}
		}

		public byte FifoThreshold
		{
			get
			{
				return (byte)this.nudFifoThreshold.Value;
			}
			set
			{
				this.nudFifoThreshold.Value = (Decimal)value;
			}
		}

		public int InterPacketRxDelay
		{
			get
			{
				return this.cBoxInterPacketRxDelay.SelectedIndex;
			}
			set
			{
				if (value >= 12)
					this.cBoxInterPacketRxDelay.SelectedIndex = this.cBoxInterPacketRxDelay.Items.Count - 1;
				else
					this.cBoxInterPacketRxDelay.SelectedIndex = value;
			}
		}

		public bool AesOn
		{
			get
			{
				return this.rBtnAesOn.Checked;
			}
			set
			{
				this.tBoxAesKey.Enabled = value;
				this.rBtnAesOn.Checked = value;
				this.rBtnAesOff.Checked = !value;
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
				try
				{
					this.tBoxAesKey.TextChanged -= new EventHandler(this.tBoxAesKey_TextChanged);
					this.tBoxAesKey.MaskInputRejected -= new MaskInputRejectedEventHandler(this.tBoxAesKey_MaskInputRejected);
					this.aesWord.ArrayValue = this.aesKey;
					this.tBoxAesKey.Text = this.aesWord.StringValue;
				}
				catch (Exception ex)
				{
					this.OnError((byte)1, ex.Message);
				}
				finally
				{
					this.tBoxAesKey.TextChanged += new EventHandler(this.tBoxAesKey_TextChanged);
					this.tBoxAesKey.MaskInputRejected += new MaskInputRejectedEventHandler(this.tBoxAesKey_MaskInputRejected);
				}
			}
		}

		public int MessageLength
		{
			get
			{
				return Convert.ToInt32(this.lblPacketLength.Text, 16);
			}
			set
			{
				this.lblPacketLength.Text = value.ToString("X02");
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
				DynamicByteProvider dynamicByteProvider = this.hexBoxPayload.ByteProvider as DynamicByteProvider;
				dynamicByteProvider.Bytes.Clear();
				dynamicByteProvider.Bytes.AddRange(value);
				this.hexBoxPayload.ByteProvider.ApplyChanges();
				this.hexBoxPayload.Invalidate();
			}
		}

		public ushort Crc
		{
			get
			{
				return this.crc;
			}
			set
			{
				this.crc = value;
				this.lblPacketCrc.Text = ((int)value >> 8 & (int)byte.MaxValue).ToString("X02") + "-" + ((int)value & (int)byte.MaxValue).ToString("X02");
			}
		}

		public bool StartStop
		{
			get
			{
				return this.cBtnPacketHandlerStartStop.Checked;
			}
			set
			{
				this.cBtnPacketHandlerStartStop.Checked = value;
			}
		}

		public int PacketNumber
		{
			get
			{
				return Convert.ToInt32(this.tBoxPacketsNb.Text);
			}
			set
			{
				this.tBoxPacketsNb.Text = value.ToString();
			}
		}

		public int MaxPacketNumber
		{
			get
			{
				return Convert.ToInt32(this.tBoxPacketsRepeatValue.Text);
			}
			set
			{
				this.tBoxPacketsRepeatValue.Text = value.ToString();
			}
		}

		public bool LogEnabled
		{
			get
			{
				return this.cBtnLog.Checked;
			}
			set
			{
				this.cBtnLog.Checked = value;
			}
		}

		public event ErrorEventHandler Error;

		public event Int32EventHandler PreambleSizeChanged;

		public event BooleanEventHandler SyncOnChanged;

		public event FifoFillConditionEventHandler FifoFillConditionChanged;

		public event ByteEventHandler SyncSizeChanged;

		public event ByteEventHandler SyncTolChanged;

		public event ByteArrayEventHandler SyncValueChanged;

		public event PacketFormatEventHandler PacketFormatChanged;

		public event DcFreeEventHandler DcFreeChanged;

		public event BooleanEventHandler CrcOnChanged;

		public event BooleanEventHandler CrcAutoClearOffChanged;

		public event BooleanEventHandler CrcIbmChanged;

		public event AddressFilteringEventHandler AddressFilteringChanged;

		public event ByteEventHandler PayloadLengthChanged;

		public event ByteEventHandler NodeAddressChanged;

		public event ByteEventHandler BroadcastAddressChanged;

		public event EnterConditionEventHandler EnterConditionChanged;

		public event ExitConditionEventHandler ExitConditionChanged;

		public event IntermediateModeEventHandler IntermediateModeChanged;

		public event BooleanEventHandler TxStartConditionChanged;

		public event ByteEventHandler FifoThresholdChanged;

		public event Int32EventHandler InterPacketRxDelayChanged;

		public event BooleanEventHandler AesOnChanged;

		public event ByteArrayEventHandler AesKeyChanged;

		public event Int32EventHandler MessageLengthChanged;

		public event ByteArrayEventHandler MessageChanged;

		public event BooleanEventHandler StartStopChanged;

		public event Int32EventHandler MaxPacketNumberChanged;

		public event BooleanEventHandler PacketHandlerLogEnableChanged;

		public event DocumentationChangedEventHandler DocumentationChanged;

		public PacketHandlerView()
		{
			this.InitializeComponent();
			this.tBoxSyncValue.TextChanged -= new EventHandler(this.tBoxSyncValue_TextChanged);
			this.tBoxSyncValue.MaskInputRejected -= new MaskInputRejectedEventHandler(this.tBoxSyncValue_MaskInputRejected);
			this.tBoxSyncValue.ValidatingType = typeof(MaskValidationType);
			this.tBoxSyncValue.Mask = "&&-&&-&&-&&";
			this.tBoxSyncValue.TextChanged += new EventHandler(this.tBoxSyncValue_TextChanged);
			this.tBoxSyncValue.MaskInputRejected += new MaskInputRejectedEventHandler(this.tBoxSyncValue_MaskInputRejected);
			this.tBoxAesKey.TextChanged -= new EventHandler(this.tBoxAesKey_TextChanged);
			this.tBoxAesKey.MaskInputRejected -= new MaskInputRejectedEventHandler(this.tBoxAesKey_MaskInputRejected);
			this.tBoxAesKey.ValidatingType = typeof(MaskValidationType);
			this.tBoxAesKey.Mask = "&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&";
			this.tBoxAesKey.TextChanged += new EventHandler(this.tBoxAesKey_TextChanged);
			this.tBoxAesKey.MaskInputRejected += new MaskInputRejectedEventHandler(this.tBoxAesKey_MaskInputRejected);
			this.message = new byte[0];
			this.hexBoxPayload.ByteProvider = (IByteProvider)new DynamicByteProvider(new byte[this.Message.Length]);
			this.hexBoxPayload.ByteProvider.Changed += new EventHandler(this.hexBoxPayload_DataChanged);
			this.hexBoxPayload.ByteProvider.ApplyChanges();
		}

		private void OnError(byte status, string message)
		{
			if (this.Error == null)
				return;
			this.Error((object)this, new ErrorEventArgs(status, message));
		}

		private void OnPreambleSizeChanged(int value)
		{
			if (this.PreambleSizeChanged == null)
				return;
			this.PreambleSizeChanged((object)this, new Int32EventArg(value));
		}

		private void OnFifoFillConditionChanged(FifoFillConditionEnum value)
		{
			if (this.FifoFillConditionChanged == null)
				return;
			this.FifoFillConditionChanged((object)this, new FifoFillConditionEventArg(value));
		}

		private void OnSyncOnChanged(bool value)
		{
			if (this.SyncOnChanged == null)
				return;
			this.SyncOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnSyncSizeChanged(byte value)
		{
			if (this.SyncSizeChanged == null)
				return;
			this.SyncSizeChanged((object)this, new ByteEventArg(value));
		}

		private void OnSyncTolChanged(byte value)
		{
			if (this.SyncTolChanged == null)
				return;
			this.SyncTolChanged((object)this, new ByteEventArg(value));
		}

		private void OnSyncValueChanged(byte[] value)
		{
			if (this.SyncValueChanged == null)
				return;
			this.SyncValueChanged((object)this, new ByteArrayEventArg(value));
		}

		private void OnPacketFormatChanged(PacketFormatEnum value)
		{
			if (this.PacketFormatChanged == null)
				return;
			this.PacketFormatChanged((object)this, new PacketFormatEventArg(value));
		}

		private void OnDcFreeChanged(DcFreeEnum value)
		{
			if (this.DcFreeChanged == null)
				return;
			this.DcFreeChanged((object)this, new DcFreeEventArg(value));
		}

		private void OnCrcOnChanged(bool value)
		{
			if (this.CrcOnChanged == null)
				return;
			this.CrcOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnCrcAutoClearOffChanged(bool value)
		{
			if (this.CrcAutoClearOffChanged == null)
				return;
			this.CrcAutoClearOffChanged((object)this, new BooleanEventArg(value));
		}

		private void OnCrcIbmChanged(bool value)
		{
			if (this.CrcIbmChanged == null)
				return;
			this.CrcIbmChanged((object)this, new BooleanEventArg(value));
		}

		private void OnAddressFilteringChanged(AddressFilteringEnum value)
		{
			if (this.AddressFilteringChanged == null)
				return;
			this.AddressFilteringChanged((object)this, new AddressFilteringEventArg(value));
		}

		private void OnPayloadLengthChanged(byte value)
		{
			if (this.PayloadLengthChanged == null)
				return;
			this.PayloadLengthChanged((object)this, new ByteEventArg(value));
		}

		private void OnNodeAddressChanged(byte value)
		{
			if (this.NodeAddressChanged == null)
				return;
			this.NodeAddressChanged((object)this, new ByteEventArg(value));
		}

		private void OnBroadcastAddressChanged(byte value)
		{
			if (this.BroadcastAddressChanged == null)
				return;
			this.BroadcastAddressChanged((object)this, new ByteEventArg(value));
		}

		private void OnEnterConditionChanged(EnterConditionEnum value)
		{
			if (this.EnterConditionChanged == null)
				return;
			this.EnterConditionChanged((object)this, new EnterConditionEventArg(value));
		}

		private void OnExitConditionChanged(ExitConditionEnum value)
		{
			if (this.ExitConditionChanged == null)
				return;
			this.ExitConditionChanged((object)this, new ExitConditionEventArg(value));
		}

		private void OnIntermediateModeChanged(IntermediateModeEnum value)
		{
			if (this.IntermediateModeChanged == null)
				return;
			this.IntermediateModeChanged((object)this, new IntermediateModeEventArg(value));
		}

		private void OnTxStartConditionChanged(bool value)
		{
			if (this.TxStartConditionChanged == null)
				return;
			this.TxStartConditionChanged((object)this, new BooleanEventArg(value));
		}

		private void OnFifoThresholdChanged(byte value)
		{
			if (this.FifoThresholdChanged == null)
				return;
			this.FifoThresholdChanged((object)this, new ByteEventArg(value));
		}

		private void OnInterPacketRxDelayChanged(int value)
		{
			if (this.InterPacketRxDelayChanged == null)
				return;
			this.InterPacketRxDelayChanged((object)this, new Int32EventArg(value));
		}

		private void OnAesOnChanged(bool value)
		{
			if (this.AesOnChanged == null)
				return;
			this.AesOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnAesKeyChanged(byte[] value)
		{
			if (this.AesKeyChanged == null)
				return;
			this.AesKeyChanged((object)this, new ByteArrayEventArg(value));
		}

		private void OnMessageLengthChanged(int value)
		{
			if (this.MessageLengthChanged == null)
				return;
			this.MessageLengthChanged((object)this, new Int32EventArg(value));
		}

		private void OnMessageChanged(byte[] value)
		{
			if (this.MessageChanged == null)
				return;
			this.MessageChanged((object)this, new ByteArrayEventArg(value));
		}

		private void OnStartStopChanged(bool value)
		{
			if (this.StartStopChanged == null)
				return;
			this.StartStopChanged((object)this, new BooleanEventArg(value));
		}

		private void OnMaxPacketNumberChanged(int value)
		{
			if (this.MaxPacketNumberChanged == null)
				return;
			this.MaxPacketNumberChanged((object)this, new Int32EventArg(value));
		}

		private void OnPacketHandlerLogEnableChanged(bool value)
		{
			if (this.PacketHandlerLogEnableChanged == null)
				return;
			this.PacketHandlerLogEnableChanged((object)this, new BooleanEventArg(value));
		}

		public void UpdateSyncValueLimits(LimitCheckStatusEnum status, string message)
		{
			switch (status)
			{
				case LimitCheckStatusEnum.OK:
					this.tBoxSyncValue.BackColor = SystemColors.Window;
					break;
				case LimitCheckStatusEnum.OUT_OF_RANGE:
					this.tBoxSyncValue.BackColor = ControlPaint.LightLight(Color.Orange);
					break;
				case LimitCheckStatusEnum.ERROR:
					this.tBoxSyncValue.BackColor = ControlPaint.LightLight(Color.Red);
					break;
			}
			this.errorProvider.SetError((Control)this.tBoxSyncValue, message);
		}

		private void nudPreambleSize_ValueChanged(object sender, EventArgs e)
		{
			this.PreambleSize = (int)this.nudPreambleSize.Value;
			this.OnPreambleSizeChanged(this.PreambleSize);
		}

		private void rBtnSyncOn_CheckedChanged(object sender, EventArgs e)
		{
			this.SyncOn = this.rBtnSyncOn.Checked;
			this.OnSyncOnChanged(this.SyncOn);
		}

		private void rBtnFifoFill_CheckedChanged(object sender, EventArgs e)
		{
			this.FifoFillCondition = this.rBtnFifoFillSyncAddress.Checked ? FifoFillConditionEnum.OnSyncAddressIrq : FifoFillConditionEnum.Allways;
			this.OnFifoFillConditionChanged(this.FifoFillCondition);
		}

		private void nudSyncSize_ValueChanged(object sender, EventArgs e)
		{
			this.SyncSize = (byte)this.nudSyncSize.Value;
			this.OnSyncSizeChanged(this.SyncSize);
		}

		private void nudSyncTol_ValueChanged(object sender, EventArgs e)
		{
			this.SyncTol = (byte)this.nudSyncTol.Value;
			this.OnSyncTolChanged(this.SyncTol);
		}

		private void tBoxSyncValue_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			this.OnError((byte)1, "Input rejected at position: " + e.Position.ToString((IFormatProvider)CultureInfo.CurrentCulture));
		}

		private void tBoxSyncValue_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
		{
			try
			{
				if (e.IsValidInput)
				{
					if (!(e.ReturnValue is MaskValidationType))
						return;
					this.SyncValue = (e.ReturnValue as MaskValidationType).ArrayValue;
				}
				else
				{
					this.SyncValue = MaskValidationType.InvalidMask.ArrayValue;
					throw new Exception("Wrong Sync word entered.  Message: " + e.Message);
				}
			}
			catch (Exception ex)
			{
				this.OnError((byte)1, ex.Message);
			}
		}

		private void tBoxSyncValue_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Shift || e.Control || Uri.IsHexDigit((char)e.KeyData) || (e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9 || (e.KeyData == Keys.Back || e.KeyData == Keys.Delete)) || (e.KeyData == Keys.Left || e.KeyData == Keys.Right))
			{
				this.OnError((byte)0, "-");
			}
			else
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void tBoxSyncValue_TextChanged(object sender, EventArgs e)
		{
			this.OnError((byte)0, "-");
			this.syncWord.StringValue = this.tBoxSyncValue.Text;
			this.syncValue = this.syncWord.ArrayValue;
			this.lblPacketSyncValue.Text = this.syncWord.StringValue;
		}

		private void rBtnPacketFormat_CheckedChanged(object sender, EventArgs e)
		{
			this.PacketFormat = !this.rBtnPacketFormatVariable.Checked ? PacketFormatEnum.Fixed : PacketFormatEnum.Variable;
			this.OnPacketFormatChanged(this.PacketFormat);
		}

		private void nudPayloadLength_ValueChanged(object sender, EventArgs e)
		{
			this.PayloadLength = (byte)this.nudPayloadLength.Value;
			this.OnPayloadLengthChanged(this.PayloadLength);
		}

		private void cBoxEnterCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.EnterCondition = (EnterConditionEnum)this.cBoxEnterCondition.SelectedIndex;
			this.OnEnterConditionChanged(this.EnterCondition);
		}

		private void cBoxExitCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ExitCondition = (ExitConditionEnum)this.cBoxExitCondition.SelectedIndex;
			this.OnExitConditionChanged(this.ExitCondition);
		}

		private void cBoxIntermediateMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.IntermediateMode = (IntermediateModeEnum)this.cBoxIntermediateMode.SelectedIndex;
			this.OnIntermediateModeChanged(this.IntermediateMode);
		}

		private void rBtnAddressFilteringOff_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnAddressFilteringOff.Checked)
				return;
			this.AddressFiltering = AddressFilteringEnum.OFF;
			this.OnAddressFilteringChanged(this.AddressFiltering);
		}

		private void rBtnAddressFilteringNode_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnAddressFilteringNode.Checked)
				return;
			this.AddressFiltering = AddressFilteringEnum.Node;
			this.OnAddressFilteringChanged(this.AddressFiltering);
		}

		private void rBtnAddressFilteringNodeBroadcast_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnAddressFilteringNodeBroadcast.Checked)
				return;
			this.AddressFiltering = AddressFilteringEnum.NodeBroadcast;
			this.OnAddressFilteringChanged(this.AddressFiltering);
		}

		private void nudNodeAddress_ValueChanged(object sender, EventArgs e)
		{
			this.NodeAddress = (byte)this.nudNodeAddress.Value;
			this.OnNodeAddressChanged(this.NodeAddress);
		}

		private void nudBroadcastAddress_ValueChanged(object sender, EventArgs e)
		{
			this.BroadcastAddress = (byte)this.nudBroadcastAddress.Value;
			this.OnBroadcastAddressChanged(this.BroadcastAddress);
		}

		private void rBtnDcFreeOff_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnDcFreeOff.Checked)
				return;
			this.DcFree = DcFreeEnum.OFF;
			this.OnDcFreeChanged(this.DcFree);
		}

		private void rBtnDcFreeManchester_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnDcFreeManchester.Checked)
				return;
			this.DcFree = DcFreeEnum.Manchester;
			this.OnDcFreeChanged(this.DcFree);
		}

		private void rBtnDcFreeWhitening_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnDcFreeWhitening.Checked)
				return;
			this.DcFree = DcFreeEnum.Whitening;
			this.OnDcFreeChanged(this.DcFree);
		}

		private void rBtnCrcOn_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnCrcOn.Checked)
				return;
			this.CrcOn = this.rBtnCrcOn.Checked;
			this.OnCrcOnChanged(this.CrcOn);
		}

		private void rBtnCrcOff_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnCrcOff.Checked)
				return;
			this.CrcOn = this.rBtnCrcOn.Checked;
			this.OnCrcOnChanged(this.CrcOn);
		}

		private void rBtnCrcAutoClearOn_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnCrcAutoClearOn.Checked)
				return;
			this.CrcAutoClearOff = this.rBtnCrcAutoClearOff.Checked;
			this.OnCrcAutoClearOffChanged(this.CrcAutoClearOff);
		}

		private void rBtnCrcAutoClearOff_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnCrcAutoClearOff.Checked)
				return;
			this.CrcAutoClearOff = this.rBtnCrcAutoClearOff.Checked;
			this.OnCrcAutoClearOffChanged(this.CrcAutoClearOff);
		}

		private void rBtnCrcIbm_CheckedChanged(object sender, EventArgs e)
		{
			this.OnCrcIbmChanged(this.CrcIbmOn);
		}

		private void rBtnAesOn_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnAesOn.Checked)
				return;
			this.AesOn = this.rBtnAesOn.Checked;
			this.OnAesOnChanged(this.AesOn);
		}

		private void rBtnAesOff_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnAesOff.Checked)
				return;
			this.AesOn = this.rBtnAesOn.Checked;
			this.OnAesOnChanged(this.AesOn);
		}

		private void tBoxAesKey_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			this.OnError((byte)1, "Input rejected at position: " + e.Position.ToString((IFormatProvider)CultureInfo.CurrentCulture));
		}

		private void tBoxAesKey_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
		{
			try
			{
				if (e.IsValidInput)
				{
					if (!(e.ReturnValue is MaskValidationType))
						return;
					this.AesKey = (e.ReturnValue as MaskValidationType).ArrayValue;
				}
				else
				{
					this.AesKey = MaskValidationType.InvalidMask.ArrayValue;
					throw new Exception("Wrong AES key entered.  Message: " + e.Message);
				}
			}
			catch (Exception ex)
			{
				this.OnError((byte)1, ex.Message);
			}
		}

		private void tBoxAesKey_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Shift || e.Control || Uri.IsHexDigit((char)e.KeyData) || (e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9 || (e.KeyData == Keys.Back || e.KeyData == Keys.Delete)) || (e.KeyData == Keys.Left || e.KeyData == Keys.Right))
			{
				this.OnError((byte)0, "-");
			}
			else
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void tBoxAesKey_TextChanged(object sender, EventArgs e)
		{
			this.OnError((byte)0, "-");
			this.aesWord.StringValue = this.tBoxAesKey.Text;
			this.aesKey = this.aesWord.ArrayValue;
		}

		private void tBox_Validated(object sender, EventArgs e)
		{
			if (sender == this.tBoxSyncValue)
			{
				this.tBoxSyncValue.Text = this.tBoxSyncValue.Text.ToUpper();
				this.lblPacketSyncValue.Text = this.tBoxSyncValue.Text;
				this.OnSyncValueChanged(this.SyncValue);
			}
			else if (sender == this.tBoxAesKey)
			{
				this.tBoxAesKey.Text = this.tBoxAesKey.Text.ToUpper();
				this.OnAesKeyChanged(this.AesKey);
			}
			else
			{
				TextBox textBox = (TextBox)sender;
				if (!(textBox.Text != "") || textBox.Text.StartsWith("0x", true, (CultureInfo)null))
					return;
				textBox.Text = "0x" + Convert.ToByte(textBox.Text, 16).ToString("X02");
			}
		}

		private void rBtnTxStartFifoLevel_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnTxStartFifoLevel.Checked)
				return;
			this.TxStartCondition = !this.rBtnTxStartFifoLevel.Checked;
			this.OnTxStartConditionChanged(this.TxStartCondition);
		}

		private void rBtnTxStartFifoNotEmpty_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.rBtnTxStartFifoNotEmpty.Checked)
				return;
			this.TxStartCondition = !this.rBtnTxStartFifoLevel.Checked;
			this.OnTxStartConditionChanged(this.TxStartCondition);
		}

		private void nudFifoThreshold_ValueChanged(object sender, EventArgs e)
		{
			this.FifoThreshold = (byte)this.nudFifoThreshold.Value;
			this.OnFifoThresholdChanged(this.FifoThreshold);
		}

		private void nudInterPacketRxDelay_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.InterPacketRxDelay = this.cBoxInterPacketRxDelay.SelectedIndex;
			this.OnInterPacketRxDelayChanged(this.InterPacketRxDelay);
		}

		private void hexBoxPayload_DataChanged(object sender, EventArgs e)
		{
			if (this.inHexPayloadDataChanged)
				return;
			this.inHexPayloadDataChanged = true;
			if (this.hexBoxPayload.ByteProvider.Length > 64L)
			{
				this.hexBoxPayload.ByteProvider.DeleteBytes(64L, this.hexBoxPayload.ByteProvider.Length - 64L);
				this.hexBoxPayload.SelectionStart = 64L;
				this.hexBoxPayload.SelectionLength = 0L;
			}
			Array.Resize<byte>(ref this.message, (int)this.hexBoxPayload.ByteProvider.Length);
			for (int index = 0; index < this.message.Length; ++index)
				this.message[index] = this.hexBoxPayload.ByteProvider.ReadByte((long)index);
			this.OnMessageChanged(this.Message);
			this.inHexPayloadDataChanged = false;
		}

		private void cBtnPacketHandlerStartStop_CheckedChanged(object sender, EventArgs e)
		{
			if (this.cBtnPacketHandlerStartStop.Checked)
				this.cBtnPacketHandlerStartStop.Text = "Stop";
			else
				this.cBtnPacketHandlerStartStop.Text = "Start";
			this.tableLayoutPanel1.Enabled = !this.cBtnPacketHandlerStartStop.Checked;
			this.tableLayoutPanel2.Enabled = !this.cBtnPacketHandlerStartStop.Checked;
			this.gBoxPacket.Enabled = !this.cBtnPacketHandlerStartStop.Checked;
			this.tBoxPacketsRepeatValue.Enabled = !this.cBtnPacketHandlerStartStop.Checked;
			try
			{
				this.MaxPacketNumber = Convert.ToInt32(this.tBoxPacketsRepeatValue.Text);
			}
			catch
			{
				this.MaxPacketNumber = 0;
				this.tBoxPacketsRepeatValue.Text = "0";
				this.OnError((byte)1, "Wrong max packet value! Value has been reseted.");
			}
			this.OnMaxPacketNumberChanged(this.MaxPacketNumber);
			this.OnStartStopChanged(this.cBtnPacketHandlerStartStop.Checked);
		}

		private void cBtnLog_CheckedChanged(object sender, EventArgs e)
		{
			this.OnPacketHandlerLogEnableChanged(this.cBtnLog.Checked);
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == this.nudPreambleSize)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Preamble size"));
			else if (sender == this.pnlSync || sender == this.rBtnSyncOn || sender == this.rBtnSyncOff)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Sync word"));
			else if (sender == this.pnlFifoFillCondition || sender == this.rBtnFifoFillSyncAddress || sender == this.rBtnFifoFillAlways)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Fifo fill condition"));
			else if (sender == this.nudSyncSize)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Sync word size"));
			else if (sender == this.nudSyncTol)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Sync word tolerance"));
			else if (sender == this.tBoxSyncValue)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Sync word value"));
			else if (sender == this.pnlPacketFormat || sender == this.rBtnPacketFormatFixed || sender == this.rBtnPacketFormatVariable)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Packet format"));
			else if (sender == this.pnlPayloadLength || sender == this.nudPayloadLength || sender == this.lblPayloadLength)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Payload length"));
			else if (sender == this.cBoxEnterCondition)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Intermediate mode enter"));
			else if (sender == this.cBoxExitCondition)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Intermediate mode exit"));
			else if (sender == this.cBoxIntermediateMode)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Intermediate mode"));
			else if (sender == this.pnlAddressInPayload || sender == this.rBtnNodeAddressInPayloadYes || sender == this.rBtnNodeAddressInPayloadNo)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Address in payload"));
			else if (sender == this.pnlAddressFiltering || sender == this.rBtnAddressFilteringOff || (sender == this.rBtnAddressFilteringNode || sender == this.rBtnAddressFilteringNodeBroadcast))
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Address filtering"));
			else if (sender == this.pnlNodeAddress || sender == this.nudNodeAddress || sender == this.lblNodeAddress)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Node address"));
			else if (sender == this.pnlBroadcastAddress || sender == this.nudBroadcastAddress || sender == this.lblBroadcastAddress)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Broadcast address"));
			else if (sender == this.pnlDcFree || sender == this.rBtnDcFreeOff || (sender == this.rBtnDcFreeManchester || sender == this.rBtnDcFreeWhitening))
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Dc free"));
			else if (sender == this.pnlCrcCalculation || sender == this.rBtnCrcOn || sender == this.rBtnCrcOff)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Crc calculation"));
			else if (sender == this.pnlCrcAutoClear || sender == this.rBtnCrcAutoClearOn || sender == this.rBtnCrcAutoClearOff)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Crc auto clear"));
			else if (sender == this.pnlCrcPolynom || sender == this.rBtnCrcIbm || sender == this.rBtnCrcCcitt)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Crc polynom"));
			else if (sender == this.pnlAesEncryption || sender == this.rBtnAesOn || sender == this.rBtnAesOff)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Aes encryption"));
			else if (sender == this.tBoxAesKey)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Aes key value"));
			else if (sender == this.pnlTxStart || sender == this.rBtnTxStartFifoLevel || sender == this.rBtnTxStartFifoNotEmpty)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Tx start"));
			else if (sender == this.nudFifoThreshold)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Fifo threshold"));
			else if (sender == this.cBoxInterPacketRxDelay)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Inter packet rx delay"));
			else if (sender == this.gBoxControl)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Control"));
			else if (sender == this.gBoxPacket)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Packet"));
			else if (sender == this.gBoxMessage)
			{
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Packet handler", "Message"));
			}
			else
			{
				if (sender != this.gBoxDeviceStatus)
					return;
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Common", "Device status"));
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
			this.tBoxSyncValue = new MaskedTextBox();
			this.nudPreambleSize = new NumericUpDownEx();
			this.label12 = new Label();
			this.label1 = new Label();
			this.label6 = new Label();
			this.label8 = new Label();
			this.label2 = new Label();
			this.label18 = new Label();
			this.tBoxAesKey = new MaskedTextBox();
			this.label11 = new Label();
			this.label20 = new Label();
			this.label10 = new Label();
			this.label21 = new Label();
			this.label7 = new Label();
			this.label5 = new Label();
			this.label25 = new Label();
			this.label24 = new Label();
			this.label19 = new Label();
			this.lblInterPacketRxDelayUnit = new Label();
			this.cBoxEnterCondition = new ComboBox();
			this.label14 = new Label();
			this.cBoxExitCondition = new ComboBox();
			this.label15 = new Label();
			this.cBoxIntermediateMode = new ComboBox();
			this.label28 = new Label();
			this.label16 = new Label();
			this.label27 = new Label();
			this.label26 = new Label();
			this.pnlAesEncryption = new Panel();
			this.rBtnAesOff = new RadioButton();
			this.rBtnAesOn = new RadioButton();
			this.pnlDcFree = new Panel();
			this.rBtnDcFreeWhitening = new RadioButton();
			this.rBtnDcFreeManchester = new RadioButton();
			this.rBtnDcFreeOff = new RadioButton();
			this.pnlAddressInPayload = new Panel();
			this.rBtnNodeAddressInPayloadNo = new RadioButton();
			this.rBtnNodeAddressInPayloadYes = new RadioButton();
			this.label17 = new Label();
			this.pnlFifoFillCondition = new Panel();
			this.rBtnFifoFillAlways = new RadioButton();
			this.rBtnFifoFillSyncAddress = new RadioButton();
			this.label4 = new Label();
			this.pnlSync = new Panel();
			this.rBtnSyncOff = new RadioButton();
			this.rBtnSyncOn = new RadioButton();
			this.label3 = new Label();
			this.label9 = new Label();
			this.pnlCrcAutoClear = new Panel();
			this.rBtnCrcAutoClearOff = new RadioButton();
			this.rBtnCrcAutoClearOn = new RadioButton();
			this.label23 = new Label();
			this.pnlCrcCalculation = new Panel();
			this.rBtnCrcOff = new RadioButton();
			this.rBtnCrcOn = new RadioButton();
			this.label22 = new Label();
			this.pnlTxStart = new Panel();
			this.rBtnTxStartFifoNotEmpty = new RadioButton();
			this.rBtnTxStartFifoLevel = new RadioButton();
			this.pnlAddressFiltering = new Panel();
			this.rBtnAddressFilteringNodeBroadcast = new RadioButton();
			this.rBtnAddressFilteringNode = new RadioButton();
			this.rBtnAddressFilteringOff = new RadioButton();
			this.lblNodeAddress = new Label();
			this.lblPayloadLength = new Label();
			this.lblBroadcastAddress = new Label();
			this.pnlPacketFormat = new Panel();
			this.rBtnPacketFormatFixed = new RadioButton();
			this.rBtnPacketFormatVariable = new RadioButton();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.pnlPayloadLength = new Panel();
			this.nudPayloadLength = new NumericUpDownEx();
			this.nudSyncSize = new NumericUpDownEx();
			this.nudSyncTol = new NumericUpDownEx();
			this.pnlNodeAddress = new Panel();
			this.nudNodeAddress = new NumericUpDownEx();
			this.pnlBroadcastAddress = new Panel();
			this.nudBroadcastAddress = new NumericUpDownEx();
			this.tableLayoutPanel2 = new TableLayoutPanel();
			this.label13 = new Label();
			this.pnlCrcPolynom = new Panel();
			this.rBtnCrcCcitt = new RadioButton();
			this.rBtnCrcIbm = new RadioButton();
			this.nudFifoThreshold = new NumericUpDownEx();
			this.cBoxInterPacketRxDelay = new ComboBox();
			this.gBoxDeviceStatus = new GroupBoxEx();
			this.lblOperatingMode = new Label();
			this.label37 = new Label();
			this.lblBitSynchroniser = new Label();
			this.lblDataMode = new Label();
			this.label38 = new Label();
			this.label39 = new Label();
			this.gBoxControl = new GroupBoxEx();
			this.tBoxPacketsNb = new TextBox();
			this.cBtnLog = new CheckBox();
			this.cBtnPacketHandlerStartStop = new CheckBox();
			this.lblPacketsNb = new Label();
			this.tBoxPacketsRepeatValue = new TextBox();
			this.lblPacketsRepeatValue = new Label();
			this.gBoxPacket = new GroupBoxEx();
			this.imgPacketMessage = new PayloadImg();
			this.gBoxMessage = new GroupBoxEx();
			this.tblPayloadMessage = new TableLayoutPanel();
			this.hexBoxPayload = new HexBox();
			this.label36 = new Label();
			this.label35 = new Label();
			this.tblPacket = new TableLayoutPanel();
			this.label29 = new Label();
			this.label30 = new Label();
			this.label31 = new Label();
			this.label32 = new Label();
			this.label33 = new Label();
			this.label34 = new Label();
			this.lblPacketPreamble = new Label();
			this.lblPayload = new Label();
			this.pnlPacketCrc = new Panel();
			this.ledPacketCrc = new Led();
			this.lblPacketCrc = new Label();
			this.pnlPacketAddr = new Panel();
			this.lblPacketAddr = new Label();
			this.lblPacketLength = new Label();
			this.lblPacketSyncValue = new Label();
			this.nudPreambleSize.BeginInit();
			this.pnlAesEncryption.SuspendLayout();
			this.pnlDcFree.SuspendLayout();
			this.pnlAddressInPayload.SuspendLayout();
			this.pnlFifoFillCondition.SuspendLayout();
			this.pnlSync.SuspendLayout();
			this.pnlCrcAutoClear.SuspendLayout();
			this.pnlCrcCalculation.SuspendLayout();
			this.pnlTxStart.SuspendLayout();
			this.pnlAddressFiltering.SuspendLayout();
			this.pnlPacketFormat.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.pnlPayloadLength.SuspendLayout();
			this.nudPayloadLength.BeginInit();
			this.nudSyncSize.BeginInit();
			this.nudSyncTol.BeginInit();
			this.pnlNodeAddress.SuspendLayout();
			this.nudNodeAddress.BeginInit();
			this.pnlBroadcastAddress.SuspendLayout();
			this.nudBroadcastAddress.BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.pnlCrcPolynom.SuspendLayout();
			this.nudFifoThreshold.BeginInit();
			this.gBoxDeviceStatus.SuspendLayout();
			this.gBoxControl.SuspendLayout();
			this.gBoxPacket.SuspendLayout();
			this.gBoxMessage.SuspendLayout();
			this.tblPayloadMessage.SuspendLayout();
			this.tblPacket.SuspendLayout();
			this.pnlPacketCrc.SuspendLayout();
			this.pnlPacketAddr.SuspendLayout();
			this.SuspendLayout();
			this.errorProvider.ContainerControl = (ContainerControl)this;
			this.tBoxSyncValue.Anchor = AnchorStyles.Left;
			this.errorProvider.SetIconPadding((Control)this.tBoxSyncValue, 6);
			this.tBoxSyncValue.InsertKeyMode = InsertKeyMode.Overwrite;
			this.tBoxSyncValue.Location = new Point(163, 123);
			this.tBoxSyncValue.Margin = new Padding(3, 2, 3, 2);
			this.tBoxSyncValue.Mask = "&&-&&-&&-&&-&&-&&-&&-&&";
			this.tBoxSyncValue.Name = "tBoxSyncValue";
			this.tBoxSyncValue.Size = new Size(143, 21);
			this.tBoxSyncValue.TabIndex = 14;
			this.tBoxSyncValue.Text = "AAAAAAAAAAAAAAAA";
			this.tBoxSyncValue.MaskInputRejected += new MaskInputRejectedEventHandler(this.tBoxSyncValue_MaskInputRejected);
			this.tBoxSyncValue.TypeValidationCompleted += new TypeValidationEventHandler(this.tBoxSyncValue_TypeValidationCompleted);
			this.tBoxSyncValue.TextChanged += new EventHandler(this.tBoxSyncValue_TextChanged);
			this.tBoxSyncValue.KeyDown += new KeyEventHandler(this.tBoxSyncValue_KeyDown);
			this.tBoxSyncValue.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.tBoxSyncValue.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.tBoxSyncValue.Validated += new EventHandler(this.tBox_Validated);
			this.nudPreambleSize.Anchor = AnchorStyles.Left;
			this.errorProvider.SetIconPadding((Control)this.nudPreambleSize, 6);
			this.nudPreambleSize.Location = new Point(163, 2);
			this.nudPreambleSize.Margin = new Padding(3, 2, 3, 2);
			int[] bits1 = new int[4];
			bits1[0] = (int)ushort.MaxValue;
			Decimal num1 = new Decimal(bits1);
			this.nudPreambleSize.Maximum = num1;
			this.nudPreambleSize.Name = "nudPreambleSize";
			this.nudPreambleSize.Size = new Size(59, 21);
			this.nudPreambleSize.TabIndex = 1;
			int[] bits2 = new int[4];
			bits2[0] = 3;
			Decimal num2 = new Decimal(bits2);
			this.nudPreambleSize.Value = num2;
			this.nudPreambleSize.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.nudPreambleSize.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudPreambleSize.ValueChanged += new EventHandler(this.nudPreambleSize_ValueChanged);
			this.label12.Anchor = AnchorStyles.None;
			this.label12.AutoSize = true;
			this.label12.Location = new Point(341, 175);
			this.label12.Name = "label12";
			this.label12.Size = new Size(35, 12);
			this.label12.TabIndex = 19;
			this.label12.Text = "bytes";
			this.label12.TextAlign = ContentAlignment.MiddleLeft;
			this.label1.Anchor = AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label1.Location = new Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new Size(75, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Preamble size:";
			this.label1.TextAlign = ContentAlignment.MiddleLeft;
			this.label6.Anchor = AnchorStyles.None;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(341, 77);
			this.label6.Name = "label6";
			this.label6.Size = new Size(35, 12);
			this.label6.TabIndex = 9;
			this.label6.Text = "bytes";
			this.label6.TextAlign = ContentAlignment.MiddleLeft;
			this.label8.Anchor = AnchorStyles.None;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(344, 102);
			this.label8.Name = "label8";
			this.label8.Size = new Size(29, 12);
			this.label8.TabIndex = 12;
			this.label8.Text = "bits";
			this.label8.TextAlign = ContentAlignment.MiddleLeft;
			this.label2.Anchor = AnchorStyles.None;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(341, 6);
			this.label2.Name = "label2";
			this.label2.Size = new Size(35, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "bytes";
			this.label2.TextAlign = ContentAlignment.MiddleLeft;
			this.label18.Anchor = AnchorStyles.Left;
			this.label18.AutoSize = true;
			this.label18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label18.Location = new Point(3, 28);
			this.label18.Name = "label18";
			this.label18.Size = new Size(116, 13);
			this.label18.TabIndex = 2;
			this.label18.Text = "Address based filtering:";
			this.label18.TextAlign = ContentAlignment.MiddleLeft;
			this.tBoxAesKey.Anchor = AnchorStyles.Left;
			this.tableLayoutPanel2.SetColumnSpan((Control)this.tBoxAesKey, 2);
			this.tBoxAesKey.InsertKeyMode = InsertKeyMode.Overwrite;
			this.tBoxAesKey.Location = new Point(152, 211);
			this.tBoxAesKey.Margin = new Padding(3, 0, 3, 0);
			this.tBoxAesKey.Mask = "&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&-&&";
			this.tBoxAesKey.Name = "tBoxAesKey";
			this.tBoxAesKey.Size = new Size(277, 21);
			this.tBoxAesKey.TabIndex = 15;
			this.tBoxAesKey.Text = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
			this.tBoxAesKey.TextAlign = HorizontalAlignment.Center;
			this.tBoxAesKey.MaskInputRejected += new MaskInputRejectedEventHandler(this.tBoxAesKey_MaskInputRejected);
			this.tBoxAesKey.TypeValidationCompleted += new TypeValidationEventHandler(this.tBoxAesKey_TypeValidationCompleted);
			this.tBoxAesKey.TextChanged += new EventHandler(this.tBoxAesKey_TextChanged);
			this.tBoxAesKey.KeyDown += new KeyEventHandler(this.tBoxAesKey_KeyDown);
			this.tBoxAesKey.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.tBoxAesKey.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.tBoxAesKey.Validated += new EventHandler(this.tBox_Validated);
			this.label11.Anchor = AnchorStyles.Left;
			this.label11.AutoSize = true;
			this.label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label11.Location = new Point(3, 175);
			this.label11.Name = "label11";
			this.label11.Size = new Size(80, 13);
			this.label11.TabIndex = 17;
			this.label11.Text = "Payload length:";
			this.label11.TextAlign = ContentAlignment.MiddleLeft;
			this.label20.Anchor = AnchorStyles.Left;
			this.label20.AutoSize = true;
			this.label20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label20.Location = new Point(3, 77);
			this.label20.Name = "label20";
			this.label20.Size = new Size(98, 13);
			this.label20.TabIndex = 5;
			this.label20.Text = "Broadcast address:";
			this.label20.TextAlign = ContentAlignment.MiddleLeft;
			this.label10.Anchor = AnchorStyles.Left;
			this.label10.AutoSize = true;
			this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label10.Location = new Point(3, 151);
			this.label10.Name = "label10";
			this.label10.Size = new Size(76, 13);
			this.label10.TabIndex = 15;
			this.label10.Text = "Packet format:";
			this.label10.TextAlign = ContentAlignment.MiddleLeft;
			this.label21.Anchor = AnchorStyles.Left;
			this.label21.AutoSize = true;
			this.label21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label21.Location = new Point(3, 101);
			this.label21.Name = "label21";
			this.label21.Size = new Size(46, 13);
			this.label21.TabIndex = 6;
			this.label21.Text = "DC-free:";
			this.label21.TextAlign = ContentAlignment.MiddleLeft;
			this.label7.Anchor = AnchorStyles.Left;
			this.label7.AutoSize = true;
			this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label7.Location = new Point(3, 102);
			this.label7.Name = "label7";
			this.label7.Size = new Size(107, 13);
			this.label7.TabIndex = 10;
			this.label7.Text = "Sync word tolerance:";
			this.label7.TextAlign = ContentAlignment.MiddleLeft;
			this.label5.Anchor = AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.BackColor = Color.Transparent;
			this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label5.Location = new Point(3, 77);
			this.label5.Name = "label5";
			this.label5.Size = new Size(81, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "Sync word size:";
			this.label5.TextAlign = ContentAlignment.MiddleLeft;
			this.label25.Anchor = AnchorStyles.Left;
			this.label25.AutoSize = true;
			this.label25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label25.Location = new Point(3, 215);
			this.label25.Name = "label25";
			this.label25.Size = new Size(51, 13);
			this.label25.TabIndex = 14;
			this.label25.Text = "AES key:";
			this.label25.TextAlign = ContentAlignment.MiddleLeft;
			this.label24.Anchor = AnchorStyles.Left;
			this.label24.AutoSize = true;
			this.label24.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label24.Location = new Point(3, 193);
			this.label24.Name = "label24";
			this.label24.Size = new Size(31, 13);
			this.label24.TabIndex = 12;
			this.label24.Text = "AES:";
			this.label24.TextAlign = ContentAlignment.MiddleLeft;
			this.label19.Anchor = AnchorStyles.Left;
			this.label19.AutoSize = true;
			this.label19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label19.Location = new Point(3, 52);
			this.label19.Name = "label19";
			this.label19.Size = new Size(76, 13);
			this.label19.TabIndex = 4;
			this.label19.Text = "Node address:";
			this.label19.TextAlign = ContentAlignment.MiddleLeft;
			this.lblInterPacketRxDelayUnit.Anchor = AnchorStyles.None;
			this.lblInterPacketRxDelayUnit.AutoSize = true;
			this.lblInterPacketRxDelayUnit.Location = new Point(404, 283);
			this.lblInterPacketRxDelayUnit.Name = "lblInterPacketRxDelayUnit";
			this.lblInterPacketRxDelayUnit.Size = new Size(17, 12);
			this.lblInterPacketRxDelayUnit.TabIndex = 22;
			this.lblInterPacketRxDelayUnit.Text = "ms";
			this.lblInterPacketRxDelayUnit.TextAlign = ContentAlignment.MiddleLeft;
			this.cBoxEnterCondition.Anchor = AnchorStyles.Left;
			this.cBoxEnterCondition.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cBoxEnterCondition.FormattingEnabled = true;
			this.cBoxEnterCondition.Items.AddRange(new object[8]
      {
        (object) "None ( Auto Modes OFF )",
        (object) "Rising edge of FifoNotEmpty",
        (object) "Rising edge of FifoLevel",
        (object) "Rising edge of CrcOk",
        (object) "Rising edge of PayloadReady",
        (object) "Rising edge of SyncAddress",
        (object) "Rising edge of PacketSent",
        (object) "Falling edge of FifoNotEmpty"
      });
			this.cBoxEnterCondition.Location = new Point(163, 196);
			this.cBoxEnterCondition.Margin = new Padding(3, 2, 3, 2);
			this.cBoxEnterCondition.Name = "cBoxEnterCondition";
			this.cBoxEnterCondition.Size = new Size(172, 20);
			this.cBoxEnterCondition.TabIndex = 23;
			this.cBoxEnterCondition.SelectedIndexChanged += new EventHandler(this.cBoxEnterCondition_SelectedIndexChanged);
			this.cBoxEnterCondition.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.cBoxEnterCondition.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label14.Anchor = AnchorStyles.Left;
			this.label14.AutoSize = true;
			this.label14.Location = new Point(3, 200);
			this.label14.Name = "label14";
			this.label14.Size = new Size(149, 12);
			this.label14.TabIndex = 22;
			this.label14.Text = "Intermediate mode enter:";
			this.label14.TextAlign = ContentAlignment.MiddleLeft;
			this.cBoxExitCondition.Anchor = AnchorStyles.Left;
			this.cBoxExitCondition.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cBoxExitCondition.FormattingEnabled = true;
			this.cBoxExitCondition.Items.AddRange(new object[8]
      {
        (object) "None ( Auto Modes OFF )",
        (object) "Falling edge of FifoNotEmpty",
        (object) "Rising edge of FifoLevel or Timeout",
        (object) "Rising edge of CrcOk or TimeOut",
        (object) "Rising edge of PayloadReady or Timeout",
        (object) "Rising edge of SyncAddress or Timeout",
        (object) "Rising edge of PacketSent",
        (object) "Rising edge of Timeout"
      });
			this.cBoxExitCondition.Location = new Point(163, 220);
			this.cBoxExitCondition.Margin = new Padding(3, 2, 3, 2);
			this.cBoxExitCondition.Name = "cBoxExitCondition";
			this.cBoxExitCondition.Size = new Size(172, 20);
			this.cBoxExitCondition.TabIndex = 25;
			this.cBoxExitCondition.SelectedIndexChanged += new EventHandler(this.cBoxExitCondition_SelectedIndexChanged);
			this.cBoxExitCondition.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.cBoxExitCondition.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label15.Anchor = AnchorStyles.Left;
			this.label15.AutoSize = true;
			this.label15.Location = new Point(3, 224);
			this.label15.Name = "label15";
			this.label15.Size = new Size(143, 12);
			this.label15.TabIndex = 24;
			this.label15.Text = "Intermediate mode exit:";
			this.label15.TextAlign = ContentAlignment.MiddleLeft;
			this.cBoxIntermediateMode.Anchor = AnchorStyles.Left;
			this.cBoxIntermediateMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cBoxIntermediateMode.FormattingEnabled = true;
			this.cBoxIntermediateMode.Items.AddRange(new object[4]
      {
        (object) "Sleep",
        (object) "Standby",
        (object) "Rx",
        (object) "Tx"
      });
			this.cBoxIntermediateMode.Location = new Point(163, 244);
			this.cBoxIntermediateMode.Margin = new Padding(3, 2, 3, 2);
			this.cBoxIntermediateMode.Name = "cBoxIntermediateMode";
			this.cBoxIntermediateMode.Size = new Size(172, 20);
			this.cBoxIntermediateMode.TabIndex = 27;
			this.cBoxIntermediateMode.SelectedIndexChanged += new EventHandler(this.cBoxIntermediateMode_SelectedIndexChanged);
			this.cBoxIntermediateMode.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.cBoxIntermediateMode.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label28.Anchor = AnchorStyles.Left;
			this.label28.AutoSize = true;
			this.label28.Location = new Point(3, 283);
			this.label28.Name = "label28";
			this.label28.Size = new Size(137, 12);
			this.label28.TabIndex = 20;
			this.label28.Text = "Inter packet Rx delay:";
			this.label28.TextAlign = ContentAlignment.MiddleLeft;
			this.label16.Anchor = AnchorStyles.Left;
			this.label16.AutoSize = true;
			this.label16.Location = new Point(3, 248);
			this.label16.Name = "label16";
			this.label16.Size = new Size(119, 12);
			this.label16.TabIndex = 26;
			this.label16.Text = "Intermediate  mode:";
			this.label16.TextAlign = ContentAlignment.MiddleLeft;
			this.label27.Anchor = AnchorStyles.Left;
			this.label27.AutoSize = true;
			this.label27.Location = new Point(3, 260);
			this.label27.Name = "label27";
			this.label27.Size = new Size(95, 12);
			this.label27.TabIndex = 18;
			this.label27.Text = "FIFO Threshold:";
			this.label27.TextAlign = ContentAlignment.MiddleLeft;
			this.label26.Anchor = AnchorStyles.Left;
			this.label26.AutoSize = true;
			this.label26.Location = new Point(3, 238);
			this.label26.Name = "label26";
			this.label26.Size = new Size(59, 12);
			this.label26.TabIndex = 16;
			this.label26.Text = "Tx start:";
			this.label26.TextAlign = ContentAlignment.MiddleLeft;
			this.pnlAesEncryption.Anchor = AnchorStyles.Left;
			this.pnlAesEncryption.AutoSize = true;
			this.pnlAesEncryption.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlAesEncryption.Controls.Add((Control)this.rBtnAesOff);
			this.pnlAesEncryption.Controls.Add((Control)this.rBtnAesOn);
			this.pnlAesEncryption.Location = new Point(152, 190);
			this.pnlAesEncryption.Margin = new Padding(3, 2, 3, 2);
			this.pnlAesEncryption.Name = "pnlAesEncryption";
			this.pnlAesEncryption.Size = new Size(98, 19);
			this.pnlAesEncryption.TabIndex = 13;
			this.pnlAesEncryption.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlAesEncryption.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnAesOff.AutoSize = true;
			this.rBtnAesOff.Location = new Point(54, 3);
			this.rBtnAesOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAesOff.Name = "rBtnAesOff";
			this.rBtnAesOff.Size = new Size(41, 16);
			this.rBtnAesOff.TabIndex = 1;
			this.rBtnAesOff.Text = "OFF";
			this.rBtnAesOff.UseVisualStyleBackColor = true;
			this.rBtnAesOff.CheckedChanged += new EventHandler(this.rBtnAesOff_CheckedChanged);
			this.rBtnAesOff.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnAesOff.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnAesOn.AutoSize = true;
			this.rBtnAesOn.Checked = true;
			this.rBtnAesOn.Location = new Point(3, 3);
			this.rBtnAesOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAesOn.Name = "rBtnAesOn";
			this.rBtnAesOn.Size = new Size(35, 16);
			this.rBtnAesOn.TabIndex = 0;
			this.rBtnAesOn.TabStop = true;
			this.rBtnAesOn.Text = "ON";
			this.rBtnAesOn.UseVisualStyleBackColor = true;
			this.rBtnAesOn.CheckedChanged += new EventHandler(this.rBtnAesOn_CheckedChanged);
			this.rBtnAesOn.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnAesOn.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.pnlDcFree.Anchor = AnchorStyles.Left;
			this.pnlDcFree.AutoSize = true;
			this.pnlDcFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlDcFree.Controls.Add((Control)this.rBtnDcFreeWhitening);
			this.pnlDcFree.Controls.Add((Control)this.rBtnDcFreeManchester);
			this.pnlDcFree.Controls.Add((Control)this.rBtnDcFreeOff);
			this.pnlDcFree.Location = new Point(152, 98);
			this.pnlDcFree.Margin = new Padding(3, 2, 3, 2);
			this.pnlDcFree.Name = "pnlDcFree";
			this.pnlDcFree.Size = new Size(221, 19);
			this.pnlDcFree.TabIndex = 7;
			this.pnlDcFree.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlDcFree.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnDcFreeWhitening.AutoSize = true;
			this.rBtnDcFreeWhitening.Location = new Point(141, 3);
			this.rBtnDcFreeWhitening.Margin = new Padding(3, 0, 3, 0);
			this.rBtnDcFreeWhitening.Name = "rBtnDcFreeWhitening";
			this.rBtnDcFreeWhitening.Size = new Size(77, 16);
			this.rBtnDcFreeWhitening.TabIndex = 2;
			this.rBtnDcFreeWhitening.Text = "Whitening";
			this.rBtnDcFreeWhitening.UseVisualStyleBackColor = true;
			this.rBtnDcFreeWhitening.CheckedChanged += new EventHandler(this.rBtnDcFreeWhitening_CheckedChanged);
			this.rBtnDcFreeWhitening.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnDcFreeWhitening.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnDcFreeManchester.AutoSize = true;
			this.rBtnDcFreeManchester.Location = new Point(54, 3);
			this.rBtnDcFreeManchester.Margin = new Padding(3, 0, 3, 0);
			this.rBtnDcFreeManchester.Name = "rBtnDcFreeManchester";
			this.rBtnDcFreeManchester.Size = new Size(83, 16);
			this.rBtnDcFreeManchester.TabIndex = 1;
			this.rBtnDcFreeManchester.Text = "Manchester";
			this.rBtnDcFreeManchester.UseVisualStyleBackColor = true;
			this.rBtnDcFreeManchester.CheckedChanged += new EventHandler(this.rBtnDcFreeManchester_CheckedChanged);
			this.rBtnDcFreeManchester.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnDcFreeManchester.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnDcFreeOff.AutoSize = true;
			this.rBtnDcFreeOff.Checked = true;
			this.rBtnDcFreeOff.Location = new Point(3, 3);
			this.rBtnDcFreeOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnDcFreeOff.Name = "rBtnDcFreeOff";
			this.rBtnDcFreeOff.Size = new Size(41, 16);
			this.rBtnDcFreeOff.TabIndex = 0;
			this.rBtnDcFreeOff.TabStop = true;
			this.rBtnDcFreeOff.Text = "OFF";
			this.rBtnDcFreeOff.UseVisualStyleBackColor = true;
			this.rBtnDcFreeOff.CheckedChanged += new EventHandler(this.rBtnDcFreeOff_CheckedChanged);
			this.rBtnDcFreeOff.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnDcFreeOff.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.pnlAddressInPayload.Anchor = AnchorStyles.Left;
			this.pnlAddressInPayload.AutoSize = true;
			this.pnlAddressInPayload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlAddressInPayload.Controls.Add((Control)this.rBtnNodeAddressInPayloadNo);
			this.pnlAddressInPayload.Controls.Add((Control)this.rBtnNodeAddressInPayloadYes);
			this.pnlAddressInPayload.Location = new Point(152, 2);
			this.pnlAddressInPayload.Margin = new Padding(3, 2, 3, 2);
			this.pnlAddressInPayload.Name = "pnlAddressInPayload";
			this.pnlAddressInPayload.Size = new Size(92, 19);
			this.pnlAddressInPayload.TabIndex = 1;
			this.pnlAddressInPayload.Visible = false;
			this.pnlAddressInPayload.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlAddressInPayload.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnNodeAddressInPayloadNo.AutoSize = true;
			this.rBtnNodeAddressInPayloadNo.Location = new Point(54, 3);
			this.rBtnNodeAddressInPayloadNo.Margin = new Padding(3, 0, 3, 0);
			this.rBtnNodeAddressInPayloadNo.Name = "rBtnNodeAddressInPayloadNo";
			this.rBtnNodeAddressInPayloadNo.Size = new Size(35, 16);
			this.rBtnNodeAddressInPayloadNo.TabIndex = 1;
			this.rBtnNodeAddressInPayloadNo.Text = "NO";
			this.rBtnNodeAddressInPayloadNo.UseVisualStyleBackColor = true;
			this.rBtnNodeAddressInPayloadNo.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnNodeAddressInPayloadNo.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnNodeAddressInPayloadYes.AutoSize = true;
			this.rBtnNodeAddressInPayloadYes.Checked = true;
			this.rBtnNodeAddressInPayloadYes.Location = new Point(3, 3);
			this.rBtnNodeAddressInPayloadYes.Margin = new Padding(3, 0, 3, 0);
			this.rBtnNodeAddressInPayloadYes.Name = "rBtnNodeAddressInPayloadYes";
			this.rBtnNodeAddressInPayloadYes.Size = new Size(41, 16);
			this.rBtnNodeAddressInPayloadYes.TabIndex = 0;
			this.rBtnNodeAddressInPayloadYes.TabStop = true;
			this.rBtnNodeAddressInPayloadYes.Text = "YES";
			this.rBtnNodeAddressInPayloadYes.UseVisualStyleBackColor = true;
			this.rBtnNodeAddressInPayloadYes.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnNodeAddressInPayloadYes.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label17.Anchor = AnchorStyles.Left;
			this.label17.AutoSize = true;
			this.label17.Location = new Point(3, 5);
			this.label17.Name = "label17";
			this.label17.Size = new Size(143, 12);
			this.label17.TabIndex = 0;
			this.label17.Text = "Add address in payload:";
			this.label17.TextAlign = ContentAlignment.MiddleLeft;
			this.label17.Visible = false;
			this.pnlFifoFillCondition.Anchor = AnchorStyles.Left;
			this.pnlFifoFillCondition.AutoSize = true;
			this.pnlFifoFillCondition.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlFifoFillCondition.Controls.Add((Control)this.rBtnFifoFillAlways);
			this.pnlFifoFillCondition.Controls.Add((Control)this.rBtnFifoFillSyncAddress);
			this.pnlFifoFillCondition.Location = new Point(163, 50);
			this.pnlFifoFillCondition.Margin = new Padding(3, 2, 3, 2);
			this.pnlFifoFillCondition.Name = "pnlFifoFillCondition";
			this.pnlFifoFillCondition.Size = new Size(160, 19);
			this.pnlFifoFillCondition.TabIndex = 6;
			this.pnlFifoFillCondition.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlFifoFillCondition.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnFifoFillAlways.AutoSize = true;
			this.rBtnFifoFillAlways.Location = new Point(98, 3);
			this.rBtnFifoFillAlways.Margin = new Padding(3, 0, 3, 0);
			this.rBtnFifoFillAlways.Name = "rBtnFifoFillAlways";
			this.rBtnFifoFillAlways.Size = new Size(59, 16);
			this.rBtnFifoFillAlways.TabIndex = 1;
			this.rBtnFifoFillAlways.Text = "Always";
			this.rBtnFifoFillAlways.UseVisualStyleBackColor = true;
			this.rBtnFifoFillAlways.CheckedChanged += new EventHandler(this.rBtnFifoFill_CheckedChanged);
			this.rBtnFifoFillAlways.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnFifoFillAlways.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnFifoFillSyncAddress.AutoSize = true;
			this.rBtnFifoFillSyncAddress.Checked = true;
			this.rBtnFifoFillSyncAddress.Location = new Point(3, 3);
			this.rBtnFifoFillSyncAddress.Margin = new Padding(3, 0, 3, 0);
			this.rBtnFifoFillSyncAddress.Name = "rBtnFifoFillSyncAddress";
			this.rBtnFifoFillSyncAddress.Size = new Size(95, 16);
			this.rBtnFifoFillSyncAddress.TabIndex = 0;
			this.rBtnFifoFillSyncAddress.TabStop = true;
			this.rBtnFifoFillSyncAddress.Text = "Sync address";
			this.rBtnFifoFillSyncAddress.UseVisualStyleBackColor = true;
			this.rBtnFifoFillSyncAddress.CheckedChanged += new EventHandler(this.rBtnFifoFill_CheckedChanged);
			this.rBtnFifoFillSyncAddress.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnFifoFillSyncAddress.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label4.Anchor = AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(3, 53);
			this.label4.Name = "label4";
			this.label4.Size = new Size(125, 12);
			this.label4.TabIndex = 5;
			this.label4.Text = "FIFO fill condition:";
			this.label4.TextAlign = ContentAlignment.MiddleLeft;
			this.pnlSync.Anchor = AnchorStyles.Left;
			this.pnlSync.AutoSize = true;
			this.pnlSync.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlSync.Controls.Add((Control)this.rBtnSyncOff);
			this.pnlSync.Controls.Add((Control)this.rBtnSyncOn);
			this.pnlSync.Location = new Point(163, 27);
			this.pnlSync.Margin = new Padding(3, 2, 3, 2);
			this.pnlSync.Name = "pnlSync";
			this.pnlSync.Size = new Size(94, 19);
			this.pnlSync.TabIndex = 4;
			this.pnlSync.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlSync.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnSyncOff.AutoSize = true;
			this.rBtnSyncOff.Location = new Point(50, 3);
			this.rBtnSyncOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnSyncOff.Name = "rBtnSyncOff";
			this.rBtnSyncOff.Size = new Size(41, 16);
			this.rBtnSyncOff.TabIndex = 1;
			this.rBtnSyncOff.Text = "OFF";
			this.rBtnSyncOff.UseVisualStyleBackColor = true;
			this.rBtnSyncOff.CheckedChanged += new EventHandler(this.rBtnSyncOn_CheckedChanged);
			this.rBtnSyncOff.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnSyncOff.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnSyncOn.AutoSize = true;
			this.rBtnSyncOn.Checked = true;
			this.rBtnSyncOn.Location = new Point(3, 3);
			this.rBtnSyncOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnSyncOn.Name = "rBtnSyncOn";
			this.rBtnSyncOn.Size = new Size(35, 16);
			this.rBtnSyncOn.TabIndex = 0;
			this.rBtnSyncOn.TabStop = true;
			this.rBtnSyncOn.Text = "ON";
			this.rBtnSyncOn.UseVisualStyleBackColor = true;
			this.rBtnSyncOn.CheckedChanged += new EventHandler(this.rBtnSyncOn_CheckedChanged);
			this.rBtnSyncOn.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnSyncOn.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label3.Anchor = AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(3, 30);
			this.label3.Name = "label3";
			this.label3.Size = new Size(65, 12);
			this.label3.TabIndex = 3;
			this.label3.Text = "Sync word:";
			this.label3.TextAlign = ContentAlignment.MiddleLeft;
			this.label9.Anchor = AnchorStyles.Left;
			this.label9.AutoSize = true;
			this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.label9.Location = new Point(3, (int)sbyte.MaxValue);
			this.label9.Name = "label9";
			this.label9.Size = new Size(89, 13);
			this.label9.TabIndex = 13;
			this.label9.Text = "Sync word value:";
			this.label9.TextAlign = ContentAlignment.MiddleLeft;
			this.pnlCrcAutoClear.Anchor = AnchorStyles.Left;
			this.pnlCrcAutoClear.AutoSize = true;
			this.pnlCrcAutoClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlCrcAutoClear.Controls.Add((Control)this.rBtnCrcAutoClearOff);
			this.pnlCrcAutoClear.Controls.Add((Control)this.rBtnCrcAutoClearOn);
			this.pnlCrcAutoClear.Location = new Point(152, 144);
			this.pnlCrcAutoClear.Margin = new Padding(3, 2, 3, 2);
			this.pnlCrcAutoClear.Name = "pnlCrcAutoClear";
			this.pnlCrcAutoClear.Size = new Size(98, 19);
			this.pnlCrcAutoClear.TabIndex = 11;
			this.pnlCrcAutoClear.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlCrcAutoClear.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnCrcAutoClearOff.AutoSize = true;
			this.rBtnCrcAutoClearOff.Location = new Point(54, 3);
			this.rBtnCrcAutoClearOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnCrcAutoClearOff.Name = "rBtnCrcAutoClearOff";
			this.rBtnCrcAutoClearOff.Size = new Size(41, 16);
			this.rBtnCrcAutoClearOff.TabIndex = 1;
			this.rBtnCrcAutoClearOff.Text = "OFF";
			this.rBtnCrcAutoClearOff.UseVisualStyleBackColor = true;
			this.rBtnCrcAutoClearOff.CheckedChanged += new EventHandler(this.rBtnCrcAutoClearOff_CheckedChanged);
			this.rBtnCrcAutoClearOff.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnCrcAutoClearOff.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnCrcAutoClearOn.AutoSize = true;
			this.rBtnCrcAutoClearOn.Checked = true;
			this.rBtnCrcAutoClearOn.Location = new Point(3, 3);
			this.rBtnCrcAutoClearOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnCrcAutoClearOn.Name = "rBtnCrcAutoClearOn";
			this.rBtnCrcAutoClearOn.Size = new Size(35, 16);
			this.rBtnCrcAutoClearOn.TabIndex = 0;
			this.rBtnCrcAutoClearOn.TabStop = true;
			this.rBtnCrcAutoClearOn.Text = "ON";
			this.rBtnCrcAutoClearOn.UseVisualStyleBackColor = true;
			this.rBtnCrcAutoClearOn.CheckedChanged += new EventHandler(this.rBtnCrcAutoClearOn_CheckedChanged);
			this.rBtnCrcAutoClearOn.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnCrcAutoClearOn.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label23.Anchor = AnchorStyles.Left;
			this.label23.AutoSize = true;
			this.label23.Location = new Point(3, 147);
			this.label23.Name = "label23";
			this.label23.Size = new Size(95, 12);
			this.label23.TabIndex = 10;
			this.label23.Text = "CRC auto clear:";
			this.label23.TextAlign = ContentAlignment.MiddleLeft;
			this.pnlCrcCalculation.Anchor = AnchorStyles.Left;
			this.pnlCrcCalculation.AutoSize = true;
			this.pnlCrcCalculation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlCrcCalculation.Controls.Add((Control)this.rBtnCrcOff);
			this.pnlCrcCalculation.Controls.Add((Control)this.rBtnCrcOn);
			this.pnlCrcCalculation.Location = new Point(152, 121);
			this.pnlCrcCalculation.Margin = new Padding(3, 2, 3, 2);
			this.pnlCrcCalculation.Name = "pnlCrcCalculation";
			this.pnlCrcCalculation.Size = new Size(98, 19);
			this.pnlCrcCalculation.TabIndex = 9;
			this.pnlCrcCalculation.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlCrcCalculation.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnCrcOff.AutoSize = true;
			this.rBtnCrcOff.Location = new Point(54, 3);
			this.rBtnCrcOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnCrcOff.Name = "rBtnCrcOff";
			this.rBtnCrcOff.Size = new Size(41, 16);
			this.rBtnCrcOff.TabIndex = 1;
			this.rBtnCrcOff.Text = "OFF";
			this.rBtnCrcOff.UseVisualStyleBackColor = true;
			this.rBtnCrcOff.CheckedChanged += new EventHandler(this.rBtnCrcOff_CheckedChanged);
			this.rBtnCrcOff.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnCrcOff.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnCrcOn.AutoSize = true;
			this.rBtnCrcOn.Checked = true;
			this.rBtnCrcOn.Location = new Point(3, 3);
			this.rBtnCrcOn.Margin = new Padding(3, 0, 3, 0);
			this.rBtnCrcOn.Name = "rBtnCrcOn";
			this.rBtnCrcOn.Size = new Size(35, 16);
			this.rBtnCrcOn.TabIndex = 0;
			this.rBtnCrcOn.TabStop = true;
			this.rBtnCrcOn.Text = "ON";
			this.rBtnCrcOn.UseVisualStyleBackColor = true;
			this.rBtnCrcOn.CheckedChanged += new EventHandler(this.rBtnCrcOn_CheckedChanged);
			this.rBtnCrcOn.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnCrcOn.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label22.Anchor = AnchorStyles.Left;
			this.label22.AutoSize = true;
			this.label22.Location = new Point(3, 124);
			this.label22.Name = "label22";
			this.label22.Size = new Size(101, 12);
			this.label22.TabIndex = 8;
			this.label22.Text = "CRC calculation:";
			this.label22.TextAlign = ContentAlignment.MiddleLeft;
			this.pnlTxStart.Anchor = AnchorStyles.Left;
			this.pnlTxStart.AutoSize = true;
			this.pnlTxStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlTxStart.Controls.Add((Control)this.rBtnTxStartFifoNotEmpty);
			this.pnlTxStart.Controls.Add((Control)this.rBtnTxStartFifoLevel);
			this.pnlTxStart.Location = new Point(152, 235);
			this.pnlTxStart.Margin = new Padding(3, 3, 3, 2);
			this.pnlTxStart.Name = "pnlTxStart";
			this.pnlTxStart.Size = new Size(175, 19);
			this.pnlTxStart.TabIndex = 17;
			this.pnlTxStart.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlTxStart.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnTxStartFifoNotEmpty.AutoSize = true;
			this.rBtnTxStartFifoNotEmpty.Checked = true;
			this.rBtnTxStartFifoNotEmpty.Location = new Point(77, 3);
			this.rBtnTxStartFifoNotEmpty.Margin = new Padding(3, 0, 3, 0);
			this.rBtnTxStartFifoNotEmpty.Name = "rBtnTxStartFifoNotEmpty";
			this.rBtnTxStartFifoNotEmpty.Size = new Size(95, 16);
			this.rBtnTxStartFifoNotEmpty.TabIndex = 1;
			this.rBtnTxStartFifoNotEmpty.TabStop = true;
			this.rBtnTxStartFifoNotEmpty.Text = "FifoNotEmpty";
			this.rBtnTxStartFifoNotEmpty.UseVisualStyleBackColor = true;
			this.rBtnTxStartFifoNotEmpty.CheckedChanged += new EventHandler(this.rBtnTxStartFifoNotEmpty_CheckedChanged);
			this.rBtnTxStartFifoNotEmpty.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnTxStartFifoNotEmpty.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnTxStartFifoLevel.AutoSize = true;
			this.rBtnTxStartFifoLevel.Location = new Point(3, 3);
			this.rBtnTxStartFifoLevel.Margin = new Padding(3, 0, 3, 0);
			this.rBtnTxStartFifoLevel.Name = "rBtnTxStartFifoLevel";
			this.rBtnTxStartFifoLevel.Size = new Size(77, 16);
			this.rBtnTxStartFifoLevel.TabIndex = 0;
			this.rBtnTxStartFifoLevel.Text = "FifoLevel";
			this.rBtnTxStartFifoLevel.UseVisualStyleBackColor = true;
			this.rBtnTxStartFifoLevel.CheckedChanged += new EventHandler(this.rBtnTxStartFifoLevel_CheckedChanged);
			this.rBtnTxStartFifoLevel.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnTxStartFifoLevel.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.pnlAddressFiltering.Anchor = AnchorStyles.Left;
			this.pnlAddressFiltering.AutoSize = true;
			this.pnlAddressFiltering.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlAddressFiltering.Controls.Add((Control)this.rBtnAddressFilteringNodeBroadcast);
			this.pnlAddressFiltering.Controls.Add((Control)this.rBtnAddressFilteringNode);
			this.pnlAddressFiltering.Controls.Add((Control)this.rBtnAddressFilteringOff);
			this.pnlAddressFiltering.Location = new Point(152, 25);
			this.pnlAddressFiltering.Margin = new Padding(3, 2, 3, 2);
			this.pnlAddressFiltering.Name = "pnlAddressFiltering";
			this.pnlAddressFiltering.Size = new Size(239, 19);
			this.pnlAddressFiltering.TabIndex = 3;
			this.pnlAddressFiltering.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlAddressFiltering.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnAddressFilteringNodeBroadcast.AutoSize = true;
			this.rBtnAddressFilteringNodeBroadcast.Location = new Point(111, 3);
			this.rBtnAddressFilteringNodeBroadcast.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAddressFilteringNodeBroadcast.Name = "rBtnAddressFilteringNodeBroadcast";
			this.rBtnAddressFilteringNodeBroadcast.Size = new Size(125, 16);
			this.rBtnAddressFilteringNodeBroadcast.TabIndex = 2;
			this.rBtnAddressFilteringNodeBroadcast.Text = "Node or Broadcast";
			this.rBtnAddressFilteringNodeBroadcast.UseVisualStyleBackColor = true;
			this.rBtnAddressFilteringNodeBroadcast.CheckedChanged += new EventHandler(this.rBtnAddressFilteringNodeBroadcast_CheckedChanged);
			this.rBtnAddressFilteringNodeBroadcast.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnAddressFilteringNodeBroadcast.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnAddressFilteringNode.AutoSize = true;
			this.rBtnAddressFilteringNode.Location = new Point(54, 3);
			this.rBtnAddressFilteringNode.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAddressFilteringNode.Name = "rBtnAddressFilteringNode";
			this.rBtnAddressFilteringNode.Size = new Size(47, 16);
			this.rBtnAddressFilteringNode.TabIndex = 1;
			this.rBtnAddressFilteringNode.Text = "Node";
			this.rBtnAddressFilteringNode.UseVisualStyleBackColor = true;
			this.rBtnAddressFilteringNode.CheckedChanged += new EventHandler(this.rBtnAddressFilteringNode_CheckedChanged);
			this.rBtnAddressFilteringNode.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnAddressFilteringNode.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnAddressFilteringOff.AutoSize = true;
			this.rBtnAddressFilteringOff.Checked = true;
			this.rBtnAddressFilteringOff.Location = new Point(3, 3);
			this.rBtnAddressFilteringOff.Margin = new Padding(3, 0, 3, 0);
			this.rBtnAddressFilteringOff.Name = "rBtnAddressFilteringOff";
			this.rBtnAddressFilteringOff.Size = new Size(41, 16);
			this.rBtnAddressFilteringOff.TabIndex = 0;
			this.rBtnAddressFilteringOff.TabStop = true;
			this.rBtnAddressFilteringOff.Text = "OFF";
			this.rBtnAddressFilteringOff.UseVisualStyleBackColor = true;
			this.rBtnAddressFilteringOff.CheckedChanged += new EventHandler(this.rBtnAddressFilteringOff_CheckedChanged);
			this.rBtnAddressFilteringOff.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnAddressFilteringOff.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.lblNodeAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblNodeAddress.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.lblNodeAddress.Location = new Point(65, 0);
			this.lblNodeAddress.Name = "lblNodeAddress";
			this.lblNodeAddress.Size = new Size(59, 18);
			this.lblNodeAddress.TabIndex = 1;
			this.lblNodeAddress.Text = "0x00";
			this.lblNodeAddress.TextAlign = ContentAlignment.MiddleCenter;
			this.lblNodeAddress.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.lblNodeAddress.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.lblPayloadLength.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblPayloadLength.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.lblPayloadLength.Location = new Point(65, 0);
			this.lblPayloadLength.Name = "lblPayloadLength";
			this.lblPayloadLength.Size = new Size(59, 18);
			this.lblPayloadLength.TabIndex = 1;
			this.lblPayloadLength.Text = "0x00";
			this.lblPayloadLength.TextAlign = ContentAlignment.MiddleCenter;
			this.lblPayloadLength.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.lblPayloadLength.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.lblBroadcastAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblBroadcastAddress.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.lblBroadcastAddress.Location = new Point(65, 0);
			this.lblBroadcastAddress.Name = "lblBroadcastAddress";
			this.lblBroadcastAddress.Size = new Size(59, 18);
			this.lblBroadcastAddress.TabIndex = 1;
			this.lblBroadcastAddress.Text = "0x00";
			this.lblBroadcastAddress.TextAlign = ContentAlignment.MiddleCenter;
			this.lblBroadcastAddress.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.lblBroadcastAddress.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.pnlPacketFormat.Anchor = AnchorStyles.Left;
			this.pnlPacketFormat.AutoSize = true;
			this.pnlPacketFormat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlPacketFormat.Controls.Add((Control)this.rBtnPacketFormatFixed);
			this.pnlPacketFormat.Controls.Add((Control)this.rBtnPacketFormatVariable);
			this.pnlPacketFormat.Location = new Point(163, 148);
			this.pnlPacketFormat.Margin = new Padding(3, 2, 3, 2);
			this.pnlPacketFormat.Name = "pnlPacketFormat";
			this.pnlPacketFormat.Size = new Size(128, 19);
			this.pnlPacketFormat.TabIndex = 16;
			this.pnlPacketFormat.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlPacketFormat.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnPacketFormatFixed.AutoSize = true;
			this.rBtnPacketFormatFixed.Location = new Point(72, 3);
			this.rBtnPacketFormatFixed.Margin = new Padding(3, 0, 3, 0);
			this.rBtnPacketFormatFixed.Name = "rBtnPacketFormatFixed";
			this.rBtnPacketFormatFixed.Size = new Size(53, 16);
			this.rBtnPacketFormatFixed.TabIndex = 1;
			this.rBtnPacketFormatFixed.Text = "Fixed";
			this.rBtnPacketFormatFixed.UseVisualStyleBackColor = true;
			this.rBtnPacketFormatFixed.CheckedChanged += new EventHandler(this.rBtnPacketFormat_CheckedChanged);
			this.rBtnPacketFormatFixed.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnPacketFormatFixed.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnPacketFormatVariable.AutoSize = true;
			this.rBtnPacketFormatVariable.Checked = true;
			this.rBtnPacketFormatVariable.Location = new Point(3, 3);
			this.rBtnPacketFormatVariable.Margin = new Padding(3, 0, 3, 0);
			this.rBtnPacketFormatVariable.Name = "rBtnPacketFormatVariable";
			this.rBtnPacketFormatVariable.Size = new Size(71, 16);
			this.rBtnPacketFormatVariable.TabIndex = 0;
			this.rBtnPacketFormatVariable.TabStop = true;
			this.rBtnPacketFormatVariable.Text = "Variable";
			this.rBtnPacketFormatVariable.UseVisualStyleBackColor = true;
			this.rBtnPacketFormatVariable.CheckedChanged += new EventHandler(this.rBtnPacketFormat_CheckedChanged);
			this.rBtnPacketFormatVariable.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnPacketFormatVariable.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			this.tableLayoutPanel1.Controls.Add((Control)this.pnlPayloadLength, 1, 7);
			this.tableLayoutPanel1.Controls.Add((Control)this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add((Control)this.pnlPacketFormat, 1, 6);
			this.tableLayoutPanel1.Controls.Add((Control)this.label3, 0, 1);
			this.tableLayoutPanel1.Controls.Add((Control)this.label4, 0, 2);
			this.tableLayoutPanel1.Controls.Add((Control)this.label5, 0, 3);
			this.tableLayoutPanel1.Controls.Add((Control)this.label7, 0, 4);
			this.tableLayoutPanel1.Controls.Add((Control)this.label9, 0, 5);
			this.tableLayoutPanel1.Controls.Add((Control)this.label10, 0, 6);
			this.tableLayoutPanel1.Controls.Add((Control)this.label11, 0, 7);
			this.tableLayoutPanel1.Controls.Add((Control)this.pnlFifoFillCondition, 1, 2);
			this.tableLayoutPanel1.Controls.Add((Control)this.pnlSync, 1, 1);
			this.tableLayoutPanel1.Controls.Add((Control)this.cBoxEnterCondition, 1, 8);
			this.tableLayoutPanel1.Controls.Add((Control)this.cBoxExitCondition, 1, 9);
			this.tableLayoutPanel1.Controls.Add((Control)this.tBoxSyncValue, 1, 5);
			this.tableLayoutPanel1.Controls.Add((Control)this.label12, 2, 7);
			this.tableLayoutPanel1.Controls.Add((Control)this.label14, 0, 8);
			this.tableLayoutPanel1.Controls.Add((Control)this.label15, 0, 9);
			this.tableLayoutPanel1.Controls.Add((Control)this.label16, 0, 10);
			this.tableLayoutPanel1.Controls.Add((Control)this.cBoxIntermediateMode, 1, 10);
			this.tableLayoutPanel1.Controls.Add((Control)this.nudPreambleSize, 1, 0);
			this.tableLayoutPanel1.Controls.Add((Control)this.label2, 2, 0);
			this.tableLayoutPanel1.Controls.Add((Control)this.nudSyncSize, 1, 3);
			this.tableLayoutPanel1.Controls.Add((Control)this.label6, 2, 3);
			this.tableLayoutPanel1.Controls.Add((Control)this.nudSyncTol, 1, 4);
			this.tableLayoutPanel1.Controls.Add((Control)this.label8, 2, 4);
			this.tableLayoutPanel1.Location = new Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 11;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.Size = new Size(379, 266);
			this.tableLayoutPanel1.TabIndex = 0;
			this.pnlPayloadLength.Anchor = AnchorStyles.Left;
			this.pnlPayloadLength.AutoSize = true;
			this.pnlPayloadLength.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlPayloadLength.Controls.Add((Control)this.lblPayloadLength);
			this.pnlPayloadLength.Controls.Add((Control)this.nudPayloadLength);
			this.pnlPayloadLength.Location = new Point(163, 171);
			this.pnlPayloadLength.Margin = new Padding(3, 2, 3, 2);
			this.pnlPayloadLength.Name = "pnlPayloadLength";
			this.pnlPayloadLength.Size = new Size((int)sbyte.MaxValue, 21);
			this.pnlPayloadLength.TabIndex = 18;
			this.pnlPayloadLength.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlPayloadLength.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudPayloadLength.Location = new Point(3, 0);
			this.nudPayloadLength.Margin = new Padding(3, 0, 3, 0);
			int[] bits3 = new int[4];
			bits3[0] = 66;
			Decimal num3 = new Decimal(bits3);
			this.nudPayloadLength.Maximum = num3;
			this.nudPayloadLength.Name = "nudPayloadLength";
			this.nudPayloadLength.Size = new Size(59, 21);
			this.nudPayloadLength.TabIndex = 0;
			int[] bits4 = new int[4];
			bits4[0] = 66;
			Decimal num4 = new Decimal(bits4);
			this.nudPayloadLength.Value = num4;
			this.nudPayloadLength.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.nudPayloadLength.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudPayloadLength.ValueChanged += new EventHandler(this.nudPayloadLength_ValueChanged);
			this.nudSyncSize.Anchor = AnchorStyles.Left;
			this.nudSyncSize.Location = new Point(163, 73);
			this.nudSyncSize.Margin = new Padding(3, 2, 3, 2);
			int[] bits5 = new int[4];
			bits5[0] = 8;
			Decimal num5 = new Decimal(bits5);
			this.nudSyncSize.Maximum = num5;
			int[] bits6 = new int[4];
			bits6[0] = 1;
			Decimal num6 = new Decimal(bits6);
			this.nudSyncSize.Minimum = num6;
			this.nudSyncSize.Name = "nudSyncSize";
			this.nudSyncSize.Size = new Size(59, 21);
			this.nudSyncSize.TabIndex = 8;
			int[] bits7 = new int[4];
			bits7[0] = 4;
			Decimal num7 = new Decimal(bits7);
			this.nudSyncSize.Value = num7;
			this.nudSyncSize.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.nudSyncSize.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudSyncSize.ValueChanged += new EventHandler(this.nudSyncSize_ValueChanged);
			this.nudSyncTol.Anchor = AnchorStyles.Left;
			this.nudSyncTol.Location = new Point(163, 98);
			this.nudSyncTol.Margin = new Padding(3, 2, 3, 2);
			int[] bits8 = new int[4];
			bits8[0] = 7;
			Decimal num8 = new Decimal(bits8);
			this.nudSyncTol.Maximum = num8;
			this.nudSyncTol.Name = "nudSyncTol";
			this.nudSyncTol.Size = new Size(59, 21);
			this.nudSyncTol.TabIndex = 11;
			this.nudSyncTol.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.nudSyncTol.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudSyncTol.ValueChanged += new EventHandler(this.nudSyncTol_ValueChanged);
			this.pnlNodeAddress.Anchor = AnchorStyles.Left;
			this.pnlNodeAddress.AutoSize = true;
			this.pnlNodeAddress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlNodeAddress.Controls.Add((Control)this.nudNodeAddress);
			this.pnlNodeAddress.Controls.Add((Control)this.lblNodeAddress);
			this.pnlNodeAddress.Location = new Point(152, 48);
			this.pnlNodeAddress.Margin = new Padding(3, 2, 3, 2);
			this.pnlNodeAddress.Name = "pnlNodeAddress";
			this.pnlNodeAddress.Size = new Size((int)sbyte.MaxValue, 21);
			this.pnlNodeAddress.TabIndex = 59;
			this.pnlNodeAddress.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlNodeAddress.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudNodeAddress.Location = new Point(0, 0);
			this.nudNodeAddress.Margin = new Padding(3, 0, 3, 0);
			int[] bits9 = new int[4];
			bits9[0] = (int)byte.MaxValue;
			Decimal num9 = new Decimal(bits9);
			this.nudNodeAddress.Maximum = num9;
			this.nudNodeAddress.Name = "nudNodeAddress";
			this.nudNodeAddress.Size = new Size(59, 21);
			this.nudNodeAddress.TabIndex = 0;
			this.nudNodeAddress.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.nudNodeAddress.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudNodeAddress.ValueChanged += new EventHandler(this.nudNodeAddress_ValueChanged);
			this.pnlBroadcastAddress.Anchor = AnchorStyles.Left;
			this.pnlBroadcastAddress.AutoSize = true;
			this.pnlBroadcastAddress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlBroadcastAddress.Controls.Add((Control)this.nudBroadcastAddress);
			this.pnlBroadcastAddress.Controls.Add((Control)this.lblBroadcastAddress);
			this.pnlBroadcastAddress.Location = new Point(152, 73);
			this.pnlBroadcastAddress.Margin = new Padding(3, 2, 3, 2);
			this.pnlBroadcastAddress.Name = "pnlBroadcastAddress";
			this.pnlBroadcastAddress.Size = new Size((int)sbyte.MaxValue, 21);
			this.pnlBroadcastAddress.TabIndex = 60;
			this.pnlBroadcastAddress.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlBroadcastAddress.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudBroadcastAddress.Location = new Point(0, 0);
			this.nudBroadcastAddress.Margin = new Padding(3, 0, 3, 0);
			int[] bits10 = new int[4];
			bits10[0] = (int)byte.MaxValue;
			Decimal num10 = new Decimal(bits10);
			this.nudBroadcastAddress.Maximum = num10;
			this.nudBroadcastAddress.Name = "nudBroadcastAddress";
			this.nudBroadcastAddress.Size = new Size(59, 21);
			this.nudBroadcastAddress.TabIndex = 0;
			this.nudBroadcastAddress.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.nudBroadcastAddress.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudBroadcastAddress.ValueChanged += new EventHandler(this.nudBroadcastAddress_ValueChanged);
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			this.tableLayoutPanel2.Controls.Add((Control)this.label13, 0, 7);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlCrcPolynom, 1, 7);
			this.tableLayoutPanel2.Controls.Add((Control)this.label17, 0, 0);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlBroadcastAddress, 1, 3);
			this.tableLayoutPanel2.Controls.Add((Control)this.lblInterPacketRxDelayUnit, 2, 12);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlTxStart, 1, 10);
			this.tableLayoutPanel2.Controls.Add((Control)this.label18, 0, 1);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlAesEncryption, 1, 8);
			this.tableLayoutPanel2.Controls.Add((Control)this.nudFifoThreshold, 1, 11);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlCrcAutoClear, 1, 6);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlNodeAddress, 1, 2);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlCrcCalculation, 1, 5);
			this.tableLayoutPanel2.Controls.Add((Control)this.tBoxAesKey, 1, 9);
			this.tableLayoutPanel2.Controls.Add((Control)this.label19, 0, 2);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlDcFree, 1, 4);
			this.tableLayoutPanel2.Controls.Add((Control)this.label20, 0, 3);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlAddressFiltering, 1, 1);
			this.tableLayoutPanel2.Controls.Add((Control)this.label21, 0, 4);
			this.tableLayoutPanel2.Controls.Add((Control)this.label22, 0, 5);
			this.tableLayoutPanel2.Controls.Add((Control)this.label23, 0, 6);
			this.tableLayoutPanel2.Controls.Add((Control)this.label24, 0, 8);
			this.tableLayoutPanel2.Controls.Add((Control)this.label25, 0, 9);
			this.tableLayoutPanel2.Controls.Add((Control)this.label26, 0, 10);
			this.tableLayoutPanel2.Controls.Add((Control)this.label27, 0, 11);
			this.tableLayoutPanel2.Controls.Add((Control)this.label28, 0, 12);
			this.tableLayoutPanel2.Controls.Add((Control)this.pnlAddressInPayload, 1, 0);
			this.tableLayoutPanel2.Controls.Add((Control)this.cBoxInterPacketRxDelay, 1, 12);
			this.tableLayoutPanel2.Location = new Point(387, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 13;
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel2.Size = new Size(432, 301);
			this.tableLayoutPanel2.TabIndex = 1;
			this.label13.Anchor = AnchorStyles.Left;
			this.label13.AutoSize = true;
			this.label13.Location = new Point(3, 170);
			this.label13.Name = "label13";
			this.label13.Size = new Size(77, 12);
			this.label13.TabIndex = 11;
			this.label13.Text = "CRC polynom:";
			this.label13.TextAlign = ContentAlignment.MiddleLeft;
			this.pnlCrcPolynom.Anchor = AnchorStyles.Left;
			this.pnlCrcPolynom.AutoSize = true;
			this.pnlCrcPolynom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlCrcPolynom.Controls.Add((Control)this.rBtnCrcCcitt);
			this.pnlCrcPolynom.Controls.Add((Control)this.rBtnCrcIbm);
			this.pnlCrcPolynom.Location = new Point(152, 167);
			this.pnlCrcPolynom.Margin = new Padding(3, 2, 3, 2);
			this.pnlCrcPolynom.Name = "pnlCrcPolynom";
			this.pnlCrcPolynom.Size = new Size(109, 19);
			this.pnlCrcPolynom.TabIndex = 61;
			this.pnlCrcPolynom.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlCrcPolynom.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnCrcCcitt.AutoSize = true;
			this.rBtnCrcCcitt.Location = new Point(53, 3);
			this.rBtnCrcCcitt.Margin = new Padding(3, 0, 3, 0);
			this.rBtnCrcCcitt.Name = "rBtnCrcCcitt";
			this.rBtnCrcCcitt.Size = new Size(53, 16);
			this.rBtnCrcCcitt.TabIndex = 1;
			this.rBtnCrcCcitt.Text = "CCITT";
			this.rBtnCrcCcitt.UseVisualStyleBackColor = true;
			this.rBtnCrcCcitt.CheckedChanged += new EventHandler(this.rBtnCrcIbm_CheckedChanged);
			this.rBtnCrcIbm.AutoSize = true;
			this.rBtnCrcIbm.Checked = true;
			this.rBtnCrcIbm.Location = new Point(3, 3);
			this.rBtnCrcIbm.Margin = new Padding(3, 0, 3, 0);
			this.rBtnCrcIbm.Name = "rBtnCrcIbm";
			this.rBtnCrcIbm.Size = new Size(41, 16);
			this.rBtnCrcIbm.TabIndex = 0;
			this.rBtnCrcIbm.TabStop = true;
			this.rBtnCrcIbm.Text = "IBM";
			this.rBtnCrcIbm.UseVisualStyleBackColor = true;
			this.rBtnCrcIbm.CheckedChanged += new EventHandler(this.rBtnCrcIbm_CheckedChanged);
			this.nudFifoThreshold.Anchor = AnchorStyles.Left;
			this.nudFifoThreshold.Location = new Point(152, 256);
			this.nudFifoThreshold.Margin = new Padding(3, 0, 3, 0);
			int[] bits11 = new int[4];
			bits11[0] = 128;
			Decimal num11 = new Decimal(bits11);
			this.nudFifoThreshold.Maximum = num11;
			this.nudFifoThreshold.Name = "nudFifoThreshold";
			this.nudFifoThreshold.Size = new Size(59, 21);
			this.nudFifoThreshold.TabIndex = 19;
			this.nudFifoThreshold.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.nudFifoThreshold.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.nudFifoThreshold.ValueChanged += new EventHandler(this.nudFifoThreshold_ValueChanged);
			this.cBoxInterPacketRxDelay.Anchor = AnchorStyles.Left;
			this.cBoxInterPacketRxDelay.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cBoxInterPacketRxDelay.FormatString = "###0.000";
			this.cBoxInterPacketRxDelay.FormattingEnabled = true;
			this.cBoxInterPacketRxDelay.Items.AddRange(new object[1]
      {
        (object) "0.208"
      });
			this.cBoxInterPacketRxDelay.Location = new Point(152, 279);
			this.cBoxInterPacketRxDelay.Margin = new Padding(3, 2, 3, 2);
			this.cBoxInterPacketRxDelay.Name = "cBoxInterPacketRxDelay";
			this.cBoxInterPacketRxDelay.Size = new Size(93, 20);
			this.cBoxInterPacketRxDelay.TabIndex = 21;
			this.cBoxInterPacketRxDelay.SelectedIndexChanged += new EventHandler(this.nudInterPacketRxDelay_SelectedIndexChanged);
			this.cBoxInterPacketRxDelay.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.cBoxInterPacketRxDelay.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.gBoxDeviceStatus.Controls.Add((Control)this.lblOperatingMode);
			this.gBoxDeviceStatus.Controls.Add((Control)this.label37);
			this.gBoxDeviceStatus.Controls.Add((Control)this.lblBitSynchroniser);
			this.gBoxDeviceStatus.Controls.Add((Control)this.lblDataMode);
			this.gBoxDeviceStatus.Controls.Add((Control)this.label38);
			this.gBoxDeviceStatus.Controls.Add((Control)this.label39);
			this.gBoxDeviceStatus.Location = new Point(565, 293);
			this.gBoxDeviceStatus.Name = "gBoxDeviceStatus";
			this.gBoxDeviceStatus.Size = new Size(231, 71);
			this.gBoxDeviceStatus.TabIndex = 3;
			this.gBoxDeviceStatus.TabStop = false;
			this.gBoxDeviceStatus.Text = "Device status";
			this.gBoxDeviceStatus.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxDeviceStatus.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.lblOperatingMode.AutoSize = true;
			this.lblOperatingMode.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.lblOperatingMode.Location = new Point(146, 54);
			this.lblOperatingMode.Margin = new Padding(3);
			this.lblOperatingMode.Name = "lblOperatingMode";
			this.lblOperatingMode.Size = new Size(39, 13);
			this.lblOperatingMode.TabIndex = 5;
			this.lblOperatingMode.Text = "Sleep";
			this.lblOperatingMode.TextAlign = ContentAlignment.MiddleLeft;
			this.label37.AutoSize = true;
			this.label37.Location = new Point(3, 54);
			this.label37.Margin = new Padding(3);
			this.label37.Name = "label37";
			this.label37.Size = new Size(95, 12);
			this.label37.TabIndex = 4;
			this.label37.Text = "Operating mode:";
			this.label37.TextAlign = ContentAlignment.MiddleLeft;
			this.lblBitSynchroniser.AutoSize = true;
			this.lblBitSynchroniser.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.lblBitSynchroniser.Location = new Point(146, 18);
			this.lblBitSynchroniser.Margin = new Padding(3);
			this.lblBitSynchroniser.Name = "lblBitSynchroniser";
			this.lblBitSynchroniser.Size = new Size(25, 13);
			this.lblBitSynchroniser.TabIndex = 1;
			this.lblBitSynchroniser.Text = "ON";
			this.lblBitSynchroniser.TextAlign = ContentAlignment.MiddleLeft;
			this.lblDataMode.AutoSize = true;
			this.lblDataMode.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.lblDataMode.Location = new Point(146, 36);
			this.lblDataMode.Margin = new Padding(3);
			this.lblDataMode.Name = "lblDataMode";
			this.lblDataMode.Size = new Size(47, 13);
			this.lblDataMode.TabIndex = 3;
			this.lblDataMode.Text = "Packet";
			this.lblDataMode.TextAlign = ContentAlignment.MiddleLeft;
			this.label38.AutoSize = true;
			this.label38.Location = new Point(3, 18);
			this.label38.Margin = new Padding(3);
			this.label38.Name = "label38";
			this.label38.Size = new Size(107, 12);
			this.label38.TabIndex = 0;
			this.label38.Text = "Bit Synchronizer:";
			this.label38.TextAlign = ContentAlignment.MiddleLeft;
			this.label39.AutoSize = true;
			this.label39.Location = new Point(3, 36);
			this.label39.Margin = new Padding(3);
			this.label39.Name = "label39";
			this.label39.Size = new Size(65, 12);
			this.label39.TabIndex = 2;
			this.label39.Text = "Data mode:";
			this.label39.TextAlign = ContentAlignment.MiddleLeft;
			this.gBoxControl.Controls.Add((Control)this.tBoxPacketsNb);
			this.gBoxControl.Controls.Add((Control)this.cBtnLog);
			this.gBoxControl.Controls.Add((Control)this.cBtnPacketHandlerStartStop);
			this.gBoxControl.Controls.Add((Control)this.lblPacketsNb);
			this.gBoxControl.Controls.Add((Control)this.tBoxPacketsRepeatValue);
			this.gBoxControl.Controls.Add((Control)this.lblPacketsRepeatValue);
			this.gBoxControl.Location = new Point(565, 364);
			this.gBoxControl.Name = "gBoxControl";
			this.gBoxControl.Size = new Size(231, 89);
			this.gBoxControl.TabIndex = 4;
			this.gBoxControl.TabStop = false;
			this.gBoxControl.Text = "Control";
			this.gBoxControl.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxControl.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.tBoxPacketsNb.Location = new Point(149, 44);
			this.tBoxPacketsNb.Name = "tBoxPacketsNb";
			this.tBoxPacketsNb.ReadOnly = true;
			this.tBoxPacketsNb.Size = new Size(79, 21);
			this.tBoxPacketsNb.TabIndex = 2;
			this.tBoxPacketsNb.Text = "0";
			this.tBoxPacketsNb.TextAlign = HorizontalAlignment.Right;
			this.cBtnLog.Appearance = Appearance.Button;
			this.cBtnLog.Location = new Point(118, 18);
			this.cBtnLog.Name = "cBtnLog";
			this.cBtnLog.Size = new Size(75, 21);
			this.cBtnLog.TabIndex = 0;
			this.cBtnLog.Text = "Log";
			this.cBtnLog.TextAlign = ContentAlignment.MiddleCenter;
			this.cBtnLog.UseVisualStyleBackColor = true;
			this.cBtnLog.CheckedChanged += new EventHandler(this.cBtnLog_CheckedChanged);
			this.cBtnPacketHandlerStartStop.Appearance = Appearance.Button;
			this.cBtnPacketHandlerStartStop.Location = new Point(37, 18);
			this.cBtnPacketHandlerStartStop.Name = "cBtnPacketHandlerStartStop";
			this.cBtnPacketHandlerStartStop.Size = new Size(75, 21);
			this.cBtnPacketHandlerStartStop.TabIndex = 0;
			this.cBtnPacketHandlerStartStop.Text = "Start";
			this.cBtnPacketHandlerStartStop.TextAlign = ContentAlignment.MiddleCenter;
			this.cBtnPacketHandlerStartStop.UseVisualStyleBackColor = true;
			this.cBtnPacketHandlerStartStop.CheckedChanged += new EventHandler(this.cBtnPacketHandlerStartStop_CheckedChanged);
			this.lblPacketsNb.AutoSize = true;
			this.lblPacketsNb.Location = new Point(3, 47);
			this.lblPacketsNb.Name = "lblPacketsNb";
			this.lblPacketsNb.Size = new Size(71, 12);
			this.lblPacketsNb.TabIndex = 1;
			this.lblPacketsNb.Text = "Tx Packets:";
			this.lblPacketsNb.TextAlign = ContentAlignment.MiddleLeft;
			this.tBoxPacketsRepeatValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.tBoxPacketsRepeatValue.Location = new Point(149, 65);
			this.tBoxPacketsRepeatValue.Name = "tBoxPacketsRepeatValue";
			this.tBoxPacketsRepeatValue.Size = new Size(79, 20);
			this.tBoxPacketsRepeatValue.TabIndex = 4;
			this.tBoxPacketsRepeatValue.Text = "0";
			this.tBoxPacketsRepeatValue.TextAlign = HorizontalAlignment.Right;
			this.lblPacketsRepeatValue.AutoSize = true;
			this.lblPacketsRepeatValue.Location = new Point(3, 67);
			this.lblPacketsRepeatValue.Name = "lblPacketsRepeatValue";
			this.lblPacketsRepeatValue.Size = new Size(83, 12);
			this.lblPacketsRepeatValue.TabIndex = 3;
			this.lblPacketsRepeatValue.Text = "Repeat value:";
			this.lblPacketsRepeatValue.TextAlign = ContentAlignment.MiddleLeft;
			this.gBoxPacket.Controls.Add((Control)this.imgPacketMessage);
			this.gBoxPacket.Controls.Add((Control)this.gBoxMessage);
			this.gBoxPacket.Controls.Add((Control)this.tblPacket);
			this.gBoxPacket.Location = new Point(3, 293);
			this.gBoxPacket.Margin = new Padding(3, 1, 3, 1);
			this.gBoxPacket.Name = "gBoxPacket";
			this.gBoxPacket.Size = new Size(557, 159);
			this.gBoxPacket.TabIndex = 2;
			this.gBoxPacket.TabStop = false;
			this.gBoxPacket.Text = "Packet";
			this.gBoxPacket.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxPacket.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.imgPacketMessage.BackColor = Color.Transparent;
			this.imgPacketMessage.Location = new Point(5, 56);
			this.imgPacketMessage.Margin = new Padding(0);
			this.imgPacketMessage.Name = "imgPacketMessage";
			this.imgPacketMessage.Size = new Size(547, 5);
			this.imgPacketMessage.TabIndex = 1;
			this.imgPacketMessage.Text = "payloadImg1";
			this.gBoxMessage.Controls.Add((Control)this.tblPayloadMessage);
			this.gBoxMessage.Location = new Point(6, 62);
			this.gBoxMessage.Margin = new Padding(1);
			this.gBoxMessage.Name = "gBoxMessage";
			this.gBoxMessage.Size = new Size(547, 93);
			this.gBoxMessage.TabIndex = 2;
			this.gBoxMessage.TabStop = false;
			this.gBoxMessage.Text = "Message";
			this.gBoxMessage.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.gBoxMessage.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.tblPayloadMessage.AutoSize = true;
			this.tblPayloadMessage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblPayloadMessage.ColumnCount = 2;
			this.tblPayloadMessage.ColumnStyles.Add(new ColumnStyle());
			this.tblPayloadMessage.ColumnStyles.Add(new ColumnStyle());
			this.tblPayloadMessage.Controls.Add((Control)this.hexBoxPayload, 0, 1);
			this.tblPayloadMessage.Controls.Add((Control)this.label36, 1, 0);
			this.tblPayloadMessage.Controls.Add((Control)this.label35, 0, 0);
			this.tblPayloadMessage.Location = new Point(20, 18);
			this.tblPayloadMessage.Name = "tblPayloadMessage";
			this.tblPayloadMessage.RowCount = 2;
			this.tblPayloadMessage.RowStyles.Add(new RowStyle());
			this.tblPayloadMessage.RowStyles.Add(new RowStyle());
			this.tblPayloadMessage.Size = new Size(507, 73);
			this.tblPayloadMessage.TabIndex = 0;
			this.hexBoxPayload.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.tblPayloadMessage.SetColumnSpan((Control)this.hexBoxPayload, 2);
			this.hexBoxPayload.Font = new Font("Courier New", 8.25f);
			this.hexBoxPayload.LineInfoDigits = (byte)2;
			this.hexBoxPayload.LineInfoForeColor = Color.Empty;
			this.hexBoxPayload.Location = new Point(3, 15);
			this.hexBoxPayload.Name = "hexBoxPayload";
			this.hexBoxPayload.ShadowSelectionColor = Color.FromArgb(100, 60, 188, (int)byte.MaxValue);
			this.hexBoxPayload.Size = new Size(501, 55);
			this.hexBoxPayload.StringViewVisible = true;
			this.hexBoxPayload.TabIndex = 2;
			this.hexBoxPayload.UseFixedBytesPerLine = true;
			this.hexBoxPayload.VScrollBarVisible = true;
			this.label36.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.label36.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.label36.Location = new Point(329, 0);
			this.label36.Name = "label36";
			this.label36.Size = new Size(175, 12);
			this.label36.TabIndex = 1;
			this.label36.Text = "ASCII";
			this.label36.TextAlign = ContentAlignment.MiddleCenter;
			this.label35.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.label35.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.label35.Location = new Point(3, 0);
			this.label35.Name = "label35";
			this.label35.Size = new Size(320, 12);
			this.label35.TabIndex = 0;
			this.label35.Text = "HEXADECIMAL";
			this.label35.TextAlign = ContentAlignment.MiddleCenter;
			this.tblPacket.AutoSize = true;
			this.tblPacket.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblPacket.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			this.tblPacket.ColumnCount = 6;
			this.tblPacket.ColumnStyles.Add(new ColumnStyle());
			this.tblPacket.ColumnStyles.Add(new ColumnStyle());
			this.tblPacket.ColumnStyles.Add(new ColumnStyle());
			this.tblPacket.ColumnStyles.Add(new ColumnStyle());
			this.tblPacket.ColumnStyles.Add(new ColumnStyle());
			this.tblPacket.ColumnStyles.Add(new ColumnStyle());
			this.tblPacket.Controls.Add((Control)this.label29, 0, 0);
			this.tblPacket.Controls.Add((Control)this.label30, 1, 0);
			this.tblPacket.Controls.Add((Control)this.label31, 2, 0);
			this.tblPacket.Controls.Add((Control)this.label32, 3, 0);
			this.tblPacket.Controls.Add((Control)this.label33, 4, 0);
			this.tblPacket.Controls.Add((Control)this.label34, 5, 0);
			this.tblPacket.Controls.Add((Control)this.lblPacketPreamble, 0, 1);
			this.tblPacket.Controls.Add((Control)this.lblPayload, 4, 1);
			this.tblPacket.Controls.Add((Control)this.pnlPacketCrc, 5, 1);
			this.tblPacket.Controls.Add((Control)this.pnlPacketAddr, 3, 1);
			this.tblPacket.Controls.Add((Control)this.lblPacketLength, 2, 1);
			this.tblPacket.Controls.Add((Control)this.lblPacketSyncValue, 1, 1);
			this.tblPacket.Location = new Point(5, 16);
			this.tblPacket.Margin = new Padding(1);
			this.tblPacket.Name = "tblPacket";
			this.tblPacket.RowCount = 2;
			this.tblPacket.RowStyles.Add(new RowStyle());
			this.tblPacket.RowStyles.Add(new RowStyle());
			this.tblPacket.Size = new Size(547, 39);
			this.tblPacket.TabIndex = 0;
			this.label29.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.label29.Location = new Point(1, 1);
			this.label29.Margin = new Padding(0);
			this.label29.Name = "label29";
			this.label29.Size = new Size(103, 18);
			this.label29.TabIndex = 0;
			this.label29.Text = "Preamble";
			this.label29.TextAlign = ContentAlignment.MiddleCenter;
			this.label30.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.label30.Location = new Point(108, 1);
			this.label30.Margin = new Padding(0);
			this.label30.Name = "label30";
			this.label30.Size = new Size(152, 18);
			this.label30.TabIndex = 1;
			this.label30.Text = "Sync";
			this.label30.TextAlign = ContentAlignment.MiddleCenter;
			this.label31.BackColor = Color.LightGray;
			this.label31.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.label31.Location = new Point(261, 1);
			this.label31.Margin = new Padding(0);
			this.label31.Name = "label31";
			this.label31.Size = new Size(59, 18);
			this.label31.TabIndex = 2;
			this.label31.Text = "Length";
			this.label31.TextAlign = ContentAlignment.MiddleCenter;
			this.label32.BackColor = Color.LightGray;
			this.label32.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.label32.Location = new Point(321, 1);
			this.label32.Margin = new Padding(0);
			this.label32.Name = "label32";
			this.label32.Size = new Size(87, 18);
			this.label32.TabIndex = 3;
			this.label32.Text = "Node Address";
			this.label32.TextAlign = ContentAlignment.MiddleCenter;
			this.label33.BackColor = Color.LightGray;
			this.label33.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.label33.ForeColor = SystemColors.WindowText;
			this.label33.Location = new Point(409, 1);
			this.label33.Margin = new Padding(0);
			this.label33.Name = "label33";
			this.label33.Size = new Size(85, 18);
			this.label33.TabIndex = 4;
			this.label33.Text = "Message";
			this.label33.TextAlign = ContentAlignment.MiddleCenter;
			this.label34.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.label34.Location = new Point(495, 1);
			this.label34.Margin = new Padding(0);
			this.label34.Name = "label34";
			this.label34.Size = new Size(51, 18);
			this.label34.TabIndex = 5;
			this.label34.Text = "CRC";
			this.label34.TextAlign = ContentAlignment.MiddleCenter;
			this.lblPacketPreamble.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.lblPacketPreamble.Location = new Point(1, 20);
			this.lblPacketPreamble.Margin = new Padding(0);
			this.lblPacketPreamble.Name = "lblPacketPreamble";
			this.lblPacketPreamble.Size = new Size(106, 18);
			this.lblPacketPreamble.TabIndex = 6;
			this.lblPacketPreamble.Text = "55-55-55-55-...-55";
			this.lblPacketPreamble.TextAlign = ContentAlignment.MiddleCenter;
			this.lblPayload.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			this.lblPayload.Location = new Point(409, 20);
			this.lblPayload.Margin = new Padding(0);
			this.lblPayload.Name = "lblPayload";
			this.lblPayload.Size = new Size(85, 18);
			this.lblPayload.TabIndex = 9;
			this.lblPayload.TextAlign = ContentAlignment.MiddleCenter;
			this.pnlPacketCrc.Controls.Add((Control)this.ledPacketCrc);
			this.pnlPacketCrc.Controls.Add((Control)this.lblPacketCrc);
			this.pnlPacketCrc.Location = new Point(495, 20);
			this.pnlPacketCrc.Margin = new Padding(0);
			this.pnlPacketCrc.Name = "pnlPacketCrc";
			this.pnlPacketCrc.Size = new Size(51, 18);
			this.pnlPacketCrc.TabIndex = 18;
			this.ledPacketCrc.BackColor = Color.Transparent;
			this.ledPacketCrc.LedColor = Color.Green;
			this.ledPacketCrc.LedSize = new Size(11, 11);
			this.ledPacketCrc.Location = new Point(17, 3);
			this.ledPacketCrc.Name = "ledPacketCrc";
			this.ledPacketCrc.Size = new Size(15, 14);
			this.ledPacketCrc.TabIndex = 1;
			this.ledPacketCrc.Text = "CRC";
			this.ledPacketCrc.Visible = false;
			this.lblPacketCrc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.lblPacketCrc.Location = new Point(0, 0);
			this.lblPacketCrc.Margin = new Padding(0);
			this.lblPacketCrc.Name = "lblPacketCrc";
			this.lblPacketCrc.Size = new Size(51, 18);
			this.lblPacketCrc.TabIndex = 0;
			this.lblPacketCrc.Text = "XX-XX";
			this.lblPacketCrc.TextAlign = ContentAlignment.MiddleCenter;
			this.pnlPacketAddr.Controls.Add((Control)this.lblPacketAddr);
			this.pnlPacketAddr.Location = new Point(321, 20);
			this.pnlPacketAddr.Margin = new Padding(0);
			this.pnlPacketAddr.Name = "pnlPacketAddr";
			this.pnlPacketAddr.Size = new Size(87, 18);
			this.pnlPacketAddr.TabIndex = 11;
			this.lblPacketAddr.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.lblPacketAddr.Location = new Point(0, 0);
			this.lblPacketAddr.Margin = new Padding(0);
			this.lblPacketAddr.Name = "lblPacketAddr";
			this.lblPacketAddr.Size = new Size(87, 18);
			this.lblPacketAddr.TabIndex = 0;
			this.lblPacketAddr.Text = "00";
			this.lblPacketAddr.TextAlign = ContentAlignment.MiddleCenter;
			this.lblPacketLength.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.lblPacketLength.Location = new Point(261, 20);
			this.lblPacketLength.Margin = new Padding(0);
			this.lblPacketLength.Name = "lblPacketLength";
			this.lblPacketLength.Size = new Size(59, 18);
			this.lblPacketLength.TabIndex = 8;
			this.lblPacketLength.Text = "00";
			this.lblPacketLength.TextAlign = ContentAlignment.MiddleCenter;
			this.lblPacketSyncValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			this.lblPacketSyncValue.Location = new Point(108, 20);
			this.lblPacketSyncValue.Margin = new Padding(0);
			this.lblPacketSyncValue.Name = "lblPacketSyncValue";
			this.lblPacketSyncValue.Size = new Size(152, 18);
			this.lblPacketSyncValue.TabIndex = 7;
			this.lblPacketSyncValue.Text = "AA-AA-AA-AA-AA-AA-AA-AA";
			this.lblPacketSyncValue.TextAlign = ContentAlignment.MiddleCenter;
			this.AutoScaleDimensions = new SizeF(6f, 12f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add((Control)this.gBoxDeviceStatus);
			this.Controls.Add((Control)this.tableLayoutPanel2);
			this.Controls.Add((Control)this.tableLayoutPanel1);
			this.Controls.Add((Control)this.gBoxControl);
			this.Controls.Add((Control)this.gBoxPacket);
			this.Name = "PacketHandlerView";
			this.Size = new Size(799, 455);
			this.nudPreambleSize.EndInit();
			this.pnlAesEncryption.ResumeLayout(false);
			this.pnlAesEncryption.PerformLayout();
			this.pnlDcFree.ResumeLayout(false);
			this.pnlDcFree.PerformLayout();
			this.pnlAddressInPayload.ResumeLayout(false);
			this.pnlAddressInPayload.PerformLayout();
			this.pnlFifoFillCondition.ResumeLayout(false);
			this.pnlFifoFillCondition.PerformLayout();
			this.pnlSync.ResumeLayout(false);
			this.pnlSync.PerformLayout();
			this.pnlCrcAutoClear.ResumeLayout(false);
			this.pnlCrcAutoClear.PerformLayout();
			this.pnlCrcCalculation.ResumeLayout(false);
			this.pnlCrcCalculation.PerformLayout();
			this.pnlTxStart.ResumeLayout(false);
			this.pnlTxStart.PerformLayout();
			this.pnlAddressFiltering.ResumeLayout(false);
			this.pnlAddressFiltering.PerformLayout();
			this.pnlPacketFormat.ResumeLayout(false);
			this.pnlPacketFormat.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.pnlPayloadLength.ResumeLayout(false);
			this.nudPayloadLength.EndInit();
			this.nudSyncSize.EndInit();
			this.nudSyncTol.EndInit();
			this.pnlNodeAddress.ResumeLayout(false);
			this.nudNodeAddress.EndInit();
			this.pnlBroadcastAddress.ResumeLayout(false);
			this.nudBroadcastAddress.EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.pnlCrcPolynom.ResumeLayout(false);
			this.pnlCrcPolynom.PerformLayout();
			this.nudFifoThreshold.EndInit();
			this.gBoxDeviceStatus.ResumeLayout(false);
			this.gBoxDeviceStatus.PerformLayout();
			this.gBoxControl.ResumeLayout(false);
			this.gBoxControl.PerformLayout();
			this.gBoxPacket.ResumeLayout(false);
			this.gBoxPacket.PerformLayout();
			this.gBoxMessage.ResumeLayout(false);
			this.gBoxMessage.PerformLayout();
			this.tblPayloadMessage.ResumeLayout(false);
			this.tblPacket.ResumeLayout(false);
			this.pnlPacketCrc.ResumeLayout(false);
			this.pnlPacketAddr.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}