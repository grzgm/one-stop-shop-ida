using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace OneStopShopIdaBackend.Controllers
{
    public partial class SlackApiController : ControllerBase
    {
        [Authorize]
        [HttpPost("send-message")]
        public async Task<IActionResult> PostSendMessage([FromQuery] string message, [FromQuery] string channel)
        {
            
            using (HttpResponseMessage response = await _slackApiServices.SendMessage(_memoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}SlackAccessToken") ?? string.Empty, message, channel))
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [Authorize]
        [HttpPut("set-status")]
        public async Task<IActionResult> PutSetStatus([FromQuery] string text = "", [FromQuery] string emoji = "", [FromQuery] string expiration = "0")
        {
            using (HttpResponseMessage response = await _slackApiServices.SetStatus(_memoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}SlackAccessToken") ?? string.Empty, text, emoji, expiration))
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}