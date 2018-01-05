using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.IO;
using System.Text.RegularExpressions;
using OpenHardwareMonitor.Hardware;
using Microsoft.Win32;
using System.Net;

namespace ETN_CPU_GPU_MINER
{
    public partial class Form1 : Form
    {
        #region Global vars
        public static string m_Version; //= "(V1.7.1)";        
        public static string m_sAggHashData = "";
        public static string m_MiningURL = "";
        public static string m_PoolWebsiteURL = "";
        public static string m_sETNCRAFTCPULogFileLocation = Application.StartupPath + "\\app_assets\\ETN_CRAFT_CPU_LOG.txt";

        public bool b_FormLoaded = false;
        public bool m_bStartTime = false;
        public bool m_bDebugging = false;
        public bool m_bReadETNCRAFTULog = false;
        public bool m_bTempWarningModalIsOpen = false;

        private Stopwatch stopwatch = new Stopwatch();
        private Logger logger = new Logger("ETN_Craft");
        private Logger loggerPool = new Logger("ETN_Craft_Pool");
        private Messager messager = new Messager();
        private RegistryManager registryManager = new RegistryManager();
        public int m_iTemperatureAlert = 90;

        //RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        #endregion

        #region Form Initialization

        public Form1()
        {
            m_Version = registryManager.GetVersion();
            messager.InitializeMessager(logger);
            InitializeComponent();

            //Set version in window header
            this.Text = "ETNCRAFT (" + m_Version + ")";
            this.Update();
            LoadPoolListFromWebsite();

            // Check Registry for AutoLoad
            PushStatusMessage("Checking for ETNCRAFT registry keys");
            if (registryManager.GetAutoLoad())
            {
                PushStatusMessage("AutoLoad registry key loaded (true)");
                LoadConfig("config_templates/ENTCRAFT.mcf");
                LoadRegistryConfig();
            }
            else
            {
                PushStatusMessage("AutoLoad registry key loaded (false)");
                LoadConfig("config_templates/ENTCRAFT-DEFAULT.mcf");
            }

            // Check Registry for NewMiner
            if (registryManager.GetNewMiner())
            {
                PushStatusMessage("Welcome New Miner!");
                DialogResult UserInput = MessageBox.Show("Welcome new miner!\r\nThe help tab has been pre selected.\r\nPlease read and follow the directions.", "WELCOME!", MessageBoxButtons.OK);
                //Load Help tab
                tabs.SelectedTab = tbHelp;
            }

            cpuorgpu.SelectedItem = cpuorgpu.Items[0];
            //Spool up timers
            InitTemps();
            //This is to keep the event handlers from firing when the form load. Just wrap functions in this.
            b_FormLoaded = true;
            this.FormClosing += new FormClosingEventHandler(CloseForm);
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            logger.Warn("ETNCRAFT window closed, beginning process cleanup.");
            EndProcesses();
            registryManager.CloseRegistryKeys();
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
            PushStatusMessage("config.txt updated");
            //Start header Timer for app run time
            m_bStartTime = true;
            stopwatch.Start();
            //Start Hash textbox reset timer
            HashTimer();
        }
        private void SpawnMiner(string sComponent)
        {
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
            PushStatusMessage(sComponent + " config.txt created");
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
            //ADD THREAD MODIFICATIONS HERE FOR CPU & GPU -- If we decide to make this configurable again with new miner stak
            #endregion
            #region Spawn miner
            PushStatusMessage("Spawning ETNCRAFT " + sComponent + " miner");
            string sFileName = Application.StartupPath + "\\app_assets\\ETNCRAFT_" + sComponent + ".exe";
            string sWorkingDirectory = Application.StartupPath + "\\app_assets";
            if (m_bDebugging)
            {
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = sFileName,
                    WorkingDirectory = sWorkingDirectory
                });
            }
            else
            {
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = sFileName,
                    WorkingDirectory = sWorkingDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
                process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) => PushWorkStatusMessage(sComponent + "> " + eOut.Data);
                process.BeginOutputReadLine();
                process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) => PushWorkStatusMessage(sComponent + "> " + eErr.Data);
                process.BeginErrorReadLine();
            }
            #endregion
        }
        private void BtnStopMining_Click(object sender, EventArgs e)
        {
            //Stop Timer
            m_bStartTime = false;
            stopwatch.Stop();
            //Kill mining
            EndProcesses();
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
            open_config_dialog.Filter = "Miner Configuration Files (*.mcf*)|*.mcf";
            if (open_config_dialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
                LoadConfig(open_config_dialog.FileName);
        }

        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            save_config_dialog.Filter = "Miner Configuration Files (*.mcf*)|*.mcf";
            if (save_config_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SaveConfig();
        }

        private void BtnLoadDefaultConfig_Click(object sender, EventArgs e)
        {
            LoadConfig("config_templates/ENTCRAFT-DEFAULT.mcf");
        }

        private void LinkWalletGen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://my.electroneum.com/offline_paper_electroneum_walletV1.6.html");
        }
        #endregion

        #region DropDown Handlers

        private void cpuorgpu_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cpuorgpu.SelectedItem == cpuorgpu.Items[1])
            {
                //change threadcount to gpu count
                //lbl_threads.Text = "GPU Count:";
                //threads.Text = "1";
                port.Text = "5555";
            }

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[0])
            {
                //change threadcount to gpu count
                //lbl_threads.Text = "Thread Count:";
                //threads.Text = "4";
            }

        }

        private void pool_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //Taking the lazy way out. Its still better than it was. Id rather not have this condition but the whole data binding with the cbo is a pain.
            PoolsCollection cPoolCollection = new PoolsCollection();
            cPoolCollection.Load();
            foreach (var cItem in cPoolCollection)
            {
                if (cItem.sDisplayName.Equals(cboPool.SelectedItem.ToString()))
                {
                    m_MiningURL = cItem.sPoolMiningURL;
                    m_PoolWebsiteURL = cItem.sPoolWebsite;
                    PushStatusMessage(cItem.sDisplayName + " selected, " + cItem.sPoolWebsite + " | " + cItem.sPoolInformation);
                    break;
                }
            }
        }

        private void txtCustomPool_TextChanged(object sender, EventArgs e)
        {
            if (!txtCustomPool.Equals(""))
            {
                m_MiningURL = txtCustomPool.Text;
                //PushStatusMessage("custom pool selected[" + txtCustomPool.Text + "], make sure to add your pool address!");
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
            PushStatusMessage("AutoLoad registry key updated.");
        }

        #endregion

        #endregion

        #region Utility Methods

        #region Config/Registry

        private void LoadRegistryConfig()
        {
            PushStatusMessage("Loading ETNCRAFT config from registry");
            wallet_address.Text = registryManager.GetWalletId();

            chkAutoLoadConfig.Checked = registryManager.GetAutoLoad();
        }

        private void LoadConfig(string sConfigFilePath)
        {
            PushStatusMessage("Loading ETNCRAFT config from " + sConfigFilePath);
            try
            {
                string[] config_contents_load = File.ReadAllLines(sConfigFilePath);
                wallet_address.Text = config_contents_load[0];
                cboPool.SelectedItem = config_contents_load[1];
                txtCustomPool.Text = config_contents_load[2];
                cboPool.SelectedValue = config_contents_load[3];
                port.Text = config_contents_load[4];
                //threads.Text = config_contents_load[5];
                cpuorgpu.SelectedItem = config_contents_load[6];
                string ht_checkstate = config_contents_load[9];
            }
            catch (Exception e)
            {
                PushStatusMessage(e.Message);
            }

        }

        private void SaveConfig()
        {
            registryManager.SetWalletId(wallet_address.Text);

            File.Delete("config_templates\\ENTCRAFT.mcf");
            File.Create("config_templates\\ENTCRAFT.mcf").Dispose();

            string ht_checkstate = "no";
            string config_contents_save = "" + Constants.vbNewLine +
                System.Convert.ToString(cboPool.SelectedItem) + Constants.vbNewLine + txtCustomPool.Text + Constants.vbNewLine + m_MiningURL + Constants.vbNewLine + port.Text + Constants.vbNewLine + "" + Constants.vbNewLine + System.Convert.ToString(cpuorgpu.SelectedItem) + Constants.vbNewLine + "" + Constants.vbNewLine + "" + Constants.vbNewLine + ht_checkstate + Constants.vbNewLine + "";
            (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.WriteAllText("config_templates\\ENTCRAFT.mcf", config_contents_save, true);
            PushStatusMessage("ENTCRAFT.mcf deleted & recreated");

        }

        #endregion

        #region Timers/Temperature Data/CPU LOG READ
        #region Hash Timers & Miner log parse
        private void HashTimer()
        {
            Timer HashTxtTimer = new Timer();
            HashTxtTimer.Interval = 300000;//5 minutes
            HashTxtTimer.Tick += new System.EventHandler(Hashtxt_Tick);
            HashTxtTimer.Start();

        }
        private void Hashtxt_Tick(object sender, EventArgs e)
        {
            WorkStatus.Text = "Log Cleared!";
            m_sAggHashData = "";
        }
        #endregion

        #region Temp/Uptime Timers etc
        void timer_Tick(object sender, EventArgs e)
        {

            GetSysTemp();
            #region Timer in window header
            //DONT BE LAZY LIAM! MAKE THIS WORK ELSEWHERE
            WorkStatus.SelectionStart = WorkStatus.Text.Length;
            WorkStatus.ScrollToCaret();

            if (m_bStartTime)
            {
                this.Text = "ETNCRAFT" + m_Version + " | Uptime " + String.Format("{0}:{1}:{2}", stopwatch.Elapsed.Hours.ToString("00"), stopwatch.Elapsed.Minutes.ToString("00"), stopwatch.Elapsed.Seconds.ToString("00")); ;
                this.Update();
            }
            #endregion
        }
        Computer myComputer;
        Timer timer = new Timer { Enabled = true, Interval = 1000 };
        public void InitTemps()
        {

            timer.Tick += new EventHandler(timer_Tick);

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
                DialogResult UserInput = MessageBox.Show(sDevice + " Temps are above " + m_iTemperatureAlert.ToString() + " degrees!\r\nConsider turning fan speeds higher.\r\nIgnore warnings?", "WARNING!", MessageBoxButtons.YesNo);
                if (UserInput.Equals(DialogResult.Yes))
                    registryManager.SetIgnoreTempWarnings(true);
                else
                    registryManager.SetIgnoreTempWarnings(false);
            }

        }
        #endregion

        #region Logger/Messager

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void PushStatusMessage(string message)
        {
            if (message != null)
            {
                status.Text = messager.PushMessage(message);
                status.SelectionStart = status.Text.Length;
                status.ScrollToCaret();
            }
        }

        private void PushWorkStatusMessage(string message)
        {
            if (message != null && !message.Equals(""))
            {
                string cleanMessage = RemoveAnsiEscapes(message);
                m_sAggHashData += cleanMessage + "\r\n";
                ThreadHelperClass.SetText(this, WorkStatus, m_sAggHashData);
                loggerPool.Debug(cleanMessage);
            }
        }

        #endregion

        private string RemoveAnsiEscapes(string message)
        {
            //string cleanMessage = Regex.Replace(message, @"\\u(?<Value>[a-zA-Z0-9]{4})", "");
            return message;
        }

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
                    PushStatusMessage("Developer Wallet Address Selected! Thanks!");
                    return true;
                }
            }
            return true;
        }

        private void EndProcesses()
        {
            Process[] localAll = Process.GetProcesses();
            foreach (Process p in localAll)
            {
                //Kill XMR miners, ccminer & cpuminer
                if (p.ProcessName.Contains("ETNCRAFT"))
                {
                    PushStatusMessage("Killing Process : " + p.ProcessName + " ( pid " + p.Id + ")");
                    p.Kill();
                    PushStatusMessage(p.ProcessName + " Process Killed!");

                }
            }
        }

        private void LoadPoolListFromWebsite()
        {
            //Set Path
            string filepath = Application.StartupPath + "\\app_assets\\pools.txt";
            //Download doc from website
            WebClient webClient = new WebClient();
            webClient.DownloadFile("http://liamthrower.com/pools.txt", filepath);
            //Build Collection of pools
            PoolsCollection cAllPools = new PoolsCollection();
            cAllPools.Load();
            foreach (var citem in cAllPools)
                cboPool.Items.Add(new PoolComboBoxItems(citem.sPoolMiningURL, citem.sDisplayName));
            //Force selected item in dropdown list and fire the onselected change event which sets some global vars
            cboPool.SelectedIndex = 0;
            //pool_SelectedIndexChanged_1(cboPool, new EventArgs());
        }

        #endregion

        #endregion

        private void btnDeleteRegKeys_Click(object sender, EventArgs e)
        {
            MessageBox.Show(registryManager.DeleteRegistryKey(), "ETNCRAFT Services");
        }

    }
}
