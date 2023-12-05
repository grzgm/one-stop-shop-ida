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
    public async Task<ActionResult<List<DeskClusterFrontend>>> GetDeskReservationForOfficeDate(
            [FromRoute] string office, [FromQuery] DateTime date)
        // string accessToken = HttpContext.Session.GetString("accessToken");
        // string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;
    {
        List<DeskClusterFrontend> deskClustersFrontend = new();
        List<OfficeDeskLayoutsItem> officeDeskLayoutsItems =
            await _databaseService.GetOfficeDeskLayoutForOffice(office);

        foreach (var officeDeskLayoutsItem in officeDeskLayoutsItems)
        {
            while (deskClustersFrontend.Count <= officeDeskLayoutsItem.ClusterId)
            {
                deskClustersFrontend.Add(new DeskClusterFrontend());
            }

            while (deskClustersFrontend[officeDeskLayoutsItem.ClusterId].Desks.Count <= officeDeskLayoutsItem.DeskId)
            {
                deskClustersFrontend[officeDeskLayoutsItem.ClusterId].Desks.Add(new DeskFrontend());
            }

            deskClustersFrontend[officeDeskLayoutsItem.ClusterId].Desks[officeDeskLayoutsItem.DeskId] =
                new DeskFrontend()
                {
                    ClusterId = officeDeskLayoutsItem.ClusterId,
                    DeskId = officeDeskLayoutsItem.DeskId,
                    Occupied = new(new bool[officeDeskLayoutsItem.AmountOfTimeSlots])
                };
            // if (deskClusterFrontend.Any(e => e.ClusterId == officeDeskLayoutsItem.ClusterId))
            // {
            //     deskClusterFrontend[officeDeskLayoutsItem.ClusterId].Desks.Insert(officeDeskLayoutsItem.DeskId, new DeskFrontend()
            //         {
            //             ClusterId = officeDeskLayoutsItem.ClusterId,
            //             DeskId = officeDeskLayoutsItem.DeskId,
            //             Occupied = new (new bool[officeDeskLayoutsItem.AmountOfTimeSlots])
            //         }
            //     );
            // }
            // else
            // {
            //     deskClusterFrontend.Insert(officeDeskLayoutsItem.ClusterId, new DeskClusterFrontend()
            //     {
            //         ClusterId = officeDeskLayoutsItem.ClusterId,
            //         Desks = new()
            //         {
            //             new DeskFrontend()
            //             {
            //                 ClusterId = officeDeskLayoutsItem.ClusterId,
            //                 DeskId = officeDeskLayoutsItem.DeskId,
            //                 Occupied = new (new bool[officeDeskLayoutsItem.AmountOfTimeSlots])
            //             }
            //         }
            //     });
            // }
        }

        foreach (var deskClusterFrontend in deskClustersFrontend)
        {
            if (!deskClusterFrontend.Desks.Any())
            {
                deskClustersFrontend.Remove(deskClusterFrontend);
            }
            foreach (var desk in deskClusterFrontend.Desks)
            {
                if (desk.DeskId == null)
                {
                    deskClusterFrontend.Desks.Remove(desk);
                }
            }
        }

        List<DeskReservationItem> deskReservationItems =
            await _databaseService.GetDeskReservationForOfficeDate(office, date);

        foreach (var deskReservationItem in deskReservationItems)
        {
            deskClustersFrontend[deskReservationItem.ClusterId].Desks[deskReservationItem.DeskId]
                .Occupied[deskReservationItem.TimeSlot] = true;
        }

        return deskClustersFrontend;
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