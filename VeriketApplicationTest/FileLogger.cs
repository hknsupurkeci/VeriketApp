using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeriketApplicationTest
{
    public class FileLogger : ILogger
    {
        private readonly string logPath;

        public FileLogger(string logPath)
        {
            this.logPath = logPath;
        }

        /// <summary>
        /// Writes a log message to the log file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            using (FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.WriteLine(message);
            }
        }
    }
}
