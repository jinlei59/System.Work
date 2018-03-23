using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.Config.MenuConfig
{
    public class MenuItemElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get;
            set;
        }

        [ConfigurationProperty("DisplayName", IsRequired = true)]
        public string DisplayName
        {
            get;
            set;
        }

        [ConfigurationProperty("DisplayType", IsRequired = true)]
        public DisplayType DisplayType
        {
            get;
            set;
        }

        [ConfigurationProperty("SourceType", IsRequired = true)]
        public string SourceType
        {
            get;
            set;
        }
    }

    public enum DisplayType
    {
        Form = 0,
        View = 1,
        Tool = 2
    }
}
