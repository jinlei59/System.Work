using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.MVP.Core
{
    public class WinformBase : Form, IView
    {
        private System.ComponentModel.IContainer components = null;
        private readonly PresenterBinder presenterBinder = new PresenterBinder();
        public WinformBase()
        {
            InitializeComponent();
            ThrowExceptionIfNoPresenterBound = true;
            presenterBinder.PerformBinding(this);
        }

        public bool ThrowExceptionIfNoPresenterBound
        {
            get;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
            this.Text = "WinformBase";
        }
    }
}
