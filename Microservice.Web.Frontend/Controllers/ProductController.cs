using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Frontend.Controllers;

public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
