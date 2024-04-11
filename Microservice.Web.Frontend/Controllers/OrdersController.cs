using Microservice.Web.Frontend.Services.OrderServices;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Frontend.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderService orderService;
    private readonly string UserId = "1";

    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }
    public IActionResult Index()
    {
        var orders = orderService.GetOrders(UserId);
        return View(orders);
    }

    public IActionResult Detail(Guid Id)
    {
        var order = orderService.OrderDetail(Id);
        return View(order);
    }
}
