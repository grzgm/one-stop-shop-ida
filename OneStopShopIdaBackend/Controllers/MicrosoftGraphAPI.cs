using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using OneStopShopIdaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("microsoft")]
    public class MicrosoftGraphAPIController : ControllerBase
    {
        private readonly SessionEntryContext _context;
        private readonly ILogger<MicrosoftGraphAPIController> _logger;
        private readonly HttpClient _httpClient;

        private string MY_MAIL = "";
        private string MICROSOFT_CLIENT_ID = "ff6757d9-6533-46f4-99c7-32db8a7d606d";
        private string TENANT = "organizations";
        private string SCOPES = "offline_access user.read mail.read mail.send calendars.readwrite";
        private string redirectUri = "http://localhost:3002/microsoft/auth/callback";
        private string codeVerifier = "4fe484d8f9b55c7bbf2e1eceae2d3f069f2b99735dcde507df69e8e23cc461a4";
        private string codeChallenge = "Dhi_pRwq9DzmaCpZynqxaaNMreGnAuZwvzX-gQrcbgQ";

        public MicrosoftGraphAPIController(SessionEntryContext context, ILogger<MicrosoftGraphAPIController> logger, HttpClient httpClient)
        {
            _context = context;
            _logger = logger;
            _httpClient = httpClient;
        }

        // OAuth Step 1: Redirect users to microsoft's authorization URL
        [HttpGet("auth")]
        public async Task<IActionResult> GetAuth()
        {
            var sessionId = HttpContext.Session.Id;

            HttpContext.Session.SetString("key", "value");

            string authUrl =
            $"https://login.microsoftonline.com/{TENANT}/oauth2/v2.0/authorize?" +
            $"client_id={MICROSOFT_CLIENT_ID}" +
            $"&response_type=code" +
            $"&redirect_uri={redirectUri}" +
            $"&response_mode=query" +
            $"&scope={SCOPES}" +
            $"&state={sessionId}" +
            $"&code_challenge={codeChallenge}" +
            $"&code_challenge_method=S256";
            try
            {
                //HttpResponseMessage response = await _httpClient.GetAsync(authUrl);
                //response.EnsureSuccessStatusCode();

                // Deserialize the response body if necessary
                // Example: var result = await response.Content.ReadAsAsync<MyModel>();

                // Handle the response data as needed
                // Example: return Ok(result);
                //return Ok(HttpUtility.UrlEncode(authUrl));
                return Redirect(authUrl);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // OAuth Step 2: Handle the OAuth callback
        [HttpGet("auth/callback")]
        public async Task<IActionResult> GetAuthCallback([FromQuery] string code, [FromQuery] string state)
        {
            try
            {
                var data = new Dictionary<string, string>
            {
                { "client_id", MICROSOFT_CLIENT_ID },
                { "scope", SCOPES },
                { "code", code },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" },
                { "code_verifier", codeVerifier }
            }; 
                var content = new FormUrlEncodedContent(data);
                content.Headers.Clear();
                content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                content.Headers.Add("Origin", "http://localhost");

                HttpResponseMessage response = await _httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token", content);
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);
                // Access the access_token property
                string accessToken = responseObject.access_token;
                string REFRESH_TOKEN = responseObject.refresh_token;

                SessionEntryItem sessionEntryItem = new SessionEntryItem();
                sessionEntryItem.Id = state;
                sessionEntryItem.AccessToken = accessToken;
                sessionEntryItem.RefreshToken = REFRESH_TOKEN;

                _context.SessionEntryItems.Add(sessionEntryItem);
                await _context.SaveChangesAsync();

                return Redirect("http://localhost:5173");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        // GET: api/TodoItems
        [HttpGet("/check-token")]
        public async Task<ActionResult<Boolean>> GetCheckToken()
        {
            var sessionId = HttpContext.Session.Id;

            var items = await _context.SessionEntryItems.ToListAsync();

            Boolean isToken = false;

            foreach (var item in items)
            {
                if (item.Id == sessionId)
                {
                    isToken = true;
                    break; // At least one item has an id
                }
            }

            return isToken;
        }
        // GET: api/TodoItems
        [HttpGet("/get-token")]
        public async Task<ActionResult<SessionEntryItem>> GetGetToken()
        {
            var sessionId = HttpContext.Session.Id;
            return await _context.SessionEntryItems.FindAsync(sessionId);
        }



        // GET: api/TodoItems
        [HttpGet("auth/tokens")]
        public async Task<ActionResult<IEnumerable<SessionEntryItem>>> GetSessionEntryItems()
        {
            if (_context.SessionEntryItems == null)
            {
                return NotFound();
            }
            return await _context.SessionEntryItems.ToListAsync();
        }
    }
}
