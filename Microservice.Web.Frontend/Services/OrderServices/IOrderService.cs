using Newtonsoft.Json;
using RestSharp;

namespace Microservice.Web.Frontend.Services.OrderServices;
public interface IOrderService
{
    List<OrderDto> GetOrders(string UserId);
    OrderDetailDto OrderDetail(Guid OrderId);
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
}
