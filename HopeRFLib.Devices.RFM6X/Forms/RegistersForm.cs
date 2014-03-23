using SemtechLib.Devices.SX1231;
using SemtechLib.Devices.SX1231.Controls;
using SemtechLib.General;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Forms
{
	public class RegistersForm : Form, INotifyPropertyChanged
	{
		private RegisterTableControl registerTableControl1;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel ssLblStatus;
		private Panel panel1;
		private ApplicationSettings appSettings;
		private SX1231 sx1231;
		private bool registersFormEnabled;

		public ApplicationSettings AppSettings
		{
			get { return appSettings; }
			set { appSettings = value; }
		}

		public SX1231 SX1231
		{
			set
			{
				try
				{
					sx1231 = value;
					sx1231.PropertyChanged += new PropertyChangedEventHandler(SX1231_PropertyChanged);
					registerTableControl1.Registers = sx1231.Registers;
					sx1231.ReadRegisters();
				}
				catch (Exception ex)
				{
					OnError((byte)1, ex.Message);
				}
			}
		}

		public bool RegistersFormEnabled
		{
			get
			{
				return registersFormEnabled;
			}
			set
			{
				registersFormEnabled = value;
				panel1.Enabled = value;
				OnPropertyChanged("RegistersFormEnabled");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public RegistersForm()
		{
			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(RegistersForm));
			statusStrip1 = new StatusStrip();
			ssLblStatus = new ToolStripStatusLabel();
			panel1 = new Panel();
			registerTableControl1 = new RegisterTableControl();
			statusStrip1.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			statusStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) ssLblStatus
      });
			statusStrip1.Location = new Point(0, 224);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size(292, 22);
			statusStrip1.TabIndex = 1;
			statusStrip1.Text = "statusStrip1";
			ssLblStatus.Name = "ssLblStatus";
			ssLblStatus.Size = new Size(11, 17);
			ssLblStatus.Text = "-";
			panel1.AutoSize = true;
			panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel1.Controls.Add(registerTableControl1);
			panel1.Dock = DockStyle.Fill;
			panel1.Location = new Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new Size(292, 224);
			panel1.TabIndex = 0;
			registerTableControl1.AutoSize = true;
			registerTableControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			registerTableControl1.Location = new Point(3, 3);
			registerTableControl1.Name = "registerTableControl1";
			registerTableControl1.Size = new Size(208, 25);
			registerTableControl1.Split = 4U;
			registerTableControl1.TabIndex = 0;
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			ClientSize = new Size(292, 246);
			Controls.Add((Control)panel1);
			Controls.Add((Control)statusStrip1);
			DoubleBuffered = true;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Icon = (Icon)resources.GetObject("$Icon");
			KeyPreview = true;
			MaximizeBox = false;
			Name = "RegistersForm";
			Text = "RFM6X Registers display";
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
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

		private void OnError(byte status, string message)
		{
			if ((int)status != 0)
				ssLblStatus.Text = "ERROR: " + message;
			else
				ssLblStatus.Text = message;
			Refresh();
		}

		private void RegistersForm_Load(object sender, EventArgs e)
		{
			string s1 = appSettings.GetValue("RegistersTop");
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
			string s2 = appSettings.GetValue("RegistersLeft");
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

		private void RegistersForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				appSettings.SetValue("RegistersTop", Top.ToString());
				appSettings.SetValue("RegistersLeft", Left.ToString());
			}
			catch (Exception)
			{
			}
		}

		private void SX1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "Version":
					registerTableControl1.Registers = sx1231.Registers;
					break;
			}
		}

		private void OnPropertyChanged(string propName)
		{
			if (PropertyChanged == null)
				return;
			PropertyChanged((object)this, new PropertyChangedEventArgs(propName));
		}
	}
}