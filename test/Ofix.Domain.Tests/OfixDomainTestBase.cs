using Volo.Abp.Modularity;

namespace Ofix;

/* Inherit from this class for your domain layer tests. */
public abstract class OfixDomainTestBase<TStartupModule> : OfixTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
