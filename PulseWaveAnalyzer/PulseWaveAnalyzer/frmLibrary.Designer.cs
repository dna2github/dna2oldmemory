namespace PulseWaveAnalyzer
{
    partial class frmLibrary
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLibrary = new System.Windows.Forms.TabPage();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnmOK = new System.Windows.Forms.Button();
            this.btnmNew = new System.Windows.Forms.Button();
            this.txtmName = new System.Windows.Forms.TextBox();
            this.gbmMerdian = new System.Windows.Forms.GroupBox();
            this.cbPC = new System.Windows.Forms.CheckBox();
            this.cbSI = new System.Windows.Forms.CheckBox();
            this.cbTB = new System.Windows.Forms.CheckBox();
            this.cbLI = new System.Windows.Forms.CheckBox();
            this.cbBL = new System.Windows.Forms.CheckBox();
            this.cbGB = new System.Windows.Forms.CheckBox();
            this.cbST = new System.Windows.Forms.CheckBox();
            this.cbLU = new System.Windows.Forms.CheckBox();
            this.cbSP = new System.Windows.Forms.CheckBox();
            this.cbKI = new System.Windows.Forms.CheckBox();
            this.cbLR = new System.Windows.Forms.CheckBox();
            this.cbHT = new System.Windows.Forms.CheckBox();
            this.gbmMild = new System.Windows.Forms.GroupBox();
            this.rbWarm = new System.Windows.Forms.RadioButton();
            this.rbCool = new System.Windows.Forms.RadioButton();
            this.rbHot = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbCold = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.mlist = new System.Windows.Forms.ListBox();
            this.txtmSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtpName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnpCancel = new System.Windows.Forms.Button();
            this.btnpOK = new System.Windows.Forms.Button();
            this.btnpNew = new System.Windows.Forms.Button();
            this.txtpmSearch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.numpmWeight = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.pplist = new System.Windows.Forms.ListBox();
            this.pmlist = new System.Windows.Forms.ListBox();
            this.picpVal = new System.Windows.Forms.PictureBox();
            this.plist = new System.Windows.Forms.ListBox();
            this.txtpSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabLibrary.SuspendLayout();
            this.gbmMerdian.SuspendLayout();
            this.gbmMild.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numpmWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picpVal)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLibrary);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(453, 308);
            this.tabControl1.TabIndex = 0;
            // 
            // tabLibrary
            // 
            this.tabLibrary.Controls.Add(this.btnCancel);
            this.tabLibrary.Controls.Add(this.btnmOK);
            this.tabLibrary.Controls.Add(this.btnmNew);
            this.tabLibrary.Controls.Add(this.txtmName);
            this.tabLibrary.Controls.Add(this.gbmMerdian);
            this.tabLibrary.Controls.Add(this.gbmMild);
            this.tabLibrary.Controls.Add(this.label2);
            this.tabLibrary.Controls.Add(this.mlist);
            this.tabLibrary.Controls.Add(this.txtmSearch);
            this.tabLibrary.Controls.Add(this.label1);
            this.tabLibrary.Location = new System.Drawing.Point(4, 22);
            this.tabLibrary.Name = "tabLibrary";
            this.tabLibrary.Padding = new System.Windows.Forms.Padding(3);
            this.tabLibrary.Size = new System.Drawing.Size(445, 282);
            this.tabLibrary.TabIndex = 0;
            this.tabLibrary.Text = "中药库";
            this.tabLibrary.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(364, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 41);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnmOK
            // 
            this.btnmOK.Location = new System.Drawing.Point(283, 6);
            this.btnmOK.Name = "btnmOK";
            this.btnmOK.Size = new System.Drawing.Size(75, 41);
            this.btnmOK.TabIndex = 8;
            this.btnmOK.Text = "确定(&O)";
            this.btnmOK.UseVisualStyleBackColor = true;
            this.btnmOK.Click += new System.EventHandler(this.btnmOK_Click);
            // 
            // btnmNew
            // 
            this.btnmNew.Location = new System.Drawing.Point(202, 6);
            this.btnmNew.Name = "btnmNew";
            this.btnmNew.Size = new System.Drawing.Size(75, 41);
            this.btnmNew.TabIndex = 7;
            this.btnmNew.Text = "新药物(&N)";
            this.btnmNew.UseVisualStyleBackColor = true;
            this.btnmNew.Click += new System.EventHandler(this.btnmNew_Click);
            // 
            // txtmName
            // 
            this.txtmName.Location = new System.Drawing.Point(204, 53);
            this.txtmName.Name = "txtmName";
            this.txtmName.Size = new System.Drawing.Size(235, 21);
            this.txtmName.TabIndex = 6;
            // 
            // gbmMerdian
            // 
            this.gbmMerdian.Controls.Add(this.cbPC);
            this.gbmMerdian.Controls.Add(this.cbSI);
            this.gbmMerdian.Controls.Add(this.cbTB);
            this.gbmMerdian.Controls.Add(this.cbLI);
            this.gbmMerdian.Controls.Add(this.cbBL);
            this.gbmMerdian.Controls.Add(this.cbGB);
            this.gbmMerdian.Controls.Add(this.cbST);
            this.gbmMerdian.Controls.Add(this.cbLU);
            this.gbmMerdian.Controls.Add(this.cbSP);
            this.gbmMerdian.Controls.Add(this.cbKI);
            this.gbmMerdian.Controls.Add(this.cbLR);
            this.gbmMerdian.Controls.Add(this.cbHT);
            this.gbmMerdian.Location = new System.Drawing.Point(153, 129);
            this.gbmMerdian.Name = "gbmMerdian";
            this.gbmMerdian.Size = new System.Drawing.Size(286, 142);
            this.gbmMerdian.TabIndex = 5;
            this.gbmMerdian.TabStop = false;
            this.gbmMerdian.Text = "归经";
            // 
            // cbPC
            // 
            this.cbPC.AutoSize = true;
            this.cbPC.Location = new System.Drawing.Point(196, 113);
            this.cbPC.Name = "cbPC";
            this.cbPC.Size = new System.Drawing.Size(60, 16);
            this.cbPC.TabIndex = 11;
            this.cbPC.Text = "心包经";
            this.cbPC.UseVisualStyleBackColor = true;
            // 
            // cbSI
            // 
            this.cbSI.AutoSize = true;
            this.cbSI.Location = new System.Drawing.Point(106, 113);
            this.cbSI.Name = "cbSI";
            this.cbSI.Size = new System.Drawing.Size(60, 16);
            this.cbSI.TabIndex = 10;
            this.cbSI.Text = "小肠经";
            this.cbSI.UseVisualStyleBackColor = true;
            // 
            // cbTB
            // 
            this.cbTB.AutoSize = true;
            this.cbTB.Location = new System.Drawing.Point(19, 113);
            this.cbTB.Name = "cbTB";
            this.cbTB.Size = new System.Drawing.Size(60, 16);
            this.cbTB.TabIndex = 9;
            this.cbTB.Text = "三焦经";
            this.cbTB.UseVisualStyleBackColor = true;
            // 
            // cbLI
            // 
            this.cbLI.AutoSize = true;
            this.cbLI.Location = new System.Drawing.Point(196, 84);
            this.cbLI.Name = "cbLI";
            this.cbLI.Size = new System.Drawing.Size(60, 16);
            this.cbLI.TabIndex = 8;
            this.cbLI.Text = "大肠经";
            this.cbLI.UseVisualStyleBackColor = true;
            // 
            // cbBL
            // 
            this.cbBL.AutoSize = true;
            this.cbBL.Location = new System.Drawing.Point(106, 84);
            this.cbBL.Name = "cbBL";
            this.cbBL.Size = new System.Drawing.Size(60, 16);
            this.cbBL.TabIndex = 7;
            this.cbBL.Text = "膀胱经";
            this.cbBL.UseVisualStyleBackColor = true;
            // 
            // cbGB
            // 
            this.cbGB.AutoSize = true;
            this.cbGB.Location = new System.Drawing.Point(19, 84);
            this.cbGB.Name = "cbGB";
            this.cbGB.Size = new System.Drawing.Size(48, 16);
            this.cbGB.TabIndex = 6;
            this.cbGB.Text = "胆经";
            this.cbGB.UseVisualStyleBackColor = true;
            // 
            // cbST
            // 
            this.cbST.AutoSize = true;
            this.cbST.Location = new System.Drawing.Point(196, 55);
            this.cbST.Name = "cbST";
            this.cbST.Size = new System.Drawing.Size(48, 16);
            this.cbST.TabIndex = 5;
            this.cbST.Text = "胃经";
            this.cbST.UseVisualStyleBackColor = true;
            // 
            // cbLU
            // 
            this.cbLU.AutoSize = true;
            this.cbLU.Location = new System.Drawing.Point(106, 55);
            this.cbLU.Name = "cbLU";
            this.cbLU.Size = new System.Drawing.Size(48, 16);
            this.cbLU.TabIndex = 4;
            this.cbLU.Text = "肺经";
            this.cbLU.UseVisualStyleBackColor = true;
            // 
            // cbSP
            // 
            this.cbSP.AutoSize = true;
            this.cbSP.Location = new System.Drawing.Point(19, 55);
            this.cbSP.Name = "cbSP";
            this.cbSP.Size = new System.Drawing.Size(48, 16);
            this.cbSP.TabIndex = 3;
            this.cbSP.Text = "脾经";
            this.cbSP.UseVisualStyleBackColor = true;
            // 
            // cbKI
            // 
            this.cbKI.AutoSize = true;
            this.cbKI.Location = new System.Drawing.Point(196, 26);
            this.cbKI.Name = "cbKI";
            this.cbKI.Size = new System.Drawing.Size(48, 16);
            this.cbKI.TabIndex = 2;
            this.cbKI.Text = "肾经";
            this.cbKI.UseVisualStyleBackColor = true;
            // 
            // cbLR
            // 
            this.cbLR.AutoSize = true;
            this.cbLR.Location = new System.Drawing.Point(106, 26);
            this.cbLR.Name = "cbLR";
            this.cbLR.Size = new System.Drawing.Size(48, 16);
            this.cbLR.TabIndex = 1;
            this.cbLR.Text = "肝经";
            this.cbLR.UseVisualStyleBackColor = true;
            // 
            // cbHT
            // 
            this.cbHT.AutoSize = true;
            this.cbHT.Location = new System.Drawing.Point(19, 26);
            this.cbHT.Name = "cbHT";
            this.cbHT.Size = new System.Drawing.Size(48, 16);
            this.cbHT.TabIndex = 0;
            this.cbHT.Text = "心经";
            this.cbHT.UseVisualStyleBackColor = true;
            // 
            // gbmMild
            // 
            this.gbmMild.Controls.Add(this.rbWarm);
            this.gbmMild.Controls.Add(this.rbCool);
            this.gbmMild.Controls.Add(this.rbHot);
            this.gbmMild.Controls.Add(this.rbNone);
            this.gbmMild.Controls.Add(this.rbCold);
            this.gbmMild.Location = new System.Drawing.Point(156, 76);
            this.gbmMild.Name = "gbmMild";
            this.gbmMild.Size = new System.Drawing.Size(286, 47);
            this.gbmMild.TabIndex = 4;
            this.gbmMild.TabStop = false;
            this.gbmMild.Text = "药性";
            // 
            // rbWarm
            // 
            this.rbWarm.AutoSize = true;
            this.rbWarm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rbWarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.rbWarm.Location = new System.Drawing.Point(171, 20);
            this.rbWarm.Name = "rbWarm";
            this.rbWarm.Size = new System.Drawing.Size(46, 16);
            this.rbWarm.TabIndex = 3;
            this.rbWarm.Text = "偏温";
            this.rbWarm.UseVisualStyleBackColor = true;
            // 
            // rbCool
            // 
            this.rbCool.AutoSize = true;
            this.rbCool.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rbCool.ForeColor = System.Drawing.Color.Cyan;
            this.rbCool.Location = new System.Drawing.Point(67, 20);
            this.rbCool.Name = "rbCool";
            this.rbCool.Size = new System.Drawing.Size(46, 16);
            this.rbCool.TabIndex = 3;
            this.rbCool.Text = "偏凉";
            this.rbCool.UseVisualStyleBackColor = true;
            // 
            // rbHot
            // 
            this.rbHot.AutoSize = true;
            this.rbHot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rbHot.ForeColor = System.Drawing.Color.Red;
            this.rbHot.Location = new System.Drawing.Point(231, 20);
            this.rbHot.Name = "rbHot";
            this.rbHot.Size = new System.Drawing.Size(46, 16);
            this.rbHot.TabIndex = 2;
            this.rbHot.Text = "偏热";
            this.rbHot.UseVisualStyleBackColor = true;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rbNone.Location = new System.Drawing.Point(119, 20);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(46, 16);
            this.rbNone.TabIndex = 1;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "中性";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rbCold
            // 
            this.rbCold.AutoSize = true;
            this.rbCold.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rbCold.ForeColor = System.Drawing.Color.Blue;
            this.rbCold.Location = new System.Drawing.Point(16, 20);
            this.rbCold.Name = "rbCold";
            this.rbCold.Size = new System.Drawing.Size(46, 16);
            this.rbCold.TabIndex = 0;
            this.rbCold.Text = "偏寒";
            this.rbCold.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "名称:";
            // 
            // mlist
            // 
            this.mlist.FormattingEnabled = true;
            this.mlist.ItemHeight = 12;
            this.mlist.Location = new System.Drawing.Point(8, 39);
            this.mlist.Name = "mlist";
            this.mlist.Size = new System.Drawing.Size(139, 232);
            this.mlist.TabIndex = 2;
            this.mlist.SelectedIndexChanged += new System.EventHandler(this.mlist_SelectedIndexChanged);
            // 
            // txtmSearch
            // 
            this.txtmSearch.Location = new System.Drawing.Point(47, 9);
            this.txtmSearch.Name = "txtmSearch";
            this.txtmSearch.Size = new System.Drawing.Size(100, 21);
            this.txtmSearch.TabIndex = 1;
            this.txtmSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtmSearch_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "搜索:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtpName);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.btnpCancel);
            this.tabPage2.Controls.Add(this.btnpOK);
            this.tabPage2.Controls.Add(this.btnpNew);
            this.tabPage2.Controls.Add(this.txtpmSearch);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.btnDel);
            this.tabPage2.Controls.Add(this.btnAdd);
            this.tabPage2.Controls.Add(this.numpmWeight);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.pplist);
            this.tabPage2.Controls.Add(this.pmlist);
            this.tabPage2.Controls.Add(this.picpVal);
            this.tabPage2.Controls.Add(this.plist);
            this.tabPage2.Controls.Add(this.txtpSearch);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(445, 282);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "方剂库";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtpName
            // 
            this.txtpName.Location = new System.Drawing.Point(194, 50);
            this.txtpName.Name = "txtpName";
            this.txtpName.Size = new System.Drawing.Size(244, 21);
            this.txtpName.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(157, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "名称:";
            // 
            // btnpCancel
            // 
            this.btnpCancel.Location = new System.Drawing.Point(364, 6);
            this.btnpCancel.Name = "btnpCancel";
            this.btnpCancel.Size = new System.Drawing.Size(75, 41);
            this.btnpCancel.TabIndex = 18;
            this.btnpCancel.Text = "取消(&C)";
            this.btnpCancel.UseVisualStyleBackColor = true;
            this.btnpCancel.Click += new System.EventHandler(this.btnpCancel_Click);
            // 
            // btnpOK
            // 
            this.btnpOK.Location = new System.Drawing.Point(283, 6);
            this.btnpOK.Name = "btnpOK";
            this.btnpOK.Size = new System.Drawing.Size(75, 41);
            this.btnpOK.TabIndex = 17;
            this.btnpOK.Text = "确定(&O)";
            this.btnpOK.UseVisualStyleBackColor = true;
            this.btnpOK.Click += new System.EventHandler(this.btnpOK_Click);
            // 
            // btnpNew
            // 
            this.btnpNew.Location = new System.Drawing.Point(202, 6);
            this.btnpNew.Name = "btnpNew";
            this.btnpNew.Size = new System.Drawing.Size(75, 41);
            this.btnpNew.TabIndex = 16;
            this.btnpNew.Text = "新方剂(&N)";
            this.btnpNew.UseVisualStyleBackColor = true;
            this.btnpNew.Click += new System.EventHandler(this.btnpNew_Click);
            // 
            // txtpmSearch
            // 
            this.txtpmSearch.Location = new System.Drawing.Point(194, 100);
            this.txtpmSearch.Name = "txtpmSearch";
            this.txtpmSearch.Size = new System.Drawing.Size(83, 21);
            this.txtpmSearch.TabIndex = 15;
            this.txtpmSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtpmSearch_KeyUp);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(158, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "搜索:";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(283, 147);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(26, 64);
            this.btnDel.TabIndex = 13;
            this.btnDel.Text = "<";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(283, 77);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(26, 64);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // numpmWeight
            // 
            this.numpmWeight.DecimalPlaces = 1;
            this.numpmWeight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numpmWeight.Location = new System.Drawing.Point(194, 77);
            this.numpmWeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numpmWeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numpmWeight.Name = "numpmWeight";
            this.numpmWeight.Size = new System.Drawing.Size(70, 21);
            this.numpmWeight.TabIndex = 11;
            this.numpmWeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(158, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "剂量:             g";
            // 
            // pplist
            // 
            this.pplist.FormattingEnabled = true;
            this.pplist.ItemHeight = 12;
            this.pplist.Location = new System.Drawing.Point(315, 76);
            this.pplist.Name = "pplist";
            this.pplist.Size = new System.Drawing.Size(124, 136);
            this.pplist.TabIndex = 9;
            // 
            // pmlist
            // 
            this.pmlist.FormattingEnabled = true;
            this.pmlist.ItemHeight = 12;
            this.pmlist.Location = new System.Drawing.Point(157, 124);
            this.pmlist.Name = "pmlist";
            this.pmlist.Size = new System.Drawing.Size(120, 88);
            this.pmlist.TabIndex = 8;
            // 
            // picpVal
            // 
            this.picpVal.BackColor = System.Drawing.Color.White;
            this.picpVal.Location = new System.Drawing.Point(160, 218);
            this.picpVal.Name = "picpVal";
            this.picpVal.Size = new System.Drawing.Size(276, 53);
            this.picpVal.TabIndex = 6;
            this.picpVal.TabStop = false;
            // 
            // plist
            // 
            this.plist.FormattingEnabled = true;
            this.plist.ItemHeight = 12;
            this.plist.Location = new System.Drawing.Point(8, 39);
            this.plist.Name = "plist";
            this.plist.Size = new System.Drawing.Size(139, 232);
            this.plist.TabIndex = 5;
            this.plist.SelectedIndexChanged += new System.EventHandler(this.plist_SelectedIndexChanged);
            // 
            // txtpSearch
            // 
            this.txtpSearch.Location = new System.Drawing.Point(47, 9);
            this.txtpSearch.Name = "txtpSearch";
            this.txtpSearch.Size = new System.Drawing.Size(100, 21);
            this.txtpSearch.TabIndex = 4;
            this.txtpSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtpSearch_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "搜索:";
            // 
            // frmLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 337);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLibrary";
            this.Text = "数据库";
            this.tabControl1.ResumeLayout(false);
            this.tabLibrary.ResumeLayout(false);
            this.tabLibrary.PerformLayout();
            this.gbmMerdian.ResumeLayout(false);
            this.gbmMerdian.PerformLayout();
            this.gbmMild.ResumeLayout(false);
            this.gbmMild.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numpmWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picpVal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLibrary;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox mlist;
        private System.Windows.Forms.TextBox txtmSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbmMerdian;
        private System.Windows.Forms.GroupBox gbmMild;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtmName;
        private System.Windows.Forms.RadioButton rbHot;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbCold;
        private System.Windows.Forms.CheckBox cbPC;
        private System.Windows.Forms.CheckBox cbSI;
        private System.Windows.Forms.CheckBox cbTB;
        private System.Windows.Forms.CheckBox cbLI;
        private System.Windows.Forms.CheckBox cbBL;
        private System.Windows.Forms.CheckBox cbGB;
        private System.Windows.Forms.CheckBox cbST;
        private System.Windows.Forms.CheckBox cbLU;
        private System.Windows.Forms.CheckBox cbSP;
        private System.Windows.Forms.CheckBox cbKI;
        private System.Windows.Forms.CheckBox cbLR;
        private System.Windows.Forms.CheckBox cbHT;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnmOK;
        private System.Windows.Forms.Button btnmNew;
        private System.Windows.Forms.ListBox plist;
        private System.Windows.Forms.TextBox txtpSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picpVal;
        private System.Windows.Forms.NumericUpDown numpmWeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox pplist;
        private System.Windows.Forms.ListBox pmlist;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtpmSearch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtpName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnpCancel;
        private System.Windows.Forms.Button btnpOK;
        private System.Windows.Forms.Button btnpNew;
        private System.Windows.Forms.RadioButton rbWarm;
        private System.Windows.Forms.RadioButton rbCool;
    }
}