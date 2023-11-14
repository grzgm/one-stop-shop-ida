using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OneStopShopIdaBackend.Models;

public class PushSubscription
{
    public PushSubscription()
    {
    }

    public PushSubscription(string microsoftId, WebPush.PushSubscription subscription)
    {
        MicrosoftId = microsoftId;
        Endpoint = subscription.Endpoint;
        ExpirationTime = null;
        P256Dh = subscription.P256DH;
        Auth = subscription.Auth;
    }

    [Required]
    [ForeignKey(nameof(UserItem))]
    public string MicrosoftId { get; set; }

    [Required] public string Endpoint { get; set; }

    public double? ExpirationTime { get; set; }

    [Required] [Key] public string P256Dh { get; set; }

    [Required] public string Auth { get; set; }

    public WebPush.PushSubscription ToWebPushSubscription()
    {
        return new WebPush.PushSubscription(Endpoint, P256Dh, Auth);
    }
}