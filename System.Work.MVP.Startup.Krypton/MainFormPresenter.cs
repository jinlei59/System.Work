using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.MVP.Core;

namespace System.Work.MVP.Startup.Krypton
{
    public class MainFormPresenter : Presenter<IMainForm>
    {
        IMainService _service=null;
        public MainFormPresenter(IMainForm view,IMainService service) : base(view)
        {
            _service = service;
            View.Load += View_Load;
        }

        private void View_Load(object sender, EventArgs e)
        {
            View.Model = new MainFormVM();
        }
    }
}
