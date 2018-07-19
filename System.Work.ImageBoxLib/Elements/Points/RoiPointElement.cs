using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.ImageBoxLib
{
    public class RoiPointElement : PointElement
    {
        #region 变量
        private bool _isLeftMouseDown = false;
        private DragHandleAnchor _leftMouseDownAnchor = DragHandleAnchor.None;
        private Point _lastMousePoint = Point.Empty;
        private PointF _lastImagePoint = PointF.Empty;
        #endregion

        #region 构造函数
        public RoiPointElement() : this(PointF.Empty)
        { }

        public RoiPointElement(float x, float y) : this(new PointF(x, y))
        { }

        public RoiPointElement(PointF pt) : base(pt)
        {
            DragHandleCollection = new DragHandleCollection();
            DragHandleCollection.AddCustomDragHandle(DragHandleAnchor.MiddleCenter);
        }
        #endregion

        #region 自定义方法

        #endregion

        #region 重写

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty())
                return;
            base.Draw(g, box);
            #region 绘制锚点

            if (Selected)
            {
                var pt = box.GetOffsetPoint(Pt);
                int halfDragHandleSize = DragHandleSize / 2;
                int cx = (int)pt.X - halfDragHandleSize;
                int cy = (int)pt.Y - halfDragHandleSize;

                DragHandleCollection[DragHandleAnchor.MiddleCenter].Bounds = new Rectangle(cx, cy, this.DragHandleSize, this.DragHandleSize);

                DrawDragHandles(g);
            }

            #endregion
        }

        #region 鼠标操作
        internal override void MouseDown(MouseEventArgs e, ImageBox box)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = true;
                _leftMouseDownAnchor = SetCursor(e.Location, box);
                _lastMousePoint = e.Location;
                _lastImagePoint = Pt;

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
                        float x = _lastImagePoint.X + (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                        float y = _lastImagePoint.Y + (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;
                        Pt = new PointF(x, y);

                        box.Invalidate();
                    }
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

        #endregion
    }
}
