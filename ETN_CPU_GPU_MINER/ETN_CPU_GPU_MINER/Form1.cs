using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Globalization;
using OpenHardwareMonitor.Hardware;
using System.Net;
using Newtonsoft.Json;
using ETNCRAFT;

namespace ETN_CPU_GPU_MINER
{
    public partial class Form1 : Form
    {
        #region Global vars
        public static string m_Version;
        public static string m_sAggHashData = "";
        public static string m_MiningURL = "";
        public static string m_PoolWebsiteURL = "";
        public static int m_IPoolID = 0;
        public static string m_sETNCRAFTCPULogFileLocation = Application.StartupPath + "\\app_assets\\ETN_CRAFT_CPU_LOG.txt";
        public static string m_sMinerCpuConfig = Application.StartupPath + "\\app_assets\\cpu.txt";
        public static string m_sMinerAMDGPUConfig = Application.StartupPath + "\\app_assets\\amd.txt";
        public static string m_sMinerNVIDIAGPUConfig = Application.StartupPath + "\\app_assets\\nvidia.txt";
        public static double m_dMaxUpTimeSec;

        public bool b_FormLoaded = false;
        public bool m_bStartTime = false;
        public bool m_bDebugging = false;
        public bool m_bReadETNCRAFTULog = false;
        public bool m_bTempWarningModalIsOpen = false;
        public bool m_bDoLog = true;
        public string m_sScheduleData = "";
        private List<ScheduleEvent> m_cScheduleList = new List<ScheduleEvent>();

        private Stopwatch stopwatch = new Stopwatch();
        private Logger logger;
        private Logger loggerPool;
        private Messager messager = new Messager();
        private RegistryManager registryManager = new RegistryManager();
        private int m_iTemperatureAlert = 90;
        private Timer upTimer;

        private Computer myComputer;
        #endregion

