using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Ofix.Web.Components.Layout;

public class OfixSiteFooterViewComponent : AbpViewComponent
{
    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(
            View("~/Components/Layout/OfixSiteFooter/Default.cshtml"));
    }
}
