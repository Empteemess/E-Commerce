namespace Web.Api.Commands;

public record AddUserCommand(string UserName, int Age,string Email);