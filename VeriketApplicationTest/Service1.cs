using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VeriketApplicationTest
{
    public partial class Service1 : ServiceBase
    {
        private Timer logTimer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logTimer = new Timer();
            logTimer.Interval = 5000; // 5 saniye
            logTimer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
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

        private void WriteLog()
        {
            string logPath = @"C:\ProgramData\VeriketApp\VeriketAppTest.csv";
            string logEntry = $"{DateTime.Now}, {Environment.MachineName}, {Environment.UserName}";
            if (!File.Exists(logPath))
                File.Create(logPath);

            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine(logEntry);
            }
        }
    }
}
