using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Forms
{
	public class TemperatureCalibrationForm : Form
	{
		// private IContainer components;
		private Label label1;
		private NumericUpDown nudTempRoom;
		private Label label2;
		private Label label3;
		private Button btnOk;

		public Decimal TempValueRoom
		{
			get
			{
				return nudTempRoom.Value;
			}
			set
			{
				nudTempRoom.Value = value;
			}
		}

		public TemperatureCalibrationForm()
		{
			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
//			if (disposing && components != null)
//				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(TemperatureCalibrationForm));
			label1 = new Label();
			nudTempRoom = new NumericUpDown();
			label2 = new Label();
			label3 = new Label();
			btnOk = new Button();
			nudTempRoom.BeginInit();
			SuspendLayout();
			label1.AutoSize = true;
			label1.Location = new Point(12, 71);
			label1.Name = "label1";
			label1.Size = new Size(149, 12);
			label1.TabIndex = 1;
			label1.Text = "Actual room temperature:";
			nudTempRoom.Location = new Point(143, 67);
			int[] bits1 = new int[4];
			bits1[0] = 85;
			Decimal num1 = new Decimal(bits1);
			nudTempRoom.Maximum = num1;
			nudTempRoom.Minimum = new Decimal(new int[4]
      {
        40,
        0,
        0,
        int.MinValue
      });
			nudTempRoom.Name = "nudTempRoom";
			nudTempRoom.Size = new Size(39, 21);
			nudTempRoom.TabIndex = 2;
			int[] bits2 = new int[4];
			bits2[0] = 25;
			Decimal num2 = new Decimal(bits2);
			nudTempRoom.Value = num2;
			label2.AutoSize = true;
			label2.Location = new Point(188, 71);
			label2.Name = "label2";
			label2.Size = new Size(23, 12);
			label2.TabIndex = 3;
			label2.Text = "°C";
			label3.AutoSize = true;
			label3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			label3.Location = new Point(13, 9);
			label3.MaximumSize = new Size(238, 0);
			label3.Name = "label3";
			label3.Size = new Size(218, 51);
			label3.TabIndex = 0;
			label3.Text = "Please enter the actual room temperature measured on an auxiliary thermometer!";
			label3.TextAlign = ContentAlignment.MiddleCenter;
			btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			btnOk.Location = new Point(85, 91);
			btnOk.Name = "btnOk";
			btnOk.Size = new Size(75, 21);
			btnOk.TabIndex = 4;
			btnOk.Text = "OK";
			btnOk.UseVisualStyleBackColor = true;
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new Size(244, 123);
			Controls.Add((Control)btnOk);
			Controls.Add((Control)nudTempRoom);
			Controls.Add((Control)label2);
			Controls.Add((Control)label1);
			Controls.Add((Control)label3);
			DoubleBuffered = true;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Icon = (Icon)resources.GetObject("$Icon");
			KeyPreview = true;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "TemperatureCalibrationForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Temperature Calibration";
			nudTempRoom.EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}