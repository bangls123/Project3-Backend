using System.Reflection;
using Abp.Modules;

namespace Vnext.Intern
{
    public class InternCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

