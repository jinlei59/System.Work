using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Work.MVP.Core;

namespace System.Work.MVP.Shell.Krypton
{
    public class KryViewBase : KryptonForm, IView
    {
        private IContainer components = null;
        private readonly PresenterBinder presenterBinder = new PresenterBinder();
        public KryViewBase()
        {
            InitializeComponent();
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
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "KryformBase";
        }
    }
}
