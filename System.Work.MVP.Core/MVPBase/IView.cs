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
