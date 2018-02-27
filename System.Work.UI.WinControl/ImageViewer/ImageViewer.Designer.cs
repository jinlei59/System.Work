namespace System.Work.UI.WinControl
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssRGB = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssImageInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsNormal = new System.Windows.Forms.ToolStripButton();
            this.tsZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageBox = new System.Work.UI.WinControl.ImageBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.tssLocation,
            this.toolStripStatusLabel1,
            this.tssRGB,
            this.tssImageInfo,
            this.tssZoom});
            this.statusStrip1.Location = new System.Drawing.Point(0, 340);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(447, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel2.Image = global::System.Work.UI.WinControl.Properties.Resources.Position;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // tssLocation
            // 
            this.tssLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssLocation.Image = global::System.Work.UI.WinControl.Properties.Resources.cursor;
            this.tssLocation.Name = "tssLocation";
            this.tssLocation.Size = new System.Drawing.Size(137, 17);
            this.tssLocation.Spring = true;
            this.tssLocation.Text = "X:0 Y:0";
            this.tssLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssRGB
            // 
            this.tssRGB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssRGB.Image = global::System.Work.UI.WinControl.Properties.Resources.map;
            this.tssRGB.Name = "tssRGB";
            this.tssRGB.Size = new System.Drawing.Size(137, 17);
            this.tssRGB.Spring = true;
            this.tssRGB.Text = "R:0 G:0 B:0";
            this.tssRGB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssImageInfo
            // 
            this.tssImageInfo.Image = global::System.Work.UI.WinControl.Properties.Resources.Size;
            this.tssImageInfo.Name = "tssImageInfo";
            this.tssImageInfo.Size = new System.Drawing.Size(69, 17);
            this.tssImageInfo.Text = "W:0 H:0";
            this.tssImageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsOpen,
            this.toolStripSeparator1,
            this.tsNormal,
            this.tsZoomIn,
            this.tsZoomOut});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(447, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsOpen
            // 
            this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsOpen.Image = global::System.Work.UI.WinControl.Properties.Resources.Open;
            this.tsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsOpen.Name = "tsOpen";
            this.tsOpen.Size = new System.Drawing.Size(23, 22);
            this.tsOpen.Text = "toolStripButton1";
            this.tsOpen.Click += new System.EventHandler(this.tsOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsNormal
            // 
            this.tsNormal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNormal.Image = global::System.Work.UI.WinControl.Properties.Resources.image_resize_actual;
            this.tsNormal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNormal.Name = "tsNormal";
            this.tsNormal.Size = new System.Drawing.Size(23, 22);
            this.tsNormal.Text = "toolStripButton2";
            this.tsNormal.Click += new System.EventHandler(this.tsNormal_Click);
            // 
            // tsZoomIn
            // 
            this.tsZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsZoomIn.Image = global::System.Work.UI.WinControl.Properties.Resources.magnifier_zoom_in;
            this.tsZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsZoomIn.Name = "tsZoomIn";
            this.tsZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tsZoomIn.Text = "toolStripButton3";
            this.tsZoomIn.Click += new System.EventHandler(this.tsZoomIn_Click);
            // 
            // tsZoomOut
            // 
            this.tsZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsZoomOut.Image = global::System.Work.UI.WinControl.Properties.Resources.magnifier_zoom_out;
            this.tsZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsZoomOut.Name = "tsZoomOut";
            this.tsZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tsZoomOut.Text = "toolStripButton4";
            this.tsZoomOut.Click += new System.EventHandler(this.tsZoomOut_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel1.Image = global::System.Work.UI.WinControl.Properties.Resources.map;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // tssZoom
            // 
            this.tssZoom.Image = global::System.Work.UI.WinControl.Properties.Resources.magnifier_zoom;
            this.tssZoom.Name = "tssZoom";
            this.tssZoom.Size = new System.Drawing.Size(56, 17);
            this.tssZoom.Text = "100%";
            // 
            // imageBox
            // 
            this.imageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox.Location = new System.Drawing.Point(0, 0);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(447, 340);
            this.imageBox.TabIndex = 3;
            this.imageBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.imageBox_MouseWheel);
            this.imageBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox_MouseDown);
            this.imageBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBox_MouseMove);
            this.imageBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageBox_MouseUp);
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.imageBox);
            this.Controls.Add(this.statusStrip1);
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(447, 362);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Windows.Forms.StatusStrip statusStrip1;
        private ImageBox imageBox;
        private Windows.Forms.ToolStrip toolStrip1;
        private Windows.Forms.ToolStripButton tsOpen;
        private Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Windows.Forms.ToolStripButton tsNormal;
        private Windows.Forms.ToolStripButton tsZoomIn;
        private Windows.Forms.ToolStripButton tsZoomOut;
        private Windows.Forms.ToolStripStatusLabel tssLocation;
        private Windows.Forms.ToolStripStatusLabel tssRGB;
        private Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private Windows.Forms.ToolStripStatusLabel tssImageInfo;
        private Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Windows.Forms.ToolStripStatusLabel tssZoom;
    }
}
