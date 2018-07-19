using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    [Serializable]
    public class PolygonElement : Element
    {
        #region 变量

        protected Dictionary<Guid, PointF> _polygonPts = new Dictionary<Guid, PointF>();

        #endregion

        #region 属性
        public Dictionary<Guid, PointF> PolygonPts { get { return _polygonPts; } }

        public PointF GravityPt
        {
            get
            {
                return GetGravityPt();
            }
        }

        public RectangleF OuterRect
        {
            get
            {
                if (_polygonPts == null || _polygonPts.Count < 3)
                    return RectangleF.Empty;
                float xmin = float.MaxValue, ymin = float.MaxValue, xmax = float.MinValue, ymax = float.MinValue;
                foreach (var keyVal in _polygonPts)
                {
                    if (keyVal.Value.X < xmin)
                        xmin = keyVal.Value.X;
                    if (keyVal.Value.Y < ymin)
                        ymin = keyVal.Value.Y;

                    if (keyVal.Value.X > xmax)
                        xmax = keyVal.Value.X;
                    if (keyVal.Value.Y > ymax)
                        ymax = keyVal.Value.Y;
                }
                return new RectangleF(xmin, ymin, xmax - xmin, ymax - ymin);
            }
        }

        #endregion

        #region 构造函数

        public PolygonElement() : this(new List<PointF>()
        {
            new PointF(100,100),
            new PointF(300,110),
            new PointF(310,310),
            new PointF(90,300)
        })
        {

        }

        public PolygonElement(List<PointF> pts)
        {
            if (pts == null || pts.Count < 3)
                throw new ArgumentException("多边形点数最少为3");
            foreach (var pt in pts)
                _polygonPts.Add(Guid.NewGuid(), pt);

            Type = ElementType.Polygon;
            AutoChangeSize = false;
        }

        public PolygonElement(Dictionary<Guid, PointF> pts)
        {
            if (pts == null || pts.Count < 3)
                throw new ArgumentException("多边形点数最少为3");
            foreach (var keyVal in pts)
                _polygonPts.Add(keyVal.Key, keyVal.Value);

            Type = ElementType.Polygon;
            AutoChangeSize = false;
        }

        #endregion

        #region 自定义

        protected virtual void OnPolygonShapeChannged()
        {
            var e = new PolygonElementEventArgs() { PolygonPts = _polygonPts };
            OnROIShapeChannged(this, e);
        }

        protected PointF GetPt(int index)
        {
            var key = _polygonPts.Keys.ToList()[index];
            var pt = PointF.Empty;
            _polygonPts.TryGetValue(key, out pt);
            return pt;
        }

        protected PointF GetPt(Guid key)
        {
            var pt = PointF.Empty;
            _polygonPts.TryGetValue(key, out pt);
            return pt;
        }

        protected void SetPt(Guid key, PointF value)
        {
            try
            {
                _polygonPts[key] = value;
            }
            catch (KeyNotFoundException)
            {
                //忽略
            }
        }

        protected float GetTriangleArea(PointF pt0, PointF pt1, PointF pt2)
        {
            return Math.Abs(((pt1.X - pt0.X) * (pt2.Y - pt0.Y) - (pt2.X - pt0.X) * (pt1.Y - pt0.Y)) / 2);
        }

        protected virtual PointF[] GetPolygonPts(ImageBox box)
        {
            int count = _polygonPts.Count;
            var pts = new PointF[count];
            int index = 0;
            foreach (var keyVal in _polygonPts)
            {
                pts[index] = box.GetOffsetPoint(keyVal.Value);
                index++;
            }
            return pts;
        }

        protected PointF GetTriangleGravityPt(PointF pt0, PointF pt1, PointF pt2)
        {
            return new PointF((pt0.X + pt1.X + pt2.X) / 3, (pt0.Y + pt1.Y + pt2.Y) / 3);
        }
        protected PointF GetGravityPt()
        {
            if (_polygonPts == null || _polygonPts.Count < 3)
                return PointF.Empty;
            PointF gravity = PointF.Empty;
            float sumXa = 0f, sumYa = 0f, sumArea = 0f;
            int count = _polygonPts.Count - 1;
            for (int i = 1; i < count; i++)
            {
                var pt = GetTriangleGravityPt(GetPt(0), GetPt(i), GetPt(i + 1));
                var area = GetTriangleArea(GetPt(0), GetPt(i), GetPt(i + 1));
                sumXa += (pt.X * area);
                sumYa += (pt.Y * area);
                sumArea += area;
            }
            gravity = new PointF(sumXa / sumArea, sumYa / sumArea);
            return gravity;
        }
        #endregion

        #region 重写

        public override bool Contains(float x, float y)
        {
            if (_polygonPts == null || _polygonPts.Count < 3)
                return false;
            bool br = OuterRect.Contains(x, y);
            //if (br)
            //{//再次判断是否在多边形里面
            //    int count = _polygonPts.Count;
            //    var pt0 = new PointF(x, y);
            //    float sumArea = 0f;
            //    for (int i = 0; i < count - 1; i++)
            //    {
            //        var pt1 = GetPt(i);
            //        var pt2 = GetPt(i + 1);
            //        sumArea += GetTriangleArea(pt0, pt1, pt2);
            //    }
            //    {
            //        var pt1 = GetPt(0);
            //        var pt2 = GetPt(count - 1);
            //        sumArea += GetTriangleArea(pt0, pt1, pt2);
            //    }
            //    br = sumArea <= AreaValue();
            //}
            return br;
        }

        public override double AreaValue()
        {
            if (_polygonPts == null || _polygonPts.Count < 3)
                return 0d;
            double sum = 0d;
            int count = _polygonPts.Count - 1;
            for (int i = 1; i < count; i++)
                sum += GetTriangleArea(GetPt(0), GetPt(i), GetPt(i + 1));
            return sum;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty())
                return;
            using (Pen p = new Pen(ForeColor, AutoChangeSize ? BorderWidth * box.ZoomFactor : BorderWidth))
            {
                var pts = GetPolygonPts(box);
                g.DrawPolygon(p, pts);
                //var gravity = box.GetOffsetPoint(GetGravityPt());
                //g.DrawRectangle(p, gravity.X, gravity.Y, 2, 2);
                //var are = AreaValue();
                //var rect = box.GetOffsetRectangle( OuterRect);
                //g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_polygonPts != null)
            {
                _polygonPts.Clear();
                _polygonPts = null;
            }
        }
        #endregion
    }

    public class PolygonElementEventArgs : ElementEventArgs
    {
        public Dictionary<Guid, PointF> PolygonPts { get; set; }

        public PolygonElementEventArgs()
        { }
    }
}
