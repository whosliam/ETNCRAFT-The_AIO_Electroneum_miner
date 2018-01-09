using System.Diagnostics;
using System.Windows.Forms;

namespace ETNCRAFT
{
    static class ProcessUtil
    {
        public static void CheckForExistingProcesses()
        {
            if (GetMinerProcessCount() > 0)
            {
                int count = GetMinerProcessCount();
                string msg = count + " existing etncraft-xmr process(es) found.\n End process(es)? \nYou probably should.";
                DialogResult UserInput = MessageBox.Show(msg, "Existing Process!", MessageBoxButtons.YesNo);
                if (UserInput == DialogResult.Yes)
                    EndProcesses();
            }
        }

        public static Process SpawnMinerProcess(string args, bool debug)
        {
            string sFileName = Application.StartupPath + "\\app_assets\\etncraft-xmr.exe";
            string sWorkingDirectory = Application.StartupPath + "\\app_assets";
            
            if (debug)
            {
                return Process.Start(new ProcessStartInfo()
                {
                    FileName = sFileName,
                    Arguments = args,
                    WorkingDirectory = sWorkingDirectory
                });
            }
            else
            {
                return  Process.Start(new ProcessStartInfo()
                {
                    FileName = sFileName,
                    WorkingDirectory = sWorkingDirectory,
                    UseShellExecute = false,
                    Arguments = args,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
            }
        }

        public static void EndProcesses()
        {
            Process[] minerProcessArr = Process.GetProcessesByName("etncraft-xmr");
            foreach (Process p in minerProcessArr)
            {
                p.Kill();
            }
        }

        private static int GetMinerProcessCount()
        {
            return Process.GetProcessesByName("etncraft-xmr") != null ? Process.GetProcessesByName("etncraft-xmr").Length : 0;
        }        
    }
}
