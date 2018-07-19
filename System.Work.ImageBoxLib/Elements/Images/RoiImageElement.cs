using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.ImageBoxLib
{
    public class RoiImageElement : ImageElement
    {
        #region 变量

        private bool _isLeftMouseDown = false;
        private DragHandleAnchor _leftMouseDownAnchor = DragHandleAnchor.None;
        private RectangleF _lastRoiRegion = RectangleF.Empty;
        private Point _lastMousePoint = Point.Empty;
        private PointF _lastImagePoint = PointF.Empty;

        #endregion

        #region 构造函数

        public RoiImageElement(Bitmap image) : this(image, new RectangleF(0, 0, image.Width, image.Height))
        {
        }

        public RoiImageElement(Bitmap image, RectangleF dstRect) : this(image, new RectangleF(0, 0, image.Width, image.Height), dstRect)
        { }

        public RoiImageElement(Bitmap image, RectangleF srcRect, RectangleF dstRect) : base(image, srcRect, dstRect)
        {
            DragHandleCollection = new DragHandleCollection();
            DragHandleCollection.AddDefaultDragHandle();
        }
        #endregion

        #region 重写

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty() || _image == null)
                return;
            try
            {
                base.Draw(g, box);
                //绘制锚点
                if(Selected)
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
                    halfWidth = Convert.ToInt32(_dstRect.Width * box.ZoomFactor) / 2;
                    halfHeight = Convert.ToInt32(_dstRect.Height * box.ZoomFactor) / 2;

                    left = Convert.ToInt32((_dstRect.Left * box.ZoomFactor) + offsetX);
                    top = Convert.ToInt32((_dstRect.Top * box.ZoomFactor) + offsetY);
                    right = left + Convert.ToInt32(_dstRect.Width * box.ZoomFactor);
                    bottom = top + Convert.ToInt32(_dstRect.Height * box.ZoomFactor);

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
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.ToString());
            }
        }

        #region 鼠标操作
        internal override void MouseDown(MouseEventArgs e, ImageBox box)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = true;
                _leftMouseDownAnchor = SetCursor(e.Location, box);
                _lastRoiRegion = _dstRect;
                _lastMousePoint = e.Location;
                _lastImagePoint = _dstRect.Location;
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
                        _dstRect = new RectangleF(x, y, _dstRect.Width, _dstRect.Height);
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
                        _dstRect = new RectangleF(left, top, right - left, bottom - top);
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

        #endregion
    }
}
