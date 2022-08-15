using Vnext.Intern.WinServiceBase;
using Vnext.Intern.WinServiceBase.Utils;
using System;

namespace InternDemoService
{
    public class MainProcess : AbstractService
    {
        public override ProcessResultDto Process(string[] cmdArgs)
        {
            try
            {
                return new ProcessResultDto
                {
                    Code = 0
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
