using IdentityModel.Client;
using Microservice.Web.Frontend.Models.Dtos;
using Microservice.Web.Frontend.Models.Links;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using NuGet.Common;
using RestSharp;
using System.Text;

namespace Microservice.Web.Frontend.Services.OrderServices;
public interface IOrderService
{
    Task<List<OrderDto>> GetOrders(string UserId);
    Task<OrderDetailDto> OrderDetail(Guid OrderId);
    Task<ResultDto> RequestPayment(Guid OrderId);
}

public class ROrderService : IOrderService
{
    private readonly HttpClient httpClint;
    private string _accessToken = null;
    public ROrderService(HttpClient httpClint)
    {
        this.httpClint = httpClint;
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
                //GrantType = "client_credentials",
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

    public async Task<List<OrderDto>> GetOrders(string UserId)
    {
        var response = await httpClint.GetAsync(string.Format("/api/Order"));
        var json = await response.Content.ReadAsStringAsync();

        var orders = JsonConvert.DeserializeObject<List<OrderDto>>(json);
        return orders;
    }

    public async Task<OrderDetailDto> OrderDetail(Guid OrderId)
    {

        var response = await httpClint.GetAsync(string.Format("/api/Order/{0}", OrderId));
        var json = await response.Content.ReadAsStringAsync();
        var orderdetail = JsonConvert.DeserializeObject<OrderDetailDto>(json);
        return orderdetail;

        //var response =  httpClint.GetAsync(string.Format("/api/Order"));
        ////var request = new RestRequest($"/api/Order/{OrderId}", Method.Get);

        ////var response = httpClint.Execute(request);
        //var orderdetail = JsonConvert.DeserializeObject<OrderDetailDto>(response.Content);
        //return orderdetail;
    }
    public async Task<ResultDto> RequestPayment(Guid OrderId)
    {
        var url = $"/api/OrderPayment?OrderId={OrderId}";
        var content = new StringContent("{}", Encoding.UTF8, "application/json"); // Empty JSON body

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode(); // Throw exception for non-success status codes
            var responseString = await response.Content.ReadAsStringAsync();
            return GetResponseStatusCode(responseString); // Assuming GetResponseStatusCode parses JSON for status code
        }
    }

    //public ResultDto RequestPayment(Guid OrderId)
    //{
    //    var request = new RestRequest($"/api/OrderPayment?OrderId={OrderId}", Method.Post);

    //    request.AddHeader("Content-Type", "application/json");
    //    var response = httpClint.Execute(request);
    //    return GetResponseStatusCode(response);
    //}
    private static ResultDto GetResponseStatusCode(string responseString)
    {
        // Assuming responseString is a valid JSON string
        try
        {
            var responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
            if (responseObject.ContainsKey("IsSuccess") && (bool)responseObject["IsSuccess"])
            {
                return new ResultDto { IsSuccess = true };
            }
            else if (responseObject.ContainsKey("Message"))
            {
                return new ResultDto { IsSuccess = false, Message = (string)responseObject["Message"] };
            }
            else
            {
                // Handle unexpected response format
                return new ResultDto { IsSuccess = false, Message = "Unexpected response format" };
            }
        }
        catch (JsonException)
        {
            // Handle invalid JSON format
            return new ResultDto { IsSuccess = false, Message = "Invalid response format" };
        }
    }

    //private static ResultDto GetResponseStatusCode(RestResponse response)
    //{
    //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
    //    {
    //        return new ResultDto
    //        {
    //            IsSuccess = true,
    //        };
    //    }
    //    else
    //    {
    //        return new ResultDto
    //        {
    //            IsSuccess = false,
    //            Message = response.ErrorMessage
    //        };
    //    }
    //}
}
