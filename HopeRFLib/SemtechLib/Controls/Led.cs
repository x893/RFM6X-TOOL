using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SemtechLib.Controls
{
	public class Led : Control
	{
		private Color ledColor = Color.Green;
		private ContentAlignment ledAlign = ContentAlignment.MiddleCenter;
		private Size itemSize = new Size(11, 11);
		private bool _checked;

		[DefaultValue(false)]
		[Category("Appearance")]
		[Description("Indicates whether the component is in the checked state")]
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

		[Description("Indicates the color of the LED")]
		[Category("Appearance")]
		public Color LedColor
		{
			get
			{
				return this.ledColor;
			}
			set
			{
				this.ledColor = value;
				this.Invalidate();
			}
		}

		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Category("Appearance")]
		[Description("Indicates how the LED should be aligned")]
		public ContentAlignment LedAlign
		{
			get
			{
				return this.ledAlign;
			}
			set
			{
				this.ledAlign = value;
				this.Invalidate();
			}
		}

		[Description("Sets the size of the led")]
		[Category("Layout")]
		public Size LedSize
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

		private Point PosFromAlignment
		{
			get
			{
				Point point = new Point();
				switch (this.ledAlign)
				{
					case ContentAlignment.BottomCenter:
						point.X = (int)((double)this.Width / 2.0 - (double)this.itemSize.Width / 2.0);
						point.Y = this.Height - this.itemSize.Height - 1;
						return point;
					case ContentAlignment.BottomRight:
						point.X = this.Width - this.itemSize.Width - 1;
						point.Y = this.Height - this.itemSize.Height - 1;
						return point;
					case ContentAlignment.MiddleRight:
						point.X = this.Width - this.itemSize.Width - 1;
						point.Y = (int)((double)this.Height / 2.0 - (double)this.itemSize.Height / 2.0);
						return point;
					case ContentAlignment.BottomLeft:
						point.X = 0;
						point.Y = this.Height - this.itemSize.Height - 1;
						return point;
					case ContentAlignment.TopLeft:
						point.X = 1;
						point.Y = 1;
						return point;
					case ContentAlignment.TopCenter:
						point.X = (int)((double)this.Width / 2.0 - (double)this.itemSize.Width / 2.0);
						point.Y = 1;
						return point;
					case ContentAlignment.TopRight:
						point.X = this.Width - this.itemSize.Width - 1;
						point.Y = 1;
						return point;
					case ContentAlignment.MiddleLeft:
						point.X = 1;
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

		public Led()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.BackColor = Color.Transparent;
			this.Size = new Size(15, 15);
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
				float angle = (float)(50.0 - 15.0 * (1.0 - (double)this.Width / (double)this.Height));
				Rectangle rect1 = new Rectangle(this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
				LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush(rect1, ControlPaint.Dark(this.Parent.BackColor), ControlPaint.LightLight(this.Parent.BackColor), angle);
				linearGradientBrush1.Blend = new Blend()
				{
					Positions = new float[6] { 0.0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f },
					Factors = new float[6] { 0.2f, 0.2f, 0.4f, 0.4f, 1f, 1f }
				};
				Rectangle rect2 = rect1;
				rect2.Inflate(1, 1);
				e.Graphics.FillEllipse((Brush)linearGradientBrush1, rect2);
				if (this.Enabled)
				{
					if (this.Checked)
						e.Graphics.FillEllipse((Brush)new SolidBrush(ControlPaint.Light(this.ledColor)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
					else
						e.Graphics.FillEllipse((Brush)new SolidBrush(ControlPaint.Dark(this.ledColor)), this.PosFromAlignment.X, this.PosFromAlignment.Y, this.itemSize.Width, this.itemSize.Height);
				}
				LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rect1, Color.FromArgb(150, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue), Color.Transparent, angle);
				LinearGradientBrush linearGradientBrush3 = new LinearGradientBrush(rect1, Color.FromArgb(100, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue), Color.FromArgb(100, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue), angle);
				Blend blend = new Blend();
				blend.Positions = new float[6] { 0.0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
				blend.Factors = new float[6] { 0.2f, 0.2f, 0.4f, 0.4f, 1f, 1f };
				linearGradientBrush2.Blend = blend;
				linearGradientBrush3.Blend = blend;
				e.Graphics.FillEllipse((Brush)linearGradientBrush3, this.PosFromAlignment.X + this.itemSize.Width * 13 / 100, this.PosFromAlignment.Y + this.itemSize.Height * 13 / 100, this.itemSize.Width * 40 / 100, this.itemSize.Height * 40 / 100);
				e.Graphics.FillEllipse((Brush)linearGradientBrush2, new Rectangle(this.PosFromAlignment, this.itemSize));
			}
		}
	}
}