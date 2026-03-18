using Ofix.Samples;
using Xunit;

namespace Ofix.EntityFrameworkCore.Domains;

[Collection(OfixTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<OfixEntityFrameworkCoreTestModule>
{

}
