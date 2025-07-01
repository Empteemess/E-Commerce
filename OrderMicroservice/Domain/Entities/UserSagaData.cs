using MassTransit;

namespace Domain.Entities;

public class UserSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public Guid UserId { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }

    public bool DiscountSent { get; set; }
    public bool UserAdded { get; set; }
    public bool WelcomeEmailSend { get; set; }
    public bool OnboardingCompleted { get; set; }
}