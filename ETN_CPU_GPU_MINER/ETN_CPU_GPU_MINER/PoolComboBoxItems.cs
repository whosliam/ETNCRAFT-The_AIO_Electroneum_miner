using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETN_CPU_GPU_MINER
{
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
}
