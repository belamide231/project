using Microsoft.AspNetCore.Authorization;
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
        var result = await _services.SendAsync(DTO, User.FindFirst(f => f.Type == JwtHelper._userId)!.Value);
        return StatusCode(result.Status, result);
    }
}
