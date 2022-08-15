using InternDepartmentServiceReport;

namespace InternDemoDebugService
{
    class Program
    {
        static void Main(string[] args)
        {
            MainProcess mainProcess = new MainProcess();
            mainProcess.Process(args);
        }
    }
}
