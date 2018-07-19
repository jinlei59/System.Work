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
    public class RoiPolygonElement : PolygonElement
    {
        #region 变量

        List<DragHandle> DragHandleList = new List<DragHandle>();
        protected Guid _gravityPtUid = Guid.NewGuid();

        private bool _isLeftMouseDown = false;
        private DragHandleAnchor _leftMouseDownAnchor = DragHandleAnchor.None;
        private Point _lastMousePoint = Point.Empty;
        private PointF _lastImagePoint = PointF.Empty;
        protected Dictionary<Guid, PointF> _lastPolygonPts = new Dictionary<Guid, PointF>();
        private DragHandle _selectHandle = null;

        #endregion

        #region 构造函数
        public RoiPolygonElement() : base()
        {
            InitDragHandleList();
        }

        public RoiPolygonElement(List<PointF> pts) : base(pts)
        {
            InitDragHandleList();
        }

        public RoiPolygonElement(Dictionary<Guid, PointF> pts) : base(pts)
        {
            InitDragHandleList();
        }
        #endregion

        #region 自定义方法

        protected virtual void InitDragHandleList()
        {
            foreach (var keyVal in _polygonPts)
            {
                DragHandleList.Add(new DragHandle(DragHandleAnchor.TopLeft) { Uid = keyVal.Key });
            }
            DragHandleList.Add(new DragHandle(DragHandleAnchor.MiddleCenter) { Uid = _gravityPtUid });
        }

        #endregion

        #region 重写

        #region 绘制
        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty())
                return;
            #region 绘制多边形
            base.Draw(g, box);
            #endregion
            #region 绘制锚点
            if (Selected)
            {
                int halfDragHandleSize = DragHandleSize / 2;
                foreach (var keyval in _polygonPts)
                {
                    var pt = box.GetOffsetPoint(keyval.Value);
                    var drag = DragHandleList.FirstOrDefault(x => x.Uid.Equals(keyval.Key));
                    if (drag != null)
                        drag.Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                }
                {
                    //重心
                    var pt = box.GetOffsetPoint(GravityPt);
                    var drag = DragHandleList.FirstOrDefault(x => x.Uid.Equals(_gravityPtUid));
                    if (drag != null)
                        drag.Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                }
                DrawDragHandles(g);
            }
            #endregion
        }

        protected override void DrawDragHandles(Graphics graphics)
        {
            if (DragHandleList != null && DragHandleList.Count > 0)
            {
                foreach (var handle in DragHandleList)
                    DrawDragHandle(graphics, handle);
            }
        }

        #endregion

        #region 击中测试

        public override DragHandle HitTest(Point point)
        {
            if (DragHandleList == null || DragHandleList.Count < 1)
                return null;
            DragHandle result = null;
            foreach (DragHandle handle in DragHandleList)
            {
                if (handle.Visible && handle.Bounds.Contains(point))
                {
                    result = handle;
                    break;
                }
            }
            return result;
        }

        #endregion

        #region 设置鼠标样式

        protected override DragHandleAnchor SetCursor(Point point, ImageBox box)
        {
            Cursor cursor = Cursors.Default;
            DragHandleAnchor handleAnchor = DragHandleAnchor.None;
            if (Selected)
            {
                var handle = HitTest(point);
                if (handle != null)
                {
                    handleAnchor = handle.Anchor;
                    if (handleAnchor != DragHandleAnchor.None && handle.Enabled)
                    {
                        _selectHandle = handle;
                        switch (handleAnchor)
                        {
                            case DragHandleAnchor.TopLeft:
                                cursor = Cursors.SizeAll;
                                break;
                            case DragHandleAnchor.MiddleCenter:
                                cursor = Cursors.SizeAll;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }

            box.Cursor = cursor;
            return handleAnchor;
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
                if (_leftMouseDownAnchor == DragHandleAnchor.TopLeft)
                    _lastImagePoint = GetPt(_selectHandle.Uid);
                else if (_leftMouseDownAnchor == DragHandleAnchor.MiddleCenter)
                {
                    _lastPolygonPts.Clear();
                    foreach (var keyVal in _polygonPts)
                        _lastPolygonPts.Add(keyVal.Key, keyVal.Value);
                }
                box.Invalidate();
            }
        }
        internal override void MouseMove(MouseEventArgs e, ImageBox box)
        {
            if (e.Button == MouseButtons.Left)
            {//移动、改变大小
                if (_isLeftMouseDown && _leftMouseDownAnchor != DragHandleAnchor.None)
                {
                    switch (_leftMouseDownAnchor)
                    {
                        case DragHandleAnchor.TopLeft:
                            if (_selectHandle != null)
                            {
                                float x = _lastImagePoint.X + (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                                float y = _lastImagePoint.Y + (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;
                                var pt = new PointF(x, y);
                                SetPt(_selectHandle.Uid, pt);
                                OnPolygonShapeChannged();
                            }
                            break;
                        case DragHandleAnchor.MiddleCenter:
                            if (_selectHandle != null)
                            {
                                foreach (var keyVal in _lastPolygonPts)
                                {
                                    float x = keyVal.Value.X + (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                                    float y = keyVal.Value.Y + (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;
                                    var pt = new PointF(x, y);
                                    SetPt(keyVal.Key, pt);
                                    OnPolygonShapeChannged();
                                }
                            }
                            break;
                        default:
                            break;
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

        #endregion
    }
}
