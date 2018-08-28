namespace System.Work.ImageBoxLib
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewer));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssXY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssRGB = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssWH = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsOpen = new System.Windows.Forms.ToolStripButton();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsZoomFit = new System.Windows.Forms.ToolStripButton();
            this.tsActualSize = new System.Windows.Forms.ToolStripButton();
            this.tsExpand = new System.Windows.Forms.ToolStripButton();
            this.tsShrink = new System.Windows.Forms.ToolStripButton();
            this.imageBox1 = new System.Work.ImageBoxLib.ImageBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel5,
            this.tssXY,
            this.toolStripStatusLabel4,
            this.tssRGB,
            this.tssWH});
            this.statusStrip1.Location = new System.Drawing.Point(0, 323);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(468, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel5.Image")));
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(16, 17);
            // 
            // tssXY
            // 
            this.tssXY.Name = "tssXY";
            this.tssXY.Size = new System.Drawing.Size(176, 17);
            this.tssXY.Spring = true;
            this.tssXY.Text = "X:0 Y:0";
            this.tssXY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Image = global::System.Work.ImageBoxLib.Properties.Resources.map;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(16, 17);
            // 
            // tssRGB
            // 
            this.tssRGB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssRGB.Name = "tssRGB";
            this.tssRGB.Size = new System.Drawing.Size(176, 17);
            this.tssRGB.Spring = true;
            this.tssRGB.Text = "R:0 G:0 B:0";
            this.tssRGB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssWH
            // 
            this.tssWH.Image = ((System.Drawing.Image)(resources.GetObject("tssWH.Image")));
            this.tssWH.Name = "tssWH";
            this.tssWH.Size = new System.Drawing.Size(69, 17);
            this.tssWH.Text = "W:0 H:0";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsOpen,
            this.tsSave,
            this.toolStripSeparator1,
            this.tsZoomFit,
            this.tsActualSize,
            this.tsExpand,
            this.tsShrink});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(468, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsOpen
            // 
            this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsOpen.Image = global::System.Work.ImageBoxLib.Properties.Resources.Open;
            this.tsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsOpen.Name = "tsOpen";
            this.tsOpen.Size = new System.Drawing.Size(23, 22);
            this.tsOpen.Text = "打开";
            this.tsOpen.Click += new System.EventHandler(this.tsOpen_Click);
            // 
            // tsSave
            // 
            this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSave.Image = global::System.Work.ImageBoxLib.Properties.Resources.save;
            this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSave.Name = "tsSave";
            this.tsSave.Size = new System.Drawing.Size(23, 22);
            this.tsSave.Text = "保存";
            this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsZoomFit
            // 
            this.tsZoomFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsZoomFit.Image = global::System.Work.ImageBoxLib.Properties.Resources.image_resize_actual;
            this.tsZoomFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsZoomFit.Name = "tsZoomFit";
            this.tsZoomFit.Size = new System.Drawing.Size(23, 22);
            this.tsZoomFit.Text = "自适应";
            this.tsZoomFit.Click += new System.EventHandler(this.tsZoomFit_Click);
            // 
            // tsActualSize
            // 
            this.tsActualSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsActualSize.Image = global::System.Work.ImageBoxLib.Properties.Resources.magnifier_zoom;
            this.tsActualSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsActualSize.Name = "tsActualSize";
            this.tsActualSize.Size = new System.Drawing.Size(23, 22);
            this.tsActualSize.Text = "1：1";
            this.tsActualSize.Click += new System.EventHandler(this.tsActualSize_Click);
            // 
            // tsExpand
            // 
            this.tsExpand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsExpand.Image = global::System.Work.ImageBoxLib.Properties.Resources.magnifier_zoom_in;
            this.tsExpand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsExpand.Name = "tsExpand";
            this.tsExpand.Size = new System.Drawing.Size(23, 22);
            this.tsExpand.Text = "放大";
            this.tsExpand.Click += new System.EventHandler(this.tsExpand_Click);
            // 
            // tsShrink
            // 
            this.tsShrink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsShrink.Image = global::System.Work.ImageBoxLib.Properties.Resources.magnifier_zoom_out;
            this.tsShrink.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsShrink.Name = "tsShrink";
            this.tsShrink.Size = new System.Drawing.Size(23, 22);
            this.tsShrink.Text = "缩小";
            this.tsShrink.Click += new System.EventHandler(this.tsShrink_Click);
            // 
            // imageBox1
            // 
            this.imageBox1.BackColor = System.Drawing.Color.Gray;
            this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox1.Location = new System.Drawing.Point(0, 25);
            this.imageBox1.Margin = new System.Windows.Forms.Padding(0);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.SelectionColor = System.Drawing.Color.Orange;
            this.imageBox1.Size = new System.Drawing.Size(468, 298);
            this.imageBox1.TabIndex = 0;
            this.imageBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseWheel);
            this.imageBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.imageBox1_Paint);
            this.imageBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseDoubleClick);
            this.imageBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseDown);
            this.imageBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseMove);
            this.imageBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseUp);
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(468, 345);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageBox imageBox1;
        private Windows.Forms.StatusStrip statusStrip1;
        private Windows.Forms.ToolStrip toolStrip1;
        private Windows.Forms.ToolStripButton tsZoomFit;
        private Windows.Forms.ToolStripButton tsActualSize;
        private Windows.Forms.ToolStripButton tsExpand;
        private Windows.Forms.ToolStripButton tsShrink;
        private Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Windows.Forms.ToolStripButton tsOpen;
        private Windows.Forms.ToolStripButton tsSave;
        private Windows.Forms.ToolStripStatusLabel tssXY;
        private Windows.Forms.ToolStripStatusLabel tssRGB;
        private Windows.Forms.ToolStripStatusLabel tssWH;
        private Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
    }
}
