using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETN_CPU_GPU_MINER
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static bool m_bAutoRun = false;
        [STAThread]
        static void Main(string[] args)
        {
            foreach (string sArg in args)
            {
                if (sArg.ToLower().Equals("autorun"))
                    m_bAutoRun = true;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
