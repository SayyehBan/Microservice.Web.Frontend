using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.Frontend.Services.ProductServices;

public class ProductDto
{
    [Display(Name = "شناسه")]
    public string id { get; set; }
    [Display(Name = "نام")]
    public string name { get; set; }
    [Display(Name = "توضیحات")]
    public string description { get; set; }
    [Display(Name = "تصویر")]
    public string image { get; set; }
    [Display(Name = "قیمت")]
    public int price { get; set; }
    public ProductCategory productCategory { get; set; }
}
