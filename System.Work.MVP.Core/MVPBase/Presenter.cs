using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.MVP.Core
{
    public class Presenter<TView> : IPresenter<TView>, IPresenter where TView : class, IView
    {
        private readonly TView view;

        public TView View
        {
            get
            {
                return this.view;
            }
        }

        public Presenter(TView view)
        {
            this.view = view;
        }
    }
}
