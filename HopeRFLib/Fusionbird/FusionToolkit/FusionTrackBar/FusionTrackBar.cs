using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Fusionbird.FusionToolkit.FusionTrackBar
{
	public class FusionTrackBar : System.Windows.Forms.TrackBar
	{
		public event EventHandler<TrackBarDrawItemEventArgs> DrawChannel;
		public event EventHandler<TrackBarDrawItemEventArgs> DrawThumb;
		public event EventHandler<TrackBarDrawItemEventArgs> DrawTicks;

		private Rectangle ChannelBounds;
		private TrackBarOwnerDrawParts m_OwnerDrawParts;
		private Rectangle ThumbBounds;
		private int ThumbState;

		[Editor(typeof(TrackDrawModeEditor), typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Description("Gets/sets the trackbar parts that will be OwnerDrawn.")]
		[DefaultValue(typeof(TrackBarOwnerDrawParts), "None")]
		public TrackBarOwnerDrawParts OwnerDrawParts
		{
			get { return m_OwnerDrawParts; }
			set { m_OwnerDrawParts = value; }
		}

		public FusionTrackBar()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 20)
			{
				m.Result = IntPtr.Zero;
			}
			else
			{
				base.WndProc(ref m);
				if (m.Msg != 8270)
					return;
				Fusionbird.FusionToolkit.NativeMethods.NMHDR nmhdr = (Fusionbird.FusionToolkit.NativeMethods.NMHDR)Marshal.PtrToStructure(m.LParam, typeof(Fusionbird.FusionToolkit.NativeMethods.NMHDR));
				if (nmhdr.code != -12)
					return;
				Marshal.StructureToPtr((object)nmhdr, m.LParam, false);
				Fusionbird.FusionToolkit.NativeMethods.NMCUSTOMDRAW nmcustomdraw = (Fusionbird.FusionToolkit.NativeMethods.NMCUSTOMDRAW)Marshal.PtrToStructure(m.LParam, typeof(Fusionbird.FusionToolkit.NativeMethods.NMCUSTOMDRAW));
				if (nmcustomdraw.dwDrawStage == Fusionbird.FusionToolkit.NativeMethods.CustomDrawDrawStage.CDDS_PREPAINT)
				{
					Graphics graphics = Graphics.FromHdc(nmcustomdraw.hdc);
					PaintEventArgs e = new PaintEventArgs(graphics, Bounds);
					e.Graphics.TranslateTransform((float)-Left, (float)-Top);
					InvokePaintBackground(Parent, e);
					InvokePaint(Parent, e);
					SolidBrush solidBrush = new SolidBrush(BackColor);
					e.Graphics.FillRectangle((Brush)solidBrush, Bounds);
					solidBrush.Dispose();
					e.Graphics.ResetTransform();
					e.Dispose();
					graphics.Dispose();
					IntPtr num = new IntPtr(48);
					m.Result = num;
				}
				else if (nmcustomdraw.dwDrawStage == Fusionbird.FusionToolkit.NativeMethods.CustomDrawDrawStage.CDDS_POSTPAINT)
				{
					OnDrawTicks(nmcustomdraw.hdc);
					OnDrawChannel(nmcustomdraw.hdc);
					OnDrawThumb(nmcustomdraw.hdc);
				}
				else
				{
					if (nmcustomdraw.dwDrawStage != Fusionbird.FusionToolkit.NativeMethods.CustomDrawDrawStage.CDDS_ITEMPREPAINT)
						return;
					if (nmcustomdraw.dwItemSpec.ToInt32() == 2)
					{
						ThumbBounds = nmcustomdraw.rc.ToRectangle();
						ThumbState = !Enabled ? 5 : (nmcustomdraw.uItemState != Fusionbird.FusionToolkit.NativeMethods.CustomDrawItemState.CDIS_SELECTED ? 1 : 3);
						OnDrawThumb(nmcustomdraw.hdc);
					}
					else if (nmcustomdraw.dwItemSpec.ToInt32() == 3)
					{
						ChannelBounds = nmcustomdraw.rc.ToRectangle();
						OnDrawChannel(nmcustomdraw.hdc);
					}
					else if (nmcustomdraw.dwItemSpec.ToInt32() == 1)
						OnDrawTicks(nmcustomdraw.hdc);
					IntPtr num = new IntPtr(4);
					m.Result = num;
				}
			}
		}

		private void DrawHorizontalTicks(Graphics g, Color color)
		{
			int num1 = Maximum / TickFrequency - 1;
			Pen pen = new Pen(color);
			RectangleF rectangleF1 = new RectangleF((float)(ChannelBounds.Left + ThumbBounds.Width / 2), (float)(ThumbBounds.Top - 5), 0.0f, 3f);
			RectangleF rectangleF2 = new RectangleF((float)(ChannelBounds.Right - ThumbBounds.Width / 2 - 1), (float)(ThumbBounds.Top - 5), 0.0f, 3f);
			float x = (rectangleF2.Right - rectangleF1.Left) / (float)(num1 + 1);
			RectangleF rectangleF3;
			if (TickStyle != TickStyle.BottomRight)
			{
				g.DrawLine(pen, rectangleF1.Left, rectangleF1.Top, rectangleF1.Right, rectangleF1.Bottom);
				g.DrawLine(pen, rectangleF2.Left, rectangleF2.Top, rectangleF2.Right, rectangleF2.Bottom);
				rectangleF3 = rectangleF1;
				--rectangleF3.Height;
				rectangleF3.Offset(x, 1f);
				int num2 = num1 - 1;
				for (int index = 0; index <= num2; ++index)
				{
					g.DrawLine(pen, rectangleF3.Left, rectangleF3.Top, rectangleF3.Left, rectangleF3.Bottom);
					rectangleF3.Offset(x, 0.0f);
				}
			}
			rectangleF1.Offset(0.0f, (float)(ThumbBounds.Height + 6));
			rectangleF2.Offset(0.0f, (float)(ThumbBounds.Height + 6));
			if (TickStyle != TickStyle.TopLeft)
			{
				g.DrawLine(pen, rectangleF1.Left, rectangleF1.Top, rectangleF1.Left, rectangleF1.Bottom);
				g.DrawLine(pen, rectangleF2.Left, rectangleF2.Top, rectangleF2.Left, rectangleF2.Bottom);
				rectangleF3 = rectangleF1;
				--rectangleF3.Height;
				rectangleF3.Offset(x, 0.0f);
				int num2 = num1 - 1;
				for (int index = 0; index <= num2; ++index)
				{
					g.DrawLine(pen, rectangleF3.Left, rectangleF3.Top, rectangleF3.Left, rectangleF3.Bottom);
					rectangleF3.Offset(x, 0.0f);
				}
			}
			pen.Dispose();
		}

		private void DrawVerticalTicks(Graphics g, Color color)
		{
			int num1 = Maximum / TickFrequency - 1;
			Pen pen = new Pen(color);
			RectangleF rectangleF1 = new RectangleF((float)(ThumbBounds.Left - 5), (float)(ChannelBounds.Bottom - ThumbBounds.Height / 2 - 1), 3f, 0.0f);
			RectangleF rectangleF2 = new RectangleF((float)(ThumbBounds.Left - 5), (float)(ChannelBounds.Top + ThumbBounds.Height / 2), 3f, 0.0f);
			float y = (rectangleF2.Bottom - rectangleF1.Top) / (float)(num1 + 1);
			RectangleF rectangleF3;
			if (TickStyle != TickStyle.BottomRight)
			{
				g.DrawLine(pen, rectangleF1.Left, rectangleF1.Top, rectangleF1.Right, rectangleF1.Bottom);
				g.DrawLine(pen, rectangleF2.Left, rectangleF2.Top, rectangleF2.Right, rectangleF2.Bottom);
				rectangleF3 = rectangleF1;
				--rectangleF3.Width;
				rectangleF3.Offset(1f, y);
				int num2 = num1 - 1;
				for (int index = 0; index <= num2; ++index)
				{
					g.DrawLine(pen, rectangleF3.Left, rectangleF3.Top, rectangleF3.Right, rectangleF3.Bottom);
					rectangleF3.Offset(0.0f, y);
				}
			}
			rectangleF1.Offset((float)(ThumbBounds.Width + 6), 0.0f);
			rectangleF2.Offset((float)(ThumbBounds.Width + 6), 0.0f);
			if (TickStyle != TickStyle.TopLeft)
			{
				g.DrawLine(pen, rectangleF1.Left, rectangleF1.Top, rectangleF1.Right, rectangleF1.Bottom);
				g.DrawLine(pen, rectangleF2.Left, rectangleF2.Top, rectangleF2.Right, rectangleF2.Bottom);
				rectangleF3 = rectangleF1;
				--rectangleF3.Width;
				rectangleF3.Offset(0.0f, y);
				int num2 = num1 - 1;
				for (int index = 0; index <= num2; ++index)
				{
					g.DrawLine(pen, rectangleF3.Left, rectangleF3.Top, rectangleF3.Right, rectangleF3.Bottom);
					rectangleF3.Offset(0.0f, y);
				}
			}
			pen.Dispose();
		}

		private void DrawPointerDown(Graphics g)
		{
			Point[] points1 = new Point[6]
				{
					new Point(ThumbBounds.Left + ThumbBounds.Width / 2, ThumbBounds.Bottom - 1),
					new Point(ThumbBounds.Left, ThumbBounds.Bottom - ThumbBounds.Width / 2 - 1),
					ThumbBounds.Location,
					new Point(ThumbBounds.Right - 1, ThumbBounds.Top),
					new Point(ThumbBounds.Right - 1, ThumbBounds.Bottom - ThumbBounds.Width / 2 - 1),
					new Point(ThumbBounds.Left + ThumbBounds.Width / 2, ThumbBounds.Bottom - 1)
				};
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points1);
			Region region = new Region(path);
			g.Clip = region;
			if (ThumbState == 3 || !Enabled)
				ControlPaint.DrawButton(g, ThumbBounds, ButtonState.All);
			else
				g.Clear(SystemColors.Control);
			g.ResetClip();
			region.Dispose();
			path.Dispose();
			Point[] points2 = new Point[4] { points1[0], points1[1], points1[2], points1[3] };
			g.DrawLines(SystemPens.ControlLightLight, points2);
			Point[] points3 = new Point[3] { points1[3], points1[4], points1[5] };
			g.DrawLines(SystemPens.ControlDarkDark, points3);
			points1[0].Offset(0, -1);
			points1[1].Offset(1, 0);
			points1[2].Offset(1, 1);
			points1[3].Offset(-1, 1);
			points1[4].Offset(-1, 0);
			points1[5] = points1[0];
			Point[] points4 = new Point[4] { points1[0], points1[1], points1[2], points1[3] };
			g.DrawLines(SystemPens.ControlLight, points4);
			Point[] points5 = new Point[3] { points1[3], points1[4], points1[5] };
			g.DrawLines(SystemPens.ControlDark, points5);
		}

		private void DrawPointerLeft(Graphics g)
		{
			Point[] points1 = new Point[6]
				{
					new Point(ThumbBounds.Left, ThumbBounds.Top + ThumbBounds.Height / 2),
					new Point(ThumbBounds.Left + ThumbBounds.Height / 2, ThumbBounds.Top),
					new Point(ThumbBounds.Right - 1, ThumbBounds.Top),
					new Point(ThumbBounds.Right - 1, ThumbBounds.Bottom - 1),
					new Point(ThumbBounds.Left + ThumbBounds.Height / 2, ThumbBounds.Bottom - 1),
					new Point(ThumbBounds.Left, ThumbBounds.Top + ThumbBounds.Height / 2)
				};
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points1);
			Region region = new Region(path);
			g.Clip = region;
			if (ThumbState == 3 || !Enabled)
				ControlPaint.DrawButton(g, ThumbBounds, ButtonState.All);
			else
				g.Clear(SystemColors.Control);
			g.ResetClip();
			region.Dispose();
			path.Dispose();
			Point[] points2 = new Point[3] { points1[0], points1[1], points1[2] };
			g.DrawLines(SystemPens.ControlLightLight, points2);
			Point[] points3 = new Point[4] { points1[2], points1[3], points1[4], points1[5] };
			g.DrawLines(SystemPens.ControlDarkDark, points3);
			points1[0].Offset(1, 0);
			points1[1].Offset(0, 1);
			points1[2].Offset(-1, 1);
			points1[3].Offset(-1, -1);
			points1[4].Offset(0, -1);
			points1[5] = points1[0];
			Point[] points4 = new Point[3] { points1[0], points1[1], points1[2] };
			g.DrawLines(SystemPens.ControlLight, points4);
			Point[] points5 = new Point[4] { points1[2], points1[3], points1[4], points1[5] };
			g.DrawLines(SystemPens.ControlDark, points5);
		}

		private void DrawPointerRight(Graphics g)
		{
			Point[] points1 = new Point[6]
				{
					new Point(ThumbBounds.Left, ThumbBounds.Bottom - 1),
					new Point(ThumbBounds.Left, ThumbBounds.Top),
					new Point(ThumbBounds.Right - ThumbBounds.Height / 2 - 1, ThumbBounds.Top),
					new Point(ThumbBounds.Right - 1, ThumbBounds.Top + ThumbBounds.Height / 2),
					new Point(ThumbBounds.Right - ThumbBounds.Height / 2 - 1, ThumbBounds.Bottom - 1),
					new Point(ThumbBounds.Left, ThumbBounds.Bottom - 1)
				};
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points1);
			Region region = new Region(path);
			g.Clip = region;
			if (ThumbState == 3 || !Enabled)
				ControlPaint.DrawButton(g, ThumbBounds, ButtonState.All);
			else
				g.Clear(SystemColors.Control);
			g.ResetClip();
			region.Dispose();
			path.Dispose();
			Point[] points2 = new Point[4] { points1[0], points1[1], points1[2], points1[3] };
			g.DrawLines(SystemPens.ControlLightLight, points2);
			Point[] points3 = new Point[3] { points1[3], points1[4], points1[5] };
			g.DrawLines(SystemPens.ControlDarkDark, points3);
			points1[0].Offset(1, -1);
			points1[1].Offset(1, 1);
			points1[2].Offset(0, 1);
			points1[3].Offset(-1, 0);
			points1[4].Offset(0, -1);
			points1[5] = points1[0];
			Point[] points4 = new Point[4] { points1[0], points1[1], points1[2], points1[3] };
			g.DrawLines(SystemPens.ControlLight, points4);
			Point[] points5 = new Point[3] { points1[3], points1[4], points1[5] };
			g.DrawLines(SystemPens.ControlDark, points5);
		}

		private void DrawPointerUp(Graphics g)
		{
			Point[] points1 = new Point[6]
				{
					new Point(ThumbBounds.Left, ThumbBounds.Bottom - 1),
					new Point(ThumbBounds.Left, ThumbBounds.Top + ThumbBounds.Width / 2),
					new Point(ThumbBounds.Left + ThumbBounds.Width / 2, ThumbBounds.Top),
					new Point(ThumbBounds.Right - 1, ThumbBounds.Top + ThumbBounds.Width / 2),
					new Point(ThumbBounds.Right - 1, ThumbBounds.Bottom - 1),
					new Point(ThumbBounds.Left, ThumbBounds.Bottom - 1)
				};
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points1);
			Region region = new Region(path);
			g.Clip = region;
			if (ThumbState == 3 || !Enabled)
				ControlPaint.DrawButton(g, ThumbBounds, ButtonState.All);
			else
				g.Clear(SystemColors.Control);
			g.ResetClip();
			region.Dispose();
			path.Dispose();
			Point[] points2 = new Point[3] { points1[0], points1[1], points1[2] };
			g.DrawLines(SystemPens.ControlLightLight, points2);
			Point[] points3 = new Point[4] { points1[2], points1[3], points1[4], points1[5] };
			g.DrawLines(SystemPens.ControlDarkDark, points3);
			points1[0].Offset(1, -1);
			points1[1].Offset(1, 0);
			points1[2].Offset(0, 1);
			points1[3].Offset(-1, 0);
			points1[4].Offset(-1, -1);
			points1[5] = points1[0];
			Point[] points4 = new Point[3] { points1[0], points1[1], points1[2] };
			g.DrawLines(SystemPens.ControlLight, points4);
			Point[] points5 = new Point[4] { points1[2], points1[3], points1[4], points1[5] };
			g.DrawLines(SystemPens.ControlDark, points5);
		}

		protected virtual void OnDrawTicks(IntPtr hdc)
		{
			Graphics graphics = Graphics.FromHdc(hdc);
			if ((OwnerDrawParts & TrackBarOwnerDrawParts.Ticks) == TrackBarOwnerDrawParts.Ticks && !DesignMode)
			{
				TrackBarDrawItemEventArgs e = new TrackBarDrawItemEventArgs(graphics, ClientRectangle, (TrackBarItemState)ThumbState);
				if (DrawTicks != null)
					DrawTicks((object)this, e);
			}
			else
			{
				if (TickStyle == TickStyle.None || ThumbBounds.Equals((object)Rectangle.Empty))
					return;
				Color color = Color.Black;
				if (VisualStyleRenderer.IsSupported)
					color = new VisualStyleRenderer("TRACKBAR", 9, ThumbState).GetColor(ColorProperty.TextColor);
				if (Orientation == Orientation.Horizontal)
					DrawHorizontalTicks(graphics, color);
				else
					DrawVerticalTicks(graphics, color);
			}
			graphics.Dispose();
		}

		protected virtual void OnDrawThumb(IntPtr hdc)
		{
			Graphics graphics = Graphics.FromHdc(hdc);
			graphics.Clip = new Region(ThumbBounds);
			if ((OwnerDrawParts & TrackBarOwnerDrawParts.Thumb) == TrackBarOwnerDrawParts.Thumb && !DesignMode)
			{
				TrackBarDrawItemEventArgs e = new TrackBarDrawItemEventArgs(graphics, ThumbBounds, (TrackBarItemState)ThumbState);
				if (DrawThumb != null)
					DrawThumb((object)this, e);
			}
			else
			{
				Fusionbird.FusionToolkit.NativeMethods.TrackBarParts trackBarParts = Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMB;
				if (ThumbBounds.Equals((object)Rectangle.Empty))
					return;
				switch (TickStyle)
				{
					case TickStyle.None:
					case TickStyle.BottomRight:
						trackBarParts = Orientation != Orientation.Horizontal ? Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBRIGHT : Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBBOTTOM;
						break;
					case TickStyle.TopLeft:
						trackBarParts = Orientation != Orientation.Horizontal ? Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBLEFT : Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBTOP;
						break;
					case TickStyle.Both:
						trackBarParts = Orientation != Orientation.Horizontal ? Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBVERT : Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMB;
						break;
				}
				if (VisualStyleRenderer.IsSupported)
				{
					new VisualStyleRenderer("TRACKBAR", (int)trackBarParts, ThumbState).DrawBackground((IDeviceContext)graphics, ThumbBounds);
					graphics.ResetClip();
					graphics.Dispose();
					return;
				}
				else
				{
					switch (trackBarParts)
					{
						case Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBBOTTOM:
							DrawPointerDown(graphics);
							break;
						case Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBTOP:
							DrawPointerUp(graphics);
							break;
						case Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBLEFT:
							DrawPointerLeft(graphics);
							break;
						case Fusionbird.FusionToolkit.NativeMethods.TrackBarParts.TKP_THUMBRIGHT:
							DrawPointerRight(graphics);
							break;
						default:
							if (ThumbState == 3 || !Enabled)
								ControlPaint.DrawButton(graphics, ThumbBounds, ButtonState.All);
							else
								graphics.FillRectangle(SystemBrushes.Control, ThumbBounds);
							ControlPaint.DrawBorder3D(graphics, ThumbBounds, Border3DStyle.Raised);
							break;
					}
				}
			}
			graphics.ResetClip();
			graphics.Dispose();
		}

		protected virtual void OnDrawChannel(IntPtr hdc)
		{
			Graphics graphics = Graphics.FromHdc(hdc);
			if ((OwnerDrawParts & TrackBarOwnerDrawParts.Channel) == TrackBarOwnerDrawParts.Channel && !DesignMode)
			{
				TrackBarDrawItemEventArgs e = new TrackBarDrawItemEventArgs(graphics, ChannelBounds, (TrackBarItemState)ThumbState);
				if (DrawChannel != null)
					DrawChannel((object)this, e);
			}
			else
			{
				if (ChannelBounds.Equals((object)Rectangle.Empty))
					return;
				if (VisualStyleRenderer.IsSupported)
				{
					new VisualStyleRenderer("TRACKBAR", 1, 1).DrawBackground((IDeviceContext)graphics, ChannelBounds);
					graphics.ResetClip();
					graphics.Dispose();
					return;
				}
				else
					ControlPaint.DrawBorder3D(graphics, ChannelBounds, Border3DStyle.Sunken);
			}
			graphics.Dispose();
		}
	}
}