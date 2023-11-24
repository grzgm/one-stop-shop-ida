using Microsoft.AspNetCore.Mvc;

namespace OneStopShopIdaBackend.Controllers
{
    public partial class SlackApiController : ControllerBase
    {
        // OAuth Step 1: Redirect users to Slack's authorization URL
        [HttpPost("send-message")]
        public async Task<IActionResult> PostSendMessage([FromQuery] string message, [FromQuery] string channel)
        {
            using (HttpResponseMessage response = await _slackApiServices.SendMessage(HttpContext.Session.GetString("slackAccessToken"), message, channel))
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        // OAuth Step 2: Handle the OAuth callback
        [HttpPut("set-status")]
        public async Task<IActionResult> PutSetStatus([FromQuery] string text = "", [FromQuery] string emoji = "", [FromQuery] string expiration = "0")
        {
            using (HttpResponseMessage response = await _slackApiServices.SetStatus(HttpContext.Session.GetString("slackAccessToken"), text, emoji, expiration))
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}