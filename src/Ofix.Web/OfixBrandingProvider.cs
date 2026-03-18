using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using Ofix.Localization;

namespace Ofix.Web;

[Dependency(ReplaceServices = true)]
public class OfixBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<OfixResource> _localizer;

    public OfixBrandingProvider(IStringLocalizer<OfixResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
