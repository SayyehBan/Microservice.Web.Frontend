using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.Frontend.Services.BasketServices;

public class BasketDto
{
    [Display(Name = "شناسه")]
    public string id { get; set; }
    [Display(Name = "شناسه")]
    public string userId { get; set; }
    public List<BasketItem> items { get; set; }
}
