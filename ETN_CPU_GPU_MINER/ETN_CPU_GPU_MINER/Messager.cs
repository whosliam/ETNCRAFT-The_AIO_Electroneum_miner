using Microsoft.VisualBasic;

namespace ETNCRAFT
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
            PushMessage("Current cpuminer multi and ccminer by tpruvot.");
            PushMessage("Current xmr-stak-amd and nvidia and cpu by fireice-uk");
            PushMessage("xmr-stak miners do take a 1% fee, keep this in mind if you choose to use them.");
            PushMessage("Make sure to use your own wallet address.");
        }
        
        public string PushMessage(string Message, bool WriteToLog = true)
        {
            if (WriteToLog)
                Logger.Debug(Message);

            Text += Constants.vbNewLine + "> " + Message;

            return Text;
        }

        public string ClearMessages()
        {
            Text = Constants.vbNewLine;
            return Text;
        }

    }
}
