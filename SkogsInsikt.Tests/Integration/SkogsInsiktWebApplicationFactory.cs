using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SkogsInsikt.Infrastructure.Data;

namespace SkogsInsikt.Tests.Integration;

public class SkogsInsiktWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly string _databaseName =
        $"SkogsInsiktTests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting(
            "Jwt:Key",
            "IntegrationTestJwtKeyThatIsLongEnoughForTesting123456789");

        builder.UseSetting(
            "Jwt:Issuer",
            "SkogsInsikt.Tests");

        builder.UseSetting(
            "Jwt:Audience",
            "SkogsInsikt.Tests");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<
                IDbContextOptionsConfiguration<SkogsInsiktDbContext>>();

            services.RemoveAll<
                DbContextOptions<SkogsInsiktDbContext>>();

            services.RemoveAll<SkogsInsiktDbContext>();

            services.AddDbContext<SkogsInsiktDbContext>(
                options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                });
        });
    }
}
