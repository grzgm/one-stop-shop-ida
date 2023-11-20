using OneStopShopIdaBackend.Models;

namespace TestsNUnit.FakeServices;

public static class ModelsObjectsFake
{
    public static UserItem testUserItem = new()
        { MicrosoftId = "testMicrosoftId", FirstName = "testFirstName", Surname = "testSurname", Email = "testEmail" };

    public static LunchTodayItem testLunchTodayItem = new()
    {
        MicrosoftId = "testMicrosoftId",
        IsRegistered = false
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

    public static LunchRecurringRegistrationItem testLunchRecurringRegistrationItem = new()
    {
        MicrosoftId = "testMicrosoftId",
        LastRegistered = new DateTime(2000,1,1)
    };
}