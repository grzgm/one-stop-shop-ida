using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Controllers;

public partial class MicrosoftGraphApiController : ControllerBase
{
    [HttpPost("resources/send-email")]
    public async Task<IActionResult> PostSendEmail([FromQuery] string message, [FromQuery] string address)
    {
        try
        {
            HttpResponseMessage response = await
                _microsoftGraphApiService.SendEmail(HttpContext.Session.GetString("accessToken"), message, address);
            return StatusCode((int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{this.GetType().Name}\nError: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    //[HttpPost("resources/register-lunch-today")]
    //public async Task<IActionResult> PostRegisterLunchToday([FromQuery] string message)
    //{
    //    try
    //    {
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
    //    }
    //    catch (HttpRequestException ex)
    //    {
    //        _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
    //        return StatusCode(500, $"Internal Server Error \n {ex.Message}");
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"{this.GetType().Name}\nError: {ex.Message}");
    //        return StatusCode(500, $"Internal Server Error \n {ex.Message}");
    //    }
    //}

    [HttpPost("resources/create-event")]
    public async Task<IActionResult> PostCreateEvent([FromQuery] string address, [FromQuery] string title,
        [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string description)
    {
        try
        {
            HttpResponseMessage response = await
                _microsoftGraphApiService.CreateEvent(HttpContext.Session.GetString("accessToken"), address, title,
                    startDate, endDate, description);

            return StatusCode((int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{this.GetType().Name}\nError: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    //[HttpGet("resources/me")]
    //public async Task<UserItem> GetMe(string accessToken)
    //{
    //    try
    //    {
    //        UserItem user = await _microsoftGraphApiService.GetMe(accessToken);

    //        return user;
    //    }
    //    catch (HttpRequestException ex)
    //    {
    //        _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
    //        throw;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"{this.GetType().Name}\nError: {ex.Message}");
    //        throw;
    //    }
    //}
}