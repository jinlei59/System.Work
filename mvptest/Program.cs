﻿using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.MVP.Core;
using System.Work.MVP.Startup.Krypton;
using WinFormsMvp.Unity;

namespace mvptest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region 通用服务注入
           var  _currentContainer = new UnityContainer();

            _currentContainer.RegisterInstance<IMainFormService>(new MainFormService());

            #endregion
            PresenterBinder.Factory = new UnityPresenterFactory(_currentContainer);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            Application.Run(new MainForm() { GlobalPaletteMode =  ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Black});
        }
    }
}
