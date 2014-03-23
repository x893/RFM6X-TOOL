using SemtechLib.Controls;
using SemtechLib.Devices.SX1231;
using SemtechLib.Devices.SX1231.Controls;
using SemtechLib.Devices.SX1231.Enumerations;
using SemtechLib.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace SemtechLib.Devices.SX1231.Forms
{
	public class SpectrumAnalyserForm : Form
	{
		private Decimal rxBw = new Decimal(10417);
		private PointPairList points;
		private ApplicationSettings appSettings;
		private SX1231 sx1231;
		private Panel panel1;
		private Panel panel2;
		private SpectrumGraphControl graph;
		private NumericUpDownEx nudFreqCenter;
		private NumericUpDownEx nudFreqSpan;
		private NumericUpDownEx nudChanBw;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private ComboBox cBoxLanGainSelect;
		private System.Windows.Forms.Label label7;

		private Decimal FrequencyRf
		{
			get
			{
				return nudFreqCenter.Value;
			}
			set
			{
				try
				{
					nudFreqCenter.ValueChanged -= new EventHandler(nudFreqCenter_ValueChanged);
					nudFreqCenter.Value = (Decimal)(uint)Math.Round(value / sx1231.FrequencyStep, MidpointRounding.AwayFromZero) * sx1231.FrequencyStep;
				}
				catch (Exception)
				{
					nudFreqCenter.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudFreqCenter.ValueChanged += new EventHandler(nudFreqCenter_ValueChanged);
				}
			}
		}

		private Decimal FrequencySpan
		{
			get
			{
				return nudFreqSpan.Value;
			}
			set
			{
				try
				{
					nudFreqSpan.ValueChanged -= new EventHandler(nudFreqSpan_ValueChanged);
					nudFreqSpan.Value = (Decimal)(uint)Math.Round(value / sx1231.FrequencyStep, MidpointRounding.AwayFromZero) * sx1231.FrequencyStep;
				}
				catch (Exception)
				{
					nudFreqSpan.BackColor = ControlPaint.LightLight(Color.Red);
				}
				finally
				{
					nudFreqSpan.ValueChanged += new EventHandler(nudFreqSpan_ValueChanged);
				}
			}
		}

		private Decimal RxBw
		{
			get
			{
				return rxBw;
			}
			set
			{
				try
				{
					nudChanBw.ValueChanged -= new EventHandler(nudChanBw_ValueChanged);
					int mant = 0;
					int exp = 0;
					SX1231.ComputeRxBwMantExp(sx1231.FrequencyXo, sx1231.ModulationType, value, ref mant, ref exp);
					rxBw = SX1231.ComputeRxBw(sx1231.FrequencyXo, sx1231.ModulationType, mant, exp);
					nudChanBw.Value = rxBw;
				}
				catch (Exception)
				{
				}
				finally
				{
					nudChanBw.ValueChanged += new EventHandler(nudChanBw_ValueChanged);
				}
			}
		}

		private LnaGainEnum LnaGainSelect
		{
			get
			{
				return (LnaGainEnum)(cBoxLanGainSelect.SelectedIndex + 1);
			}
			set
			{
				cBoxLanGainSelect.SelectedIndex = (int)(value - 1);
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
				FrequencyRf = sx1231.FrequencyRf;
				RxBw = sx1231.RxBw;
				LnaGainSelect = sx1231.LnaGainSelect;
				UpdatePointsList();
			}
		}

		public SpectrumAnalyserForm()
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

		private void UpdatePointsList()
		{
			GraphPane graphPane = ((List<GraphPane>)graph.PaneList)[0];
			graphPane.XAxis.Scale.Max = (double)sx1231.SpectrumFrequencyMax;
			graphPane.XAxis.Scale.Min = (double)sx1231.SpectrumFrequencyMin;
			sx1231.SpectrumFrequencyId = 0;
			points = new PointPairList();
			for (int index = 0; index < sx1231.SpectrumNbFrequenciesMax; ++index)
				points.Add(new PointPair((double)(sx1231.SpectrumFrequencyMin + sx1231.SpectrumFrequencyStep * (Decimal)index), -127.5));
			graphPane.CurveList[0] = (CurveItem)new LineItem("", (IPointList)points, Color.Yellow, SymbolType.None);
			graphPane.AxisChange();
			graph.Invalidate();
			graph.Refresh();
		}

		private void OnSX1231PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			try
			{
				switch (e.PropertyName)
				{
					case "FrequencyRf":
						FrequencyRf = sx1231.FrequencyRf;
						UpdatePointsList();
						break;
					case "RxBw":
						RxBw = sx1231.RxBw;
						UpdatePointsList();
						break;
					case "RxBwMin":
						nudChanBw.Minimum = sx1231.RxBwMin;
						break;
					case "RxBwMax":
						nudChanBw.Maximum = sx1231.RxBwMax;
						break;
					case "SpectrumFreqSpan":
						FrequencySpan = sx1231.SpectrumFrequencySpan;
						UpdatePointsList();
						break;
					case "LnaGainSelect":
						LnaGainSelect = sx1231.LnaGainSelect;
						break;
					case "SpectrumData":
						int num = sx1231.SpectrumRssiValue < new Decimal(-40) ? 1 : 0;
						graph.UpdateLineGraph(sx1231.SpectrumFrequencyId, (double)sx1231.SpectrumRssiValue);
						break;
				}
			}
			catch (Exception ex)
			{
				OnError((byte)1, ex.Message);
			}
		}

		private void OnError(byte status, string message)
		{
			Refresh();
		}

		private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (InvokeRequired)
				BeginInvoke((Delegate)new SpectrumAnalyserForm.SX1231DataChangedDelegate(OnSX1231PropertyChanged), sender, (object)e);
			else
				OnSX1231PropertyChanged(sender, e);
		}

		private void SpectrumAnalyserForm_Load(object sender, EventArgs e)
		{
			string s1 = appSettings.GetValue("SpectrumAnalyserTop");
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
			string s2 = appSettings.GetValue("SpectrumAnalyserLeft");
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
			if (!IsFormLocatedInScreen((Form)this, allScreens))
			{
				Top = allScreens[0].WorkingArea.Top;
				Left = allScreens[0].WorkingArea.Left;
			}
			sx1231.SpectrumOn = true;
		}

		private void SpectrumAnalyserForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				appSettings.SetValue("SpectrumAnalyserTop", Top.ToString());
				appSettings.SetValue("SpectrumAnalyserLeft", Left.ToString());
				sx1231.SpectrumOn = false;
			}
			catch (Exception)
			{
			}
		}

		private void nudFreqCenter_ValueChanged(object sender, EventArgs e)
		{
			FrequencyRf = nudFreqCenter.Value;
			sx1231.SetFrequencyRf(FrequencyRf);
		}

		private void nudFreqSpan_ValueChanged(object sender, EventArgs e)
		{
			FrequencySpan = nudFreqSpan.Value;
			sx1231.SpectrumFrequencySpan = FrequencySpan;
		}

		private void nudChanBw_ValueChanged(object sender, EventArgs e)
		{
			Decimal[] rxBwFreqTable = SX1231.ComputeRxBwFreqTable(sx1231.FrequencyXo, sx1231.ModulationType);
			int num1 = (int)(nudChanBw.Value - RxBw);
			int index;
			if (num1 >= -1 && num1 <= 1)
			{
				index = Array.IndexOf<Decimal>(rxBwFreqTable, RxBw) - num1;
			}
			else
			{
				int mant = 0;
				int exp = 0;
				Decimal num2 = new Decimal(0);
				SX1231.ComputeRxBwMantExp(sx1231.FrequencyXo, sx1231.ModulationType, nudChanBw.Value, ref mant, ref exp);
				Decimal rxBw = SX1231.ComputeRxBw(sx1231.FrequencyXo, sx1231.ModulationType, mant, exp);
				index = Array.IndexOf<Decimal>(rxBwFreqTable, rxBw);
			}
			nudChanBw.ValueChanged -= new EventHandler(nudChanBw_ValueChanged);
			nudChanBw.Value = rxBwFreqTable[index];
			nudChanBw.ValueChanged += new EventHandler(nudChanBw_ValueChanged);
			RxBw = nudChanBw.Value;
			sx1231.SetRxBw(RxBw);
		}

		private void cBoxLanGainSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			sx1231.SetLnaGainSelect(LnaGainSelect);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(SpectrumAnalyserForm));
			panel1 = new Panel();
			cBoxLanGainSelect = new ComboBox();
			nudFreqCenter = new NumericUpDownEx();
			nudFreqSpan = new NumericUpDownEx();
			nudChanBw = new NumericUpDownEx();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			panel2 = new Panel();
			graph = new SpectrumGraphControl();
			panel1.SuspendLayout();
			nudFreqCenter.BeginInit();
			nudFreqSpan.BeginInit();
			nudChanBw.BeginInit();
			panel2.SuspendLayout();
			SuspendLayout();
			panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			panel1.BackColor = Color.Black;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.Controls.Add((Control)cBoxLanGainSelect);
			panel1.Controls.Add((Control)nudFreqCenter);
			panel1.Controls.Add((Control)nudFreqSpan);
			panel1.Controls.Add((Control)nudChanBw);
			panel1.Controls.Add((Control)label2);
			panel1.Controls.Add((Control)label1);
			panel1.Controls.Add((Control)label6);
			panel1.Controls.Add((Control)label7);
			panel1.Controls.Add((Control)label3);
			panel1.Controls.Add((Control)label4);
			panel1.Controls.Add((Control)label5);
			panel1.Location = new Point(557, 0);
			panel1.Margin = new Padding(0);
			panel1.Name = "panel1";
			panel1.Size = new Size(223, 342);
			panel1.TabIndex = 0;
			cBoxLanGainSelect.FormattingEnabled = true;
			cBoxLanGainSelect.Items.AddRange(new object[6]
      {
        (object) "G1",
        (object) "G2",
        (object) "G3",
        (object) "G4",
        (object) "G5",
        (object) "G6"
      });
			cBoxLanGainSelect.Location = new Point(99, 167);
			cBoxLanGainSelect.Name = "cBoxLanGainSelect";
			cBoxLanGainSelect.Size = new Size(98, 20);
			cBoxLanGainSelect.TabIndex = 10;
			cBoxLanGainSelect.SelectedIndexChanged += new EventHandler(cBoxLanGainSelect_SelectedIndexChanged);
			nudFreqCenter.Anchor = AnchorStyles.None;
			int[] bits1 = new int[4];
			bits1[0] = 61;
			Decimal num1 = new Decimal(bits1);
			nudFreqCenter.Increment = num1;
			nudFreqCenter.Location = new Point(99, 95);
			int[] bits2 = new int[4];
			bits2[0] = 1020000000;
			Decimal num2 = new Decimal(bits2);
			nudFreqCenter.Maximum = num2;
			int[] bits3 = new int[4];
			bits3[0] = 290000000;
			Decimal num3 = new Decimal(bits3);
			nudFreqCenter.Minimum = num3;
			nudFreqCenter.Name = "nudFreqCenter";
			nudFreqCenter.Size = new Size(98, 21);
			nudFreqCenter.TabIndex = 1;
			nudFreqCenter.ThousandsSeparator = true;
			int[] bits4 = new int[4];
			bits4[0] = 915000000;
			Decimal num4 = new Decimal(bits4);
			nudFreqCenter.Value = num4;
			nudFreqCenter.ValueChanged += new EventHandler(nudFreqCenter_ValueChanged);
			nudFreqSpan.Anchor = AnchorStyles.None;
			int[] bits5 = new int[4];
			bits5[0] = 61;
			Decimal num5 = new Decimal(bits5);
			nudFreqSpan.Increment = num5;
			nudFreqSpan.Location = new Point(99, 119);
			int[] bits6 = new int[4];
			bits6[0] = 100000000;
			Decimal num6 = new Decimal(bits6);
			nudFreqSpan.Maximum = num6;
			nudFreqSpan.Name = "nudFreqSpan";
			nudFreqSpan.Size = new Size(98, 21);
			nudFreqSpan.TabIndex = 4;
			nudFreqSpan.ThousandsSeparator = true;
			int[] bits7 = new int[4];
			bits7[0] = 1000000;
			Decimal num7 = new Decimal(bits7);
			nudFreqSpan.Value = num7;
			nudFreqSpan.ValueChanged += new EventHandler(nudFreqSpan_ValueChanged);
			nudChanBw.Anchor = AnchorStyles.None;
			nudChanBw.Location = new Point(99, 143);
			int[] bits8 = new int[4];
			bits8[0] = 500000;
			Decimal num8 = new Decimal(bits8);
			nudChanBw.Maximum = num8;
			int[] bits9 = new int[4];
			bits9[0] = 3906;
			Decimal num9 = new Decimal(bits9);
			nudChanBw.Minimum = num9;
			nudChanBw.Name = "nudChanBw";
			nudChanBw.Size = new Size(98, 21);
			nudChanBw.TabIndex = 7;
			nudChanBw.ThousandsSeparator = true;
			int[] bits10 = new int[4];
			bits10[0] = 10417;
			Decimal num10 = new Decimal(bits10);
			nudChanBw.Value = num10;
			nudChanBw.ValueChanged += new EventHandler(nudChanBw_ValueChanged);
			label2.Anchor = AnchorStyles.None;
			label2.AutoSize = true;
			label2.BackColor = Color.Transparent;
			label2.ForeColor = Color.Gray;
			label2.Location = new Point(-2, 123);
			label2.Name = "label2";
			label2.Size = new Size(35, 12);
			label2.TabIndex = 3;
			label2.Text = "Span:";
			label1.Anchor = AnchorStyles.None;
			label1.AutoSize = true;
			label1.BackColor = Color.Transparent;
			label1.ForeColor = Color.Gray;
			label1.Location = new Point(-2, 99);
			label1.Name = "label1";
			label1.Size = new Size(107, 12);
			label1.TabIndex = 0;
			label1.Text = "Center frequency:";
			label6.Anchor = AnchorStyles.None;
			label6.AutoSize = true;
			label6.ForeColor = Color.Gray;
			label6.Location = new Point(203, 147);
			label6.Name = "label6";
			label6.Size = new Size(17, 12);
			label6.TabIndex = 8;
			label6.Text = "Hz";
			label7.Anchor = AnchorStyles.None;
			label7.AutoSize = true;
			label7.BackColor = Color.Transparent;
			label7.ForeColor = Color.Gray;
			label7.Location = new Point(-2, 171);
			label7.Name = "label7";
			label7.Size = new Size(59, 12);
			label7.TabIndex = 9;
			label7.Text = "LNA gain:";
			label3.Anchor = AnchorStyles.None;
			label3.AutoSize = true;
			label3.BackColor = Color.Transparent;
			label3.ForeColor = Color.Gray;
			label3.Location = new Point(-2, 147);
			label3.Name = "label3";
			label3.Size = new Size(113, 12);
			label3.TabIndex = 6;
			label3.Text = "Channel bandwidth:";
			label4.Anchor = AnchorStyles.None;
			label4.AutoSize = true;
			label4.ForeColor = Color.Gray;
			label4.Location = new Point(203, 99);
			label4.Name = "label4";
			label4.Size = new Size(17, 12);
			label4.TabIndex = 2;
			label4.Text = "Hz";
			label5.Anchor = AnchorStyles.None;
			label5.AutoSize = true;
			label5.ForeColor = Color.Gray;
			label5.Location = new Point(203, 123);
			label5.Name = "label5";
			label5.Size = new Size(17, 12);
			label5.TabIndex = 5;
			label5.Text = "Hz";
			panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel2.BorderStyle = BorderStyle.FixedSingle;
			panel2.Controls.Add((Control)graph);
			panel2.Location = new Point(0, 0);
			panel2.Margin = new Padding(0);
			panel2.Name = "panel2";
			panel2.Size = new Size(557, 342);
			panel2.TabIndex = 2;
			graph.Dock = DockStyle.Fill;
			graph.Location = new Point(0, 0);
			graph.Name = "graph";
			graph.Size = new Size(555, 340);
			graph.TabIndex = 0;
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new Size(780, 342);
			Controls.Add((Control)panel2);
			Controls.Add((Control)panel1);
			Icon = (Icon)resources.GetObject("$Icon");
			Name = "SpectrumAnalyserForm";
			Text = "Spectrum analyser";
			FormClosed += new FormClosedEventHandler(SpectrumAnalyserForm_FormClosed);
			Load += new EventHandler(SpectrumAnalyserForm_Load);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			nudFreqCenter.EndInit();
			nudFreqSpan.EndInit();
			nudChanBw.EndInit();
			panel2.ResumeLayout(false);
			ResumeLayout(false);
		}

		private delegate void SX1231DataChangedDelegate(object sender, PropertyChangedEventArgs e);
	}
}