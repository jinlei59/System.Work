using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace System.Work.ImageBoxLib.ImageViewerCore
{
    public partial class ImageViewer : UserControl
    {
        #region 变量

        private List<Element> _roiElements = null;
        private List<Element> _otherElements = null;

        #endregion

        #region 属性

        #endregion

        #region 事件

        #endregion

        #region 构造函数
        public ImageViewer()
        {
            InitializeComponent();
            _roiElements = new List<Element>();
            _otherElements = new List<Element>();
        }
        #endregion

        #region 自定义方法

        public void NewBeginDisplay()
        {
            imageBox1.BeginUpdate();
        }

        public void NewDisplayImage(Bitmap bmp)
        {
            imageBox1.Image = bmp;
        }

        public void NewAddRoiElements(List<Element> rois)
        {
            _roiElements.AddRange(rois);
        }

        public void NewClearRoiElements()
        {
            _roiElements.Clear();
        }

        public void NewAddOtherElements(List<Element> others)
        {
            _otherElements.AddRange(others);
        }
        public void NewClearOtherElements()
        {
            _otherElements.Clear();
        }

        public void NewEndDisplay()
        {
            imageBox1.EndUpdate();
        }

        public void SetRoiSelected(Element e)
        {
            _roiElements.ForEach(x => x.Selected = false);
            if (e != null)
            {
                _roiElements.ForEach(x => x.Selected = false);
                e.Selected = true;
            }
        }

        public void SetRoiSelected(Guid uid)
        {
            var element = _roiElements.FirstOrDefault(x => x.ParentUid.Equals(uid));
            SetRoiSelected(element);
        }

        #endregion

        #region 界面事件
        private void imageBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void imageBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void imageBox1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void imageBox1_MouseWheel(object sender, MouseEventArgs e)
        {

        }

        private void imageBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!imageBox1.AllowPainting)
                    return;

                foreach (var element in _otherElements)
                    element.Draw(e.Graphics, imageBox1);
                foreach (var element in _roiElements)
                    element.Draw(e.Graphics, imageBox1);

                this.OnPaint(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
