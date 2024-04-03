namespace Microservice.Web.Frontend.Services.ProductServices;


public interface IProductService
{
    IEnumerable<ProductDto> GetAllProduct();
    ProductDto GetProduct(Guid Id);
}
