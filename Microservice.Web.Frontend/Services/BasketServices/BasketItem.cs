using SayyehBanTools.Calc;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.Frontend.Services.BasketServices;

public class BasketItem
{
    [Display(Name = "شناسه")]
    public string id { get; set; }
    [Display(Name = "شناسه")]
    public string productId { get; set; }
    [Display(Name = "نام محصول")]
    public string productName { get; set; }
    [Display(Name = "قیمت واحد")]
    public int unitPrice { get; set; }
    [Display(Name = "تعداد")]
    public int quantity { get; set; }
    [Display(Name = "تصویر")]
    public string imageUrl { get; set; }
    public int TotalPrice()
    {
        return Convert.ToInt32(Calculator.Multiply(unitPrice, quantity));
    }
}
