using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Route("desk/reservation")]
[ApiController]
public class DeskReservationItemsController : CustomControllerBase
{
    private readonly ILogger<DeskReservationItemsController> _logger;
    private readonly DatabaseService _databaseService;

    public DeskReservationItemsController(ILogger<DeskReservationItemsController> logger,
        DatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService) : base(
        microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    private static string RegisterRecurringMessage(string officeName, string name,
        DeskReservationItem deskReservationItem) =>
        "Hi,\n" +
        $"I would like to register for lunch at {officeName} Office on {deskReservationItem} in the next week.\n" +
        "Kind Regards,\n" +
        $"{name}";

    private static string DeregisterRecurringMessage(string officeName, string name) =>
        "Hi,\n" +
        $"I would like to deregister from lunch list at {officeName} Office in the next week.\n" +
        "Kind Regards,\n" +
        $"{name}";

    [HttpGet("{office}")]
    public async Task<ActionResult<Dictionary<int, DeskClusterFrontend>>> GetDeskReservationForOfficeDate(
            [FromRoute] string office, [FromQuery] DateTime date)
    {
        // string accessToken = HttpContext.Session.GetString("accessToken");
        // string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;
        List<OfficeDeskLayoutsItem> officeDeskLayoutsItems =
            await _databaseService.GetOfficeDeskLayoutForOffice(office);
        Dictionary<int, DeskClusterFrontend> deskClusterFrontend = new Dictionary<int, DeskClusterFrontend>();

        foreach (var officeDeskLayoutsItem in officeDeskLayoutsItems)
        {
            int clusterId = officeDeskLayoutsItem.ClusterId;
            if (!deskClusterFrontend.ContainsKey(clusterId))
            {
                deskClusterFrontend[clusterId] = new DeskClusterFrontend() { ClusterId = clusterId, Desks = new Dictionary<int, DeskFrontend>() };
            }

            deskClusterFrontend[clusterId].Desks[officeDeskLayoutsItem.DeskId] = new DeskFrontend()
            {
                ClusterId = clusterId,
                DeskId = officeDeskLayoutsItem.DeskId,
                Occupied = new List<bool>(new bool[officeDeskLayoutsItem.AmountOfTimeSlots])
            };
        }

        List<DeskReservationItem> deskReservationItems =
            await _databaseService.GetDeskReservationForOfficeDate(office, date);

        foreach (var deskReservationItem in deskReservationItems)
        {
            deskClusterFrontend[deskReservationItem.ClusterId].Desks[deskReservationItem.DeskId].Occupied[deskReservationItem.TimeSlot] = true;
        }

        return deskClusterFrontend;
    }

    [HttpGet("for-user")]
    public async Task<ActionResult<List<DeskReservationItem>>> GetDeskReservationForUser([FromQuery] string microsoftId)
    {
        // string accessToken = HttpContext.Session.GetString("accessToken");
        // string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;
        return await _databaseService.GetDeskReservationForUser(microsoftId);
    }

    // [HttpPut("update-registered-days")]
    // public async Task<IActionResult> PutDeskReservationItem(DeskReservationItem deskReservationItemFrontend)
    // {
    //     string accessToken = HttpContext.Session.GetString("accessToken");
    //     string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;
    //
    //     DeskReservationItem deskReservationItem =
    //         new(microsoftId, deskReservationItemFrontend);
    //     await _databaseService.PutDeskReservation(deskReservationItem);
    //     return NoContent();
    // }

    //[HttpPost("create-lunch-recurring")]
    //public async Task<IActionResult> PostDeskReservationItem(string microsoftId)
    //{
    //        string accessToken = HttpContext.Session.GetString("accessToken");
    //        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(async (accessToken) => await _microsoftGraphApiService.GetMe(accessToken), accessToken)).MicrosoftId;
    //
    //        await _databaseService.PostDeskReservationItem(microsoftId);
    //        return NoContent();
    //}

    // [HttpPut("register-for-lunch-recurring")]
    // public async Task<IActionResult> PutDeskReservationRegistrationItem([FromQuery] string officeName)
    // {
    //     string accessToken = HttpContext.Session.GetString("accessToken");
    //     string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;
    //
    //     var user = await _microsoftGraphApiService.GetMe(accessToken);
    //
    //     DeskReservationItem deskReservationItem = await _databaseService.GetRegisteredDays(microsoftId);
    //
    //     string message;
    //
    //     if (deskReservationItem.IsRegistered()) message = RegisterRecurringMessage(officeName, $"{user.FirstName} {user.Surname}", deskReservationItem);
    //     else message = DeregisterRecurringMessage(officeName, $"{user.FirstName} {user.Surname}");
    //
    //     await _microsoftGraphApiService.SendEmail(accessToken, _lunchEmailAddress, "Lunch Registration", message);
    //
    //     DeskReservationItem deskReservationRegistrationItem = new() { MicrosoftId = microsoftId, LastRegistered = DateTime.Now };
    //     await _databaseService.PutDeskReservation(deskReservationRegistrationItem);
    //     return NoContent();
    // }
}