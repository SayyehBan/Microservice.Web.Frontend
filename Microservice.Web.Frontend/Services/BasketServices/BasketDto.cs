using SayyehBanTools.Calc;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.Frontend.Services.BasketServices;

public class BasketDto
{
    [Display(Name = "شناسه")]
    public string id { get; set; }
    [Display(Name = "شناسه")]
    public string userId { get; set; }
    public List<BasketItem> items { get; set; }
    public int TotalPrice()
    {
        return items.Sum(p => Convert.ToInt32(Calculator.Multiply(p.unitPrice, p.quantity)));
    }
}
