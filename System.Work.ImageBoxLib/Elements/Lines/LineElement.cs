﻿using System;
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

        public bool ShowArrow { get; set; }

        /// <summary>
        /// 起点
        /// </summary>
        public PointF Pt1 { get; set; }
        /// <summary>
        /// 终点
        /// </summary>
        public PointF Pt2 { get; set; }
        protected float Length
        {
            get
            {
                return (float)Math.Sqrt((Pt1.X - Pt2.X) * (Pt1.X - Pt2.X) + (Pt1.Y - Pt2.Y) * (Pt1.Y - Pt2.Y));
            }
        }

        #region 构造函数

        public LineElement(Point pt1, PointF pt2)
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
            return (float)Math.Sqrt((x1 -x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        protected bool IsEmpty()
        {
            return Length <= 0;
        }
        #endregion

        #region 重写

        public override bool Contains(float x, float y)
        {
            float len1 = GetLength(x, y, Pt1.X, Pt1.Y), len2 = GetLength(x, y, Pt2.X, Pt2.Y);
            return len1 + len2 <= Length * 1.02f;
        }

        public override double AreaValue()
        {
            return Length ;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            using (Pen p = new Pen(ForeColor, BorderWidth * box.ZoomFactor))
            {
                if (ShowArrow)
                {
                    float len = 8 / box.ZoomFactor;
                    len = len > Length * 0.4f ? Length * 0.4f : len;
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
}