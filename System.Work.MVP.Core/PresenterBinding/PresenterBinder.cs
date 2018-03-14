using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.MVP.Core
{
    public sealed class PresenterBinder
    {
        private static IPresenterFactory factory;

        ///<summary>
        /// Gets or sets the factory that the binder will use to create
        /// new presenter instances. This is pre-initialized to a
        /// default implementation but can be overriden if desired.
        /// This property can only be set once.
        ///</summary>
        ///<exception cref="ArgumentNullException">Thrown if a null value is passed to the setter.</exception>
        ///<exception cref="InvalidOperationException">Thrown if the property is being set for a second time.</exception>
        public static IPresenterFactory Factory
        {
            get { return factory ?? (factory = new DefaultPresenterFactory()); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (factory != null)
                {
                    throw new InvalidOperationException(
                        factory is DefaultPresenterFactory
                            ? "The factory has already been set, and can be not changed at a later time. In this case, it has been set to the default implementation. This happens if the factory is used before being explicitly set. If you wanted to supply your own factory, you need to do this in your Application_Start event."
                            : "You can only set your factory once, and should really do this in Application_Start.");
                }
                factory = value;
            }
        }

        public PresenterBinder()
        { }

        public IPresenter PerformBinding(IView viewInstance)
        {
            IPresenter presenter = PerformBinding(viewInstance, Factory);
            return presenter;
        }

        private static IPresenter PerformBinding(IView viewInstance, IPresenterFactory presenterFactory)
        {
            IPresenter presenter = null;
            //获取该视图的类类型
            Type t = viewInstance.GetType();
            //获取该类上的附加特性集合
            object[] attrs = t.GetCustomAttributes(typeof(PresenterBindingAttribute), false);
            //遍历特性集合，找到Presenter类型附加的特性，通过该特性建立实例
            foreach (PresenterBindingAttribute pba in attrs)
            {
                //获取Presenter类类型
                Type newt = pba.PresenterType;
                presenter = presenterFactory.Create(newt, t, viewInstance);
            }
            return presenter;
        }
    }
}
