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

        var brandsPermission = myGroup.AddPermission(OfixPermissions.Brands.Default, L("Permission:Brands"));
        brandsPermission.AddChild(OfixPermissions.Brands.Create, L("Permission:Brands.Create"));
        brandsPermission.AddChild(OfixPermissions.Brands.Edit, L("Permission:Brands.Edit"));
        brandsPermission.AddChild(OfixPermissions.Brands.Delete, L("Permission:Brands.Delete"));

        var modelsPermission = myGroup.AddPermission(OfixPermissions.Models.Default, L("Permission:Models"));
        modelsPermission.AddChild(OfixPermissions.Models.Create, L("Permission:Models.Create"));
        modelsPermission.AddChild(OfixPermissions.Models.Edit, L("Permission:Models.Edit"));
        modelsPermission.AddChild(OfixPermissions.Models.Delete, L("Permission:Models.Delete"));

        var subModelsPermission = myGroup.AddPermission(OfixPermissions.SubModels.Default, L("Permission:SubModels"));
        subModelsPermission.AddChild(OfixPermissions.SubModels.Create, L("Permission:SubModels.Create"));
        subModelsPermission.AddChild(OfixPermissions.SubModels.Edit, L("Permission:SubModels.Edit"));
        subModelsPermission.AddChild(OfixPermissions.SubModels.Delete, L("Permission:SubModels.Delete"));

        var carListingsPermission = myGroup.AddPermission(OfixPermissions.CarListings.Default, L("Permission:CarListings"));
        carListingsPermission.AddChild(OfixPermissions.CarListings.Create, L("Permission:CarListings.Create"));
        carListingsPermission.AddChild(OfixPermissions.CarListings.Edit, L("Permission:CarListings.Edit"));
        carListingsPermission.AddChild(OfixPermissions.CarListings.Delete, L("Permission:CarListings.Delete"));

        var carListingImagesPermission = myGroup.AddPermission(OfixPermissions.CarListingImages.Default, L("Permission:CarListingImages"));
        carListingImagesPermission.AddChild(OfixPermissions.CarListingImages.Create, L("Permission:CarListingImages.Create"));
        carListingImagesPermission.AddChild(OfixPermissions.CarListingImages.Edit, L("Permission:CarListingImages.Edit"));
        carListingImagesPermission.AddChild(OfixPermissions.CarListingImages.Delete, L("Permission:CarListingImages.Delete"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(OfixPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OfixResource>(name);
    }
}
