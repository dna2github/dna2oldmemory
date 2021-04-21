namespace Yiguanjia
{
    partial class frmMain
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
            this.txtComName = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtTmp = new System.Windows.Forms.TextBox();
            this.txtCmd = new System.Windows.Forms.ComboBox();
            this.tabData = new System.Windows.Forms.TabControl();
            this.tabPulsewave = new System.Windows.Forms.TabPage();
            this.tabAccupoint = new System.Windows.Forms.TabPage();
            this.btnAccuPointValue = new System.Windows.Forms.Button();
            this.tabData.SuspendLayout();
            this.tabAccupoint.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtComName
            // 
            this.txtComName.FormattingEnabled = true;
            this.txtComName.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9"});
            this.txtComName.Location = new System.Drawing.Point(12, 12);
            this.txtComName.Name = "txtComName";
            this.txtComName.Size = new System.Drawing.Size(284, 26);
            this.txtComName.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(302, 13);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(133, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(302, 45);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(133, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtTmp
            // 
            this.txtTmp.Location = new System.Drawing.Point(12, 76);
            this.txtTmp.Multiline = true;
            this.txtTmp.Name = "txtTmp";
            this.txtTmp.Size = new System.Drawing.Size(940, 84);
            this.txtTmp.TabIndex = 3;
            // 
            // txtCmd
            // 
            this.txtCmd.FormattingEnabled = true;
            this.txtCmd.Items.AddRange(new object[] {
            "00 Stop",
            "03 AccuPoint",
            "04 ???",
            "05 Temp",
            "09 ECG",
            "0A ???",
            "0C PulseWave"});
            this.txtCmd.Location = new System.Drawing.Point(12, 44);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(284, 26);
            this.txtCmd.TabIndex = 4;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.tabPulsewave);
            this.tabData.Controls.Add(this.tabAccupoint);
            this.tabData.Location = new System.Drawing.Point(12, 166);
            this.tabData.Name = "tabData";
            this.tabData.SelectedIndex = 0;
            this.tabData.Size = new System.Drawing.Size(940, 483);
            this.tabData.TabIndex = 5;
            // 
            // tabPulsewave
            // 
            this.tabPulsewave.Location = new System.Drawing.Point(4, 28);
            this.tabPulsewave.Name = "tabPulsewave";
            this.tabPulsewave.Padding = new System.Windows.Forms.Padding(3);
            this.tabPulsewave.Size = new System.Drawing.Size(932, 451);
            this.tabPulsewave.TabIndex = 0;
            this.tabPulsewave.Text = "PulseWave";
            this.tabPulsewave.UseVisualStyleBackColor = true;
            // 
            // tabAccupoint
            // 
            this.tabAccupoint.Controls.Add(this.btnAccuPointValue);
            this.tabAccupoint.Location = new System.Drawing.Point(4, 28);
            this.tabAccupoint.Name = "tabAccupoint";
            this.tabAccupoint.Padding = new System.Windows.Forms.Padding(3);
            this.tabAccupoint.Size = new System.Drawing.Size(932, 451);
            this.tabAccupoint.TabIndex = 1;
            this.tabAccupoint.Text = "Accupoint";
            this.tabAccupoint.UseVisualStyleBackColor = true;
            // 
            // btnAccuPointValue
            // 
            this.btnAccuPointValue.Location = new System.Drawing.Point(17, 18);
            this.btnAccuPointValue.Name = "btnAccuPointValue";
            this.btnAccuPointValue.Size = new System.Drawing.Size(387, 23);
            this.btnAccuPointValue.TabIndex = 0;
            this.btnAccuPointValue.UseVisualStyleBackColor = true;
            this.btnAccuPointValue.Click += new System.EventHandler(this.btnAccuPointValue_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 661);
            this.Controls.Add(this.tabData);
            this.Controls.Add(this.txtCmd);
            this.Controls.Add(this.txtTmp);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtComName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "YiGuanJia collector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabData.ResumeLayout(false);
            this.tabAccupoint.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox txtComName;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtTmp;
        private System.Windows.Forms.ComboBox txtCmd;
        private System.Windows.Forms.TabControl tabData;
        private System.Windows.Forms.TabPage tabPulsewave;
        private System.Windows.Forms.TabPage tabAccupoint;
        private System.Windows.Forms.Button btnAccuPointValue;
    }
}

