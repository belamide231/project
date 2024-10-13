using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {


    private readonly UserServices _services;
    public UserController(UserServices services) => _services = services;


    // NEEDS AN IMPROVEMENT
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyAsync([FromBody] VerifyDTO DTO) {

        var result = await _services.F_VerifyAsync(DTO);
        return StatusCode(result.Status, result);
    }


    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO DTO) {

        var result = await _services.F_RegisterAsync(DTO);
        return StatusCode(result.Status, result);
    }    


    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO DTO) {

        var result = await _services.F_LoginAsync(DTO);
        return StatusCode(result.Status, result);
    }
}