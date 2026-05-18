using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ofix.Web.Layout;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Users;

namespace Ofix.Web.Components.Layout;

public class OfixLayoutStijlViewComponent : AbpViewComponent
{
    private readonly ICurrentUser _currentUser;
    private readonly IPermissionChecker _permissionChecker;

    public OfixLayoutStijlViewComponent(
        ICurrentUser currentUser,
        IPermissionChecker permissionChecker)
    {
        _currentUser = currentUser;
        _permissionChecker = permissionChecker;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var bodyClass = await OfixLayoutHelper.GetBodyClassAsync(_currentUser, _permissionChecker);
        return View("~/Components/Layout/OfixLayoutStijl/Default.cshtml", bodyClass);
    }
}
