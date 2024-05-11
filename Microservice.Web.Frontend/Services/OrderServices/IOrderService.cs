using IdentityModel.Client;
using Microservice.Web.Frontend.Models.Dtos;
using Microservice.Web.Frontend.Models.Links;
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
    private string _accessToken = null;
    public ROrderService(RestClient restClient)
    {
        this.restClient = restClient;
    }
    private async Task<string> GetAccsessToken()
    {
        if (!string.IsNullOrWhiteSpace(_accessToken))
        {
            return _accessToken;
        }
        else
        {
            HttpClient client = new HttpClient();
            var discovery = await client.GetDiscoveryDocumentAsync(LinkServices.IdentityServer);
            var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = "webfrontend",
                ClientSecret = "123456",
                GrantType = "client_credentials",
                Scope = "orderservice.fullaccsess"
            });
            if (token.IsError)
            {
                throw new Exception(token.Error);
            }
            _accessToken = token.AccessToken;
            return _accessToken;
        }
    }

    public List<OrderDto> GetOrders(string UserId)
    {
        var request = new RestRequest("/api/Order", Method.Get);
        var token = GetAccsessToken().Result;
        request.AddHeader("Authorization", $"Bearer {token}");
        var response = restClient.Execute(request);
        var orders = JsonConvert.DeserializeObject<List<OrderDto>>(response.Content);
        return orders;
    }

    public OrderDetailDto OrderDetail(Guid OrderId)
    {
        var request = new RestRequest($"/api/Order/{OrderId}", Method.Get);
        var token = GetAccsessToken().Result;
        request.AddHeader("Authorization", $"Bearer {token}");
        var response = restClient.Execute(request);
        var orderdetail = JsonConvert.DeserializeObject<OrderDetailDto>(response.Content);
        return orderdetail;
    }

    public ResultDto RequestPayment(Guid OrderId)
    {
        var request = new RestRequest($"/api/OrderPayment?OrderId={OrderId}", Method.Post);
        var token = GetAccsessToken().Result;
        request.AddHeader("Authorization", $"Bearer {token}");
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
