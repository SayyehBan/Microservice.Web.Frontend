using Microservice.Web.Frontend.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Frontend.Controllers;

public class ProductController : Controller
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }
    public IActionResult Index()
    {
        var products = productService.GetAllProduct();
        return View(products);
    }
    public IActionResult Details(Guid Id)
    {
        var product = productService.GetProduct(Id);
        return View(product);
    }
    
}
