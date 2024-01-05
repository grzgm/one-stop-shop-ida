using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Authorize]
[Route("desk/reservation")]
[ApiController]
public class DeskReservationItemsController : CustomControllerBase
{
    private readonly ILogger<DeskReservationItemsController> _logger;
    private readonly DatabaseService _databaseService;

    public DeskReservationItemsController(ILogger<DeskReservationItemsController> logger, IMemoryCache memoryCache,
        DatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService) : base(memoryCache, 
        microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [HttpGet("{office}")]
    public async Task<ActionResult<Dictionary<DateTime, Dictionary<int, DeskClusterFrontend>>>>
        GetDeskReservationForOfficeDate(
            [FromRoute] string office, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        // string microsoftId = "22";
        office = office.ToLower();
        DateTime startDate2 = startDate ?? DateTime.Now;
        DateTime endDate2 = endDate ?? DateTime.Now;

        List<OfficeDeskLayoutsItem> officeDeskLayoutsItems =
            await _databaseService.GetOfficeDeskLayoutForOffice(office);
        Dictionary<DateTime, Dictionary<int, DeskClusterFrontend>> deskClusterFrontendByDate =
            new Dictionary<DateTime, Dictionary<int, DeskClusterFrontend>>();


        for (DateTime currentDate = startDate2; currentDate <= endDate2; currentDate = currentDate.AddDays(1))
        {
            deskClusterFrontendByDate[currentDate] = new Dictionary<int, DeskClusterFrontend>();
            foreach (var officeDeskLayoutsItem in officeDeskLayoutsItems)
            {
                int clusterId = officeDeskLayoutsItem.ClusterId;
                if (!deskClusterFrontendByDate[currentDate].ContainsKey(clusterId))
                {
                    deskClusterFrontendByDate[currentDate][clusterId] = new DeskClusterFrontend()
                        { ClusterId = clusterId, Desks = new Dictionary<int, DeskFrontend>() };
                }

                deskClusterFrontendByDate[currentDate][clusterId].Desks[officeDeskLayoutsItem.DeskId] =
                    new DeskFrontend()
                    {
                        ClusterId = clusterId,
                        DeskId = officeDeskLayoutsItem.DeskId,
                        Occupied = new List<bool>(new bool[officeDeskLayoutsItem.AmountOfTimeSlots]),
                        UserReservations = new List<bool>(new bool[officeDeskLayoutsItem.AmountOfTimeSlots]),
                    };
            }
        }

        List<DeskReservationItem> deskReservationItems =
            await _databaseService.GetDeskReservationForOfficeDate(office, startDate2, endDate2);

        foreach (var deskReservationItem in deskReservationItems)
        {
            if (deskReservationItem.MicrosoftId == microsoftId)
            {
                deskClusterFrontendByDate[deskReservationItem.Date][deskReservationItem.ClusterId]
                    .Desks[deskReservationItem.DeskId]
                    .UserReservations[deskReservationItem.TimeSlot] = true;
            }
            else
            {
                deskClusterFrontendByDate[deskReservationItem.Date][deskReservationItem.ClusterId]
                    .Desks[deskReservationItem.DeskId]
                    .Occupied[deskReservationItem.TimeSlot] = true;
            }
        }

        return deskClusterFrontendByDate;
    }

    [HttpGet("{office}/layout")]
    public async Task<ActionResult<Dictionary<int, DeskClusterFrontend>>> GetDeskReservationOfficeLayout(
        [FromRoute] string office)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        office = office.ToLower();

        List<OfficeDeskLayoutsItem> officeDeskLayoutsItems =
            await _databaseService.GetOfficeDeskLayoutForOffice(office);
        Dictionary<int, DeskClusterFrontend> deskClusterFrontend = new Dictionary<int, DeskClusterFrontend>();

        foreach (var officeDeskLayoutsItem in officeDeskLayoutsItems)
        {
            int clusterId = officeDeskLayoutsItem.ClusterId;
            if (!deskClusterFrontend.ContainsKey(clusterId))
            {
                deskClusterFrontend[clusterId] = new DeskClusterFrontend()
                    { ClusterId = clusterId, Desks = new Dictionary<int, DeskFrontend>() };
            }

            deskClusterFrontend[clusterId].Desks[officeDeskLayoutsItem.DeskId] = new DeskFrontend()
            {
                ClusterId = clusterId,
                DeskId = officeDeskLayoutsItem.DeskId,
                Occupied = new List<bool>(new bool[officeDeskLayoutsItem.AmountOfTimeSlots]),
                UserReservations = new List<bool>(new bool[officeDeskLayoutsItem.AmountOfTimeSlots]),
            };
        }

        return deskClusterFrontend;
    }


