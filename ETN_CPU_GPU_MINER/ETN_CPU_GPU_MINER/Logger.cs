using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace ETNCRAFT
{
    public class Logger
    {
        private string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private string LogFileDateTimeFormat = "yyyy-MM-dd";
        private string LogFileDir = "logs";
        private string LogFilePre = "ETN_Craft_";
        private string LogFileExt = ".log";
        private string LogFilePath;

        /// <summary>
        /// Contruct a new instance of Logger.                
        /// </summary>
        /// <param name="append">True to append to existing log file, False to overwrite and create new log file</param>
        public Logger(bool append = true)
        {
            InitializeLogger(append);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="append"></param>
        public void InitializeLogger(bool append)
        {
            Directory.CreateDirectory(LogFileDir);

            LogFilePath = Path.Combine(LogFileDir, LogFilePre + DateTime.Now.ToString(LogFileDateTimeFormat) + LogFileExt);

            if (!File.Exists(LogFilePath) || !append)
            {
                CreateLogFile();
                Debug("ETN_Craft Logging Enabled");
            }
            else
                Debug("ETN_Craft Logging Enabled");
        }

        private void CreateLogFile()
        {
            FileStream LogFileStream = File.Create(LogFilePath);
            LogFileStream.Close();

        }

        public string GetLogFilePath()
        {            
            return LogFilePath;
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
                    PreText = DateTime.Now.ToString(DateTimeFormat) + " [INFO] ";
                    break;
                case LEVEL.DEBUG:
                    PreText = DateTime.Now.ToString(DateTimeFormat) + " [DEBUG] ";
                    break;
                case LEVEL.WARN:
                    PreText = DateTime.Now.ToString(DateTimeFormat) + " [WARN]  ";
                    break;
                case LEVEL.ERROR:
                    PreText = DateTime.Now.ToString(DateTimeFormat) + " [ERROR] ";
                    break;
                case LEVEL.FATAL:
                    PreText = DateTime.Now.ToString(DateTimeFormat) + " [FATAL] ";
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
                using (StreamWriter Writer = new StreamWriter(LogFilePath, true, Encoding.UTF8))
                {
                    if (Line != "") Writer.WriteLine(Line);
                }
            }
            catch
            {
                throw;
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