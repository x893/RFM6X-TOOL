using SemtechLib.Devices.SX1231;
using SemtechLib.Devices.SX1231.General;
using SemtechLib.General;
using SemtechLib.General.Events;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Forms
{
	public class PacketLogForm : Form
	{
		private int tickStart = Environment.TickCount;
		private PacketLog log = new PacketLog();
		private ApplicationSettings appSettings;
		private SX1231 sx1231;
		private string previousValue;
		private GroupBox groupBox5;
		private Button btnLogBrowseFile;
		private ProgressBar pBarLog;
		private TableLayoutPanel tableLayoutPanel3;
		private TextBox tBoxLogMaxSamples;
		private Label lblCommandsLogMaxSamples;
		private CheckBox cBtnLogOnOff;
		private Button btnClose;
		private SaveFileDialog sfLogSaveFileDlg;

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
				Log.SX1231 = value;
			}
		}

		public PacketLog Log
		{
			get
			{
				return log;
			}
		}

		public PacketLogForm()
		{
			InitializeComponent();
			log.PropertyChanged += new PropertyChangedEventHandler(log_PropertyChanged);
			log.Stoped += new EventHandler(log_Stoped);
			log.ProgressChanged += new ProgressEventHandler(log_ProgressChanged);
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

		private void UpdateProgressBarStyle()
		{
			if ((long)log.MaxSamples == 0L && cBtnLogOnOff.Checked)
				pBarLog.Style = ProgressBarStyle.Marquee;
			else
				pBarLog.Style = ProgressBarStyle.Continuous;
		}

		private void OnError(byte status, string message)
		{
			Refresh();
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


		private void PacketLogForm_Load(object sender, EventArgs e)
		{
			try
			{
				string s1 = appSettings.GetValue("PacketLogTop");
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
				string s2 = appSettings.GetValue("PacketLogLeft");
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
				string folderPath = appSettings.GetValue("PacketLogPath");
				if (folderPath == null)
				{
					folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					appSettings.SetValue("PacketLogPath", folderPath);
				}
				log.Path = folderPath;
				string str = appSettings.GetValue("PacketLogFileName");
				if (str == null)
				{
					str = "sx1231-pkt.log";
					appSettings.SetValue("PacketLogFileName", str);
				}
				log.FileName = str;
				string s3 = appSettings.GetValue("PacketLogMaxSamples");
				if (s3 == null)
				{
					s3 = "1000";
					appSettings.SetValue("PacketLogMaxSamples", s3);
				}
				log.MaxSamples = ulong.Parse(s3);
			}
			catch (Exception ex)
			{
				OnError((byte)1, ex.Message);
			}
		}

		private void PacketLogForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				appSettings.SetValue("PacketLogTop", Top.ToString());
				appSettings.SetValue("PacketLogLeft", Left.ToString());
				appSettings.SetValue("PacketLogPath", log.Path);
				appSettings.SetValue("PacketLogFileName", log.FileName);
				appSettings.SetValue("PacketLogMaxSamples", log.MaxSamples.ToString());
			}
			catch (Exception)
			{
			}
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
					PacketLog packetLog = log;
					string str = packetLog.Path + strArray[index] + "\\";
					packetLog.Path = str;
				}
				PacketLog packetLog1 = log;
				string str1 = packetLog1.Path + strArray[index];
				packetLog1.Path = str1;
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

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(PacketLogForm));
			groupBox5 = new GroupBox();
			btnLogBrowseFile = new Button();
			pBarLog = new ProgressBar();
			tableLayoutPanel3 = new TableLayoutPanel();
			tBoxLogMaxSamples = new TextBox();
			lblCommandsLogMaxSamples = new Label();
			cBtnLogOnOff = new CheckBox();
			btnClose = new Button();
			sfLogSaveFileDlg = new SaveFileDialog();
			groupBox5.SuspendLayout();
			tableLayoutPanel3.SuspendLayout();
			SuspendLayout();
			groupBox5.Controls.Add((Control)btnLogBrowseFile);
			groupBox5.Controls.Add((Control)pBarLog);
			groupBox5.Controls.Add((Control)tableLayoutPanel3);
			groupBox5.Controls.Add((Control)cBtnLogOnOff);
			groupBox5.Location = new Point(12, 11);
			groupBox5.Name = "groupBox5";
			groupBox5.Size = new Size(209, 95);
			groupBox5.TabIndex = 4;
			groupBox5.TabStop = false;
			groupBox5.Text = "Log control";
			btnLogBrowseFile.Location = new Point(15, 65);
			btnLogBrowseFile.Name = "btnLogBrowseFile";
			btnLogBrowseFile.Size = new Size(75, 21);
			btnLogBrowseFile.TabIndex = 2;
			btnLogBrowseFile.Text = "Browse...";
			btnLogBrowseFile.UseVisualStyleBackColor = true;
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
			lblCommandsLogMaxSamples.Location = new Point(3, 0);
			lblCommandsLogMaxSamples.Name = "lblCommandsLogMaxSamples";
			lblCommandsLogMaxSamples.Size = new Size(85, 21);
			lblCommandsLogMaxSamples.TabIndex = 0;
			lblCommandsLogMaxSamples.Text = "Max samples:";
			lblCommandsLogMaxSamples.TextAlign = ContentAlignment.MiddleLeft;
			cBtnLogOnOff.Appearance = Appearance.Button;
			cBtnLogOnOff.CheckAlign = ContentAlignment.MiddleCenter;
			cBtnLogOnOff.Location = new Point(119, 65);
			cBtnLogOnOff.Name = "cBtnLogOnOff";
			cBtnLogOnOff.Size = new Size(75, 21);
			cBtnLogOnOff.TabIndex = 3;
			cBtnLogOnOff.Text = "Start";
			cBtnLogOnOff.TextAlign = ContentAlignment.MiddleCenter;
			cBtnLogOnOff.UseVisualStyleBackColor = true;
			cBtnLogOnOff.CheckedChanged += new EventHandler(cBtnLogOnOff_CheckedChanged);
			btnClose.Location = new Point(79, 112);
			btnClose.Name = "btnClose";
			btnClose.Size = new Size(75, 21);
			btnClose.TabIndex = 2;
			btnClose.Text = "Close";
			btnClose.UseVisualStyleBackColor = true;
			btnClose.Click += new EventHandler(btnClose_Click);
			sfLogSaveFileDlg.DefaultExt = "*.log";
			sfLogSaveFileDlg.Filter = "Log Files(*.log)|*.log|AllFiles(*.*)|*.*";
			AcceptButton = (IButtonControl)btnClose;
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new Size(233, 143);
			Controls.Add((Control)btnClose);
			Controls.Add((Control)groupBox5);
			DoubleBuffered = true;
			Icon = (Icon)resources.GetObject("$Icon");
			KeyPreview = true;
			MaximizeBox = false;
			Name = "PacketLogForm";
			Opacity = 0.9;
			Text = "Packet Log";
			FormClosed += new FormClosedEventHandler(PacketLogForm_FormClosed);
			Load += new EventHandler(PacketLogForm_Load);
			groupBox5.ResumeLayout(false);
			groupBox5.PerformLayout();
			tableLayoutPanel3.ResumeLayout(false);
			tableLayoutPanel3.PerformLayout();
			ResumeLayout(false);
		}

		private delegate void SX1231DataChangedDelegate(object sender, PropertyChangedEventArgs e);
	}
}