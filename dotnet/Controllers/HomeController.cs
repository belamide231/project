using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller {


    [Authorize]
    [Authorize(Policy = "CERTIFIED_USER")]
    [HttpGet("/")]
    public IActionResult Home() => Content("YOU ARE AUTHORIZED");
}