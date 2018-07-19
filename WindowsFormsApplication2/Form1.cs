using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.ImageBoxLib;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        Bitmap _image = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "All Supported Images (*.bmp;*.dib;*.rle;*.gif;*.jpg;*.png)|*.bmp;*.dib;*.rle;*.gif;*.jpg;*.png|Bitmaps (*.bmp;*.dib;*.rle)|*.bmp;*.dib;*.rle|Graphics Interchange Format (*.gif)|*.gif|Joint Photographic Experts (*.jpg)|*.jpg|Portable Network Graphics (*.png)|*.png|All Files (*.*)|*.*";
                dialog.DefaultExt = "png";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        if (_image != null)
                            _image.Dispose();
                        _image = null;
                        _image = new Bitmap(dialog.FileName);
                        imageViewer1.NewDisplayImage(_image);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //imageViewer1.NewAddOtherElements(new List<System.Work.ImageBoxLib.Element>() { new RectElement(new RectangleF(10, 10, 200, 200), 30) });
            imageViewer1.NewAddRoiElements(new List<Element>() { new RoiRectElement(new RectangleF(10, 10, 200, 200)) });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //imageViewer1.NewAddOtherElements(new List<System.Work.ImageBoxLib.Element>() { new LineElement(new Point(20, 20), new PointF(200, 200)) { BorderWidth=3} });
            imageViewer1.NewAddRoiElements(new List<System.Work.ImageBoxLib.Element>() { new RoiLineElement(new Point(0, 0), new PointF(100, 100)) { BorderWidth = 1 } });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageViewer1.NewAddOtherElements(new List<System.Work.ImageBoxLib.Element>() { new RectLineElement(new RectangleF(10, 10, 200, 200), 0) { LineCount = 4 } });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            imageViewer1.NewAddRoiElements(new List<System.Work.ImageBoxLib.Element>() { new RoiDispersePointLineElement(new Point(100, 100), new PointF(100, 500)) { BorderWidth = 1, DispersePointCount = 4 } });
            imageViewer1.NewAddRoiElements(new List<System.Work.ImageBoxLib.Element>() { new RoiDispersePointLineElement(new Point(100, 100), new PointF(500, 100)) { BorderWidth = 1, DispersePointCount = 4 } });
        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageViewer1.NewAddOtherElements(new List<System.Work.ImageBoxLib.Element>() { new BlobElement(new PointF[] { new PointF(100, 100), new PointF(100, 200), new PointF(200, 200), new PointF(200, 100) }) { ForeColor = Color.Yellow } });
        }

        private void button7_Click(object sender, EventArgs e)
        {
            imageViewer1.NewAddOtherElements(new List<System.Work.ImageBoxLib.Element>() { new DotMatrixElement(new PointF[] { new PointF(10, 10), new PointF(10, 20), new PointF(20, 10), new PointF(20, 20) }) });
        }

        private void button8_Click(object sender, EventArgs e)
        {
            imageViewer1.NewAddOtherElements(new List<Element>() { new RingElement(new PointF(400, 400), 50f, 100f) });
        }

        private void button9_Click(object sender, EventArgs e)
        {
            imageViewer1.NewAddRoiElements(new List<Element>() { new RoiRingElement(new PointF(400, 400), 50f, 100f) { ForeColor = Color.Yellow } });
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //imageViewer1.NewAddOtherElements(new List<Element>() { new PolygonElement() { ForeColor = Color.Blue } });
            imageViewer1.NewAddRoiElements(new List<Element>() { new RoiPolygonElement() { ForeColor = Color.Blue } });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //imageViewer1.NewAddRoiElements(new List<Element>() { new PointElement(110, 110) { ForeColor = Color.Blue } });
            imageViewer1.NewAddRoiElements(new List<Element>() { new RoiPointElement(110, 110) { ForeColor = Color.Blue } });
        }
    }
}
