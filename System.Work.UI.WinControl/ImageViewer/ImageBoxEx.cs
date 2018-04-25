using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.UI.WinControl
{
    public class ImageBoxEx : ImageBox
    {
        #region 变量
        private IList<Element> _elements;
        private readonly DragHandleCollection _dragHandles;
        private int _dragHandleSize;
        #endregion

        #region 属性

        [Category("Appearance")]
        [DefaultValue(8)]
        public virtual int DragHandleSize
        {
            get { return _dragHandleSize; }
            set
            {
                if (this.DragHandleSize != value)
                {
                    _dragHandleSize = value;

                    this.OnDragHandleSizeChanged(EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        public DragHandleCollection DragHandles
        {
            get { return _dragHandles; }
        }

        #endregion


        #region 事件

        /// <summary>
        /// Occurs when the DragHandleSize property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler DragHandleSizeChanged;
        /// <summary>
        /// Raises the <see cref="DragHandleSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnDragHandleSizeChanged(EventArgs e)
        {
            EventHandler handler;

            this.PositionDragHandles();
            this.Invalidate();

            handler = this.DragHandleSizeChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region 构造函数
        public ImageBoxEx()
        {
            this.SelectionColor = Color.Orange;
            this.SelectionMode = ImageBoxSelectionMode.Rectangle;

            _dragHandles = new DragHandleCollection();
            this.DragHandleSize = 8;
            this.PositionDragHandles();
        }
        #endregion

        #region 重写

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.AllowPainting && !this.SelectionRegion.IsEmpty&&this.SelectionMode== ImageBoxSelectionMode.Rectangle)
            {
                foreach (DragHandle handle in this.DragHandles)
                {
                    if (handle.Visible)
                    {
                        this.DrawDragHandle(e.Graphics, handle);
                    }
                }
            }

            // draw lines、Rectangles、strings
            if (_elements != null && _elements.Count > 0)
            {
                foreach (var element in _elements)
                    element.Draw(e, Zoom);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.PositionDragHandles();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);

            this.PositionDragHandles();
        }

        protected override void OnZoomChanged(EventArgs e)
        {
            base.OnZoomChanged(e);

            this.PositionDragHandles();
        }
        protected override void OnSelectionRegionChanged(EventArgs e)
        {
            base.OnSelectionRegionChanged(e);

            this.PositionDragHandles();
        }
        #endregion

        #region ROI

        private void PositionDragHandles()
        {
            if (this.DragHandles != null && this.DragHandleSize > 0)
            {
                if (this.SelectionRegion.IsEmpty)
                {
                    foreach (DragHandle handle in this.DragHandles)
                    {
                        handle.Bounds = Rectangle.Empty;
                    }
                }
                else
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

                    viewport = this.GetImageViewPort();
                    offsetX = viewport.Left + this.Padding.Left + this.AutoScrollPosition.X;
                    offsetY = viewport.Top + this.Padding.Top + this.AutoScrollPosition.Y;
                    halfDragHandleSize = this.DragHandleSize / 2;
                    left = Convert.ToInt32((this.SelectionRegion.Left * this.ZoomFactor) + offsetX);
                    top = Convert.ToInt32((this.SelectionRegion.Top * this.ZoomFactor) + offsetY);
                    right = left + Convert.ToInt32(this.SelectionRegion.Width * this.ZoomFactor);
                    bottom = top + Convert.ToInt32(this.SelectionRegion.Height * this.ZoomFactor);
                    halfWidth = Convert.ToInt32(this.SelectionRegion.Width * this.ZoomFactor) / 2;
                    halfHeight = Convert.ToInt32(this.SelectionRegion.Height * this.ZoomFactor) / 2;

                    this.DragHandles[DragHandleAnchor.TopLeft].Bounds = new Rectangle(left - this.DragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    this.DragHandles[DragHandleAnchor.TopCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    this.DragHandles[DragHandleAnchor.TopRight].Bounds = new Rectangle(right, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    this.DragHandles[DragHandleAnchor.MiddleLeft].Bounds = new Rectangle(left - this.DragHandleSize, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    this.DragHandles[DragHandleAnchor.MiddleRight].Bounds = new Rectangle(right, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    this.DragHandles[DragHandleAnchor.BottomLeft].Bounds = new Rectangle(left - this.DragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                    this.DragHandles[DragHandleAnchor.BottomCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                    this.DragHandles[DragHandleAnchor.BottomRight].Bounds = new Rectangle(right, bottom, this.DragHandleSize, this.DragHandleSize);
                    this.DragHandles[DragHandleAnchor.Rotation].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                }
            }
        }

        protected virtual void DrawDragHandle(Graphics graphics, DragHandle handle)
        {
            int left;
            int top;
            int width;
            int height;
            Pen outerPen;
            Brush innerBrush;

            left = handle.Bounds.Left;
            top = handle.Bounds.Top;
            width = handle.Bounds.Width;
            height = handle.Bounds.Height;

            if (handle.Enabled)
            {
                outerPen = new Pen(Color.Orange);
                innerBrush = Brushes.Orange;
            }
            else
            {
                outerPen = SystemPens.ControlDark;
                innerBrush = SystemBrushes.Control;
            }

            graphics.FillRectangle(innerBrush, left + 1, top + 1, width - 2, height - 2);
            graphics.DrawLine(outerPen, left + 1, top, left + width - 2, top);
            graphics.DrawLine(outerPen, left, top + 1, left, top + height - 2);
            graphics.DrawLine(outerPen, left + 1, top + height - 1, left + width - 2, top + height - 1);
            graphics.DrawLine(outerPen, left + width - 1, top + 1, left + width - 1, top + height - 2);
        }
        #endregion

        #region 添加元素
        public void AddElement(Element element)
        {
            element.ImageBox = this;
            _elements = _elements ?? new List<Element>();
            _elements.Add(element);
        }

        public void ClearElement(ElementType type)
        {
            if (_elements != null)
            {
                var es = _elements.Where(x => x.Type == type).ToList();
                foreach (var e in es)
                {
                    e.ImageBox = null;
                    _elements.Remove(e);
                }
            }
        }

                #endregion
    }
}
