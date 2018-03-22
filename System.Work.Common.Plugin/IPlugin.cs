using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.Common.Plugin
{
    public interface IPlugin
    {
        string PluginName { get; set; }
        /// <summary>
        /// 预留作为经后的插件内容扩展
        /// </summary>
        IPluginContext Context { get; set; }
    }
}
