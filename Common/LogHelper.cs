using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    public class LogHelper
    {
        private static string exceptionLogFileName = DateTime.Now.ToString("yyyyMMdd") + ".exception.log";
        private static string exceptionLogFileName_BackUp = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".exception.log";
        private static string logFileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
        private static string logFileName_BackUp = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".log";
        private static int MAX_LOG_SIZE = 5;

        /// <summary>
        /// 写一条异常日志
        /// </summary>
        /// <param name="classLibrary">类库名</param>
        /// <param name="fileName">文件名</param>
        /// <param name="method">方法名</param>
        /// <param name="exception">异常对象</param>
        public static void WriteExceptionLog(string classLibrary, string fileName, string method, Exception exception)
        {
            try
            {
                String logDirectory = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Logs";
                String logFile = logDirectory + "\\" + exceptionLogFileName;

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                if (!File.Exists(logFile))
                {
                    String newLine = Environment.NewLine;
                    System.Text.StringBuilder logContent = new System.Text.StringBuilder();

                    logContent.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + newLine);
                    logContent.Append("<exceptions>" + newLine);
                    logContent.Append("</exceptions>");

                    File.WriteAllText(logFile, logContent.ToString(), System.Text.Encoding.UTF8);
                }

                XmlDocument logDoc = new XmlDocument();
                logDoc.Load(logFile);

                XmlElement itemElement = logDoc.CreateElement("exception");
                logDoc.DocumentElement.AppendChild(itemElement);

                XmlElement classLibraryElement = logDoc.CreateElement("classLibrary");
                classLibraryElement.InnerText = classLibrary;
                itemElement.AppendChild(classLibraryElement);

                XmlElement fileNameElement = logDoc.CreateElement("fileName");
                fileNameElement.InnerText = fileName;
                itemElement.AppendChild(fileNameElement);

                XmlElement methodElement = logDoc.CreateElement("method");
                methodElement.InnerText = method;
                itemElement.AppendChild(methodElement);

                XmlElement dateElement = logDoc.CreateElement("time");
                dateElement.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                itemElement.AppendChild(dateElement);

                XmlElement messageElement = logDoc.CreateElement("message");
                messageElement.InnerText = exception.Message;
                itemElement.AppendChild(messageElement);

                XmlElement stackTraceElement = logDoc.CreateElement("stackTrace");
                stackTraceElement.InnerText = exception.StackTrace;
                itemElement.AppendChild(stackTraceElement);

                logDoc.Save(logFile);

                //日志文件超过最大限制大小，就自动备份到新文件，然后重新生成日志文件。
                if (logDoc.OuterXml.Length > MAX_LOG_SIZE * 1024 * 1024)
                {
                    File.Move(logFile, String.Format(@"{0}\{1}", logDirectory, exceptionLogFileName_BackUp));
                }
            }
            catch(Exception ex)
            {
            }
        }
        
        /// <summary>
        /// 写一条日志记录
        /// </summary>
        /// <param name="classLibrary">类库名</param>
        /// <param name="fileName">文件名</param>
        /// <param name="method">方法名</param>
        /// <param name="message">日志信息</param>
        public static void WriteLog(string classLibrary, string fileName, string method, string message)
        {
            try
            {
                String logDirectory = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Logs";
                String logFile = logDirectory + "\\" + logFileName;

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                if (!File.Exists(logFile))
                {
                    String newLine = Environment.NewLine;
                    System.Text.StringBuilder logContent = new System.Text.StringBuilder();

                    logContent.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + newLine);
                    logContent.Append("<logs>" + newLine);
                    logContent.Append("</logs>");

                    File.WriteAllText(logFile, logContent.ToString(), System.Text.Encoding.UTF8);
                }

                XmlDocument logDoc = new XmlDocument();
                logDoc.Load(logFile);

                XmlElement itemElement = logDoc.CreateElement("log");
                logDoc.DocumentElement.AppendChild(itemElement);

                XmlElement classLibraryElement = logDoc.CreateElement("classLibrary");
                classLibraryElement.InnerText = classLibrary;
                itemElement.AppendChild(classLibraryElement);

                XmlElement fileNameElement = logDoc.CreateElement("fileName");
                fileNameElement.InnerText = fileName;
                itemElement.AppendChild(fileNameElement);

                XmlElement methodElement = logDoc.CreateElement("method");
                methodElement.InnerText = method;
                itemElement.AppendChild(methodElement);

                XmlElement dateElement = logDoc.CreateElement("time");
                dateElement.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                itemElement.AppendChild(dateElement);

                XmlElement messageElement = logDoc.CreateElement("message");
                messageElement.InnerText = message;
                itemElement.AppendChild(messageElement);

                logDoc.Save(logFile);

                //日志文件超过最大限制大小，就自动备份到新文件，然后重新生成日志文件。
                if (logDoc.OuterXml.Length > MAX_LOG_SIZE * 1024 * 1024)
                {
                    File.Move(logFile, String.Format(@"{0}\{1}", logDirectory, logFileName_BackUp));
                }
            }
            catch
            {
            }
        }
    }
}
