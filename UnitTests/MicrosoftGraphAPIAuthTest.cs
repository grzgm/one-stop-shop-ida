using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace UnitTests
{
    public class MicrosoftGraphAPIAuthTest
    {
        private readonly HttpClient _client;
        public MicrosoftGraphAPIAuthTest() {
            _client = new WebApplicationFactory<Program>().CreateClient();
        }
        [Fact]
        public async void GetAuthAPIEndpointTest()
        {
            // Needs to run server
            // Arrange
            var http = new HttpClient();

            // Act
            var response = await http.GetAsync($"http://localhost:3002/microsoft/auth?route=test");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void GetAuthCallbackAPIEndpointTest()
        {
            // Needs to run server
            // Arrange
            var http = new HttpClient();
            // Act
            var response = await http.GetAsync($"http://localhost:3002/microsoft/auth/callback?code=test&state=test");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async void GetAuthRefreshAPIEndpointTest()
        {
            // Needs to run server
            // Arrange
            var http = new HttpClient();

            // Act
            var response = await http.GetAsync($"http://localhost:3002/microsoft/auth/refresh");

            // Assert
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
        [Fact]
        public async void GetCheckTokenAPIEndpointTest()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("/microsoft/auth/check-token");
            var data = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void GetCheckTokenAPIEndpointPayloadTest()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("/microsoft/auth/check-token");
            var data = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("false", data);
        }

        //[Fact]
        //public async void APIEndpointTest()
        //{
        //    var response = await _client.GetAsync("");
        //    var data = await response.Content.ReadAsStringAsync();
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

    }
}
