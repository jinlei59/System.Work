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
        private RectangleF _region = RectangleF.Empty;
        private DragHandleCollection dragHandleCollection = null;
        private float _angle = 0f;
        public Guid uid { get; set; }

        public Guid ParentUid { get; set; }
        public RectangleF Region
        {
            get
            {
                return _region;
            }
            set
            {
                if (_region != value)
                {
                    var e = new ElementEventArgs(_region, value, _angle, _angle);
                    OnRegionChanged(e);
                    if (!e.Cancel)
                        _region = value;
                }
            }
        }
        public bool IsRotation { get; set; }
        /// <summary>
        /// 顺时针旋转，0°~360°
        /// </summary>
        public float Angle
        {
            get { return IsRotation ? _angle : 0f; }
            set
            {
                if (IsRotation)
                    if (_angle != value)
                    {
                        var e = new ElementEventArgs(_region, _region, _angle, value);
                        OnRegionChanged(e);
                        if (!e.Cancel)
                        {
                            _angle = value % 360f;
                        }
                    }
                    else
                        _angle = 0f;
            }
        }
        public bool IsDirection { get; set; }

        public ElementType Type { get; set; }
        public bool Enable { get; set; }
        public bool Visible { get; set; }
        public bool Selected { get; set; }
        public Color ForeColor { get; set; }
        public Color RotationForeColor { get; set; }
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

        public PointF RegionCenterPoint
        {
            get
            {
                return new PointF((Region.Left + Region.Right) / 2, (Region.Top + Region.Bottom) / 2);
            }
        }

        public event EventHandler<ElementEventArgs> RegionChanged;
        protected virtual void OnRegionChanged(ElementEventArgs e)
        {
            RegionChanged?.Invoke(this, e);
        }

        public Element()
        {
            uid = Guid.NewGuid();
            ParentUid = Guid.Empty;
            Region = RectangleF.Empty;
            Angle = 0f;
            IsRotation = false;
            ForeColor = Color.Red;
            RotationForeColor = Color.Blue;
            IsDirection = false;
            BorderWidth = 1f;
            Enable = true;
            Visible = true;
            Selected = false;
            DragHandleCollection = new DragHandleCollection();
        }

        public virtual void DrawElement(Graphics g, PointF[] points)
        { }

        public virtual void DrawElement(Graphics g, float zoom, PointF[] points)
        { }

        public virtual void DrawElement(Graphics g, float zoomScale, float x1, float y1, float x2, float y2)
        {
            if (!Enable || !Visible || Region.IsEmpty)
                return;
            using (Pen p = new Pen(ForeColor, BorderWidth * zoomScale))
            {
                g.DrawRectangle(p, x1, y1, x2 - x1, y2 - y1);
            }
        }

        public virtual void DrawElement(Graphics g, float zoomScale, RectangleF rect)
        {
            if (!Enable || !Visible || Region.IsEmpty)
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

        public virtual void DrawDragHandle(Graphics graphics, DragHandle handle)
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
        public virtual 
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

    public class ElementEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public RectangleF OldRegion { get; }
        public RectangleF NewRegion { get; }
        public float OldAngle { get; }
        public float NewAngle { get; }

        public ElementEventArgs() : this(RectangleF.Empty, RectangleF.Empty, 0f, 0f)
        { }
        public ElementEventArgs(RectangleF oldRegion, RectangleF newRegion, float oldAngle, float newAngle)
        {
            Cancel = false;
            OldRegion = oldRegion;
            NewRegion = newRegion;
            OldAngle = oldAngle;
            NewAngle = newAngle;
        }
    }
}
