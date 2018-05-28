using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class DotMatrixElement : Element
    {
        private PointF[] _points = null;
        private float _size = 1f;
        public DotMatrixElement(Color c, PointF[] points, float size = 1f)
        {
            Type = ElementType.DotMatrix;
            ForeColor = c;
            _points = points;
            _size = size;
        }

        public PointF[] GetPoints()
        {
            return _points;
        }

        public override void DrawElement(Graphics g, float zoom, PointF[] points)
        {
            if (_points != null)
            {
                using (Pen p = new Pen(ForeColor, 1f))
                {
                    GraphicsPath path = new GraphicsPath();
                    foreach (var pt in points)
                    {
                        path.AddEllipse(pt.X, pt.Y, 1f, 1f);
                    }
                    g.DrawPath(p, path);
                    path.Dispose();
                }
            }
        }
    }
}
