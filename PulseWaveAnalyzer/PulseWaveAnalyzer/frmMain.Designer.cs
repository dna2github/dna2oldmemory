namespace PulseWaveAnalyzer
{
    partial class frmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.picView = new System.Windows.Forms.PictureBox();
            this.picF = new System.Windows.Forms.PictureBox();
            this.dlg = new System.Windows.Forms.OpenFileDialog();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picF)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpen,
            this.menuLibrary});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(703, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // menuOpen
            // 
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.Size = new System.Drawing.Size(59, 20);
            this.menuOpen.Text = "打开(&O)";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // menuLibrary
            // 
            this.menuLibrary.Name = "menuLibrary";
            this.menuLibrary.Size = new System.Drawing.Size(71, 20);
            this.menuLibrary.Text = "数据库(&L)";
            this.menuLibrary.Click += new System.EventHandler(this.menuLibrary_Click);
            // 
            // picView
            // 
            this.picView.BackColor = System.Drawing.Color.White;
            this.picView.Location = new System.Drawing.Point(0, 27);
            this.picView.Name = "picView";
            this.picView.Size = new System.Drawing.Size(703, 145);
            this.picView.TabIndex = 4;
            this.picView.TabStop = false;
            this.picView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picView_MouseDown);
            // 
            // picF
            // 
            this.picF.BackColor = System.Drawing.Color.White;
            this.picF.Location = new System.Drawing.Point(0, 178);
            this.picF.Name = "picF";
            this.picF.Size = new System.Drawing.Size(703, 117);
            this.picF.TabIndex = 7;
            this.picF.TabStop = false;
            // 
            // dlg
            // 
            this.dlg.Filter = "Pulse Wave Raw Data|*.dat";
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtResult.Location = new System.Drawing.Point(0, 301);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(703, 159);
            this.txtResult.TabIndex = 9;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 465);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.picF);
            this.Controls.Add(this.picView);
            this.Controls.Add(this.menuMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Pulse Wave";
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picF)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.PictureBox picView;
        private System.Windows.Forms.PictureBox picF;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.OpenFileDialog dlg;
        private System.Windows.Forms.ToolStripMenuItem menuLibrary;
        private System.Windows.Forms.TextBox txtResult;

    }
}

