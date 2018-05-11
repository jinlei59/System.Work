using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class RectangleElement : Element
    {
        Pen _pen;
        public RectangleElement(Pen pen, float x, float y, float width, float height)
        {
            Type = ElementType.Rectangle;
            _pen = new Pen(pen.Brush, pen.Width);
            Region = new RectangleF(x, y, width, height);
        }
    }
}
