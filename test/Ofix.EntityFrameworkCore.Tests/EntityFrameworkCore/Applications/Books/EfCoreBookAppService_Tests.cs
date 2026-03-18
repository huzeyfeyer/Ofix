using Ofix.Books;
using Xunit;

namespace Ofix.EntityFrameworkCore.Applications.Books;

[Collection(OfixTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<OfixEntityFrameworkCoreTestModule>
{

}