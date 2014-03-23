using SemtechLib.Controls;
using SemtechLib.Devices.SX1231;
using SemtechLib.Devices.SX1231.Controls;
using SemtechLib.Devices.SX1231.General;
using SemtechLib.General;
using SemtechLib.General.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace SemtechLib.Devices.SX1231.Forms
{
	public class RssiAnalyserForm : Form
	{
		private int tickStart = Environment.TickCount;
		private DataLog log = new DataLog();
		private Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private Panel panel2;
		private RssiGraphControl graph;
		private NumericUpDownEx nudRssiThresh;
		private System.Windows.Forms.Label label55;
		private Panel panel3;
		private RadioButton rBtnRssiAutoThreshOff;
		private RadioButton rBtnRssiAutoThreshOn;
		private System.Windows.Forms.Label label6;
		private GroupBoxEx groupBox5;
		private Button btnLogBrowseFile;
		private ProgressBar pBarLog;
		private TableLayoutPanel tableLayoutPanel3;
		private TextBox tBoxLogMaxSamples;
		private System.Windows.Forms.Label lblCommandsLogMaxSamples;
		private CheckBox cBtnLogOnOff;
		private SaveFileDialog sfLogSaveFileDlg;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private ApplicationSettings appSettings;
		private SX1231 sx1231;
		private double time;
		private string previousValue;

		public ApplicationSettings AppSettings
		{
			get { return appSettings; }
			set { appSettings = value; }
		}

		public SX1231 SX1231
		{
			set
			{
				if (sx1231 == value)
					return;
				sx1231 = value;
				Log.SX1231 = value;
				sx1231.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(sx1231_PropertyChanged);
				CreateThreshold();
				if (!sx1231.RssiAutoThresh)
					nudRssiThresh.Value = sx1231.RssiThresh;
				else if (sx1231.AgcReference < -127)
					nudRssiThresh.Value = new Decimal(1275, 0, 0, true, (byte)1);
				else
					nudRssiThresh.Value = (Decimal)(sx1231.AgcReference - (int)sx1231.AgcSnrMargin);
				UpdateThreshold((double)nudRssiThresh.Value);
			}
		}

		public DataLog Log
		{
			get
			{
				return log;
			}
		}

		public RssiAnalyserForm()
		{
			InitializeComponent();
			graph.MouseWheel += new MouseEventHandler(graph_MouseWheel);
			log.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(log_PropertyChanged);
			log.Stoped += new EventHandler(log_Stoped);
			log.ProgressChanged += new ProgressEventHandler(log_ProgressChanged);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(RssiAnalyserForm));
			panel1 = new Panel();
			groupBox5 = new GroupBoxEx();
			btnLogBrowseFile = new Button();
			pBarLog = new ProgressBar();
			tableLayoutPanel3 = new TableLayoutPanel();
			tBoxLogMaxSamples = new TextBox();
			lblCommandsLogMaxSamples = new System.Windows.Forms.Label();
			cBtnLogOnOff = new CheckBox();
			panel3 = new Panel();
			rBtnRssiAutoThreshOff = new RadioButton();
			rBtnRssiAutoThreshOn = new RadioButton();
			label6 = new System.Windows.Forms.Label();
			nudRssiThresh = new NumericUpDownEx();
			label55 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			panel2 = new Panel();
			graph = new RssiGraphControl();
			sfLogSaveFileDlg = new SaveFileDialog();
			panel1.SuspendLayout();
			groupBox5.SuspendLayout();
			tableLayoutPanel3.SuspendLayout();
			panel3.SuspendLayout();
			nudRssiThresh.BeginInit();
			panel2.SuspendLayout();
			SuspendLayout();

			panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			panel1.BackColor = Color.Black;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.Controls.Add((Control)groupBox5);
			panel1.Controls.Add((Control)panel3);
			panel1.Controls.Add((Control)label6);
			panel1.Controls.Add((Control)nudRssiThresh);
			panel1.Controls.Add((Control)label55);
			panel1.Controls.Add((Control)label3);
			panel1.Controls.Add((Control)label1);
			panel1.Controls.Add((Control)label9);
			panel1.Controls.Add((Control)label7);
			panel1.Controls.Add((Control)label5);
			panel1.Controls.Add((Control)label4);
			panel1.Controls.Add((Control)label2);
			panel1.Controls.Add((Control)label8);
			panel1.Location = new Point(553, 0);
			panel1.Margin = new Padding(0);
			panel1.Name = "panel1";
			panel1.Size = new Size(223, 338);
			panel1.TabIndex = 1;
			groupBox5.BackColor = Color.Transparent;
			groupBox5.Controls.Add((Control)btnLogBrowseFile);
			groupBox5.Controls.Add((Control)pBarLog);
			groupBox5.Controls.Add((Control)tableLayoutPanel3);
			groupBox5.Controls.Add((Control)cBtnLogOnOff);
			groupBox5.ForeColor = Color.Gray;
			groupBox5.Location = new Point(9, 181);
			groupBox5.Name = "groupBox5";
			groupBox5.Size = new Size(209, 95);
			groupBox5.TabIndex = 8;
			groupBox5.TabStop = false;
			groupBox5.Text = "Log control";
			btnLogBrowseFile.BackColor = SystemColors.Control;
			btnLogBrowseFile.ForeColor = SystemColors.ControlText;
			btnLogBrowseFile.Location = new Point(15, 65);
			btnLogBrowseFile.Name = "btnLogBrowseFile";
			btnLogBrowseFile.Size = new Size(75, 21);
			btnLogBrowseFile.TabIndex = 2;
			btnLogBrowseFile.Text = "Browse...";
			btnLogBrowseFile.UseVisualStyleBackColor = false;
			btnLogBrowseFile.Click += new EventHandler(btnLogBrowseFile_Click);
			pBarLog.Location = new Point(15, 47);
			pBarLog.Name = "pBarLog";
			pBarLog.Size = new Size(179, 12);
			pBarLog.Step = 1;
			pBarLog.Style = ProgressBarStyle.Continuous;
			pBarLog.TabIndex = 1;
			tableLayoutPanel3.AutoSize = true;
			tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tableLayoutPanel3.ColumnCount = 2;
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel3.Controls.Add((Control)tBoxLogMaxSamples, 1, 0);
			tableLayoutPanel3.Controls.Add((Control)lblCommandsLogMaxSamples, 0, 0);
			tableLayoutPanel3.Location = new Point(15, 18);
			tableLayoutPanel3.Name = "tableLayoutPanel3";
			tableLayoutPanel3.RowCount = 1;
			tableLayoutPanel3.RowStyles.Add(new RowStyle());
			tableLayoutPanel3.Size = new Size(179, 27);
			tableLayoutPanel3.TabIndex = 0;
			tBoxLogMaxSamples.Location = new Point(94, 3);
			tBoxLogMaxSamples.Name = "tBoxLogMaxSamples";
			tBoxLogMaxSamples.Size = new Size(82, 21);
			tBoxLogMaxSamples.TabIndex = 1;
			tBoxLogMaxSamples.Text = "1000";
			tBoxLogMaxSamples.TextAlign = HorizontalAlignment.Center;
			tBoxLogMaxSamples.Enter += new EventHandler(tBoxLogMaxSamples_Enter);
			tBoxLogMaxSamples.Validating += new CancelEventHandler(tBoxLogMaxSamples_Validating);
			tBoxLogMaxSamples.Validated += new EventHandler(tBoxLogMaxSamples_Validated);
			lblCommandsLogMaxSamples.ForeColor = Color.Gray;
			lblCommandsLogMaxSamples.Location = new Point(3, 0);
			lblCommandsLogMaxSamples.Name = "lblCommandsLogMaxSamples";
			lblCommandsLogMaxSamples.Size = new Size(85, 21);
			lblCommandsLogMaxSamples.TabIndex = 0;
			lblCommandsLogMaxSamples.Text = "Max samples:";
			lblCommandsLogMaxSamples.TextAlign = ContentAlignment.MiddleLeft;
			cBtnLogOnOff.Appearance = Appearance.Button;
			cBtnLogOnOff.BackColor = SystemColors.Control;
			cBtnLogOnOff.CheckAlign = ContentAlignment.MiddleCenter;
			cBtnLogOnOff.ForeColor = SystemColors.ControlText;
			cBtnLogOnOff.Location = new Point(119, 65);
			cBtnLogOnOff.Name = "cBtnLogOnOff";
			cBtnLogOnOff.Size = new Size(75, 21);
			cBtnLogOnOff.TabIndex = 3;
			cBtnLogOnOff.Text = "Start";
			cBtnLogOnOff.TextAlign = ContentAlignment.MiddleCenter;
			cBtnLogOnOff.UseVisualStyleBackColor = false;
			cBtnLogOnOff.CheckedChanged += new EventHandler(cBtnLogOnOff_CheckedChanged);
			panel3.Anchor = AnchorStyles.None;
			panel3.AutoSize = true;
			panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel3.Controls.Add((Control)rBtnRssiAutoThreshOff);
			panel3.Controls.Add((Control)rBtnRssiAutoThreshOn);
			panel3.ForeColor = Color.Gray;
			panel3.Location = new Point(117, 134);
			panel3.Name = "panel3";
			panel3.Size = new Size(94, 22);
			panel3.TabIndex = 5;
			rBtnRssiAutoThreshOff.AutoSize = true;
			rBtnRssiAutoThreshOff.Location = new Point(50, 3);
			rBtnRssiAutoThreshOff.Name = "rBtnRssiAutoThreshOff";
			rBtnRssiAutoThreshOff.Size = new Size(41, 16);
			rBtnRssiAutoThreshOff.TabIndex = 1;
			rBtnRssiAutoThreshOff.Text = "OFF";
			rBtnRssiAutoThreshOff.UseVisualStyleBackColor = true;
			rBtnRssiAutoThreshOff.CheckedChanged += new EventHandler(rBtnRssiAutoThreshOff_CheckedChanged);
			rBtnRssiAutoThreshOn.AutoSize = true;
			rBtnRssiAutoThreshOn.Checked = true;
			rBtnRssiAutoThreshOn.Location = new Point(3, 3);
			rBtnRssiAutoThreshOn.Name = "rBtnRssiAutoThreshOn";
			rBtnRssiAutoThreshOn.Size = new Size(35, 16);
			rBtnRssiAutoThreshOn.TabIndex = 0;
			rBtnRssiAutoThreshOn.TabStop = true;
			rBtnRssiAutoThreshOn.Text = "ON";
			rBtnRssiAutoThreshOn.UseVisualStyleBackColor = true;
			rBtnRssiAutoThreshOn.CheckedChanged += new EventHandler(rBtnRssiAutoThreshOn_CheckedChanged);
			label6.Anchor = AnchorStyles.None;
			label6.AutoSize = true;
			label6.ForeColor = Color.Gray;
			label6.Location = new Point(6, 138);
			label6.Name = "label6";
			label6.Size = new Size(125, 12);
			label6.TabIndex = 4;
			label6.Text = "RSSI auto threshold:";
			nudRssiThresh.Anchor = AnchorStyles.None;
			nudRssiThresh.DecimalPlaces = 1;
			nudRssiThresh.Enabled = false;
			nudRssiThresh.Increment = new Decimal(new int[4]
      {
        5,
        0,
        0,
        65536
      });
			nudRssiThresh.Location = new Point(117, 158);
			nudRssiThresh.Margin = new Padding(0);
			nudRssiThresh.Maximum = new Decimal(new int[4]);
			nudRssiThresh.Minimum = new Decimal(new int[4]
      {
        1275,
        0,
        0,
        -2147418112
      });
			nudRssiThresh.Name = "nudRssiThresh";
			nudRssiThresh.Size = new Size(60, 21);
			nudRssiThresh.TabIndex = 7;
			nudRssiThresh.ThousandsSeparator = true;
			nudRssiThresh.Value = new Decimal(new int[4]
      {
        114,
        0,
        0,
        int.MinValue
      });
			nudRssiThresh.ValueChanged += new EventHandler(nudRssiThresh_ValueChanged);
			label55.Anchor = AnchorStyles.None;
			label55.AutoSize = true;
			label55.BackColor = Color.Transparent;
			label55.ForeColor = Color.Gray;
			label55.Location = new Point(6, 158);
			label55.Margin = new Padding(0);
			label55.Name = "label55";
			label55.Size = new Size(59, 12);
			label55.TabIndex = 6;
			label55.Text = "Threshold";
			label55.TextAlign = ContentAlignment.MiddleCenter;
			label3.Anchor = AnchorStyles.None;
			label3.AutoSize = true;
			label3.ForeColor = Color.Green;
			label3.Location = new Point(6, 83);
			label3.Name = "label3";
			label3.Size = new Size(89, 12);
			label3.TabIndex = 2;
			label3.Text = "RSSI Threshold";
			label1.Anchor = AnchorStyles.None;
			label1.BackColor = Color.Green;
			label1.Location = new Point(117, 88);
			label1.Margin = new Padding(3, 3, 0, 3);
			label1.Name = "label1";
			label1.Size = new Size(25, 2);
			label1.TabIndex = 3;
			label1.Text = "label7";
			label9.Anchor = AnchorStyles.None;
			label9.AutoSize = true;
			label9.ForeColor = Color.Aqua;
			label9.Location = new Point(6, 27);
			label9.Margin = new Padding(3);
			label9.Name = "label9";
			label9.Size = new Size(65, 12);
			label9.TabIndex = 0;
			label9.Text = "RF_PA RSSI";
			label9.Visible = false;
			label7.Anchor = AnchorStyles.None;
			label7.BackColor = Color.Aqua;
			label7.Location = new Point(117, 31);
			label7.Margin = new Padding(3);
			label7.Name = "label7";
			label7.Size = new Size(25, 2);
			label7.TabIndex = 1;
			label7.Text = "label7";
			label7.Visible = false;
			label5.Anchor = AnchorStyles.None;
			label5.AutoSize = true;
			label5.ForeColor = Color.Yellow;
			label5.Location = new Point(6, 44);
			label5.Margin = new Padding(3);
			label5.Name = "label5";
			label5.Size = new Size(65, 12);
			label5.TabIndex = 0;
			label5.Text = "RF_IO RSSI";
			label5.Visible = false;
			label4.Anchor = AnchorStyles.None;
			label4.BackColor = Color.Yellow;
			label4.Location = new Point(117, 49);
			label4.Margin = new Padding(3);
			label4.Name = "label4";
			label4.Size = new Size(25, 2);
			label4.TabIndex = 1;
			label4.Text = "label7";
			label4.Visible = false;
			label2.Anchor = AnchorStyles.None;
			label2.AutoSize = true;
			label2.ForeColor = Color.Red;
			label2.Location = new Point(6, 62);
			label2.Margin = new Padding(3);
			label2.Name = "label2";
			label2.Size = new Size(29, 12);
			label2.TabIndex = 0;
			label2.Text = "RSSI";
			label8.Anchor = AnchorStyles.None;
			label8.BackColor = Color.Red;
			label8.Location = new Point(117, 66);
			label8.Margin = new Padding(3);
			label8.Name = "label8";
			label8.Size = new Size(25, 2);
			label8.TabIndex = 1;
			label8.Text = "label7";
			panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel2.BorderStyle = BorderStyle.FixedSingle;
			panel2.Location = new Point(0, 0);
			panel2.Margin = new Padding(0);
			panel2.Name = "panel2";
			panel2.Size = new Size(553, 338);
			panel2.TabIndex = 0;

			graph.Dock = DockStyle.Fill;
			graph.Location = new Point(0, 0);
			graph.Name = "graph";
			graph.Size = new Size(551, 336);
			graph.TabIndex = 0;

			panel2.Controls.Add(graph);

			sfLogSaveFileDlg.DefaultExt = "*.log";
			sfLogSaveFileDlg.Filter = "Log Files(*.log)|*.log|AllFiles(*.*)|*.*";
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new Size(776, 338);
			Controls.Add(panel2);
			Controls.Add(panel1);

			Icon = (Icon)resources.GetObject("$Icon");
			Name = "RssiAnalyserForm";
			Text = "Rssi analyser";
			FormClosed += new FormClosedEventHandler(RssiAnalyserForm_FormClosed);
			Load += new EventHandler(RssiAnalyserForm_Load);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			groupBox5.ResumeLayout(false);
			groupBox5.PerformLayout();
			tableLayoutPanel3.ResumeLayout(false);
			tableLayoutPanel3.PerformLayout();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			nudRssiThresh.EndInit();
			panel2.ResumeLayout(false);
			ResumeLayout(false);
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

		private void CreateThreshold()
		{
			GraphPane graphPane = ((List<GraphPane>)graph.PaneList)[0];
			double num = 0.0;
			LineObj lineObj = new LineObj(Color.Green, 0.0, num, 1.0, num);
			lineObj.Location.CoordinateFrame = CoordType.XChartFractionYScale;
			lineObj.IsVisible = true;
			graphPane.GraphObjList.Add((GraphObj)lineObj);
			graphPane.AxisChange();
			graph.Invalidate();
		}

		public void UpdateThreshold(double thres)
		{
			GraphPane graphPane = ((List<GraphPane>)graph.PaneList)[0];
			(((List<GraphObj>)graphPane.GraphObjList)[0] as LineObj).Location.Y = thres;
			(((List<GraphObj>)graphPane.GraphObjList)[0] as LineObj).Location.Y1 = thres;
			if (thres < graphPane.YAxis.Scale.Max && thres > graphPane.YAxis.Scale.Min)
				(((List<GraphObj>)graphPane.GraphObjList)[0] as LineObj).IsVisible = true;
			else
				(((List<GraphObj>)graphPane.GraphObjList)[0] as LineObj).IsVisible = false;
			graphPane.AxisChange();
			graph.Invalidate();
		}

		private void UpdateProgressBarStyle()
		{
			if ((long)log.MaxSamples == 0L && cBtnLogOnOff.Checked)
				pBarLog.Style = ProgressBarStyle.Marquee;
			else
				pBarLog.Style = ProgressBarStyle.Continuous;
		}

		private void OnSX1231PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "RssiAutoThresh":
					rBtnRssiAutoThreshOn.Checked = sx1231.RssiAutoThresh;
					rBtnRssiAutoThreshOff.Checked = !sx1231.RssiAutoThresh;
					nudRssiThresh.Enabled = !sx1231.RssiAutoThresh;
					break;
				case "RssiValue":
					if (sx1231.RfPaSwitchEnabled == 0)
					{
						time = (double)(Environment.TickCount - tickStart) / 1000.0;
						graph.UpdateLineGraph(time, (double)sx1231.RssiValue);
						break;
					}
					else
						break;
				case "RfPaSwitchEnabled":
					label9.Visible = sx1231.RfPaSwitchEnabled != 0;
					label7.Visible = sx1231.RfPaSwitchEnabled != 0;
					label5.Visible = sx1231.RfPaSwitchEnabled != 0;
					label4.Visible = sx1231.RfPaSwitchEnabled != 0;
					label2.Visible = sx1231.RfPaSwitchEnabled == 0;
					label8.Visible = sx1231.RfPaSwitchEnabled == 0;
					break;
				case "RfPaRssiValue":
					if (sx1231.RfPaSwitchEnabled == 1)
					{
						time = (double)(Environment.TickCount - tickStart) / 1000.0;
						graph.UpdateLineGraph(1, time, (double)sx1231.RfPaRssiValue);
						graph.UpdateLineGraph(2, time, (double)sx1231.RfIoRssiValue);
						break;
					}
					else
						break;
				case "RfIoRssiValue":
					if (sx1231.RfPaSwitchEnabled != 0)
					{
						time = (double)(Environment.TickCount - tickStart) / 1000.0;
						graph.UpdateLineGraph(1, time, (double)sx1231.RfPaRssiValue);
						graph.UpdateLineGraph(2, time, (double)sx1231.RfIoRssiValue);
						break;
					}
					else
						break;
				case "RssiThresh":
					if (!sx1231.RssiAutoThresh)
					{
						nudRssiThresh.Value = sx1231.RssiThresh;
						break;
					}
					else if (sx1231.AgcReference < -127)
					{
						nudRssiThresh.Value = new Decimal(1275, 0, 0, true, (byte)1);
						break;
					}
					else
					{
						nudRssiThresh.Value = (Decimal)(sx1231.AgcReference - (int)sx1231.AgcSnrMargin);
						break;
					}
			}
			UpdateThreshold((double)nudRssiThresh.Value);
		}

		private void OnError(byte status, string message)
		{
			Refresh();
		}

		private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (InvokeRequired)
				BeginInvoke((Delegate)new RssiAnalyserForm.SX1231DataChangedDelegate(OnSX1231PropertyChanged), sender, (object)e);
			else
				OnSX1231PropertyChanged(sender, e);
		}

		private void tBoxLogMaxSamples_Enter(object sender, EventArgs e)
		{
			previousValue = tBoxLogMaxSamples.Text;
		}

		private void tBoxLogMaxSamples_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				long num = (long)Convert.ToUInt64(tBoxLogMaxSamples.Text);
			}
			catch (Exception ex)
			{
				int num = (int)MessageBox.Show(ex.Message + (object)"\rInput Format: " + (string)(object)0 + " - " + ulong.MaxValue.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tBoxLogMaxSamples.Text = previousValue;
			}
		}

		private void tBoxLogMaxSamples_Validated(object sender, EventArgs e)
		{
			log.MaxSamples = ulong.Parse(tBoxLogMaxSamples.Text);
		}

		private void log_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "MaxSamples":
					tBoxLogMaxSamples.Text = log.MaxSamples.ToString();
					break;
			}
		}

		private void log_ProgressChanged(object sender, ProgressEventArg e)
		{
			MethodInvoker method = null;
			if (base.InvokeRequired)
			{
				if (method == null)
				{
					method = (MethodInvoker)(() => pBarLog.Value = (int)e.Progress);
				}
				base.BeginInvoke(method, null);
			}
			else
			{
				pBarLog.Value = (int)e.Progress;
			}
		}

		private void log_Stoped(object sender, EventArgs e)
		{
			MethodInvoker method = null;
			if (base.InvokeRequired)
			{
				if (method == null)
				{
					method = delegate
					{
						cBtnLogOnOff.Checked = false;
						cBtnLogOnOff.Text = "Start";
						tBoxLogMaxSamples.Enabled = true;
						btnLogBrowseFile.Enabled = true;
						log.Stop();
						UpdateProgressBarStyle();
					};
				}
				base.BeginInvoke(method, null);
			}
			else
			{
				cBtnLogOnOff.Checked = false;
				cBtnLogOnOff.Text = "Start";
				tBoxLogMaxSamples.Enabled = true;
				btnLogBrowseFile.Enabled = true;
				log.Stop();
				UpdateProgressBarStyle();
			}
		}

		private void RssiAnalyserForm_Load(object sender, EventArgs e)
		{
			try
			{
				string s1 = appSettings.GetValue("RssiAnalyserTop");
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
				string s2 = appSettings.GetValue("RssiAnalyserLeft");
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
				string folderPath = appSettings.GetValue("LogPath");
				if (folderPath == null)
				{
					folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					appSettings.SetValue("LogPath", folderPath);
				}
				log.Path = folderPath;
				string str = appSettings.GetValue("LogFileName");
				if (str == null)
				{
					str = "sx1231-Rssi.log";
					appSettings.SetValue("LogFileName", str);
				}
				log.FileName = str;
				string s3 = appSettings.GetValue("LogMaxSamples");
				if (s3 == null)
				{
					s3 = "1000";
					appSettings.SetValue("LogMaxSamples", s3);
				}
				log.MaxSamples = ulong.Parse(s3);
			}
			catch (Exception ex)
			{
				OnError((byte)1, ex.Message);
			}
		}

		private void RssiAnalyserForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				appSettings.SetValue("RssiAnalyserTop", Top.ToString());
				appSettings.SetValue("RssiAnalyserLeft", Left.ToString());
				appSettings.SetValue("LogPath", log.Path);
				appSettings.SetValue("LogFileName", log.FileName);
				appSettings.SetValue("LogMaxSamples", log.MaxSamples.ToString());
			}
			catch (Exception)
			{
			}
		}

		private void nudRssiThresh_ValueChanged(object sender, EventArgs e)
		{
			sx1231.SetRssiThresh(nudRssiThresh.Value);
		}

		private void rBtnRssiAutoThreshOn_CheckedChanged(object sender, EventArgs e)
		{
			sx1231.RssiAutoThresh = rBtnRssiAutoThreshOn.Checked;
		}

		private void rBtnRssiAutoThreshOff_CheckedChanged(object sender, EventArgs e)
		{
			sx1231.RssiAutoThresh = rBtnRssiAutoThreshOn.Checked;
		}

		private void graph_MouseWheel(object sender, MouseEventArgs e)
		{
			UpdateThreshold((double)nudRssiThresh.Value);
		}

		private void btnLogBrowseFile_Click(object sender, EventArgs e)
		{
			OnError((byte)0, "-");
			try
			{
				sfLogSaveFileDlg.InitialDirectory = log.Path;
				sfLogSaveFileDlg.FileName = log.FileName;
				if (sfLogSaveFileDlg.ShowDialog() != DialogResult.OK)
					return;
				string[] strArray = sfLogSaveFileDlg.FileName.Split(new char[1]
        {
          '\\'
        });
				log.FileName = strArray[strArray.Length - 1];
				log.Path = "";
				int index;
				for (index = 0; index < strArray.Length - 2; ++index)
				{
					DataLog dataLog = log;
					string str = dataLog.Path + strArray[index] + "\\";
					dataLog.Path = str;
				}
				DataLog dataLog1 = log;
				string str1 = dataLog1.Path + strArray[index];
				dataLog1.Path = str1;
			}
			catch (Exception ex)
			{
				OnError((byte)1, ex.Message);
			}
		}

		private void cBtnLogOnOff_CheckedChanged(object sender, EventArgs e)
		{
			OnError((byte)0, "-");
			try
			{
				if (cBtnLogOnOff.Checked)
				{
					cBtnLogOnOff.Text = "Stop";
					tBoxLogMaxSamples.Enabled = false;
					btnLogBrowseFile.Enabled = false;
					log.Start();
				}
				else
				{
					cBtnLogOnOff.Text = "Start";
					tBoxLogMaxSamples.Enabled = true;
					btnLogBrowseFile.Enabled = true;
					log.Stop();
				}
			}
			catch (Exception ex)
			{
				cBtnLogOnOff.Checked = false;
				cBtnLogOnOff.Text = "Start";
				tBoxLogMaxSamples.Enabled = true;
				btnLogBrowseFile.Enabled = true;
				log.Stop();
				OnError((byte)1, ex.Message);
			}
			finally
			{
				UpdateProgressBarStyle();
			}
		}

		private delegate void SX1231DataChangedDelegate(object sender, PropertyChangedEventArgs e);
	}
}