    [HttpGet("{office}/user")]
    public async Task<ActionResult<List<DeskReservationItem>>> GetDeskReservationsOfUser([FromRoute] string office,
        [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        // string microsoftId = "22";
        office = office.ToLower();
        DateTime startDate2 = startDate ?? DateTime.Now;
        DateTime endDate2 = endDate ?? DateTime.Now;

        return await _databaseService.GetDeskReservationsOfUser(microsoftId, office, startDate2, endDate2);
    }


    [HttpGet("{office}/all")]
    public async Task<ActionResult<Dictionary<DateTime, DeskReservationsDayFrontend>>> GetDeskReservationsForOfficeDate(
        [FromRoute] string office,
        [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        // string microsoftId = "22";
        office = office.ToLower();
        DateTime startDate2 = startDate.HasValue ? startDate.Value.Date : DateTime.Now;
        DateTime endDate2 = endDate.HasValue ? endDate.Value.Date : DateTime.Now;

        Dictionary<DateTime, DeskReservationsDayFrontend> deskReservationsDayFrontend = new();
        for (DateTime currentDate = startDate2; currentDate <= endDate2; currentDate = currentDate.AddDays(1))
        {
            deskReservationsDayFrontend[currentDate] = new();
        }

        List<DeskReservationItem> deskReservationItems =
            await _databaseService.GetDeskReservationForOfficeDate(office, startDate2, endDate2);
        foreach (var deskReservationItem in deskReservationItems)
        {
            if (deskReservationItem.MicrosoftId == microsoftId)
            {
                deskReservationsDayFrontend[deskReservationItem.Date].UserReservations.Add(
                    new DeskReservationItemFrontend()
                    {
                        IsUser = true,
                        Date = deskReservationItem.Date,
                        ClusterId = deskReservationItem.ClusterId,
                        DeskId = deskReservationItem.DeskId,
                        TimeSlot = deskReservationItem.TimeSlot,
                    });
            }
            else
            {
                deskReservationsDayFrontend[deskReservationItem.Date].Occupied.Add(
                    new DeskReservationItemFrontend()
                    {
                        IsUser = false,
                        Date = deskReservationItem.Date,
                        ClusterId = deskReservationItem.ClusterId,
                        DeskId = deskReservationItem.DeskId,
                        TimeSlot = deskReservationItem.TimeSlot,
                    });
            }
        }

        return deskReservationsDayFrontend;
    }

    // [HttpPut("update-registered-days")]
    // public async Task<IActionResult> PutDeskReservationItem(DeskReservationItem deskReservationItemFrontend)
    // {
    //     string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
    //     string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;
    //
    //     DeskReservationItem deskReservationItem =
    //         new(microsoftId, deskReservationItemFrontend);
    //     await _databaseService.PutDeskReservation(deskReservationItem);
    //     return NoContent();
    // }

    [HttpPost("{office}")]
    public async Task<IActionResult> PostDeskReservation([FromRoute] string office, [FromQuery] DateTime date,
        [FromQuery] int clusterId, [FromQuery] int deskId, [FromQuery] List<int> timeSlots)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        List<DeskReservationItem> deskReservationItems = new();

        foreach (var timeSlot in timeSlots)
        {
            deskReservationItems.Add(new DeskReservationItem(microsoftId, office.ToLower(), date, clusterId, deskId,
                timeSlot));
        }

        if (!await _databaseService.AreDeskResservationTimeslotsDifferent(microsoftId, office, date, date, deskReservationItems))
        {
            return UnprocessableEntity();
        }
        
        await _databaseService.PostDeskReservations(deskReservationItems);
        return NoContent();
    }

    [HttpDelete("{office}")]
    public async Task<IActionResult> DeleteDeskReservation([FromRoute] string office, [FromQuery] DateTime date,
        [FromQuery] int clusterId, [FromQuery] int deskId, [FromQuery] List<int> timeSlots)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        List<DeskReservationItem> deskReservationItems = new();

        foreach (var timeSlot in timeSlots)
        {
            deskReservationItems.Add(new DeskReservationItem(microsoftId, office.ToLower(), date, clusterId, deskId,
                timeSlot));
        }

        await _databaseService.DeleteDeskReservations(deskReservationItems);
        return NoContent();
    }
}