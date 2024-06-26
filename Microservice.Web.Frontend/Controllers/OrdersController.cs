﻿using Microservice.Web.Frontend.Services.OrderServices;
using Microservice.Web.Frontend.Services.PaymentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Frontend.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderService orderService;
    private readonly IPaymentService paymentService;
    private readonly string UserId = "1";

    public OrdersController(IOrderService orderService, IPaymentService paymentService)
    {
        this.orderService = orderService;
        this.paymentService = paymentService;
    }
    public async Task<IActionResult> Index()
    {
        var orders = await orderService.GetOrders(UserId);
        return View(orders);
    }

    public async Task<IActionResult> Detail(Guid Id)
    {
        var order = orderService.OrderDetail(Id);
        return View(order);
    }
    public async Task<IActionResult> Pay(Guid OrderId)
    {
        var order =await  orderService.OrderDetail(OrderId);

        if (order.PaymentStatus == PaymentStatus.isPaid)
        {
            return RedirectToAction(nameof(Detail), new { Id = OrderId });
        }
        if (order.PaymentStatus == PaymentStatus.unPaid)
        {
            //ارسال درخواست پرداخت برای سرویس سفارش
            var paymentRequest = orderService.RequestPayment(OrderId);
        }

        // دریافت لینک پرداخت از سرویس پرداخت
        string callbackUrl = Url.Action(nameof(Detail), "Orders",
            new { Id = OrderId }, protocol: Request.Scheme);
        var paymentlink = paymentService.GetPaymentlink(order.Id, callbackUrl);

        if (paymentlink.IsSuccess)
        {
            return Redirect(paymentlink.Data.PaymentLink);
        }
        else
        {
            return NotFound();
        }
    }
}
