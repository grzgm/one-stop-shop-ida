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

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("microsoft")]
    public partial class MicrosoftGraphAPIController : ControllerBase
    {
        private readonly SessionEntryContext _context;
        private readonly ILogger<MicrosoftGraphAPIController> _logger;
        private readonly HttpClient _httpClient;

        private const string MicrosoftClientId = "ff6757d9-6533-46f4-99c7-32db8a7d606d";
        private const string Tenant = "organizations";
        private const string Scopes = "offline_access user.read mail.read mail.send calendars.readwrite";
        private const string CodeVerifier = "4fe484d8f9b55c7bbf2e1eceae2d3f069f2b99735dcde507df69e8e23cc461a4";
        private const string CodeChallenge = "Dhi_pRwq9DzmaCpZynqxaaNMreGnAuZwvzX-gQrcbgQ";


        private const string RedirectUri = "http://localhost:3002/microsoft/auth/callback";
        private const string FrontendUri = "http://localhost:5173";

        public MicrosoftGraphAPIController(SessionEntryContext context, ILogger<MicrosoftGraphAPIController> logger, HttpClient httpClient)
        {
            _context = context;
            _logger = logger;
            _httpClient = httpClient;
        }
    }
}
