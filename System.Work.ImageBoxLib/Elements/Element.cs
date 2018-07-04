using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Work.ImageBoxLib
{
    [Serializable]
    public abstract class Element:IDisposable
    {
        #region 公共属性
        public Guid uid { get; set; }
        public Guid ParentUid { get; set; }
        public ElementType Type { get; set; }
        public bool Enable { get; set; }
        public bool Visible { get; set; }
        public bool Selected { get; set; }
        public Color ForeColor { get; set; }
        public float BorderWidth { get; set; }

        public bool AutoChangeSize { get; set; }
        #endregion

        #region 事件

        public event EventHandler<ElementEventArgs> ROIShapeChannged;

        #endregion

        #region 特殊属性

        internal DragHandleCollection DragHandleCollection { get; set; }
        internal int DragHandleSize { get; private set; } = 8;
        internal int MinimumRoiSize { get; set; } = 1;
        #endregion

        #region 构造函数
        public Element()
        {
            uid = Guid.NewGuid();
            ParentUid = Guid.Empty;
            Enable = true;
            Visible = true;
            Selected = false;
            ForeColor = Color.Red;
            BorderWidth = 1f;
            AutoChangeSize = true;
        }
        #endregion

        #region 自定义方法
        public virtual bool Contains(float x, float y)
        {
            return false;
        }
        public virtual double AreaValue()
        {
            return 0;
        }
        public virtual DragHandleAnchor HitTest(Point point)
        {
            return DragHandleCollection.HitTest(point);
        }
        internal virtual void Draw(Graphics g, ImageBox box)
        {
            return;
        }

        protected void DrawDragHandles(Graphics g)
        {
            if (DragHandleCollection != null && DragHandleCollection.Count > 0)
                foreach (var handle in DragHandleCollection)
                    DrawDragHandle(g, handle);
        }
        private void DrawDragHandle(Graphics graphics, DragHandle handle)
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

        internal virtual void OnROIShapeChannged(object sender, ElementEventArgs e)
        {
            ROIShapeChannged?.Invoke(sender, e);
        }
        #endregion

        #region 鼠标操作

        protected virtual DragHandleAnchor SetCursor(Point point, ImageBox box)
        {
            Cursor cursor;
            DragHandleAnchor handleAnchor;
            if (!Selected)
            {
                cursor = Cursors.Default;
                handleAnchor = DragHandleAnchor.None;
            }
            else
            {
                handleAnchor = HitTest(point);
                if (handleAnchor != DragHandleAnchor.None && DragHandleCollection[handleAnchor].Enabled)
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
                        case DragHandleAnchor.MiddleCenter:
                            cursor = Cursors.SizeAll;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    cursor = Cursors.Default;
                }
            }

            box.Cursor = cursor;
            return handleAnchor;
        }

        internal virtual void MouseDown(MouseEventArgs e, ImageBox box)
        {
        }

        internal virtual void MouseMove(MouseEventArgs e, ImageBox box)
        { }

        internal virtual void MouseUp(MouseEventArgs e, ImageBox box)
        { }

        public virtual void Dispose()
        {
            
        }

        #endregion
    }

    public enum ElementType
    {
        None = 0,
        Rectangle = 1,
        Circle = 2,
        Ellipse = 3,
        Line = 4,
        Point = 5,
        String = 6,
        Blob = 7,
        DotMatrix = 8,
        /// <summary>
        /// 带箭头的线
        /// </summary>
        LineCap = 9
    }

    public class ElementEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public RectangleF OldRegion { get; }
        public RectangleF NewRegion { get; }
        public float OldAngle { get; }
        public float NewAngle { get; }

        public ElementEventArgs() : this(RectangleF.Empty, RectangleF.Empty, 0f, 0f)
        { }
        public ElementEventArgs(RectangleF oldRegion, RectangleF newRegion, float oldAngle, float newAngle)
        {
            Cancel = false;
            OldRegion = oldRegion;
            NewRegion = newRegion;
            OldAngle = oldAngle;
            NewAngle = newAngle;
        }
    }
}
