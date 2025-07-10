namespace Web.Api.Events;

public class UserAddedEvent
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}