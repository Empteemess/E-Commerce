namespace Web.Api.Commands;

public record SendDiscountCommand(Guid UserId, string UserName);