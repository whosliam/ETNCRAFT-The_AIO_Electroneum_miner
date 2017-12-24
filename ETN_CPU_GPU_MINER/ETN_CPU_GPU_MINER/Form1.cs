using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.IO;
using OpenHardwareMonitor.Hardware;
using ETNCRAFT;
using Microsoft.Win32;
using System.Linq;
using System.Net;

namespace ETN_CPU_GPU_MINER
{
    public partial class Form1 : Form
    {
        public static string m_Version = "(V1.6.1)";
        public bool b_FormLoaded = false;
        public static string m_sAggHashData = "";
        public static string m_MiningURL = "";
        public static string m_PoolWebsiteURL = "";
        public bool m_bStartTime = false;
        private Stopwatch stopwatch = new Stopwatch();
        private Logger Logger = new Logger();
        private Messager Messager = new Messager();
        RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);

        #region Form Initialization

        public Form1()
        {
            Messager.InitializeMessager(Logger);
            InitializeComponent();
            LoadPoolListFromWebsite();

            //Check Registry for autoload & if user is new
            if (CheckRegistry("AutoLoad"))
                LoadConfig("config_templates/ENTCRAFT.mcf");
            else
                LoadConfig("config_templates/ENTCRAFT-DEFAULT.mcf");
            xmr_stak_perf_box.SelectedItem = xmr_stak_perf_box.Items[0];
            cpuorgpu.SelectedItem = cpuorgpu.Items[0];
            gpubrand.Visible = false;
            lbl_gpubrand.Visible = false;
            gpubrand.SelectedItem = gpubrand.Items[0];
            //Spool up timers
            GetTemps();
            //This is to keep the event handlers from fireing when the form load. Just wrapp functions in this.
            b_FormLoaded = true;
            this.FormClosing += new FormClosingEventHandler(CloseForm);
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            Messager.PushMessage("ETNCRAFT window closed, beginning process cleanup.");
            EndProcesses();
        }


        #endregion

        #region Control Handlers

        #region Click Handlers

