using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.UI.WinControl
{
    internal class RectangleElement : Element
    {
        Pen _pen;


        public RectangleElement(Pen pen, RectangleF rect)
        {
            Type = ElementType.Rectangle;
            _pen = new Pen(pen.Brush, pen.Width);
            _rect = rect;
        }
        public RectangleElement(Pen pen, float x, float y, float width, float height) : this(pen, new RectangleF(x, y, width, height))
        {
        }

        public override bool Contains(float x, float y)
        {
            return _rect.Contains(x, y);
        }

        public override void Draw(PaintEventArgs e, double zoomScale)
        {
            if (_rect.IsEmpty)
                return;
            RectangleF rect = this.ImageBox.GetOffsetRectangle(_rect);
            Pen p = new Pen(_pen.Brush);
            e.Graphics.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
            base.Draw(e, zoomScale);
        }

        public override string ToString()
        {
            return _rect.ToString();
        }
    }
}
