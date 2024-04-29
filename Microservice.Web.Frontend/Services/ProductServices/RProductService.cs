using Microservice.Web.Frontend.Models.Links;
using RestSharp;
using System.Text.Json;

namespace Microservice.Web.Frontend.Services.ProductServices;

public class RProductService : IProductService
{
    private readonly RestClient restClient;
    public RProductService(RestClient restClient)
    {
        this.restClient = restClient;
    }

    public IEnumerable<ProductDto> GetAllProduct()
    {
        var request = new RestRequest("api/Product", Method.Get);
        var response = restClient.Execute(request);
        if (response.Content != null)
        {
            var product = JsonSerializer.Deserialize<List<ProductDto>>(response.Content);
            return product;
        }
        return null;
    }

    public ProductDto GetProduct(Guid Id)
    {
        var request = new RestRequest($"api/Product/{Id}", Method.Get);
        var response = restClient.Execute(request);
        var product = JsonSerializer.Deserialize<ProductDto>(response.Content);
        return product;
    }
}
