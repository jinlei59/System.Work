using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class StringElement : Element
    {
        string _str = string.Empty;
        Font _font;
        Brush _brush;
        public StringElement(string s, Font font, Brush brush, float x, float y)
        {
            this.Type = ElementType.String;
            _str = s;
            _font = new Font(font, font.Style);
            _brush = brush;
            Region = new RectangleF(x, y, 100, 100);
        }
        public override void DrawElement(Graphics g, float zoomScale, RectangleF rect)
        {
            if (string.IsNullOrEmpty(_str))
                return;
            using (Font f = new Font(_font.FontFamily, (float)(_font.Size * zoomScale), _font.Style))
            {
                g.DrawString(_str, f, _brush, rect.X, rect.Y);
            }
        }
    }
}
