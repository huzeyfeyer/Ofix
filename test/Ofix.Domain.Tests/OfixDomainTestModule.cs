using Volo.Abp.Modularity;

namespace Ofix;

[DependsOn(
    typeof(OfixDomainModule),
    typeof(OfixTestBaseModule)
)]
public class OfixDomainTestModule : AbpModule
{

}
