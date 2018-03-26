using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.Config.MenuConfig;
using System.Work.MVP.Core;
using System.Work.MVP.Shell.Krypton;

namespace System.Work.MVP.Startup.Krypton
{
    [PresenterBinding(typeof(MainFormPresenter))]
    public partial class MainForm : KryViewBase, IMainForm
    {
        public PaletteModeManager GlobalPaletteMode
        {
            set
            {
                if (kryptonManager1 != null)
                    kryptonManager1.GlobalPaletteMode = value;
            }
            get
            {
                return kryptonManager1 != null ? kryptonManager1.GlobalPaletteMode : PaletteModeManager.Office2010Blue;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            LoadMenu();
        }

        #region 加载菜单栏

        private void LoadMenu()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.Sections["Menu"] == null)
            {
                config.Sections.Add("Menu", new MenuSection());
                config.Save();
            }
            else
            {
                MenuSection section = (MenuSection)config.GetSection("Menu");
                foreach (MenuItemElement item in section.Items)
                {
                    CreteMenuItem(item, menuStrip1.Items, MenuFontLevel.First);
                }
            }
        }

        private void CreteMenuItem(MenuItemElement element, ToolStripItemCollection collection, MenuFontLevel level)
        {
            ToolStripItem item = null;
            switch (element.DisplayType)
            {
                case DisplayType.None:
                    item = new ToolStripMenuItem();
                    foreach (MenuItemElement temp in element.Items)
                    {
                        CreteMenuItem(temp, (item as ToolStripMenuItem).DropDownItems, (int)level + 1 > 2 ? MenuFontLevel.Third : level + 1);
                    }
                    break;
                case DisplayType.Form:
                case DisplayType.View:
                case DisplayType.Tool:
                case DisplayType.Func:
                    item = new ToolStripMenuItem();
                    item.Click += Item_Click;
                    break;
                case DisplayType.Partition:
                    item = new ToolStripSeparator();
                    break;
                default:
                    break;
            }
            if (item != null)
            {
                item.Tag = element;
                item.Text = element.DisplayName;
                item.Font = FontFactory.GetMenuFont(level);
                collection.Add(item);
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;
            MenuItemElement element = (sender as ToolStripItem).Tag as MenuItemElement;
            switch (element.DisplayType)
            {
                case DisplayType.Form:
                    break;
                case DisplayType.View:
                case DisplayType.Tool:
                case DisplayType.Func:
                    break;
            }
        }

        #endregion

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
