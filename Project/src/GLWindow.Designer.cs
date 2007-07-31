namespace monoCAM
{
    partial class GLWindow
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSTLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addGeometryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geoPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRandomGeoLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perspectiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orthographicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cAMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testCAMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GLPanel = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isinsideTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(558, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.addGeometryToolStripMenuItem,
            this.projectionToolStripMenuItem,
            this.cAMToolStripMenuItem,
            this.testsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(558, 26);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSTLToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 22);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openSTLToolStripMenuItem
            // 
            this.openSTLToolStripMenuItem.Name = "openSTLToolStripMenuItem";
            this.openSTLToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.openSTLToolStripMenuItem.Text = "Open STL";
            this.openSTLToolStripMenuItem.Click += new System.EventHandler(this.openSTLToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // addGeometryToolStripMenuItem
            // 
            this.addGeometryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geoPointToolStripMenuItem,
            this.addRandomGeoLineToolStripMenuItem});
            this.addGeometryToolStripMenuItem.Name = "addGeometryToolStripMenuItem";
            this.addGeometryToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.addGeometryToolStripMenuItem.Text = "Add Geometry";
            // 
            // geoPointToolStripMenuItem
            // 
            this.geoPointToolStripMenuItem.Name = "geoPointToolStripMenuItem";
            this.geoPointToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.geoPointToolStripMenuItem.Text = "Add random GeoPoint";
            this.geoPointToolStripMenuItem.Click += new System.EventHandler(this.geoPointToolStripMenuItem_Click);
            // 
            // addRandomGeoLineToolStripMenuItem
            // 
            this.addRandomGeoLineToolStripMenuItem.Name = "addRandomGeoLineToolStripMenuItem";
            this.addRandomGeoLineToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.addRandomGeoLineToolStripMenuItem.Text = "Add random GeoLine";
            this.addRandomGeoLineToolStripMenuItem.Click += new System.EventHandler(this.addRandomGeoLineToolStripMenuItem_Click);
            // 
            // projectionToolStripMenuItem
            // 
            this.projectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perspectiveToolStripMenuItem,
            this.orthographicToolStripMenuItem});
            this.projectionToolStripMenuItem.Name = "projectionToolStripMenuItem";
            this.projectionToolStripMenuItem.Size = new System.Drawing.Size(83, 22);
            this.projectionToolStripMenuItem.Text = "Projection";
            // 
            // perspectiveToolStripMenuItem
            // 
            this.perspectiveToolStripMenuItem.CheckOnClick = true;
            this.perspectiveToolStripMenuItem.Name = "perspectiveToolStripMenuItem";
            this.perspectiveToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.perspectiveToolStripMenuItem.Text = "Perspective";
            this.perspectiveToolStripMenuItem.Click += new System.EventHandler(this.perspectiveToolStripMenuItem_Click);
            // 
            // orthographicToolStripMenuItem
            // 
            this.orthographicToolStripMenuItem.CheckOnClick = true;
            this.orthographicToolStripMenuItem.Name = "orthographicToolStripMenuItem";
            this.orthographicToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.orthographicToolStripMenuItem.Text = "Orthographic";
            this.orthographicToolStripMenuItem.Click += new System.EventHandler(this.orthographicToolStripMenuItem_Click);
            // 
            // cAMToolStripMenuItem
            // 
            this.cAMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testCAMToolStripMenuItem});
            this.cAMToolStripMenuItem.Name = "cAMToolStripMenuItem";
            this.cAMToolStripMenuItem.Size = new System.Drawing.Size(50, 22);
            this.cAMToolStripMenuItem.Text = "CAM";
            // 
            // testCAMToolStripMenuItem
            // 
            this.testCAMToolStripMenuItem.Name = "testCAMToolStripMenuItem";
            this.testCAMToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.testCAMToolStripMenuItem.Text = "Test-CAM";
            this.testCAMToolStripMenuItem.Click += new System.EventHandler(this.testCAMToolStripMenuItem_Click);
            // 
            // GLPanel
            // 
            this.GLPanel.AccumBits = ((byte)(0));
            this.GLPanel.AutoCheckErrors = false;
            this.GLPanel.AutoFinish = false;
            this.GLPanel.AutoMakeCurrent = true;
            this.GLPanel.AutoSwapBuffers = true;
            this.GLPanel.BackColor = System.Drawing.Color.Black;
            this.GLPanel.ColorBits = ((byte)(32));
            this.GLPanel.DepthBits = ((byte)(16));
            this.GLPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLPanel.Location = new System.Drawing.Point(0, 0);
            this.GLPanel.Name = "GLPanel";
            this.GLPanel.Size = new System.Drawing.Size(558, 413);
            this.GLPanel.StencilBits = ((byte)(0));
            this.GLPanel.TabIndex = 3;
            this.GLPanel.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.GLPanel_MouseWheel);
            this.GLPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GLPanel_MouseDown);
            this.GLPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GLPanel_MouseMove);
            this.GLPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GLPanel_Paint);
            this.GLPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GLPanel_KeyDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Location = new System.Drawing.Point(3, 26);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(111, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.GLPanel);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(558, 413);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(558, 486);
            this.toolStripContainer1.TabIndex = 5;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.isinsideTestToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(57, 22);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // isinsideTestToolStripMenuItem
            // 
            this.isinsideTestToolStripMenuItem.Name = "isinsideTestToolStripMenuItem";
            this.isinsideTestToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.isinsideTestToolStripMenuItem.Text = "Isinside Test";
            this.isinsideTestToolStripMenuItem.Click += new System.EventHandler(this.isinsideTestToolStripMenuItem_Click);
            // 
            // GLWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 486);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GLWindow";
            this.Text = "GLWindow";
            this.Resize += new System.EventHandler(this.GLWindow_Resize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GLWindow_FormClosing);
            this.Load += new System.EventHandler(this.GLWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSTLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addGeometryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem geoPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRandomGeoLineToolStripMenuItem;
        private Tao.Platform.Windows.SimpleOpenGlControl GLPanel;
        private System.Windows.Forms.ToolStripMenuItem projectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perspectiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orthographicToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem cAMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testCAMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isinsideTestToolStripMenuItem;
    }
}