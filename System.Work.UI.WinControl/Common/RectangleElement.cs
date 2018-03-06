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
        float _x = 0, _y = 0, _width = 0, _height = 0;

        public RectangleElement(Pen pen, float x, float y, float width, float height)
        {
            Type = ElementType.Rectangle;
            _pen = new Pen(pen.Brush, pen.Width);
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }
        public override void Draw(PaintEventArgs e, int zoomScale)
        {
            if (_width == 0 || _height == 0)
                return;
            RectangleF rect = this.ImageBox.GetOffsetRectangle(_x, _y, _width, _height);
            Pen p = new Pen(_pen.Brush, _pen.Width * zoomScale / 100);
            e.Graphics.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
