using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller {


    public static string _directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/browser");


    [HttpGet("{*path}")]
    public IActionResult Render(string path) {

        string fullPath = Path.Combine(_directory, path);

        if (System.IO.File.Exists(fullPath)) {

            return PhysicalFile(fullPath, "text/html");
        }

        return PhysicalFile(Path.Combine(_directory, "index.html"), "text/html");
    }

}