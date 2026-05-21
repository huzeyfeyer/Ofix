using Microsoft.Extensions.Localization;
using Ofix.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Ofix.Web;

[Dependency(ReplaceServices = true)]
public class OfixBrandingProvider : DefaultBrandingProvider
{
    private readonly IStringLocalizer<OfixResource> _localizer;

    public OfixBrandingProvider(IStringLocalizer<OfixResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];

    public override string LogoUrl => "/images/branding/ofix-logo.png";

    public override string LogoReverseUrl => "/images/branding/ofix-logo.png";
}
