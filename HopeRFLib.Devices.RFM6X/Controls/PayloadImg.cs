using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SemtechLib.Devices.SX1231.Controls
{
	public class PayloadImg : Control
	{
		public new event PaintEventHandler Paint;

		public PayloadImg()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.BackColor = Color.Transparent;
			this.Size = new Size(526, 20);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.Paint != null)
			{
				this.Paint((object)this, e);
			}
			else
			{
				base.OnPaint(e);
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Image image = (Image)new Bitmap(this.Width, this.Height);
				Graphics graphics = Graphics.FromImage(image);
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				RectangleF rect = new RectangleF(0.0f, 0.0f, (float)this.Width, (float)this.Height);
				Brush brush = (Brush)new SolidBrush(SystemColors.ActiveBorder);
				graphics.DrawLine(new Pen(brush, 2f), rect.Left, rect.Bottom, rect.Right - 138f, rect.Top);
				graphics.DrawLine(new Pen(brush, 2f), rect.Right - 52f, rect.Top, rect.Right, rect.Bottom);
				e.Graphics.DrawImage(image, rect);
			}
		}
	}
}