using System.Reflection;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;

using Vnext.Intern.Authentication;

namespace Vnext.Intern
{
    [DependsOn(typeof(AbpWebApiModule), typeof(InternApplicationModule))]
    public class InternWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(InternApplicationModule).Assembly, "app")
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new AuthenticationFilter());
        }
    }
}

