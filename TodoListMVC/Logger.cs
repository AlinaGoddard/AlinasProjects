using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace TodoListMVC
{
    public class Logger
    {
        private string ErrorLogPath { get; set;}
        private string LogMessagesPath { get; set;}

        public Logger(string errorLogPath, string logMessagesPath)
        { 
            ErrorLogPath = errorLogPath;
            LogMessagesPath = logMessagesPath;
        }

        private void WriteToFile(string line, string path)
        {
            using (StreamWriter sw = !File.Exists(path) ? File.CreateText(path) : File.AppendText(path))
            {
                sw.WriteLine(string.Format("{0} {1} {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), line));
            }
        }

        public void WriteError(string type, string methodName, Exception ex, string message, params string[] arguments)
        {
            string formattedMessage = string.Format("{0}:{1} ", type, methodName);
            formattedMessage += string.Format(message, arguments);
            formattedMessage += string.Format(" Exception Message={0}, StackTrace={1}", ex.Message, ex.StackTrace);
            WriteToFile(formattedMessage, ErrorLogPath);
        }

        public void WriteLog(string type, string methodName, string message, params string[] arguments)
        {
            string formattedMessage = string.Format("{0}:{1} ", type, methodName);
            formattedMessage += string.Format(message, arguments);
            WriteToFile(formattedMessage, LogMessagesPath);
        }
    }
}