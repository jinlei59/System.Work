using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.ImageBoxLib
{
    public class RoiLineElement : LineElement
    {
        #region 变量

        private bool _isLeftMouseDown = false;
        private DragHandleAnchor _leftMouseDownAnchor = DragHandleAnchor.None;
        private Point _lastMousePoint = Point.Empty;
        private PointF _lastImagePoint1 = PointF.Empty;
        private PointF _lastImagePoint2 = PointF.Empty;

        #endregion

        #region 构造函数
        public RoiLineElement(Point pt1, PointF pt2) : base(pt1, pt2)
        {
            ShowArrow = true;
            DragHandleCollection = new DragHandleCollection();
            DragHandleCollection.AddCustomDragHandle(DragHandleAnchor.MiddleLeft);
            DragHandleCollection.AddCustomDragHandle(DragHandleAnchor.MiddleCenter);
            DragHandleCollection.AddCustomDragHandle(DragHandleAnchor.MiddleRight);
        }
        #endregion

        #region 重写

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty())
                return;
            #region 绘制线段
            using (Pen p = new Pen(ForeColor, BorderWidth * box.ZoomFactor))
            {
                if (ShowArrow)
                {
                    float len = 8 / box.ZoomFactor;
                    len = len > Length * 0.4f ? Length * 0.4f : len;
                    System.Drawing.Drawing2D.AdjustableArrowCap lineArrow = new System.Drawing.Drawing2D.AdjustableArrowCap(len, len, false);
                    p.CustomEndCap = lineArrow;
                }
                var pt1 = box.GetOffsetPoint(Pt1);
                var pt2 = box.GetOffsetPoint(Pt2);
                g.DrawLine(p, pt1, pt2);
            }
            #endregion
            #region 绘制锚点

            if (Selected)
            {
                int halfDragHandleSize = DragHandleSize / 2;
                Rectangle viewport = box.GetImageViewPort();
                int offsetX = viewport.Left + box.Padding.Left + box.AutoScrollPosition.X;
                int offsetY = viewport.Top + box.Padding.Top + box.AutoScrollPosition.Y;
                int x1 = Convert.ToInt32((Pt1.X * box.ZoomFactor) + offsetX - halfDragHandleSize);
                int y1 = Convert.ToInt32((Pt1.Y * box.ZoomFactor) + offsetX - halfDragHandleSize);
                int x2 = Convert.ToInt32((Pt2.X * box.ZoomFactor) + offsetX - halfDragHandleSize);
                int y2 = Convert.ToInt32((Pt2.Y * box.ZoomFactor) + offsetX - halfDragHandleSize);
                int cx = Convert.ToInt32((x1 + x2) / 2);
                int cy = Convert.ToInt32((y1 + y2) / 2);

                DragHandleCollection[DragHandleAnchor.MiddleLeft].Bounds = new Rectangle(x1, y1, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.MiddleCenter].Bounds = new Rectangle(cx, cy, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.MiddleRight].Bounds = new Rectangle(x2, y2, this.DragHandleSize, this.DragHandleSize);

                DrawDragHandles(g);
            }
            #endregion
        }

        #endregion

        #region 鼠标操作
        internal override void MouseDown(MouseEventArgs e, ImageBox box)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = true;
                _leftMouseDownAnchor = SetCursor(e.Location, box);
                _lastMousePoint = e.Location;
                _lastImagePoint1 = Pt1;
                _lastImagePoint2 = Pt2;
                box.Invalidate();
            }
        }
        internal override void MouseMove(MouseEventArgs e, ImageBox box)
        {
            if (e.Button == MouseButtons.Left)
            {//移动、改变大小
                if (_isLeftMouseDown && _leftMouseDownAnchor != DragHandleAnchor.None)
                {
                    if (_leftMouseDownAnchor == DragHandleAnchor.MiddleCenter)
                    {
                        float x = _lastImagePoint1.X + (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                        float y = _lastImagePoint1.Y + (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;
                        Pt1 = new PointF(x, y);
                        x = _lastImagePoint2.X + (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                        y = _lastImagePoint2.Y + (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;
                        Pt2 = new PointF(x, y);
                    }
                    else if (_leftMouseDownAnchor == DragHandleAnchor.MiddleLeft)
                    {
                        float x = _lastImagePoint1.X + (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                        float y = _lastImagePoint1.Y + (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;
                        Pt1 = new PointF(x, y);
                    }
                    else if (_leftMouseDownAnchor == DragHandleAnchor.MiddleRight)
                    {
                        float x = _lastImagePoint2.X + (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                        float y = _lastImagePoint2.Y + (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;
                        Pt2 = new PointF(x, y);
                    }
                    box.Invalidate();
                }
            }
            else if (e.Button == MouseButtons.None)
            {
                SetCursor(e.Location, box);
            }
        }
        internal override void MouseUp(MouseEventArgs e, ImageBox box)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = false;
            }
        }
        #endregion
    }
}
