using Microsoft.VisualBasic;

namespace ETN_CPU_GPU_MINER
{    
    class Messager
    {
        private string Text;
        public Logger Logger;

        public Messager()
        {            
        }
        
        public void InitializeMessager(Logger Log)
        {
            Logger = Log;
            Logger.Debug("Message Interface Enabled.");
            PushMessage("Original Fork by ParthK117");
            PushMessage("Current xmr-stak by fireice-uk");
            PushMessage("GUI,Configuration & miner compiled by ETNCRAFT team");
            PushMessage("!!Make sure to use your own wallet address!!");
        }
        
        public string PushMessage(string Message, bool WriteToLog = true)
        {
            if (WriteToLog)
                Logger.Debug(Message);

            Text += "> " + Message + Constants.vbNewLine;

            return Text;
        }

        public string ClearMessages()
        {
            Text = "";
            return Text;
        }

    }
}
