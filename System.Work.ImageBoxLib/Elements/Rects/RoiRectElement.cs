using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class RoiRectElement : RectElement
    {
        public RoiRectElement(RectangleF rect, float angle = 0f) : base(rect, angle)
        {
            DragHandleCollection = new DragHandleCollection();
            DragHandleCollection.AddDefaultDragHandle();
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || Rect.IsEmpty)
                return;
            #region 绘制矩形
            using (Pen p = new Pen(ForeColor, BorderWidth))
            {
                try
                {
                    var rect = box.GetOffsetRectangle(Rect);
                    if (Angle == 0)
                        g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
                    else
                    {
                        Pen ppp = new Pen(Color.Blue) { DashStyle = Drawing.Drawing2D.DashStyle.Dot };
                        g.DrawRectangle(ppp, rect.X, rect.Y, rect.Width, rect.Height);//绘制旋转后的边框虚线

                        float cx = rect.X + rect.Width / 2, cy = rect.Y + rect.Height / 2;
                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(Angle);
                        g.TranslateTransform(-cx, -cy);
                        g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
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
            #endregion
            #region 绘制锚点

            if (Selected)
            {

                int left;
                int top;
                int right;
                int bottom;
                int halfWidth;
                int halfHeight;
                int halfDragHandleSize;
                Rectangle viewport;
                int offsetX;
                int offsetY;

                viewport = box.GetImageViewPort();
                offsetX = viewport.Left + box.Padding.Left + box.AutoScrollPosition.X;
                offsetY = viewport.Top + box.Padding.Top + box.AutoScrollPosition.Y;
                halfDragHandleSize = DragHandleSize / 2;
                halfWidth = Convert.ToInt32(Rect.Width * box.ZoomFactor) / 2;
                halfHeight = Convert.ToInt32(Rect.Height * box.ZoomFactor) / 2;

                left = Convert.ToInt32((Rect.Left * box.ZoomFactor) + offsetX);
                top = Convert.ToInt32((Rect.Top * box.ZoomFactor) + offsetY);
                right = left + Convert.ToInt32(Rect.Width * box.ZoomFactor);
                bottom = top + Convert.ToInt32(Rect.Height * box.ZoomFactor);

                DragHandleCollection[DragHandleAnchor.TopLeft].Bounds = new Rectangle(left - this.DragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.TopCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.TopRight].Bounds = new Rectangle(right, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.MiddleLeft].Bounds = new Rectangle(left - this.DragHandleSize, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.MiddleRight].Bounds = new Rectangle(right, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.BottomLeft].Bounds = new Rectangle(left - this.DragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.BottomCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.BottomRight].Bounds = new Rectangle(right, bottom, this.DragHandleSize, this.DragHandleSize);
                DrawDragHandles(g);
            }
            #endregion
        }
    }
}
