using Microsoft.Extensions.Configuration;

namespace TestsNUnit.FakeServices;

public static class ConfigurationFake
{
    public static IConfiguration GetConfiguration()
    {
        // You can customize this method based on your testing needs
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "FrontendUri", "http://localhost:5173" }, // Replace with your test FrontendUri
                // Add other configuration settings as needed for your tests
            })
            .Build();

        return configuration;
    }
}