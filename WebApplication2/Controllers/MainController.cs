using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

public class MainController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}