using Microservice.Web.Frontend.Services.BasketServices;
using Microservice.Web.Frontend.Services.ProductServices;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;
builder.Services.AddScoped<IProductService>(p =>
{
    return new RProductService(new RestClient(builder.Configuration["MicroservicAddress:Product:uri"]));
});
builder.Services.AddScoped<IBasketService>(p =>
{
    return new RBasketService(new RestClient(builder.Configuration["MicroservicAddress:Basket:Uri"]));
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
