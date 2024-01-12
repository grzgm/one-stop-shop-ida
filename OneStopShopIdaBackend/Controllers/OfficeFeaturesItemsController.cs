using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Authorize]
[Route("offices")]
[ApiController]
public class OfficeFeaturesItemsController : CustomControllerBase
{
    private readonly ILogger<OfficeFeaturesItemsController> _logger;
    private readonly IDatabaseService _databaseService;
    public OfficeFeaturesItemsController(ILogger<OfficeFeaturesItemsController> logger, IMemoryCache memoryCache,
        IDatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService) : base(memoryCache,
        microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [HttpGet("{office}")]
    public async Task<ActionResult<OfficeFeaturesItem>> GetOfficeFeaturesItem([FromRoute] string office)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken);
        office = office.ToLower();

        return (await _databaseService.GetOfficeFeaturesItem(office));
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<OfficeFeaturesItem>>> GetAllOfficeFeaturesItem()
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken);

        return await _databaseService.GetAllOfficeFeaturesItem();
    }
}
