using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_ImageBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            imageViewer1.DrawString("ddd", new Font(FontFamily.Families.First(), 12), Brushes.Red, 100, 100);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            imageViewer1.DrawLine(Pens.Red, 100, 100, 200, 200);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageViewer1.DrawRectangle(Pens.Red, 300, 300, 600, 600);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageViewer1.DrawEllipse(Pens.Red, 1100, 1100, 100, 200);
        }
    }
}
