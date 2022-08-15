using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

namespace Vnext.Intern
{
    [DependsOn(typeof(InternCoreModule), typeof(AbpAutoMapperModule))]
    public class InternApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

