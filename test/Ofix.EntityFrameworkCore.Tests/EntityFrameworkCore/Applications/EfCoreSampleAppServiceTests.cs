using Ofix.Samples;
using Xunit;

namespace Ofix.EntityFrameworkCore.Applications;

[Collection(OfixTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<OfixEntityFrameworkCoreTestModule>
{

}
