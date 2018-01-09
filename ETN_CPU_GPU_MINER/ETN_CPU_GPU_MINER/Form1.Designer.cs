namespace ETN_CPU_GPU_MINER
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabs = new System.Windows.Forms.TabControl();
            this.tab_miner = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.status = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.wallet_address = new System.Windows.Forms.TextBox();
            this.BtnClearWallet = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lblCPUUsage = new System.Windows.Forms.Label();
            this.lblCPUTemp = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lblGPUUsage = new System.Windows.Forms.Label();
            this.lblGPUTemp = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ClearMessagesButton = new System.Windows.Forms.Button();
            this.BtnOpenLog = new System.Windows.Forms.Button();
            this.StartMining = new System.Windows.Forms.Button();
            this.BtnStopMining = new System.Windows.Forms.Button();
            this.BtnCheckBalance = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.WorkStatus = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboPool = new System.Windows.Forms.ComboBox();
            this.cpuorgpu = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tab_as = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.txtTempLimit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCustomPool = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.btnETNWorth = new System.Windows.Forms.Button();
            this.btnDeleteRegKeys = new System.Windows.Forms.Button();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.BtnLoadDefaultConfig = new System.Windows.Forms.Button();
            this.BtnLoadConfig = new System.Windows.Forms.Button();
            this.BtnSaveConfig = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chkAutoLoadConfig = new System.Windows.Forms.CheckBox();
            this.tbHelp = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.LinkWalletGen = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.open_config_dialog = new System.Windows.Forms.OpenFileDialog();
            this.save_config_dialog = new System.Windows.Forms.SaveFileDialog();
            this.tabs.SuspendLayout();
            this.tab_miner.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tab_as.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.tbHelp.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            resources.ApplyResources(this.tabs, "tabs");
            this.tabs.Controls.Add(this.tab_miner);
            this.tabs.Controls.Add(this.tab_as);
            this.tabs.Controls.Add(this.tbHelp);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            // 
            // tab_miner
            // 
            this.tab_miner.Controls.Add(this.groupBox6);
            this.tab_miner.Controls.Add(this.groupBox1);
            this.tab_miner.Controls.Add(this.groupBox3);
            this.tab_miner.Controls.Add(this.groupBox4);
            this.tab_miner.Controls.Add(this.groupBox5);
            this.tab_miner.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.tab_miner, "tab_miner");
            this.tab_miner.Name = "tab_miner";
            this.tab_miner.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.status);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // status
            // 
            resources.ApplyResources(this.status, "status");
            this.status.BackColor = System.Drawing.SystemColors.Window;
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.wallet_address);
            this.groupBox1.Controls.Add(this.BtnClearWallet);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // wallet_address
            // 
            resources.ApplyResources(this.wallet_address, "wallet_address");
            this.wallet_address.Name = "wallet_address";
            // 
            // BtnClearWallet
            // 
            resources.ApplyResources(this.BtnClearWallet, "BtnClearWallet");
            this.BtnClearWallet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnClearWallet.Name = "BtnClearWallet";
            this.BtnClearWallet.UseVisualStyleBackColor = true;
            this.BtnClearWallet.Click += new System.EventHandler(this.BtnClearWallet_Click);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Controls.Add(this.groupBox8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lblCPUUsage);
            this.groupBox7.Controls.Add(this.lblCPUTemp);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // lblCPUUsage
            // 
            resources.ApplyResources(this.lblCPUUsage, "lblCPUUsage");
            this.lblCPUUsage.Name = "lblCPUUsage";
            // 
            // lblCPUTemp
            // 
            resources.ApplyResources(this.lblCPUTemp, "lblCPUTemp");
            this.lblCPUTemp.Name = "lblCPUTemp";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lblGPUUsage);
            this.groupBox8.Controls.Add(this.lblGPUTemp);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // lblGPUUsage
            // 
            resources.ApplyResources(this.lblGPUUsage, "lblGPUUsage");
            this.lblGPUUsage.Name = "lblGPUUsage";
            // 
            // lblGPUTemp
            // 
            resources.ApplyResources(this.lblGPUTemp, "lblGPUTemp");
            this.lblGPUTemp.Name = "lblGPUTemp";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.ClearMessagesButton);
            this.groupBox4.Controls.Add(this.BtnOpenLog);
            this.groupBox4.Controls.Add(this.StartMining);
            this.groupBox4.Controls.Add(this.BtnStopMining);
            this.groupBox4.Controls.Add(this.BtnCheckBalance);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // ClearMessagesButton
            // 
            resources.ApplyResources(this.ClearMessagesButton, "ClearMessagesButton");
            this.ClearMessagesButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClearMessagesButton.Name = "ClearMessagesButton";
            this.ClearMessagesButton.UseVisualStyleBackColor = true;
            this.ClearMessagesButton.Click += new System.EventHandler(this.ClearMessagesButton_Click);
            // 
            // BtnOpenLog
            // 
            resources.ApplyResources(this.BtnOpenLog, "BtnOpenLog");
            this.BtnOpenLog.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BtnOpenLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnOpenLog.Name = "BtnOpenLog";
            this.BtnOpenLog.UseVisualStyleBackColor = false;
            this.BtnOpenLog.Click += new System.EventHandler(this.BtnOpenLog_Click);
            // 
            // StartMining
            // 
            this.StartMining.BackColor = System.Drawing.Color.LawnGreen;
            this.StartMining.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.StartMining, "StartMining");
            this.StartMining.ForeColor = System.Drawing.Color.Black;
            this.StartMining.Name = "StartMining";
            this.StartMining.UseVisualStyleBackColor = false;
            this.StartMining.Click += new System.EventHandler(this.BtnStartMining_Click);
            // 
            // BtnStopMining
            // 
            this.BtnStopMining.BackColor = System.Drawing.Color.Red;
            this.BtnStopMining.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.BtnStopMining, "BtnStopMining");
            this.BtnStopMining.ForeColor = System.Drawing.Color.Black;
            this.BtnStopMining.Name = "BtnStopMining";
            this.BtnStopMining.UseVisualStyleBackColor = false;
            this.BtnStopMining.Click += new System.EventHandler(this.BtnStopMining_Click);
            // 
            // BtnCheckBalance
            // 
            this.BtnCheckBalance.BackColor = System.Drawing.Color.LightSeaGreen;
            this.BtnCheckBalance.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.BtnCheckBalance, "BtnCheckBalance");
            this.BtnCheckBalance.Name = "BtnCheckBalance";
            this.BtnCheckBalance.UseVisualStyleBackColor = false;
            this.BtnCheckBalance.Click += new System.EventHandler(this.BtnCheckBalance_Click);
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.WorkStatus);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // WorkStatus
            // 
            resources.ApplyResources(this.WorkStatus, "WorkStatus");
            this.WorkStatus.Name = "WorkStatus";
            this.WorkStatus.ReadOnly = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.port);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cboPool);
            this.groupBox2.Controls.Add(this.cpuorgpu);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.CausesValidation = false;
            this.label6.Name = "label6";
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cboPool
            // 
            resources.ApplyResources(this.cboPool, "cboPool");
            this.cboPool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPool.FormattingEnabled = true;
            this.cboPool.Name = "cboPool";
            this.cboPool.SelectedIndexChanged += new System.EventHandler(this.pool_SelectedIndexChanged_1);
            // 
            // cpuorgpu
            // 
            this.cpuorgpu.BackColor = System.Drawing.SystemColors.Info;
            this.cpuorgpu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cpuorgpu.FormattingEnabled = true;
            this.cpuorgpu.Items.AddRange(new object[] {
            resources.GetString("cpuorgpu.Items"),
            resources.GetString("cpuorgpu.Items1"),
            resources.GetString("cpuorgpu.Items2")});
            resources.ApplyResources(this.cpuorgpu, "cpuorgpu");
            this.cpuorgpu.Name = "cpuorgpu";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // tab_as
            // 
            this.tab_as.Controls.Add(this.groupBox11);
            this.tab_as.Controls.Add(this.groupBox12);
            this.tab_as.Controls.Add(this.BtnLoadDefaultConfig);
            this.tab_as.Controls.Add(this.BtnLoadConfig);
            this.tab_as.Controls.Add(this.BtnSaveConfig);
            this.tab_as.Controls.Add(this.groupBox10);
            resources.ApplyResources(this.tab_as, "tab_as");
            this.tab_as.Name = "tab_as";
            this.tab_as.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.txtTempLimit);
            this.groupBox11.Controls.Add(this.label1);
            this.groupBox11.Controls.Add(this.txtCustomPool);
            this.groupBox11.Controls.Add(this.Label9);
            this.groupBox11.Controls.Add(this.Label5);
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // txtTempLimit
            // 
            resources.ApplyResources(this.txtTempLimit, "txtTempLimit");
            this.txtTempLimit.Name = "txtTempLimit";
            this.txtTempLimit.TextChanged += new System.EventHandler(this.txtTempLimit_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtCustomPool
            // 
            resources.ApplyResources(this.txtCustomPool, "txtCustomPool");
            this.txtCustomPool.Name = "txtCustomPool";
            // 
            // Label9
            // 
            resources.ApplyResources(this.Label9, "Label9");
            this.Label9.Name = "Label9";
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Name = "Label5";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.btnETNWorth);
            this.groupBox12.Controls.Add(this.btnDeleteRegKeys);
            this.groupBox12.Controls.Add(this.chkDebug);
            resources.ApplyResources(this.groupBox12, "groupBox12");
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.TabStop = false;
            // 
            // btnETNWorth
            // 
            this.btnETNWorth.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnETNWorth.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.btnETNWorth, "btnETNWorth");
            this.btnETNWorth.Name = "btnETNWorth";
            this.btnETNWorth.UseVisualStyleBackColor = false;
            this.btnETNWorth.Click += new System.EventHandler(this.btnETNWorth_Click);
            // 
            // btnDeleteRegKeys
            // 
            this.btnDeleteRegKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.btnDeleteRegKeys, "btnDeleteRegKeys");
            this.btnDeleteRegKeys.Name = "btnDeleteRegKeys";
            this.btnDeleteRegKeys.UseVisualStyleBackColor = true;
            this.btnDeleteRegKeys.Click += new System.EventHandler(this.btnDeleteRegKeys_Click);
            // 
            // chkDebug
            // 
            resources.ApplyResources(this.chkDebug, "chkDebug");
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // BtnLoadDefaultConfig
            // 
            this.BtnLoadDefaultConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.BtnLoadDefaultConfig, "BtnLoadDefaultConfig");
            this.BtnLoadDefaultConfig.Name = "BtnLoadDefaultConfig";
            this.BtnLoadDefaultConfig.UseVisualStyleBackColor = true;
            this.BtnLoadDefaultConfig.Click += new System.EventHandler(this.BtnLoadDefaultConfig_Click);
            // 
            // BtnLoadConfig
            // 
            this.BtnLoadConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.BtnLoadConfig, "BtnLoadConfig");
            this.BtnLoadConfig.Name = "BtnLoadConfig";
            this.BtnLoadConfig.UseVisualStyleBackColor = true;
            this.BtnLoadConfig.Click += new System.EventHandler(this.BtnLoadConfig_Click);
            // 
            // BtnSaveConfig
            // 
            this.BtnSaveConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.BtnSaveConfig, "BtnSaveConfig");
            this.BtnSaveConfig.Name = "BtnSaveConfig";
            this.BtnSaveConfig.UseVisualStyleBackColor = true;
            this.BtnSaveConfig.Click += new System.EventHandler(this.BtnSaveConfig_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.chkAutoLoadConfig);
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // chkAutoLoadConfig
            // 
            resources.ApplyResources(this.chkAutoLoadConfig, "chkAutoLoadConfig");
            this.chkAutoLoadConfig.Name = "chkAutoLoadConfig";
            this.chkAutoLoadConfig.UseVisualStyleBackColor = true;
            this.chkAutoLoadConfig.CheckedChanged += new System.EventHandler(this.chkAutoLoadConfig_CheckedChanged);
            // 
            // tbHelp
            // 
            this.tbHelp.Controls.Add(this.groupBox14);
            this.tbHelp.Controls.Add(this.groupBox9);
            resources.ApplyResources(this.tbHelp, "tbHelp");
            this.tbHelp.Name = "tbHelp";
            this.tbHelp.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label10);
            this.groupBox14.Controls.Add(this.label8);
            resources.ApplyResources(this.groupBox14, "groupBox14");
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.TabStop = false;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label19);
            this.groupBox9.Controls.Add(this.label18);
            this.groupBox9.Controls.Add(this.label17);
            this.groupBox9.Controls.Add(this.label16);
            this.groupBox9.Controls.Add(this.label15);
            this.groupBox9.Controls.Add(this.label14);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Controls.Add(this.label12);
            this.groupBox9.Controls.Add(this.LinkWalletGen);
            this.groupBox9.Controls.Add(this.label7);
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Name = "label13";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // LinkWalletGen
            // 
            resources.ApplyResources(this.LinkWalletGen, "LinkWalletGen");
            this.LinkWalletGen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LinkWalletGen.Name = "LinkWalletGen";
            this.LinkWalletGen.TabStop = true;
            this.LinkWalletGen.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkWalletGen_LinkClicked);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // open_config_dialog
            // 
            this.open_config_dialog.FileName = "File name";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.Controls.Add(this.tabs);
            this.Name = "Form1";
            this.tabs.ResumeLayout(false);
            this.tab_miner.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tab_as.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.tbHelp.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TabControl tabs;
        internal System.Windows.Forms.TabPage tab_miner;
        internal System.Windows.Forms.Button BtnStopMining;
        internal System.Windows.Forms.TextBox wallet_address;
        internal System.Windows.Forms.RichTextBox status;
        internal System.Windows.Forms.TextBox port;
        internal System.Windows.Forms.Button BtnCheckBalance;
        internal System.Windows.Forms.Button BtnClearWallet;
        internal System.Windows.Forms.ComboBox cboPool;
        internal System.Windows.Forms.Button StartMining;
        internal System.Windows.Forms.ComboBox cpuorgpu;
        internal System.Windows.Forms.TabPage tab_as;
        internal System.Windows.Forms.Button BtnLoadConfig;
        internal System.Windows.Forms.Button BtnSaveConfig;
        internal System.Windows.Forms.OpenFileDialog open_config_dialog;
        internal System.Windows.Forms.SaveFileDialog save_config_dialog;
        private System.Windows.Forms.Label lblCPUTemp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblGPUTemp;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        internal System.Windows.Forms.Button BtnOpenLog;
        private System.Windows.Forms.CheckBox chkAutoLoadConfig;
        private System.Windows.Forms.TabPage tbHelp;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.LinkLabel LinkWalletGen;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox WorkStatus;
        private System.Windows.Forms.Label lblCPUUsage;
        private System.Windows.Forms.Label lblGPUUsage;
        internal System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Button ClearMessagesButton;
        private System.Windows.Forms.Button btnDeleteRegKeys;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox11;
        internal System.Windows.Forms.TextBox txtCustomPool;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Button BtnLoadDefaultConfig;
        private System.Windows.Forms.TextBox txtTempLimit;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnETNWorth;
    }
}

