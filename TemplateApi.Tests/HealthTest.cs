using Microsoft.AspNetCore.Mvc.Testing;


namespace TemplateApi.Tests;

public class HealthTest
: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    } 

    [Theory]
    [InlineData("/health")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json", 
            response.Content.Headers.ContentType.ToString());
    }
}