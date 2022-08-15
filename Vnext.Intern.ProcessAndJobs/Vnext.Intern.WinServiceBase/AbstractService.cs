using Vnext.Intern.WinServiceBase.Utils;

namespace Vnext.Intern.WinServiceBase
{
    public abstract class AbstractService
    {
        public abstract ProcessResultDto Process(string[] cmdArgs);
    }
}
