using System.Text.Json;
using Domain.Entities;
using Domain.IRepository;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Api.Commands;
using Web.Api.Events;

namespace Application.BackgroundServices;

public class OutboxMessageBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _timeSpan;

    public OutboxMessageBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _timeSpan = TimeSpan.FromSeconds(5);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PublishEvents(stoppingToken);
            await Task.Delay(_timeSpan, stoppingToken);
        }
    }

    private async Task PublishEvents(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var unprocessedOutboxMessages = await unitOfWork.OutboxMessageRepository.GetUnprocessedAsync(cancellationToken: cancellationToken) ?? [];

        foreach (var messages in unprocessedOutboxMessages)
        {
            await PublishMessage(messages, publishEndpoint, cancellationToken);
            messages.IsProcessed = true;
            messages.ProcessedAt = DateTime.UtcNow;
        }

        await unitOfWork.SaveChangesAsync();
    }

    private async Task PublishMessage(OutboxMessage message, IPublishEndpoint publishEndpoint,
        CancellationToken cancellationToken)
    {
        switch (message.EventType)
        {
            case nameof(SendDiscountCommand):
                var paymentCommand = JsonSerializer.Deserialize<SendDiscountCommand>(message.Data);
                await publishEndpoint.Publish(paymentCommand, cancellationToken);
                break;

            case nameof(SendWelcomeEmailCommand):
                var inventoryCommand = JsonSerializer.Deserialize<SendWelcomeEmailCommand>(message.Data);
                await publishEndpoint.Publish(inventoryCommand, cancellationToken);
                break;

            case nameof(OnboardingCompletedEvent):
                var iOnboardingCompletedCommand = JsonSerializer.Deserialize<OnboardingCompletedEvent>(message.Data);
                await publishEndpoint.Publish(iOnboardingCompletedCommand, cancellationToken);
                break;

            default:
                throw new InvalidOperationException($"Unknown message type: {message.EventType}");
        }
    }
}