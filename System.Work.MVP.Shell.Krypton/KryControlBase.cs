using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.MVP.Core;

namespace System.Work.MVP.Shell.Krypton
{
    public class KryControlBase : UserControl, IView
    {
        public bool ThrowExceptionIfNoPresenterBound { get; private set; }
        private readonly PresenterBinder presenterBinder = new PresenterBinder();
        public KryControlBase()
        {
            if (DesignMode)
                return;
            try
            {
                ThrowExceptionIfNoPresenterBound = true;
                presenterBinder.PerformBinding(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