        private void BtnStartMining_Click(object sender, EventArgs e)
        {
            //Auto Save 
            //if (chkAutoLoadConfig.Checked)

            SaveConfig();

            if (!IsWalletValid())
                return;
            if (double.Parse(threads.Text) <= 1)
                threads.Text = "1";

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[0] && miner_type.SelectedItem == miner_type.Items[1])
            {
                #region CPU MINER
                PushStatusMessage("Spawning cpuminer");
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = Application.StartupPath + "\\app_assets\\cpuminer.exe",
                    Arguments = "-a cryptonight -o stratum+tcp://" + m_MiningURL + ":" + port.Text + " -u " + wallet_address.Text.Replace(" ", "") + " -p x -t " + threads.Text + "pause",
                    WorkingDirectory = Application.StartupPath + "\\app_assets",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
                process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) => PushWorkStatusMessage("output>>" + eOut.Data + "\r\n");
                // Console.WriteLine("output>>" + eOut.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) => PushWorkStatusMessage("error>>" + eErr.Data + "\r\n");
                // Console.WriteLine("error>>" + eErr.Data);
                process.BeginErrorReadLine();
                #endregion
            }

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[1] && gpubrand.SelectedItem == gpubrand.Items[0] && miner_type.SelectedItem == miner_type.Items[1])
            {
                #region CC MINER
                PushStatusMessage("Spawning ccminer");
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = Application.StartupPath + "\\app_assets\\ccminer.exe",
                    Arguments = "-o stratum+tcp://" + m_MiningURL + ":" + port.Text + " -u " + wallet_address.Text.Replace(" ", "") + " -p x -t " + threads.Text + "pause",
                    WorkingDirectory = Application.StartupPath + "\\app_assets",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });

                process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) => PushWorkStatusMessage("output>>" + eOut.Data);
                // Console.WriteLine("output>>" + eOut.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) => PushWorkStatusMessage("error>>" + eErr.Data);
                // Console.WriteLine("error>>" + eErr.Data);
                process.BeginErrorReadLine();
                #endregion
            }

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[1] && gpubrand.SelectedItem == gpubrand.Items[1])
            {
                #region UPDATE CONFIG
                string FILE_NAME_AMD = "app_assets/config.txt";
                if (File.Exists(FILE_NAME_AMD) == false)
                {
                    File.Create(FILE_NAME_AMD).Dispose();
                    PushStatusMessage("config.txt created");
                }
                else
                {
                    File.Delete(FILE_NAME_AMD);
                    PushStatusMessage("old config.txt deleted");
                    File.Create(FILE_NAME_AMD).Dispose();
                    PushStatusMessage("config.txt created");
                }
                File.Copy(@"config_templates/config-template.txt", @"app_assets/config.txt", true);
                //This can done way better but i can't be assed
                string fileReader = System.Convert.ToString((new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.ReadAllText(@"app_assets/config.txt").Replace("threads_replace", threads.Text));
                fileReader = fileReader.Replace("address_replace", m_MiningURL + ":" + port.Text);
                fileReader = fileReader.Replace("wallet_replace", wallet_address.Text.Replace(" ", ""));
                int index = System.Convert.ToInt32(threads.Text);
                while (index <= 15)
                {
                    fileReader = fileReader.Replace("{ \"index\" : " + Convert.ToString(index) + ", \"intensity\" : 1000, \"worksize\" : 8, \"affine_to_cpu\" : false },", "");
                    index++;
                }
                (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.WriteAllText(@"app_assets/config.txt", fileReader, false);
                #endregion
                #region XMR STAK AMD MINER
                PushStatusMessage("Spawning xmr-stak-amd miner");
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = Application.StartupPath + "\\app_assets\\xmr-stak-amd.exe",
                    WorkingDirectory = Application.StartupPath + "\\app_assets",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });

                process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) => PushWorkStatusMessage("output>>" + eOut.Data);
                // Console.WriteLine("output>>" + eOut.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) => PushWorkStatusMessage("error>>" + eErr.Data);
                // Console.WriteLine("error>>" + eErr.Data);
                process.BeginErrorReadLine();
                #endregion
            }

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[1] && gpubrand.SelectedItem == gpubrand.Items[0] && miner_type.SelectedItem == miner_type.Items[0] && xmr_stak_perf_box.SelectedItem == xmr_stak_perf_box.Items[0])
            {
                #region UPDATE CONFIG
                string FILE_NAME_NV = "app_assets/config.txt";
                if (File.Exists(FILE_NAME_NV) == false)
                {
                    File.Create(FILE_NAME_NV).Dispose();
                    PushStatusMessage("config.txt created");
                }
                else
                {
                    File.Delete(FILE_NAME_NV);
                    PushStatusMessage("old config.txt deleted");
                    File.Create(FILE_NAME_NV).Dispose();
                    PushStatusMessage("config.txt created");
                }
                File.Copy(@"config_templates/config-template-nv.txt", @"app_assets/config.txt", true);
                //This can done way better but i can't be assed
                string fileReader = System.Convert.ToString((new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.ReadAllText(@"app_assets/config.txt").Replace("threads_replace", threads.Text));
                fileReader = fileReader.Replace("address_replace", m_MiningURL + ":" + port.Text);
                fileReader = fileReader.Replace("wallet_replace", wallet_address.Text.Replace(" ", ""));
                int index = System.Convert.ToInt32(threads.Text);
                while (index <= 15)
                {
                    fileReader = fileReader.Replace("{ \"index\" : " + System.Convert.ToString(index) + ",\"threads\" : 16, \"blocks\" : 14,\"bfactor\" : 6, \"bsleep\" :  25,\"affine_to_cpu\" : false,},", "");
                    index++;
                }
                (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.WriteAllText(@"app_assets/config.txt", fileReader, false);
                #endregion
                #region XMR STAK NVIDIA MINER
                PushStatusMessage("Spawning xmr-stak-nvidia miner");
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = Application.StartupPath + "\\app_assets\\xmr-stak-nvidia.exe",
                    WorkingDirectory = Application.StartupPath + "\\app_assets",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });

                process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) => PushWorkStatusMessage("output>>" + eOut.Data);
                // Console.WriteLine("output>>" + eOut.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) => PushWorkStatusMessage("error>>" + eErr.Data);
                // Console.WriteLine("error>>" + eErr.Data);
                process.BeginErrorReadLine();
                #endregion
            }

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[1] && gpubrand.SelectedItem == gpubrand.Items[0] && miner_type.SelectedItem == miner_type.Items[0] && xmr_stak_perf_box.SelectedItem == xmr_stak_perf_box.Items[1])
            {
                #region UPDATE CONFIG
                string FILE_NAME_NV = "app_assets/config.txt";
                if (File.Exists(FILE_NAME_NV) == false)
                {
                    File.Create(FILE_NAME_NV).Dispose();
                    PushStatusMessage("config.txt created");
                }
                else
                {
                    File.Delete(FILE_NAME_NV);
                    PushStatusMessage("old config.txt deleted");
                    File.Create(FILE_NAME_NV).Dispose();
                    PushStatusMessage("config.txt created");
                }
                File.Copy(@"config_templates/config-template-nv-hp.txt", @"app_assets/config.txt", true);
                //This can done way better but i can't be assed
                string fileReader = System.Convert.ToString((new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.ReadAllText(@"app_assets/config.txt").Replace("threads_replace", threads.Text));
                fileReader = fileReader.Replace("address_replace", m_MiningURL + ":" + port.Text);
                fileReader = fileReader.Replace("wallet_replace", wallet_address.Text.Replace(" ", ""));
                int index = System.Convert.ToInt32(threads.Text);
                while (index <= 15)
                {
                    fileReader = fileReader.Replace("{ \"index\" : " + System.Convert.ToString(index) + ",\"threads\" : 32, \"blocks\" : 27,\"bfactor\" : 6, \"bsleep\" :  25,\"affine_to_cpu\" : false,},", "");
                    index++;
                }
                (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.WriteAllText(@"app_assets/config.txt", fileReader, false);
                #endregion
                #region XMR STAK NVIDIA MINER
                PushStatusMessage("Spawning xmr-stak-nvidia miner");
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = Application.StartupPath + "\\app_assets\\xmr-stak-nvidia.exe",
                    WorkingDirectory = Application.StartupPath + "\\app_assets",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });

                process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) =>
                    Console.WriteLine("output>>" + eOut.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) =>
                    Console.WriteLine("error>>" + eErr.Data);
                process.BeginErrorReadLine();
                #endregion
            }

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[0] && miner_type.SelectedItem == miner_type.Items[0] && hyperthread.Checked == true)
            {
                #region UPDATE CONFIG
                string FILE_NAME_CPU = "app_assets/config.txt";
                if (File.Exists(FILE_NAME_CPU) == false)
                {
                    File.Create(FILE_NAME_CPU).Dispose();
                    PushStatusMessage("config.txt created");
                }
                else
                {
                    File.Delete(FILE_NAME_CPU);
                    PushStatusMessage("old config.txt deleted");
                    File.Create(FILE_NAME_CPU).Dispose();
                    PushStatusMessage("config.txt created");
                }
                File.Copy(@"config_templates/config-template-cpu.txt", @"app_assets/config.txt", true);
                //This can done way better but i can't be assed
                string fileReader = System.Convert.ToString((new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.ReadAllText(@"app_assets/config.txt").Replace("threads_replace", threads.Text));
                fileReader = fileReader.Replace("address_replace", m_MiningURL + ":" + port.Text);
                fileReader = fileReader.Replace("wallet_replace", wallet_address.Text.Replace(" ", ""));
                int index = System.Convert.ToInt32((double.Parse(threads.Text) * 2) - 1);
                while (index <= 14)
                {
                    fileReader = fileReader.Replace("{ \"low_power_mode\" : false, \"no_prefetch\" : true, \"affine_to_cpu\" : " + System.Convert.ToString(index) + " },", "");
                    index++;
                }
                (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.WriteAllText(@"app_assets/config.txt", fileReader, false);
                #endregion
                #region XMR STAK CPU MINER
                PushStatusMessage("Spawning xmr-stak-cpu miner");
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = Application.StartupPath + "\\app_assets\\xmr-stak-cpu.exe",
                    WorkingDirectory = Application.StartupPath + "\\app_assets",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });

                process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) =>
                    Console.WriteLine("output>>" + eOut.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) =>
                    Console.WriteLine("error>>" + eErr.Data);
                process.BeginErrorReadLine();
                #endregion
            }

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[0] && miner_type.SelectedItem == miner_type.Items[0] && hyperthread.Checked == false)
            {
                #region UPDATE CONFIG
                string FILE_NAME_CPU = "app_assets/config.txt";
                if (File.Exists(FILE_NAME_CPU) == false)
                {
                    File.Create(FILE_NAME_CPU).Dispose();
                    PushStatusMessage("config.txt created");
                }
                else
                {
                    File.Delete(FILE_NAME_CPU);
                    PushStatusMessage("old config.txt deleted");
                    File.Create(FILE_NAME_CPU).Dispose();
                    PushStatusMessage("config.txt created");
                }
                File.Copy(@"config_templates/config-template-cpu-le.txt", @"app_assets/config.txt", true);
                //This can done way better but i can't be assed
                string fileReader = System.Convert.ToString((new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.ReadAllText(@"app_assets/config.txt").Replace("threads_replace", threads.Text));
                fileReader = fileReader.Replace("address_replace", m_MiningURL + ":" + port.Text);
                fileReader = fileReader.Replace("wallet_replace", wallet_address.Text.Replace(" ", ""));
                int index = System.Convert.ToInt32(threads.Text);
                while (index <= 14)
                {
                    fileReader = fileReader.Replace("{ \"low_power_mode\" : false, \"no_prefetch\" : true, \"affine_to_cpu\" : " + System.Convert.ToString(index) + " },", "");
                    index++;
                }
                (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.WriteAllText(@"app_assets/config.txt", fileReader, false);
                #endregion

                #region XMR STAK CPU MINER
                PushStatusMessage("Spawning xmr-stak-cpu miner");
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = Application.StartupPath + "\\app_assets\\xmr-stak-cpu.exe",
                    WorkingDirectory = Application.StartupPath + "\\app_assets",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });

                process.OutputDataReceived += (object SenderOut, DataReceivedEventArgs eOut) =>
                    Console.WriteLine("output>>" + eOut.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object SenderErr, DataReceivedEventArgs eErr) =>
                    Console.WriteLine("error>>" + eErr.Data);
                process.BeginErrorReadLine();
                #endregion
            }
            PushStatusMessage("config.txt updated");

            //Start header Timer for app run time
            m_bStartTime = true;
            stopwatch.Start();
            //Start Hash textbox reset timer
            HashTimer();
        }

        private void BtnStopMining_Click(object sender, EventArgs e)
        {
            //Stop Timer
            m_bStartTime = false;
            stopwatch.Stop();

            EndProcesses();
        }

        private void BtnDiagnostics_Click(object sender, EventArgs e)
        {
            string string1 = "";
            string1 = miner_type.SelectedItem.ToString();
            if (string1 == "xmr-stak-cpu")
            {
                PushStatusMessage("xmr-stak-cpu miner detected" + Constants.vbNewLine + "   ERROR HELP 1: If receiving a 'MEMORY ALLOC FAILED: VirtualAlloc failed' error, disable all auto-starting applications and run the miner after a reboot. You do not have enough free ram.");
                PushStatusMessage("   ERROR HELP 2: If receiving msvcp140.dll and vcruntime140.dll not available errors, download and install the following runtime package: https://www.microsoft.com/en-us/download/details.aspx?id=17657");
                PushStatusMessage("   ERROR HELP 3: If it still doesn't work, switch the miner backend to cpuminer-multi");
                PushStatusMessage("   ERROR HELP 4: Perhaps you have too many threads");
                PushStatusMessage("Thread count halved");
                threads.Text = System.Convert.ToString(double.Parse(threads.Text) / 2);
            }
            if (cpuorgpu.SelectedItem == cpuorgpu.Items[1] && double.Parse(threads.Text) >= 2)
            {
                port.Text = "7777";
                PushStatusMessage("gpu mining with 2+GPUs detected, setting port to 7777");
            }
            if (string1 == "xmr-stak-nvidia")
            {
                PushStatusMessage("xmr-stak-nvidia miner detected" + Constants.vbNewLine + "   ERROR HELP 1: If receiving a 'MEMORY ALLOC FAILED: VirtualAlloc failed' error, disable all auto-starting applications and run the miner after a reboot. You do not have enough free ram.");
                PushStatusMessage("   ERROR HELP 2: If receiving msvcp140.dll and vcruntime140.dll not available errors, download and install the following runtime package: https://www.microsoft.com/en-us/download/details.aspx?id=17657");
                PushStatusMessage("   ERROR HELP 3: If it still doesn't work, switch the miner backend to ccminer, xmr-stak-nvidia does not support more than 16 GPUs");
                PushStatusMessage("   ERROR HELP 4: Are you using High Performance mode? Switching to standard mode for compatibility");
                xmr_stak_perf_box.SelectedItem = xmr_stak_perf_box.Items[0];

            }
            if (string1 == "xmr-stak-amd")
            {
                PushStatusMessage("xmr-stak-amd miner detected" + Constants.vbNewLine + "   ERROR HELP 1: If receiving a 'MEMORY ALLOC FAILED: VirtualAlloc failed' error, disable all auto-starting applications and run the miner after a reboot. You do not have enough free ram.");
                PushStatusMessage("   ERROR HELP 2: If receiving msvcp140.dll and vcruntime140.dll not available errors, download and install the following runtime package: https://www.microsoft.com/en-us/download/details.aspx?id=17657");
                PushStatusMessage("   ERROR HELP 3:If it still doesn't work, reduce the number of GPUs xmr-stak-amd does not support more than 16 GPUs");
            }
            if (string1 == "ccminer")
            {
                PushStatusMessage("ccminer detected" + Constants.vbNewLine + "   ERROR HELP 1: Your GPU is probably not supported (some GTX x80 Series and 10xx series not supported. Switch to xmr-stak-nvidia");
            }
            if (string1 == "cpuminer-multi")
            {
                PushStatusMessage("cpuminer-multi detected" + Constants.vbNewLine + "   ERROR HELP 1: Your CPU is probably not supported (This build it built for Intel Core-I series, Switch to xmr-stak-cpu");
                PushStatusMessage("   ERROR HELP 2: Perhaps you have too many threads");
                PushStatusMessage("Thread count halved");
                threads.Text = System.Convert.ToString(double.Parse(threads.Text) / 2);
            }
            if (!(cboPool.SelectedItem == cboPool.Items[0] || cboPool.SelectedItem == cboPool.Items[1] || cboPool.SelectedItem == cboPool.Items[2] || cboPool.SelectedItem == cboPool.Items[3] || cboPool.SelectedItem == cboPool.Items[4] || cboPool.SelectedItem == cboPool.Items[5] || cboPool.SelectedItem == cboPool.Items[6] || cboPool.SelectedItem == cboPool.Items[7] || cboPool.SelectedItem == cboPool.Items[8] || cboPool.SelectedItem == cboPool.Items[9]))
            {
                PushStatusMessage("custom pool detected");
                PushStatusMessage("   ERROR HELP 1: Custom pool detected, is it the correct address?");
                PushStatusMessage("   ERROR HELP 2: Did you accidentally add the port number in the config file?");
            }
            if (cboPool.SelectedItem == cboPool.Items[9])
            {
                PushStatusMessage("custom pool detected");
                PushStatusMessage("   ERROR HELP 1: Custom pool detected, is it the correct address?");
            }
        }

        private void BtnCheckBalance_Click(object sender, EventArgs e)
        {
            string webAddress = m_PoolWebsiteURL;
            Process.Start(webAddress);
        }

        private void BtnClearWallet_Click(object sender, EventArgs e)
        {
            wallet_address.Text = "";
            status.Text = Messager.ClearMessages();
        }

        private void BtnOpenLog_Click(object sender, EventArgs e)
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Logger.GetLogFilePath()
            };

            process.Start();
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
                lbl_threads.Text = "GPU Count:";
                threads.Text = "1";
                port.Text = "5555";
                lbl_gpubrand.Visible = true;
                gpubrand.Visible = true;
                miner_type.Enabled = true;
                miner_type.Items.Clear();
                miner_type.Items.Add("xmr-stak-nvidia");
                miner_type.Items.Add("ccminer");
                miner_type.SelectedItem = miner_type.Items[1];
                xmr_stak_perf_box.Visible = false;
                xmr_notice.Visible = false;
                stak_nvidia_perf.Visible = false;
            }

            if (cpuorgpu.SelectedItem == cpuorgpu.Items[0])
            {
                //change threadcount to gpu count
                lbl_threads.Text = "Thread Count:";
                threads.Text = "4";
                lbl_gpubrand.Visible = false;
                gpubrand.SelectedItem = gpubrand.Items[0];
                gpubrand.Visible = false;
                miner_type.Enabled = true;
                miner_type.Items.Clear();
                miner_type.Items.Add("xmr-stak-cpu");
                miner_type.Items.Add("cpuminer-multi");
                miner_type.SelectedItem = miner_type.Items[1];
                xmr_stak_perf_box.Visible = false;
                stak_nvidia_perf.Visible = false;
                xmr_notice.Visible = false;
            }

        }

        private void gpubrand_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (gpubrand.SelectedItem == gpubrand.Items[0] && cpuorgpu.SelectedItem == cpuorgpu.Items[1])
            {
                miner_type.Enabled = true;
                miner_type.Items.Clear();
                miner_type.Items.Add("xmr-stak-nvidia");
                miner_type.Items.Add("ccminer");
                miner_type.SelectedItem = miner_type.Items[1];
                xmr_stak_perf_box.Visible = false;
                stak_nvidia_perf.Visible = false;
                xmr_notice.Visible = false;
            }
            if (gpubrand.SelectedItem == gpubrand.Items[1] && cpuorgpu.SelectedItem == cpuorgpu.Items[1])
            {
                miner_type.Items.Clear();
                miner_type.Items.Add("xmr-stak-amd");
                miner_type.Items.Add("xmr-stak-amd");
                miner_type.SelectedItem = miner_type.Items[0];
                miner_type.Enabled = false;
                xmr_stak_perf_box.Visible = false;
                stak_nvidia_perf.Visible = false;
                xmr_notice.Visible = false;
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
        private void miner_type_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cpuorgpu.SelectedItem == cpuorgpu.Items[1] && gpubrand.SelectedItem == gpubrand.Items[0] && miner_type.SelectedItem == miner_type.Items[0])
            {
                xmr_stak_perf_box.Visible = true;
                stak_nvidia_perf.Visible = true;
                xmr_stak_perf_box.SelectedItem = xmr_stak_perf_box.Items[0];
                xmr_notice.Visible = true;
            }
            else
            {
                xmr_stak_perf_box.Visible = false;
                stak_nvidia_perf.Visible = false;
                xmr_notice.Visible = false;
            }

        }

        private void xmr_stak_perf_box_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                        CreateOrEditRegistryKey("AutoLoad", false);
                        chkAutoLoadConfig.Checked = false;
                    }
                    else
                    {
                        SaveConfig();
                        CreateOrEditRegistryKey("AutoLoad", chkAutoLoadConfig.Checked);

                    }
                }
                else
                    CreateOrEditRegistryKey("AutoLoad", false);
            }



        }
        #endregion

        #endregion

        #region Utility Methods

        #region Config/Registry
        private void LoadConfig(string sConfigFilePath)
        {
            PushStatusMessage("Loading ETNCRAFT config");
            try
            {
                string[] config_contents_load = File.ReadAllLines(sConfigFilePath);
                wallet_address.Text = config_contents_load[0];
                cboPool.SelectedItem = config_contents_load[1];
                txtCustomPool.Text = config_contents_load[2];
                cboPool.SelectedValue = config_contents_load[3];
                port.Text = config_contents_load[4];
                cpuorgpu.SelectedItem = config_contents_load[6];
                gpubrand.SelectedItem = config_contents_load[7];
                miner_type.SelectedItem = config_contents_load[10];
                xmr_stak_perf_box.SelectedItem = config_contents_load[8];
                threads.Text = config_contents_load[5];
                string ht_checkstate = config_contents_load[9];
                if (ht_checkstate == "yes")
                    hyperthread.Checked = true;
                else if (ht_checkstate == "no")
                    hyperthread.Checked = false;
            }
            catch (Exception e)
            {
                PushStatusMessage(e.Message);
            }

        }
        private void SaveConfig()
        {
            File.Delete("config_templates\\ENTCRAFT.mcf");
            File.Create("config_templates\\ENTCRAFT.mcf").Dispose();

            string ht_checkstate = "no";
            if (hyperthread.Checked == true)
                ht_checkstate = "yes";
            else if (hyperthread.Checked == false)
                ht_checkstate = "no";
            string config_contents_save = wallet_address.Text.Replace(" ", "") + Constants.vbNewLine + System.Convert.ToString(cboPool.SelectedItem) + Constants.vbNewLine + txtCustomPool.Text + Constants.vbNewLine + m_MiningURL + Constants.vbNewLine + port.Text + Constants.vbNewLine + threads.Text + Constants.vbNewLine + System.Convert.ToString(cpuorgpu.SelectedItem) + Constants.vbNewLine + System.Convert.ToString(gpubrand.SelectedItem) + Constants.vbNewLine + System.Convert.ToString(xmr_stak_perf_box.SelectedItem) + Constants.vbNewLine + ht_checkstate + Constants.vbNewLine + System.Convert.ToString(miner_type.SelectedItem);
            (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.WriteAllText("config_templates\\ENTCRAFT.mcf", config_contents_save, true);
            PushStatusMessage("ENTCRAFT.mcf deleted & recreated");

        }
        private bool CheckRegistry(string sKey)
        {
            PushStatusMessage("Checking for ETNCRAFT registry keys");
            bool bAutoLoad = false;
            var key = localMachine.OpenSubKey("SOFTWARE\\ETNCRAFT", true);
            if (key != null)
            {
                PushStatusMessage(sKey + " registry key found!");
                object keyValue = key.GetValue(sKey);
                //Set Return value
                bAutoLoad = Convert.ToBoolean(keyValue);
                //Set Check box on advanced setting tab
                chkAutoLoadConfig.Checked = bAutoLoad;
            }
            else
            {
                PushStatusMessage("No registry keys found.");
                PushStatusMessage("First run detected");
                //Added miner here since they wouldnt have a key prior to running so it will throw a null value which falls in here.
                CreateOrEditRegistryKey("NewMiner", true);
                PushStatusMessage("NewMiner key created");
                //Push Message
                DialogResult UserInput = MessageBox.Show("Welcome new miner!\r\nThe help tab has been pre selected.\r\nPlease read and follow the directions.", "WELCOME!", MessageBoxButtons.OK);
                //Load Help tab
                tabs.SelectedTab = tbHelp;
            }
            return bAutoLoad;
        }
        private void CreateOrEditRegistryKey(string sKey, bool bValue)
        {
            var reg = localMachine.OpenSubKey("SOFTWARE\\ETNCRAFT", true);
            if (reg == null)
            {
                PushStatusMessage("creating ETNCRAFT " + sKey + " registry key. Set to " + bValue);
                reg = localMachine.CreateSubKey("SOFTWARE\\ETNCRAFT");
            }
            if (chkAutoLoadConfig.Checked && reg.GetValue("") != null)
            {
                // PushStatusMessage("Setting ETNCRAFT autoload registry value to \"TRUE\"");
                reg.SetValue(sKey, bValue);
            }
            else
            {
                // PushStatusMessage("Setting ETNCRAFT autoload registry value to \"FALSE\"");
                reg.SetValue(sKey, bValue);
            }
            reg.Close();

        }

        #endregion

        #region Timers/Temperature Data
        //System usage timer
        void timer_Tick(object sender, EventArgs e)
        {
            #region Temp Interval
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
                            lblCPUTemp.Text += (String.Format("{0} = {1}C", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value") + "\r\n");
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
                            lblGPUTemp.Text += (String.Format("{0} = {1}C", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value") + "\r\n");
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
            #endregion

            #endregion
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
        //clear hash data text field
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
        }

        Computer myComputer;
        Timer timer = new Timer { Enabled = true, Interval = 1000 };
        public void GetTemps()
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
        #endregion

        #region Logger/Messager

        /// <summary>
        /// Push a Message to the Message Area
        /// </summary>
        /// <param name="Message"></param>
        private void PushStatusMessage(string Message)
        {
            status.Text = Messager.PushMessage(Message);
            status.SelectionStart = status.Text.Length;
            status.ScrollToCaret();
        }

        private void PushWorkStatusMessage(string Message)
        {
            m_sAggHashData += Message + "\r\n";
            ThreadHelperClass.SetText(this, WorkStatus, m_sAggHashData);
        }

        /// <summary>
        /// Clear existing messages from the Message Area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearMessagesButton_Click(object sender, EventArgs e)
        {
            status.Text = Messager.ClearMessages();
        }

        #endregion

        private bool IsWalletValid()
        {
            if (wallet_address.Text.Equals("Enter Public Wallet Here") || wallet_address.Text.Equals(""))
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
            // Get Process Arrays
            Process[] ArrProcessCPU = Process.GetProcessesByName("cpuminer");
            Process[] ArrProcessNV = Process.GetProcessesByName("ccminer");
            Process[] ArrProcessAMD = Process.GetProcessesByName("xmr-stak-amd");
            Process[] ArrProcessNVXMR = Process.GetProcessesByName("xmr-stak-nvidia");
            Process[] ArrProcessCPUXMR = Process.GetProcessesByName("xmr-stak-cpu");

            // Build Aggregate Process Array
            int ProcessCount = ArrProcessCPU.Length + ArrProcessNV.Length + ArrProcessAMD.Length + ArrProcessNVXMR.Length + ArrProcessCPUXMR.Length;
            Process[] ArrProcesses = new Process[ProcessCount];
            int CopyStartInd = 0;
            ArrProcessCPU.CopyTo(ArrProcesses, CopyStartInd);
            CopyStartInd += ArrProcessCPU.Length;
            ArrProcessNV.CopyTo(ArrProcesses, CopyStartInd);
            CopyStartInd += ArrProcessNV.Length;
            ArrProcessAMD.CopyTo(ArrProcesses, CopyStartInd);
            CopyStartInd += ArrProcessAMD.Length;
            ArrProcessNVXMR.CopyTo(ArrProcesses, CopyStartInd);
            CopyStartInd += ArrProcessNVXMR.Length;
            ArrProcessCPUXMR.CopyTo(ArrProcesses, CopyStartInd);
            CopyStartInd += ArrProcessCPUXMR.Length;

            // Kill Processes
            foreach (Process p in ArrProcesses)
            {
                PushStatusMessage("Killing Process : " + p.ProcessName + " ( pid " + p.Id + ")");
                p.Kill();
            }

            PushStatusMessage("All Processes Killed!");
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
    }
    #region Pool Classes and collection/data parse
    public class Pools
    {
        public int iID { get; set; }
        public string sDisplayName { get; set; }
        public string sPoolMiningURL { get; set; }
        public string sPoolWebsite { get; set; }
        public string sPoolInformation { get; set; }
    }
    public class PoolsCollection : System.Collections.Generic.List<Pools>
    {
        public int Load()
        {
            this.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(Application.StartupPath + "\\app_assets\\pools.txt"))
                {
                    while (sr.Peek() >= 0)
                    {
                        Pools cItem = new Pools();
                        string str;
                        string[] strArray;
                        str = sr.ReadLine();
                        strArray = str.Split(',');
                        cItem.iID = int.Parse(strArray[0]);
                        cItem.sDisplayName = strArray[1];
                        cItem.sPoolMiningURL = strArray[2];
                        cItem.sPoolWebsite = strArray[3];
                        cItem.sPoolInformation = strArray[4];
                        Add(cItem);
                    }
                }
            }
            catch (Exception) { }
            return this.Count;
        }
    }
    public class PoolComboBoxItems
    {
        public string Value;
        public string Text;

        public PoolComboBoxItems(string val, string text)
        {
            Value = val;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    #endregion
}
