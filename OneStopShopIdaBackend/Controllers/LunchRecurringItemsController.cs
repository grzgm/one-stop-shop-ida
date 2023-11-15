using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Route("lunch/recurring")]
[ApiController]
public class LunchRecurringItemsController : ControllerBase
{
    private readonly ILogger<LunchRecurringItemsController> _logger;
    private readonly DatabaseService _databaseService;
    private readonly MicrosoftGraphAPIService _microsoftGraphApiService;
    private static string RegisterRecurringMessage(string officeName, string name, LunchRecurringItem lunchRecurringItem) =>
    "Hi,\n" +
    $"I would like to register for lunch at {officeName} Office on {lunchRecurringItem}.\n" +
    "Kind Regards,\n" +
    $"{name}";

    public LunchRecurringItemsController(ILogger<LunchRecurringItemsController> logger, DatabaseService databaseService, MicrosoftGraphAPIService microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
        _microsoftGraphApiService = microsoftGraphApiService;
    }

    [HttpGet("get-registered-days")]
    public async Task<ActionResult<LunchRecurringItemFrontend>> GetRegisteredDays()
    {
        try
        {
            return new LunchRecurringItemFrontend(await _databaseService.GetRegisteredDays(HttpContext.Session.GetString("microsoftId")));
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

    [HttpPut("update-registered-days")]
    public async Task<IActionResult> PutLunchRecurringItem(LunchRecurringItemFrontend lunchRecurringItemFrontend)
    {
        try
        {
            LunchRecurringItem lunchRecurringItem =
                new(HttpContext.Session.GetString("microsoftId"), lunchRecurringItemFrontend);
            await _databaseService.PutLunchRecurringItem(lunchRecurringItem);
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

    //[HttpPost("create-lunch-recurring")]
    //public async Task<IActionResult> PostLunchRecurringItem(string microsoftId)
    //{
    //    try
    //    {
    //        await _databaseService.PostLunchRecurringItem(HttpContext.Session.GetString("microsoftId"));
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

    [HttpPut("register-for-lunch-recurring")]
    public async Task<IActionResult> PutLunchRecurringRegistrationItem([FromQuery] string officeName)
    {
        try
        {
            var microsoftId = HttpContext.Session.GetString("microsoftId");
            var accessToken = HttpContext.Session.GetString("accessToken");

            var user = await _microsoftGraphApiService.GetMe(accessToken);

            LunchRecurringItem lunchRecurringItem = await _databaseService.GetRegisteredDays(microsoftId);

            await _microsoftGraphApiService.RegisterLunchRecurring(accessToken, RegisterRecurringMessage(officeName, $"{user.FirstName} {user.Surname}", lunchRecurringItem));

            LunchRecurringRegistrationItem lunchRecurringRegistrationItem = new() { MicrosoftId = microsoftId, IsRegistered = true };
            await _databaseService.PutLunchRecurringRegistrationItem(lunchRecurringRegistrationItem);
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
}