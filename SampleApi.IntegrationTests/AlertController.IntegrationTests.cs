using Microsoft.AspNetCore.Mvc.Testing;
using System;
using Xunit;

namespace SampleApi.IntegrationTests
{
    public class AlertController_IntegrationTests
        : IClassFixture<WebApplicationFactory<SampleApi.Startup>>
    {
        public AlertController_IntegrationTests(WebApplicationFactory<SampleApi.Startup> factory)
        {
            Factory = factory;
        }

        private readonly WebApplicationFactory<SampleApi.Startup> Factory;

        [Fact]
        public async void GetAlerts_ReturnsSuccessStatus()
        {
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("alert");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
