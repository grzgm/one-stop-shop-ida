namespace OneStopShopIdaBackend.Models
{
    public class SessionEntryItem
    {
        public long Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
