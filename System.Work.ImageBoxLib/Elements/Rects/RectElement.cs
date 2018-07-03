using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Work.ImageBoxLib
{
    public class RectElement : Element
    {
        /// <summary>
        /// 矩形
        /// </summary>
        public RectangleF Rect { get; set; }
        /// <summary>
        /// 弧度制(0° ≤ angle ＜ 360°)
        /// </summary>
        public float Angle { get; set; }

        #region 构造函数
        public RectElement(RectangleF rect, float angle=0f)
        {
            Type = ElementType.Rectangle;
            Rect = rect;
            Angle = angle;
        }
        #endregion

        #region 重写
        public override bool Contains(float x, float y)
        {
            return Rect.Contains(x, y);
        }

        public override double AreaValue()
        {
            return Rect.Height * Rect.Width;
        }

        internal override void Draw(Graphics g, ImageBox box)
        {
            if (!Visible || Rect.IsEmpty)
                return;
            using (Pen p = new Pen(ForeColor, BorderWidth))
            {
                try
                {
                    var rect = box.GetOffsetRectangle(Rect);
                    if (Angle == 0)
                        g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
                    else
                    {
                        float cx = rect.X + rect.Width / 2, cy = rect.Y + rect.Height / 2;
                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(Angle);
                        g.TranslateTransform(-cx, -cy);
                        g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
                        g.DrawString(Angle.ToString(), new Font(FontFamily.Families.First(), 15f * box.ZoomFactor), Brushes.DeepPink, rect.X, rect.Y);
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    g.ResetTransform();
                }
            }
        }
        #endregion
    }
}
