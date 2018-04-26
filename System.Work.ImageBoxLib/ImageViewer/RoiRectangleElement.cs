using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class RoiRectangleElement : Element
    {
        public RoiRectangleElement()
        {
            Type = ElementType.Rectangle;
        }

        public override void DrawElement(Graphics g, float zoomScale, RectangleF rect)
        {
            if (!Enable || Region.IsEmpty)
                return;
            using (Pen p = new Pen(ForeColor))
            {
                g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }
    }
}
