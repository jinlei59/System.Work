using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class BlobElement : Element
    {
        private PointF[] _points = null;
        public BlobElement(Color c, PointF[] points)
        {
            Type = ElementType.Blob;
            ForeColor = c;
            _points = points;
        }

        public PointF[] GetPoints()
        {
            return _points;
        }

        public override void DrawElement(Graphics g, PointF[] points)
        {
            if (_points != null)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(75, ForeColor)))
                {
                    //g.FillPolygon(b, points, Drawing.Drawing2D.FillMode.Winding);
                    g.FillClosedCurve(b, points, Drawing.Drawing2D.FillMode.Winding);
                }

                //using (Pen p = new Pen(ForeColor, 1f))
                //{
                //    GraphicsPath path = new GraphicsPath();
                //    foreach (var pt in points)
                //    {
                //        path.AddEllipse(pt.X, pt.Y, 1f, 1f);
                //    }
                //    g.DrawPath(p, path);
                //    path.Dispose();
                //}
            }
        }
    }
}
