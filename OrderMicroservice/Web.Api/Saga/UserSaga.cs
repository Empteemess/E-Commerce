using Domain.Entities;
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


    public UserSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => UserAddedEvent, e => e.CorrelateById(x => x.Message.UserId));
        Event(() => DiscountSendEvent, e => e.CorrelateById(x => x.Message.UserId));
        Event(() => WelcomeEmailSentEvent, e => e.CorrelateById(x => x.Message.UserId));

        Initially(
            When(UserAddedEvent)
                .Then(context =>
                {
                    context.Saga.Email = context.Message.Email;
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.UserName = context.Message.Name;
                })
                .TransitionTo(Welcoming)
                .Publish(context => new SendWelcomeEmailCommand(context.Saga.UserId,context.Saga.UserName ?? "Default")));

        During(Welcoming,
            When(WelcomeEmailSentEvent)
                .Then(context => { context.Saga.WelcomeEmailSend = true; })
                .TransitionTo(Discount)
                .Publish(conf => new SendDiscountCommand(conf.Saga.UserId, conf.Saga.UserName ?? "DefaultName")));

        During(Discount,
            When(DiscountSendEvent)
                .Then(context =>
                    {
                        context.Saga.DiscountSent = true;
                        context.Saga.OnboardingCompleted = true;
                    }
                )
                .TransitionTo(Onboarding)
                .Publish(context => new OnboardingCompletedEvent
                {
                    SubscriberId = context.Message.UserId,
                    Name = context.Message.Name
                })
                .Finalize());
    }
}