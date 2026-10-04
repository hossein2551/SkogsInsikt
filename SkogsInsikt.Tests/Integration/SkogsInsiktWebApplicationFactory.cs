using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SkogsInsikt.Infrastructure.Data;

namespace SkogsInsikt.Tests.Integration;

public class SkogsInsiktWebApplicationFactory
    : WebApplicationFactory<Program>
{
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
                DbContextOptions<SkogsInsiktDbContext>>();

            services.RemoveAll<SkogsInsiktDbContext>();

            services.AddDbContext<SkogsInsiktDbContext>(
                options =>
                {
                    options.UseInMemoryDatabase(
                        $"SkogsInsiktTests-{Guid.NewGuid()}");
                });

            using var scope =
                services.BuildServiceProvider()
                    .CreateScope();

            var context =
                scope.ServiceProvider
                    .GetRequiredService<SkogsInsiktDbContext>();

            context.Database.EnsureCreated();
        });
    }
}
