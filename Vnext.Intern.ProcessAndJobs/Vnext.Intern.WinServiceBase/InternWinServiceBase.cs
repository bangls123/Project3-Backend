using Vnext.Intern.WinServiceBase.Utils;
using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace Vnext.Intern.WinServiceBase
{
    public partial class InternWinServiceBase : ServiceBase
    {
        private string serviceName;
        private string sleepMinus;
        private string serviceLogFolder;
        private string processClass;

        private Timer workTimer;

        private DateTime currentTime;

        public InternWinServiceBase()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            serviceName = ConfigurationManager.AppSettings["ServiceName"];
            serviceLogFolder = ConfigurationManager.AppSettings["ServiceLogFolder"];
            sleepMinus = ConfigurationManager.AppSettings["SleepMinus"];
            processClass = ConfigurationManager.AppSettings["ProcessClass"];

            workTimer = new Timer(new TimerCallback(Processing), null, 0, int.Parse(sleepMinus) * 60000);

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            workTimer.Dispose();
            base.OnStop();
        }

        protected void Processing(object state)
        {
            currentTime = DateTime.Now;
            try
            {
                bool allowRun = bool.Parse(ConfigurationManager.AppSettings["AllowRun"]);
                if (allowRun)
                {
                    var proc = (AbstractService)ObjectCreator.CreateInstance(processClass);
                    ProcessResultDto processRs = proc.Process(Environment.GetCommandLineArgs());

                    if (processRs.Code == 0)
                    {
                        WriteLogFile("Service completed", "COMPLETED");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile(ex.Message, "EX");
            }
        }

        protected void WriteLogFile(string message, string type)
        {
            if (!Directory.Exists(serviceLogFolder))
            {
                Directory.CreateDirectory(serviceLogFolder);
            }

            string path = serviceLogFolder + "\\" + serviceName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath;
            string[] messageLogs;
            switch (type)
            {
                case "EX":
                    filepath = path + "\\ExceptionLog_" + currentTime.ToString("yyyyMMddHHmmss") + ".txt";
                    messageLogs = new string[] {
                        "================= " + currentTime.ToString("MM/dd/yyyy HH:mm:ss") + " =================",
                        message
                    };
                    break;
                default:
                    filepath = path + "\\CompletedLog_" + currentTime.ToString("yyyyMMddHHmmss") + ".txt";
                    messageLogs = new string[] {
                        "================= " + currentTime.ToString("MM/dd/yyyy HH:mm:ss") + " =================",
                        message
                    };
                    break;
            }

            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    foreach (var item in messageLogs)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    foreach (var item in messageLogs)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}
