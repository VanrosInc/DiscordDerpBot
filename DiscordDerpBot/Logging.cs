using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDerpBot.Logging
{
    public class LoggerCore
    {
        private bool allowDebug;
        private LogFile LF = null;
        private ConsoleColor originalColor = ConsoleColor.White;
        public LoggerCore(bool debug, string logFilePath)
        {
            allowDebug = debug;
            if (!(logFilePath == "" || logFilePath == null))
            {
                LF = new LogFile(logFilePath);
            }
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(string sender, int status, string message)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            if (allowDebug == true)
            {
                if (status < 0)
                {
                    //Debug
                    Console.ForegroundColor = GetStatusColor(status);
                    string data = $@"[{time}][{sender}]{GetStatusTag(status)} {message}";
                    Log(data);
                    if (!(LF == null)) LF.Write(data);
                }
                else
                {
                    Console.ForegroundColor = GetStatusColor(status);
                    string data = $@"[{time}][{sender}]{GetStatusTag(status)} {message}";
                    Log(data);
                    if (!(LF == null)) LF.Write(data);
                }
            }
            else
            {
                if (status > 0)
                {
                    Console.ForegroundColor = GetStatusColor(status);
                    string data = $@"[{DateTime.Now.ToString("HH:mm:ss")}][{sender}]{GetStatusTag(status)} {message}";
                    Log(data);
                    if (!(LF == null)) LF.Write(data);
                }
            }
        }

        public void Log(string sender, int status, string message, ConsoleColor color)
        {
            originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetStatusColor(status);
            Log(sender, status, message);
            Console.ForegroundColor = originalColor;
        }

        public void Log(string sender, int status, string message, List<string> data)
        {
            string x = "";
            foreach (string s in data)
            {
                if (x == "")
                    x = $@"    - {s}";
                else
                    x += $@"\n    - {s}";
            }
            Log(sender, status, $@"{message}\n{x}");
        }

        public void Log(string sender, int status, string message, List<string> data, ConsoleColor color)
        {
            originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetStatusColor(status);
            Log(sender, status, message, data);
            Console.ForegroundColor = originalColor;
        }

        private string GetStatusTag(int status)
        {
            string output = "";
            switch (status)
            {
                case 0: output = "[INFO]"; break;
                case 1: output = "[WARNING]"; break;
                case 2: output = "[ERROR]"; break;
                case 3: output = "[SEVERE]"; break;
                case -1: output = "[DEBUG][INFO]"; break;
                case -2: output = "[DEBUG][WARNING]"; break;
                case -3: output = "[DEBUG][ERROR]"; break;
                case -4: output = "[DEBUG][SEVERE]"; break;
            }
            return output;
        }
        private ConsoleColor GetStatusColor(int status)
        {
            ConsoleColor color = ConsoleColor.White;
            switch (status)
            {
                case 0: color = ConsoleColor.White; break;
                case 1: color = ConsoleColor.Yellow; break;
                case 2: color = ConsoleColor.DarkYellow; break;
                case 3: color = ConsoleColor.Red; break;
                case -1: color = ConsoleColor.DarkCyan; break;
                case -2: color = ConsoleColor.DarkCyan; break;
                case -3: color = ConsoleColor.DarkCyan; break;
                case -4: color = ConsoleColor.DarkCyan; break;
            }
            return color;
        }
    }

    public class LogFile
    {
        private String logFilePath = null;
        private FileInfo file;
        private readonly string timeStamp;

        public LogFile(String path)
        {
            timeStamp = DateTime.Now.ToString("yyyy-MM-dd(HH-mm-ss)");
            logFilePath = $@"{path}\\{timeStamp}.log";
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                try
                {
                    dir.Create();
                    FileInfo f = new FileInfo(logFilePath);
                    if (!f.Exists)
                    {
                        try
                        {
                            f.Create().Close();
                            file = f;
                        }
                        catch { }
                    }
                }
                catch { }
            }
            else
            {
                FileInfo f = new FileInfo(logFilePath);
                if (!f.Exists)
                {
                    try
                    {
                        f.Create().Close();
                        file = f;
                    }
                    catch { }
                }
            }
        }

        public void Write(String text)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(logFilePath, true))
                {
                    sr.WriteLine(text);
                    sr.Flush();
                    sr.Close();
                }
            }
            catch { }
        }
    }

    public sealed class Logger
    {
        private LoggerCore core = null;
        private string sender = "";
        public Logger(string sender, bool allowDebug = false, string logFilePath = "")
        {
            this.sender = sender;
            core = new LoggerCore(allowDebug, logFilePath);
        }

        public void Log(string message)
        {
            core.Log(message);
        }

        public void Log(LogStatus status, string message)
        {
            core.Log(sender, GetStatus(status), message);
        }

        public void Log(LogStatus status, string message, ConsoleColor color)
        {
            core.Log(sender, GetStatus(status), message, color);
        }

        public void Log(LogStatus status, string message, List<string> data)
        {
            core.Log(sender, GetStatus(status), message, data);
        }

        public void Log(LogStatus status, string message, List<string> data, ConsoleColor color)
        {
            core.Log(sender, GetStatus(status), message, data, color);
        }

        private int GetStatus(LogStatus status)
        {
            int output = 0;
            switch (status)
            {
                case (LogStatus.INFO): output = 0; break;
                case (LogStatus.WARNING): output = 1; break;
                case (LogStatus.ERROR): output = 2; break;
                case (LogStatus.SEVERE): output = 3; break;
                case (LogStatus.DEBUG_INFO): output = -1; break;
                case (LogStatus.DEBUG_WARNING): output = -2; break;
                case (LogStatus.DEBUG_ERROR): output = -3; break;
                case (LogStatus.DEBUG_SEVERE): output = -4; break;
            }
            return output;
        }

        public void SetSender(string sender)
        {
            this.sender = sender;
        }
    }

    public enum LogStatus
    {
        INFO, WARNING, ERROR, SEVERE, DEBUG_INFO, DEBUG_WARNING, DEBUG_ERROR, DEBUG_SEVERE
    }
}
