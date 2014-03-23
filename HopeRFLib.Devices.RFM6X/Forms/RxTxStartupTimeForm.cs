using SemtechLib.Controls;
using SemtechLib.Devices.SX1231;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.General;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Forms
{
	public class RxTxStartupTimeForm : Form
	{
		private string unit = "";
		private const double Tana = 2E-05;
		private const double PllBw = 300000.0;
		private const double Tpllafc = 1.66666666666667E-05;
		private double txTsTr;
		private double rxTsRe;
		private ushort rxTsRePreambleSize;
		private double rxTsReAgc;
		private ushort rxTsReAgcPreambleSize;
		private double rxTsReAgcAfc;
		private ushort rxTsReAgcAfcPreambleSize;
		private ApplicationSettings appSettings;
		private SX1231 sx1231;
		private double Tcf;
		private double Tdcc;
		private double Tafc;
		private double Trssi;
		private GroupBoxEx gBoxTxStartupTime;
		private Label lblTsTrUnit;
		private Label lblTsTrValue;
		private Label lblTsTr;
		private GroupBoxEx gBoxRxStartupTime;
		private Label lblTsReAgcAfcPreambleUnit;
		private Label lblTsReAgcPreambleUnit;
		private Label lblTsRePreambleUnit;
		private Label lblTsReAgcAfcUnit;
		private Label lblTsReAgcUnit;
		private Label lblTsReUnit;
		private Label lblTsReAgcAfcPreambleValue;
		private Label lblTsReAgcAfcPreamble;
		private Label lblTsReAgcPreambleValue;
		private Label lblTsReAgcPreamble;
		private Label lblTsReAgcAfcValue;
		private Label lblTsRePreambleValue;
		private Label lblTsReAgcValue;
		private Label lblTsReAgcAfc;
		private Label lblTsRePreamble;
		private Label lblTsReAgc;
		private Label lblTsReValue;
		private Label lblTsRe;
		private Label label4;
		private Label label3;

		private double TxTsTr
		{
			get
			{
				return txTsTr;
			}
			set
			{
				txTsTr = value;
				unit = "s";
				lblTsTrValue.Text = EngineeringNotation.ToString(txTsTr, ref unit);
				lblTsTrUnit.Text = unit;
			}
		}

		private double RxTsRe
		{
			get
			{
				return rxTsRe;
			}
			set
			{
				rxTsRe = value;
				unit = "s";
				lblTsReValue.Text = EngineeringNotation.ToString(rxTsRe, ref unit);
				lblTsReUnit.Text = unit;
			}
		}

		private ushort RxTsRePreambleSize
		{
			get
			{
				return rxTsRePreambleSize;
			}
			set
			{
				rxTsRePreambleSize = value;
				unit = "bytes";
				lblTsRePreambleValue.Text = rxTsRePreambleSize.ToString();
				lblTsRePreambleUnit.Text = unit;
			}
		}

		private double RxTsReAgc
		{
			get
			{
				return rxTsReAgc;
			}
			set
			{
				rxTsReAgc = value;
				unit = "s";
				lblTsReAgcValue.Text = EngineeringNotation.ToString(rxTsReAgc, ref unit);
				lblTsReAgcUnit.Text = unit;
			}
		}

		private ushort RxTsReAgcPreambleSize
		{
			get
			{
				return rxTsReAgcPreambleSize;
			}
			set
			{
				rxTsReAgcPreambleSize = value;
				unit = "bytes";
				lblTsReAgcPreambleValue.Text = rxTsReAgcPreambleSize.ToString();
				lblTsReAgcPreambleUnit.Text = unit;
			}
		}

		private double RxTsReAgcAfc
		{
			get
			{
				return rxTsReAgcAfc;
			}
			set
			{
				rxTsReAgcAfc = value;
				unit = "s";
				lblTsReAgcAfcValue.Text = EngineeringNotation.ToString(rxTsReAgcAfc, ref unit);
				lblTsReAgcAfcUnit.Text = unit;
			}
		}

		private ushort RxTsReAgcAfcPreambleSize
		{
			get
			{
				return rxTsReAgcAfcPreambleSize;
			}
			set
			{
				rxTsReAgcAfcPreambleSize = value;
				unit = "bytes";
				lblTsReAgcAfcPreambleValue.Text = rxTsReAgcAfcPreambleSize.ToString();
				lblTsReAgcAfcPreambleUnit.Text = unit;
			}
		}

		public ApplicationSettings AppSettings
		{
			get
			{
				return appSettings;
			}
			set
			{
				appSettings = value;
			}
		}

		public SX1231 SX1231
		{
			set
			{
				if (sx1231 == value)
					return;
				sx1231 = value;
				sx1231.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(sx1231_PropertyChanged);
				ComputeStartupTimmings();
			}
		}

		public RxTxStartupTimeForm()
		{
			InitializeComponent();
		}

		private bool IsFormLocatedInScreen(Form frm, Screen[] screens)
		{
			int upperBound = screens.GetUpperBound(0);
			bool flag = false;
			for (int index = 0; index <= upperBound; ++index)
			{
				if (frm.Left < screens[index].WorkingArea.Left || frm.Top < screens[index].WorkingArea.Top || (frm.Left > screens[index].WorkingArea.Right || frm.Top > screens[index].WorkingArea.Bottom))
				{
					flag = false;
				}
				else
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		private void ComputeStartupTimmings()
		{
			if (sx1231.ModulationType == ModulationTypeEnum.OOK)
			{
				Tcf = 34.0 / (4.0 * (double)sx1231.RxBw);
				TxTsTr = 5E-06 + 0.5 * (double)sx1231.Tbit;
			}
			else
			{
				Tcf = 21.0 / (4.0 * (double)sx1231.RxBw);
				double num = 4E-05;
				switch ((byte)sx1231.PaRamp)
				{
					case (byte)0:
						num = 0.0034;
						break;
					case (byte)1:
						num = 0.002;
						break;
					case (byte)2:
						num = 0.001;
						break;
					case (byte)3:
						num = 0.0005;
						break;
					case (byte)4:
						num = 0.00025;
						break;
					case (byte)5:
						num = 0.000125;
						break;
					case (byte)6:
						num = 0.0001;
						break;
					case (byte)7:
						num = 6.2E-05;
						break;
					case (byte)8:
						num = 5E-05;
						break;
					case (byte)9:
						num = 4E-05;
						break;
					case (byte)10:
						num = 3.1E-05;
						break;
					case (byte)11:
						num = 2.5E-05;
						break;
					case (byte)12:
						num = 2E-05;
						break;
					case (byte)13:
						num = 1.5E-05;
						break;
					case (byte)14:
						num = 1.2E-05;
						break;
					case (byte)15:
						num = 1E-05;
						break;
				}
				TxTsTr = 5E-06 + 1.25 * num + 0.5 * (double)sx1231.Tbit;
			}
			Tdcc = Math.Max(8.0, Math.Pow(2.0, Math.Round(Math.Log(8.0 * (double)sx1231.RxBw * (double)sx1231.Tbit, 2.0) + 1.0, MidpointRounding.AwayFromZero))) / (4.0 * (double)sx1231.RxBw);
			Tafc = 4.0 * (double)sx1231.Tbit;
			Trssi = 2.0 * Math.Round(4.0 * (double)sx1231.RxBw * (double)sx1231.Tbit) / (4.0 * (double)sx1231.RxBw);
			RxTsRe = 2E-05 + Tcf + Tdcc + 2.0 * Trssi;
			RxTsRePreambleSize = (ushort)2;
			RxTsReAgc = 2E-05 + 2.0 * Tcf + 2.0 * Tdcc + 3.0 * Trssi;
			RxTsReAgcPreambleSize = (ushort)((18.0 + (Tcf + Tdcc) * (double)sx1231.BitRate) / 8.0);
			if ((int)RxTsReAgcPreambleSize % 8 != 0)
				RxTsReAgcPreambleSize = (ushort)((uint)RxTsReAgcPreambleSize + 1U);
			RxTsReAgcAfc = 2E-05 + 3.0 * Tcf + 3.0 * Tdcc + 3.0 * Trssi + Tafc + 1.66666666666667E-05;
			RxTsReAgcAfcPreambleSize = (ushort)((22.0 + (2.0 * Tcf + 2.0 * Tdcc + 1.66666666666667E-05) * (double)sx1231.BitRate) / 8.0);
			if ((int)RxTsReAgcAfcPreambleSize % 8 == 0)
				return;
			RxTsReAgcAfcPreambleSize = (ushort)((uint)RxTsReAgcAfcPreambleSize + 1U);
		}

		private void OnSX1231PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "BitRate":
				case "PaRamp":
				case "RxBw":
					ComputeStartupTimmings();
					break;
			}
		}

		private void OnError(byte status, string message)
		{
			Refresh();
		}

		private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (InvokeRequired)
				BeginInvoke((Delegate)new RxTxStartupTimeForm.SX1231DataChangedDelegate(OnSX1231PropertyChanged), sender, (object)e);
			else
				OnSX1231PropertyChanged(sender, e);
		}

		private void RxTxStartupTimeForm_Load(object sender, EventArgs e)
		{
			try
			{
				string s1 = appSettings.GetValue("RxTxStartupTimeTop");
				if (s1 != null)
				{
					try
					{
						Top = int.Parse(s1);
					}
					catch
					{
						int num = (int)MessageBox.Show((IWin32Window)this, "Error getting Top value.");
					}
				}
				string s2 = appSettings.GetValue("RxTxStartupTimeLeft");
				if (s2 != null)
				{
					try
					{
						Left = int.Parse(s2);
					}
					catch
					{
						int num = (int)MessageBox.Show((IWin32Window)this, "Error getting Left value.");
					}
				}
				Screen[] allScreens = Screen.AllScreens;
				if (IsFormLocatedInScreen((Form)this, allScreens))
					return;
				Top = allScreens[0].WorkingArea.Top;
				Left = allScreens[0].WorkingArea.Left;
			}
			catch (Exception ex)
			{
				OnError((byte)1, ex.Message);
			}
		}

		private void RxTxStartupTimeForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				appSettings.SetValue("RxTxStartupTimeTop", Top.ToString());
				appSettings.SetValue("RxTxStartupTimeLeft", Left.ToString());
			}
			catch (Exception)
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(RxTxStartupTimeForm));
			gBoxRxStartupTime = new GroupBoxEx();
			lblTsReAgcAfcPreambleUnit = new Label();
			lblTsReAgcPreambleUnit = new Label();
			lblTsRePreambleUnit = new Label();
			lblTsReAgcAfcUnit = new Label();
			lblTsReAgcUnit = new Label();
			lblTsReUnit = new Label();
			lblTsReAgcAfcPreambleValue = new Label();
			lblTsReAgcAfcPreamble = new Label();
			lblTsReAgcPreambleValue = new Label();
			lblTsReAgcPreamble = new Label();
			lblTsReAgcAfcValue = new Label();
			lblTsRePreambleValue = new Label();
			lblTsReAgcValue = new Label();
			lblTsReAgcAfc = new Label();
			lblTsRePreamble = new Label();
			lblTsReAgc = new Label();
			lblTsReValue = new Label();
			lblTsRe = new Label();
			gBoxTxStartupTime = new GroupBoxEx();
			lblTsTrUnit = new Label();
			lblTsTrValue = new Label();
			lblTsTr = new Label();
			label4 = new Label();
			label3 = new Label();
			gBoxRxStartupTime.SuspendLayout();
			gBoxTxStartupTime.SuspendLayout();
			SuspendLayout();
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcAfcPreambleUnit);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcPreambleUnit);
			gBoxRxStartupTime.Controls.Add((Control)lblTsRePreambleUnit);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcAfcUnit);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcUnit);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReUnit);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcAfcPreambleValue);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcAfcPreamble);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcPreambleValue);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcPreamble);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcAfcValue);
			gBoxRxStartupTime.Controls.Add((Control)lblTsRePreambleValue);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcValue);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgcAfc);
			gBoxRxStartupTime.Controls.Add((Control)lblTsRePreamble);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReAgc);
			gBoxRxStartupTime.Controls.Add((Control)lblTsReValue);
			gBoxRxStartupTime.Controls.Add((Control)lblTsRe);
			gBoxRxStartupTime.Location = new Point(12, 75);
			gBoxRxStartupTime.Name = "gBoxRxStartupTime";
			gBoxRxStartupTime.Size = new Size(324, 186);
			gBoxRxStartupTime.TabIndex = 4;
			gBoxRxStartupTime.TabStop = false;
			gBoxRxStartupTime.Text = "Rx startup time";
			lblTsReAgcAfcPreambleUnit.AutoSize = true;
			lblTsReAgcAfcPreambleUnit.Location = new Point(275, 157);
			lblTsReAgcAfcPreambleUnit.Margin = new Padding(1);
			lblTsReAgcAfcPreambleUnit.MinimumSize = new Size(40, 18);
			lblTsReAgcAfcPreambleUnit.Name = "lblTsReAgcAfcPreambleUnit";
			lblTsReAgcAfcPreambleUnit.Size = new Size(40, 18);
			lblTsReAgcAfcPreambleUnit.TabIndex = 26;
			lblTsReAgcAfcPreambleUnit.Text = "bytes";
			lblTsReAgcAfcPreambleUnit.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReAgcPreambleUnit.AutoSize = true;
			lblTsReAgcPreambleUnit.Location = new Point(275, 99);
			lblTsReAgcPreambleUnit.Margin = new Padding(1);
			lblTsReAgcPreambleUnit.MinimumSize = new Size(40, 18);
			lblTsReAgcPreambleUnit.Name = "lblTsReAgcPreambleUnit";
			lblTsReAgcPreambleUnit.Size = new Size(40, 18);
			lblTsReAgcPreambleUnit.TabIndex = 26;
			lblTsReAgcPreambleUnit.Text = "bytes";
			lblTsReAgcPreambleUnit.TextAlign = ContentAlignment.MiddleLeft;
			lblTsRePreambleUnit.AutoSize = true;
			lblTsRePreambleUnit.Location = new Point(275, 45);
			lblTsRePreambleUnit.Margin = new Padding(1);
			lblTsRePreambleUnit.MinimumSize = new Size(40, 18);
			lblTsRePreambleUnit.Name = "lblTsRePreambleUnit";
			lblTsRePreambleUnit.Size = new Size(40, 18);
			lblTsRePreambleUnit.TabIndex = 26;
			lblTsRePreambleUnit.Text = "bytes";
			lblTsRePreambleUnit.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReAgcAfcUnit.AutoSize = true;
			lblTsReAgcAfcUnit.Location = new Point(275, 137);
			lblTsReAgcAfcUnit.Margin = new Padding(1);
			lblTsReAgcAfcUnit.MinimumSize = new Size(40, 18);
			lblTsReAgcAfcUnit.Name = "lblTsReAgcAfcUnit";
			lblTsReAgcAfcUnit.Size = new Size(40, 18);
			lblTsReAgcAfcUnit.TabIndex = 26;
			lblTsReAgcAfcUnit.Text = "μs";
			lblTsReAgcAfcUnit.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReAgcUnit.AutoSize = true;
			lblTsReAgcUnit.Location = new Point(275, 78);
			lblTsReAgcUnit.Margin = new Padding(1);
			lblTsReAgcUnit.MinimumSize = new Size(40, 18);
			lblTsReAgcUnit.Name = "lblTsReAgcUnit";
			lblTsReAgcUnit.Size = new Size(40, 18);
			lblTsReAgcUnit.TabIndex = 26;
			lblTsReAgcUnit.Text = "μs";
			lblTsReAgcUnit.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReUnit.AutoSize = true;
			lblTsReUnit.Location = new Point(275, 25);
			lblTsReUnit.Margin = new Padding(1);
			lblTsReUnit.MinimumSize = new Size(40, 18);
			lblTsReUnit.Name = "lblTsReUnit";
			lblTsReUnit.Size = new Size(40, 18);
			lblTsReUnit.TabIndex = 26;
			lblTsReUnit.Text = "μs";
			lblTsReUnit.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReAgcAfcPreambleValue.AutoSize = true;
			lblTsReAgcAfcPreambleValue.Location = new Point(193, 157);
			lblTsReAgcAfcPreambleValue.Margin = new Padding(1);
			lblTsReAgcAfcPreambleValue.MinimumSize = new Size(80, 18);
			lblTsReAgcAfcPreambleValue.Name = "lblTsReAgcAfcPreambleValue";
			lblTsReAgcAfcPreambleValue.Size = new Size(80, 18);
			lblTsReAgcAfcPreambleValue.TabIndex = 25;
			lblTsReAgcAfcPreambleValue.Text = "0.000";
			lblTsReAgcAfcPreambleValue.TextAlign = ContentAlignment.MiddleRight;
			lblTsReAgcAfcPreamble.AutoSize = true;
			lblTsReAgcAfcPreamble.Location = new Point(3, 157);
			lblTsReAgcAfcPreamble.Margin = new Padding(1);
			lblTsReAgcAfcPreamble.MinimumSize = new Size(0, 18);
			lblTsReAgcAfcPreamble.Name = "lblTsReAgcAfcPreamble";
			lblTsReAgcAfcPreamble.Size = new Size(137, 18);
			lblTsReAgcAfcPreamble.TabIndex = 24;
			lblTsReAgcAfcPreamble.Text = "Minimum preamble size:";
			lblTsReAgcAfcPreamble.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReAgcPreambleValue.AutoSize = true;
			lblTsReAgcPreambleValue.Location = new Point(193, 99);
			lblTsReAgcPreambleValue.Margin = new Padding(1);
			lblTsReAgcPreambleValue.MinimumSize = new Size(80, 18);
			lblTsReAgcPreambleValue.Name = "lblTsReAgcPreambleValue";
			lblTsReAgcPreambleValue.Size = new Size(80, 18);
			lblTsReAgcPreambleValue.TabIndex = 25;
			lblTsReAgcPreambleValue.Text = "0.000";
			lblTsReAgcPreambleValue.TextAlign = ContentAlignment.MiddleRight;
			lblTsReAgcPreamble.AutoSize = true;
			lblTsReAgcPreamble.Location = new Point(3, 99);
			lblTsReAgcPreamble.Margin = new Padding(1);
			lblTsReAgcPreamble.MinimumSize = new Size(0, 18);
			lblTsReAgcPreamble.Name = "lblTsReAgcPreamble";
			lblTsReAgcPreamble.Size = new Size(137, 18);
			lblTsReAgcPreamble.TabIndex = 24;
			lblTsReAgcPreamble.Text = "Minimum preamble size:";
			lblTsReAgcPreamble.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReAgcAfcValue.AutoSize = true;
			lblTsReAgcAfcValue.Location = new Point(193, 137);
			lblTsReAgcAfcValue.Margin = new Padding(1);
			lblTsReAgcAfcValue.MinimumSize = new Size(80, 18);
			lblTsReAgcAfcValue.Name = "lblTsReAgcAfcValue";
			lblTsReAgcAfcValue.Size = new Size(80, 18);
			lblTsReAgcAfcValue.TabIndex = 25;
			lblTsReAgcAfcValue.Text = "0.000";
			lblTsReAgcAfcValue.TextAlign = ContentAlignment.MiddleRight;
			lblTsRePreambleValue.AutoSize = true;
			lblTsRePreambleValue.Location = new Point(193, 45);
			lblTsRePreambleValue.Margin = new Padding(1);
			lblTsRePreambleValue.MinimumSize = new Size(80, 18);
			lblTsRePreambleValue.Name = "lblTsRePreambleValue";
			lblTsRePreambleValue.Size = new Size(80, 18);
			lblTsRePreambleValue.TabIndex = 25;
			lblTsRePreambleValue.Text = "0.000";
			lblTsRePreambleValue.TextAlign = ContentAlignment.MiddleRight;
			lblTsReAgcValue.AutoSize = true;
			lblTsReAgcValue.Location = new Point(193, 78);
			lblTsReAgcValue.Margin = new Padding(1);
			lblTsReAgcValue.MinimumSize = new Size(80, 18);
			lblTsReAgcValue.Name = "lblTsReAgcValue";
			lblTsReAgcValue.Size = new Size(80, 18);
			lblTsReAgcValue.TabIndex = 25;
			lblTsReAgcValue.Text = "0.000";
			lblTsReAgcValue.TextAlign = ContentAlignment.MiddleRight;
			lblTsReAgcAfc.AutoSize = true;
			lblTsReAgcAfc.Location = new Point(3, 137);
			lblTsReAgcAfc.Margin = new Padding(1);
			lblTsReAgcAfc.MinimumSize = new Size(0, 18);
			lblTsReAgcAfc.Name = "lblTsReAgcAfc";
			lblTsReAgcAfc.Size = new Size(89, 18);
			lblTsReAgcAfc.TabIndex = 24;
			lblTsReAgcAfc.Text = "TS_RE_AGC_AFC:";
			lblTsReAgcAfc.TextAlign = ContentAlignment.MiddleLeft;
			lblTsRePreamble.AutoSize = true;
			lblTsRePreamble.Location = new Point(3, 45);
			lblTsRePreamble.Margin = new Padding(1);
			lblTsRePreamble.MinimumSize = new Size(0, 18);
			lblTsRePreamble.Name = "lblTsRePreamble";
			lblTsRePreamble.Size = new Size(137, 18);
			lblTsRePreamble.TabIndex = 24;
			lblTsRePreamble.Text = "Minimum preamble size:";
			lblTsRePreamble.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReAgc.AutoSize = true;
			lblTsReAgc.Location = new Point(3, 78);
			lblTsReAgc.Margin = new Padding(1);
			lblTsReAgc.MinimumSize = new Size(0, 18);
			lblTsReAgc.Name = "lblTsReAgc";
			lblTsReAgc.Size = new Size(65, 18);
			lblTsReAgc.TabIndex = 24;
			lblTsReAgc.Text = "TS_RE_AGC:";
			lblTsReAgc.TextAlign = ContentAlignment.MiddleLeft;
			lblTsReValue.AutoSize = true;
			lblTsReValue.Location = new Point(193, 25);
			lblTsReValue.Margin = new Padding(1);
			lblTsReValue.MinimumSize = new Size(80, 18);
			lblTsReValue.Name = "lblTsReValue";
			lblTsReValue.Size = new Size(80, 18);
			lblTsReValue.TabIndex = 25;
			lblTsReValue.Text = "0.000";
			lblTsReValue.TextAlign = ContentAlignment.MiddleRight;
			lblTsRe.AutoSize = true;
			lblTsRe.Location = new Point(3, 25);
			lblTsRe.Margin = new Padding(1);
			lblTsRe.MinimumSize = new Size(0, 18);
			lblTsRe.Name = "lblTsRe";
			lblTsRe.Size = new Size(41, 18);
			lblTsRe.TabIndex = 24;
			lblTsRe.Text = "TS_RE:";
			lblTsRe.TextAlign = ContentAlignment.MiddleLeft;
			gBoxTxStartupTime.Controls.Add((Control)lblTsTrUnit);
			gBoxTxStartupTime.Controls.Add((Control)lblTsTrValue);
			gBoxTxStartupTime.Controls.Add((Control)lblTsTr);
			gBoxTxStartupTime.Location = new Point(12, 11);
			gBoxTxStartupTime.Name = "gBoxTxStartupTime";
			gBoxTxStartupTime.Size = new Size(324, 58);
			gBoxTxStartupTime.TabIndex = 4;
			gBoxTxStartupTime.TabStop = false;
			gBoxTxStartupTime.Text = "Tx startup time";
			lblTsTrUnit.AutoSize = true;
			lblTsTrUnit.Location = new Point(275, 25);
			lblTsTrUnit.Margin = new Padding(1);
			lblTsTrUnit.MinimumSize = new Size(40, 18);
			lblTsTrUnit.Name = "lblTsTrUnit";
			lblTsTrUnit.Size = new Size(40, 18);
			lblTsTrUnit.TabIndex = 26;
			lblTsTrUnit.Text = "μs";
			lblTsTrUnit.TextAlign = ContentAlignment.MiddleLeft;
			lblTsTrValue.AutoSize = true;
			lblTsTrValue.Location = new Point(193, 25);
			lblTsTrValue.Margin = new Padding(1);
			lblTsTrValue.MinimumSize = new Size(80, 18);
			lblTsTrValue.Name = "lblTsTrValue";
			lblTsTrValue.Size = new Size(80, 18);
			lblTsTrValue.TabIndex = 25;
			lblTsTrValue.Text = "0.000";
			lblTsTrValue.TextAlign = ContentAlignment.MiddleRight;
			lblTsTr.AutoSize = true;
			lblTsTr.Location = new Point(3, 25);
			lblTsTr.Margin = new Padding(1);
			lblTsTr.MinimumSize = new Size(0, 18);
			lblTsTr.Name = "lblTsTr";
			lblTsTr.Size = new Size(41, 18);
			lblTsTr.TabIndex = 24;
			lblTsTr.Text = "TS_TR:";
			lblTsTr.TextAlign = ContentAlignment.MiddleLeft;
			label4.AutoSize = true;
			label4.Location = new Point(59, 272);
			label4.Name = "label4";
			label4.Size = new Size(233, 12);
			label4.TabIndex = 6;
			label4.Text = "See drawings section 4.2 of datasheet.";
			label3.AutoSize = true;
			label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			label3.Location = new Point(15, 272);
			label3.Name = "label3";
			label3.Size = new Size(38, 13);
			label3.TabIndex = 5;
			label3.Text = "Note:";
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new Size(346, 294);
			Controls.Add((Control)label4);
			Controls.Add((Control)label3);
			Controls.Add((Control)gBoxRxStartupTime);
			Controls.Add((Control)gBoxTxStartupTime);
			DoubleBuffered = true;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Icon = (Icon)resources.GetObject("$Icon");
			KeyPreview = true;
			MaximizeBox = false;
			Name = "RxTxStartupTimeForm";
			Text = "RxTx Startup Time";
			FormClosed += new FormClosedEventHandler(RxTxStartupTimeForm_FormClosed);
			Load += new EventHandler(RxTxStartupTimeForm_Load);
			gBoxRxStartupTime.ResumeLayout(false);
			gBoxRxStartupTime.PerformLayout();
			gBoxTxStartupTime.ResumeLayout(false);
			gBoxTxStartupTime.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		private delegate void SX1231DataChangedDelegate(object sender, PropertyChangedEventArgs e);
	}
}