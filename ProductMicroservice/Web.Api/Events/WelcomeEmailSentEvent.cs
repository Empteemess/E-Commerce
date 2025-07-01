namespace Web.Api.Events;

public class WelcomeEmailSentEvent
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}