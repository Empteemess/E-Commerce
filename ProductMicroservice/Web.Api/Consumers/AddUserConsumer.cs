using Infrastructure.Data;
using MassTransit;
using Web.Api.Entities;
using Web.Api.Events;

namespace Web.Api.Consumers;

public class AddUserConsumer : IConsumer<AddUserEvent>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AddUserConsumer> _logger;

    public AddUserConsumer(AppDbContext dbContext,ILogger<AddUserConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AddUserEvent> context)
    {
        var user = new User
        {
            Name = context.Message.Name,
            Age = context.Message.Age
        };

        _logger.LogInformation($"User -> ({context.Message.Name}) with age -> ({context.Message.Age}) added");

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}