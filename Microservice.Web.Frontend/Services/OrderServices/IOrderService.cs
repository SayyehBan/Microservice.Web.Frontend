using Microservice.Web.Frontend.Models.Dtos;
using Newtonsoft.Json;
using RestSharp;

namespace Microservice.Web.Frontend.Services.OrderServices;
public interface IOrderService
{
    List<OrderDto> GetOrders(string UserId);
    OrderDetailDto OrderDetail(Guid OrderId);
    ResultDto RequestPayment(Guid OrderId);
}

public class ROrderService : IOrderService
{
    private readonly RestClient restClient;
    public ROrderService(RestClient restClient)
    {
        this.restClient = restClient;
    }


    public List<OrderDto> GetOrders(string UserId)
    {
        var request = new RestRequest("/api/Order", Method.Get);
        var response = restClient.Execute(request);
        var orders = JsonConvert.DeserializeObject<List<OrderDto>>(response.Content);
        return orders;
    }

    public OrderDetailDto OrderDetail(Guid OrderId)
    {
        var request = new RestRequest($"/api/Order/{OrderId}", Method.Get);
        var response = restClient.Execute(request);
        var orderdetail = JsonConvert.DeserializeObject<OrderDetailDto>(response.Content);
        return orderdetail;
    }

    public ResultDto RequestPayment(Guid OrderId)
    {
        var request = new RestRequest($"/api/OrderPayment?OrderId={OrderId}", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        var response = restClient.Execute(request);
        return GetResponseStatusCode(response);
    }

    private static ResultDto GetResponseStatusCode(RestResponse response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return new ResultDto
            {
                IsSuccess = true,
            };
        }
        else
        {
            return new ResultDto
            {
                IsSuccess = false,
                Message = response.ErrorMessage
            };
        }
    }
}
