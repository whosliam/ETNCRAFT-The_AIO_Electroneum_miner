using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
namespace ETN_CPU_GPU_MINER
{
    class GetTemperature
    {
        public string GetCPUTemp()
        {
            string sCPUTemp = "";


            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
            foreach (ManagementObject obj in searcher.Get())
            {
                Double temp = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                sCPUTemp = ((temp - 2732) / 10.0).ToString();
            }
            return sCPUTemp;
        }
    }
}
