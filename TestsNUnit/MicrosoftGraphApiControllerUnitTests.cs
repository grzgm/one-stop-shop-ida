using System.Configuration;
using Microsoft.Extensions.Logging;
using OneStopShopIdaBackend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using TestsNUnit.FakeServices;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Azure.Core;
using System.Security.Claims;

namespace TestsNUnit;

[TestFixture]
internal class MicrosoftGraphApiControllerUnitTests
{
    private FakeDatabaseService _fakeDatabaseService;
    private MicrosoftGraphApiController _microsoftGraphApiController;
    private MemoryCache _memoryCache;

    [SetUp]
    public void Setup()
    {
        // Fake cache
        _memoryCache = new MemoryCache(new MemoryCacheOptions());

        // Create Fake Database
        var options = new DbContextOptionsBuilder<FakeDatabaseService>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _fakeDatabaseService = new FakeDatabaseService(options);
        // Clear database
        _fakeDatabaseService.Database.EnsureDeleted();
        _fakeDatabaseService.Database.EnsureCreated();

        // Create Fake Microsoft Graph Api Service
        FakeMicrosoftGraphApiService fakeMicrosoftGraphApiService = new();

        // Initialise Microsoft Graph Api Controller
        _microsoftGraphApiController = new MicrosoftGraphApiController(
            new Logger<MicrosoftGraphApiController>(new LoggerFactory()), 
            FakeConfiguration.GetConfiguration(),
            _memoryCache,
            fakeMicrosoftGraphApiService,
            _fakeDatabaseService);
    }

    [Test]
    // Check whether Controller saves user information to Memory Cache correctly
    public async Task GetAuthCallbackUserInDatabaseTest()
    {
        // Arrange
        // Add Test User Item to Fake Database
        await _fakeDatabaseService.PostUserItem(FakeModelsObjects.testUserItem);

        // Act
        await _microsoftGraphApiController.GetAuthCallback("testCode", "testUserGUID");

        // Assert
        Assert.That(_memoryCache.Get<string>($"testUserGUIDAccessToken"), Is.EqualTo(FakeModelsObjects.testAccessToken));
    }

    [Test]
    // Check whether Controller saves new user information to database correctly
    public async Task GetAuthCallbackUserNotInDatabaseTest()
    {
        // Arrange
        // Fake Memory Cache
        var httpContext = FakeHttpContext.GetHttpContextFake();
        _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
        // Controller Response
        IActionResult response;

        // Act
        response = await _microsoftGraphApiController.GetAuthCallback("testCode", "testUserGUID");
        UserItem userItem = await _fakeDatabaseService.GetUserItem(FakeModelsObjects.testUserItem.MicrosoftId);
        LunchDaysItem lunchDaysItem =
            await _fakeDatabaseService.GetRegisteredDays(FakeModelsObjects.testUserItem.MicrosoftId);

        // Assert
        Assert.That(userItem.MicrosoftId, Is.EqualTo(FakeModelsObjects.testUserItem.MicrosoftId));
        Assert.That(lunchDaysItem.MicrosoftId, Is.EqualTo(FakeModelsObjects.TestLunchDaysItem.MicrosoftId));
    }

