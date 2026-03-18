using Ofix.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Ofix.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OfixEntityFrameworkCoreModule),
    typeof(OfixApplicationContractsModule)
)]
public class OfixDbMigratorModule : AbpModule
{
}
