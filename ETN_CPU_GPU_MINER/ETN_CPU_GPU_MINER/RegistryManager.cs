using Microsoft.Win32;
using System;

namespace ETN_CPU_GPU_MINER
{
    class RegistryManager
    {
        #region Variables

        private RegistryKey localMachine;
        private RegistryKey etnCraftAppKey;

        private string VERSION_NUM = "1.7.6";

        private string AUTO_LOAD_KEY_NAME = "AutoLoad";
        private string NEW_MINER_KEY_NAME = "NewMiner";
        private string VERSION_KEY_NAME = "Version";

        private string WALLET_ID_KEY_NAME = "WalletId";
        private string PORT_KEY_NAME = "Port";
        private string POOL_KEY_NAME = "Pool";
        private string CUSTOM_POOL_KEY_NAME = "CustPool";
        private string MINER_COMPONENT = "MinerComp";
        private string TEMP_LIMIT = "TempLimit";
        private string IGNORE_TEMP_WARNINGS_KEY_NAME = "IgnoreTempWarnings";
        private string SUBKEY_TREE = "SOFTWARE\\ETNCRAFT";

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
                etnCraftAppKey.SetValue(VERSION_KEY_NAME, VERSION_NUM);

            else if (!etnCraftAppKey.GetValue(VERSION_KEY_NAME).Equals(VERSION_NUM))
                etnCraftAppKey.SetValue(VERSION_KEY_NAME, VERSION_NUM);

            if (etnCraftAppKey.GetValue(PORT_KEY_NAME) == null)
                etnCraftAppKey.SetValue(PORT_KEY_NAME, "5555");

            if (etnCraftAppKey.GetValue(CUSTOM_POOL_KEY_NAME) == null)
                etnCraftAppKey.SetValue(CUSTOM_POOL_KEY_NAME, "");

            if (etnCraftAppKey.GetValue(POOL_KEY_NAME) == null)
                etnCraftAppKey.SetValue(POOL_KEY_NAME, "1");

            if (etnCraftAppKey.GetValue(MINER_COMPONENT) == null)
                etnCraftAppKey.SetValue(MINER_COMPONENT, "CPU");

            if (etnCraftAppKey.GetValue(TEMP_LIMIT) == null)
                etnCraftAppKey.SetValue(TEMP_LIMIT, "90");


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

        public bool GetNewMiner()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(NEW_MINER_KEY_NAME));
        }

        public void SetNewMiner(bool isNewMiner)
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

        public void SetPort(string Port)
        {
            etnCraftAppKey.SetValue(PORT_KEY_NAME, Port);
        }

        public string GetPortNumber()
        {
            return Convert.ToString(etnCraftAppKey.GetValue(PORT_KEY_NAME));
        }

        public void SetCustomPool(string CustPool)
        {
            etnCraftAppKey.SetValue(CUSTOM_POOL_KEY_NAME, CustPool);
        }

        public string GetCustomPool()
        {
            return Convert.ToString(etnCraftAppKey.GetValue(CUSTOM_POOL_KEY_NAME));
        }

        public void SetPool(int Pool)
        {
            etnCraftAppKey.SetValue(POOL_KEY_NAME, Pool);
        }

        public int GetPool()
        {
            int iID = 0;
            int.TryParse(etnCraftAppKey.GetValue(POOL_KEY_NAME).ToString(), out iID);
            return iID;
        }

        public void SetComponent(string Component)
        {
            etnCraftAppKey.SetValue(MINER_COMPONENT, Component);
        }

        public string GetComponent()
        {
           return etnCraftAppKey.GetValue(MINER_COMPONENT).ToString();
             
        }

        public void SetTempLimit(string Limit)
        {
            etnCraftAppKey.SetValue(TEMP_LIMIT, Limit);
        }

        public string GetTempLimit()
        {
            return etnCraftAppKey.GetValue(TEMP_LIMIT).ToString();
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

        private void SetVersionValue()
        {

        }

        #endregion
    }
}
