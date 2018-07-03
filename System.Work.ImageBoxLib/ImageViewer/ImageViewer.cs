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
using System.IO;

namespace System.Work.ImageBoxLib
{
    public partial class ImageViewer : UserControl
    {
        #region 变量
        private List<DotMatrixElement> _dotElements = null;
        private List<BlobElement> _blobElements = null;
        private List<Element> _elements = null;
        private List<Element> _roiElements = null;
        private List<Element> _otherElements = null;
        private bool _isLeftMouseDown = false;
        private DragHandleAnchor _leftMouseDownAnchor = DragHandleAnchor.None;
        private Point _lastMousePoint = Point.Empty;
        private PointF _lastImagePoint = PointF.Empty;
        private RectangleF _lastRoiRegion = RectangleF.Empty;
        private float _lastRoiAngle = 0f;

        private Element _selectElement = null;
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

        protected Element CurRoi
        {
            get
            {
                var e = _roiElements.FirstOrDefault(x => x.Selected);
                if (e != null)
                {
                    if (_selectElement == null || !_selectElement.uid.Equals(e.uid))
                    {
                        _selectElement = e;
                        SelectedRoiChanaged?.Invoke(this, e);
                    }
                }
                return e;
            }
        }

        public int DragHandleSize { get; private set; } = 8;


        public virtual int MinimumRoiSize { get; set; } = 1;

