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
        public async void CheckTokenAPIEndpointTest()
        {
            var response = await _client.GetAsync("/microsoft/auth/check-token");
            var data = await response.Content.ReadAsStringAsync();
            //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //[Fact]
        //public async void APIEndpointTest()
        //{
        //    var response = await _client.GetAsync("");
        //    var data = await response.Content.ReadAsStringAsync();
        //    //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

    }
}
