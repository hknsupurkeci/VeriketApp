using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeriketApplicationTest
{
    /// <summary>
    /// Defines a simple logging interface with a method to log messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Log(string message);
    }
}
