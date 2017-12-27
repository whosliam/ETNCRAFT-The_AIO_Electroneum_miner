using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace ETN_CPU_GPU_MINER
{
    public class Logger
    {
        private static string dateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private static string logFileNameDateTimeFormat = "yyyy-MM-dd";
        private static string logFileDir = "logs";
        private static string logFileExt = ".log";
        private string logFilePath;
   
        public Logger(string logFileName,bool append = true)
        {
            InitializeLogger(logFileName, append);
        }
       
        public void InitializeLogger(string logFileName, bool append)
        {
            if (!Directory.Exists(logFileDir))
                Directory.CreateDirectory(logFileDir);

            logFilePath = Path.Combine(logFileDir, logFileName + "_" + DateTime.Now.ToString(logFileNameDateTimeFormat) + logFileExt);

            if (!File.Exists(logFilePath) || !append)
                CreateLogFile();

            Debug("ETN_Craft Logging Started");
        }

        private void CreateLogFile()
        {
            FileStream LogFileStream = File.Create(logFilePath);
            LogFileStream.Close();
        }

        public string GetLogFilePath()
        {            
            return logFilePath;
        }


        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="text">Message</param>
        public void Info(string text)
        {
            Log(LEVEL.INFO, text);
        }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="text">Message</param>
        public void Debug(string text)
        {
            Log(LEVEL.DEBUG, text);
        }

        /// <summary>
        /// Log a waning message
        /// </summary>
        /// <param name="text">Message</param>
        public void Warn(string text)
        {
            Log(LEVEL.WARN, text);
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="text">Message</param>
        public void Error(string text)
        {
            Log(LEVEL.ERROR, text);
        }

        /// <summary>
        /// Log a fatal error message
        /// </summary>
        /// <param name="text">Message</param>
        public void Fatal(string text)
        {
            Log(LEVEL.FATAL, text);
        }

        /// <summary>
        /// Log message according to LogLevel
        /// </summary>
        /// <param name="LogLevel"></param>
        /// <param name="Message"></param>
        private void Log(LEVEL LogLevel, string Message)
        {
            string PreText;
            switch (LogLevel)
            {
                case LEVEL.INFO:
                    PreText = DateTime.Now.ToString(dateTimeFormat) + " [INFO] ";
                    break;
                case LEVEL.DEBUG:
                    PreText = DateTime.Now.ToString(dateTimeFormat) + " [DEBUG] ";
                    break;
                case LEVEL.WARN:
                    PreText = DateTime.Now.ToString(dateTimeFormat) + " [WARN]  ";
                    break;
                case LEVEL.ERROR:
                    PreText = DateTime.Now.ToString(dateTimeFormat) + " [ERROR] ";
                    break;
                case LEVEL.FATAL:
                    PreText = DateTime.Now.ToString(dateTimeFormat) + " [FATAL] ";
                    break;
                default: PreText = ""; break;
            }

            WriteLine(PreText + Message);
        }

        /// <summary>
        /// Write line of data to file
        /// </summary>
        /// <param name="Line"></param>
        private void WriteLine(string Line)
        {
            try
            {
                using (StreamWriter Writer = new StreamWriter(logFilePath, true, Encoding.UTF8))
                {
                    if (Line != "") Writer.WriteLine(Line);
                }
            }
            catch
            {
                //throw;
            }
        }

        [Flags]
        private enum LEVEL
        {
            INFO,
            DEBUG,
            WARN,
            ERROR,
            FATAL
        }
    }
}