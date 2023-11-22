using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Route("lunch/today")]
[ApiController]
public class LunchTodayItemsController : ControllerBase
{
    private readonly ILogger<LunchTodayItemsController> _logger;
    private readonly IMicrosoftGraphApiService _microsoftGraphApiService;
    private readonly IDatabaseService _databaseService;

    public LunchTodayItemsController(ILogger<LunchTodayItemsController> logger,
        IMicrosoftGraphApiService microsoftGraphApiService, IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
        _microsoftGraphApiService = microsoftGraphApiService;
    }
    private static string RegisterTodayMessage(string officeName, string name) =>
        "Hi,\n" +
        $"I would like to register for today's lunch at {officeName} Office.\n" +
        "Kind Regards,\n" +
        $"{name}";

    [HttpGet("is-registered")]
    public async Task<ActionResult<bool>> GetLunchTodayIsRegistered()
    {
        try
        {
            string accessToken = HttpContext.Session.GetString("accessToken");
            string microsoftId = (await _microsoftGraphApiService.GetMe(accessToken)).MicrosoftId;

            return await _databaseService.GetLunchTodayIsRegistered(microsoftId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return Conflict();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    [HttpPut("register-lunch-today")]
    public async Task<IActionResult> PutLunchTodayRegister([FromQuery] string officeName)
    {
        try
        {
            string accessToken = HttpContext.Session.GetString("accessToken");
            string microsoftId = (await _microsoftGraphApiService.GetMe(accessToken)).MicrosoftId;

            var user = await _microsoftGraphApiService.GetMe(accessToken);
            HttpResponseMessage response = await
                _microsoftGraphApiService.RegisterLunchToday(accessToken,
                    microsoftId, RegisterTodayMessage(officeName, $"{user.FirstName} {user.Surname}"));

            if (response.IsSuccessStatusCode)
            {
            LunchTodayItem lunchTodayItem = new()
            {
                MicrosoftId = microsoftId,
                IsRegistered = true,
            };

            await _databaseService.PutLunchTodayRegister(lunchTodayItem);
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return Conflict();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    [HttpPost("create-is-registered/{microsoftId}")]
    public async Task<IActionResult> PostLunchTodayItem(string microsoftId)
    {
        try
        {
            LunchTodayItem lunchTodayItem = new LunchTodayItem()
            {
                MicrosoftId = microsoftId,
                IsRegistered = false
            };
            await _databaseService.PostLunchTodayItem(lunchTodayItem);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return Conflict();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    //[ApiExplorerSettings(IgnoreApi = true)]
    //public async Task<IActionResult> UpdateAllLunchTodayItems(bool isRegistered)
    //{
    //    try
    //    {
    //        await _databaseService.UpdateAllLunchTodayItems(isRegistered);
    //        return NoContent();
    //    }
    //    catch (InvalidOperationException ex)
    //    {
    //        _logger.LogError($"Error calling external API: {ex.Message}");
    //        return Conflict();
    //    }
    //    catch (KeyNotFoundException ex)
    //    {
    //        _logger.LogError($"Error calling external API: {ex.Message}");
    //        return NotFound();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"Error: {ex.Message}");
    //        return StatusCode(500, $"Internal Server Error \n {ex.Message}");
    //    }
    //}
}