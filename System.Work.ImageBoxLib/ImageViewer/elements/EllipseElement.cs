using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class EllipseElement : Element
    {
        Pen _pen;
        public EllipseElement(Pen pen, float x, float y, float width, float height)
        {
            Type = ElementType.Ellipse;
            _pen = new Pen(pen.Brush, pen.Width);
            Region = new RectangleF(x, y, width, height);
        }

        public override void DrawElement(Graphics g, float zoomScale, RectangleF rect)
        {
            using (Pen p = new Pen(_pen.Brush, (float)(_pen.Width * zoomScale)))
            {
                g.DrawEllipse(p, rect);
            }
        }
    }
}
