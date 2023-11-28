using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;
public class CustomControllerBase : ControllerBase
{
    protected readonly IMicrosoftGraphApiService _microsoftGraphApiService;
    public CustomControllerBase(IMicrosoftGraphApiService microsoftGraphApiService)
    {
        _microsoftGraphApiService = microsoftGraphApiService;
    }

    public async Task<T> ExecuteWithRetry<T>(Func<string, Task<T>> action, string accessToken)
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
                (string newAccessToken, string refreshToken) = await _microsoftGraphApiService.CallAuthRefresh(HttpContext.Session.GetString("refreshToken"));

                // Store accessToken and refreshToken in the session
                HttpContext.Session.SetString("accessToken", accessToken);
                HttpContext.Session.SetString("refreshToken", refreshToken);

                // Call Function with new Access Token
                return await action(newAccessToken);
            }
            throw;
        }
    }
}
