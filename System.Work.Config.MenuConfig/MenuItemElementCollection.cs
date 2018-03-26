using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.Config.MenuConfig
{
    [ConfigurationCollection(typeof(MenuItemElement), AddItemName = "Item")]
    public class MenuItemElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MenuItemElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MenuItemElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public void Add(ConfigurationElement element)
        {
            base.BaseAdd(element);
        }

        public MenuItemElement this[int index]
        {
            get
            {
                return (MenuItemElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }
    }
}
