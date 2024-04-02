using RestSharp;
using System.Text.Json;

namespace Microservice.Web.Frontend.Services.ProductServices;


public interface IProductService
{
    IEnumerable<ProductDto> GetAllProduct();
    ProductDto GetProduct(Guid Id);
}
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
        Console.WriteLine(response.Content);
        var product = JsonSerializer.Deserialize<List<ProductDto>>(response.Content);
        return product;
    }

    public ProductDto GetProduct(Guid Id)
    {
        var request = new RestRequest($"api/Product/{Id}", Method.Get);
        var response = restClient.Execute(request);
        Console.WriteLine(response.Content);
        var product = JsonSerializer.Deserialize<ProductDto>(response.Content);
        return product;
    }
}
public class ProductCategory
{
    public string CategoryId;
    public string Category;
}

public class ProductDto
{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string image { get; set; }
    public int? price { get; set; }
    public ProductCategory productCategory { get; set; }
}
