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
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }

        [ConfigurationProperty("DisplayName", IsRequired = true)]
        public string DisplayName
        {
            get { return (string)base["DisplayName"]; }
            set { base["DisplayName"] = value; }
        }

        [ConfigurationProperty("DisplayType", IsRequired = true)]
        public DisplayType DisplayType
        {
            get { return (DisplayType)base["DisplayType"]; }
            set { base["DisplayType"] = value; }
        }

        [ConfigurationProperty("SourceType", IsRequired = true)]
        public string SourceType
        {
            get { return (string)base["SourceType"]; }
            set { base["SourceType"] = value; }
        }

        [ConfigurationProperty("Items", IsDefaultCollection = true)]
        public MenuItemElementCollection SecondMenuItems
        {
            get { return (MenuItemElementCollection)base["Items"]; }
            set { base["Items"] = value; }
        }
    }

    public enum DisplayType
    {
        Form = 0,
        View = 1,
        Tool = 2
    }
}
