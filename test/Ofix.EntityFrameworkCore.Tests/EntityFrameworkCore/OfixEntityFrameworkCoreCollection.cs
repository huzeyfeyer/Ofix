using Xunit;

namespace Ofix.EntityFrameworkCore;

[CollectionDefinition(OfixTestConsts.CollectionDefinitionName)]
public class OfixEntityFrameworkCoreCollection : ICollectionFixture<OfixEntityFrameworkCoreFixture>
{

}
