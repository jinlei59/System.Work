using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.MVP.Core
{
    public interface IView
    {
        event EventHandler Load;
        object Invoke(Delegate method);
        object Invoke(Delegate method, params object[] args);
        IAsyncResult BeginInvoke(Delegate method);
        IAsyncResult BeginInvoke(Delegate method, params object[] args);

        bool ThrowExceptionIfNoPresenterBound
        {
            get;
        }
    }

    public interface IView<TModel> : IView
    {
        TModel Model { get; set; }
    }
}
