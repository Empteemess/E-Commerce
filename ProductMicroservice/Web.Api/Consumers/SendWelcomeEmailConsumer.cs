using MassTransit;
using Web.Api.Commands;
using Web.Api.Events;

namespace Web.Api.Consumers;

public class SendWelcomeEmailConsumer(IPublishEndpoint publish) : IConsumer<SendWelcomeEmailCommand>
{
    public async Task Consume(ConsumeContext<SendWelcomeEmailCommand> context)
    {
        Console.WriteLine($"User -> ( {context.Message.Email} ) Send (WelcomeMessage) ()() --> ProductMicroservice <--()()");

        await publish.Publish(new WelcomeEmailSentEvent
        {
            Name = context.Message.Email,
            UserId = context.Message.UserId,
            Email = context.Message.Email
        });
    }
}