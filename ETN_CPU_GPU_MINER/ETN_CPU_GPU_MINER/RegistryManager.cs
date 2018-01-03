using Microsoft.Win32;
using System;

namespace ETN_CPU_GPU_MINER
{
    class RegistryManager
    {
        #region Variables

        private RegistryKey localMachine;
        private RegistryKey etnCraftAppKey;

        private string versionNum                       = "1.7.1";

        private string AUTO_LOAD_KEY_NAME               = "AutoLoad";
        private string NEW_MINER_KEY_NAME               = "NewMiner";
        private string VERSION_KEY_NAME                 = "Version";
        private string WALLET_ID_KEY_NAME               = "WalletId";
        private string IGNORE_TEMP_WARNINGS_KEY_NAME    = "IgnoreTempWarnings";
        private string SUBKEY_TREE                      = "SOFTWARE\\ETNCRAFT";

        private bool autoLoadDefault = false;
        private bool newMinerDefault = true;
        private bool ignoreTempWarnDefault = false;
        private string walletIdDefault = "Enter Your Public Wallet Address";
        
        #endregion

        public RegistryManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            etnCraftAppKey = GetAppKey();

            if (etnCraftAppKey.GetValue(AUTO_LOAD_KEY_NAME) == null)
                etnCraftAppKey.SetValue(AUTO_LOAD_KEY_NAME, autoLoadDefault);

            if (etnCraftAppKey.GetValue(NEW_MINER_KEY_NAME) == null)
                etnCraftAppKey.SetValue(NEW_MINER_KEY_NAME, newMinerDefault);

            if (etnCraftAppKey.GetValue(IGNORE_TEMP_WARNINGS_KEY_NAME) == null)
                etnCraftAppKey.SetValue(IGNORE_TEMP_WARNINGS_KEY_NAME, ignoreTempWarnDefault);

            if (etnCraftAppKey.GetValue(WALLET_ID_KEY_NAME) == null)
                etnCraftAppKey.SetValue(WALLET_ID_KEY_NAME, walletIdDefault);

            if (etnCraftAppKey.GetValue(VERSION_KEY_NAME) == null)
                etnCraftAppKey.SetValue(VERSION_KEY_NAME, versionNum);

            etnCraftAppKey.Close();
            etnCraftAppKey = localMachine.OpenSubKey(SUBKEY_TREE, true);
        }

        #region Getters/Setters

        public bool GetAutoLoad()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(AUTO_LOAD_KEY_NAME));
        }

        public void SetAutoLoad(bool autoLoad)
        {
            etnCraftAppKey.SetValue(AUTO_LOAD_KEY_NAME, autoLoad);
        }

        public bool GetIgnoreTempWarnings()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(IGNORE_TEMP_WARNINGS_KEY_NAME));
        }

        public void SetIgnoreTempWarnings(bool Ignore)
        {
            etnCraftAppKey.SetValue(IGNORE_TEMP_WARNINGS_KEY_NAME, Ignore);
        }

        public bool GetIsNewMiner()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(NEW_MINER_KEY_NAME));
        }

        public void SetIsNewMiner(bool isNewMiner)
        {
            etnCraftAppKey.SetValue(NEW_MINER_KEY_NAME, isNewMiner);
        }

        public string GetVersion()
        {
            return Convert.ToString(etnCraftAppKey.GetValue(VERSION_KEY_NAME));
        }

        public void SetVersion(string version)
        {
            etnCraftAppKey.SetValue(VERSION_KEY_NAME, version);
        }

        public string GetWalletId()
        {
            return Convert.ToString(etnCraftAppKey.GetValue(WALLET_ID_KEY_NAME));
        }

        public void SetWalletId(string walletId)
        {
            etnCraftAppKey.SetValue(WALLET_ID_KEY_NAME, walletId);
        }

        #endregion

        #region Utilities

        private RegistryKey GetAppKey()
        {
            return localMachine.CreateSubKey(SUBKEY_TREE);
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

        public string DeleteRegistryKey()
        {
            string sRetVal = "";
            try
            {
                localMachine.DeleteSubKeyTree(SUBKEY_TREE);
                sRetVal = "ETNCRAFT Keys Deleted";
            }
            catch (Exception e)
            {
                sRetVal = e.Message;
            }

            return sRetVal;
        }

        #endregion
    }
}
