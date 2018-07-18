using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    [Serializable]
    public class BlobElement : Element
    {
        private PointF[] _points = null;
        public BlobElement(PointF[] points)
        {
            Type = ElementType.Blob;
            _points = points;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || _points == null || _points.Length < 1)
                return;
            using (Brush b = new SolidBrush(Color.FromArgb(75, ForeColor)))
            {
                int len = _points.Length;
                var points = new PointF[len];
                for (int i = 0; i < len; i++)
                    points[i] = box.GetOffsetPoint(_points[i]);
                g.FillClosedCurve(b, points, Drawing.Drawing2D.FillMode.Alternate);
            }
        }

        public override void Dispose()
        {
            _points = null;
        }
    }
}
