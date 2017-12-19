using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using OpenHardwareMonitor.Hardware;
using System.Windows.Forms;
using System.Diagnostics;

namespace ETN_CPU_GPU_MINER
{
    class GetTemperature
    {
        public class System : ISettings
        {
            private IDictionary<string, string> settings = new Dictionary<string, string>();

            public System(IDictionary<string, string> settings)
            {
                this.settings = settings;
            }

            public bool Contains(string name)
            {
                return settings.ContainsKey(name);
            }

            public string GetValue(string name, string value)
            {
                string result;
                if (settings.TryGetValue(name, out result))
                    return result;
                else
                    return value;
            }

            public void Remove(string name)
            {
                settings.Remove(name);
            }

            public void SetValue(string name, string value)
            {
                settings[name] = value;
            }
        }
    }
}
