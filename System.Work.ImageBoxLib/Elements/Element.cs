using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    [Serializable]
    public abstract class Element
    {
        #region 公共属性
        public Guid uid { get; set; }
        public Guid ParentUid { get; set; }
        public ElementType Type { get; set; }
        public bool Enable { get; set; }
        public bool Visible { get; set; }
        public bool Selected { get; set; }
        public Color ForeColor { get; set; }
        public float BorderWidth { get; set; }
        #endregion

        #region 特殊属性

        internal DragHandleCollection DragHandleCollection { get; set; }
        internal int DragHandleSize { get; private set; } = 8;
        #endregion

        public Element()
        {
            Enable = true;
            Visible = true;
            Selected = false;
            ForeColor = Color.Red;
            BorderWidth = 1f;
        }

        public virtual bool Contains(float x, float y)
        {
            return false;
        }
        public virtual double AreaValue()
        {
            return 0;
        }
        public virtual DragHandleAnchor HitTest(Point point)
        {
            return DragHandleAnchor.None;
        }
        internal virtual void Draw(Graphics g, ImageBox box)
        {
            return;
        }

        protected void DrawDragHandles(Graphics g)
        {
            if (DragHandleCollection != null && DragHandleCollection.Count > 0)
                foreach (var handle in DragHandleCollection)
                    DrawDragHandle(g, handle);
        }
        private void DrawDragHandle(Graphics graphics, DragHandle handle)
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

    public enum ElementType
    {
        None = 0,
        Rectangle = 1,
        Circle = 2,
        Ellipse = 3,
        Line = 4,
        Point = 5,
        String = 6,
        Blob = 7,
        DotMatrix = 8
    }
}
