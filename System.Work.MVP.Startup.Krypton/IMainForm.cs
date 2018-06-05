using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.MVP.Core;

namespace System.Work.MVP.Startup.Krypton
{
    public interface IMainForm : IView<MainFormVM>
    {
        Form NativeForm { get; }
    }
}
