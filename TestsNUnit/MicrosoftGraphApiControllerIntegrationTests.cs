using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using OneStopShopIdaBackend.Controllers;
using OneStopShopIdaBackend.Services;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using TestsNUnit.FakeServices;

namespace TestsNUnit;

[TestFixture]
internal class MicrosoftGraphApiControllerIntegrationTests
{
    private readonly HttpClient _client;
    private DatabaseServiceFake _databaseServiceFake;
    private MicrosoftGraphApiController _microsoftGraphApiController;

    // public MicrosoftGraphApiControllerIntegrationTests()
    // {
    //     _client = new WebApplicationFactory<Program>().CreateClient();
    // }

    [SetUp]
    public void Setup()
    {
        // Create Fake Database
        var options = new DbContextOptionsBuilder<DatabaseServiceFake>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _databaseServiceFake = new DatabaseServiceFake(options);
        // Clear database
        _databaseServiceFake.Database.EnsureDeleted();
        _databaseServiceFake.Database.EnsureCreated();

        // Create Fake Microsoft Graph Api Service
        MicrosoftGraphApiServiceFake microsoftGraphApiServiceFake = new();

        // Initialise Microsoft Graph Api Controller
        _microsoftGraphApiController = new MicrosoftGraphApiController(
            new Logger<MicrosoftGraphApiController>(new LoggerFactory()), microsoftGraphApiServiceFake,
            _databaseServiceFake);
    }

    [Test]
    // Check whether Controller saves user information to session correctly
    public async Task GetAuthCallbackUserInDatabaseTest()
    {
        // Arrange
        // Add Test User Item to Fake Database
        await _databaseServiceFake.PostUserItem(ModelsObjectsFake.testUserItem);
        // Fake Session
        var httpContext = HttpContextFake.GetHttpContextFake();
        _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
        // Controller Response
        IActionResult response;

        // Act
        response = await _microsoftGraphApiController.GetAuthCallback("testCode", "testState");

        // Assert
        Assert.AreEqual(_microsoftGraphApiController.HttpContext.Session.GetString("microsoftId"),
            ModelsObjectsFake.testUserItem.MicrosoftId);
    }

    [Test]
    // Check whether Controller saves new user information to database correctly
    public async Task GetAuthCallbackUserNotInDatabaseTest()
    {
        // Arrange
        // Fake Session
        var httpContext = HttpContextFake.GetHttpContextFake();
        _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
        // Controller Response
        IActionResult response;

        // Act
        response = await _microsoftGraphApiController.GetAuthCallback("testCode", "testState");
        UserItem userItem = await _databaseServiceFake.GetUserItem(ModelsObjectsFake.testUserItem.MicrosoftId);
        LunchRecurringItem lunchRecurringItem =
            await _databaseServiceFake.GetRegisteredDays(ModelsObjectsFake.testUserItem.MicrosoftId);

        // Assert
        Assert.AreEqual(userItem.MicrosoftId, ModelsObjectsFake.testUserItem.MicrosoftId);
        Assert.AreEqual(lunchRecurringItem.MicrosoftId, ModelsObjectsFake.testLunchRecurringItem.MicrosoftId);
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
    // [Test]
    // public async Task GetCheckTokenAPIEndpointTest()
    // {
    //     // Arrange
    //     // Act
    //     var response = await _client.GetAsync("/microsoft/auth/check-token");
    //     var data = await response.Content.ReadAsStringAsync();
    //
    //     // Assert
    //     Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    // }
    // [Test]
    // public async Task GetCheckTokenAPIEndpointPayloadTest()
    // {
    //     // Arrange
    //     // Act
    //     var response = await _client.GetAsync("/microsoft/auth/check-token");
    //     var data = await response.Content.ReadAsStringAsync();
    //
    //     // Assert
    //     Assert.AreEqual("false", data);
    // }
}