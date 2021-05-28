using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CreditCardGenerator.Api;
using CreditCardGenerator.Api.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace CreditCardGeneratorAPITests.Integration
{
    [Collection(nameof(CreditCardCollection))]
    public class ApiTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly CreditCardFixture _fixture;

        public ApiTests(WebApplicationFactory<Startup> factory, CreditCardFixture fixture)
        {
            _factory = factory;
            _fixture = fixture;
        }

        [Theory]
        [InlineData("api/creditcards/")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            string email = "example@teste.com";
            var client = _factory.CreateClient();
            var request = $"{url}{email}";

            // Act
            var response = await client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("api/creditcards/")]
        public async Task Get_EndpointsReturnMethodNotAllowed(string url)
        {
            // Arrange
            string email = "";
            var client = _factory.CreateClient();
            var request = $"{url}{email}";

            // Act
            var response = await client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            // Assert.Equal("application/problem+json; charset=utf-8", 
            //     response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("api/creditcards/")]
        public async Task Delete_EndpointsReturnNotFound(string url)
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var client = _factory.CreateClient();
            var request = $"{url}{id}";

            // Act
            var response = await client.DeleteAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("api/creditcards/")]
        public async Task Post_EndpointCreateAndReturnSuccess(string url)
        {
            // Arrange
            var creditCard = JsonConvert.SerializeObject(_fixture.Valid());
            var client = _factory.CreateClient();

            var request = $"{url}";
            var content = new StringContent(creditCard, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(request, content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }
    }
}
