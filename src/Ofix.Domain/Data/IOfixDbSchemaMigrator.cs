using System.Threading.Tasks;

namespace Ofix.Data;

public interface IOfixDbSchemaMigrator
{
    Task MigrateAsync();
}
