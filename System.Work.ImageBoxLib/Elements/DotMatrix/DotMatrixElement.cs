using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    [Serializable]
    public class DotMatrixElement : Element
    {
        private PointF[] _points = null;
        public DotMatrixElement(PointF[] points)
        {
            Type = ElementType.DotMatrix;
            _points = points;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || _points == null || _points.Length < 1)
                return;
            using (Pen p = new Pen(ForeColor, BorderWidth))
            {
                GraphicsPath path = new GraphicsPath();
                int len = _points.Length;
                for (int i = 0; i < len; i++)
                {
                    var pt = box.GetOffsetPoint(_points[i]);
                    path.AddEllipse(pt.X, pt.Y, 1f, 1f);
                }
                g.DrawPath(p, path);
                path.Dispose();
            }
        }

        public override void Dispose()
        {
            _points = null;
        }
    }
}
