using MassTransit;
using Web.Api.Events;

namespace Web.Api.Consumers;

public class UserConsumers : IConsumer<UserEvent>
{
    public Task Consume(ConsumeContext<UserEvent> context)
    {
        Console.WriteLine($"Message with Name -> {context.Message.Name} sent From OrderMicroservice To ProductMicroservice");
        return Task.CompletedTask;
    }
}