using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Controls
{
	public class SwitchBtn : Control
	{
		private ContentAlignment controlAlign = ContentAlignment.MiddleCenter;
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

		[Description("Indicates how the LED should be aligned")]
		[Category("Appearance")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		public ContentAlignment ControlAlign
		{
			get
			{
				return this.controlAlign;
			}
			set
			{
				this.controlAlign = value;
				this.Invalidate();
			}
		}

		private Point PosFromAlignment
		{
			get
			{
				Point point = new Point();
				switch (this.controlAlign)
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

		protected Size ItemSize
		{
			get
			{
				return this.itemSize;
			}
			set
			{
				this.itemSize = value;
				this.Invalidate();
			}
		}

		public new event PaintEventHandler Paint;

		public SwitchBtn()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.BackColor = Color.Transparent;
			this.Width = 15;
			this.Height = 25;
			this.itemSize.Width = 10;
			this.itemSize.Height = 23;
			this.MouseDown += new MouseEventHandler(this.mouseDown);
			this.MouseUp += new MouseEventHandler(this.mouseUp);
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
				if (this.Enabled)
				{
					e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb((int)byte.MaxValue, 0, 0)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
					e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(150, 150, 150)), this.PosFromAlignment.X + 2, this.PosFromAlignment.Y + 5, this.itemSize.Width - 4, this.itemSize.Height - 10);
					if (this.Checked)
						e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(0, 0, 0)), this.PosFromAlignment.X + 3, this.PosFromAlignment.Y + 6, this.itemSize.Width - 6, this.itemSize.Height - 16);
					else
						e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(0, 0, 0)), this.PosFromAlignment.X + 3, this.PosFromAlignment.Y + 10, this.itemSize.Width - 6, this.itemSize.Height - 16);
				}
				else
				{
					e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(200, 120, 120)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
					e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(150, 150, 150)), this.PosFromAlignment.X + 2, this.PosFromAlignment.Y + 5, this.itemSize.Width - 4, this.itemSize.Height - 10);
					if (this.Checked)
						e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(100, 100, 100)), this.PosFromAlignment.X + 3, this.PosFromAlignment.Y + 6, this.itemSize.Width - 6, this.itemSize.Height - 16);
					else
						e.Graphics.FillRectangle((Brush)new SolidBrush(Color.FromArgb(100, 100, 100)), this.PosFromAlignment.X + 3, this.PosFromAlignment.Y + 10, this.itemSize.Width - 6, this.itemSize.Height - 16);
				}
			}
		}

		protected void mouseDown(object sender, MouseEventArgs e)
		{
			this.buttonDown();
		}

		protected void mouseUp(object sender, MouseEventArgs e)
		{
			this.buttonUp();
		}

		protected void buttonDown()
		{
			this.Invalidate();
		}

		protected void buttonUp()
		{
			this.Checked = !this.Checked;
			this.Invalidate();
		}
	}
}