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

    public async Task<T> ExecuteWithRetry<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var t = HttpContext.Session.GetString("refreshToken");
                // Access the access_token property
                (string accessToken, string refreshToken) = await _microsoftGraphApiService.CallAuthRefresh(HttpContext.Session.GetString("refreshToken"));

                // Store accessToken and refreshToken in the session
                HttpContext.Session.SetString("accessToken", accessToken);
                HttpContext.Session.SetString("refreshToken", refreshToken);
                return await action();
            }
            throw;
        }
    }
}
