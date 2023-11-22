using Microsoft.AspNetCore.Mvc;

namespace OneStopShopIdaBackend.Controllers
{
    public partial class SlackApiController : ControllerBase
    {
        // OAuth Step 1: Redirect users to Slack's authorization URL
        [HttpPost("send-message")]
        public async Task<IActionResult> PostSendMessage([FromQuery] string message, [FromQuery] string channel)
        {
            try
            {
                using (HttpResponseMessage response = await _slackApiServices.SendMessage(HttpContext.Session.GetString("slackAccessToken"), message, channel))
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
                return StatusCode(500, $"Internal Server Error \n {ex.Message}");
            }
        }

        // OAuth Step 2: Handle the OAuth callback
        [HttpPut("set-status")]
        public async Task<IActionResult> PutSetStatus([FromQuery] string text = "", [FromQuery] string emoji = "", [FromQuery] string expiration = "0")
        {
            try
            {
                using (HttpResponseMessage response = await _slackApiServices.SetStatus(HttpContext.Session.GetString("slackAccessToken"), text, emoji, expiration))
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
                return StatusCode(500, $"Internal Server Error \n {ex.Message}");
            }
        }
    }
}