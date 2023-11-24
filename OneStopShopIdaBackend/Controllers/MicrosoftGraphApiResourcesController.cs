using System.Net;
using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Controllers;

public partial class MicrosoftGraphApiController : ControllerBase
{
    [HttpPost("resources/send-email")]
    public async Task<IActionResult> PostSendEmail([FromQuery] string message, [FromQuery] string address)
    {
        HttpResponseMessage response = await
            _microsoftGraphApiService.SendEmail(HttpContext.Session.GetString("accessToken"), message, address);
        return StatusCode((int)response.StatusCode);
    }

    //[HttpPost("resources/register-lunch-today")]
    //public async Task<IActionResult> PostRegisterLunchToday([FromQuery] string message)
    //{
    //        string accessToken = HttpContext.Session.GetString("accessToken");
    //        string microsoftId = (await _microsoftGraphApiService.GetMe(accessToken)).MicrosoftId;
    //        HttpResponseMessage response = await
    //            _microsoftGraphApiService.RegisterLunchToday(accessToken,
    //                microsoftId, message);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            LunchTodayItem lunchTodayItem = new()
    //            {
    //                MicrosoftId = microsoftId,
    //                IsRegistered = true,
    //            };

    //            await _databaseService.PutLunchTodayRegister(lunchTodayItem);
    //        }

    //        return StatusCode((int)response.StatusCode);
    //}

    [HttpPost("resources/create-event")]
    public async Task<IActionResult> PostCreateEvent([FromQuery] string address, [FromQuery] string title,
        [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string description)
    {
        HttpResponseMessage response = await
            _microsoftGraphApiService.CreateEvent(HttpContext.Session.GetString("accessToken"), address, title,
                startDate, endDate, description);

        return StatusCode((int)response.StatusCode);
    }

    //[HttpGet("resources/me")]
    //public async Task<UserItem> GetMe(string accessToken)
    //{
    //        UserItem user = await _microsoftGraphApiService.GetMe(accessToken);
    //
    //        return user;
    //}
}