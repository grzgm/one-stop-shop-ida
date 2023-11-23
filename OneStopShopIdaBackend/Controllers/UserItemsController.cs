//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using OneStopShopIdaBackend.Models;
//using OneStopShopIdaBackend.Services;

//namespace OneStopShopIdaBackend.Controllers;

//[Route("users")]
//[ApiController]
//public class UserItemsController : ControllerBase
//{
//    private readonly ILogger<UserItemsController> _logger;
//    private readonly IDatabaseService _databaseService;

//    public UserItemsController(ILogger<UserItemsController> logger, IDatabaseService databaseService)
//    {
//        _logger = logger;
//        _databaseService = databaseService;
//    }

//    [HttpGet("get-users")]
//    public async Task<ActionResult<IEnumerable<UserItem>>> GetUserItems()
//    {
//        try
//        {
//            return Ok(await _databaseService.GetUserItems());
//        }
//        catch (InvalidOperationException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return Conflict();
//        }
//        catch (KeyNotFoundException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return NotFound();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Error: {ex.Message}");
//            return StatusCode(500);
//        }
//    }

//    [HttpGet("get-user")]
//    public async Task<ActionResult<UserItem>> GetUserItem(string microsoftId)
//    {
//        try
//        {
//            return await _databaseService.GetUserItem(microsoftId);
//        }
//        catch (InvalidOperationException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return Conflict();
//        }
//        catch (KeyNotFoundException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return NotFound();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Error: {ex.Message}");
//            return StatusCode(500);
//        }
//    }

//    [HttpGet("user-exists")]
//    public async Task<bool> GetIsUserInDatabase(string microsoftId)
//    {
//        var userItem = await _databaseService.GetUserItem(microsoftId);

//        if (userItem == null)
//        {
//            return false;
//        }

//        return true;
//    }

//    [HttpPut("put-user")]
//    public async Task<IActionResult> PutUserItem(UserItem userItem)
//    {
//        try
//        {
//            await _databaseService.PutUserItem(userItem);
//            return NoContent();
//        }
//        catch (InvalidOperationException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return Conflict();
//        }
//        catch (KeyNotFoundException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return NotFound();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Error: {ex.Message}");
//            return StatusCode(500);
//        }
//    }

//    [HttpPost("post-user")]
//    public async Task<ActionResult<UserItem>> PostUserItem(UserItem userItem)
//    {
//        try
//        {
//            await _databaseService.PostUserItem(userItem);
//            return CreatedAtAction("GetUserItem", new { id = userItem.MicrosoftId }, userItem);
//        }
//        catch (InvalidOperationException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return Conflict();
//        }
//        catch (KeyNotFoundException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return NotFound();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Error: {ex.Message}");
//            return StatusCode(500);
//        }
//    }

//    [HttpDelete("delete-user")]
//    public async Task<IActionResult> DeleteUserItem(string microsoftId)
//    {
//        try
//        {
//            _databaseService.DeleteUserItem(microsoftId);

//            return NoContent();
//        }
//        catch (InvalidOperationException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return Conflict();
//        }
//        catch (KeyNotFoundException ex)
//        {
//            _logger.LogError($"Error calling external API: {ex.Message}");
//            return NotFound();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Error: {ex.Message}");
//            return StatusCode(500);
//        }
//    }
//}