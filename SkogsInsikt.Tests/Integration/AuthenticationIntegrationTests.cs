using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SkogsInsikt.Tests.Integration;

public class AuthenticationIntegrationTests
    : IClassFixture<SkogsInsiktWebApplicationFactory>
{
    private readonly SkogsInsiktWebApplicationFactory _factory;

    public AuthenticationIntegrationTests(
        SkogsInsiktWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Register_ReturnsJwtToken()
    {
        using var client = _factory.CreateClient();

        var email =
            $"register-{Guid.NewGuid()}@skogsinsikt.test";

        var response = await client.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                fullName = "Integration Test User",
                email,
                password = "Test12345"
            });

        response.EnsureSuccessStatusCode();

        using var json = JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());

        var token =
            json.RootElement
                .GetProperty("token")
                .GetString();

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task ForestAreas_WithoutToken_ReturnsUnauthorized()
    {
        using var client = _factory.CreateClient();

        var response =
            await client.GetAsync("/api/ForestAreas");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task UserCannotAccessAnotherUsersForestArea()
    {
        using var userAClient = _factory.CreateClient();
        using var userBClient = _factory.CreateClient();

        var tokenA = await RegisterAndGetToken(
            userAClient,
            $"user-a-{Guid.NewGuid()}@skogsinsikt.test");

        var tokenB = await RegisterAndGetToken(
            userBClient,
            $"user-b-{Guid.NewGuid()}@skogsinsikt.test");

        userAClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                tokenA);

        userBClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                tokenB);

        var createResponse =
            await userBClient.PostAsJsonAsync(
                "/api/ForestAreas",
                new
                {
                    name = "User B Forest",
                    areaHectares = 25.5,
                    treeSpecies = "Gran",
                    plantingYear = 2010,
                    latitude = 56.66,
                    longitude = 16.35
                });

        createResponse.EnsureSuccessStatusCode();

        using var createdJson = JsonDocument.Parse(
            await createResponse.Content.ReadAsStringAsync());

        var forestAreaId =
            createdJson.RootElement
                .GetProperty("id")
                .GetInt32();

        var ownResponse =
            await userBClient.GetAsync(
                $"/api/ForestAreas/{forestAreaId}");

        Assert.Equal(
            HttpStatusCode.OK,
            ownResponse.StatusCode);

        var otherUserResponse =
            await userAClient.GetAsync(
                $"/api/ForestAreas/{forestAreaId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            otherUserResponse.StatusCode);
    }

    private static async Task<string> RegisterAndGetToken(
        HttpClient client,
        string email)
    {
        var response = await client.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                fullName = "Integration Test User",
                email,
                password = "Test12345"
            });

        response.EnsureSuccessStatusCode();

        using var json = JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());

        return json.RootElement
            .GetProperty("token")
            .GetString()
            ?? throw new InvalidOperationException(
                "JWT token saknas i svaret.");
    }
}
