using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


// TEMPORARY ONLY FOR TESTING API

public class UserController : ControllerBase {


    private readonly UserServices _services;
    public UserController(UserServices services) => _services = services;


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