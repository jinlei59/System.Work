namespace System.Work.ImageBoxLib.ImageViewerCore
{
    partial class ImageViewer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.imageBox1 = new System.Work.ImageBoxLib.ImageBox();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.BackColor = System.Drawing.Color.Gray;
            this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox1.Location = new System.Drawing.Point(0, 0);
            this.imageBox1.Margin = new System.Windows.Forms.Padding(0);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.SelectionColor = System.Drawing.Color.Orange;
            this.imageBox1.Size = new System.Drawing.Size(468, 345);
            this.imageBox1.TabIndex = 0;
            this.imageBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseWheel);
            this.imageBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.imageBox1_Paint);
            this.imageBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseDown);
            this.imageBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseMove);
            this.imageBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseUp);
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(468, 345);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageBox imageBox1;
    }
}
