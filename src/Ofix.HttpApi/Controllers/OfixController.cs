using Ofix.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Ofix.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class OfixController : AbpControllerBase
{
    protected OfixController()
    {
        LocalizationResource = typeof(OfixResource);
    }
}
