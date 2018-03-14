using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.MVP.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PresenterBindingAttribute : Attribute
    {
        public Type PresenterType
        {
            get;
            private set;
        }

        public Type ViewType
        {
            get;
            set;
        }

        public PresenterBindingAttribute(Type presenterType)
        {
            this.PresenterType = presenterType;
            this.ViewType = null;
        }
    }
}
