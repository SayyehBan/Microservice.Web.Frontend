using Microservice.Web.Frontend.Models.Dtos;
using RestSharp;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microservice.Web.Frontend.Helper;

namespace Microservice.Web.Frontend.Services.BasketServices;

public interface IBasketService
{
    BasketDto GetBasket(string UserId);
    ResultDto AddToBasket(AddToBasketDto addToBasket, string UserId);
    ResultDto DeleteFromBasket(Guid Id);
    ResultDto UpdateQuantity(Guid BasketItemId, int quantity);
    ResultDto ApplyDiscountToBasket(Guid basketId, Guid discountId); ResultDto Checkout(CheckoutDto checkout);
}
public class RBasketService : IBasketService
{
    private readonly RestClient restClient;
    public RBasketService(RestClient restClient)
    {
        this.restClient = restClient;
    }

    public ResultDto AddToBasket(AddToBasketDto addToBasket, string UserId)
    {
        var request = new RestRequest($"api/Basket?UserId={UserId}", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        string serializeModel = JsonSerializer.Serialize(addToBasket);
        request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
        var response = restClient.Execute(request);
        return ResultAPI.GetResponseStatusCode(response);
    }

    public ResultDto ApplyDiscountToBasket(Guid basketId, Guid discountId)
    {
        var request = new RestRequest($"api/Basket/{basketId}/{discountId}",Method.Put);
        var response= restClient.Execute(request);
        return ResultAPI.GetResponseStatusCode(response);
    }

    public ResultDto Checkout(CheckoutDto checkout)
    {
        var request = new RestRequest($"api/Basket/CheckoutBasket", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        string serializeModel = JsonSerializer.Serialize(checkout);
        request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
        var response = restClient.Execute(request);
        return ResultAPI.GetResponseStatusCode(response);

    }

    public ResultDto DeleteFromBasket(Guid Id)
    {
        var request = new RestRequest($"api/Basket?ItemId={Id}", Method.Delete);
        var response = restClient.Execute(request);
        return ResultAPI.GetResponseStatusCode(response);
    }

    public BasketDto GetBasket(string UserId)
    {
        var request = new RestRequest($"api/Basket?UserId={UserId}", Method.Get);
        var response = restClient.Execute(request);
        var basket = JsonSerializer.Deserialize<BasketDto>(response.Content);
        return basket;
    }

    public ResultDto UpdateQuantity(Guid BasketItemId, int quantity)
    {
        var request = new RestRequest($"api/Basket?basketItemId={BasketItemId}&quantity={quantity}", Method.Put);
        var response = restClient.Execute(request);
        return ResultAPI.GetResponseStatusCode(response);
    }
}
public class AddToBasketDto
{
    [Display(Name = "شناسه")]
    public string BasketId { get; set; }
    [Display(Name = "شناسه")]
    public string ProductId { get; set; }
    [Display(Name = "نام مصحول")]
    public string ProductName { get; set; }
    [Display(Name = "قیمت واحد")]
    public int UnitPrice { get; set; }
    [Display(Name = "تعداد")]
    public int Quantity { get; set; }
    [Display(Name = "تصویر")]
    public string ImageUrl { get; set; }
}