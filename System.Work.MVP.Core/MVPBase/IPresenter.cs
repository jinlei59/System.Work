using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.MVP.Core
{
    public interface IPresenter
    {
    }

    public interface IPresenter<TView> : IPresenter where TView : class, IView
    {
        TView View { get; }
    }
}
