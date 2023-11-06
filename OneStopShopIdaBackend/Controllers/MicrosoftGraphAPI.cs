using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using OneStopShopIdaBackend.Models;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("microsoft")]
    public partial class MicrosoftGraphAPIController : ControllerBase
    {
        private readonly ILogger<MicrosoftGraphAPIController> _logger;
        private readonly HttpClient _httpClient;
        private readonly CodeChallengeGeneratorService _codeChallengeGeneratorService;
        private readonly UserItemsController _userItemsController;

        private const string MicrosoftClientId = "ff6757d9-6533-46f4-99c7-32db8a7d606d";
        private const string Tenant = "organizations";
        private const string Scopes = "offline_access user.read mail.read mail.send calendars.readwrite";

        private const string RedirectUri = "http://localhost:3002/microsoft/auth/callback";
        private const string FrontendUri = "http://localhost:5173";

        public MicrosoftGraphAPIController(ILogger<MicrosoftGraphAPIController> logger, HttpClient httpClient, CodeChallengeGeneratorService codeChallengeGeneratorService, UserItemsController userItemsController)
        {
            _logger = logger;
            _httpClient = httpClient;
            _codeChallengeGeneratorService = codeChallengeGeneratorService;
            _userItemsController = userItemsController;
        }
    }
}
