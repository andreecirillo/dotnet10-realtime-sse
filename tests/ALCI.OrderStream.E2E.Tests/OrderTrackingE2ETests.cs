using Microsoft.Playwright;
using System.Net;

namespace ALCI.OrderStream.E2E.Tests;

public class OrderTrackingE2ETests
{
    [Fact]
    public async Task Should_Receive_SSE_Updates_And_Display_On_UI()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = Environment.GetEnvironmentVariable("HEADED") != "1"
        });

        var page = await browser.NewPageAsync();

        // Arrange
        var orderId = "550e8400-e29b-41d4-a716-446655440000";
        var clientUrl = "http://localhost:8080";
        var apiUrl = "http://localhost:8081";

        // Act
        await page.GotoAsync($"{clientUrl}/order-tracking/{orderId}");

        // Assert
        var header = page.GetByRole(AriaRole.Heading, new() { Name = "Order Tracking" });
        await Assertions.Expect(header).ToBeVisibleAsync();

        // Act
        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync($"{apiUrl}/orders/{orderId}/simulate", null);

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK);

        var statusItem = page.GetByText("Warehouse Processing");
        await Assertions.Expect(statusItem).ToBeVisibleAsync(new() { Timeout = 15000 });

        var streamingIndicator = page.GetByText("Streaming Active");
        var greenDot = page.Locator(".bg-green-500");

        await Assertions.Expect(streamingIndicator).ToBeVisibleAsync(new() { Timeout = 10000 });
        await Assertions.Expect(greenDot).ToBeVisibleAsync();
    }
}