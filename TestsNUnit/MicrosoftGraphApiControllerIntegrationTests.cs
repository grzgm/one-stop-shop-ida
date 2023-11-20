using System.Text.RegularExpressions;
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
        await Page.GotoAsync("http://localhost:3002/microsoft/auth?route=/office-details/lunch");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        // Expect a title "to contain" a substring.
        //await Expect(Page).ToHaveTitleAsync(new Regex("Sign in to your account"));

        // create a locator
        await Page.WaitForTimeoutAsync(1000);
        var emailInput = await Page.QuerySelectorAsync("input[type=email]");
        await emailInput.FillAsync("");

        var button = await Page.QuerySelectorAsync("input[type=submit]");
        await button.ClickAsync();

        // Password page
        await Page.WaitForTimeoutAsync(2000);
        var passwordInput = await Page.QuerySelectorAsync("input[type=password]");
        await passwordInput.FillAsync("");

        button = await Page.QuerySelectorAsync("input[type=submit]");
        await button.ClickAsync();

        // Microsoft Two step Auth
        //string divTextContent = await Page.TextContentAsync("#idRichContext_DisplaySign");

        await Page.WaitForTimeoutAsync(1000);

        button = await Page.QuerySelectorAsync("input[type=submit][value=Tak]");
        await button.ClickAsync();

        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(1000);
        await Expect(Page).ToHaveURLAsync(new Regex("/office-details/lunch"));
        //await Expect(Page).ToHaveTitleAsync(new Regex("Sign in to your account"));
    }
}