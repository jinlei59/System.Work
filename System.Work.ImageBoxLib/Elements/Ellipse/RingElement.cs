using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    [Serializable]
    public class RingElement : Element
    {
        #region 变量

        protected readonly float MinRadiusValue = 1f;

        private PointF _cpt = PointF.Empty;
        private float _sradius = 10;
        private float _eradius = 30;

        #endregion
        #region 属性

        public PointF Cpt
        {
            get
            {
                return _cpt;
            }

            set
            {
                if (!_cpt.Equals(value))
                {
                    _cpt = value;
                    var e = new RingElementEventArgs(_cpt, _sradius, _eradius);
                    OnROIShapeChannged(this, e);
                }
            }
        }

        public float Sradius
        {
            get
            {
                return _sradius;
            }

            set
            {
                if (value <= MinRadiusValue)
                    value = MinRadiusValue;
                if (_sradius != value)
                {
                    _sradius = value;
                    var e = new RingElementEventArgs(_cpt, _sradius, _eradius);
                    OnROIShapeChannged(this, e);
                }
            }
        }

        public float Eradius
        {
            get
            {
                return _eradius;
            }

            set
            {
                if (value <= MinRadiusValue)
                    value = MinRadiusValue;
                if (_eradius != value)
                {
                    _eradius = value;
                    var e = new RingElementEventArgs(_cpt, _sradius, _eradius);
                    OnROIShapeChannged(this, e);
                }
            }
        }

        #endregion
        #region 构造函数
        public RingElement(PointF cpt, float sradius, float eradius)
        {
            Cpt = cpt;
            Type = ElementType.Ring;
            Sradius = sradius;
            Eradius = eradius;
            AutoChangeSize = false;
        }
        #endregion

        #region 自定义

        protected RectangleF GetRect(float radius)
        {
            if (radius <= 0)
                return RectangleF.Empty;
            return new RectangleF(Cpt.X - radius, Cpt.Y - radius, 2 * radius, 2 * radius);
        }

        protected void GetEdgePoint(out PointF pt1, out PointF pt2)
        {
            pt1 = new PointF(Cpt.X + Sradius, Cpt.Y);
            pt2 = new PointF(Cpt.X + Eradius, Cpt.Y);
        }

        #endregion

        #region 重写

        public override bool Contains(float x, float y)
        {
            double dis = GetLength(Cpt.X, Cpt.Y, x, y);
            return dis <= Sradius || dis <= Eradius;
        }

        public override double AreaValue()
        {
            return Sradius > Eradius ? Math.PI * Sradius * Sradius : Math.PI * Eradius * Eradius;
        }

        protected override bool IsEmpty()
        {
            return Sradius <= 0 && Eradius <= 0;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty())
                return;
            using (Pen p = new Pen(ForeColor, AutoChangeSize ? BorderWidth * box.ZoomFactor : BorderWidth))
            {
                //绘制圆
                var rect = GetRect(Sradius);
                var rect1 = box.GetOffsetRectangle(rect);
                g.DrawEllipse(p, rect1);
                rect = GetRect(Eradius);
                rect1 = box.GetOffsetRectangle(rect);
                g.DrawEllipse(p, rect1);
                //绘制箭头
                PointF pt1 = PointF.Empty, pt2 = PointF.Empty;
                GetEdgePoint(out pt1, out pt2);

                float len = 8;
                System.Drawing.Drawing2D.AdjustableArrowCap lineArrow = new System.Drawing.Drawing2D.AdjustableArrowCap(len, len, false);
                p.CustomEndCap = lineArrow;
                p.DashStyle = Drawing.Drawing2D.DashStyle.Dash;
                var pp1 = box.GetOffsetPoint(pt1);
                var pp2 = box.GetOffsetPoint(pt2);

                var cpt = box.GetOffsetPoint(Cpt);

                for (int i = 0; i < 13; i++)
                {
                    g.TranslateTransform(cpt.X, cpt.Y);
                    g.RotateTransform(30);
                    g.TranslateTransform(-cpt.X, -cpt.Y);
                    g.DrawLine(p, pp1, pp2);
                }
                g.ResetTransform();
            }
        }
        #endregion
    }

    public class RingElementEventArgs : ElementEventArgs
    {
        public PointF CenterPt { get; set; }
        public float SRadius { get; set; }
        public float ERadius { get; set; }

        public static new readonly RingElementEventArgs Empty = new RingElementEventArgs();

        public RingElementEventArgs() : this(new PointF(0, 0), 0, 0)
        {

        }

        public RingElementEventArgs(PointF cpt, float sradius, float eradius)
        {
            CenterPt = cpt;
            SRadius = sradius;
            ERadius = eradius;
        }
    }
}
