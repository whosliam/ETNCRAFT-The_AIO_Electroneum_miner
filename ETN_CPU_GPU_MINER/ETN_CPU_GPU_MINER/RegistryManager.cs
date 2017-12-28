using Microsoft.Win32;
using System;

namespace ETN_CPU_GPU_MINER
{
    class RegistryManager
    {
        private RegistryKey localMachine;
        private RegistryKey etnCraftAppKey;

        private string AUTO_LOAD_KEY_NAME = "AutoLoad";
        private string NEW_MINER_KEY_NAME = "NewMiner";

        public RegistryManager()
        {
            Initialize();
        }

        private void CloseRegistryKey(RegistryKey key)
        {
            key.Close();
        }

        public void CloseRegistryKeys()
        {
            etnCraftAppKey.Close();
            localMachine.Close();
        }

        private void Initialize()
        {
            localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            if (!AppKeyExists())
            {
                etnCraftAppKey = CreateAppKey();
                etnCraftAppKey.SetValue(AUTO_LOAD_KEY_NAME, false);
                etnCraftAppKey.SetValue(NEW_MINER_KEY_NAME, true);
                CloseRegistryKey(etnCraftAppKey);                
            }
            etnCraftAppKey = localMachine.OpenSubKey("SOFTWARE\\ETNCRAFT", true);
        }

        public bool AppKeyExists()
        {
            return localMachine.OpenSubKey("SOFTWARE\\ETNCRAFT", true) != null;
        }

        public RegistryKey CreateAppKey()
        {
            return localMachine.CreateSubKey("SOFTWARE\\ETNCRAFT");
        }

        public bool GetAutoLoad()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(AUTO_LOAD_KEY_NAME));
        }

        public void SetAutoLoad(bool autoLoad)
        {
            etnCraftAppKey.SetValue(AUTO_LOAD_KEY_NAME, autoLoad);
        }

        public bool GetIsNewMiner()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(NEW_MINER_KEY_NAME));
        }

        public void SetIsNewMiner(bool isNewMiner)
        {
            etnCraftAppKey.SetValue(NEW_MINER_KEY_NAME, isNewMiner);
        }
    }
}
