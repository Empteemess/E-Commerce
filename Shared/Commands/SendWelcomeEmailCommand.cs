namespace Web.Api.Commands;

public record SendWelcomeEmailCommand(Guid UserId,string Email);