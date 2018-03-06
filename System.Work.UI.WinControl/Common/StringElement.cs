using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.UI.WinControl
{
    internal class StringElement : Element
    {
        string _str = string.Empty;
        Font _font;
        Brush _brush;
        float _x = 0f, _y = 0f;
        public StringElement(string s, Font font, Brush brush, float x, float y)
        {
            this.Type = ElementType.String;
            _str = s;
            _font = new Font(font, font.Style);
            _brush = brush;
            _x = x;
            _y = y;
        }

        public override void Draw(PaintEventArgs e, int zoomScale)
        {
            if (string.IsNullOrEmpty(_str))
                return;
            Font f = new Font(_font.FontFamily, (float)(_font.Size * zoomScale / 100), _font.Style);
            PointF pf = ImageBox.GetOffsetPoint(_x, _y);
            e.Graphics.DrawString(_str, f, _brush, pf.X, pf.Y);
        }
    }
}
