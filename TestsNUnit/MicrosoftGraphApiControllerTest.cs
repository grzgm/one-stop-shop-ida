using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using OneStopShopIdaBackend.Controllers;
using OneStopShopIdaBackend.Services;
using System.Net;
using TestsNUnit.FakeServices;

namespace TestsNUnit;
internal class MicrosoftGraphApiControllerTest
{
    private readonly HttpClient _client;
    private MicrosoftGraphApiController _microsoftGraphApiController;
    public MicrosoftGraphApiControllerTest()
    {
        _client = new WebApplicationFactory<Program>().CreateClient();
        DatabaseServiceFake databaseServiceFake = new ();
        MicrosoftGraphApiServiceFake microsoftGraphApiServiceFake = new ();
        _microsoftGraphApiController = new MicrosoftGraphApiController(new Logger<MicrosoftGraphApiController>(new LoggerFactory()), microsoftGraphApiServiceFake, databaseServiceFake);
    }
    [SetUp]
    public void Setup()
    {
        DatabaseServiceFake databaseServiceFake = new ();
        MicrosoftGraphApiServiceFake microsoftGraphApiServiceFake = new ();
        _microsoftGraphApiController = new MicrosoftGraphApiController(new Logger<MicrosoftGraphApiController>(new LoggerFactory()), microsoftGraphApiServiceFake, databaseServiceFake);
    }
    //[Test]
    //public async Task GetAuthAPIEndpointTest()
    //{
    //    // Needs to run server
    //    // Arrange
    //    var http = new HttpClient();

    //    // Act
    //    var response = await http.GetAsync($"http://localhost:3002/microsoft/auth?route=test");

    //    // Assert
    //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    //}
    //[Test]
    //public async Task GetAuthCallbackAPIEndpointTest()
    //{
    //    // Needs to run server
    //    // Arrange
    //    var http = new HttpClient();
    //    // Act
    //    var response = await http.GetAsync($"http://localhost:3002/microsoft/auth/callback?code=test&state=test");

    //    // Assert
    //    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    //}
    //[Test]
    //public async Task GetAuthRefreshAPIEndpointTest()
    //{
    //    // Needs to run server
    //    // Arrange
    //    var http = new HttpClient();

    //    // Act
    //    var response = await http.GetAsync($"http://localhost:3002/microsoft/auth/refresh");

    //    // Assert
    //    Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    //}
    [Test]
    public async Task GetCheckTokenAPIEndpointTest()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/microsoft/auth/check-token");
        var data = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
    [Test]
    public async Task GetCheckTokenAPIEndpointPayloadTest()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/microsoft/auth/check-token");
        var data = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual("false", data);
    }
}
