using OneStopShopIdaBackend.Models;

namespace TestsNUnit.FakeServices;

public static class FakeModelsObjects
{
    public static string testAccessToken = "testAccessToken";
    public static string testRefreshToken = "testRefreshToken";

    public static UserItem testUserItem = new()
        { MicrosoftId = "testMicrosoftId", FirstName = "testFirstName", Surname = "testSurname", Email = "testEmail" };

    public static LunchRegistrationsItem TestLunchRegistrationsItem = new()
    {
        MicrosoftId = "testMicrosoftId",
        RegistrationDate = new DateTime(),
        Office = "testOffice"
    };

    public static LunchDaysItem TestLunchDaysItem = new()
    {
        MicrosoftId = "testMicrosoftId",
        Monday = false,
        Tuesday = false,
        Wednesday = false,
        Thursday = false,
        Friday = false,
    };
}