        #region Form Contructor/Events
        public Form1()
        {
            m_dMaxUpTimeSec = Program.m_dMaxRuntime * 60.00;
            m_bDoLog = Program.m_bDoLog;
            ProcessUtil.CheckForExistingProcesses();

            if (m_bDoLog)
            {
                logger = new Logger("ETN_Craft");
                loggerPool = new Logger("ETN_Craft_Pool");
                messager.InitializeMessager(logger);
            }


            m_Version = registryManager.GetVersion();
            CheckVersion();
            InitializeComponent();
            //Set version in window header
            this.Text = "ETNCRAFT (" + m_Version + ")";
            this.Update();
            LoadPoolListFromWebsite();
            //force first item in index -- just in case for new user.
            cpuorgpu.SelectedIndex = 0;
            // Check Registry for AutoLoad
            PushStatusMessage("Checking for ETNCRAFT registry keys", m_bDoLog);
            if (registryManager.GetAutoLoad())
                LoadRegistryConfig();
            PushStatusMessage("AutoLoad registry key loaded (" + registryManager.GetAutoLoad() + ")", m_bDoLog);

            if (!chkMaxUp.Checked)
            {
                strMinutes.Visible = false;
                maxUpTimeMin.Visible = false;
            }
            // overload registry config with cmd arg
            if (m_dMaxUpTimeSec > 0.00)
                maxUpTimeMin.Text = Convert.ToString(m_dMaxUpTimeSec / 60.00);

            // Check Registry for NewMiner
            if (registryManager.GetNewMiner())
            {
                PushStatusMessage("Welcome New Miner!", m_bDoLog);
                DialogResult UserInput = MessageBox.Show("Welcome new miner!\r\nThe help tab has been pre selected.\r\nPlease read and follow the directions.", "WELCOME!", MessageBoxButtons.OK);
                //Load Help tab
                tabs.SelectedTab = tbHelp;
            }

            //Spool up timers
            InitTemps();

            //This is to keep the event handlers from firing when the form load. Just wrap functions in this.
            b_FormLoaded = true;
            this.FormClosing += new FormClosingEventHandler(CloseForm);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            chkEnabled.Checked = registryManager.GetScheduleEnabled();
            LoadScheduleData();
            EnabledScheduleControls();
            m_cTimer.Interval = 1000;
            m_cTimer.Enabled = true;
            m_cTimer.Start();
            if (chkEnabled.Checked)
            {
                m_cScheduleTimer.Enabled = true;
                m_cScheduleTimer.Start();
            }
        }
        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            if (m_bDoLog)
                logger.Warn("ETNCRAFT window closed, beginning process cleanup.");
            ProcessUtil.EndProcesses();
            registryManager.CloseRegistryKeys();
        }
        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
        private void Form1_Resize_1(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(2000);
            }
        }
        #endregion
        #region Control Handlers
        #region Click Handlers

        private void BtnStartMining_Click(object sender, EventArgs e)
        {
            //Init Registry Keys just in case dude man decided to delete them.
            registryManager.Initialize();
            SaveConfig();
            registryManager.SetNewMiner(false);
            m_bDebugging = chkDebug.Checked;
            if (!IsWalletValid())
                return;
            SpawnMiner(cpuorgpu.SelectedItem.ToString());
            PushStatusMessage("config.txt updated", m_bDoLog);
            //Start header Timer for app run time
            m_bStartTime = true;
            stopwatch.Start();
            //Start Hash textbox reset timer
            HashTimer();

        }

        private void SpawnMiner(string sComponent)
        {
            #region Check for cust pool
            if (!txtCustomPool.Text.Equals(""))
            {
                m_MiningURL = txtCustomPool.Text;
                PushStatusMessage("custom pool selected[" + txtCustomPool.Text + "], make sure to add your port if needed!", m_bDoLog);
            }

            #endregion
            #region UPDATE CONFIG
            #region House Keeping
            string sConfig_Template_File_Name = "config_templates/config-etncraft.txt";
            string sConfig_File_Name = "app_assets/config.txt";
            #endregion
            #region Delete and recreate config.txt
            if (File.Exists(sConfig_File_Name))
                File.Delete(sConfig_File_Name);
            else
                File.Create(sConfig_File_Name).Dispose();
            #endregion
            #region push msg
            PushStatusMessage("config created for " + sComponent, m_bDoLog);
            #endregion
            #region  copy template to new config.txt
            File.Copy(sConfig_Template_File_Name, sConfig_File_Name, true);
            #endregion
            #region  replace vars in new config.txt with GUI info
            var CONFIG_CONTENTS = File.ReadAllText(sConfig_File_Name);
            CONFIG_CONTENTS = CONFIG_CONTENTS.Replace("address_replace", m_MiningURL + ":" + port.Text);
            CONFIG_CONTENTS = CONFIG_CONTENTS.Replace("wallet_replace", wallet_address.Text.Replace(" ", ""));
            File.SetAttributes(Application.StartupPath + "\\" + sConfig_File_Name, FileAttributes.Normal);
            File.WriteAllText(Application.StartupPath + "\\" + sConfig_File_Name, CONFIG_CONTENTS);
            #endregion
            #endregion
            #region Spawn miner
            PushStatusMessage("Spawning ETNCRAFT miner for " + sComponent, m_bDoLog);
            string sArgs = "";
            if (sComponent.Equals("CPU"))
                sArgs = "--noAMD --noNVIDIA";
            else if (sComponent.Equals("GPU"))
                sArgs = "--noCPU";

            Process process = ProcessUtil.SpawnMinerProcess(sArgs, m_bDebugging);
            process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) => PushWorkStatusMessage(eOut.Data);
            process.BeginOutputReadLine();
            process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) => PushWorkStatusMessage(eErr.Data);
            process.BeginErrorReadLine();

            #endregion
            StartMining.Enabled = false;
            StartMining.BackColor = Color.LightGray;
            //BtnStopMining.Enabled = true;
        }

        private void BtnStopMining_Click(object sender, EventArgs e)
        {
            //Stop Timer
            m_bStartTime = false;
            stopwatch.Stop();
            //Kill mining
            ProcessUtil.EndProcesses();
            StartMining.Enabled = true;
            StartMining.BackColor = Color.LawnGreen;

        }

        private void BtnCheckBalance_Click(object sender, EventArgs e)
        {
            string webAddress = m_PoolWebsiteURL;
            Process.Start(webAddress);
        }

        private void BtnClearWallet_Click(object sender, EventArgs e)
        {
            wallet_address.Text = "";
            status.Text = messager.ClearMessages();
        }

        private void BtnOpenLog_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = logger.GetLogFilePath()
            });
            Process.Start(new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = loggerPool.GetLogFilePath()
            });
        }

        private void ClearMessagesButton_Click(object sender, EventArgs e)
        {
            status.Text = messager.ClearMessages();
            WorkStatus.Text = "";
            m_sAggHashData = "";
        }

        private void BtnLoadConfig_Click(object sender, EventArgs e)
        {
            registryManager.Initialize();
        }

        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void BtnLoadDefaultConfig_Click(object sender, EventArgs e)
        {
            registryManager.DeleteRegistryKey();
            registryManager.Initialize();
            Application.Restart();
            Environment.Exit(0);
        }

        private void LinkWalletGen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://my.electroneum.com/offline_paper_electroneum_walletV1.6.html");
        }

        private void btnETNWorth_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetCurrentCoinPrice(), "Current ETN Worth");
        }

        private void btnDeleteRegKeys_Click(object sender, EventArgs e)
        {
            MessageBox.Show(registryManager.DeleteRegistryKey(), "ETNCRAFT Services");
        }

        private void btnCPUConfig_Click(object sender, EventArgs e)
        {
            if (!File.Exists(m_sMinerCpuConfig))
                MessageBox.Show("You must run the CPU miner first.\r\nThis will build the init config.", "Run Miner");
            else
            {
                Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = m_sMinerCpuConfig
                });
            }
        }

        private void btnGPUConfig_Click(object sender, EventArgs e)
        {
            bool bAMD = false;
            bool bNivida = false;

            if (File.Exists(m_sMinerAMDGPUConfig))
                bAMD = true;
            if (File.Exists(m_sMinerNVIDIAGPUConfig))
                bNivida = true;
            if (!bAMD && !bNivida)
                MessageBox.Show("You must run the GPU miner first.\r\nThis will build the init config.", "Run Miner");
            else
            {
                if (bAMD)
                    Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = m_sMinerAMDGPUConfig });
                else if (bNivida)
                    Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = m_sMinerNVIDIAGPUConfig });
            }
        }
        #endregion
        #region DropDown Handlers

        private void pool_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            PoolsCollection cPoolCollection = new PoolsCollection();
            cPoolCollection.Load();
            foreach (var cItem in cPoolCollection)
            {
                if (cItem.sDisplayName.Equals(cboPool.SelectedItem.ToString()))
                {
                    m_MiningURL = cItem.sPoolMiningURL;
                    m_PoolWebsiteURL = cItem.sPoolWebsite;
                    m_IPoolID = cItem.iID;
                    PushStatusMessage(cItem.sDisplayName + " selected, " + cItem.sPoolWebsite + " | " + cItem.sPoolInformation, m_bDoLog);
                    break;
                }
            }
        }

        #endregion
        #region Check Box Handlers

        private void chkAutoLoadConfig_CheckedChanged(object sender, EventArgs e)
        {
            if (b_FormLoaded)
            {
                DialogResult UserInput;
                if (chkAutoLoadConfig.Checked)
                {
                    new DialogResult();
                    UserInput = MessageBox.Show("Make sure your wallet information has been entered!\r\nThis will save your info and also setup auto config when the app restarts\r\nClick yes to continue", "ALERT", MessageBoxButtons.YesNo);
                    if (UserInput == DialogResult.No)
                    {
                        registryManager.SetAutoLoad(false);
                        chkAutoLoadConfig.Checked = false;
                    }
                    else
                    {
                        SaveConfig();
                        registryManager.SetAutoLoad(chkAutoLoadConfig.Checked);

                    }
                }
                else
                    registryManager.SetAutoLoad(false);
            }
            PushStatusMessage("AutoLoad registry key updated.", m_bDoLog);
        }

        private void chkMaxUp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMaxUp.Checked)
            {
                strMinutes.Visible = true;
                maxUpTimeMin.Visible = true;
            }
        }

        #endregion
        #region Text Box Handlers

        private void wallet_address_Click(object sender, EventArgs e)
        {
            wallet_address.SelectAll();
        }

        private void maxUpTimeMin_TextChanged(object sender, EventArgs e)
        {
            if (maxUpTimeMin.Text.Length > 0)
                m_dMaxUpTimeSec = Convert.ToDouble(maxUpTimeMin.Text) * 60.00;
            else
                m_dMaxUpTimeSec = 0;
        }

        private void txtTempLimit_TextChanged(object sender, EventArgs e)
        {
            m_iTemperatureAlert = int.Parse(CheckTempLimitEntry(txtTempLimit.Text));
        }

        #endregion
        #endregion
        #region Utility Methods
        private void CheckVersion()
        {
            //Set Path
            string filepath = Application.StartupPath + "\\app_assets\\version.txt";
            //Download doc from website
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://liamthrower.com/version.txt", filepath);
            }
            catch (Exception e)
            {
                messager.PushMessage(e.Message, true);
            }
            //Check version
            StreamReader sr = new StreamReader(filepath);
            string sVersion = sr.ReadLine();
            DialogResult UserInput = new DialogResult();
            if (!sVersion.Equals(m_Version))
                UserInput = MessageBox.Show("Version " + sVersion + " is avaliable \r\n Download?", "New Version", MessageBoxButtons.YesNo);
            if (UserInput.Equals(DialogResult.Yes))
                Process.Start("https://github.com/whosliam/ETNCRAFT-The_AIO_Electroneum_miner/releases");


        }
        #region Config/Registry

        private void LoadRegistryConfig()
        {
            PushStatusMessage("Loading ETNCRAFT config from registry", m_bDoLog);
            wallet_address.Text = registryManager.GetWalletId();
            port.Text = registryManager.GetPortNumber();
            chkAutoLoadConfig.Checked = registryManager.GetAutoLoad();
            txtCustomPool.Text = registryManager.GetCustomPool();
            cpuorgpu.SelectedItem = registryManager.GetComponent();
            txtTempLimit.Text = CheckTempLimitEntry(registryManager.GetTempLimit());
            m_iTemperatureAlert = int.Parse(txtTempLimit.Text);
            chkMaxUp.Checked = registryManager.GetEnforceMaxUpTime();
            m_sScheduleData = registryManager.GetScheduleData();

            if (m_dMaxUpTimeSec == 0.00)
                maxUpTimeMin.Text = Convert.ToString(registryManager.GetMaxUpTimeMin());

            #region Get Pool and select drop down
            bool bFoundPool = false;
            //i know i know.... this is the wrong way to go about this. Just for quick testing of registry additions. Git blame Liam
            PoolsCollection cPools = new PoolsCollection();
            cPools.Load();
            if (cPools.Count > 0)
                foreach (var Pool in cPools)
                    if (Pool.iID.Equals(registryManager.GetPool()))
                    {
                        int index = cboPool.FindString(Pool.sDisplayName);
                        cboPool.SelectedIndex = index;
                        m_IPoolID = Pool.iID;
                        bFoundPool = true;
                        break;
                    }
            if (!bFoundPool)
                PushStatusMessage("Saved pool no longer exists in our database", m_bDoLog);
            #endregion
        }

        private void SaveConfig()
        {
            registryManager.SetAutoLoad(chkAutoLoadConfig.Checked);
            registryManager.SetCustomPool(txtCustomPool.Text);
            registryManager.SetComponent(cpuorgpu.SelectedItem.ToString());
            registryManager.SetPort(port.Text);
            registryManager.SetPool(m_IPoolID);
            registryManager.SetWalletId(wallet_address.Text);
            registryManager.SetTempLimit(CheckTempLimitEntry(txtTempLimit.Text));
            registryManager.SetEnforceMaxUpTime(chkMaxUp.Checked);
            registryManager.SetMaxUpTimeMin(maxUpTimeMin.Text.Length > 0 ? Convert.ToDouble(maxUpTimeMin.Text) : 0);
            registryManager.SetScheduleData(m_sScheduleData);
            PushStatusMessage("Configuration Updated", m_bDoLog);
        }

        #endregion
        #region Timers/Temperature Data/CPU LOG READ

        #region Hash Timer

        private void HashTimer()
        {
            Timer HashTxtTimer = new Timer();
            HashTxtTimer.Interval = 600000;//10 minutes
            HashTxtTimer.Tick += new System.EventHandler(Hashtxt_Tick);
            HashTxtTimer.Start();

        }

        private void Hashtxt_Tick(object sender, EventArgs e)
        {
            WorkStatus.Text = "Log Cleared!";
            m_sAggHashData = "";
        }

        private void m_cTimer_Tick(object sender, EventArgs e)
        {
            m_cTimer.Stop();
            m_cTimer.Enabled = false;

            if (Program.m_bAutoRun)
                BtnStartMining_Click(sender, e);

            if (Program.m_bMinimize)
                WindowState = FormWindowState.Minimized;
        }

        private void UpTimer_Tick(object sender, EventArgs e)
        {
            double m_dElapsedSeconds = stopwatch.Elapsed.TotalSeconds;
            if (m_dElapsedSeconds < m_dMaxUpTimeSec || m_dMaxUpTimeSec == 0.00)
            {
                GetSysTemp();
                #region Timer in window header
                if (m_bStartTime)
                {
                    this.Text = "ETNCRAFT " + m_Version + " | Uptime " + String.Format("{0} Days {1}:{2}:{3}", stopwatch.Elapsed.Days.ToString("00"), stopwatch.Elapsed.Hours.ToString("00"), stopwatch.Elapsed.Minutes.ToString("00"), stopwatch.Elapsed.Seconds.ToString("00")); ;
                    this.Update();
                }
                #endregion
            }
            else
            {
                stopwatch.Stop();
                this.Text = ("Maximum UpTime Limit Reached At : " + String.Format("{0}:{1}:{2}", stopwatch.Elapsed.Hours.ToString("00"), stopwatch.Elapsed.Minutes.ToString("00"), stopwatch.Elapsed.Seconds.ToString("00")));
                ProcessUtil.EndProcesses();
                this.Update();
                Environment.Exit(0);
            }
        }

        #endregion

        #region Temp/Uptime Timers etc

        public void InitTemps()
        {
            upTimer = new Timer { Enabled = true, Interval = 1000 };
            upTimer.Tick += new EventHandler(UpTimer_Tick);

            GetTemperature.System settings = new GetTemperature.System(new Dictionary<string, string>
            {
                { "/intelcpu/0/temperature/0/values", "H4sIAAAAAAAEAOy9B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Iu6//MH37x79i9/+NX6N3/TJm9/5f/01fw1+fosnv+A/+OlfS37/jZ/s/Lpv9fff6Ml/NTef/yZPnozc5679b+i193//TQZ+/w2Dd+P9/sZeX/67v/GTf/b3iP3u4/ObBL//73+i+f039+D8Zk/+xz/e/P6beu2TQZju8yH8f6OgzcvPv/U3/Rb8+z/0f/9b/+yfaOn8079X6fr6Cws7ln/iHzNwflPv99/wyS/+xY4+v/evcJ+733+jJ5//Cw7/4ndy9Im3+U2e/Fbnrk31C93vrt/fyPvdb+N//hsF7/4/AQAA//9NLZZ8WAIAAA==" },
                { "/intelcpu/0/load/0/values", "H4sIAAAAAAAEAOy9B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Iu6//MH37x79i9++mpwcv/md/9df89egZ/xX/ym/5y/4D37618Lv7ya//u+58+u+5d9/z7/5t/w9/6u5fP5bH/6av+eTkXyefXxp26ONaf/v/dG/sf39D/rvnv4e5vc/0IP56/waK/vuHzf5I38P8/tv+mv8Rbb9f0pwTF9/zr/1X9vP/8I//+/6Pf7Z30N+/zdf/HX29zd/859q4aCNP5b//U+U3/+7f+zXOjZwfqvDX/V7/o9/vPz+a1G/pv0f+fGlhfk7eZ//N3/0v28//5X0u/n8Cxq7+f1X/tHft20A5x8a/W5/02+BP36Nf+j/nv8XfzrT+c2//Ob4p3+vktvUhNs/+xcWikP6e/4T/5jS5M8/sL8vP/5ff49f/Ivl9//sHzv6PX/vXyG//9R/94/9HuZ34P/5vyC//3W/5e/1exa/k+Bw4bUBnU2bP4Xg/1bn0uafeTH6PatfKL//N3/0t2y/gG9+/8+IzqYNxmU+/+jwX7afY67/nwAAAP//GYSA31gCAAA=" },
            });

            myComputer = new Computer(settings)
            {
                GPUEnabled = true,
                CPUEnabled = true
                //MainboardEnabled = true,
                //RAMEnabled = true,
                // FanControllerEnabled = true,
                //HDDEnabled = true
            };
            myComputer.Open();
        }

        public void GetSysTemp()
        {
            lblCPUTemp.Text = "";
            lblCPUUsage.Text = "";
            lblGPUTemp.Text = "";
            lblGPUUsage.Text = "";

            foreach (var hardwareItem in myComputer.Hardware)
            {
                hardwareItem.Update();
                if (hardwareItem.HardwareType.Equals(HardwareType.CPU))
                {
                    hardwareItem.Update();
                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                        subHardware.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType.Equals(SensorType.Temperature))
                        {
                            lblCPUTemp.Text += (String.Format("{0} = {1}C", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value") + "\r\n");
                            if (sensor.Value > m_iTemperatureAlert)
                                HighTempAlert(sensor.Name);
                        }
                        else if (sensor.SensorType.Equals(SensorType.Load))
                            lblCPUUsage.Text += (String.Format("{0} = {1}%", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value") + "\r\n");

                    }
                }
                if (hardwareItem.HardwareType.Equals(HardwareType.GpuAti) || hardwareItem.HardwareType.Equals(HardwareType.GpuNvidia))
                {
                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                        subHardware.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType.Equals(SensorType.Temperature))
                        {
                            lblGPUTemp.Text += (String.Format("{0} = {1}C", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value") + "\r\n");
                            if (sensor.Value > m_iTemperatureAlert)
                                HighTempAlert(sensor.Name);
                        }
                        else if (sensor.SensorType.Equals(SensorType.Load))
                            lblGPUUsage.Text += (String.Format("{0} = {1}%", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value") + "\r\n");
                    }
                }
            }
            #region Use This loop for all hardware info
            //foreach (var hardwareItem in myComputer.Hardware)
            //{
            //    hardwareItem.Update();

            //    if (hardwareItem.SubHardware.Length > 0)
            //    {
            //        foreach (IHardware subHardware in hardwareItem.SubHardware)
            //        {
            //            subHardware.Update();

            //            foreach (var sensor in subHardware.Sensors)
            //            {

            //                lblGPUTemp.Text += (String.Format("{0} {1} = {2}", sensor.Name, sensor.Hardware, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value"));
            //            }
            //        }
            //    }
            //    else
            //    {
            //        foreach (var sensor in hardwareItem.Sensors)
            //        {

            //            lblGPUTemp.Text += (String.Format("{0} {1} = {2}", sensor.Identifier, sensor.Hardware, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value"));
            //        }
            //    }
            //}
        }
        #endregion

        private void HighTempAlert(string sDevice)
        {
            if (!m_bTempWarningModalIsOpen && !registryManager.GetIgnoreTempWarnings())
            {
                m_bTempWarningModalIsOpen = true;
                DialogResult UserInput = MessageBox.Show(sDevice + " Temps are above " + m_iTemperatureAlert.ToString() + " degrees!\r\nConsider turning fan speeds higher.\r\n\r\nSave ignore warnings in config?", "WARNING!", MessageBoxButtons.YesNo);
                if (UserInput.Equals(DialogResult.Yes))
                    registryManager.SetIgnoreTempWarnings(true);
                else
                    registryManager.SetIgnoreTempWarnings(false);
            }
        }

        #endregion

        #region Logger/Messager

        private void PushStatusMessage(string message, bool doLog = true)
        {
            if (message != null)
            {
                status.Text = messager.PushMessage(message, doLog);
                status.SelectionStart = status.Text.Length;
                status.ScrollToCaret();
            }
        }

        private void PushWorkStatusMessage(string message)
        {
            if (message != null && !message.Equals(""))
            {
                m_sAggHashData += message + "\r\n";
                ThreadHelperClass.SetText(this, WorkStatus, m_sAggHashData);
                if (m_bDoLog)
                    loggerPool.Debug(message);

                try
                {
                    //TEMP SOLUTION -- COMMENT THESE TWO LINES OUT IN LOCAL DEBUG
                    WorkStatus.SelectionStart = WorkStatus.Text.Length;
                    WorkStatus.ScrollToCaret();
                }
                catch { }
            }
        }

        #endregion

        private bool IsWalletValid()
        {
            if (wallet_address.Text.Equals("Enter Public Wallet Here") || wallet_address.Text.Equals("") || wallet_address.Text.Equals("EnterPublicWalletHere"))
            {
                DialogResult UserInput = MessageBox.Show("Developer Wallet Will Be Used!\r\nARE YOU SURE?!", "READ THIS", MessageBoxButtons.OKCancel);
                if (UserInput.Equals(DialogResult.Cancel))
                {
                    return false;
                }
                else if (UserInput.Equals(DialogResult.OK))
                {
                    wallet_address.Text = "etnk73mQE5yfqZUnMYeJPyJUb5AigTtox8cgd3zw493uRwgG6fKXUdeaBcny4kuy5DN3XiizKUCPjM2ySkJvK9Cm7ZTGJMr7gT";
                    PushStatusMessage("Developer Wallet Address Selected! Thanks!", m_bDoLog);
                    return true;
                }
            }

            var minLength = 98;
            if (wallet_address.Text.Length < minLength || !wallet_address.Text.StartsWith("etn"))
            {
                DialogResult UserInput = MessageBox.Show("Wallet ID Invalid! \n ID must be 98 chracters in length, \nand start with etn.", "Invalid Wallet", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void LoadPoolListFromWebsite()
        {
            //Set Path
            string filepath = Application.StartupPath + "\\app_assets\\pools.txt";
            //Download doc from website
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://liamthrower.com/pools.txt", filepath);
            }
            catch (Exception e)
            {
                messager.PushMessage(e.Message, true);
            }
            //Build Collection of pools
            PoolsCollection cAllPools = new PoolsCollection();
            cAllPools.Load();
            foreach (var citem in cAllPools)
                cboPool.Items.Add(new PoolComboBoxItems(citem.iID.ToString(), citem.sDisplayName));
            //Force selected item in dropdown list and fire the onselected change event which sets some global vars
            cboPool.SelectedIndex = 0;

        }

        #endregion
        private string CheckTempLimitEntry(string sText)
        {
            bool bFailed = false;
            string sTemperature = "";
            if (!string.IsNullOrWhiteSpace(sText))
            {
                int temp;
                if (int.TryParse(sText, out temp))
                    if (temp >= 0)
                        sTemperature = sText;
                    else
                        bFailed = true;
                else
                    bFailed = true;
            }
            else
                sTemperature = "90";
            if (bFailed)
            {
                sTemperature = "90";
                PushStatusMessage("Temp field in not an int. Limit set to 90", m_bDoLog);
            }
            return sTemperature;
        }
        private string GetCurrentCoinPrice()
        {
            string sETNUSD = "";
            var json = new WebClient().DownloadString("https://api.nanopool.org/v1/etn/prices");
            if (!json.Equals(""))
            {
                PRICE_Rootobject r = JsonConvert.DeserializeObject<PRICE_Rootobject>(json);
                sETNUSD = "ETN Price USD: " + r.data.price_usd + "\r\n";
                sETNUSD += "ETN Price BTN: " + r.data.price_btc + "\r\n";
                sETNUSD += "ETN Price EUR: " + r.data.price_eur + "\r\n";
                sETNUSD += "ETN Price RUR: " + r.data.price_rur + "\r\n";
                sETNUSD += "ETN Price CNY: " + r.data.price_cny + "\r\n";
            }
            return sETNUSD;
        }


        private void LoadScheduleData()
        {
            lvList.Items.Clear();
            char[] sDelim1 = { '|' };
            char[] sDelim2 = { ',' };
            string[] sRecs = m_sScheduleData.Split(sDelim1);
            foreach (string sRec in sRecs)
            {
                string[] sParts = sRec.Split(sDelim2);
                if (sParts.Length.Equals(3))
                {
                    ListViewItem lvi = new ListViewItem(CultureInfo.CurrentCulture.DateTimeFormat.DayNames[int.Parse(sParts[0])]);
                    lvi.SubItems.Add(sParts[1]);
                    lvi.SubItems.Add(sParts[2]);
                    lvList.Items.Add(lvi);
                }
            }
        }
        private List<ScheduleEvent> LoadScheduleDataOnly()
        {
            List<ScheduleEvent> cList = new List<ScheduleEvent>();
            char[] sDelim1 = { '|' };
            char[] sDelim2 = { ',' };
            string[] sRecs = m_sScheduleData.Split(sDelim1);
            foreach (string sRec in sRecs)
            {
                string[] sParts = sRec.Split(sDelim2);
                if (sParts.Length.Equals(3))
                {
                    ScheduleEvent cEvent = new ScheduleEvent(int.Parse(sParts[0]), DateTime.Parse(sParts[1]), DateTime.Parse(sParts[2]));
                    cList.Add(cEvent);
                }
            }
            return cList;
        }
        private void SaveScheduleData()
        {
            string sWad = "";
            foreach (ListViewItem lvi in lvList.Items)
            {
                string sRec = GetDayIndex(lvi.SubItems[0].Text).ToString() + "," + lvi.SubItems[1].Text + "," + lvi.SubItems[2].Text;
                if (sWad.Length > 0) sWad += "|";
                sWad += sRec;
            }
            m_sScheduleData = sWad;
            registryManager.SetScheduleData(m_sScheduleData);
            registryManager.SetScheduleEnabled(chkEnabled.Checked);
            // force data reload for timer checks
            m_cScheduleList.Clear();
            if (chkEnabled.Checked && !m_cScheduleTimer.Enabled)
            {
                // start the timer
                m_cScheduleTimer.Enabled = true;
                m_cScheduleTimer.Start();
            }
        }
        #endregion
        #region Event Handlers
        private void lnkgit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/whosliam/ETNCRAFT-The_AIO_Electroneum_miner");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveScheduleData();
        }
        private void lvList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                foreach (int iNdx in lvList.SelectedIndices)
                    lvList.Items.RemoveAt(iNdx);
        }
        private void EnabledScheduleControls()
        {
            chkDay0.Enabled = chkEnabled.Checked;
            chkDay1.Enabled = chkEnabled.Checked;
            chkDay2.Enabled = chkEnabled.Checked;
            chkDay3.Enabled = chkEnabled.Checked;
            chkDay4.Enabled = chkEnabled.Checked;
            chkDay5.Enabled = chkEnabled.Checked;
            chkDay6.Enabled = chkEnabled.Checked;
            dtStart.Enabled = chkEnabled.Checked;
            dtEnd.Enabled = chkEnabled.Checked;
            btnAdd.Enabled = chkEnabled.Checked;
            btnSave.Enabled = chkEnabled.Checked;
            lvList.Enabled = chkEnabled.Checked;
        }
        private void OnScheduleTimer(object sender, EventArgs e)
        {
            if (!chkEnabled.Checked)
            {
                // turn it off and get out
                m_cScheduleTimer.Stop();
                m_cScheduleTimer.Enabled = false;
                return;
            }

            // if not loaded, load up the schedule data
            if (m_cScheduleList.Count.Equals(0))
                m_cScheduleList = LoadScheduleDataOnly();

            if (m_cScheduleList.Count.Equals(0) && m_bStartTime)
                return;// no records, manually started, get out

            int iDayOfWeek = ((int)DateTime.Today.DayOfWeek);
            bool bShouldBeMining = false;

            foreach (ScheduleEvent cEvent in m_cScheduleList)
            {
                if (cEvent.DayOfWeek.Equals(iDayOfWeek))
                {
                    // this record is for today
                    // let see if we should be running or not.
                    DateTime cStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, cEvent.StartTime.Hour, cEvent.StartTime.Minute, cEvent.StartTime.Second);
                    DateTime cEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, cEvent.EndTime.Hour, cEvent.EndTime.Minute, cEvent.EndTime.Second);
                    if (!bShouldBeMining)
                        bShouldBeMining = (DateTime.Now.CompareTo(cStart) >= 0 && DateTime.Now.CompareTo(cEnd) <= 0);
                }
            }
            if (bShouldBeMining && !m_bStartTime)
            {
                // start the miner
                BtnStartMining_Click(null, null);
                PushStatusMessage("Mining started by scheduler.");
            }
            else if (!bShouldBeMining && m_bStartTime)
            {
                // stop the miner
                BtnStopMining_Click(null, null);
                PushStatusMessage("Mining stopped by scheduler.");
            }
        }
        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            EnabledScheduleControls();
            SaveScheduleData();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // build list of all events
            List<ScheduleEvent> cEvents = new List<ScheduleEvent>();
            #region first, get all the new events
            CheckBox[] cCBs = { chkDay0, chkDay1, chkDay2, chkDay3, chkDay4, chkDay5, chkDay6 };
            foreach (CheckBox cCB in cCBs)
            {
                if (!cCB.Checked)
                    continue;
                string sIndex = cCB.Name.Substring(6, 1);
                int iIndex = int.Parse(sIndex);
                ScheduleEvent cEvent = new ScheduleEvent(iIndex, dtStart.Value, dtEnd.Value);
                cEvents.Add(cEvent);
            }
            #endregion
            #region add all existing events
            foreach (ListViewItem cItem in lvList.Items)
            {
                ScheduleEvent cEvent = new ScheduleEvent(GetDayIndex(cItem.SubItems[0].Text), DateTime.Parse(cItem.SubItems[1].Text), DateTime.Parse(cItem.SubItems[2].Text));
                cEvents.Add(cEvent);
            }
            #endregion
            #region sort the list
            cEvents.Sort();
            #endregion
            #region re-populate the listview control
            lvList.Items.Clear();
            foreach (ScheduleEvent cEvent in cEvents)
            {
                ListViewItem lvi = new ListViewItem(((DayOfWeek)cEvent.DayOfWeek).ToString());
                lvi.SubItems.Add(cEvent.StartTime.ToShortTimeString());
                lvi.SubItems.Add(cEvent.EndTime.ToShortTimeString());
                lvList.Items.Add(lvi);
            }
            #endregion
        }
        private int GetDayIndex(string sDay)
        {
            switch (sDay.ToLower())
            {
                case "sunday":
                    return 0;
                case "monday":
                    return 1;
                case "tuesday":
                    return 2;
                case "wednesday":
                    return 3;
                case "thursday":
                    return 4;
                case "friday":
                    return 5;
                case "saturday":
                    return 6;
            }
            return -1;
        }
        #endregion
        #region custom drawing methods
        private void lvList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;

                using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Bold))
                {
                    e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.Black, e.Bounds, sf);
                }
            }
        }
        private void lvList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;

                using (var itemFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular))
                {
                    Brush cBrush = Brushes.Black;
                    if (e.Item.Selected)
                    {
                        e.Graphics.FillRectangle(Brushes.LightSlateGray, e.Bounds);
                        cBrush = Brushes.White;
                    }
                    e.Graphics.DrawString(e.Item.Text, itemFont, cBrush, e.Bounds, sf);
                }
            }
        }
        private void lvList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;

                using (var itemFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular))
                {
                    Brush cBrush = Brushes.Black;
                    if (e.Item.Selected)
                    {
                        e.Graphics.FillRectangle(Brushes.LightSlateGray, e.Bounds);
                        cBrush = Brushes.White;
                    }
                    e.Graphics.DrawString(e.SubItem.Text, itemFont, cBrush, e.Bounds, sf);
                }
            }
        }
        #endregion
    }
    public class PRICE_Rootobject
    {
        public bool status { get; set; }
        public PRICE_Data data { get; set; }
    }
    public class PRICE_Data
    {
        public float price_btc { get; set; }
        public float price_usd { get; set; }
        public float price_eur { get; set; }
        public float price_rur { get; set; }
        public float price_cny { get; set; }
    }

    public class ScheduleEvent : IComparable
    {
        #region member definitions (private)
        private int m_iDayOfWeek = 0;
        private DateTime m_cStartTime = new DateTime();
        private DateTime m_cEndTime = new DateTime();
        #endregion
        #region property declarations (public)
        public int DayOfWeek
        {
            get { return m_iDayOfWeek; }
            set { m_iDayOfWeek = value; }
        }
        public DateTime StartTime
        {
            get { return m_cStartTime; }
            set { m_cStartTime = value; }
        }
        public DateTime EndTime
        {
            get { return m_cEndTime; }
            set { m_cEndTime = value; }
        }
        #endregion
        #region constructors
        public ScheduleEvent()
        {
        }
        public ScheduleEvent(int iDayOfWeek, DateTime cStartTime, DateTime cEndTime)
        {
            DayOfWeek = iDayOfWeek;
            StartTime = cStartTime;
            EndTime = cEndTime;
        }
        #endregion
        #region utility methods
        int IComparable.CompareTo(object cObj)
        {
            ScheduleEvent cEvent = (ScheduleEvent)cObj;
            if (cEvent.DayOfWeek > DayOfWeek)
                return -1;
            if (cEvent.DayOfWeek < DayOfWeek)
                return 1;
            if (cEvent.StartTime > StartTime)
                return -1;
            if (cEvent.StartTime < StartTime)
                return 1;
            return 0;
        }
        #endregion
    }
}
