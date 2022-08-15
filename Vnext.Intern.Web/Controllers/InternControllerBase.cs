using Abp.Web.Mvc.Controllers;

namespace Vnext.Intern.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class InternControllerBase : AbpController
    {
        protected InternControllerBase()
        {
            LocalizationSourceName = InternConsts.LocalizationSourceName;
        }
    }
}
