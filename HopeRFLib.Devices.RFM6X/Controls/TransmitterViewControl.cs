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
	public class TransmitterViewControl : UserControl, INotifyDocumentationChanged
	{
		private Version version = new Version(2, 4);
		private Decimal ocpTrim = new Decimal(100);
		private IContainer components;
		private NumericUpDownEx nudOutputPower;
		private NumericUpDownEx nudOcpTrim;
		private ComboBox cBoxPaRamp;
		private Panel panel4;
		private RadioButton rBtnOcpOff;
		private RadioButton rBtnOcpOn;
		private Panel pnlPaMode;
		private RadioButton rBtnPaControlPa1Pa2;
		private RadioButton rBtnPaControlPa1;
		private RadioButton rBtnPaControlPa0;
		private Label suffixOutputPower;
		private Label suffixPAramp;
		private Label suffixOCPtrim;
		private Label label3;
		private Label label5;
		private ErrorProvider errorProvider;
		private GroupBoxEx gBoxPowerAmplifier;
		private GroupBoxEx gBoxOverloadCurrentProtection;
		private GroupBoxEx gBoxOutputPower;
		private Panel pnlPa20dBm;
		private RadioButton rBtnPa20dBmOff;
		private RadioButton rBtnPa20dBmOn;
		private Label lblPa20dBm;

		public Version Version
		{
			get
			{
				return version;
			}
			set
			{
				version = value;
				if (value >= new Version(2, 4))
				{
					lblPa20dBm.Visible = true;
					pnlPa20dBm.Visible = true;
				}
				else
				{
					lblPa20dBm.Visible = false;
					pnlPa20dBm.Visible = false;
				}
			}
		}

		public PaModeEnum PaMode
		{
			get
			{
				if (rBtnPaControlPa0.Checked)
					return PaModeEnum.PA0;
				if (rBtnPaControlPa1.Checked)
					return PaModeEnum.PA1;
				return rBtnPaControlPa1Pa2.Checked ? PaModeEnum.PA1_PA2 : PaModeEnum.PA0;
			}
			set
			{
				rBtnPaControlPa0.CheckedChanged -= new EventHandler(rBtnPaControl_CheckedChanged);
				rBtnPaControlPa1.CheckedChanged -= new EventHandler(rBtnPaControl_CheckedChanged);
				rBtnPaControlPa1Pa2.CheckedChanged -= new EventHandler(rBtnPaControl_CheckedChanged);
				switch (value)
				{
					case PaModeEnum.PA0:
						rBtnPaControlPa0.Checked = true;
						rBtnPaControlPa1.Checked = false;
						rBtnPaControlPa1Pa2.Checked = false;
						break;
					case PaModeEnum.PA1:
						rBtnPaControlPa0.Checked = false;
						rBtnPaControlPa1.Checked = true;
						rBtnPaControlPa1Pa2.Checked = false;
						break;
					case PaModeEnum.PA1_PA2:
						rBtnPaControlPa0.Checked = false;
						rBtnPaControlPa1.Checked = false;
						rBtnPaControlPa1Pa2.Checked = true;
						break;
				}
				rBtnPaControlPa0.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
				rBtnPaControlPa1.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
				rBtnPaControlPa1Pa2.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
			}
		}

		public Decimal OutputPower
		{
			get
			{
				return nudOutputPower.Value;
			}
			set
			{
				try
				{
					nudOutputPower.ValueChanged -= new EventHandler(nudOutputPower_ValueChanged);
					nudOutputPower.BackColor = SystemColors.Window;
					switch (PaMode)
					{
						case PaModeEnum.PA0:
							nudOutputPower.Maximum = new Decimal(13);
							nudOutputPower.Minimum = new Decimal(-18);
							break;
						case PaModeEnum.PA1:
							nudOutputPower.Maximum = new Decimal(13);
							nudOutputPower.Minimum = new Decimal(-2);
							break;
						case PaModeEnum.PA1_PA2:
							if (!Pa20dBm)
							{
								nudOutputPower.Maximum = new Decimal(17);
								nudOutputPower.Minimum = new Decimal(2);
								break;
							}
							else
							{
								nudOutputPower.Maximum = new Decimal(20);
								nudOutputPower.Minimum = new Decimal(5);
								break;
							}
					}
					if (value > nudOutputPower.Maximum)
						value = nudOutputPower.Maximum;
					if (value < nudOutputPower.Minimum)
						value = nudOutputPower.Minimum;
					nudOutputPower.Value = value;
				}
				catch (Exception)
				{
					nudOutputPower.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudOutputPower.ValueChanged += new EventHandler(nudOutputPower_ValueChanged);
				}
			}
		}

		public PaRampEnum PaRamp
		{
			get
			{
				return (PaRampEnum)cBoxPaRamp.SelectedIndex;
			}
			set
			{
				cBoxPaRamp.SelectedIndexChanged -= new EventHandler(cBoxPaRamp_SelectedIndexChanged);
				cBoxPaRamp.SelectedIndex = (int)value;
				cBoxPaRamp.SelectedIndexChanged += new EventHandler(cBoxPaRamp_SelectedIndexChanged);
			}
		}

		public bool OcpOn
		{
			get
			{
				return rBtnOcpOn.Checked;
			}
			set
			{
				rBtnOcpOn.CheckedChanged -= new EventHandler(rBtnOcpOn_CheckedChanged);
				rBtnOcpOff.CheckedChanged -= new EventHandler(rBtnOcpOn_CheckedChanged);
				if (value)
				{
					rBtnOcpOn.Checked = true;
					rBtnOcpOff.Checked = false;
				}
				else
				{
					rBtnOcpOn.Checked = false;
					rBtnOcpOff.Checked = true;
				}
				rBtnOcpOn.CheckedChanged += new EventHandler(rBtnOcpOn_CheckedChanged);
				rBtnOcpOff.CheckedChanged += new EventHandler(rBtnOcpOn_CheckedChanged);
			}
		}

		public Decimal OcpTrim
		{
			get
			{
				return ocpTrim;
			}
			set
			{
				try
				{
					nudOcpTrim.ValueChanged -= new EventHandler(nudOcpTrim_ValueChanged);
					ocpTrim = new Decimal(450, 0, 0, false, (byte)1) + new Decimal(50, 0, 0, false, (byte)1) * (Decimal)(ushort)Math.Round((value - new Decimal(450, 0, 0, false, (byte)1)) / new Decimal(50, 0, 0, false, (byte)1), MidpointRounding.AwayFromZero);
					nudOcpTrim.Value = ocpTrim;
					nudOcpTrim.ValueChanged += new EventHandler(nudOcpTrim_ValueChanged);
				}
				catch (Exception)
				{
					nudOcpTrim.BackColor = ControlPaint.LightLight(Color.Red);
					nudOcpTrim.ValueChanged += new EventHandler(nudOcpTrim_ValueChanged);
				}
			}
		}

		public bool Pa20dBm
		{
			get
			{
				return rBtnPa20dBmOn.Checked;
			}
			set
			{
				rBtnPa20dBmOn.CheckedChanged -= new EventHandler(rBtnPa20dBm_CheckedChanged);
				rBtnPa20dBmOff.CheckedChanged -= new EventHandler(rBtnPa20dBm_CheckedChanged);
				if (value)
				{
					rBtnPa20dBmOn.Checked = true;
					rBtnPa20dBmOff.Checked = false;
					pnlPaMode.Enabled = false;
					gBoxOverloadCurrentProtection.Enabled = false;
				}
				else
				{
					rBtnPa20dBmOn.Checked = false;
					rBtnPa20dBmOff.Checked = true;
					pnlPaMode.Enabled = true;
					gBoxOverloadCurrentProtection.Enabled = true;
				}
				rBtnPa20dBmOn.CheckedChanged += new EventHandler(rBtnPa20dBm_CheckedChanged);
				rBtnPa20dBmOff.CheckedChanged += new EventHandler(rBtnPa20dBm_CheckedChanged);
			}
		}

		public event PaModeEventHandler PaModeChanged;

		public event DecimalEventHandler OutputPowerChanged;

		public event PaRampEventHandler PaRampChanged;

		public event BooleanEventHandler OcpOnChanged;

		public event DecimalEventHandler OcpTrimChanged;

		public event BooleanEventHandler Pa20dBmChanged;

		public event DocumentationChangedEventHandler DocumentationChanged;

		public TransmitterViewControl()
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
			errorProvider = new ErrorProvider(components);
			nudOutputPower = new NumericUpDownEx();
			gBoxOverloadCurrentProtection = new GroupBoxEx();
			panel4 = new Panel();
			rBtnOcpOff = new RadioButton();
			rBtnOcpOn = new RadioButton();
			label5 = new Label();
			nudOcpTrim = new NumericUpDownEx();
			suffixOCPtrim = new Label();
			gBoxOutputPower = new GroupBoxEx();
			pnlPa20dBm = new Panel();
			rBtnPa20dBmOff = new RadioButton();
			rBtnPa20dBmOn = new RadioButton();
			lblPa20dBm = new Label();
			suffixOutputPower = new Label();
			gBoxPowerAmplifier = new GroupBoxEx();
			cBoxPaRamp = new ComboBox();
			pnlPaMode = new Panel();
			rBtnPaControlPa1Pa2 = new RadioButton();
			rBtnPaControlPa1 = new RadioButton();
			rBtnPaControlPa0 = new RadioButton();
			suffixPAramp = new Label();
			label3 = new Label();
			nudOutputPower.BeginInit();
			gBoxOverloadCurrentProtection.SuspendLayout();
			panel4.SuspendLayout();
			nudOcpTrim.BeginInit();
			gBoxOutputPower.SuspendLayout();
			pnlPa20dBm.SuspendLayout();
			gBoxPowerAmplifier.SuspendLayout();
			pnlPaMode.SuspendLayout();
			SuspendLayout();
			errorProvider.ContainerControl = (ContainerControl)this;
			errorProvider.SetIconPadding((Control)nudOutputPower, 35);
			nudOutputPower.Location = new Point(176, 18);
			int[] bits1 = new int[4];
			bits1[0] = 13;
			Decimal num1 = new Decimal(bits1);
			nudOutputPower.Maximum = num1;
			nudOutputPower.Minimum = new Decimal(new int[4]
      {
        18,
        0,
        0,
        int.MinValue
      });
			nudOutputPower.Name = "nudOutputPower";
			nudOutputPower.Size = new Size(124, 21);
			nudOutputPower.TabIndex = 0;
			nudOutputPower.ThousandsSeparator = true;
			int[] bits2 = new int[4];
			bits2[0] = 13;
			Decimal num2 = new Decimal(bits2);
			nudOutputPower.Value = num2;
			nudOutputPower.ValueChanged += new EventHandler(nudOutputPower_ValueChanged);
			gBoxOverloadCurrentProtection.Controls.Add((Control)panel4);
			gBoxOverloadCurrentProtection.Controls.Add((Control)label5);
			gBoxOverloadCurrentProtection.Controls.Add((Control)nudOcpTrim);
			gBoxOverloadCurrentProtection.Controls.Add((Control)suffixOCPtrim);
			gBoxOverloadCurrentProtection.Location = new Point(217, 290);
			gBoxOverloadCurrentProtection.Name = "gBoxOverloadCurrentProtection";
			gBoxOverloadCurrentProtection.Size = new Size(364, 64);
			gBoxOverloadCurrentProtection.TabIndex = 2;
			gBoxOverloadCurrentProtection.TabStop = false;
			gBoxOverloadCurrentProtection.Text = "Overload current protection";
			gBoxOverloadCurrentProtection.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxOverloadCurrentProtection.MouseLeave += new EventHandler(control_MouseLeave);
			panel4.AutoSize = true;
			panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel4.Controls.Add((Control)rBtnOcpOff);
			panel4.Controls.Add((Control)rBtnOcpOn);
			panel4.Location = new Point(176, 18);
			panel4.Name = "panel4";
			panel4.Size = new Size(98, 19);
			panel4.TabIndex = 0;
			rBtnOcpOff.AutoSize = true;
			rBtnOcpOff.Location = new Point(54, 3);
			rBtnOcpOff.Margin = new Padding(3, 0, 3, 0);
			rBtnOcpOff.Name = "rBtnOcpOff";
			rBtnOcpOff.Size = new Size(41, 16);
			rBtnOcpOff.TabIndex = 1;
			rBtnOcpOff.Text = "OFF";
			rBtnOcpOff.UseVisualStyleBackColor = true;
			rBtnOcpOff.CheckedChanged += new EventHandler(rBtnOcpOn_CheckedChanged);
			rBtnOcpOn.AutoSize = true;
			rBtnOcpOn.Checked = true;
			rBtnOcpOn.Location = new Point(3, 3);
			rBtnOcpOn.Margin = new Padding(3, 0, 3, 0);
			rBtnOcpOn.Name = "rBtnOcpOn";
			rBtnOcpOn.Size = new Size(35, 16);
			rBtnOcpOn.TabIndex = 0;
			rBtnOcpOn.TabStop = true;
			rBtnOcpOn.Text = "ON";
			rBtnOcpOn.UseVisualStyleBackColor = true;
			rBtnOcpOn.CheckedChanged += new EventHandler(rBtnOcpOn_CheckedChanged);
			label5.AutoSize = true;
			label5.Location = new Point(6, 45);
			label5.Name = "label5";
			label5.Size = new Size(59, 12);
			label5.TabIndex = 1;
			label5.Text = "Trimming:";
			nudOcpTrim.Location = new Point(176, 42);
			int[] bits3 = new int[4];
			bits3[0] = 120;
			Decimal num3 = new Decimal(bits3);
			nudOcpTrim.Maximum = num3;
			int[] bits4 = new int[4];
			bits4[0] = 45;
			Decimal num4 = new Decimal(bits4);
			nudOcpTrim.Minimum = num4;
			nudOcpTrim.Name = "nudOcpTrim";
			nudOcpTrim.Size = new Size(124, 21);
			nudOcpTrim.TabIndex = 2;
			nudOcpTrim.ThousandsSeparator = true;
			int[] bits5 = new int[4];
			bits5[0] = 100;
			Decimal num5 = new Decimal(bits5);
			nudOcpTrim.Value = num5;
			nudOcpTrim.ValueChanged += new EventHandler(nudOcpTrim_ValueChanged);
			suffixOCPtrim.AutoSize = true;
			suffixOCPtrim.Location = new Point(306, 45);
			suffixOCPtrim.Name = "suffixOCPtrim";
			suffixOCPtrim.Size = new Size(17, 12);
			suffixOCPtrim.TabIndex = 3;
			suffixOCPtrim.Text = "mA";
			gBoxOutputPower.Controls.Add((Control)pnlPa20dBm);
			gBoxOutputPower.Controls.Add((Control)nudOutputPower);
			gBoxOutputPower.Controls.Add((Control)lblPa20dBm);
			gBoxOutputPower.Controls.Add((Control)suffixOutputPower);
			gBoxOutputPower.Location = new Point(217, 217);
			gBoxOutputPower.Name = "gBoxOutputPower";
			gBoxOutputPower.Size = new Size(364, 67);
			gBoxOutputPower.TabIndex = 1;
			gBoxOutputPower.TabStop = false;
			gBoxOutputPower.Text = "Output power";
			gBoxOutputPower.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxOutputPower.MouseLeave += new EventHandler(control_MouseLeave);
			pnlPa20dBm.AutoSize = true;
			pnlPa20dBm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlPa20dBm.Controls.Add((Control)rBtnPa20dBmOff);
			pnlPa20dBm.Controls.Add((Control)rBtnPa20dBmOn);
			pnlPa20dBm.Location = new Point(176, 42);
			pnlPa20dBm.Name = "pnlPa20dBm";
			pnlPa20dBm.Size = new Size(98, 19);
			pnlPa20dBm.TabIndex = 0;
			rBtnPa20dBmOff.AutoSize = true;
			rBtnPa20dBmOff.Checked = true;
			rBtnPa20dBmOff.Location = new Point(54, 3);
			rBtnPa20dBmOff.Margin = new Padding(3, 0, 3, 0);
			rBtnPa20dBmOff.Name = "rBtnPa20dBmOff";
			rBtnPa20dBmOff.Size = new Size(41, 16);
			rBtnPa20dBmOff.TabIndex = 1;
			rBtnPa20dBmOff.TabStop = true;
			rBtnPa20dBmOff.Text = "OFF";
			rBtnPa20dBmOff.UseVisualStyleBackColor = true;
			rBtnPa20dBmOff.CheckedChanged += new EventHandler(rBtnPa20dBm_CheckedChanged);
			rBtnPa20dBmOn.AutoSize = true;
			rBtnPa20dBmOn.Location = new Point(3, 3);
			rBtnPa20dBmOn.Margin = new Padding(3, 0, 3, 0);
			rBtnPa20dBmOn.Name = "rBtnPa20dBmOn";
			rBtnPa20dBmOn.Size = new Size(35, 16);
			rBtnPa20dBmOn.TabIndex = 0;
			rBtnPa20dBmOn.Text = "ON";
			rBtnPa20dBmOn.UseVisualStyleBackColor = true;
			rBtnPa20dBmOn.CheckedChanged += new EventHandler(rBtnPa20dBm_CheckedChanged);
			lblPa20dBm.AutoSize = true;
			lblPa20dBm.Location = new Point(6, 45);
			lblPa20dBm.Name = "lblPa20dBm";
			lblPa20dBm.Size = new Size(149, 12);
			lblPa20dBm.TabIndex = 1;
			lblPa20dBm.Text = "+20 dBm on pin PA_BOOST:";
			suffixOutputPower.AutoSize = true;
			suffixOutputPower.Location = new Point(306, 21);
			suffixOutputPower.Name = "suffixOutputPower";
			suffixOutputPower.Size = new Size(23, 12);
			suffixOutputPower.TabIndex = 1;
			suffixOutputPower.Text = "dBm";
			gBoxPowerAmplifier.Controls.Add((Control)cBoxPaRamp);
			gBoxPowerAmplifier.Controls.Add((Control)pnlPaMode);
			gBoxPowerAmplifier.Controls.Add((Control)suffixPAramp);
			gBoxPowerAmplifier.Controls.Add((Control)label3);
			gBoxPowerAmplifier.Location = new Point(217, 102);
			gBoxPowerAmplifier.Name = "gBoxPowerAmplifier";
			gBoxPowerAmplifier.Size = new Size(364, 110);
			gBoxPowerAmplifier.TabIndex = 0;
			gBoxPowerAmplifier.TabStop = false;
			gBoxPowerAmplifier.Text = "Power Amplifier";
			gBoxPowerAmplifier.MouseEnter += new EventHandler(control_MouseEnter);
			gBoxPowerAmplifier.MouseLeave += new EventHandler(control_MouseLeave);
			cBoxPaRamp.Items.AddRange(new object[16]
      {
        (object) "3400",
        (object) "2000",
        (object) "1000",
        (object) "500",
        (object) "250",
        (object) "125",
        (object) "100",
        (object) "62",
        (object) "50",
        (object) "40",
        (object) "31",
        (object) "25",
        (object) "20",
        (object) "15",
        (object) "12",
        (object) "10"
      });
			cBoxPaRamp.Location = new Point(176, 87);
			cBoxPaRamp.Name = "cBoxPaRamp";
			cBoxPaRamp.Size = new Size(124, 20);
			cBoxPaRamp.TabIndex = 2;
			cBoxPaRamp.SelectedIndexChanged += new EventHandler(cBoxPaRamp_SelectedIndexChanged);
			pnlPaMode.AutoSize = true;
			pnlPaMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			pnlPaMode.Controls.Add((Control)rBtnPaControlPa1Pa2);
			pnlPaMode.Controls.Add((Control)rBtnPaControlPa1);
			pnlPaMode.Controls.Add((Control)rBtnPaControlPa0);
			pnlPaMode.Location = new Point(65, 18);
			pnlPaMode.Name = "pnlPaMode";
			pnlPaMode.Size = new Size(257, 64);
			pnlPaMode.TabIndex = 0;
			rBtnPaControlPa1Pa2.AutoSize = true;
			rBtnPaControlPa1Pa2.Location = new Point(3, 45);
			rBtnPaControlPa1Pa2.Name = "rBtnPaControlPa1Pa2";
			rBtnPaControlPa1Pa2.Size = new Size(251, 16);
			rBtnPaControlPa1Pa2.TabIndex = 2;
			rBtnPaControlPa1Pa2.Text = "PA1 + PA2 -> Transmits on pin PA_BOOST";
			rBtnPaControlPa1Pa2.UseVisualStyleBackColor = true;
			rBtnPaControlPa1Pa2.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
			rBtnPaControlPa1.AutoSize = true;
			rBtnPaControlPa1.Location = new Point(3, 24);
			rBtnPaControlPa1.Name = "rBtnPaControlPa1";
			rBtnPaControlPa1.Size = new Size(215, 16);
			rBtnPaControlPa1.TabIndex = 1;
			rBtnPaControlPa1.Text = "PA1 -> Transmits on pin PA_BOOST";
			rBtnPaControlPa1.UseVisualStyleBackColor = true;
			rBtnPaControlPa1.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
			rBtnPaControlPa0.AutoSize = true;
			rBtnPaControlPa0.Checked = true;
			rBtnPaControlPa0.Location = new Point(3, 3);
			rBtnPaControlPa0.Name = "rBtnPaControlPa0";
			rBtnPaControlPa0.Size = new Size(191, 16);
			rBtnPaControlPa0.TabIndex = 0;
			rBtnPaControlPa0.TabStop = true;
			rBtnPaControlPa0.Text = "PA0 -> Transmits on pin RFIO";
			rBtnPaControlPa0.UseVisualStyleBackColor = true;
			rBtnPaControlPa0.CheckedChanged += new EventHandler(rBtnPaControl_CheckedChanged);
			suffixPAramp.AutoSize = true;
			suffixPAramp.Location = new Point(306, 90);
			suffixPAramp.Name = "suffixPAramp";
			suffixPAramp.Size = new Size(17, 12);
			suffixPAramp.TabIndex = 3;
			suffixPAramp.Text = "µs";
			label3.AutoSize = true;
			label3.Location = new Point(6, 90);
			label3.Name = "label3";
			label3.Size = new Size(53, 12);
			label3.TabIndex = 1;
			label3.Text = "PA ramp:";
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add((Control)gBoxOverloadCurrentProtection);
			Controls.Add((Control)gBoxOutputPower);
			Controls.Add((Control)gBoxPowerAmplifier);
			Name = "TransmitterViewControl";
			Size = new Size(799, 455);
			nudOutputPower.EndInit();
			gBoxOverloadCurrentProtection.ResumeLayout(false);
			gBoxOverloadCurrentProtection.PerformLayout();
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			nudOcpTrim.EndInit();
			gBoxOutputPower.ResumeLayout(false);
			gBoxOutputPower.PerformLayout();
			pnlPa20dBm.ResumeLayout(false);
			pnlPa20dBm.PerformLayout();
			gBoxPowerAmplifier.ResumeLayout(false);
			gBoxPowerAmplifier.PerformLayout();
			pnlPaMode.ResumeLayout(false);
			pnlPaMode.PerformLayout();
			ResumeLayout(false);
		}

		private void OnPaModeChanged(PaModeEnum value)
		{
			if (PaModeChanged == null)
				return;
			PaModeChanged((object)this, new PaModeEventArg(value));
		}

		private void OnOutputPowerChanged(Decimal value)
		{
			if (OutputPowerChanged == null)
				return;
			OutputPowerChanged((object)this, new DecimalEventArg(value));
		}

		private void OnPaRampChanged(PaRampEnum value)
		{
			if (PaRampChanged == null)
				return;
			PaRampChanged((object)this, new PaRampEventArg(value));
		}

		private void OnOcpOnChanged(bool value)
		{
			if (OcpOnChanged == null)
				return;
			OcpOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnOcpTrimChanged(Decimal value)
		{
			if (OcpTrimChanged == null)
				return;
			OcpTrimChanged((object)this, new DecimalEventArg(value));
		}

		private void OnPa20dBmChanged(bool value)
		{
			if (Pa20dBmChanged == null)
				return;
			Pa20dBmChanged((object)this, new BooleanEventArg(value));
		}

		private void rBtnPaControl_CheckedChanged(object sender, EventArgs e)
		{
			if (rBtnPaControlPa0.Checked)
				PaMode = PaModeEnum.PA0;
			else if (rBtnPaControlPa1.Checked)
				PaMode = PaModeEnum.PA1;
			else if (rBtnPaControlPa1Pa2.Checked)
				PaMode = PaModeEnum.PA1_PA2;
			OnPaModeChanged(PaMode);
		}

		private void nudOutputPower_ValueChanged(object sender, EventArgs e)
		{
			OutputPower = nudOutputPower.Value;
			OnOutputPowerChanged(OutputPower);
		}

		private void cBoxPaRamp_SelectedIndexChanged(object sender, EventArgs e)
		{
			PaRamp = (PaRampEnum)cBoxPaRamp.SelectedIndex;
			OnPaRampChanged(PaRamp);
		}

		private void rBtnOcpOn_CheckedChanged(object sender, EventArgs e)
		{
			OcpOn = rBtnOcpOn.Checked;
			OnOcpOnChanged(OcpOn);
		}

		private void nudOcpTrim_ValueChanged(object sender, EventArgs e)
		{
			int num1 = (int)Math.Round((OcpTrim - new Decimal(450, 0, 0, false, (byte)1)) / new Decimal(50, 0, 0, false, (byte)1), MidpointRounding.AwayFromZero);
			int num2 = (int)Math.Round((nudOcpTrim.Value - new Decimal(450, 0, 0, false, (byte)1)) / new Decimal(50, 0, 0, false, (byte)1), MidpointRounding.AwayFromZero);
			int num3 = (int)(nudOcpTrim.Value - OcpTrim);
			if (num3 >= -1 && num3 <= 1 && num1 == num2)
			{
				nudOcpTrim.ValueChanged -= new EventHandler(nudOcpTrim_ValueChanged);
				nudOcpTrim.Value = new Decimal(450, 0, 0, false, (byte)1) + new Decimal(50, 0, 0, false, (byte)1) * (Decimal)(num2 + num3);
				nudOcpTrim.ValueChanged += new EventHandler(nudOcpTrim_ValueChanged);
			}
			OcpTrim = nudOcpTrim.Value;
			OnOcpTrimChanged(OcpTrim);
		}

		private void rBtnPa20dBm_CheckedChanged(object sender, EventArgs e)
		{
			Pa20dBm = rBtnPa20dBmOn.Checked;
			OnPa20dBmChanged(Pa20dBm);
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == gBoxPowerAmplifier)
				OnDocumentationChanged(new DocumentationChangedEventArgs("Transmitter", "Power amplifier"));
			else if (sender == gBoxOutputPower)
			{
				OnDocumentationChanged(new DocumentationChangedEventArgs("Transmitter", "Output power"));
			}
			else
			{
				if (sender != gBoxOverloadCurrentProtection)
					return;
				OnDocumentationChanged(new DocumentationChangedEventArgs("Transmitter", "Overload current protection"));
			}
		}

		private void control_MouseLeave(object sender, EventArgs e)
		{
			OnDocumentationChanged(new DocumentationChangedEventArgs(".", "Overview"));
		}

		private void OnDocumentationChanged(DocumentationChangedEventArgs e)
		{
			if (DocumentationChanged == null)
				return;
			DocumentationChanged((object)this, e);
		}
	}
}