using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class LineElement : Element
    {
        protected int LineWidth = 3;
        protected PointF _pt1 = PointF.Empty;
        protected PointF _pt2 = PointF.Empty;

        public bool ShowArrow { get; set; }

        /// <summary>
        /// 起点
        /// </summary>
        public PointF Pt1
        {
            get { return _pt1; }
            set
            {
                if (!_pt1.Equals(value))
                {
                    _pt1 = value;
                    var e = new LineElementEventArgs() { Pt1 = _pt1, Pt2 = _pt2 };
                    OnROIShapeChannged(this, e);
                }
            }
        }
        /// <summary>
        /// 终点
        /// </summary>
        public PointF Pt2
        {
            get { return _pt2; }
            set
            {
                if (!_pt2.Equals(value))
                {
                    _pt2 = value;
                    var e = new LineElementEventArgs() { Pt1 = _pt1, Pt2 = _pt2 };
                    OnROIShapeChannged(this, e);
                }
            }
        }
        protected float Length
        {
            get
            {
                return (float)Math.Sqrt((Pt1.X - Pt2.X) * (Pt1.X - Pt2.X) + (Pt1.Y - Pt2.Y) * (Pt1.Y - Pt2.Y));
            }
        }

        #region 构造函数

        public LineElement(PointF pt1, PointF pt2)
        {
            Type = ElementType.Line;
            ShowArrow = false;
            Pt1 = pt1;
            Pt2 = pt2;
        }

        #endregion

        #region 自定义

        protected float GetLength(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        protected bool IsEmpty()
        {
            return Length <= 0;
        }

        public float GetDistance(float x, float y)
        {
            float dis = 0f;
            if (Pt1.Equals(Pt2))
            {
                dis = GetLength(x, y, Pt1.X, Pt1.Y);
            }
            else
            {
                float a = Pt2.Y - Pt1.Y;
                float b = Pt1.X - Pt2.X;
                float c = Pt2.X * Pt1.Y - Pt1.X * Pt2.Y;
                dis = (float)(Math.Abs(a * x + b * y + c) / Math.Sqrt(a * a + b * b));
            }
            return dis;
        }
        #endregion

        #region 重写

        public override bool Contains(float x, float y)
        {
            float len1 = GetLength(x, y, Pt1.X, Pt1.Y), len2 = GetLength(x, y, Pt2.X, Pt2.Y);
            float dis = GetDistance(x, y);
            return len1 + len2 <= Length * 1.02f && dis < LineWidth;
        }

        public override double AreaValue()
        {
            return Length;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty())
                return;
            using (Pen p = new Pen(ForeColor, AutoChangeSize ? BorderWidth * box.ZoomFactor : BorderWidth))
            {
                if (ShowArrow)
                {
                    float len = 8 * box.ZoomFactor;
                    System.Drawing.Drawing2D.AdjustableArrowCap lineArrow = new System.Drawing.Drawing2D.AdjustableArrowCap(len, len, false);
                    p.CustomEndCap = lineArrow;
                }
                var pt1 = box.GetOffsetPoint(Pt1);
                var pt2 = box.GetOffsetPoint(Pt2);
                g.DrawLine(p, pt1, pt2);
            }
        }

        #endregion
    }

    public class LineElementEventArgs : ElementEventArgs
    {
        public PointF Pt1 { get; set; }
        public PointF Pt2 { get; set; }

        public LineElementEventArgs()
        { }
    }
}
