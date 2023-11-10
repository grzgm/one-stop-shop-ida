using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Route("lunch/recurring")]
[ApiController]
public class LunchRecurringItemsController : ControllerBase
{
    private readonly ILogger<LunchTodayItemsController> _logger;
    private readonly DatabaseService _databaseService;

    public LunchRecurringItemsController(ILogger<LunchTodayItemsController> logger, DatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [HttpGet("get-registered-days")]
    public async Task<ActionResult<LunchRecurringItem>> GetRegisteredDays()
    {
        try
        {
            return await _databaseService.GetRegisteredDays(HttpContext.Session.GetString("microsoftId"));
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

    [HttpPost("create-lunch-recurring")]
    public async Task<IActionResult> PostLunchRecurringItem(string microsoftId)
    {
        try
        {
            await _databaseService.PostLunchRecurringItem(HttpContext.Session.GetString("microsoftId"));
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

    private bool LunchRecurringItemExists(string id)
    {
        return (_databaseService.LunchRecurring?.Any(e => e.MicrosoftId == id)).GetValueOrDefault();
    }
}