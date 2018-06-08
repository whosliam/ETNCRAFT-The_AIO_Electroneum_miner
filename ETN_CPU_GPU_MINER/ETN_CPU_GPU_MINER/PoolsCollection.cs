using System;
using System.IO;
using System.Windows.Forms;

namespace ETN_CPU_GPU_MINER
{
    public class PoolsCollection : System.Collections.Generic.List<Pools>
    {
        public int Load()
        {
            this.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(Application.StartupPath + "\\app_assets\\etnpools.txt"))
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

}
