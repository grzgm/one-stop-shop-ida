using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Controllers;

public partial class MicrosoftGraphAPIController : ControllerBase
{
    [HttpPost("resources/send-email")]
    public async Task<IActionResult> PostSendEmail([FromQuery] string message, [FromQuery] string address)
    {
        try
        {
            var response = await
                _microsoftGraphApiService.SendEmail(HttpContext.Session.GetString("accessToken"), message, address);
            return StatusCode((int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    [HttpPost("resources/register-lunch-today")]
    public async Task<IActionResult> PostRegisterLunchToday([FromQuery] string message)
    {
        try
        {
            var response = await
                _microsoftGraphApiService.RegisterLunchToday(HttpContext.Session.GetString("accessToken"),
                    HttpContext.Session.GetString("microsoftId"), message);

            if (response.IsSuccessStatusCode)
            {
                await _lunchTodayItemsController.PutLunchTodayRegister(HttpContext.Session.GetString("microsoftId"),
                    true);
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    [HttpPost("resources/create-event")]
    public async Task<IActionResult> PostCreateEvent([FromQuery] string address, [FromQuery] string title,
        [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string description)
    {
        try
        {
            var response = await
                _microsoftGraphApiService.CreateEvent(HttpContext.Session.GetString("accessToken"), address, title,
                    startDate, endDate, description);

            return StatusCode((int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    [HttpGet("resources/me")]
    public async Task<UserItem> GetMe()
    {
        try
        {
            UserItem user = await _microsoftGraphApiService.GetMe(HttpContext.Session.GetString("accessToken"));

            HttpContext.Session.SetString("microsoftId", user.MicrosoftId);

            return user;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            throw;
        }
    }
}