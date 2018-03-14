using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.MVP.Core
{
    public class ControlBase : UserControl, IView
    {
        public bool ThrowExceptionIfNoPresenterBound { get; private set; }
        private readonly PresenterBinder presenterBinder = new PresenterBinder();
        public ControlBase()
        {
            ThrowExceptionIfNoPresenterBound = true;
            presenterBinder.PerformBinding(this);
        }
    }
}
