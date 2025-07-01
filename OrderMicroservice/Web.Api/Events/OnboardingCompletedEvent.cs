namespace Web.Api.Events;

public class OnboardingCompletedEvent
{
    public Guid UserId { get; init; }

    public string Name { get; init; }
}