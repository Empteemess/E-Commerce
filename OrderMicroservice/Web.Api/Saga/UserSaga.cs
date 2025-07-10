using Domain.Entities;
using Domain.IRepository;
using Infrastructure.Repository;
using MassTransit;
using Web.Api.Commands;
using Web.Api.Events;

namespace Web.Api.Saga;

public class UserSaga : MassTransitStateMachine<UserSagaData>
{
    public State Discount { get; set; }
    public State Welcoming { get; set; }
    public State Onboarding { get; set; }


    public Event<UserAddedEvent> UserAddedEvent { get; set; }
    public Event<DiscountSendEvent> DiscountSendEvent { get; set; }
    public Event<WelcomeEmailSentEvent> WelcomeEmailSentEvent { get; set; }


    public UserSaga(IServiceProvider serviceProvider)
    {
        InstanceState(x => x.CurrentState);

        Event(() => UserAddedEvent, e => e.CorrelateById(x => x.Message.UserId));
        Event(() => DiscountSendEvent, e => e.CorrelateById(x => x.Message.UserId));
        Event(() => WelcomeEmailSentEvent, e => e.CorrelateById(x => x.Message.UserId));

        var scope = serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        Initially(
            When(UserAddedEvent)
                .ThenAsync(async context =>
                {
                    context.Saga.Email = context.Message.Email;
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.UserName = context.Message.Name;

                    var command = new SendWelcomeEmailCommand(context.Saga.UserId, context.Saga.UserName ?? "Default");

                    var outboxMessage = unitOfWork.OutboxMessageRepository.MapToOutboxMessage(command);

                    await unitOfWork.OutboxMessageRepository.AddAsync(outboxMessage);
                })
                .TransitionTo(Welcoming));

        During(Welcoming,
            When(WelcomeEmailSentEvent)
                .ThenAsync(async context =>
                {
                    context.Saga.WelcomeEmailSend = true;

                    var command = new SendDiscountCommand(context.Saga.UserId, context.Saga.UserName ?? "DefaultName");

                    var outboxMessage = unitOfWork.OutboxMessageRepository.MapToOutboxMessage(command);

                    await unitOfWork.OutboxMessageRepository.AddAsync(outboxMessage);
                })
                .TransitionTo(Discount));

        During(Discount,
            When(DiscountSendEvent)
                .Then(context =>
                {
                    context.Saga.DiscountSent = true;
                    context.Saga.OnboardingCompleted = true;
                })
                .TransitionTo(Onboarding)
                .ThenAsync(async context =>
                {
                    var command = new OnboardingCompletedEvent
                    {
                        SubscriberId = context.Message.UserId,
                        Name = context.Message.Name
                    };

                    var outboxMessage = unitOfWork.OutboxMessageRepository.MapToOutboxMessage(command);

                    await unitOfWork.OutboxMessageRepository.AddAsync(outboxMessage);
                })
                .Finalize());
    }
}