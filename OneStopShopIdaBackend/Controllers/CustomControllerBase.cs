using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;
public class CustomControllerBase : ControllerBase
{
    protected readonly IMemoryCache MemoryCache;
    protected readonly IMicrosoftGraphApiService MicrosoftGraphApiService;
    public CustomControllerBase(IMemoryCache memoryCache, IMicrosoftGraphApiService microsoftGraphApiService)
    {
        MemoryCache = memoryCache;
        MicrosoftGraphApiService = microsoftGraphApiService;
    }

    public async Task<T> ExecuteWithRetryMicrosoftGraphApi<T>(Func<string, Task<T>> action, string accessToken)
    {
        try
        {
            // Try to call Function with given Access Token
            return await action(accessToken);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Access the access_token property
                (string newAccessToken, string refreshToken) = await MicrosoftGraphApiService.CallAuthRefresh(MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}RefreshToken") ?? string.Empty);

                // Store Access Token and Refresh Token in Memory Cache with GUID
                MemoryCache.Set($"{User.FindFirst("UserId")?.Value}AccessToken", accessToken, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Adjust expiration as needed
                });
                MemoryCache.Set($"{User.FindFirst("UserId")?.Value}RefreshToken", refreshToken, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) // Adjust expiration as needed
                });

                // Call Function with new Access Token
                return await action(newAccessToken);
            }
            throw;
        }
    }
}
