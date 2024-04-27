using Microservice.Web.Frontend.Models.Dtos;
using Newtonsoft.Json;
using RestSharp;

namespace Microservice.Web.Frontend.Services.PaymentServices;

public interface IPaymentService
{
    ResultDto<ReturnPaymentLinkDto> GetPaymentlink(Guid OrderId,
        string CallbackUrl);

}
public class RPaymentService : IPaymentService
{
    private readonly RestClient restClient;
    public RPaymentService(RestClient restClient)
    {
        this.restClient = restClient;
    }

    public ResultDto<ReturnPaymentLinkDto> GetPaymentlink(Guid OrderId, string CallbackUrl)
    {
        var request = new RestRequest($"/api/Pay?OrderId={OrderId}&callbackUrlFront={CallbackUrl}", Method.Get);
        var response = restClient.Execute(request);
        var orders = JsonConvert.DeserializeObject<ResultDto<ReturnPaymentLinkDto>>(response.Content);
        return orders;
    }
}

public class ReturnPaymentLinkDto
{
    public string PaymentLink { get; set; }
}