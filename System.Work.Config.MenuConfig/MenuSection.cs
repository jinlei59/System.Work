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
        #region 变量

        private const string SectionName = "MenuItems";

        private static MenuSection _instance = null;
        public static MenuSection Instance
        {
            get
            {
                if (_instance == null)
                {
                    var config = GetConfiguration();
                    if (config.Sections[SectionName] == null)
                    {
                        _instance = new MenuSection();
                        config.Sections.Add(SectionName, Instance);
                        config.Save(ConfigurationSaveMode.Modified);
                    }
                    else
                        _instance = (MenuSection)config.Sections[SectionName];
                }
                return _instance;
            }
        }

        #endregion

        #region 属性

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public MenuItemElementCollection MenuItems
        {
            get { return (MenuItemElementCollection)base[""]; }
            set { base[""] = value; }
        }
        
        #endregion

        #region 构造函数

        public MenuSection()
        { }
        #endregion

        #region 自定义方法

        private static System.Configuration.Configuration GetConfiguration()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        #endregion
    }
}
