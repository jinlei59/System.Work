using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.UI.WinControl
{
    public abstract class Element
    {
        public ImageBox ImageBox { set; get; }
        public abstract void Draw(PaintEventArgs e, int zoomScale);
    }
}
