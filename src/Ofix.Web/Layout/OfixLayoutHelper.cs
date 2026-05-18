using System.Linq;
using System.Threading.Tasks;
using Ofix.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Users;

namespace Ofix.Web.Layout;

public static class OfixLayoutHelper
{
    public const string AdminBodyClass = "ofix-layout-admin";
    public const string GebruikerBodyClass = "ofix-layout-gebruiker";

    private static readonly string[] AdminPermissions =
    [
        OfixPermissions.CarListings.Default,
        OfixPermissions.Brands.Default,
        OfixPermissions.Models.Default,
        OfixPermissions.SubModels.Default
    ];

    // Bepaalt of de gebruiker het admin-layout krijgt (donkere navbar)
    public static async Task<string> GetBodyClassAsync(
        ICurrentUser currentUser,
        IPermissionChecker permissionChecker)
    {
        if (!currentUser.IsAuthenticated)
        {
            return GebruikerBodyClass;
        }

        if (currentUser.IsInRole("admin"))
        {
            return AdminBodyClass;
        }

        var grantResult = await permissionChecker.IsGrantedAsync(AdminPermissions);
        if (grantResult.Result.Values.Any(x => x == PermissionGrantResult.Granted))
        {
            return AdminBodyClass;
        }

        return GebruikerBodyClass;
    }
}
