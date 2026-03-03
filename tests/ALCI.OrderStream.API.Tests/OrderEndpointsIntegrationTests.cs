using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ALCI.OrderStream.API.Tests;

public class OrderEndpointsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderEndpointsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SimulateOrder_ShouldReturnAccepted_OrOk()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync($"/orders/{orderId}/simulate", null);

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted);
    }

    [Fact]
    public async Task StreamEndpoint_ShouldHaveCorrectContentType()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)); // Safety timeout

        // Act        
        var request = new HttpRequestMessage(HttpMethod.Get, $"/orders/{orderId}/stream");

        using var response = await _client.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cts.Token);

        // Assert
        Assert.Equal("text/event-stream", response.Content.Headers.ContentType?.MediaType);
                
        request.Dispose();
    }
}