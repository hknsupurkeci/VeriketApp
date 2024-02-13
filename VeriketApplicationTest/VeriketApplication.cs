using System;
using System.IO;
using System.Security.Principal;
using System.ServiceProcess;
using System.Timers;
using System.Configuration; // For ConfigurationManager

namespace VeriketApplicationTest
{
    public partial class VeriketApplication : ServiceBase
    {
        private Timer logTimer;
        private readonly ILogger logger;
        private readonly int timerIntervalMilliseconds = Convert.ToInt32(ConfigurationManager.AppSettings["TimerIntervalMilliseconds"]);
        private readonly string logDirectoryName = ConfigurationManager.AppSettings["LogDirectoryName"];
        private readonly string logFileName = ConfigurationManager.AppSettings["LogFileName"];
        private readonly string catchLogFileName = ConfigurationManager.AppSettings["CatchLogFileName"];
        private readonly string logPath;
        private readonly string catchLogPath;

        public VeriketApplication()
        {
            InitializeComponent();
            // Setting up the full paths for the log file and the catch log file
            logPath = GetLogPath(logFileName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), logDirectoryName));
            catchLogPath = GetLogPath(catchLogFileName, AppDomain.CurrentDomain.BaseDirectory);
            logger = new FileLogger(logPath);
        }

        protected override void OnStart(string[] args)
        {
            logTimer = new Timer(timerIntervalMilliseconds);
            logTimer.Elapsed += OnTimerElapsed;
            logTimer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            WriteLog();
        }

        protected override void OnStop()
        {
            logTimer.Stop();
        }

        public void WriteLog()
        {
            try
            {
                logger.Log($"{DateTime.Now}, {Environment.MachineName}, {WindowsIdentity.GetCurrent().Name}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"{DateTime.Now} - Error writing log: {ex.Message}\n";
                File.AppendAllText(catchLogPath, errorMessage);
            }
        }

        /// <summary>
        /// Gets the full path for a specified file name within a specified directory path.
        /// If the directory does not exist, it is created. If the file does not exist, it is created.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="directoryPath">The directory path where the file is located.</param>
        /// <returns>The full path of the file.</returns>
        private string GetLogPath(string fileName, string directoryPath)
        {
            // Create directory if it does not exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fullPath = Path.Combine(directoryPath, fileName);

            // Create file if it does not exist
            if (!File.Exists(fullPath))
            {
                using (var fs = File.Create(fullPath)) { }
            }

            return fullPath;
        }
    }
}