        public bool AllowZoom
        {
            get { return imageBox.AllowZoom; }
            set
            {
                imageBox.AllowZoom = value;
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
            get { return imageBox.AllowPainting; }
        }

        public float ZoomScale
        {
            get
            {
                return imageBox.ZoomFactor;
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
            _elements = new List<Element>();
            _roiElements = new List<Element>();
            _blobElements = new List<BlobElement>();
            _dotElements = new List<DotMatrixElement>();
            _otherElements = new List<Element>();
            imageBox.BeginUpdate();
            AllowZoom = true;
            imageBox.EndUpdate();
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

        #region 过时的 Obsolete
        #region 显示所有内容
        [Obsolete]
        public void BeginDisplay()
        {
            imageBox.BeginUpdate();
        }
        [Obsolete]
        public void EndDisplay()
        {
            imageBox.EndUpdate();
        }
        [Obsolete]
        public void Display()
        {
            imageBox.Invalidate();
        }

        #endregion

        #region 显示图像
        [Obsolete]
        public void DisplayImage(Bitmap image, bool immediatelyDraw = false)
        {
            Image = image;
            if (immediatelyDraw)
                this.Display();
        }

        #endregion

        #region 添加&删除字符串
        [Obsolete]
        public void AddString(string s, Font font, Brush brush, float x, float y)
        {
            _elements.Add(new StringElement(s, font, brush, x, y));
        }
        [Obsolete]
        public void ClearString()
        {
            _elements.RemoveAll(x => x.Type == ElementType.String);
        }
        #endregion

        #region 添加&删除直线
        [Obsolete]
        public void AddLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            _elements.Add(new LineElement(pen, x1, y1, x2, y2));
        }
        [Obsolete]
        public void ClearLine()
        {
            _elements.RemoveAll(x => x.Type == ElementType.Line);
        }
        #endregion

        #region 添加&删除矩形框
        [Obsolete]
        public void AddRectangle(Pen pen, float x, float y, float width, float height)
        {
            _elements.Add(new RectangleElement(pen, x, y, width, height));
        }
        [Obsolete]
        public void ClearRectangle()
        {
            _elements.RemoveAll(x => x.Type == ElementType.Rectangle);
        }
        #endregion

        #region 添加&删除圆
        [Obsolete]
        public void AddEllipse(Pen pen, float x, float y, float width, float height)
        {
            _elements.Add(new EllipseElement(pen, x, y, width, height));
        }
        [Obsolete]
        public void ClearEllipse()
        {
            _elements.RemoveAll(x => x.Type == ElementType.Ellipse);
        }
        #endregion

        #region 添加/删除ROI
        [Obsolete]
        public void AddElement(Element e)
        {
            _elements.Add(e);
        }
        [Obsolete]
        public void AddRoiElement(Element e)
        {
            _roiElements.Add(e);
        }
        [Obsolete]
        public void RemoveRoiElement(Element e)
        {
            _roiElements.Remove(e);
        }
        [Obsolete]
        public void ClearAllROI()
        {
            _roiElements.Clear();
        }
        [Obsolete]
        public void SelectRoi(Guid uid)
        {
            if (_roiElements == null || _roiElements.Count < 1)
                return;
            _roiElements.ForEach(x => x.Selected = false);
            var element = _roiElements.FirstOrDefault(x => x.uid.Equals(uid));
            _selectElement = element;
            if (element != null)
            {
                element.Selected = true;
            }
        }
        #endregion

        #region 添加删除Blob
        [Obsolete]
        public void AddBlobElement(Element e)
        {
            if (e is BlobElement)
            {
                _blobElements.Add(e as BlobElement);
            }
        }
        [Obsolete]
        public void RemoveBlobElement(Element e)
        {
            if (e is BlobElement)
            {
                _blobElements.Remove(e as BlobElement);
            }
        }
        [Obsolete]
        public void ClearBlobElement()
        {
            _blobElements.Clear();
        }
        #endregion

        #region 添加删除Blob
        [Obsolete]
        public void AddDotMatrixElement(Element e)
        {
            if (e is DotMatrixElement)
            {
                _dotElements.Add(e as DotMatrixElement);
            }
        }
        [Obsolete]
        public void RemoveDotMatrixElement(Element e)
        {
            if (e is DotMatrixElement)
            {
                _dotElements.Remove(e as DotMatrixElement);
            }
        }
        [Obsolete]
        public void ClearDotMatrixElement()
        {
            _dotElements.Clear();
        }
        #endregion
        #endregion

        #region new

        public void NewBeginDisplay()
        {
            imageBox.BeginUpdate();
        }

        public void NewDisplayImage(Bitmap bmp)
        {
            imageBox.Image = bmp;
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
            imageBox.EndUpdate();
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

        #region 自适应

        public void ZoomToFit()
        {
            this.imageBox.ZoomToFit();
        }

        #endregion

        #region 获取图像中的位置

        public PointF GetOffsetPoint(PointF pt)
        {
            return imageBox.GetOffsetPoint(pt);
        }

        public Point PointToImage(float x, float y)
        {
            return imageBox.PointToImage(x, y, false);
        }

        public RectangleF GetOffsetRectangle(RectangleF source)
        {
            RectangleF viewport;
            RectangleF scaled;
            float offsetX;
            float offsetY;

            viewport = imageBox.GetImageViewPort();
            scaled = imageBox.GetScaledRectangle(source);
            offsetX = viewport.Left + this.Padding.Left + this.AutoScrollPosition.X;
            offsetY = viewport.Top + this.Padding.Top + this.AutoScrollPosition.Y;

            return new RectangleF(new PointF(scaled.Left + offsetX, scaled.Top + offsetY), scaled.Size);
        }

        public virtual Rectangle GetImageViewPort()
        {
            return imageBox.GetImageViewPort();
        }
        #endregion

        #endregion
        #region protected &private

        protected virtual void PositionDragHandles()
        {
            Element e = this.CurRoi;
            if (e != null && e.Selected && !e.Region.IsEmpty)
            {
                int left;
                int top;
                int right;
                int bottom;
                int halfWidth;
                int halfHeight;
                int halfDragHandleSize;
                Rectangle viewport;
                int offsetX;
                int offsetY;

                viewport = this.imageBox.GetImageViewPort();
                offsetX = viewport.Left + this.imageBox.Padding.Left + this.imageBox.AutoScrollPosition.X;
                offsetY = viewport.Top + this.imageBox.Padding.Top + this.imageBox.AutoScrollPosition.Y;
                halfDragHandleSize = DragHandleSize / 2;                
                halfWidth = Convert.ToInt32(e.Region.Width * this.imageBox.ZoomFactor) / 2;
                halfHeight = Convert.ToInt32(e.Region.Height * this.imageBox.ZoomFactor) / 2;

                if (e.Type == ElementType.Line)
                {
                    var line = e as RoiLineElement;

                    left = Convert.ToInt32((e.Region.Left * this.imageBox.ZoomFactor) + offsetX);
                    top = Convert.ToInt32((e.Region.Top * this.imageBox.ZoomFactor) + offsetY);
                    right = left + Convert.ToInt32(e.Region.Width * this.imageBox.ZoomFactor);
                    bottom = top + Convert.ToInt32(e.Region.Height * this.imageBox.ZoomFactor);

                    e.DragHandleCollection[DragHandleAnchor.TopLeft].Bounds = new Rectangle(left - this.DragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    e.DragHandleCollection[DragHandleAnchor.BottomRight].Bounds = new Rectangle(right, bottom, this.DragHandleSize, this.DragHandleSize);

                    e.DragHandleCollection[DragHandleAnchor.TopCenter].Enabled = false;
                    e.DragHandleCollection[DragHandleAnchor.TopCenter].Visible = false;

                    e.DragHandleCollection[DragHandleAnchor.TopRight].Enabled = false;
                    e.DragHandleCollection[DragHandleAnchor.TopRight].Visible = false;

                    e.DragHandleCollection[DragHandleAnchor.MiddleLeft].Enabled = false;
                    e.DragHandleCollection[DragHandleAnchor.MiddleLeft].Visible = false;

                    e.DragHandleCollection[DragHandleAnchor.MiddleRight].Enabled = false;
                    e.DragHandleCollection[DragHandleAnchor.MiddleRight].Visible = false;

                    e.DragHandleCollection[DragHandleAnchor.BottomLeft].Enabled = false;
                    e.DragHandleCollection[DragHandleAnchor.BottomLeft].Visible = false;

                    e.DragHandleCollection[DragHandleAnchor.BottomCenter].Enabled = false;
                    e.DragHandleCollection[DragHandleAnchor.BottomCenter].Visible = false;

                    e.DragHandleCollection[DragHandleAnchor.Rotation].Enabled = false;
                    e.DragHandleCollection[DragHandleAnchor.Rotation].Visible = false;
                }
                else
                {
                    left = Convert.ToInt32((e.Region.Left * this.imageBox.ZoomFactor) + offsetX);
                    top = Convert.ToInt32((e.Region.Top * this.imageBox.ZoomFactor) + offsetY);
                    right = left + Convert.ToInt32(e.Region.Width * this.imageBox.ZoomFactor);
                    bottom = top + Convert.ToInt32(e.Region.Height * this.imageBox.ZoomFactor);

                    e.DragHandleCollection[DragHandleAnchor.TopLeft].Bounds = new Rectangle(left - this.DragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    e.DragHandleCollection[DragHandleAnchor.TopCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    e.DragHandleCollection[DragHandleAnchor.TopRight].Bounds = new Rectangle(right, top - this.DragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    e.DragHandleCollection[DragHandleAnchor.MiddleLeft].Bounds = new Rectangle(left - this.DragHandleSize, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    e.DragHandleCollection[DragHandleAnchor.MiddleRight].Bounds = new Rectangle(right, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    e.DragHandleCollection[DragHandleAnchor.BottomLeft].Bounds = new Rectangle(left - this.DragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                    e.DragHandleCollection[DragHandleAnchor.BottomCenter].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, bottom, this.DragHandleSize, this.DragHandleSize);
                    e.DragHandleCollection[DragHandleAnchor.BottomRight].Bounds = new Rectangle(right, bottom, this.DragHandleSize, this.DragHandleSize);
                    if (e.IsRotation)
                        e.DragHandleCollection[DragHandleAnchor.Rotation].Bounds = new Rectangle(left + halfWidth - halfDragHandleSize, top + halfHeight - halfDragHandleSize, this.DragHandleSize, this.DragHandleSize);
                    else
                    {
                        e.DragHandleCollection[DragHandleAnchor.Rotation].Enabled = false;
                        e.DragHandleCollection[DragHandleAnchor.Rotation].Visible = false;
                    }
                }
            }
        }

        protected virtual void DrawDragHandle(Graphics graphics, DragHandle handle)
        {
            int left;
            int top;
            int width;
            int height;
            Pen outerPen;
            Brush innerBrush;

            left = handle.Bounds.Left;
            top = handle.Bounds.Top;
            width = handle.Bounds.Width;
            height = handle.Bounds.Height;

            if (handle.Enabled)
            {
                outerPen = new Pen(Color.Orange);
                innerBrush = Brushes.OrangeRed;
            }
            else
            {
                outerPen = SystemPens.ControlDark;
                innerBrush = SystemBrushes.Control;
            }

            graphics.FillRectangle(innerBrush, left + 1, top + 1, width - 2, height - 2);
            graphics.DrawLine(outerPen, left + 1, top, left + width - 2, top);
            graphics.DrawLine(outerPen, left, top + 1, left, top + height - 2);
            graphics.DrawLine(outerPen, left + 1, top + height - 1, left + width - 2, top + height - 1);
            graphics.DrawLine(outerPen, left + width - 1, top + 1, left + width - 1, top + height - 2);
        }

        protected virtual DragHandleAnchor SetCursor(Point point)
        {
            Cursor cursor;
            DragHandleAnchor handleAnchor;
            if (this.CurRoi == null || !this.CurRoi.Selected || this.CurRoi.Region.IsEmpty)
            {
                cursor = Cursors.Default;
                handleAnchor = DragHandleAnchor.None;
            }
            else
            {
                handleAnchor = this.CurRoi.HitTest(point);
                if (handleAnchor != DragHandleAnchor.None && this.CurRoi.DragHandleCollection[handleAnchor].Enabled)
                {
                    switch (handleAnchor)
                    {
                        case DragHandleAnchor.TopLeft:
                        case DragHandleAnchor.BottomRight:
                            cursor = Cursors.SizeNWSE;
                            break;
                        case DragHandleAnchor.TopCenter:
                        case DragHandleAnchor.BottomCenter:
                            cursor = Cursors.SizeNS;
                            break;
                        case DragHandleAnchor.TopRight:
                        case DragHandleAnchor.BottomLeft:
                            cursor = Cursors.SizeNESW;
                            break;
                        case DragHandleAnchor.MiddleLeft:
                        case DragHandleAnchor.MiddleRight:
                            cursor = Cursors.SizeWE;
                            break;
                        case DragHandleAnchor.Rotation:
                            cursor = CursorGenerator.CreateCursor(Properties.Resources.rotation, this.Cursor.HotSpot);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (this.CurRoi.Region.Contains(this.imageBox.PointToImage(point)))
                {
                    cursor = Cursors.SizeAll;
                    handleAnchor = DragHandleAnchor.Move;
                }
                else
                {
                    cursor = Cursors.Default;
                }
            }

            this.Cursor = cursor;
            return handleAnchor;
        }

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
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = true;
                _leftMouseDownAnchor = SetCursor(e.Location);
                var imagePt = imageBox.PointToImage(e.Location);
                var element = _roiElements.Where(x => x.Visible && x.Enable && x.Contains(imagePt.X, imagePt.Y) || x.HitTest(e.Location) != DragHandleAnchor.None).OrderBy(x => x.AreaValue()).FirstOrDefault();
                _roiElements.ForEach(x => x.Selected = false);
                if (element != null)
                {
                    element.Selected = true;
                    _lastMousePoint = e.Location;
                    _lastImagePoint = element.Region.Location;
                    _lastRoiRegion = element.Region;
                    _lastRoiAngle = element.Angle;
                }
                imageBox.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                Cursor = CursorGenerator.CreateCursor(Properties.Resources.hand, this.Cursor.HotSpot);
            }

            this.OnMouseDown(e);
        }
        private void imageBox_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateCursorPosition(e.Location);
            UpdateRGB(e.Location);
            if (e.Button == MouseButtons.Right)
            {
                Cursor = CursorGenerator.CreateCursor(Properties.Resources.hand, this.Cursor.HotSpot);
            }
            else if (e.Button == MouseButtons.Left)
            {
                #region 左键操作
                if (_isLeftMouseDown && this.CurRoi != null && _leftMouseDownAnchor != DragHandleAnchor.None)
                {
                    if (_leftMouseDownAnchor == DragHandleAnchor.Move)
                    {
                        float x = _lastImagePoint.X + (e.Location.X - _lastMousePoint.X) / imageBox.ZoomFactor;
                        float y = _lastImagePoint.Y + (e.Location.Y - _lastMousePoint.Y) / imageBox.ZoomFactor;
                        this.CurRoi.Region = new RectangleF(x, y, this.CurRoi.Region.Width, this.CurRoi.Region.Height);
                    }
                    else
                    {
                        #region Resize & Rotation
                        float left = this._lastRoiRegion.Left;
                        float top = this._lastRoiRegion.Top;
                        float right = this._lastRoiRegion.Right;
                        float bottom = this._lastRoiRegion.Bottom;

                        float offx = (e.Location.X - _lastMousePoint.X) / imageBox.ZoomFactor;
                        float offy = (e.Location.Y - _lastMousePoint.Y) / imageBox.ZoomFactor;

                        switch (_leftMouseDownAnchor)
                        {
                            #region Resize
                            case DragHandleAnchor.TopLeft:
                                top += offy;
                                if (bottom - top < MinimumRoiSize)
                                    top = bottom - MinimumRoiSize;
                                left += offx;
                                if (right - left < MinimumRoiSize)
                                    left = right - MinimumRoiSize;
                                break;
                            case DragHandleAnchor.TopCenter:
                                top += offy;
                                if (bottom - top < MinimumRoiSize)
                                    top = bottom - MinimumRoiSize;
                                break;
                            case DragHandleAnchor.TopRight:
                                top += offy;
                                if (bottom - top < MinimumRoiSize)
                                    top = bottom - MinimumRoiSize;
                                right += offx;
                                if (right - left < MinimumRoiSize)
                                    right = left + MinimumRoiSize;
                                break;
                            case DragHandleAnchor.MiddleLeft:
                                left += offx;
                                if (right - left < MinimumRoiSize)
                                    left = right - MinimumRoiSize;
                                break;
                            case DragHandleAnchor.MiddleRight:
                                right += offx;
                                if (right - left < MinimumRoiSize)
                                    right = left + MinimumRoiSize;
                                break;
                            case DragHandleAnchor.BottomLeft:
                                bottom += offy;
                                if (bottom - top < MinimumRoiSize)
                                    bottom = top + MinimumRoiSize;
                                left += offx;
                                if (right - left < MinimumRoiSize)
                                    left = right - MinimumRoiSize;
                                break;
                            case DragHandleAnchor.BottomCenter:
                                bottom += offy;
                                if (bottom - top < MinimumRoiSize)
                                    bottom = top + MinimumRoiSize;
                                break;
                            case DragHandleAnchor.BottomRight:
                                bottom += offy;
                                if (bottom - top < MinimumRoiSize)
                                    bottom = top + MinimumRoiSize;
                                right += offx;
                                if (right - left < MinimumRoiSize)
                                    right = left + MinimumRoiSize;
                                break;
                            #endregion
                            #region 角度
                            case DragHandleAnchor.Rotation:
                                double distance = Math.Sqrt(offx * offx + offy * offy);
                                double sin = Math.Abs(offy / distance);
                                double angle = Math.Asin(sin) / Math.PI * 180 + Math.PI;
                                if (offx > 0)
                                {
                                    if (offy > 0)
                                        angle += 0;
                                    else
                                        angle = 360 - angle;
                                }
                                else
                                {
                                    if (offy > 0)
                                        angle = 180 - angle;
                                    else
                                        angle = 180 + angle;
                                }
                                this.CurRoi.Angle = /*_lastRoiAngle +*/ (float)angle;
                                break;
                            #endregion
                            default:
                                break;
                        }
                        this.CurRoi.Region = new RectangleF(left, top, right - left, bottom - top);
                        #endregion
                    }
                }
                imageBox.Invalidate();
                #endregion
            }
            else
            {
                SetCursor(e.Location);
            }

            this.OnMouseMove(e);
        }

        private void imageBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftMouseDown = false;
            }
            else if (e.Button == MouseButtons.Right)
            {
                Cursor = Cursors.Default;
            }
            this.OnMouseUp(e);
        }

        private void imageBox_MouseWheel(object sender, MouseEventArgs e)
        {
            UpdateCursorPosition(e.Location);
            UpdateRGB(e.Location);
            UpdateStatusBar();

            this.OnMouseWheel(e);
        }

        private void imageBox_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!imageBox.AllowPainting)
                    return;

                #region old
                /*
                foreach (var element in _blobElements)
                {
                    var points = element.GetPoints();
                    if (points == null)
                        continue;
                    int len = points.Length;
                    PointF[] pts = new PointF[len];
                    for (int i = 0; i < len; i++)
                    {
                        pts[i] = imageBox.GetOffsetPoint(points[i]);
                    }
                    element.DrawElement(e.Graphics, pts);
                    Array.Clear(pts, 0, len);
                }

                foreach (var element in _dotElements)
                {
                    var points = element.GetPoints();
                    if (points == null)
                        continue;
                    int len = points.Length;
                    PointF[] pts = new PointF[len];
                    for (int i = 0; i < len; i++)
                    {
                        pts[i] = imageBox.GetOffsetPoint(points[i]);
                    }
                    element.DrawElement(e.Graphics, imageBox.ZoomFactor, pts);
                    Array.Clear(pts, 0, len);
                }

                foreach (var element in _elements)
                {
                    //element.DrawElement(e.Graphics, imageBox.ZoomFactor, imageBox.GetOffsetRectangle(element.Region));
                    if (element.Type == ElementType.Line)
                    {
                        var p1 = imageBox.GetOffsetPoint(element.Region.X, element.Region.Y);
                        var p2 = imageBox.GetOffsetPoint(element.Region.Width, element.Region.Height);
                        element.DrawElement(e.Graphics, imageBox.ZoomFactor, p1.X, p1.Y, p2.X, p2.Y);
                    }
                    else
                    {
                        element.DrawElement(e.Graphics, imageBox.ZoomFactor, imageBox.GetOffsetRectangle(element.Region));
                    }
                }
                */
                #endregion

                #region Other Elements
                foreach (var element in _otherElements)
                {
                    if (element is BlobElement)
                    {
                        #region 绘制Blob
                        var blob = element as BlobElement;
                        var points = blob.GetPoints();
                        if (points == null)
                            continue;
                        int len = points.Length;
                        PointF[] pts = new PointF[len];
                        for (int i = 0; i < len; i++)
                        {
                            pts[i] = imageBox.GetOffsetPoint(points[i]);
                        }
                        element.DrawElement(e.Graphics, pts);
                        Array.Clear(pts, 0, len);
                        #endregion
                    }
                    else if (element is DotMatrixElement)
                    {
                        #region 绘制点阵轮廓
                        var dot = element as DotMatrixElement;
                        var points = dot.GetPoints();
                        if (points == null)
                            continue;
                        int len = points.Length;
                        PointF[] pts = new PointF[len];
                        for (int i = 0; i < len; i++)
                        {
                            pts[i] = imageBox.GetOffsetPoint(points[i]);
                        }
                        element.DrawElement(e.Graphics, imageBox.ZoomFactor, pts);
                        Array.Clear(pts, 0, len);
                        #endregion
                    }
                    else if (element is LineElement)
                    {
                        #region 绘制直线
                        var p1 = imageBox.GetOffsetPoint(element.Region.X, element.Region.Y);
                        var p2 = imageBox.GetOffsetPoint(element.Region.Width, element.Region.Height);
                        element.DrawElement(e.Graphics, imageBox.ZoomFactor, p1.X, p1.Y, p2.X, p2.Y);
                        #endregion
                    }
                    else
                    {
                        #region 绘制其他
                        element.DrawElement(e.Graphics, imageBox.ZoomFactor, imageBox.GetOffsetRectangle(element.Region));
                        #endregion
                    }
                }
                #endregion

                #region Roi Elements
                var curRoi = this.CurRoi;
                if (curRoi != null && !curRoi.Region.IsEmpty)
                {
                    PositionDragHandles();
                    foreach (DragHandle handle in curRoi.DragHandleCollection)
                    {
                        if (handle.Visible)
                        {
                            this.DrawDragHandle(e.Graphics, handle);
                        }
                    }
                }
                foreach (var element in _roiElements)
                {
                    switch (element.Type)
                    {
                        case ElementType.Line:
                            var rl = element as RoiLineElement;
                            var p1 = imageBox.GetOffsetPoint(rl.X1, rl.Y1);
                            var p2 = imageBox.GetOffsetPoint(rl.X2, rl.Y2);
                            element.DrawElement(e.Graphics, imageBox.ZoomFactor, p1.X, p1.Y, p2.X, p2.Y);
                            break;
                        default:
                            element.DrawElement(e.Graphics, imageBox.ZoomFactor, imageBox.GetOffsetRectangle(element.Region));
                            break;
                    }
                }
                #endregion

                this.OnPaint(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion


        private void imageBox_Resize(object sender, EventArgs e)
        {
        }

        private void imageBox_Scroll(object sender, ScrollEventArgs e)
        {
        }

        private void imageBox_ZoomChanged(object sender, EventArgs e)
        {
        }
    }
}
