using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.UI.WinControl
{
    public class StringElement : Element
    {
        private string _str = string.Empty;
        Font _font = new Font(FontFamily.Families.First(), 8);
        Brush _brush = Brushes.Black;
        float _x = 0f, _y = 0f;
        public StringElement(string s, Font font, Brush brush, float x, float y)
        {
            _str = s;
            _font = font;
            _brush = brush;
            _x = x;
            _y = y;
        }

        public override void Draw(PaintEventArgs e, int zoomScale)
        {
            if (string.IsNullOrEmpty(_str))
                return;
            Font f = new Font(_font.FontFamily, (float)(_font.Size * zoomScale), _font.Style);
            e.Graphics.DrawString(_str, f, _brush, _x, _y);
        }
    }
}
