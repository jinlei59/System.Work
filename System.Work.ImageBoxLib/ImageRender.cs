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
                _zoom = value;
            }
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
            if (_displayImage == null)
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

        public virtual RectangleF GetImageViewPort()
        {
            return this.ClientRectangle;
        }
        public virtual RectangleF GetSourceImageRegion()
        {
            RectangleF region = Rectangle.Empty;
            region = new RectangleF(0, 0, _displayImage.Width, _displayImage.Height);
            return region;
        }

        #endregion
        #region 私有方法

        #endregion
        #endregion
    }
}
