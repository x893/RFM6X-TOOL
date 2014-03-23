using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Controls
{
	public class Jumper : Control
	{
		private ContentAlignment jumperAlign = ContentAlignment.MiddleCenter;
		private Size itemSize = new Size();
		private bool _checked;

		[DefaultValue(false)]
		[Description("Indicates whether the component is in the checked state")]
		[Category("Appearance")]
		public bool Checked
		{
			get
			{
				return this._checked;
			}
			set
			{
				this._checked = value;
				this.Invalidate();
			}
		}

		[Description("Indicates how the Jumper should be aligned")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Category("Appearance")]
		public ContentAlignment JumperAlign
		{
			get
			{
				return this.jumperAlign;
			}
			set
			{
				this.jumperAlign = value;
				this.Invalidate();
			}
		}

		private Point PosFromAlignment
		{
			get
			{
				Point point = new Point();
				switch (this.jumperAlign)
				{
					case ContentAlignment.BottomCenter:
						point.X = (int)((double)this.Width / 2.0 - (double)this.itemSize.Width / 2.0);
						point.Y = this.Height - this.itemSize.Height;
						return point;
					case ContentAlignment.BottomRight:
						point.X = this.Width - this.itemSize.Width;
						point.Y = this.Height - this.itemSize.Height;
						return point;
					case ContentAlignment.MiddleRight:
						point.X = this.Width - this.itemSize.Width;
						point.Y = (int)((double)this.Height / 2.0 - (double)this.itemSize.Height / 2.0);
						return point;
					case ContentAlignment.BottomLeft:
						point.X = 0;
						point.Y = this.Height - this.itemSize.Height;
						return point;
					case ContentAlignment.TopLeft:
						point.X = 0;
						point.Y = 0;
						return point;
					case ContentAlignment.TopCenter:
						point.X = (int)((double)this.Width / 2.0 - (double)this.itemSize.Width / 2.0);
						point.Y = 0;
						return point;
					case ContentAlignment.TopRight:
						point.X = this.Width - this.itemSize.Width;
						point.Y = 0;
						return point;
					case ContentAlignment.MiddleLeft:
						point.X = 0;
						point.Y = (int)((double)this.Height / 2.0 - (double)this.itemSize.Height / 2.0);
						return point;
					case ContentAlignment.MiddleCenter:
						point.X = (int)((double)this.Width / 2.0 - (double)this.itemSize.Width / 2.0);
						point.Y = (int)((double)this.Height / 2.0 - (double)this.itemSize.Height / 2.0);
						return point;
					default:
						point.X = 0;
						point.Y = 0;
						return point;
				}
			}
		}

		public new event PaintEventHandler Paint;

		public Jumper()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.BackColor = Color.Transparent;
			this.Size = new Size(19, 35);
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
				this.itemSize.Width = this.Size.Width * 66 / 100;
				this.itemSize.Height = this.Size.Height * 92 / 100;
				if (this.Enabled)
				{
					Size size = new Size(this.itemSize.Width * 40 / 100, this.itemSize.Width * 40 / 100);
					e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(150, 150, 150)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
					e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(0, 0, 0)), this.PosFromAlignment.X + this.itemSize.Width / 2 - size.Width / 2, this.PosFromAlignment.Y + this.itemSize.Height / 4 - size.Height / 2, size.Width, size.Height);
					e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(0, 0, 0)), this.PosFromAlignment.X + this.itemSize.Width / 2 - size.Width / 2, this.PosFromAlignment.Y + this.itemSize.Height / 2 - size.Height / 2, size.Width, size.Height);
					e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(0, 0, 0)), this.PosFromAlignment.X + this.itemSize.Width / 2 - size.Width / 2, this.PosFromAlignment.Y + 3 * (this.itemSize.Height / 4) - size.Height / 2, size.Width, size.Height);
					if (this.Checked)
						e.Graphics.FillRectangle((Brush)new SolidBrush(this.ForeColor), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, 3 * (this.itemSize.Height / 5));
					else
						e.Graphics.FillRectangle((Brush)new SolidBrush(this.ForeColor), this.PosFromAlignment.X, this.PosFromAlignment.Y + 2 * (this.itemSize.Height / 5), this.itemSize.Width, 3 * (this.itemSize.Height / 5));
				}
				else
				{
					Size size = new Size(this.itemSize.Width * 40 / 100, this.itemSize.Width * 40 / 100);
					e.Graphics.FillRectangle((Brush)new SolidBrush(SystemColors.InactiveCaption), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
					e.Graphics.FillRectangle((Brush)new SolidBrush(SystemColors.InactiveBorder), this.PosFromAlignment.X + this.itemSize.Width / 2 - size.Width / 2, this.PosFromAlignment.Y + this.itemSize.Height / 4 - size.Height / 2, size.Width, size.Height);
					e.Graphics.FillRectangle((Brush)new SolidBrush(SystemColors.InactiveBorder), this.PosFromAlignment.X + this.itemSize.Width / 2 - size.Width / 2, this.PosFromAlignment.Y + this.itemSize.Height / 2 - size.Height / 2, size.Width, size.Height);
					e.Graphics.FillRectangle((Brush)new SolidBrush(SystemColors.InactiveBorder), this.PosFromAlignment.X + this.itemSize.Width / 2 - size.Width / 2, this.PosFromAlignment.Y + 3 * (this.itemSize.Height / 4) - size.Height / 2, size.Width, size.Height);
				}
			}
		}
	}
}