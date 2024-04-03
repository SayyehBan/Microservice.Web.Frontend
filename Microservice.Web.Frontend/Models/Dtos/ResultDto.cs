using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.Frontend.Models.Dtos;

public class ResultDto
{
    [Display(Name = "تایید")]
    public bool IsSuccess { get; set; }
    [Display(Name = "پیغام")]
    public string Message { get; set; }
}

public class ResultDto<T>
{
    [Display(Name = "تایید")]
    public bool IsSuccess { get; set; }
    [Display(Name = "پیغام")]
    public string Message { get; set; }
    public T Data { get; set; }

}
