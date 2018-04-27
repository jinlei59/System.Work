using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class RoiRectangleElement : Element
    {
        public RoiRectangleElement()
        {
            Type = ElementType.Rectangle;

            //this.DragHandleCollection[DragHandleAnchor.Rotation].Enabled = false;
            //this.DragHandleCollection[DragHandleAnchor.Rotation].Visible = false;
        }

        public override void DrawElement(Graphics g, float zoomScale, RectangleF rect)
        {
            if (!Enable || Region.IsEmpty)
                return;
            using (Pen p = new Pen(ForeColor))
            {
                try
                {
                    Pen ppp = new Pen(ForeColor) { DashStyle = Drawing.Drawing2D.DashStyle.Dot };
                    g.DrawRectangle(ppp, rect.X, rect.Y, rect.Width, rect.Height);//绘制旋转后的边框虚线
                    if (!IsRotation)
                        Draw(g, zoomScale, rect,ForeColor);
                    else
                    {
                        float cx = rect.X + rect.Width / 2, cy = rect.Y + rect.Height / 2;
                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(Angle);
                        g.TranslateTransform(-cx, -cy);
                        Draw(g, zoomScale, rect,RotationForeColor);
                        g.DrawString(Angle.ToString(), new Font(FontFamily.Families.First(), 15f * zoomScale/*(zoomScale < 1 ? 1 : zoomScale)*/), Brushes.DeepPink, rect.X, rect.Y);
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

        private void Draw(Graphics g, float zoomScale, RectangleF rect,Color color)
        {
            using (Pen p = new Pen(color))
            {
                g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
                if (IsDirection)
                {
                    float offset = rect.Width / 4;
                    using (Pen pp = new Pen(color) { DashStyle = Drawing.Drawing2D.DashStyle.Dash })
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            PointF p1 = new PointF(rect.Left + offset * (i + 1), rect.Top);
                            PointF p2 = new PointF(rect.Left + offset * (i + 1), rect.Bottom);
                            PointF p3 = new PointF(p2.X - 5, p2.Y - 5);
                            PointF p4 = new PointF(p2.X + 5, p2.Y - 5);
                            g.DrawLine(pp, p1, p2);
                            g.DrawLine(pp, p3, p2);
                            g.DrawLine(pp, p4, p2);
                        }
                    }
                }
            }
        }
    }
}
