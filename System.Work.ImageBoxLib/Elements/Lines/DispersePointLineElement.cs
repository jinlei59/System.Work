using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    /// <summary>
    /// 离散点线
    /// </summary>
    [Serializable]
    public class DispersePointLineElement : LineElement
    {
        List<Element> _rects = null;

        /// <summary>
        /// 需要进行检测的边缘点
        /// 按照从起点到终点顺序依次存储并存储起点和终点
        /// </summary>
        public List<PointF> Pts { get; set; }
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }


        private int _dispersePointCount = 4;
        /// <summary>
        /// 包含两个端点(ex: DispersePointCount=3表示中间一个点两端两个点)
        /// 最小值为2
        /// </summary>
        public int DispersePointCount
        {
            get { return _dispersePointCount; }
            set
            {
                if (value < 2)
                    throw new ArgumentOutOfRangeException("最小值为2");
                if (_dispersePointCount != value)
                {
                    _dispersePointCount = value;
                    GetRects();
                }
            }
        }

        private int _pointLineWidth = 100;
        public int PointLineWidth
        {
            get { return _pointLineWidth; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("最小值为1");
                if (_pointLineWidth != value)
                {
                    _pointLineWidth = value;
                    GetRects();
                }
            }
        }

        private int _pointLineHeight = 100;
        public int PointLineHeight
        {
            get { return _pointLineHeight; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("最小值为1");
                if (_pointLineHeight != value)
                {
                    _pointLineHeight = value;
                    GetRects();
                }
            }
        }

        public DispersePointLineElement(PointF pt1, PointF pt2) : base(pt1, pt2)
        {
            DispersePointCount = 4;
            PointLineWidth = 100;
            PointLineHeight = 100;
            _rects = new List<Element>();
            Pts = new List<PointF>();
            A = B = C = 0;
            GetRects();
        }

        private List<Element> GetRects()
        {
            Pts.Clear();
            _rects.Clear();
            if (Pt1.Equals(Pt2))
            {
                int halfW = PointLineWidth / 2;
                int halfH = PointLineHeight / 2;
                for (int i = 0; i < DispersePointCount; i++)
                {
                    RectangleF rect = new RectangleF(Pt1.X - halfW, Pt1.Y - halfH, PointLineWidth, PointLineHeight);
                    _rects.Add(new RectLineElement(rect) { AutoChangeSize = false, BorderWidth = this.BorderWidth, ForeColor = this.ForeColor });
                }
            }
            else
            {
                A = Pt2.Y - Pt1.Y;
                B = Pt1.X - Pt2.X;
                C = Pt2.X * Pt1.Y - Pt1.X * Pt2.Y;

                float offsetX = (Pt2.X - Pt1.X) / (DispersePointCount - 1);
                float offsetY = (Pt2.Y - Pt1.Y) / (DispersePointCount - 1);

                int halfW = PointLineWidth / 2;
                int halfH = PointLineHeight / 2;

                double aphla = Math.Atan((Pt2.Y - Pt1.Y) / (Pt2.X - Pt1.X));
                int angle = (int)(aphla / Math.PI * 180);
                if ((Pt2.X - Pt1.X) < 0)
                    angle = angle + 180;
                Debug.WriteLine($"aphla={aphla} angle={angle}");

                for (int i = 0; i < DispersePointCount; i++)
                {
                    float x = Pt1.X + i * offsetX;
                    float y = B == 0 ? Pt1.Y + i * offsetY : -(A * x + C) / B;

                    Pts.Add(new PointF(x, y));

                    RectangleF rect = new RectangleF(x - halfW, y - halfH, PointLineWidth, PointLineHeight);
                    _rects.Add(new RectLineElement(rect, angle) { AutoChangeSize = false, BorderWidth = this.BorderWidth,ForeColor= this.ForeColor });
                }
            }
            return _rects;
        }

        public override bool Contains(float x, float y)
        {
            bool br = base.Contains(x, y) || (_rects == null ? false : _rects.Exists(rect => rect.Contains(x, y)));
            return br;
        }

        public override double AreaValue()
        {
            return _rects == null ? 0d : _rects.Sum(rect => rect.AreaValue());
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty())
                return;
            var pt1 = box.GetOffsetPoint(Pt1);
            var pt2 = box.GetOffsetPoint(Pt2);
            #region 绘制相关矩形

            GetRects();
            foreach (var rect in _rects)
                rect.Draw(g, box);

            #endregion
            #region 绘制线段
            using (Pen p = new Pen(ForeColor, BorderWidth) { DashStyle = Drawing.Drawing2D.DashStyle.Dot })
            {
                if (ShowArrow)
                {
                    float len = GetLength(pt1.X, pt1.Y, pt2.X, pt2.Y) * 0.2f;
                    len = len < 2 ? 2 : len > 6 ? 6 : len;
                    System.Drawing.Drawing2D.AdjustableArrowCap lineArrow = new System.Drawing.Drawing2D.AdjustableArrowCap(len, len, true);
                    p.CustomEndCap = lineArrow;
                }
                g.DrawLine(p, pt1, pt2);
            }
            #endregion
        }

        public override void Dispose()
        {
            if (_rects != null)
            {
                _rects.Clear();
                _rects = null;
            }
        }
    }
}
