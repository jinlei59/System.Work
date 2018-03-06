using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.UI.WinControl
{
    public enum ElementType
    {
        None = 0,
        Rectangle = 1,
        String = 2,
        Line = 3,
        Ellipse = 4,
        RoiRectangle = 5
    }
    public abstract class Element
    {
        public ElementType Type { get; set; }
        public ImageBox ImageBox { set; get; }
        public abstract void Draw(PaintEventArgs e, int zoomScale);
    }
}
