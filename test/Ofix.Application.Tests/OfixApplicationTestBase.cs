using Volo.Abp.Modularity;

namespace Ofix;

public abstract class OfixApplicationTestBase<TStartupModule> : OfixTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
