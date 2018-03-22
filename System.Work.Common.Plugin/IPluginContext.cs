using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.Common.Plugin
{
    public interface IPluginContext : INotifyPropertyChanged
    {
        void SetProperty<T>(ref T prop, T value, string callerName = null);
        void SetProperty<T>(T value, string callerName = null);
    }
}
