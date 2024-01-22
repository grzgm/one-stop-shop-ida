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
    private readonly IDatabaseService _databaseService;
    public OfficeFeaturesItemsController(IMemoryCache memoryCache,
        IDatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService) : base(memoryCache,
        microsoftGraphApiService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("{office}")]
    public async Task<ActionResult<OfficeFeaturesItem>> GetOfficeFeaturesItem([FromRoute] string office)
    {
        string accessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}AccessToken") ?? string.Empty;
        await ExecuteWithRetryMicrosoftGraphApi(MicrosoftGraphApiService.GetMe, accessToken);
        office = office.ToLower();

        return (await _databaseService.GetOfficeFeaturesItem(office));
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<OfficeFeaturesItem>>> GetAllOfficeFeaturesItem()
    {
        string accessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}AccessToken") ?? string.Empty;
        await ExecuteWithRetryMicrosoftGraphApi(MicrosoftGraphApiService.GetMe, accessToken);
            
        return await _databaseService.GetAllOfficeFeaturesItem();
    }
}
