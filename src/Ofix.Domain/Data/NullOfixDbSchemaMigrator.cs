using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Ofix.Data;

/* This is used if database provider does't define
 * IOfixDbSchemaMigrator implementation.
 */
public class NullOfixDbSchemaMigrator : IOfixDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
