using SemtechLib.Controls;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.Devices.SX1231.Forms;
using SemtechLib.General.Events;
using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class TemperatureViewControl : UserControl, INotifyDocumentationChanged
	{
		private OperatingModeEnum mode = OperatingModeEnum.Stdby;
		private Decimal tempValueRoom = new Decimal(250, 0, 0, false, (byte)1);
		private bool tempCalDone;
		private IContainer components;
		private ErrorProvider errorProvider;
		private Button btnCalibrate;
		private TempCtrl thermometerCtrl;
		private Timer tmrTempMeasStart;
		private Led ledTempMeasRunning;
		private Label lblMeasuring;
		private Panel pnlAdcLowPower;
		private RadioButton rBtnAdcLowPowerOff;
		private RadioButton rBtnAdcLowPowerOn;
		private Label label3;
		private Panel panel1;

		public OperatingModeEnum Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
				switch (this.mode)
				{
					case OperatingModeEnum.Sleep:
					case OperatingModeEnum.Tx:
					case OperatingModeEnum.Rx:
						this.panel1.Enabled = false;
						this.thermometerCtrl.Enabled = false;
						break;
					case OperatingModeEnum.Stdby:
					case OperatingModeEnum.Fs:
						this.panel1.Enabled = true;
						if (!this.TempCalDone)
							break;
						this.thermometerCtrl.Enabled = true;
						break;
				}
			}
		}

		public bool TempMeasRunning
		{
			get
			{
				return this.ledTempMeasRunning.Checked;
			}
			set
			{
				this.ledTempMeasRunning.Checked = value;
			}
		}

		public bool AdcLowPowerOn
		{
			get
			{
				return this.rBtnAdcLowPowerOn.Checked;
			}
			set
			{
				this.rBtnAdcLowPowerOn.CheckedChanged -= new EventHandler(this.rBtnAdcLowPower_CheckedChanged);
				this.rBtnAdcLowPowerOff.CheckedChanged -= new EventHandler(this.rBtnAdcLowPower_CheckedChanged);
				if (value)
				{
					this.rBtnAdcLowPowerOn.Checked = true;
					this.rBtnAdcLowPowerOff.Checked = false;
				}
				else
				{
					this.rBtnAdcLowPowerOn.Checked = false;
					this.rBtnAdcLowPowerOff.Checked = true;
				}
				this.rBtnAdcLowPowerOn.CheckedChanged += new EventHandler(this.rBtnAdcLowPower_CheckedChanged);
				this.rBtnAdcLowPowerOff.CheckedChanged += new EventHandler(this.rBtnAdcLowPower_CheckedChanged);
			}
		}

		public Decimal TempValue
		{
			get
			{
				return (Decimal)this.thermometerCtrl.Value;
			}
			set
			{
				this.thermometerCtrl.Value = (double)value;
			}
		}

		public bool TempCalDone
		{
			get
			{
				return this.tempCalDone;
			}
			set
			{
				this.tempCalDone = value;
				this.thermometerCtrl.Enabled = value;
			}
		}

		public Decimal TempValueRoom
		{
			get
			{
				return this.tempValueRoom;
			}
			set
			{
				this.tempValueRoom = value;
			}
		}

		public event BooleanEventHandler AdcLowPowerOnChanged;

		public event DecimalEventHandler TempCalibrateChanged;

		public event DocumentationChangedEventHandler DocumentationChanged;

		public TemperatureViewControl()
		{
			this.InitializeComponent();
		}

		private void OnAdcLowPowerOnChanged(bool value)
		{
			if (this.AdcLowPowerOnChanged == null)
				return;
			this.AdcLowPowerOnChanged((object)this, new BooleanEventArg(value));
		}

		private void OnTempCalibrateChanged(Decimal value)
		{
			if (this.TempCalibrateChanged == null)
				return;
			this.TempCalibrateChanged((object)this, new DecimalEventArg(value));
		}

		private void rBtnAdcLowPower_CheckedChanged(object sender, EventArgs e)
		{
			this.AdcLowPowerOn = this.rBtnAdcLowPowerOn.Checked;
			this.OnAdcLowPowerOnChanged(this.AdcLowPowerOn);
		}

		private void btnTempCalibrate_Click(object sender, EventArgs e)
		{
			TemperatureCalibrationForm temperatureCalibrationForm = new TemperatureCalibrationForm();
			temperatureCalibrationForm.TempValueRoom = this.TempValueRoom;
			if (temperatureCalibrationForm.ShowDialog() != DialogResult.OK)
				return;
			try
			{
				this.Cursor = Cursors.WaitCursor;
				this.TempValueRoom = temperatureCalibrationForm.TempValueRoom;
				this.OnTempCalibrateChanged(this.TempValueRoom);
			}
			catch
			{
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			if (sender == this.thermometerCtrl)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Temperature", "Thermometer"));
			else if (sender == this.btnCalibrate)
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Temperature", "Calibrate"));
			else if (sender == this.ledTempMeasRunning || sender == this.lblMeasuring)
			{
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Temperature", "Measure running"));
			}
			else
			{
				if (sender != this.panel1 && sender != this.rBtnAdcLowPowerOn && sender != this.rBtnAdcLowPowerOff)
					return;
				this.OnDocumentationChanged(new DocumentationChangedEventArgs("Temperature", "Adc low power"));
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
			this.btnCalibrate = new Button();
			this.lblMeasuring = new Label();
			this.tmrTempMeasStart = new Timer(this.components);
			this.pnlAdcLowPower = new Panel();
			this.rBtnAdcLowPowerOff = new RadioButton();
			this.rBtnAdcLowPowerOn = new RadioButton();
			this.label3 = new Label();
			this.panel1 = new Panel();
			this.ledTempMeasRunning = new Led();
			this.thermometerCtrl = new TempCtrl();
			this.pnlAdcLowPower.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			this.errorProvider.ContainerControl = (ContainerControl)this;
			this.btnCalibrate.Location = new Point(37, 3);
			this.btnCalibrate.Name = "btnCalibrate";
			this.btnCalibrate.Size = new Size(75, 21);
			this.btnCalibrate.TabIndex = 0;
			this.btnCalibrate.Text = "Calibrate";
			this.btnCalibrate.UseVisualStyleBackColor = true;
			this.btnCalibrate.Click += new EventHandler(this.btnTempCalibrate_Click);
			this.btnCalibrate.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.btnCalibrate.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.lblMeasuring.AutoSize = true;
			this.lblMeasuring.Location = new Point(34, 27);
			this.lblMeasuring.Name = "lblMeasuring";
			this.lblMeasuring.Size = new Size(65, 12);
			this.lblMeasuring.TabIndex = 1;
			this.lblMeasuring.Text = "Measuring:";
			this.lblMeasuring.TextAlign = ContentAlignment.MiddleLeft;
			this.lblMeasuring.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.lblMeasuring.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.tmrTempMeasStart.Interval = 1000;
			this.pnlAdcLowPower.AutoSize = true;
			this.pnlAdcLowPower.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlAdcLowPower.Controls.Add((Control)this.rBtnAdcLowPowerOff);
			this.pnlAdcLowPower.Controls.Add((Control)this.rBtnAdcLowPowerOn);
			this.pnlAdcLowPower.Location = new Point(694, 431);
			this.pnlAdcLowPower.Name = "pnlAdcLowPower";
			this.pnlAdcLowPower.Size = new Size(98, 22);
			this.pnlAdcLowPower.TabIndex = 3;
			this.pnlAdcLowPower.Visible = false;
			this.pnlAdcLowPower.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.pnlAdcLowPower.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnAdcLowPowerOff.AutoSize = true;
			this.rBtnAdcLowPowerOff.Location = new Point(54, 3);
			this.rBtnAdcLowPowerOff.Name = "rBtnAdcLowPowerOff";
			this.rBtnAdcLowPowerOff.Size = new Size(41, 16);
			this.rBtnAdcLowPowerOff.TabIndex = 1;
			this.rBtnAdcLowPowerOff.Text = "OFF";
			this.rBtnAdcLowPowerOff.UseVisualStyleBackColor = true;
			this.rBtnAdcLowPowerOff.CheckedChanged += new EventHandler(this.rBtnAdcLowPower_CheckedChanged);
			this.rBtnAdcLowPowerOff.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnAdcLowPowerOff.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.rBtnAdcLowPowerOn.AutoSize = true;
			this.rBtnAdcLowPowerOn.Checked = true;
			this.rBtnAdcLowPowerOn.Location = new Point(3, 3);
			this.rBtnAdcLowPowerOn.Name = "rBtnAdcLowPowerOn";
			this.rBtnAdcLowPowerOn.Size = new Size(35, 16);
			this.rBtnAdcLowPowerOn.TabIndex = 0;
			this.rBtnAdcLowPowerOn.TabStop = true;
			this.rBtnAdcLowPowerOn.Text = "ON";
			this.rBtnAdcLowPowerOn.UseVisualStyleBackColor = true;
			this.rBtnAdcLowPowerOn.CheckedChanged += new EventHandler(this.rBtnAdcLowPower_CheckedChanged);
			this.rBtnAdcLowPowerOn.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.rBtnAdcLowPowerOn.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(602, 436);
			this.label3.Name = "label3";
			this.label3.Size = new Size(89, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "ADC low power:";
			this.label3.TextAlign = ContentAlignment.MiddleLeft;
			this.label3.Visible = false;
			this.label3.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.label3.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.panel1.Controls.Add((Control)this.btnCalibrate);
			this.panel1.Controls.Add((Control)this.lblMeasuring);
			this.panel1.Controls.Add((Control)this.ledTempMeasRunning);
			this.panel1.Location = new Point(325, 409);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(148, 43);
			this.panel1.TabIndex = 1;
			this.panel1.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.panel1.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.ledTempMeasRunning.BackColor = Color.Transparent;
			this.ledTempMeasRunning.LedColor = Color.Green;
			this.ledTempMeasRunning.LedSize = new Size(11, 11);
			this.ledTempMeasRunning.Location = new Point(99, 27);
			this.ledTempMeasRunning.Name = "ledTempMeasRunning";
			this.ledTempMeasRunning.Size = new Size(15, 14);
			this.ledTempMeasRunning.TabIndex = 2;
			this.ledTempMeasRunning.Text = "Measuring";
			this.ledTempMeasRunning.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.ledTempMeasRunning.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.thermometerCtrl.BackColor = Color.Transparent;
			this.thermometerCtrl.DrawTics = true;
			this.thermometerCtrl.Enabled = false;
			this.thermometerCtrl.ForeColor = Color.Red;
			this.thermometerCtrl.LargeTicFreq = 10;
			this.thermometerCtrl.Location = new Point(325, 3);
			this.thermometerCtrl.Name = "thermometerCtrl";
			this.thermometerCtrl.Range.Max = 90.0;
			this.thermometerCtrl.Range.Min = -40.0;
			this.thermometerCtrl.Size = new Size(148, 401);
			this.thermometerCtrl.SmallTicFreq = 5;
			this.thermometerCtrl.TabIndex = 0;
			this.thermometerCtrl.Text = "Thermometer";
			this.thermometerCtrl.Value = 25.0;
			this.thermometerCtrl.MouseEnter += new EventHandler(this.control_MouseEnter);
			this.thermometerCtrl.MouseLeave += new EventHandler(this.control_MouseLeave);
			this.AutoScaleDimensions = new SizeF(6f, 12f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add((Control)this.thermometerCtrl);
			this.Controls.Add((Control)this.pnlAdcLowPower);
			this.Controls.Add((Control)this.panel1);
			this.Controls.Add((Control)this.label3);
			this.Name = "TemperatureViewControl";
			this.Size = new Size(799, 455);
			this.pnlAdcLowPower.ResumeLayout(false);
			this.pnlAdcLowPower.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}