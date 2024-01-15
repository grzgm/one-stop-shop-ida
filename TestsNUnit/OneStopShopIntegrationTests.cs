using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace TestsNUnit;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
internal class OneStopShopIntegrationTests : PageTest
{
    private const string p = "zaq1@#$ES";
    private const string e = "malisgr@cronos.be";

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

        // Click Button to register
        var microsoftAuthPage = await Context.RunAndWaitForPageAsync(async () =>
        {
            await Page.ClickAsync(".button");
        });
        await microsoftAuthPage.WaitForLoadStateAsync();

        // create a locator
        await microsoftAuthPage.WaitForTimeoutAsync(2000);
        var emailInput = await microsoftAuthPage.QuerySelectorAsync("input[type=email]");
        await emailInput.FillAsync(e);

        var button = await microsoftAuthPage.QuerySelectorAsync("input[type=submit]");
        await button.ClickAsync();

        // Password page
        await microsoftAuthPage.WaitForTimeoutAsync(2000);
        var passwordInput = await microsoftAuthPage.QuerySelectorAsync("input[type=password]");
        await passwordInput.FillAsync(p);

        button = await microsoftAuthPage.QuerySelectorAsync("input[type=submit]");
        await microsoftAuthPage.WaitForTimeoutAsync(1000);
        await button.ClickAsync();
        await microsoftAuthPage.WaitForTimeoutAsync(1000);

        // Microsoft Two step Auth
        //string divTextContent = await Page.TextContentAsync("#idRichContext_DisplaySign");

        await microsoftAuthPage.WaitForTimeoutAsync(15000);

        // if asks to show message everytime
        //button = await newPage.QuerySelectorAsync("input[type=submit][value=Tak]");
        //await button.ClickAsync();

        // Navigate to Reserve Desk Page
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Expect(Page).ToHaveURLAsync("http://localhost:5173/");
    }

    [Test]
    public async Task ReserveDeskPageTest()
    {
        // Go to Home Page
        await Page.GotoAsync("http://localhost:5173");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        // Expect a title "to contain" a substring.
        //await Expect(Page).ToHaveTitleAsync(new Regex("Sign in to your account"));

        // Click Button to register
        var microsoftAuthPage = await Context.RunAndWaitForPageAsync(async () =>
        {
            await Page.ClickAsync(".button");
        });
        await microsoftAuthPage.WaitForLoadStateAsync();

        // create a locator
        await microsoftAuthPage.WaitForTimeoutAsync(2000);
        var emailInput = await microsoftAuthPage.QuerySelectorAsync("input[type=email]");
        await emailInput.FillAsync(e);

        var button = await microsoftAuthPage.QuerySelectorAsync("input[type=submit]");
        await button.ClickAsync();

        // Password page
        await microsoftAuthPage.WaitForTimeoutAsync(2000);
        var passwordInput = await microsoftAuthPage.QuerySelectorAsync("input[type=password]");
        await passwordInput.FillAsync(p);

        button = await microsoftAuthPage.QuerySelectorAsync("input[type=submit]");
        await microsoftAuthPage.WaitForTimeoutAsync(1000);
        await button.ClickAsync();
        await microsoftAuthPage.WaitForTimeoutAsync(1000);

        // Microsoft Two step Auth
        //string divTextContent = await Page.TextContentAsync("#idRichContext_DisplaySign");

        await microsoftAuthPage.WaitForTimeoutAsync(15000);

        // if asks to show message everytime
        //button = await newPage.QuerySelectorAsync("input[type=submit][value=Tak]");
        //await button.ClickAsync();

        // Navigate to Reserve Desk Page
        await Page.GotoAsync("http://localhost:5173/office-details/reserve-desk");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Expect(Page).ToHaveURLAsync("http://localhost:5173/office-details/reserve-desk");
        await Page.WaitForTimeoutAsync(1000);
        // Find the div with class "desk-cluster" and id "0"
        await Page.Locator("#dc0 > #d3").ClickAsync();
        //
        await Page.Locator("input[type=checkbox]").First.ClickAsync();
        // 
        await Page.Locator("button[type=button]").ClickAsync();
        await Page.WaitForTimeoutAsync(1000);
        // Wait for an alert with class "alert" and text "Successfully Reserved."
        var successAlertElement = await Page.QuerySelectorAsync(".alert:has-text('Successfully Reserved.')");

        // Wait for an alert with class "alert" and text "You have too many desk Reservations."
        var errorAlertElement = await Page.QuerySelectorAsync(".alert:has-text('You have too many desk Reservations.')");
        Assert.IsNotNull(successAlertElement ?? errorAlertElement, "Alert element not found on the page.");
    }
}