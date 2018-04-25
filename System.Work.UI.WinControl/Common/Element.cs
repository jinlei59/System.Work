using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.UI.WinControl
{
    public enum ElementType
    {
        None = 0,
        Rectangle = 1,
        String = 2,
        Line = 3,
        Ellipse = 4,
        RoiRectangle = 5
    }
    public abstract class Element
    {
        protected readonly DragHandleCollection _dragHandles;
        private int _dragHandleSize = 8;
        protected RectangleF _rect = RectangleF.Empty;

        public Element()
        {
            _dragHandles = new DragHandleCollection();
            _dragHandleSize = 8;
            IsSelected = false;
        }

        public bool IsSelected { get; set; }
        public ElementType Type { get; set; }
        public ImageBox ImageBox { set; get; }

        protected int DragHandleSize
        {
            get
            {
                return _dragHandleSize;
            }
        }

        public virtual void Draw(PaintEventArgs e, double zoomScale)
        {
            if (IsSelected)
            {
                PositionDragHandles();
                foreach (var anchor in _dragHandles)
                    this.DrawDragHandle(e.Graphics, anchor);
            }
        }
        public abstract bool Contains(float x, float y);

        public virtual double AreaValue()
        {
            return _rect.Width * _rect.Height;
        }

        protected virtual void PositionDragHandles()
        {
            if (this._dragHandles != null && this.DragHandleSize > 0 && IsSelected && !_rect.IsEmpty)
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

                viewport = this.ImageBox.GetImageViewPort();
                offsetX = viewport.Left + this.ImageBox.Padding.Left + this.ImageBox.AutoScrollPosition.X;
                offsetY = viewport.Top + this.ImageBox.Padding.Top + this.ImageBox.AutoScrollPosition.Y;
                halfDragHandleSize = DragHandleSize / 2;
                left = Convert.ToInt32((this._rect.Left * this.ImageBox.ZoomFactor) + offsetX);
                top = Convert.ToInt32((this._rect.Top * this.ImageBox.ZoomFactor) + offsetY);
                right = left + Convert.ToInt32(this._rect.Width * this.ImageBox.ZoomFactor);
                bottom = top + Convert.ToInt32(this._rect.Height * this.ImageBox.ZoomFactor);
                halfWidth = Convert.ToInt32(this._rect.Width * this.ImageBox.ZoomFactor) / 2;
                halfHeight = Convert.ToInt32(this._rect.Height * this.ImageBox.ZoomFactor) / 2;

                this._dragHandles[DragHandleAnchor.TopLeft].Bounds = new Rectangle(left - this.DragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                this._dragHandles[DragHandleAnchor.TopCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                this._dragHandles[DragHandleAnchor.TopRight].Bounds = new Rectangle(right, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                this._dragHandles[DragHandleAnchor.MiddleLeft].Bounds = new Rectangle(left - this.DragHandleSize, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                this._dragHandles[DragHandleAnchor.MiddleRight].Bounds = new Rectangle(right, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                this._dragHandles[DragHandleAnchor.BottomLeft].Bounds = new Rectangle(left - this.DragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                this._dragHandles[DragHandleAnchor.BottomCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                this._dragHandles[DragHandleAnchor.BottomRight].Bounds = new Rectangle(right, bottom, this.DragHandleSize, this.DragHandleSize);
                this._dragHandles[DragHandleAnchor.Rotation].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
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
                innerBrush = Brushes.OrangeRed;
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
    }
}
