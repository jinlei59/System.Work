using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.ImageBoxLib
{
    public class RoiRectElement : RectElement
    {
        #region 变量

        private bool _isLeftMouseDown = false;
        private DragHandleAnchor _leftMouseDownAnchor = DragHandleAnchor.None;
        private RectangleF _lastRoiRegion = RectangleF.Empty;
        private Point _lastMousePoint = Point.Empty;
        private PointF _lastImagePoint = PointF.Empty;

        #endregion

        #region 构造函数
        public RoiRectElement(RectangleF rect, float angle = 0f) : base(rect, angle)
        {
            DragHandleCollection = new DragHandleCollection();
            DragHandleCollection.AddDefaultDragHandle();
        }

        public RoiRectElement(float x, float y, float w, float h, float angle = 0f) : base(x, y, w, h, angle)
        {
            DragHandleCollection = new DragHandleCollection();
            DragHandleCollection.AddDefaultDragHandle();
        }
        #endregion

        #region 绘制
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
                DragHandleCollection[DragHandleAnchor.MiddleCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.MiddleRight].Bounds = new Rectangle(right, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);

                DragHandleCollection[DragHandleAnchor.BottomLeft].Bounds = new Rectangle(left - this.DragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.BottomCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                DragHandleCollection[DragHandleAnchor.BottomRight].Bounds = new Rectangle(right, bottom, this.DragHandleSize, this.DragHandleSize);

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
                _lastRoiRegion = Rect;
                _lastMousePoint = e.Location;
                _lastImagePoint = Rect.Location;
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
                        Rect = new RectangleF(x, y, Rect.Width, Rect.Height);
                    }
                    else
                    {
                        #region Resize
                        float left = this._lastRoiRegion.Left;
                        float top = this._lastRoiRegion.Top;
                        float right = this._lastRoiRegion.Right;
                        float bottom = this._lastRoiRegion.Bottom;

                        float offx = (e.Location.X - _lastMousePoint.X) / box.ZoomFactor;
                        float offy = (e.Location.Y - _lastMousePoint.Y) / box.ZoomFactor;

                        switch (_leftMouseDownAnchor)
                        {
                            #region Resize
                            case DragHandleAnchor.TopLeft:
                                top += offy;
                                if (bottom - top < MinimumRoiSize)
                                    top = bottom - MinimumRoiSize;
                                left += offx;
                                if (right - left < MinimumRoiSize)
                                    left = right - MinimumRoiSize;
                                break;
                            case DragHandleAnchor.TopCenter:
                                top += offy;
                                if (bottom - top < MinimumRoiSize)
                                    top = bottom - MinimumRoiSize;
                                break;
                            case DragHandleAnchor.TopRight:
                                top += offy;
                                if (bottom - top < MinimumRoiSize)
                                    top = bottom - MinimumRoiSize;
                                right += offx;
                                if (right - left < MinimumRoiSize)
                                    right = left + MinimumRoiSize;
                                break;
                            case DragHandleAnchor.MiddleLeft:
                                left += offx;
                                if (right - left < MinimumRoiSize)
                                    left = right - MinimumRoiSize;
                                break;
                            case DragHandleAnchor.MiddleRight:
                                right += offx;
                                if (right - left < MinimumRoiSize)
                                    right = left + MinimumRoiSize;
                                break;
                            case DragHandleAnchor.BottomLeft:
                                bottom += offy;
                                if (bottom - top < MinimumRoiSize)
                                    bottom = top + MinimumRoiSize;
                                left += offx;
                                if (right - left < MinimumRoiSize)
                                    left = right - MinimumRoiSize;
                                break;
                            case DragHandleAnchor.BottomCenter:
                                bottom += offy;
                                if (bottom - top < MinimumRoiSize)
                                    bottom = top + MinimumRoiSize;
                                break;
                            case DragHandleAnchor.BottomRight:
                                bottom += offy;
                                if (bottom - top < MinimumRoiSize)
                                    bottom = top + MinimumRoiSize;
                                right += offx;
                                if (right - left < MinimumRoiSize)
                                    right = left + MinimumRoiSize;
                                break;
                            #endregion
                            default:
                                break;
                        }
                        Rect = new RectangleF(left, top, right - left, bottom - top);
                        #endregion
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
