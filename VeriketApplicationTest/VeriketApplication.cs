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
    public partial class VeriketApplication : ServiceBase
    {
        private Timer logTimer;
        public VeriketApplication()
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

        public void WriteLog()
        {
            string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VeriketApp\\VeriketAppTest.csv");
            string directoryPath = Path.GetDirectoryName(logPath);
            string logEntry = $"{DateTime.Now}, {Environment.MachineName}, {Environment.UserName}";

            // Klasörün var olup olmadığını kontrol et ve yoksa oluştur
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            // Dosyanın var olup olmadığını kontrol et ve yoksa oluştur
            if (!File.Exists(logPath))
            {
                using (var fs = File.Create(logPath))
                {
                    // FileStream kullanıldıktan sonra kapatılır, böylece dosya kilidi kalkar
                }
            }

            // Dosyaya yazarken, dosyanın başka bir uygulama tarafından da kullanılabilmesine izin ver
            using (FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.WriteLine(logEntry);
            }
        }
    }
}
