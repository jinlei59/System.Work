using ComponentFactory.Krypton.Navigator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.Config.MenuConfig;

namespace System.Work.MVP.Startup.Krypton
{
    public class KryDockPage : KryptonPage
    {
        private MenuItemElement _element = null;

        #region 构造函数

        public KryDockPage(MenuItemElement element, Control content)
        {
            _element = element;
            this.Text = Element.DisplayName;
            this.TextTitle = Element.DisplayName;
            this.TextDescription = Element.DisplayName;
            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            this.Controls.Add(content);
        }

        public MenuItemElement Element
        {
            get
            {
                return _element;
            }
        }

        #endregion
    }
}
