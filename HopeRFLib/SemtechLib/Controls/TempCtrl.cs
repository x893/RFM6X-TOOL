using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SemtechLib.Controls
{
	public class TempCtrl : Control
	{
		private TempCtrl.Ranges range = new TempCtrl.Ranges();
		private double value = 25.0;
		private bool drawTics = true;
		private int smallTicFreq = 5;
		private int largeTicFreq = 10;
		private Font fntText = new Font("Arial", 10f, FontStyle.Bold);
		private StringFormat strfmtText = new StringFormat();
		private bool enableTransparentBackground;
		private bool requiresRedraw;
		private Image backgroundImg;
		private RectangleF rectBackgroundImg;
		private Color colorFore;
		private Color colorBack;
		private Color colorScale;
		private Color colorScaleText;
		private Color colorOutline;
		private Color colorBackground;
		private Pen forePen;
		private Pen scalePen;
		private Pen outlinePen;
		private SolidBrush blackBrush;
		private SolidBrush fillBrush;
		private LinearGradientBrush bulbBrush;
		private RectangleF rectCylinder;
		private RectangleF rectBulb;
		private PointF pointCenter;
		private float fTmpWidth;
		private float fRange;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TempCtrl.Ranges Range
		{
			get
			{
				return this.range;
			}
			set
			{
				this.range = value;
				this.requiresRedraw = true;
				this.Invalidate();
			}
		}

		public double Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				if (value > this.range.Max)
					this.value = this.range.Max;
				if (value < this.range.Min)
					this.value = this.range.Min;
				this.Invalidate();
			}
		}

		public bool DrawTics
		{
			get
			{
				return this.drawTics;
			}
			set
			{
				this.drawTics = value;
				this.requiresRedraw = true;
				this.Invalidate();
			}
		}

		public int SmallTicFreq
		{
			get
			{
				return this.smallTicFreq;
			}
			set
			{
				this.smallTicFreq = value;
				this.requiresRedraw = true;
				this.Invalidate();
			}
		}

		public int LargeTicFreq
		{
			get
			{
				return this.largeTicFreq;
			}
			set
			{
				this.largeTicFreq = value;
				this.requiresRedraw = true;
				this.Invalidate();
			}
		}

		[Description("Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker.")]
		[DefaultValue(false)]
		public bool EnableTransparentBackground
		{
			get
			{
				return this.enableTransparentBackground;
			}
			set
			{
				this.enableTransparentBackground = value;
				this.SetStyle(ControlStyles.OptimizedDoubleBuffer, !this.enableTransparentBackground);
				this.requiresRedraw = true;
				this.Refresh();
			}
		}

		public new event PaintEventHandler Paint;

		public TempCtrl()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.Size = new Size(75, 253);
			this.ForeColor = Color.Red;
			this.colorFore = this.ForeColor;
			this.colorBack = SystemColors.Control;
			this.colorScale = Color.FromArgb(0, 0, 0);
			this.colorScaleText = Color.FromArgb(0, 0, 0);
			this.colorOutline = Color.FromArgb(64, 0, 0);
			this.colorBackground = Color.FromKnownColor(KnownColor.Transparent);
			this.EnabledChanged += new EventHandler(this.TempCtrl_EnabledChanged);
			this.SizeChanged += new EventHandler(this.TempCtrl_SizeChanged);
		}

		private void TempCtrl_EnabledChanged(object sender, EventArgs e)
		{
			this.requiresRedraw = true;
			this.Refresh();
		}

		private void TempCtrl_SizeChanged(object sender, EventArgs e)
		{
			this.requiresRedraw = true;
			this.Refresh();
		}

		protected Color OffsetColor(Color color, short offset)
		{
			short val1 = offset;
			short val2_1 = offset;
			short val2_2 = offset;
			if ((int)offset < -255 || (int)offset > (int)byte.MaxValue)
				return color;
			byte r = color.R;
			byte g = color.G;
			byte b = color.B;
			if ((int)offset > 0)
			{
				if ((int)r + (int)offset > (int)byte.MaxValue)
					val1 = (short)((int)byte.MaxValue - (int)r);
				if ((int)g + (int)offset > (int)byte.MaxValue)
					val2_1 = (short)((int)byte.MaxValue - (int)g);
				if ((int)b + (int)offset > (int)byte.MaxValue)
					val2_2 = (short)((int)byte.MaxValue - (int)b);
				offset = Math.Min(Math.Min(val1, val2_1), val2_2);
			}
			else
			{
				if ((int)r + (int)offset < 0)
					val1 = (short)-r;
				if ((int)g + (int)offset < 0)
					val2_1 = (short)-g;
				if ((int)b + (int)offset < 0)
					val2_2 = (short)-b;
				offset = Math.Max(Math.Max(val1, val2_1), val2_2);
			}
			return Color.FromArgb((int)color.A, (int)r + (int)offset, (int)g + (int)offset, (int)b + (int)offset);
		}

		protected void FillCylinder(Graphics g, RectangleF ctrl, Brush fillBrush, Color outlineColor)
		{
			RectangleF rect1 = new RectangleF(ctrl.X, ctrl.Y - 5f, ctrl.Width, 5f);
			RectangleF rect2 = new RectangleF(ctrl.X, ctrl.Bottom - 5f, ctrl.Width, 5f);
			Pen pen = new Pen(outlineColor);
			GraphicsPath path = new GraphicsPath();
			path.AddArc(rect1, 0.0f, 180f);
			path.AddArc(rect2, 180f, -180f);
			path.CloseFigure();
			g.FillPath(fillBrush, path);
			g.DrawPath(pen, path);
			path.Reset();
			path.AddEllipse(rect1);
			g.FillPath(fillBrush, path);
			g.DrawPath(pen, path);
		}

		private double Fahrenheit2Celsius(double fahrenheit)
		{
			return (fahrenheit - 32.0) / 1.8;
		}

		private double Celsius2Fahrenheit(double celsius)
		{
			return celsius * 1.8 + 32.0;
		}

		private void DrawBulb(Graphics g, RectangleF rect, bool enabled)
		{
			g.FillEllipse((Brush)this.bulbBrush, this.rectBulb);
			g.DrawEllipse(this.outlinePen, this.rectBulb);
		}

		private void DrawCylinder(Graphics g, RectangleF rect, bool enabled)
		{
			this.FillCylinder(g, this.rectCylinder, (Brush)this.fillBrush, this.colorOutline);
		}

		private void DrawValue(Graphics g, RectangleF rect, bool enabled)
		{
			if (!enabled)
				return;
			this.fRange = (float)(this.Range.Max - this.Range.Min);
			float num = (float)this.value;
			if (this.Range.Min < 0.0)
				num += (float)Math.Abs((int)this.Range.Min);
			if ((double)num > 0.0)
			{
				float height = this.rectCylinder.Height / 100f * (float)((double)num / (double)this.fRange * 100.0);
				RectangleF ctrl = new RectangleF(this.rectCylinder.Left, this.rectCylinder.Bottom - height, this.rectCylinder.Width, height);
				this.FillCylinder(g, ctrl, (Brush)this.bulbBrush, this.colorOutline);
			}
			RectangleF layoutRectangle = new RectangleF(this.pointCenter.X + 10f, this.rectBulb.Bottom + 5f, 70f, 20f);
			g.DrawString(this.value.ToString("0 [癈]"), this.fntText, (Brush)this.blackBrush, layoutRectangle, this.strfmtText);
			layoutRectangle = new RectangleF(this.pointCenter.X - 80f, this.rectBulb.Bottom + 5f, 70f, 20f);
			g.DrawString(this.Celsius2Fahrenheit(this.value).ToString("0 [癋]"), this.fntText, (Brush)this.blackBrush, layoutRectangle, this.strfmtText);
		}

		private void DrawTicks(Graphics g, RectangleF rect, bool enabled)
		{
			if (!this.drawTics)
				return;
			this.fRange = (float)(this.Range.Max - this.Range.Min);
			Font font = new Font("Arial", 7f);
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Far;
			format.LineAlignment = StringAlignment.Center;
			float num1 = this.rectCylinder.Height / this.fRange;
			float num2 = num1 * (float)this.largeTicFreq;
			long num3 = (long)this.Range.Max;
			float top1 = this.rectCylinder.Top;
			Point pt1;
			Point pt2;
			PointF point;
			while ((double)top1 <= (double)this.rectCylinder.Bottom)
			{
				pt1 = new Point((int)this.rectCylinder.Right + 3, (int)top1);
				pt2 = new Point((int)this.rectCylinder.Right + 10, (int)top1);
				g.DrawLine(this.scalePen, pt1, pt2);
				point = new PointF(this.rectCylinder.Right + 30f, top1);
				g.DrawString(num3.ToString(), font, (Brush)this.blackBrush, point, format);
				num3 -= (long)this.largeTicFreq;
				top1 += num2;
			}
			float num4 = num1 * (float)this.smallTicFreq;
			float top2 = this.rectCylinder.Top;
			while ((double)top2 <= (double)this.rectCylinder.Bottom)
			{
				pt1 = new Point((int)this.rectCylinder.Right + 3, (int)top2);
				pt2 = new Point((int)this.rectCylinder.Right + 8, (int)top2);
				g.DrawLine(this.scalePen, pt1, pt2);
				top2 += num4;
			}
			double num5 = this.Celsius2Fahrenheit(this.Range.Max);
			int num6 = (int)(num5 % 10.0);
			if (num6 != 0)
				num6 = 10 - num6;
			double num7 = num5 - (double)num6;
			this.fRange = (float)(this.Celsius2Fahrenheit(this.Range.Max) - this.Celsius2Fahrenheit(this.Range.Min));
			float num8 = this.rectCylinder.Height / this.fRange;
			float num9 = num8 * (float)this.largeTicFreq;
			num3 = (long)this.Celsius2Fahrenheit(this.Range.Min);
			float bottom1 = this.rectCylinder.Bottom;
			while ((double)bottom1 >= (double)this.rectCylinder.Top)
			{
				pt1 = new Point((int)this.rectCylinder.Left - 10, (int)bottom1);
				pt2 = new Point((int)this.rectCylinder.Left - 3, (int)bottom1);
				g.DrawLine(this.scalePen, pt1, pt2);
				point = new PointF(this.rectCylinder.Left - 15f, bottom1);
				g.DrawString(num3.ToString(), font, (Brush)this.blackBrush, point, format);
				num3 += (long)this.largeTicFreq;
				bottom1 -= num9;
			}
			float num10 = num8 * (float)this.smallTicFreq;
			float bottom2 = this.rectCylinder.Bottom;
			while ((double)bottom2 >= (double)this.rectCylinder.Top)
			{
				pt1 = new Point((int)this.rectCylinder.Left - 8, (int)bottom2);
				pt2 = new Point((int)this.rectCylinder.Left - 3, (int)bottom2);
				g.DrawLine(this.scalePen, pt1, pt2);
				bottom2 -= num10;
			}
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
				Graphics g = Graphics.FromImage(image);
				g.SmoothingMode = SmoothingMode.HighQuality;
				RectangleF rect = new RectangleF(0.0f, 0.0f, (float)this.Width, (float)this.Height);
				this.DrawValue(g, rect, this.Enabled);
				e.Graphics.DrawImage(image, rect);
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (!this.enableTransparentBackground)
				base.OnPaintBackground(e);
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.FillRectangle((Brush)new SolidBrush(Color.Transparent), new RectangleF(0.0f, 0.0f, (float)this.Width, (float)this.Height));
			if (this.backgroundImg == null || this.requiresRedraw)
			{
				this.backgroundImg = (Image)new Bitmap(this.Width, this.Height);
				Graphics g = Graphics.FromImage(this.backgroundImg);
				g.SmoothingMode = SmoothingMode.HighQuality;
				this.rectBackgroundImg = new RectangleF(0.0f, 0.0f, (float)this.Width, (float)this.Height);
				this.pointCenter = new PointF(this.rectBackgroundImg.Left + this.rectBackgroundImg.Width / 2f, this.rectBackgroundImg.Top + this.rectBackgroundImg.Height / 2f);
				this.fTmpWidth = this.rectBackgroundImg.Width / 5f;
				this.rectBulb = new RectangleF(this.pointCenter.X - this.fTmpWidth, this.rectBackgroundImg.Bottom - (float)((double)this.fTmpWidth * 2.0 + 25.0), this.fTmpWidth * 2f, this.fTmpWidth * 2f);
				this.rectCylinder = new RectangleF(this.pointCenter.X - this.fTmpWidth / 2f, this.rectBackgroundImg.Top + (this.drawTics ? 25f : 10f), this.fTmpWidth, (float)((double)this.rectBulb.Top - (double)this.rectBackgroundImg.Top - (this.drawTics ? 20.0 : 5.0)));
				if (!this.Enabled)
				{
					this.colorFore = SystemColors.ControlDark;
					this.colorScale = SystemColors.GrayText;
					this.colorScaleText = SystemColors.GrayText;
					this.colorOutline = SystemColors.ControlDark;
				}
				else
				{
					this.colorFore = this.ForeColor;
					this.colorScale = Color.FromArgb(0, 0, 0);
					this.colorScaleText = Color.FromArgb(0, 0, 0);
					this.colorOutline = Color.FromArgb(64, 0, 0);
				}
				this.forePen = new Pen(this.colorFore);
				this.scalePen = new Pen(this.colorScale);
				this.outlinePen = new Pen(this.colorOutline);
				this.blackBrush = new SolidBrush(this.colorScaleText);
				this.fillBrush = new SolidBrush(this.colorBack);
				this.bulbBrush = new LinearGradientBrush(this.rectBulb, this.OffsetColor(this.colorFore, (short)55), this.OffsetColor(this.colorFore, (short)-55), LinearGradientMode.Horizontal);
				this.strfmtText.Alignment = StringAlignment.Center;
				this.strfmtText.LineAlignment = StringAlignment.Center;
				this.DrawBulb(g, this.rectBackgroundImg, this.Enabled);
				this.DrawCylinder(g, this.rectBackgroundImg, this.Enabled);
				RectangleF rect = new RectangleF(this.rectCylinder.X, this.rectCylinder.Y - 5f, this.rectCylinder.Width, 5f);
				g.DrawEllipse(this.outlinePen, rect);
				this.DrawTicks(g, this.rectBackgroundImg, this.Enabled);
				this.requiresRedraw = false;
			}
			e.Graphics.DrawImage(this.backgroundImg, this.rectBackgroundImg);
		}

		public class RangeTypeConverter : TypeConverter
		{
			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				return TypeDescriptor.GetProperties(typeof(TempCtrl.Ranges));
			}
		}

		[Description("Range.")]
		[TypeConverter(typeof(TempCtrl.RangeTypeConverter))]
		[Category("Behavior")]
		public class Ranges
		{
			private double min;
			private double max;

			[Description("Minimum value.")]
			public double Min
			{
				get
				{
					return this.min;
				}
				set
				{
					this.min = value;
					if (this.PropertyChanged == null)
						return;
					this.PropertyChanged();
				}
			}

			[Description("Maximum value.")]
			public double Max
			{
				get
				{
					return this.max;
				}
				set
				{
					this.max = value;
					if (this.PropertyChanged == null)
						return;
					this.PropertyChanged();
				}
			}

			public event TempCtrl.Ranges.PropertyChangedEventHandler PropertyChanged;

			public Ranges()
			{
				this.min = -40.0;
				this.Max = 365.0;
			}

			public Ranges(double min, double max)
			{
				this.min = min;
				this.max = max;
			}

			public override string ToString()
			{
				return (string)(object)this.max + (object)"; " + (string)(object)this.min;
			}

			public delegate void PropertyChangedEventHandler();
		}
	}
}