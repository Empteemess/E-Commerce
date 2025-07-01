using MassTransit;
using Web.Api.Commands;
using Web.Api.Events;

namespace Web.Api.Consumers;

public class SendDiscountConsumer(IPublishEndpoint publish) : IConsumer<SendDiscountCommand>
{
    public async Task Consume(ConsumeContext<SendDiscountCommand> context)
    {
        Console.WriteLine($"User -> ( {context.Message.UserName} ) Send (Discount) ()() --> ProductMicroservice <--()()");

        await publish.Publish(new DiscountSendEvent
        {
            UserId = context.Message.UserId,
            Name = context.Message.UserName
        });
    }
}