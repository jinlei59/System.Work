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
    public class RoiRingElement : RingElement
    {
        #region 变量

        private bool _isLeftMouseDown = false;
        private DragHandleAnchor _leftMouseDownAnchor = DragHandleAnchor.None;
        private Point _lastMousePoint = Point.Empty;
        private PointF _lastImagePoint = PointF.Empty;
        private float _lastSradius = 1f;
        private float _lastEradius = 1f;

        #endregion

        #region 属性
        /// <summary>
        /// 外-上-TopLeft
        /// </summary>
        public PointF Outer_Top_TopLeft { get { return new PointF(Cpt.X, Cpt.Y - Eradius); } }
        /// <summary>
        /// 外-下-TopRight
        /// </summary>
        public PointF Outer_Bottom_TopRight { get { return new PointF(Cpt.X, Cpt.Y + Eradius); } }
        /// <summary>
        /// 外-左-BottomLeft
        /// </summary>
        public PointF Outer_Left_BottomLeft { get { return new PointF(Cpt.X - Eradius, Cpt.Y); } }
        /// <summary>
        /// 外-右-BottomRight
        /// </summary>
        public PointF Outer_Right_BottomRight { get { return new PointF(Cpt.X + Eradius, Cpt.Y); } }
        /// <summary>
        /// 内-上-TopCenter
        /// </summary>
        public PointF Inner_Top_TopCenter { get { return new PointF(Cpt.X, Cpt.Y - Sradius); } }
        /// <summary>
        /// 内-下-BottomCenter
        /// </summary>
        public PointF Inner_Bottom_BottomCenter { get { return new PointF(Cpt.X, Cpt.Y + Sradius); } }
        /// <summary>
        /// 内-左-MiddleLeft
        /// </summary>
        public PointF Inner_Left_MiddleLeft { get { return new PointF(Cpt.X - Sradius, Cpt.Y); } }
        /// <summary>
        /// 内-右-MiddleRight
        /// </summary>
        public PointF Inner_Right_MiddleRight { get { return new PointF(Cpt.X + Sradius, Cpt.Y); } }

        #endregion

        #region 构造函数
        public RoiRingElement(PointF cpt, float sradius, float eradius) : base(cpt, sradius, eradius)
        {
            DragHandleCollection = new DragHandleCollection();
            DragHandleCollection.AddDefaultDragHandle();
        }
        #endregion

        #region 自定义

        #endregion

        #region 重写

        #region 设置鼠标样式

        protected override DragHandleAnchor SetCursor(Point point, ImageBox box)
        {
            Cursor cursor;
            DragHandleAnchor handleAnchor;
            if (!Selected)
            {
                cursor = Cursors.Default;
                handleAnchor = DragHandleAnchor.None;
            }
            else
            {
                var handle = HitTest(point);
                handleAnchor = handle == null ? DragHandleAnchor.None : handle.Anchor;
                if (handleAnchor != DragHandleAnchor.None && DragHandleCollection[handleAnchor].Enabled)
                {
                    switch (handleAnchor)
                    {
                        case DragHandleAnchor.TopLeft:
                        case DragHandleAnchor.TopRight:
                        case DragHandleAnchor.TopCenter:
                        case DragHandleAnchor.BottomCenter:
                            cursor = Cursors.SizeNS;
                            break;
                        case DragHandleAnchor.BottomLeft:
                        case DragHandleAnchor.BottomRight:
                        case DragHandleAnchor.MiddleLeft:
                        case DragHandleAnchor.MiddleRight:
                            cursor = Cursors.SizeWE;
                            break;
                        case DragHandleAnchor.MiddleCenter:
                            cursor = Cursors.SizeAll;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    cursor = Cursors.Default;
                }
            }

            box.Cursor = cursor;
            return handleAnchor;
        }

        #endregion

        #region 绘制
        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty())
                return;
            #region 绘制圆环
            base.Draw(g, box);
            #endregion
            #region 绘制锚点
            //外圆:左上，左下，右上，右下
            //内圆：上中心，下中心，左中心，右中心
            if (Selected)
            {
                int halfDragHandleSize = DragHandleSize / 2;
                var pt = PointF.Empty;
                //外-上-TopLeft
                pt = box.GetOffsetPoint(Outer_Top_TopLeft);
                DragHandleCollection[DragHandleAnchor.TopLeft].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                //外-下-TopRight
                pt = box.GetOffsetPoint(Outer_Bottom_TopRight);
                DragHandleCollection[DragHandleAnchor.TopRight].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                //外-左-BottomLeft
                pt = box.GetOffsetPoint(Outer_Left_BottomLeft);
                DragHandleCollection[DragHandleAnchor.BottomLeft].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                //外-右-BottomRight
                pt = box.GetOffsetPoint(Outer_Right_BottomRight);
                DragHandleCollection[DragHandleAnchor.BottomRight].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                //内-上-TopCenter
                pt = box.GetOffsetPoint(Inner_Top_TopCenter);
                DragHandleCollection[DragHandleAnchor.TopCenter].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                //内-下-BottomCenter
                pt = box.GetOffsetPoint(Inner_Bottom_BottomCenter);
                DragHandleCollection[DragHandleAnchor.BottomCenter].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                //内-左-MiddleLeft
                pt = box.GetOffsetPoint(Inner_Left_MiddleLeft);
                DragHandleCollection[DragHandleAnchor.MiddleLeft].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                //内-右-MiddleRight
                pt = box.GetOffsetPoint(Inner_Right_MiddleRight);
                DragHandleCollection[DragHandleAnchor.MiddleRight].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                //圆心
                pt = box.GetOffsetPoint(Cpt);
                DragHandleCollection[DragHandleAnchor.MiddleCenter].Bounds = new Rectangle((int)pt.X - halfDragHandleSize, (int)pt.Y - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);

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
                _lastImagePoint = Cpt;
                _lastSradius = Sradius;
                _lastEradius = Eradius;
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
                        Cpt = new PointF(x, y);
                    }
                    else
                    {
                        float offx = (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                        float offy = (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;
                        switch (_leftMouseDownAnchor)
                        {
                            #region Resize
                            #region 外圆
                            case DragHandleAnchor.TopLeft:
                                Eradius = _lastEradius - offy;
                                break;
                            case DragHandleAnchor.TopRight:
                                Eradius = _lastEradius + offy;
                                break;
                            case DragHandleAnchor.BottomLeft:
                                Eradius = _lastEradius - offx;
                                break;
                            case DragHandleAnchor.BottomRight:
                                Eradius = _lastEradius + offx;
                                break;
                            #endregion

                            #region 内圆
                            case DragHandleAnchor.TopCenter:
                                Sradius = _lastSradius - offy;
                                break;
                            case DragHandleAnchor.BottomCenter:
                                Sradius = _lastSradius + offy;
                                break;
                            case DragHandleAnchor.MiddleLeft:
                                Sradius = _lastSradius - offx;
                                break;
                            case DragHandleAnchor.MiddleRight:
                                Sradius = _lastSradius + offx;
                                break;
                            #endregion
                            #endregion
                            default:
                                break;
                        }
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
