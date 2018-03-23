namespace System.Work.MVP.Startup.Krypton
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.kdWorkSpace = new ComponentFactory.Krypton.Docking.KryptonDockableWorkspace();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kdWorkSpace)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kdWorkSpace);
            this.kryptonPanel1.Controls.Add(this.menuStrip1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(768, 451);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(768, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // kdWorkSpace
            // 
            this.kdWorkSpace.AutoHiddenHost = false;
            this.kdWorkSpace.CompactFlags = ((ComponentFactory.Krypton.Workspace.CompactFlags)(((ComponentFactory.Krypton.Workspace.CompactFlags.RemoveEmptyCells | ComponentFactory.Krypton.Workspace.CompactFlags.RemoveEmptySequences) 
            | ComponentFactory.Krypton.Workspace.CompactFlags.PromoteLeafs)));
            this.kdWorkSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kdWorkSpace.Location = new System.Drawing.Point(0, 24);
            this.kdWorkSpace.Name = "kdWorkSpace";
            // 
            // 
            // 
            this.kdWorkSpace.Root.UniqueName = "3A830200AEC949721986DEB2E894678F";
            this.kdWorkSpace.Root.WorkspaceControl = this.kdWorkSpace;
            this.kdWorkSpace.ShowMaximizeButton = false;
            this.kdWorkSpace.Size = new System.Drawing.Size(768, 427);
            this.kdWorkSpace.TabIndex = 1;
            this.kdWorkSpace.TabStop = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 451);
            this.Controls.Add(this.kryptonPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "AOI通用软件";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kdWorkSpace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Windows.Forms.MenuStrip menuStrip1;
        private ComponentFactory.Krypton.Docking.KryptonDockableWorkspace kdWorkSpace;
    }
}