using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Ofix.Web.Components.Layout;

public class OfixBrandingHeadViewComponent : AbpViewComponent
{
    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(
            View("~/Components/Layout/OfixBrandingHead/Default.cshtml"));
    }
}
