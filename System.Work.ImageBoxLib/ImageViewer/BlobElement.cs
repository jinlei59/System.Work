using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class BlobElement : Element
    {
        private Color _color = Color.Black;
        private PointF[] _points = null;
        public BlobElement(Color b, PointF[] points)
        {
            Type = ElementType.Blob;
            _color = b;
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
                using (Brush b = new SolidBrush(Color.FromArgb(75, _color)))
                {
                    g.FillPolygon(b, points);
                }
            }
        }
    }
}
