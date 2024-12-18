using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
namespace TravelInspiration.API.IntegrationTests.Factories;

public sealed class TravelInspirationWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            const string dbName = "TravelInspirationDb_IntegrationTests";
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                {
                    "ConnectionStrings:TravelInspirationDbConnection",
                    $"Server=(localdb)\\mssqllocaldb;Database={dbName};" +
                    "Trusted_Connection=true;MultipleActiveResultSets=true;"
                }
            });
        });
        base.ConfigureWebHost(builder);
    }
}
