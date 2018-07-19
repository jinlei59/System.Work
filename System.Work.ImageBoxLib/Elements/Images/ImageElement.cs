using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class ImageElement : Element
    {
        #region 变量

        protected Bitmap _image = null;
        protected RectangleF _srcRect = RectangleF.Empty;
        protected RectangleF _dstRect = RectangleF.Empty;

        #endregion

        #region 构造函数

        public ImageElement(Bitmap image) : this(image, new RectangleF(0, 0, image.Width, image.Height))
        {
        }

        public ImageElement(Bitmap image, RectangleF dstRect) : this(image, new RectangleF(0, 0, image.Width, image.Height), dstRect)
        { }

        public ImageElement(Bitmap image, RectangleF srcRect, RectangleF dstRect)
        {
            Type = ElementType.Image;
            _image = image;
            _srcRect = srcRect;
            _dstRect = dstRect;
        }
        #endregion

        #region 重写

        public override double AreaValue()
        {
            return _dstRect.Width * _dstRect.Height;
        }

        public override bool Contains(float x, float y)
        {
            return _dstRect.Contains(x, y);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || this.IsEmpty() || _image == null)
                return;
            try
            {
                var rect = box.GetOffsetRectangle(_dstRect);
                g.DrawImage(_image, rect, _srcRect, GraphicsUnit.Pixel);
            }
            catch (Exception ex)
            {
                //
                Debug.Fail(ex.Message, ex.ToString());
            }
        }
        #endregion
    }

    public class ImageElementEventArgs : ElementEventArgs
    {
        public Guid Uid { get; set; }
        public Bitmap Image { get; set; }

        public ImageElementEventArgs(Bitmap bmp) : this(Guid.NewGuid(), bmp)
        {

        }

        public ImageElementEventArgs(Guid uid, Bitmap bmp)
        {
            Uid = uid;
            Image = bmp;
        }
    }
}
