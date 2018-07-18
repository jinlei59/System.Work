using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.ImageBoxLib
{
    [Serializable]
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
        public RoiLineElement(PointF pt1, PointF pt2) : base(pt1, pt2)
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
            var pt1 = box.GetOffsetPoint(Pt1);
            var pt2 = box.GetOffsetPoint(Pt2);
            #region 绘制线段
            using (Pen p = new Pen(ForeColor, BorderWidth))
            {
                if (ShowArrow)
                {
                    float len = GetLength(pt1.X, pt1.Y, pt2.X, pt2.Y) * 0.2f;
                    len = len < 2 ? 2 : len > 6 ? 6 : len;
                    System.Drawing.Drawing2D.AdjustableArrowCap lineArrow = new System.Drawing.Drawing2D.AdjustableArrowCap(len, len, true);
                    p.CustomEndCap = lineArrow;
                }
                g.DrawLine(p, pt1, pt2);
            }
            #endregion
            #region 绘制锚点

            if (Selected)
            {
                int halfDragHandleSize = DragHandleSize / 2;
                int cx = Convert.ToInt32((pt1.X + pt2.X) / 2) - halfDragHandleSize;
                int cy = Convert.ToInt32((pt1.Y + pt2.Y) / 2) - halfDragHandleSize;

                DragHandleCollection[DragHandleAnchor.MiddleLeft].Bounds = new Rectangle((int)pt1.X - halfDragHandleSize, (int)pt1.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.MiddleCenter].Bounds = new Rectangle(cx, cy, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.MiddleRight].Bounds = new Rectangle((int)pt2.X - halfDragHandleSize, (int)pt2.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);

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
