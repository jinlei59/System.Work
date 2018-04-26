using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace System.Work.ImageBoxLib
{
    public partial class ImageViewer : UserControl
    {
        #region 变量
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

        protected Bitmap Image
        {
            get
            {
                return imageBox.Image;
            }

            set
            {
                imageBox.Image = value;
            }
        }

        #endregion

        #region 事件

        #endregion

        #region 构造函数
        public ImageViewer()
        {
            InitializeComponent();
        }
        #endregion

        #region private&protected method

        #region Protected Members

        protected string FormatPoint(Point point)
        {
            return string.Format("X:{0} Y:{1}", point.X, point.Y);
        }

        protected string GetImageInfo()
        {
            return Image == null ? "W:0 H:0" : string.Format("W:{0} H:{1}", Image.Width, Image.Height);
        }

        #endregion

        private void OpenImage(Bitmap image)
        {
            this.Image = null;
            this.Image = image;
            ZoomToFit();
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            tssZoom.Text = string.Format("{0}%", imageBox.Zoom);
            tssImageInfo.Text = GetImageInfo();
        }

        private void UpdateCursorPosition(Point location)
        {
            if (imageBox.IsPointInImage(location))
            {
                Point point;

                point = imageBox.PointToImage(location);
                tssLocation.Text = this.FormatPoint(point);
            }
            else
            {
                tssLocation.Text = this.FormatPoint(Point.Empty);
            }
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

        public void UpdateRGB(Point location)
        {
            if (imageBox.IsPointInImage(location))
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
            IntPtr dC = GetDC(imageBox.Handle);
            uint pixel = GetPixel(dC, x, y);
            ReleaseDC(IntPtr.Zero, dC);
            return Color.FromArgb((int)(pixel & 255u), (int)(pixel & 65280u) >> 8, (int)(pixel & 16711680u) >> 16);
        }
        #endregion

        #endregion

        #region 公共方法

        #region 显示所有内容

        public void BeginDisplay()
        {
            imageBox.BeginUpdate();
        }
        public void EndDisplay()
        {
            imageBox.EndUpdate();
        }

        public void Display()
        {
            while (!imageBox.AllowPainting)
                EndDisplay();
        }

        #endregion

        #region 显示图像

        public void DisplayImage(Bitmap image, bool immediatelyDraw = false)
        {
            Image = image;
            if (immediatelyDraw)
                this.Display();
        }

        #endregion
        #region 添加&删除字符串
        public void DrawString(string s, Font font, Brush brush, float x, float y)
        {
            //imageBox.AddElement(new StringElement(s, font, brush, x, y));
        }
        public void ClearString()
        {
            //imageBox.ClearElement(ElementType.String);
        }
        #endregion

        #region 添加&删除直线
        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            //imageBox.AddElement(new LineElement(pen, x1, y1, x2, y2));
        }
        public void ClearLine()
        {
            //imageBox.ClearElement(ElementType.Line);
        }
        #endregion

        #region 添加&删除矩形框
        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            //imageBox.AddElement(new RectangleElement(pen, x, y, width, height));
        }

        public void ClearRectangle()
        {
            //imageBox.ClearElement(ElementType.Rectangle);
        }
        #endregion

        #region 添加&删除圆
        public void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            //imageBox.AddElement(new EllipseElement(pen, x, y, width, height));
        }

        public void ClearEllipse()
        {
            //imageBox.ClearElement(ElementType.Ellipse);
        }
        #endregion

        #region 添加/删除ROI

        #endregion

        #region 缩放

        public void ZoomToFit()
        {
            this.imageBox.ZoomToFit();
        }

        #endregion

        #endregion

        #region 窗体按钮事件
        private void tsOpen_Click(object sender, EventArgs e)
        {
            using (FileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "All Supported Images (*.bmp;*.dib;*.rle;*.gif;*.jpg;*.png)|*.bmp;*.dib;*.rle;*.gif;*.jpg;*.png|Bitmaps (*.bmp;*.dib;*.rle)|*.bmp;*.dib;*.rle|Graphics Interchange Format (*.gif)|*.gif|Joint Photographic Experts (*.jpg)|*.jpg|Portable Network Graphics (*.png)|*.png|All Files (*.*)|*.*";
                dialog.DefaultExt = "png";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        this.OpenImage(new Bitmap(dialog.FileName));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tsNormal_Click(object sender, EventArgs e)
        {
            imageBox.ActualSize();
            imageBox.CenterToImage();
            UpdateStatusBar();
        }

        private void tsZoomIn_Click(object sender, EventArgs e)
        {
            imageBox.ZoomIn();
            UpdateStatusBar();
        }

        private void tsZoomOut_Click(object sender, EventArgs e)
        {
            imageBox.ZoomOut();
            UpdateStatusBar();
        }

        private void imageBox_MouseDown(object sender, MouseEventArgs e)
        {

        }
        private void imageBox_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateCursorPosition(e.Location);
            UpdateRGB(e.Location);
        }

        private void imageBox_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void imageBox_MouseWheel(object sender, MouseEventArgs e)
        {
            UpdateCursorPosition(e.Location);
            UpdateRGB(e.Location);
            UpdateStatusBar();
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {
            if (!imageBox.AllowPainting)
                return;
        }
        #endregion
    }
}
