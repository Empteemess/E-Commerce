using Application.Dtos;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Commands;

namespace Web.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class UserController(IPublishEndpoint publisher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
    {

        Console.WriteLine($"--------------------------------------Start--------------------------------------");
        await publisher.Publish(new AddUserCommand(userDto.Name, userDto.Age,userDto.Email));

        return Ok();
    }
}