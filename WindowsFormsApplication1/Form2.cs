﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Work.ImageBoxLib;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        Bitmap _image = null;
        public Form2()
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
                        imageViewer1.DisplayImage(_image);
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
            imageViewer1.NewBeginDisplay();
            var el = new RoiRectangleElement() { IsRotation = false, IsDirection = true, Region = new RectangleF(100, 100, 300, 300), Angle = 30 };
            el.RegionChanged += El_RegionChanged;
            imageViewer1.NewAddRoiElements(new List<Element>() { el });
            imageViewer1.NewEndDisplay();
        }

        private void El_RegionChanged(object sender, ElementEventArgs e)
        {
            return;
            MessageBox.Show($"old:{e.OldRegion.ToString()}{Environment.NewLine}new:{e.NewRegion.ToString()}{Environment.NewLine}old angle:{e.OldAngle}{Environment.NewLine}new angle:{e.NewAngle}");
            e.Cancel = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageViewer1.AddString("你好", new Font("宋体", 16f), Brushes.Red, 100, 100);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageViewer1.AddRectangle(Pens.Yellow, 150, 150, 100, 100);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            imageViewer1.AddLine(Pens.LightBlue, 180, 180, 200, 200);

            imageViewer1.AddLine(Pens.LightBlue, 600, 600, 500, 345);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageViewer1.AddEllipse(Pens.Lime, 100, 400, 100, 50);

            imageViewer1.AddEllipse(Pens.Lime, 500, 500, 300, 300);
        }

        private void imageViewer1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            imageViewer1.AddBlobElement(new BlobElement(Color.Yellow, new PointF[]
            {
                new PointF(10,20),
                new PointF(60,60),
                new PointF(10,240),
                new PointF(100,340),
                new PointF(30,460),
                new PointF(500,500),
                new PointF(600,460),
                new PointF(700,380),
                new PointF(600,300),
                new PointF(500,200),
                new PointF(800,100),
                new PointF(20,10)
            }));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            imageViewer1.AddDotMatrixElement(new DotMatrixElement(Color.Red, new PointF[]
            {
                new PointF(10,20),
                new PointF(60,60),
                new PointF(10,240),
                new PointF(100,340),
                new PointF(30,460),
                new PointF(500,500),
                new PointF(600,460),
                new PointF(700,380),
                new PointF(600,300),
                new PointF(500,200),
                new PointF(800,100),
                new PointF(20,10)
            }, 50));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            imageViewer1.NewAddRoiElements(new List<Element>()
            {
                new RoiLineElement(66,888,321,555)
            });
            imageViewer1.NewEndDisplay();
        }
    }
}
