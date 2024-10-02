using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase {

    private readonly MessageServices _services;
    public MessageController(MessageServices service) => _services = service;


    [Authorize]
    [Authorize(policy: KeyRequirement._policy)]
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendDTO DTO) {
        Console.WriteLine(User.FindFirst(f => f.Type == JwtHelper._userId)?.Value);
        var result = await _services.SendAsync(DTO);
        return StatusCode(result.Status, result);
    }
}
