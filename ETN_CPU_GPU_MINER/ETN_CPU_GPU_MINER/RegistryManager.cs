using Microsoft.Win32;
using System;

namespace ETN_CPU_GPU_MINER
{
    class RegistryManager
    {
        #region Variables

        private RegistryKey localMachine;
        private RegistryKey etnCraftAppKey;

        private const string VERSION_NUM = "1.7.9";

        private const string AUTO_LOAD_KEY_NAME = "AutoLoad";
        private const string NEW_MINER_KEY_NAME = "NewMiner";
        private const string VERSION_KEY_NAME = "Version";

        private const string WALLET_ID_KEY_NAME = "WalletId";
        private const string PORT_KEY_NAME = "Port";
        private const string POOL_KEY_NAME = "Pool";
        private const string CUSTOM_POOL_KEY_NAME = "CustPool";
        private const string ENFORCE_MAX_UPTIME = "EnforceMaxUpTime";
        private const string MAX_UPTIME_MIN = "MaxUpTime";
        private const string MINER_COMPONENT = "MinerComp";
        private const string TEMP_LIMIT = "TempLimit";
        private const string IGNORE_TEMP_WARNINGS_KEY_NAME = "IgnoreTempWarnings";
        private const string SUBKEY_TREE = "SOFTWARE\\ETNCRAFT";
        private const string SCHEDULE_DATA = "SchedDat";
        private const string SCHEDULE_ENABLED = "SchedEnabled";

        private const bool autoLoadDefault = false;
        private const bool newMinerDefault = true;
        private const bool ignoreTempWarnDefault = false;
        private const bool enforceMaxUpTimeDefault = false;
        private const double maxUpTimeDefault = 0.0;
        private const string walletIdDefault = "Enter Your Public Wallet Address";
        private const string portDefault = "5555";
        private const string customPoolDefault = "";
        private const int poolDefault = 1;
        private const string minerComponentDefault = "CPU";
        private const string tempLimitDefault = "90";

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
                SetAutoLoad(autoLoadDefault);

            if (etnCraftAppKey.GetValue(NEW_MINER_KEY_NAME) == null)
                SetNewMiner(newMinerDefault);

            if (etnCraftAppKey.GetValue(IGNORE_TEMP_WARNINGS_KEY_NAME) == null)
                SetIgnoreTempWarnings(ignoreTempWarnDefault);

            if (etnCraftAppKey.GetValue(WALLET_ID_KEY_NAME) == null)
                SetWalletId(walletIdDefault);

            if (etnCraftAppKey.GetValue(VERSION_KEY_NAME) == null || !etnCraftAppKey.GetValue(VERSION_KEY_NAME).Equals(VERSION_NUM))
               SetVersion(VERSION_NUM);            

            if (etnCraftAppKey.GetValue(PORT_KEY_NAME) == null)
                SetPort(portDefault);

            if (etnCraftAppKey.GetValue(CUSTOM_POOL_KEY_NAME) == null)
                SetCustomPool(customPoolDefault);

            if (etnCraftAppKey.GetValue(POOL_KEY_NAME) == null)
                SetPool(poolDefault);

            if (etnCraftAppKey.GetValue(MINER_COMPONENT) == null)
                SetComponent(minerComponentDefault);

            if (etnCraftAppKey.GetValue(TEMP_LIMIT) == null)
                SetTempLimit(tempLimitDefault);

            if (etnCraftAppKey.GetValue(ENFORCE_MAX_UPTIME) == null)
                SetEnforceMaxUpTime(enforceMaxUpTimeDefault);

            if (etnCraftAppKey.GetValue(MAX_UPTIME_MIN) == null)
                SetMaxUpTimeMin(maxUpTimeDefault);


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

        private void SetVersion(string version)
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

        public void SetEnforceMaxUpTime(bool enforceMaxUpTime)
        {
            etnCraftAppKey.SetValue(ENFORCE_MAX_UPTIME, enforceMaxUpTime);
        }

        public bool GetEnforceMaxUpTime()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(ENFORCE_MAX_UPTIME));
        }

        public void SetMaxUpTimeMin(double maxUpTime)
        {
            etnCraftAppKey.SetValue(MAX_UPTIME_MIN, maxUpTime);
        }

        public double GetMaxUpTimeMin()
        {
            return Convert.ToDouble(etnCraftAppKey.GetValue(MAX_UPTIME_MIN));
        }

        public void SetScheduleData(string sData)
        {
            etnCraftAppKey.SetValue(SCHEDULE_DATA, sData);
        }

        public string GetScheduleData()
        {
            return Convert.ToString(etnCraftAppKey.GetValue(SCHEDULE_DATA));
        }
        public bool GetScheduleEnabled()
        {
            return Convert.ToBoolean(etnCraftAppKey.GetValue(SCHEDULE_ENABLED));
        }

        public void SetScheduleEnabled(bool bScheduleEnabled)
        {
            etnCraftAppKey.SetValue(SCHEDULE_ENABLED, bScheduleEnabled);
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
