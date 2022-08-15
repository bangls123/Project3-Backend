using System.ServiceProcess;

namespace Vnext.Itern.DepartmentServiceReport
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new InternDepartmentServiceReport()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
