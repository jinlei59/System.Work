using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.MVP.Core;

namespace System.Work.MVP.WinModule
{
    [PresenterBinding(typeof(MainFormPresenter))]
    public partial class MainForm : WinformBase, IMainForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public MainFormVM Model
        {
            get; set;
        }
    }
}
