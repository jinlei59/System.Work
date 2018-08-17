using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.UI.GLView;

namespace EmguCVImageBoxTestApp
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> img = null;
        public Form1()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdOpenLocalImage = new OpenFileDialog();
            ofdOpenLocalImage.Title = "请选择要打开的图片";
            ofdOpenLocalImage.Multiselect = true;
            ofdOpenLocalImage.Filter = "jpg图片|*.jpg|jpeg图片|*.jpeg|bmp图片|*.bmp|png图片|*.png|pdf文件|*.pdf";
            ofdOpenLocalImage.CheckFileExists = true;
            ofdOpenLocalImage.Multiselect = false;
            if (ofdOpenLocalImage.ShowDialog(this) == DialogResult.OK)
            {
                if (img != null)
                {
                    img.Dispose();
                    img = null;
                }
                img = new Image<Bgr, byte>(ofdOpenLocalImage.FileName);
                imageBox1.Image = img;

                //ImageViewer viewer = new ImageViewer();
                //viewer.Image = img;
                //viewer.Show();

                GLImageView gviewer = new GLImageView();
                gviewer.SetImage(img, new GeometricChange());
                gviewer.Show();
            }
            else
            {
                MessageBox.Show("你没有选择图片");
            }
        }
    }
}
