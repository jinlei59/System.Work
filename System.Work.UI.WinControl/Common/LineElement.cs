using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.UI.WinControl
{
    internal class LineElement : Element
    {
        Pen _pen;
        float _x1 = 0, _y1 = 0, _x2 = 0, _y2 = 0;

        public LineElement(Pen pen, float x1, float y1, float x2, float y2)
        {
            Type = ElementType.Line;
            _pen = new Pen(pen.Brush, pen.Width);
            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
        }

        public override void Draw(PaintEventArgs e, int zoomScale)
        {
            PointF p1 = this.ImageBox.GetOffsetPoint(_x1, _y1);
            PointF p2 = this.ImageBox.GetOffsetPoint(_x2, _y2);
            Pen p = new Pen(_pen.Brush, _pen.Width * zoomScale / 100);
            e.Graphics.DrawLine(p, p1, p2);
        }
    }
}
