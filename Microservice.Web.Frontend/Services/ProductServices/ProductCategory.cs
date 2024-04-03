using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.Frontend.Services.ProductServices;

public class ProductCategory
{
    [Display(Name ="شناسه")]
    public string CategoryId;
    [Display(Name = "عنوان")]
    public string Category;
}
