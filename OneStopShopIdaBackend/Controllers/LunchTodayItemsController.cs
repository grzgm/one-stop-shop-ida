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
    private readonly DatabaseService _databaseService;

    public LunchTodayItemsController(ILogger<LunchTodayItemsController> logger, DatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [HttpGet("is-registered")]
    public async Task<ActionResult<bool>> GetLunchTodayIsRegistered()
    {
        try
        {
            return await _databaseService.GetLunchTodayIsRegistered(HttpContext.Session.GetString("microsoftId"));
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

    [HttpPut("update-is-registered")]
    public async Task<IActionResult> PutLunchTodayRegister([FromQuery] string microsoftId, [FromQuery] bool isRegistered)
    {
        try
        {
            LunchTodayItem lunchTodayItem = new()
            {
                MicrosoftId = microsoftId,
                IsRegistered = isRegistered,
            };
            await _databaseService.PutLunchTodayRegister(lunchTodayItem);
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

    [HttpPost("create-is-registered/{microsoftId}")]
    public async Task<IActionResult> PostLunchTodayItem(string microsoftId)
    {
        try
        {
            await _databaseService.PostLunchTodayItem(microsoftId);
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

    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> UpdateAllLunchTodayItems(bool isRegistered)
    {
        try
        {
            await _databaseService.UpdateAllLunchTodayItems(isRegistered);
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