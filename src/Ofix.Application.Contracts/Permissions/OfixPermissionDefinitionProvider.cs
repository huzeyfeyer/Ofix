using Ofix.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Ofix.Permissions;

public class OfixPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OfixPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(OfixPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(OfixPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(OfixPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(OfixPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(OfixPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OfixResource>(name);
    }
}
