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
        private DragHandleCollection dragHandleCollection = null;
        private float _angle = 0f;
        public Guid uid { get; set; }
        public RectangleF Region { get; set; }

        /// <summary>
        /// 顺时针旋转，0°~360°
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set
            {
                if (value > 360f || value < 0f)
                    throw new Exception("The Angle is between 0° and 360°.");
                if (_angle != value)
                    _angle = value;
            }
        }
        public ElementType Type { get; set; }
        public bool Enable { get; set; }
        public bool Selected { get; set; }
        public Color ForeColor { get; set; }
        public float BorderWidth { get; set; }

        public DragHandleCollection DragHandleCollection
        {
            get
            {
                return dragHandleCollection;
            }

            set
            {
                dragHandleCollection = value;
            }
        }
        public Element()
        {
            uid = Guid.NewGuid();
            Region = RectangleF.Empty;
            Angle = 0f;
            ForeColor = Color.Red;
            BorderWidth = 1f;
            Enable = true;
            Selected = false;
            DragHandleCollection = new DragHandleCollection();
        }

        public virtual void DrawElement(Graphics g, float zoomScale, RectangleF rect)
        {
            if (!Enable || Region.IsEmpty)
                return;
            using (Pen p = new Pen(ForeColor, BorderWidth * zoomScale))
            {
                g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);                
            }
        }

        public virtual bool Contains(float x, float y)
        {
            return Region.Contains(x, y);
        }
        public virtual double AreaValue()
        {
            return Region.Width * Region.Height;
        }

        public DragHandleAnchor HitTest(Point point)
        {
            return DragHandleCollection.HitTest(point);
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
        String = 6
    }
}
