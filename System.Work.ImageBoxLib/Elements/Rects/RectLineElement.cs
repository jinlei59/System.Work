using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class RectLineElement : RectElement
    {
        public int LineCount { get; set; }

        public RectLineElement(RectangleF rect, float angle = 0) : base(rect, angle)
        {
            LineCount = 1;
        }

        public RectLineElement(float x, float y, float w, float h, float angle = 0f) : base(x, y, w, h, angle)
        {
            LineCount = 1;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || Rect.IsEmpty)
                return;
            using (Pen p = new Pen(ForeColor, AutoChangeSize ? BorderWidth * box.ZoomFactor : BorderWidth))
            {
                try
                {
                    var rect = box.GetOffsetRectangle(Rect);
                    if (Angle == 0)
                    {
                        g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
                        Draw(g, rect, box);
                    }
                    else
                    {
                        float cx = rect.X + rect.Width / 2, cy = rect.Y + rect.Height / 2;
                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(Angle);
                        g.TranslateTransform(-cx, -cy);
                        g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
                        Draw(g, rect, box);
                        g.DrawString(Angle.ToString(), new Font(FontFamily.Families.First(), 15f * box.ZoomFactor), Brushes.DeepPink, rect.X, rect.Y);
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    g.ResetTransform();
                }
            }
        }

        internal void Draw(Graphics g, RectangleF rect, ImageBox box)
        {
            float offset = rect.Width / (LineCount + 1);
            using (Pen pp = new Pen(ForeColor, AutoChangeSize ? BorderWidth * box.ZoomFactor : BorderWidth) { DashStyle = DashStyle.Dash })
            {
                float len = 5;
                for (int i = 0; i < LineCount; i++)
                {
                    PointF p1 = new PointF(rect.Left + offset * (i + 1), rect.Top);
                    PointF p2 = new PointF(rect.Left + offset * (i + 1), rect.Bottom);
                    PointF p3 = new PointF(p2.X - len, p2.Y - len);
                    PointF p4 = new PointF(p2.X + len, p2.Y - len);
                    g.DrawLine(pp, p1, p2);
                    g.DrawLine(pp, p3, p2);
                    g.DrawLine(pp, p4, p2);
                }
            }
        }
    }
}
