using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class PointElement : Element
    {
        #region 变量
        private PointF _pt = PointF.Empty;

        private float _zoomScale = 1f;

        #endregion

        #region 属性
        public PointF Pt
        {
            get
            {
                return _pt;
            }

            set
            {
                if (!_pt.Equals(value))
                {
                    _pt = value;
                    var e = new PointElementEventArgs(_pt);
                    OnROIShapeChannged(this, e);
                }
            }
        }

        protected RectangleF ImageBounds { get; set; }
        #endregion

        #region 构造函数
        public PointElement() : this(PointF.Empty)
        { }

        public PointElement(float x, float y) : this(new PointF(x, y))
        { }

        public PointElement(PointF pt)
        {
            Pt = pt;

            Type = ElementType.Point;
            AutoChangeSize = false;
        }
        #endregion

        #region 自定义方法


        #endregion

        #region 重写

        public override bool Contains(float x, float y)
        {
            var dis = GetLength(x, y, _pt.X, _pt.Y);
            var len = 2 / _zoomScale;
            len = len < 2 ? 2 : len;
            return dis <= len;
        }

        public override double AreaValue()
        {
            return 1;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            _zoomScale = box.ZoomFactor;
            if (!Visible || this.IsEmpty())
                return;
            using (Pen p = new Pen(ForeColor, AutoChangeSize ? BorderWidth * box.ZoomFactor : BorderWidth))
            {
                //if (box.ZoomFactor < 1)
                //{
                int len = 8;
                int halfLen = len / 2;

                var pt = box.GetOffsetPoint((int)Pt.X, (int)Pt.Y);

                var pt1 = new Point(pt.X - len, pt.Y);
                var pt2 = new Point(pt.X + len, pt.Y);
                g.DrawLine(p, pt1, pt2);

                pt1 = new Point(pt.X, pt.Y - len);
                pt2 = new Point(pt.X, pt.Y + len);
                g.DrawLine(p, pt1, pt2);

                var rect = new Rectangle((int)(pt.X - halfLen), (int)(pt.Y - halfLen), len, len);

                g.DrawRectangle(p, rect);
                //}
                //else
                //{
                //    var pt1 = box.GetOffsetPoint(new PointF(Pt.X - 2, Pt.Y));
                //    var pt2 = box.GetOffsetPoint(new PointF(Pt.X + 2, Pt.Y));
                //    g.DrawLine(p, pt1, pt2);

                //    pt1 = box.GetOffsetPoint(new PointF(Pt.X, Pt.Y - 2));
                //    pt2 = box.GetOffsetPoint(new PointF(Pt.X, Pt.Y + 2));
                //    g.DrawLine(p, pt1, pt2);

                //    var rect = box.GetOffsetRectangle(new RectangleF(Pt.X - 1f, Pt.Y - 1f, 2, 2));
                //    g.DrawEllipse(p, rect);
                //}
            }
        }
        #endregion
    }

    public class PointElementEventArgs : ElementEventArgs
    {
        protected PointF Pt { get; set; }

        public static new readonly PointElementEventArgs Empty = new PointElementEventArgs();

        public PointElementEventArgs() : this(PointF.Empty)
        { }

        public PointElementEventArgs(PointF pt) : base()
        {
            Pt = pt;
        }
    }
}
