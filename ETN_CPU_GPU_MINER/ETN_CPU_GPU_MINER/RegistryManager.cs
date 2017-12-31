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
        private string IGNORE_TEMP_WARNINGS_KEY_NAME = "IgnoreTempWarnings";
        private string SUBKEY_TREE = "SOFTWARE\\ETNCRAFT";

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
        public void Initialize()
        {
            localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            if (!AppKeyExists())
            {
                etnCraftAppKey = CreateAppKey();
                etnCraftAppKey.SetValue(IGNORE_TEMP_WARNINGS_KEY_NAME, false);
                etnCraftAppKey.SetValue(AUTO_LOAD_KEY_NAME, false);
                etnCraftAppKey.SetValue(NEW_MINER_KEY_NAME, true);
                CloseRegistryKey(etnCraftAppKey);
            }
            etnCraftAppKey = localMachine.OpenSubKey(SUBKEY_TREE, true);
        }
        public bool AppKeyExists()
        {
            return localMachine.OpenSubKey(SUBKEY_TREE, true) != null;
        }
        public RegistryKey CreateAppKey()
        {
            return localMachine.CreateSubKey(SUBKEY_TREE);
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
        public void SetIgnoreTempWarnings(bool Ignore)
        {
            etnCraftAppKey.SetValue(IGNORE_TEMP_WARNINGS_KEY_NAME, Ignore);
        }
        public bool ReturnIgnoreTempWarnings()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(IGNORE_TEMP_WARNINGS_KEY_NAME));
        }
        public string DeleteRegistryKey()
        {
            string sRetVal = "";
            try
            {
                localMachine.DeleteSubKeyTree(SUBKEY_TREE);
                sRetVal = "ETNCRAFT Keys Deleted";
            }
            catch(Exception e)
            {
                sRetVal = e.Message;
            }

            return sRetVal;
        }
    }
}
