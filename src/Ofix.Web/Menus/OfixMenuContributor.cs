using System.Threading.Tasks;
using Ofix.Localization;
using Ofix.Permissions;
using Ofix.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace Ofix.Web.Menus;

public class OfixMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<OfixResource>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                OfixMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "Ofix.Marketplace",
                l["Menu:Marketplace"],
                "~/Marketplace",
                icon: "fa fa-car",
                order: 2
            )
        );


        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);
        
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 8);
    
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "BooksStore",
                l["Menu:Ofix"],
                icon: "fa fa-book"
            ).AddItem(
            new ApplicationMenuItem(
                "BooksStore.Books",
                l["Menu:Books"],
                url: "/Books"
                ).RequirePermissions(OfixPermissions.Books.Default) 
            ).AddItem(
                new ApplicationMenuItem(
                    "Ofix.Brands",
                    l["Menu:Brands"],
                    url: "/Brands"
                ).RequirePermissions(OfixPermissions.Brands.Default)
                ).AddItem(
            new ApplicationMenuItem(
                "Ofix.Models",
                l["Menu:Models"],
                url: "/Models"
                ).RequirePermissions(OfixPermissions.Models.Default)
            ).AddItem(
            new ApplicationMenuItem(
                "Ofix.SubModels",
                l["Menu:SubModels"],
                url: "/SubModels"
                ).RequirePermissions(OfixPermissions.SubModels.Default)
            ).AddItem(
            new ApplicationMenuItem(
                "Ofix.CarListings",
                l["Menu:CarListings"],
                url: "/CarListings"
                ).RequirePermissions(OfixPermissions.CarListings.Default)
            )
        );


        
        return Task.CompletedTask;
    }
}
