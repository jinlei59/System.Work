using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.MVP.Startup.Krypton
{
    public enum MenuFontLevel
    {
        First = 0,
        Second = 1,
        Third = 2
    }
    public class FontFactory
    {
        public static Font GetMenuFont(MenuFontLevel level)
        {
            Font font = null;
            switch (level)
            {
                case MenuFontLevel.First:
                    font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    break;
                case MenuFontLevel.Second:
                    font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    break;
                default:
                    font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    break;
            }
            return font;
        }
    }
}
