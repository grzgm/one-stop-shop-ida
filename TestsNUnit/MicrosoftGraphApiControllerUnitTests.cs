// using System.Configuration;
// using Microsoft.Extensions.Logging;
// using OneStopShopIdaBackend.Controllers;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using OneStopShopIdaBackend.Models;
// using TestsNUnit.FakeServices;
// using System.Text;
// using Microsoft.Extensions.Caching.Memory;
//
// namespace TestsNUnit;
//
// [TestFixture]
// internal class MicrosoftGraphApiControllerUnitTests
// {
//     private DatabaseServiceFake _databaseServiceFake;
//     private MicrosoftGraphApiController _microsoftGraphApiController;
//
//     [SetUp]
//     public void Setup()
//     {
//         // Create Fake Database
//         var options = new DbContextOptionsBuilder<DatabaseServiceFake>()
//             .UseInMemoryDatabase(databaseName: "TestDatabase")
//             .Options;
//         _databaseServiceFake = new DatabaseServiceFake(options);
//         // Clear database
//         _databaseServiceFake.Database.EnsureDeleted();
//         _databaseServiceFake.Database.EnsureCreated();
//
//         // Create Fake Microsoft Graph Api Service
//         MicrosoftGraphApiServiceFake microsoftGraphApiServiceFake = new();
//
//         // Initialise Microsoft Graph Api Controller
//         _microsoftGraphApiController = new MicrosoftGraphApiController(
//             new Logger<MicrosoftGraphApiController>(new LoggerFactory()), 
//             ConfigurationFake.GetConfiguration(),
//             new MemoryCache(),
//             microsoftGraphApiServiceFake,
//             _databaseServiceFake);
//     }
//
//     [Test]
//     // Check whether Controller saves user information to session correctly
//     public async Task GetAuthCallbackUserInDatabaseTest()
//     {
//         // Arrange
//         // Add Test User Item to Fake Database
//         await _databaseServiceFake.PostUserItem(ModelsObjectsFake.testUserItem);
//         // Fake Session
//         var httpContext = HttpContextFake.GetHttpContextFake();
//         _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
//         // Controller Response
//         IActionResult response;
//
//         // Act
//         response = await _microsoftGraphApiController.GetAuthCallback("testCode", "testState");
//
//         // Assert
//         Assert.AreEqual(ModelsObjectsFake.testAccessToken, _microsoftGraphApiController.HttpContext.Session.GetString("accessToken"));
//     }
//
//     [Test]
//     // Check whether Controller saves new user information to database correctly
//     public async Task GetAuthCallbackUserNotInDatabaseTest()
//     {
//         // Arrange
//         // Fake Session
//         var httpContext = HttpContextFake.GetHttpContextFake();
//         _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
//         // Controller Response
//         IActionResult response;
//
//         // Act
//         response = await _microsoftGraphApiController.GetAuthCallback("testCode", "testState");
//         UserItem userItem = await _databaseServiceFake.GetUserItem(ModelsObjectsFake.testUserItem.MicrosoftId);
//         LunchRecurringItem lunchRecurringItem =
//             await _databaseServiceFake.GetRegisteredDays(ModelsObjectsFake.testUserItem.MicrosoftId);
//
//         // Assert
//         Assert.AreEqual(ModelsObjectsFake.testUserItem.MicrosoftId, userItem.MicrosoftId);
//         Assert.AreEqual(ModelsObjectsFake.testLunchRecurringItem.MicrosoftId, lunchRecurringItem.MicrosoftId);
//     }
//
//     [Test]
//     // Check whether Controller saves user information to session correctly
//     public async Task GetAuthCallbackUserAuthorisedAlready()
//     {
//         // Arrange
//         // Add Test User Item to Fake Database
//         await _databaseServiceFake.PostUserItem(ModelsObjectsFake.testUserItem);
//         // Fake Session
//         var httpContext = HttpContextFake.GetHttpContextFake();
//         httpContext.Session.SetString("accessToken", "oldAccessToken");
//         httpContext.Session.SetString("refreshToken", "oldRefreshToken");
//         _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
//         // Controller Response
//         IActionResult response;
//
//         // Act
//         response = await _microsoftGraphApiController.GetAuthCallback("testCode", "testState");
//
//         // Assert
//         Assert.AreNotEqual("oldAccessToken", _microsoftGraphApiController.HttpContext.Session.GetString("accessToken"));
//         Assert.AreNotEqual("oldRefreshToken", _microsoftGraphApiController.HttpContext.Session.GetString("refreshToken"));
//     }
//
//
//     [Test]
//     // Check whether Controller saves new user information to database correctly
//     public async Task GetAuthRefreshTest()
//     {
//         // Arrange
//         // Fake Session
//         var httpContext = HttpContextFake.GetHttpContextFake();
//         httpContext.Session.SetString("accessToken", "oldAccessToken");
//         httpContext.Session.SetString("refreshToken", "oldRefreshToken");
//         _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
//         // Controller Response
//         IActionResult response;
//
//         // Act
//         response = await _microsoftGraphApiController.GetAuthRefresh();
//
//         // Assert
//         Assert.AreNotEqual("oldAccessToken", _microsoftGraphApiController.HttpContext.Session.GetString("accessToken"));
//         Assert.AreNotEqual("oldRefreshToken", _microsoftGraphApiController.HttpContext.Session.GetString("refreshToken"));
//     }
//
//     [Test]
//     public async Task GetCheckTokenTokenInSessionTest()
//     {
//         // Arrange
//         // Fake Session
//         var httpContext = HttpContextFake.GetHttpContextFake();
//         httpContext.Session.SetString("accessToken", "oldAccessToken");
//         httpContext.Session.SetString("refreshToken", "oldRefreshToken");
//         _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
//         // Controller Response
//         ActionResult<bool> response;
//
//         // Act
//         response = await _microsoftGraphApiController.GetCheckToken();
//         var okObjectResult = response.Result as OkObjectResult;
//         var isToken = (bool)okObjectResult.Value;
//
//         // Assert
//         Assert.IsNotNull(okObjectResult);
//         Assert.AreEqual(true, isToken);
//     }
//
//     [Test]
//     public async Task GetCheckTokenTokenNotInSessionTest()
//     {
//         // Arrange
//         // Fake Session
//         var httpContext = HttpContextFake.GetHttpContextFake();
//         _microsoftGraphApiController.ControllerContext = new ControllerContext() { HttpContext = httpContext };
//         // Controller Response
//         ActionResult<bool> response;
//
//         // Act
//         response = await _microsoftGraphApiController.GetCheckToken();
//         var okObjectResult = response.Result as OkObjectResult;
//         var isToken = (bool)okObjectResult.Value;
//
//         // Assert
//         Assert.IsNotNull(okObjectResult);
//         Assert.AreEqual(false, isToken);
//     }
// }