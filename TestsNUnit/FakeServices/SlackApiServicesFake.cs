using OneStopShopIdaBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsNUnit.FakeServices;
internal class SlackApiServicesFake : ISlackApiServices
{
    public Task<string> CallAuthCallback(string code, string state)
    {
        throw new NotImplementedException();
    }

    public string GenerateSlackAPIAuthUrl(string route)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> SendMessage(string slackAccessToken, string message, string channel)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> SetStatus(string slackAccessToken, string text = "", string emoji = "", string expiration = "0")
    {
        throw new NotImplementedException();
    }
}
