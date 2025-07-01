namespace Web.Api.Events;

public class OnboardingCompletedEvent
{
    public Guid SubscriberId { get; init; }

    public string Name { get; init; }
}