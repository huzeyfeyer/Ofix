using Microsoft.AspNetCore.Builder;
using Ofix;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Ofix.Web.csproj"); 
await builder.RunAbpModuleAsync<OfixWebTestModule>(applicationName: "Ofix.Web");

public partial class Program
{
}
