using System;
using System.Collections.Generic;
using System.IO;
using DiscordBotFramework.Enums;

namespace DiscordBotFramework.Utilities
{
    public class LogUtility
    {
        public string Date { get; set; }
        public string User { get; set; }
        public string Server { get; set; }
        public string LogType { get; set; }
        public string Logged { get; set; }

        public static void WriteLogFile(string logPath, List<LogUtility> logName)
        {
            using (StreamWriter file = new StreamWriter(logPath, true))
            {
                foreach (LogUtility log in logName)
                {
                    file.WriteLine($"[{log.Date}] [{log.LogType}] [{log.User} in {log.Server}] -- {log.Logged}");
                }
                file.Close();
            }
        }

        public static void AddToLog(string user, string server, string msg, List<LogUtility> log, LogTypeEnum type)
        => log.Add(new LogUtility()
        {
            Date = $"{DateTime.Now.Date.ToString($"dd.MM.yy")}",
            LogType = $"{type}",
            User = $"{user}",
            Server = $"{server}",
            Logged = $"{msg}"
        });
    }
}
