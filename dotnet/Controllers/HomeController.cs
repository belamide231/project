using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller {


    [Authorize]
    [Authorize(policy: KeyRequirement._policy)]
    [HttpGet("/")]
    public IActionResult Home() => Content("YOU ARE AUTHORIZED");
}