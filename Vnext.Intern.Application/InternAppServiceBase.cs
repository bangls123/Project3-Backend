using Abp.Application.Services;

namespace Vnext.Intern
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class InternAppServiceBase : ApplicationService
    {
        protected InternAppServiceBase()
        {
            LocalizationSourceName = InternConsts.LocalizationSourceName;
        }
    }
}
