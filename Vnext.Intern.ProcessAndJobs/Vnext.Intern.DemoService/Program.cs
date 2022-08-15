using System.ServiceProcess;

namespace InternDepartmentServiceReport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new InternDemoService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
