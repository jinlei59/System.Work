using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpDXApp
{
    public partial class SDXRenderBox : Form
    {
        #region 变量

        #endregion
        #region 属性

        #endregion
        #region 事件

        #endregion
        #region 构造函数
        public SDXRenderBox()
        {
            this.TopLevel = false;
            this.Cursor = Cursors.Cross;
            this.DoubleBuffered = true;
            InitializeComponent();
        }
        #endregion
        #region 自定义方法
        #region 公有方法

        public void ConvertToForm()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        #endregion
        #region 私有方法

        #endregion
        #endregion
    }
}
