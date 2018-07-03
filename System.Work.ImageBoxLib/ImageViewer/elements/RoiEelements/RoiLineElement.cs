using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class RoiLineElement : Element
    {
        private int _lineWidth = 3;

        public float X1 = 0;
        public float Y1 = 0;
        public float X2 = 0;
        public float Y2 = 0;

        public RoiLineElement(float x1, float y1, float x2, float y2)
        {
            Type = ElementType.Line;

            Region = new RectangleF(0, 0, 1, 1);

            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        private float Length
        {
            get
            {
                return (float)Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));
            }
        }

        private float GetLength(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        private bool IsEmpty()
        {
            return Length <= 0;
        }

        public override void DrawElement(Graphics g, float zoomScale, float x1, float y1, float x2, float y2)
        {
            if (!Enable || !Visible || IsEmpty())
                return;
            using (Pen p = new Pen(ForeColor))
            {
                float len = Length * 0.2f * zoomScale;
                len = len > 5 ? 5 : len;
                System.Drawing.Drawing2D.AdjustableArrowCap lineArrow = new System.Drawing.Drawing2D.AdjustableArrowCap(len, len, true);
                p.CustomEndCap = lineArrow;
                g.DrawLine(p, x1, y1, x2, y2);//直线
                lineArrow.Dispose();
            }
        }

        public override bool Contains(float x, float y)
        {
            float len1 = GetLength(x, y, X1, Y1), len2 = GetLength(x, y, X2, Y2);
            return len1 + len2 <= Length * 1.2f;
        }

        public override double AreaValue()
        {
            return Length * _lineWidth * 2;
        }
    }
}
