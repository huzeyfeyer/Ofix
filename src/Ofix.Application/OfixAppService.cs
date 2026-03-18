using Ofix.Localization;
using Volo.Abp.Application.Services;

namespace Ofix;

/* Inherit your application services from this class.
 */
public abstract class OfixAppService : ApplicationService
{
    protected OfixAppService()
    {
        LocalizationResource = typeof(OfixResource);
    }
}
