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

namespace System.Work.ImageBoxLib
{
    public partial class ImageViewer : UserControl
    {
        #region 变量

        private List<Element> _roiElements = null;
        private List<Element> _otherElements = null;
        private bool _isLeftMouseDown = false;
        private Element _selectRoi = null;

        #endregion

        #region 属性

        public bool ToolStripVisible
        {
            get; set;
        }

        public bool StatusStripVisible
        {
            get; set;
        }

        public Element SelectRoi
        {
            get
            {
                return _selectRoi;
            }

            set
            {
                _selectRoi = value;
                var e = _roiElements.FirstOrDefault(x => x.Selected);
                if (e != null)
                {
                    if (_selectRoi == null || !_selectRoi.uid.Equals(e.uid))
                        SelectedRoiChanaged?.Invoke(this, _selectRoi);
                }
                else
                {
                    if (_selectRoi != null)
                        SelectedRoiChanaged?.Invoke(this, _selectRoi);
                }
            }
        }

        public int DragHandleSize { get; private set; } = 8;


        public virtual int MinimumRoiSize { get; set; } = 1;

        public bool AllowZoom
        {
            get { return imageBox1.AllowZoom; }
            set
            {
                imageBox1.AllowZoom = value;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether painting of the control is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if painting of the control is allowed; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AllowPainting
        {
            get { return imageBox1.AllowPainting; }
        }

        public float ZoomScale
        {
            get
            {
                return imageBox1.ZoomFactor;
            }
        }
        #endregion

        #region 事件

        public event EventHandler<Element> SelectedRoiChanaged;

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

        public void ZoomToFit()
        {
            this.imageBox1.ZoomToFit();
        }
        #endregion

        #region 界面事件
        private void imageBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {//选中ROI
                _isLeftMouseDown = true;
                var imagePt = imageBox1.PointToImage(e.Location);
                if (imageBox1.Cursor == Cursors.Default)
                {
                    SelectRoi = _roiElements.Where(x => x.Visible && x.Enable && x.Contains(imagePt.X, imagePt.Y) || (x.HitTest(e.Location) != null && x.HitTest(e.Location).Anchor != DragHandleAnchor.None)).OrderBy(x => x.AreaValue()).FirstOrDefault();
                    _roiElements.ForEach(x => x.Selected = false);
                }
                if (SelectRoi != null)
                {
                    SelectRoi.Selected = true;
                    SelectRoi.MouseDown(e, imageBox1);
                }
                else
                    imageBox1.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
            }

            this.OnMouseDown(e);
        }

        private void imageBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {//操作ROI
                if (SelectRoi != null)
                {
                    SelectRoi.MouseMove(e, imageBox1);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
            }
            else if (e.Button == MouseButtons.None)
            {
                if (SelectRoi != null)
                {
                    SelectRoi.MouseMove(e, imageBox1);
                }
            }
            this.OnMouseMove(e);
        }

        private void imageBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = false;
                if (SelectRoi != null)
                {
                    SelectRoi.MouseUp(e, imageBox1);
                }
                imageBox1.Cursor = Cursors.Default;
            }
            else if (e.Button == MouseButtons.Right)
            {
            }
            this.OnMouseUp(e);
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
