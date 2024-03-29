﻿// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Caching.Memory;
//
// namespace OneStopShopIdaBackend.Controllers;
//
// public partial class MicrosoftGraphApiController
// {
//      [HttpPost("resources/send-email")]
//      public async Task<IActionResult> PostSendEmail([FromQuery] string address, [FromQuery] string subject,
//          [FromQuery] string message)
//      {
//          string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
//     
//          HttpResponseMessage response = await ExecuteWithRetryMicrosoftGraphApi(
//              async (accessToken) => await
//                  _microsoftGraphApiService.SendEmail(accessToken, address, subject, message), accessToken);
//          return StatusCode((int)response.StatusCode);
//      }
//     
//      [HttpPost("resources/create-event")]
//      public async Task<IActionResult> PostCreateEvent([FromQuery] string address, [FromQuery] string title,
//          [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string description)
//      {
//          string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
//     
//          HttpResponseMessage response = await ExecuteWithRetryMicrosoftGraphApi(async (accessToken) =>
//              await _microsoftGraphApiService.CreateEvent(accessToken, address, title,
//                  startDate, endDate, description), accessToken);
//     
//          return StatusCode((int)response.StatusCode);
//      }
//
//     [HttpGet("resources/me")]
//     public async Task<UserItem> GetMe(string accessToken)
//     {
//             UserItem user = await _microsoftGraphApiService.GetMe(accessToken);
//     
//             return user;
//     }
// }