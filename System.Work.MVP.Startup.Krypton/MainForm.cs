﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.MVP.Core;
using System.Work.MVP.Shell.Krypton;

namespace System.Work.MVP.Startup.Krypton
{
    [PresenterBinding(typeof(MainFormPresenter))]
    public partial class MainForm : KryViewBase, IMainForm
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