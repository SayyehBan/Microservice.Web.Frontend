using Microservice.Web.Frontend.Models.Links;
using Microservice.Web.Frontend.Services.BasketServices;
using Microservice.Web.Frontend.Services.DiscountServices;
using Microservice.Web.Frontend.Services.OrderServices;
using Microservice.Web.Frontend.Services.PaymentServices;
using Microservice.Web.Frontend.Services.ProductServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using RestSharp;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mvcService = builder.Services.AddControllersWithViews();


if (builder.Environment.IsDevelopment())
{
    mvcService.AddRazorRuntimeCompilation();
}
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IProductService>(p =>
{
    return new RProductService(new RestClient(LinkServices.ApiGatewayForWeb));
});

builder.Services.AddScoped<IBasketService>(p =>
{
    return new RBasketService(new RestClient(LinkServices.ApiGatewayForWeb));
});

builder.Services.AddScoped<IOrderService>(p =>
{
    return new ROrderService(
        new RestClient(LinkServices.ApiGatewayForWeb), new HttpContextAccessor());
});

builder.Services.AddScoped<IPaymentService>(p =>
{
    return new RPaymentService(
        new RestClient(LinkServices.ApiGatewayForWeb));
});
builder.Services.AddAuthentication(c =>
{
    c.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    c.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme).
AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = LinkServices.IdentityServer;
    options.ClientId = "webfrontendcode";
    options.ClientSecret = "123456";
    options.ResponseType = "code";
    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;
    options.Scope.Add("profile");
    options.Scope.Add("openid");
    options.Scope.Add("orderservice.getorders");
    options.Scope.Add("basket.fullaccess");
    options.Scope.Add("apigatewayforweb.fullaccess");
});
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

// اضافه کردن کد زیر به قسمت ابتدایی کد شما
ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
//builder.Services.AddScoped<IDiscountService, RDiscountService>();
builder.Services.AddScoped<IDiscountService>(p =>
{
    return new DiscountServiceRestful(new RestClient(LinkServices.ApiGatewayForWeb));
});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
