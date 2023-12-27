using OneStopShopIdaBackend.Models;

namespace TestsNUnit.FakeServices;

public static class ModelsObjectsFake
{
    public static string testAccessToken = "testAccessToken";
    public static string testRefreshToken = "testRefreshToken";

    public static UserItem testUserItem = new()
        { MicrosoftId = "testMicrosoftId", FirstName = "testFirstName", Surname = "testSurname", Email = "testEmail" };

    public static LunchTodayItem testLunchTodayItem = new()
    {
        MicrosoftId = "testMicrosoftId",
        RegistrationDate = new DateTime(),
        Office = "testOffice"
    };

    public static LunchRecurringItem testLunchRecurringItem = new()
    {
        MicrosoftId = "testMicrosoftId",
        Monday = false,
        Tuesday = false,
        Wednesday = false,
        Thursday = false,
        Friday = false,
    };
}