    [Test]
    // Check whether Controller saves user information to Memory Cache correctly
    public async Task GetAuthCallbackUserAuthorisedAlready()
    {
        // Arrange
        // Add Test User Item to Fake Database
        await _fakeDatabaseService.PostUserItem(FakeModelsObjects.testUserItem);
        // Fake Memory Cache
        _memoryCache.Set("testUserGUIDAccessToken", "oldAccessToken", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration as needed
        });
        _memoryCache.Set("testUserGUIDRefreshToken", "oldRefreshToken", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration as needed
        });
        // Controller Response
        IActionResult response;

        // Act
        response = await _microsoftGraphApiController.GetAuthCallback("testCode", "testUserGUID");

        // Assert
        Assert.That(_memoryCache.Get<string>($"testUserGUIDAccessToken"), Is.Not.EqualTo("oldAccessToken"));
        Assert.That(_memoryCache.Get<string>($"testUserGUIDRefreshToken"), Is.Not.EqualTo("oldRefreshToken"));
    }

    [Test]
    // Check whether Controller saves new user information to database correctly
    public async Task GetAuthRefreshTest()
    {
        // Arrange
        // Fake Memory Cache
        _memoryCache.Set("testUserGUIDAccessToken", "oldAccessToken", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration as needed
        });
        _memoryCache.Set("testUserGUIDRefreshToken", "oldRefreshToken", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration as needed
        });
        // Fake Http Context
        var httpContext = FakeHttpContext.GetHttpContextFake();
        _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
        // Fake User Id from JWT
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("UserId", "testUserGUID"),
            // Add other claims as needed
        }, "mock"));
        // Controller Response
        IActionResult response;

        // Act
        response = await _microsoftGraphApiController.GetAuthRefresh();

        // Assert
        Assert.That(_memoryCache.Get<string>($"testUserGUIDAccessToken"), Is.Not.EqualTo("oldAccessToken"));
        Assert.That(_memoryCache.Get<string>($"testUserGUIDRefreshToken"), Is.Not.EqualTo("oldRefreshToken"));
    }

    [Test]
    // Check whether Controller correctly returns information about user authentication
    public async Task GetIsAuthUserAuthenticatedTest()
    {
        // Arrange
        // Fake Memory Cache
        _memoryCache.Set("testUserGUIDAccessToken", FakeModelsObjects.testAccessToken, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration as needed
        });
        _memoryCache.Set("testUserGUIDRefreshToken", FakeModelsObjects.testRefreshToken, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration as needed
        });
        // Fake Http Context
        var httpContext = FakeHttpContext.GetHttpContextFake();
        _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
        // Fake User Id from JWT
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("UserId", "testUserGUID"),
            // Add other claims as needed
        }, "mock"));
        ActionResult<bool> response;

        // Act
        response = await _microsoftGraphApiController.GetIsAuth();

        // Assert
        Assert.That(response.Value, Is.EqualTo(true));
    }

    [Test]
    // Check whether Controller correctly returns information about user authentication
    public async Task GetIsAuthUserNotAuthenticatedTest()
    {
        // Arrange

        // Act
        var response = await _microsoftGraphApiController.GetIsAuth();

        // Assert
        Assert.That(response.Value, Is.EqualTo(false));
    }

    [Test]
    public async Task GetCheckTokenTokenInMemoryCacheTest()
    {
        // Arrange
        // Fake Memory Cache
        _memoryCache.Set("testUserGUIDAccessToken", "oldAccessToken", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration as needed
        });
        _memoryCache.Set("testUserGUIDRefreshToken", "oldRefreshToken", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Adjust expiration as needed
        });
        // Fake Http Context
        var httpContext = FakeHttpContext.GetHttpContextFake();
        _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
        // Fake User Id from JWT
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("UserId", "testUserGUID"),
            // Add other claims as needed
        }, "mock"));
        // Controller Response
        ActionResult<bool> response;

        // Act
        response = await _microsoftGraphApiController.GetCheckToken();
        var okObjectResult = response.Result as OkObjectResult;
        var isToken = (bool)okObjectResult.Value;

        // Assert
        Assert.IsNotNull(okObjectResult);
        Assert.That(isToken, Is.EqualTo(true));
    }

    [Test]
    public async Task GetCheckTokenTokenNotInMemoryCacheTest()
    {
        // Arrange
        // Fake Http Context
        var httpContext = FakeHttpContext.GetHttpContextFake();
        _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
        // Fake User Id from JWT
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("UserId", "testUserGUID"),
            // Add other claims as needed
        }, "mock"));
        // Controller Response
        ActionResult<bool> response;

        // Act
        response = await _microsoftGraphApiController.GetCheckToken();
        var okObjectResult = response.Result as OkObjectResult;
        var isToken = (bool)okObjectResult.Value;

        // Assert
        Assert.IsNotNull(okObjectResult);
        Assert.That(isToken, Is.EqualTo(false));
    }
}