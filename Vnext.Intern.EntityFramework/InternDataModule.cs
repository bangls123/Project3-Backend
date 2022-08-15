using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;

using Vnext.Intern.EntityFramework.InternDb;

namespace Vnext.Intern
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(InternCoreModule))]
    public class InternDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "InternDb";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<InternDbDbContext>(null);
        }
    }
}

