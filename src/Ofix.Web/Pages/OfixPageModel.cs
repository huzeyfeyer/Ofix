using Ofix.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Ofix.Web.Pages;

public abstract class OfixPageModel : AbpPageModel
{
    protected OfixPageModel()
    {
        LocalizationResourceType = typeof(OfixResource);
    }
}
