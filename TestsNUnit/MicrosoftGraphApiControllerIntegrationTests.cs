using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Org.BouncyCastle.Utilities.Collections;

namespace TestsNUnit;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class MicrosoftGraphApiControllerIntegrationTests : PageTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task MicrosoftAuthTest()
    {
        // Go to Home Page
        await Page.GotoAsync("http://localhost:5173");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        // Expect a title "to contain" a substring.
        //await Expect(Page).ToHaveTitleAsync(new Regex("Sign in to your account"));

        // Navigate to Reserve Desk Page
        await Page.GotoAsync("http://localhost:5173/microsoft-auth?previousLocation=/office-details/reserve-desk");

        // Click Button to register
        var newPage = await Context.RunAndWaitForPageAsync(async () =>
        {
            await Page.ClickAsync(".button");
        });
        await newPage.WaitForLoadStateAsync();
        
        // create a locator
        await newPage.WaitForTimeoutAsync(2000);
        var emailInput = await newPage.QuerySelectorAsync("input[type=email]");
        await emailInput.FillAsync("");

        var button = await newPage.QuerySelectorAsync("input[type=submit]");
        await button.ClickAsync();

        // Password page
        await newPage.WaitForTimeoutAsync(2000);
        var passwordInput = await newPage.QuerySelectorAsync("input[type=password]");
        await passwordInput.FillAsync("");

        button = await newPage.QuerySelectorAsync("input[type=submit]");
        await button.ClickAsync();

        // Microsoft Two step Auth
        //string divTextContent = await Page.TextContentAsync("#idRichContext_DisplaySign");

        await newPage.WaitForTimeoutAsync(25000);

        //button = await newPage.QuerySelectorAsync("input[type=submit][value=Tak]");
        //await button.ClickAsync();

        await Page.WaitForTimeoutAsync(7000);

        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Expect(Page).ToHaveURLAsync(new Regex("\\/office-details\\/reserve-desk"));
        //await Expect(Page).ToHaveTitleAsync(new Regex("Sign in to your account"));
    }
}