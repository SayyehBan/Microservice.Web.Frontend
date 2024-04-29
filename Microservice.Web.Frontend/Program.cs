using Microservice.Web.Frontend.Models.Links;
using Microservice.Web.Frontend.Services.BasketServices;
using Microservice.Web.Frontend.Services.DiscountServices;
using Microservice.Web.Frontend.Services.OrderServices;
using Microservice.Web.Frontend.Services.PaymentServices;
using Microservice.Web.Frontend.Services.ProductServices;
using RestSharp;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mvcService = builder.Services.AddControllersWithViews();


if (builder.Environment.IsDevelopment())
{
    mvcService.AddRazorRuntimeCompilation();
}
builder.Services.AddScoped<IProductService>(p =>
{
    return new RProductService(new RestClient(LinkServices.ProductService));
});
builder.Services.AddScoped<IBasketService>(p =>
{
    return new RBasketService(new RestClient(LinkServices.BasketService));
});

builder.Services.AddScoped<IOrderService>(p =>
{
    return new ROrderService(
        new RestClient(LinkServices.OrderService));
}); 

builder.Services.AddScoped<IPaymentService>(p =>
{
    return new RPaymentService(
        new RestClient(LinkServices.PaymentService));
}); 
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

// اضافه کردن کد زیر به قسمت ابتدایی کد شما
ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
builder.Services.AddScoped<IDiscountService, RDiscountService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
