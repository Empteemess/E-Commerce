using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Events;

namespace Web.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class UserController(IPublishEndpoint publisher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PublishUser(UserEvent userEvent)
    {
        Console.WriteLine($"Sent {userEvent.Name} From OrderService. <-----------------------------------");

        await publisher.Publish(userEvent);

        return NoContent();
    }
}