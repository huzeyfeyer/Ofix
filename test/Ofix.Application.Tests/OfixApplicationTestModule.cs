using Volo.Abp.Modularity;

namespace Ofix;

[DependsOn(
    typeof(OfixApplicationModule),
    typeof(OfixDomainTestModule)
)]
public class OfixApplicationTestModule : AbpModule
{

}
