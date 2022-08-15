using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Web.Mvc;

namespace Vnext.Intern.Web
{
    [DependsOn(
        typeof(InternDataModule), 
        typeof(InternApplicationModule), 
        typeof(InternWebApiModule),
        typeof(AbpWebMvcModule))]
    public class InternWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            Logger.Info("PreInitialize App");

            //Add/remove languages for your application
            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england", true));
            Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe", "famfamfam-flag-tr"));

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    "Intern",
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Vnext.Intern.Web.Localization.Intern"
                    )
                )
            );

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<InternNavigationProvider>();
        }

        public override void Initialize()
        {
            Logger.Info("Initialize App");

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
            Logger.Info("PostInitialize App");
        }
    }
}

