using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.ImageBoxLib
{
    public class ImageRender : Control
    {
        #region 变量

        private int _displayCount = 0;
        private Bitmap _displayImage = null;
        private InterpolationMode _interpolationMode;
        private float _zoom = 1f;
        private Point _startMousePosition = Point.Empty;
        private Point _lastDisplaayImagePoint = Point.Empty;
        private Point _displayImagePoint = Point.Empty;

        #endregion

        #region 属性

        [Browsable(false)]
        public virtual bool AllowPainting
        {
            get { return _displayCount == 0; }
        }

        [Category("Appearance")]
        [DefaultValue(InterpolationMode.NearestNeighbor)]
        public virtual InterpolationMode InterpolationMode
        {
            get { return _interpolationMode; }
            set
            {
                if (value == InterpolationMode.Invalid)
                {
                    value = InterpolationMode.Default;
                }

                if (_interpolationMode != value)
                {
                    _interpolationMode = value;
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(1f)]
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                if (_zoom != value)
                    _zoom = value;
            }
        }

        public bool IsMoving { get; set; }

        #endregion

        #region 保护属性

        protected virtual Size ViewSize
        {
            get { return this.GetImageSize(); }
        }

        #endregion

        #region 事件

        #endregion

        #region 构造函数
        public ImageRender()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
            this.BeginDisplay();
            this.BackColor = Color.Black;
            this.InterpolationMode = InterpolationMode.NearestNeighbor;
            this.Zoom = 1f;
            this.IsMoving = false;
            this.EndDisplay();
        }
        #endregion

        #region 重写

        #region 绘制
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.AllowPainting)
                return;
            base.OnPaint(e);
            //绘制图像
            DrawImage(e.Graphics);
            //绘制element
        }
        #endregion

        #region 鼠标操作
        /*
         * 右键拖拽
         * 滚轮缩放
         * 左键操作roi
         */

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!this.Focused)
            {
                this.Focus();
            }
            if (e.Button == MouseButtons.Right)
            {
                _startMousePosition = e.Location;
                _lastDisplaayImagePoint = _displayImagePoint;
                IsMoving = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Right)
            {
                this.ProcessMoving(e);
            }
            else if (e.Button == MouseButtons.Left)
            { }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Right)
            {
                IsMoving = false;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }
        #endregion

        #endregion

        #region 自定义方法

        #region 公有方法

        public void BeginDisplay()
        {
            _displayCount++;
        }
        public void EndDisplay()
        {
            _displayCount--;
            _displayCount = _displayCount < 0 ? 0 : _displayCount;
            if (this.AllowPainting)
            {
                this.Invalidate();
            }
        }

        public void DispalyImage(Bitmap image)
        {
            _displayImage = image;
        }

        public virtual Rectangle GetImageViewPort()
        {
            return this.ClientRectangle;
        }

        public virtual RectangleF GetSourceImageRegion()
        {
            RectangleF region = Rectangle.Empty;
            if (!this.ViewSize.IsEmpty)
            {
                float sourceLeft;
                float sourceTop;
                float sourceWidth;
                float sourceHeight;
                Rectangle viewPort;

                viewPort = this.GetImageViewPort();
                sourceLeft = _displayImagePoint.X;
                sourceTop = _displayImagePoint.Y;
                sourceWidth = (float)(viewPort.Width / this.Zoom);
                sourceHeight = (float)(viewPort.Height / this.Zoom);

                region = new RectangleF(sourceLeft, sourceTop, sourceWidth, sourceHeight);

            }
            return region;
        }

        public Point PointToImage(Point pt)
        {
            int x = (int)(_displayImagePoint.X + pt.X / Zoom);
            int y = (int)(_displayImagePoint.Y + pt.Y / Zoom);
            return new Point(x, y);
        }

        /// <summary>
        /// 将图像居中显示
        /// </summary>
        public void CenterAt()
        {
            if (this.ViewSize.IsEmpty)
                return;
            float cx = this._displayImage.Width / 2f, cy = this._displayImage.Height / 2f;
            var rect = GetImageViewPort();
            int x = (int)(cx * Zoom - rect.Width / 2f);
            int y = (int)(cy * Zoom - rect.Height / 2f);
            _displayImagePoint = new Point(x, y);
        }

        public void ZoomFit()
        {
            if (this.ViewSize.IsEmpty)
                return;
            var rect = GetImageViewPort();
            float zx = rect.Width / (float)this._displayImage.Width;
            float zy = rect.Height / (float)this._displayImage.Height;
            Zoom = zx < zy ? zx : zy;
        }
        public void AddString(string str, float x, float y, Color color)
        { }
        public void AddRectAngle(float x, float y, float width, float height, Color color)
        { }
        public void AddLine(float x1, float y1, float x2, float y2, Color color)
        { }
        #endregion

        #region 保护方法

        protected virtual void DrawImage(Graphics g)
        {
            if (_displayImage == null || this.ViewSize.IsEmpty)
                return;
            InterpolationMode currentInterpolationMode;
            PixelOffsetMode currentPixelOffsetMode;

            currentInterpolationMode = g.InterpolationMode;
            currentPixelOffsetMode = g.PixelOffsetMode;

            g.InterpolationMode = this.GetInterpolationMode();

            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            try
            {
                g.DrawImage(this._displayImage, this.GetImageViewPort(), this.GetSourceImageRegion(), GraphicsUnit.Pixel);
            }
            catch (ArgumentException)
            {
                // ignore errors that occur due to the image being disposed
            }
            catch (OutOfMemoryException)
            {
                // also ignore errors that occur due to running out of memory
            }

            g.PixelOffsetMode = currentPixelOffsetMode;
            g.InterpolationMode = currentInterpolationMode;

        }

        protected virtual InterpolationMode GetInterpolationMode()
        {
            //return InterpolationMode.Default;
            InterpolationMode mode;

            mode = this.InterpolationMode;

            if (mode == InterpolationMode.Default)
            {
                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (this.Zoom < 1)
                {
                    // TODO: Check to see if we should cherry pick other modes depending on how much the image is actually zoomed
                    mode = InterpolationMode.HighQualityBicubic;
                }
                else
                {
                    mode = InterpolationMode.NearestNeighbor;
                }
            }

            return mode;
        }

        /// <summary>
        ///   Performs mouse based panning
        /// </summary>
        /// <param name="e">
        ///   The <see cref="MouseEventArgs" /> instance containing the event data.
        /// </param>
        protected virtual void ProcessMoving(MouseEventArgs e)
        {
            if (this.ViewSize.IsEmpty || !IsMoving)
                return;
            int offX = (int)((e.Location.X - _startMousePosition.X) / Zoom);
            int offY = (int)((e.Location.Y - _startMousePosition.Y) / Zoom);
            _displayImagePoint.X = _lastDisplaayImagePoint.X - offX;
            _displayImagePoint.Y = _lastDisplaayImagePoint.Y - offY;
            this.Invalidate();
        }
        #endregion

        #region 私有方法

        private Size GetImageSize()
        {
            Size result;

            // HACK: This whole thing stinks. Hey MS, how about an IsDisposed property for images?

            if (this._displayImage != null)
            {
                try
                {
                    result = this._displayImage.Size;
                }
                catch
                {
                    result = Size.Empty;
                }
            }
            else
            {
                result = Size.Empty;
            }

            return result;
        }

        #endregion

        #endregion
    }
}
