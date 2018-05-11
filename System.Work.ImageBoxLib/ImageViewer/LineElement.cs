using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class LineElement : Element
    {
        Pen _pen;
        public LineElement(Pen pen, float x1, float y1, float x2, float y2)
        {
            Type = ElementType.Line;
            _pen = new Pen(pen.Brush, pen.Width);
            Region = new RectangleF(x1, y1, x2, y2);
        }

        public override void DrawElement(Graphics g, float zoomScale, float x1, float y1, float x2, float y2)
        {
            using (Pen p = new Pen(_pen.Brush, (float)(_pen.Width * zoomScale)))
            {
                g.DrawLine(p, x1, y1, x2, y2);
            }
        }
    }
}
