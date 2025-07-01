using MassTransit;
using Web.Api.Events;

namespace Web.Api.Consumers;

public class OnboardingCompletedConsumer : IConsumer<OnboardingCompletedEvent>
{
    public Task Consume(ConsumeContext<OnboardingCompletedEvent> context)
    {
        Console.WriteLine($"User -> ( {context.Message.Name} ) Onboarding Completed (Onboarding) ()() --> ProductMicroservice <--()()");
        return Task.CompletedTask;
    }
}