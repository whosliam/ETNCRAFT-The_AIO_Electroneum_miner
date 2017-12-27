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
            this.BtnDiagnostics = new System.Windows.Forms.Button();
            this.BtnOpenLog = new System.Windows.Forms.Button();
            this.StartMining = new System.Windows.Forms.Button();
            this.BtnStopMining = new System.Windows.Forms.Button();
            this.BtnCheckBalance = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.WorkStatus = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCustomPool = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboPool = new System.Windows.Forms.ComboBox();
            this.gpubrand = new System.Windows.Forms.ComboBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.lbl_gpubrand = new System.Windows.Forms.Label();
            this.cpuorgpu = new System.Windows.Forms.ComboBox();
            this.threads = new System.Windows.Forms.TextBox();
            this.lbl_threads = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tab_as = new System.Windows.Forms.TabPage();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.BtnLoadDefaultConfig = new System.Windows.Forms.Button();
            this.chkAutoLoadConfig = new System.Windows.Forms.CheckBox();
            this.hyperthread = new System.Windows.Forms.CheckBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.xmr_notice = new System.Windows.Forms.Label();
            this.xmr_stak_perf_box = new System.Windows.Forms.ComboBox();
            this.stak_nvidia_perf = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.miner_type = new System.Windows.Forms.ComboBox();
            this.BtnLoadConfig = new System.Windows.Forms.Button();
            this.BtnSaveConfig = new System.Windows.Forms.Button();
            this.tbHelp = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
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
            this.tbHelp.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tab_miner);
            this.tabs.Controls.Add(this.tab_as);
            this.tabs.Controls.Add(this.tbHelp);
            resources.ApplyResources(this.tabs, "tabs");
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
            this.groupBox6.Controls.Add(this.status);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // status
            // 
            this.status.BackColor = System.Drawing.SystemColors.Window;
            this.status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.status, "status");
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.wallet_address);
            this.groupBox1.Controls.Add(this.BtnClearWallet);
            resources.ApplyResources(this.groupBox1, "groupBox1");
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
            this.BtnClearWallet.Name = "BtnClearWallet";
            this.BtnClearWallet.UseVisualStyleBackColor = true;
            this.BtnClearWallet.Click += new System.EventHandler(this.BtnClearWallet_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Controls.Add(this.groupBox8);
            resources.ApplyResources(this.groupBox3, "groupBox3");
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
            this.groupBox4.Controls.Add(this.BtnDiagnostics);
            this.groupBox4.Controls.Add(this.BtnOpenLog);
            this.groupBox4.Controls.Add(this.StartMining);
            this.groupBox4.Controls.Add(this.BtnStopMining);
            this.groupBox4.Controls.Add(this.BtnCheckBalance);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // BtnDiagnostics
            // 
            this.BtnDiagnostics.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.BtnDiagnostics, "BtnDiagnostics");
            this.BtnDiagnostics.Name = "BtnDiagnostics";
            this.BtnDiagnostics.UseVisualStyleBackColor = false;
            this.BtnDiagnostics.Click += new System.EventHandler(this.BtnDiagnostics_Click);
            // 
            // BtnOpenLog
            // 
            this.BtnOpenLog.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.BtnOpenLog, "BtnOpenLog");
            this.BtnOpenLog.Name = "BtnOpenLog";
            this.BtnOpenLog.UseVisualStyleBackColor = false;
            this.BtnOpenLog.Click += new System.EventHandler(this.BtnOpenLog_Click);
            // 
            // StartMining
            // 
            this.StartMining.BackColor = System.Drawing.Color.LawnGreen;
            resources.ApplyResources(this.StartMining, "StartMining");
            this.StartMining.ForeColor = System.Drawing.Color.Black;
            this.StartMining.Name = "StartMining";
            this.StartMining.UseVisualStyleBackColor = false;
            this.StartMining.Click += new System.EventHandler(this.BtnStartMining_Click);
            // 
            // BtnStopMining
            // 
            this.BtnStopMining.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.BtnStopMining, "BtnStopMining");
            this.BtnStopMining.ForeColor = System.Drawing.Color.Black;
            this.BtnStopMining.Name = "BtnStopMining";
            this.BtnStopMining.UseVisualStyleBackColor = false;
            this.BtnStopMining.Click += new System.EventHandler(this.BtnStopMining_Click);
            // 
            // BtnCheckBalance
            // 
            this.BtnCheckBalance.BackColor = System.Drawing.Color.LightSeaGreen;
            resources.ApplyResources(this.BtnCheckBalance, "BtnCheckBalance");
            this.BtnCheckBalance.Name = "BtnCheckBalance";
            this.BtnCheckBalance.UseVisualStyleBackColor = false;
            this.BtnCheckBalance.Click += new System.EventHandler(this.BtnCheckBalance_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.WorkStatus);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // WorkStatus
            // 
            resources.ApplyResources(this.WorkStatus, "WorkStatus");
            this.WorkStatus.Name = "WorkStatus";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtCustomPool);
            this.groupBox2.Controls.Add(this.port);
            this.groupBox2.Controls.Add(this.Label9);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cboPool);
            this.groupBox2.Controls.Add(this.gpubrand);
            this.groupBox2.Controls.Add(this.Label5);
            this.groupBox2.Controls.Add(this.lbl_gpubrand);
            this.groupBox2.Controls.Add(this.cpuorgpu);
            this.groupBox2.Controls.Add(this.threads);
            this.groupBox2.Controls.Add(this.lbl_threads);
            this.groupBox2.Controls.Add(this.label11);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.CausesValidation = false;
            this.label6.Name = "label6";
            // 
            // txtCustomPool
            // 
            resources.ApplyResources(this.txtCustomPool, "txtCustomPool");
            this.txtCustomPool.Name = "txtCustomPool";
            this.txtCustomPool.TextChanged += new System.EventHandler(this.txtCustomPool_TextChanged);
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
            // 
            // Label9
            // 
            resources.ApplyResources(this.Label9, "Label9");
            this.Label9.Name = "Label9";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cboPool
            // 
            this.cboPool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPool.FormattingEnabled = true;
            resources.ApplyResources(this.cboPool, "cboPool");
            this.cboPool.Name = "cboPool";
            this.cboPool.SelectedIndexChanged += new System.EventHandler(this.pool_SelectedIndexChanged_1);
            // 
            // gpubrand
            // 
            this.gpubrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gpubrand.FormattingEnabled = true;
            this.gpubrand.Items.AddRange(new object[] {
            resources.GetString("gpubrand.Items"),
            resources.GetString("gpubrand.Items1")});
            resources.ApplyResources(this.gpubrand, "gpubrand");
            this.gpubrand.Name = "gpubrand";
            this.gpubrand.SelectedIndexChanged += new System.EventHandler(this.gpubrand_SelectedIndexChanged_1);
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Name = "Label5";
            // 
            // lbl_gpubrand
            // 
            resources.ApplyResources(this.lbl_gpubrand, "lbl_gpubrand");
            this.lbl_gpubrand.Name = "lbl_gpubrand";
            // 
            // cpuorgpu
            // 
            this.cpuorgpu.BackColor = System.Drawing.SystemColors.Info;
            this.cpuorgpu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cpuorgpu.FormattingEnabled = true;
            this.cpuorgpu.Items.AddRange(new object[] {
            resources.GetString("cpuorgpu.Items"),
            resources.GetString("cpuorgpu.Items1")});
            resources.ApplyResources(this.cpuorgpu, "cpuorgpu");
            this.cpuorgpu.Name = "cpuorgpu";
            this.cpuorgpu.SelectedIndexChanged += new System.EventHandler(this.cpuorgpu_SelectedIndexChanged_1);
            // 
            // threads
            // 
            resources.ApplyResources(this.threads, "threads");
            this.threads.Name = "threads";
            // 
            // lbl_threads
            // 
            resources.ApplyResources(this.lbl_threads, "lbl_threads");
            this.lbl_threads.Name = "lbl_threads";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // tab_as
            // 
            this.tab_as.Controls.Add(this.chkDebug);
            this.tab_as.Controls.Add(this.BtnLoadDefaultConfig);
            this.tab_as.Controls.Add(this.chkAutoLoadConfig);
            this.tab_as.Controls.Add(this.hyperthread);
            this.tab_as.Controls.Add(this.Label3);
            this.tab_as.Controls.Add(this.Label10);
            this.tab_as.Controls.Add(this.xmr_notice);
            this.tab_as.Controls.Add(this.xmr_stak_perf_box);
            this.tab_as.Controls.Add(this.stak_nvidia_perf);
            this.tab_as.Controls.Add(this.Label4);
            this.tab_as.Controls.Add(this.miner_type);
            this.tab_as.Controls.Add(this.BtnLoadConfig);
            this.tab_as.Controls.Add(this.BtnSaveConfig);
            resources.ApplyResources(this.tab_as, "tab_as");
            this.tab_as.Name = "tab_as";
            this.tab_as.UseVisualStyleBackColor = true;
            // 
            // chkDebug
            // 
            resources.ApplyResources(this.chkDebug, "chkDebug");
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // BtnLoadDefaultConfig
            // 
            resources.ApplyResources(this.BtnLoadDefaultConfig, "BtnLoadDefaultConfig");
            this.BtnLoadDefaultConfig.Name = "BtnLoadDefaultConfig";
            this.BtnLoadDefaultConfig.UseVisualStyleBackColor = true;
            this.BtnLoadDefaultConfig.Click += new System.EventHandler(this.BtnLoadDefaultConfig_Click);
            // 
            // chkAutoLoadConfig
            // 
            resources.ApplyResources(this.chkAutoLoadConfig, "chkAutoLoadConfig");
            this.chkAutoLoadConfig.Name = "chkAutoLoadConfig";
            this.chkAutoLoadConfig.UseVisualStyleBackColor = true;
            this.chkAutoLoadConfig.CheckedChanged += new System.EventHandler(this.chkAutoLoadConfig_CheckedChanged);
            // 
            // hyperthread
            // 
            resources.ApplyResources(this.hyperthread, "hyperthread");
            this.hyperthread.Name = "hyperthread";
            this.hyperthread.UseVisualStyleBackColor = true;
            // 
            // Label3
            // 
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.Name = "Label3";
            // 
            // Label10
            // 
            resources.ApplyResources(this.Label10, "Label10");
            this.Label10.Name = "Label10";
            // 
            // xmr_notice
            // 
            resources.ApplyResources(this.xmr_notice, "xmr_notice");
            this.xmr_notice.Name = "xmr_notice";
            // 
            // xmr_stak_perf_box
            // 
            this.xmr_stak_perf_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xmr_stak_perf_box.FormattingEnabled = true;
            this.xmr_stak_perf_box.Items.AddRange(new object[] {
            resources.GetString("xmr_stak_perf_box.Items"),
            resources.GetString("xmr_stak_perf_box.Items1")});
            resources.ApplyResources(this.xmr_stak_perf_box, "xmr_stak_perf_box");
            this.xmr_stak_perf_box.Name = "xmr_stak_perf_box";
            this.xmr_stak_perf_box.SelectedIndexChanged += new System.EventHandler(this.xmr_stak_perf_box_SelectedIndexChanged);
            // 
            // stak_nvidia_perf
            // 
            resources.ApplyResources(this.stak_nvidia_perf, "stak_nvidia_perf");
            this.stak_nvidia_perf.Name = "stak_nvidia_perf";
            // 
            // Label4
            // 
            resources.ApplyResources(this.Label4, "Label4");
            this.Label4.Name = "Label4";
            // 
            // miner_type
            // 
            this.miner_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.miner_type.FormattingEnabled = true;
            resources.ApplyResources(this.miner_type, "miner_type");
            this.miner_type.Name = "miner_type";
            this.miner_type.SelectedIndexChanged += new System.EventHandler(this.miner_type_SelectedIndexChanged_1);
            // 
            // BtnLoadConfig
            // 
            resources.ApplyResources(this.BtnLoadConfig, "BtnLoadConfig");
            this.BtnLoadConfig.Name = "BtnLoadConfig";
            this.BtnLoadConfig.UseVisualStyleBackColor = true;
            this.BtnLoadConfig.Click += new System.EventHandler(this.BtnLoadConfig_Click);
            // 
            // BtnSaveConfig
            // 
            resources.ApplyResources(this.BtnSaveConfig, "BtnSaveConfig");
            this.BtnSaveConfig.Name = "BtnSaveConfig";
            this.BtnSaveConfig.UseVisualStyleBackColor = true;
            this.BtnSaveConfig.Click += new System.EventHandler(this.BtnSaveConfig_Click);
            // 
            // tbHelp
            // 
            this.tbHelp.Controls.Add(this.groupBox9);
            resources.ApplyResources(this.tbHelp, "tbHelp");
            this.tbHelp.Name = "tbHelp";
            this.tbHelp.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
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
            this.tab_as.PerformLayout();
            this.tbHelp.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TabControl tabs;
        internal System.Windows.Forms.TabPage tab_miner;
        internal System.Windows.Forms.Button BtnStopMining;
        internal System.Windows.Forms.Button BtnDiagnostics;
        internal System.Windows.Forms.TextBox wallet_address;
        internal System.Windows.Forms.RichTextBox status;
        internal System.Windows.Forms.TextBox threads;
        internal System.Windows.Forms.TextBox txtCustomPool;
        internal System.Windows.Forms.TextBox port;
        internal System.Windows.Forms.Button BtnCheckBalance;
        internal System.Windows.Forms.Button BtnClearWallet;
        internal System.Windows.Forms.ComboBox cboPool;
        internal System.Windows.Forms.ComboBox gpubrand;
        internal System.Windows.Forms.Button StartMining;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.ComboBox cpuorgpu;
        internal System.Windows.Forms.TabPage tab_as;
        internal System.Windows.Forms.CheckBox hyperthread;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Label xmr_notice;
        internal System.Windows.Forms.ComboBox xmr_stak_perf_box;
        internal System.Windows.Forms.Label stak_nvidia_perf;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.ComboBox miner_type;
        internal System.Windows.Forms.Button BtnLoadConfig;
        internal System.Windows.Forms.Button BtnSaveConfig;
        internal System.Windows.Forms.OpenFileDialog open_config_dialog;
        internal System.Windows.Forms.SaveFileDialog save_config_dialog;
        private System.Windows.Forms.Label lblCPUTemp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_gpubrand;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_threads;
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
        private System.Windows.Forms.Button BtnLoadDefaultConfig;
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
    }
}

