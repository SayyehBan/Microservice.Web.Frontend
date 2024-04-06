using SayyehBanTools.Calc;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.Frontend.Services.BasketServices;

public class BasketDto
{
    [Display(Name = "شناسه")]
    public string id { get; set; }
    [Display(Name = "شناسه")]
    public string userId { get; set; }
    [Display(Name = "شناسه")]
    public Guid? discountId { get; set; }
    public DiscountInBasketDto DiscountDetail { get; set; } = null;
    public List<BasketItem> items { get; set; }
    public int TotalPrice()
    {
        int result = items.Sum(p => Convert.ToInt32(Calculator.Multiply(p.unitPrice, p.quantity)));
        if (discountId.HasValue)
            result = Convert.ToInt32(Calculator.Subtract(result, DiscountDetail.Amount));
        return result;
    }
}
public class DiscountInBasketDto
{
    [Display(Name = "مبلغ")]
    public int Amount { get; set; }
    [Display(Name = "کد تخفیف")]
    public string DiscountCode { get; set; }
}