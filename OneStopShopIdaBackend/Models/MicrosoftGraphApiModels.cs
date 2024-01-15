namespace OneStopShopIdaBackend.Models;

public class Email
{
    public Body Body { get; set; }
    public string Subject { get; set; }
    public List<Recipient> ToRecipients { get; set; }
}

public class Recipient
{
    public EmailAddress EmailAddress { get; set; }
}

public class Event
{
    public string Subject { get; set; }
    public Body Body { get; set; }
    public Start Start { get; set; }
    public End End { get; set; }
    public List<Attendee> Attendees { get; set; }
}

public class Start
{
    public string DateTime { get; set; }
    public string TimeZone { get; set; }
}

public class End
{
    public string DateTime { get; set; }
    public string TimeZone { get; set; }
}

public class Attendee
{
    public EmailAddress EmailAddress { get; set; }
    public string Type { get; set; }
}

public class Body
{
    public string Content { get; set; }
    public string ContentType { get; set; }
}

public class EmailAddress
{
    public string Address { get; set; }
}