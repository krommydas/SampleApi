using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using SampleApi.ExternalProvider;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace SampleApi.UnitTests
{
    public class ExternalProviderHandlerTests
    {
        private HttpClient GetHttpClientMock(HttpResponseMessage serverResponseMock)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<System.Threading.Tasks.Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(serverResponseMock)
               .Verifiable();

            // use real http client with mocked handler here
            return new HttpClient(handlerMock.Object);
        }

        [Fact]
        public async void GetAlerts_When_ErrorResponse_NotFails()
        {
            var httpClientMock = GetHttpClientMock(new HttpResponseMessage(HttpStatusCode.BadRequest));
            var loggerMock = Mock.Of<ILogger>();
            var alertProviderConfigurationMock = Mock.Of<AlertExternalProviderConfiguration>();

            var externalProviderHandler = new ExternalProviderHandler(httpClientMock, loggerMock);
            var exception = await Record.ExceptionAsync(() => externalProviderHandler.GetAlerts(alertProviderConfigurationMock, CancellationToken.None));

            Assert.Null(exception);
        }
    }
}
