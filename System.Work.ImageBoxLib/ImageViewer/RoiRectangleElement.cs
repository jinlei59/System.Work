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

            //this.DragHandleCollection[DragHandleAnchor.Rotation].Enabled = false;
            //this.DragHandleCollection[DragHandleAnchor.Rotation].Visible = false;
        }

        public override void DrawElement(Graphics g, float zoomScale, RectangleF rect)
        {
            if (!Enable || Region.IsEmpty)
                return;
            using (Pen p = new Pen(ForeColor))
            {
                if (Angle == 0)
                    g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
                else
                {
                    float cx = rect.X + rect.Width / 2, cy = rect.Y + rect.Height / 2;
                    g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
                    g.TranslateTransform(cx, cy);
                    g.RotateTransform(Angle);
                    g.TranslateTransform(-cx, -cy);
                    using (Pen pp = new Pen(Color.Blue))
                        g.DrawRectangle(pp, rect.X, rect.Y, rect.Width, rect.Height);
                    g.ResetTransform();
                }
            }
        }
    }
}
