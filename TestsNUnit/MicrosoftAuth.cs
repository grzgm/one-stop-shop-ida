namespace TestsNUnit;

using System.Configuration;
using Microsoft.Extensions.Logging;
using OneStopShopIdaBackend.Controllers;
using OneStopShopIdaBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using TestsNUnit.FakeServices;
using System.Text;

public class MicrosoftAuth
{
    public MicrosoftAuth()
    {
        // Create Microsoft Graph Api Service
        MicrosoftGraphApiService microsoftGraphApiServiceFake =
            new(new Logger<MicrosoftGraphApiService>(new LoggerFactory()),
                FakeConfiguration.GetConfiguration(),
                new HttpClient(),
                new CodeChallengeGeneratorService());
    }
}