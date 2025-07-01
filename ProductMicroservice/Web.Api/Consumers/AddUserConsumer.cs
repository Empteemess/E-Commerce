using MassTransit;
using Web.Api.Commands;
using Web.Api.Events;

namespace Web.Api.Consumers;

public class AddUserConsumer(IPublishEndpoint publish) : IConsumer<AddUserCommand>
{
    public async Task Consume(ConsumeContext<AddUserCommand> context)
    {

        Console.WriteLine($"User -> ( {context.Message.UserName} ) (Added) ()() --> ProductMicroservice <--()()");


        await publish.Publish(new UserAddedEvent
        {
            UserId = Guid.NewGuid(),
            Name = context.Message.UserName,
            Age = context.Message.Age,
            Email = context.Message.Email
        });
    }
}