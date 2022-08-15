using Abp.Web.Mvc.Views;

namespace Vnext.Intern.Web.Views
{
    public abstract class InternWebViewPageBase : InternWebViewPageBase<dynamic>
    {

    }

    public abstract class InternWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected InternWebViewPageBase()
        {
            LocalizationSourceName = InternConsts.LocalizationSourceName;
        }
    }
}
