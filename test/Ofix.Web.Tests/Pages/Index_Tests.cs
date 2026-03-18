using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Ofix.Pages;

[Collection(OfixTestConsts.CollectionDefinitionName)]
public class Index_Tests : OfixWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
