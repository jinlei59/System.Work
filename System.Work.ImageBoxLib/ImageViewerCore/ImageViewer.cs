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
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace System.Work.ImageBoxLib
{
    public partial class ImageViewer : UserControl
    {
        #region 变量

        private List<Element> _roiElements = null;
        private List<Element> _otherElements = null;
        private List<ImageElement> _imageElements = null;
        private List<RoiImageElement> _roiImageElements = null;

        private bool _isLeftMouseDown = false;
        private Element _selectRoi = null;

        private Bitmap _displayImage = null;
        #endregion

        #region 属性

        public bool ToolStripVisible
        {
            get { return toolStrip1.Visible; }
            set { toolStrip1.Visible = value; }
        }

        public bool StatusStripVisible
        {
            get { return statusStrip1.Visible; }
            set { statusStrip1.Visible = value; }
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
            _imageElements = new List<ImageElement>();
            _roiImageElements = new List<RoiImageElement>();
        }
        #endregion

        #region 自定义方法

        public void NewBeginDisplay()
        {
            imageBox1.BeginUpdate();
        }

        public void NewDisplayImage(Bitmap bmp)
        {
            DisplayImage(bmp);
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

        public void NewAddImageElements(List<ImageElement> images)
        {
            if (images != null)
                _imageElements.AddRange(images);
        }
        public void NewClearImageElements(bool disposed = false)
        {
            if (_imageElements != null && _imageElements.Count > 0)
            {
                if (!disposed)
                    _imageElements.Clear();
                else
                {
                    foreach (var image in _imageElements)
                        image.Dispose();
                    _imageElements.Clear();
                }
            }
        }

        public void NewAddRoiImageElements(List<RoiImageElement> images)
        {
            if (images != null)
                _roiImageElements.AddRange(images);
        }
        public void NewClearRoiImageElements(bool disposed = false)
        {
            if (_roiImageElements != null && _roiImageElements.Count > 0)
            {
                if (!disposed)
                    _roiImageElements.Clear();
                else
                {
                    foreach (var image in _roiImageElements)
                        image.Dispose();
                    _roiImageElements.Clear();
                }
            }
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

        #region 私有方法

        private void DisposeDisplayImage()
        {
            if (_displayImage != null)
            {
                _displayImage.Dispose();
                _displayImage = null;
            }
        }

        private void DisplayImage(Bitmap bmp)
        {
            var temp = _displayImage;
            _displayImage = (Bitmap)bmp.Clone();
            imageBox1.Image = _displayImage;
            if (temp != null)
                temp.Dispose();
            SetWHText();
        }

        private void SetWHText()
        {
            if (_displayImage == null)
                tssWH.Text = $"W:0 H:0";
            else
                tssWH.Text = $"W:{_displayImage.Width} H:{_displayImage.Height}";
        }

        private void SetXYText(Point pt)
        {
            var ptr = imageBox1.PointToImage(pt);
            tssXY.Text = $"X:{ptr.X} Y:{ptr.Y}";
        }

        #region 获取RGB

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        public void SetRGBText(Point location)
        {
            if (imageBox1.IsPointInImage(location))
            {
                int x = location.X, y = location.Y;
                Color c = GetPointColor(x, y);
                tssRGB.Text = string.Format("R:{0} G:{1} B:{2}", c.R, c.G, c.B);
            }
            else
            {
                tssRGB.Text = string.Format("R:{0} G:{1} B:{2}", 0, 0, 0);
            }
        }

        private Color GetPointColor(int x, int y)
        {
            try
            {
                IntPtr dC = GetDC(imageBox1.Handle);
                uint pixel = GetPixel(dC, x, y);
                ReleaseDC(IntPtr.Zero, dC);
                return Color.FromArgb((int)(pixel & 255u), (int)(pixel & 65280u) >> 8, (int)(pixel & 16711680u) >> 16);
            }
            catch (Exception ex)
            {
                return Color.Black;
            }
        }
        #endregion
        #endregion

        #region 绘图控件事件
        private void imageBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {//选中ROI
                _isLeftMouseDown = true;
                var imagePt = imageBox1.PointToImage(e.Location);
                if (imageBox1.Cursor == Cursors.Default)
                {
                    var roirect = _roiElements.Where(x => x.Visible && x.Enable && x.Contains(imagePt.X, imagePt.Y) || (x.HitTest(e.Location) != null && x.HitTest(e.Location).Anchor != DragHandleAnchor.None)).OrderBy(x => x.AreaValue()).FirstOrDefault();
                    var imagerect = _roiImageElements.Where(x => x.Visible && x.Enable && x.Contains(imagePt.X, imagePt.Y) || (x.HitTest(e.Location) != null && x.HitTest(e.Location).Anchor != DragHandleAnchor.None)).OrderBy(x => x.AreaValue()).FirstOrDefault();
                    if (roirect == null)
                        SelectRoi = imagerect;
                    else if (imagerect == null)
                        SelectRoi = roirect;
                    else
                        SelectRoi = roirect.AreaValue() <= imagerect.AreaValue() ? roirect : imagerect;
                    _roiElements.ForEach(x => x.Selected = false);
                    _roiImageElements.ForEach(x => x.Selected = false);
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
            SetXYText(e.Location);
            SetRGBText(e.Location);
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

                foreach (var element in _imageElements)
                    element.Draw(e.Graphics, imageBox1);
                foreach (var element in _roiImageElements)
                    element.Draw(e.Graphics, imageBox1);
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

        #region 工具栏
        private void tsOpen_Click(object sender, EventArgs e)
        {
            using (FileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "BMP图像|*.bmp|JPG图像|*.jpg|PNG图像|*.png";
                dialog.DefaultExt = "bmp";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        Bitmap bmp = new Bitmap(dialog.FileName);
                        DisplayImage(bmp);
                        bmp.Dispose();
                        bmp = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            if (_displayImage == null)
                return;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "BMP图像|*.bmp|JPG图像|*.jpg|PNG图像|*.png";
                dialog.DefaultExt = "bmp";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                string ex = Path.GetExtension(dialog.FileName);
                ImageFormat format = ImageFormat.Bmp;
                switch (ex)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".png":
                        format = ImageFormat.Png;
                        break;
                    default:
                        break;
                }
                _displayImage.Save(dialog.FileName, format);
            }
        }

        private void tsZoomFit_Click(object sender, EventArgs e)
        {
            ZoomToFit();
        }

        private void tsActualSize_Click(object sender, EventArgs e)
        {
            imageBox1.ActualSize();
        }

        private void tsExpand_Click(object sender, EventArgs e)
        {
            imageBox1.ZoomIn();
        }

        private void tsShrink_Click(object sender, EventArgs e)
        {
            imageBox1.ZoomOut();
        }
        #endregion
    }
}
