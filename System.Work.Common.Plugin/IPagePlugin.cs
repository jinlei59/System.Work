using System.Drawing;

namespace System.Work.Common.Plugin
{
    public interface IPagePlugin : IPlugin
    {
        string DisplayName { get; set; }
        Image ImageIcon { get; }
        void PluginClosingCallback();
    }
}
