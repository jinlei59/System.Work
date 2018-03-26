using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.Config.MenuConfig
{
    public class MenuSection : ConfigurationSection
    {
        [ConfigurationProperty("Items", IsDefaultCollection = true)]
        public MenuItemElementCollection Items
        {
            get { return (MenuItemElementCollection)base["Items"]; }
            set { base["Items"] = value; }
        }
        #region Test Method

        /// <summary>
        /// 例子
        /// </summary>
        public static void Save()
        {
            MenuSection sectionGroup = new MenuSection();
            sectionGroup.Items.Add(new MenuItemElement()
            {
                Name = "123",
                DisplayName = "系统",
                DisplayType = DisplayType.View,
                SourceType = "ffff"
            });
            var sec = new MenuItemElementCollection()
            {
                new MenuItemElement()
                {
                    Name = "eee",
                    DisplayName = "eee",
                    DisplayType = DisplayType.View,
                    SourceType = "ee"
                }
            };
            sectionGroup.Items.Add(new MenuItemElement()
            {
                Name = "",
                DisplayName = "设置",
                DisplayType = DisplayType.Form,
                SourceType = "",
                Items = sec
            });
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Add("Menu", sectionGroup);
            config.Save();
        }
        /// <summary>
        /// 例子
        /// </summary>
        public static MenuSection Read()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return (MenuSection)config.GetSection("Menu");
        }

        #endregion
    }
}
