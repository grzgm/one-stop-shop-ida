namespace OneStopShopIdaBackend.Models
{
    public class Email
    {
        public Body Body { get; set; }
        public string Subject { get; set; }
        public List<Recipient> ToRecipients { get; set; }
    }
    public class Body
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
    }
    public class Recipient
    {
        public EmailAddress EmailAddress { get; set; }
    }
    public class EmailAddress
    {
        public string Address { get; set; }
    }
}
