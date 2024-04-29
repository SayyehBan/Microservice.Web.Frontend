using DiscountService.Proto;
using Grpc.Net.Client;
using Microservice.Web.Frontend.Models.Dtos;
using Microservice.Web.Frontend.Models.Links;

namespace Microservice.Web.Frontend.Services.DiscountServices;

public class RDiscountService : IDiscountService

{
    private readonly GrpcChannel channel;
    private readonly IConfiguration configuration;

    public RDiscountService(IConfiguration configuration)
    {
        this.configuration = configuration;
        string discountServer = LinkServices.DiscountService;

        channel = GrpcChannel.ForAddress(discountServer);
    }
    private static ResultDto<DiscountDto> GetDiscountByValue(ResultGetDiscountByCode result)
    {
        if (result.IsSuccess)
        {
            return new ResultDto<DiscountDto>
            {
                Data = new DiscountDto
                {
                    Amount = result.Data.Amount,
                    Code = result.Data.Code,
                    Id = Guid.Parse(result.Data.Id),
                    Used = result.Data.Used,
                },
                IsSuccess = result.IsSuccess,
                Message = result.Message
            };
        }
        return new ResultDto<DiscountDto>
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message
        };
    }
    public ResultDto<DiscountDto> GetDiscountByCode(string Code)
    {
        var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
        var result = grpc_discountService.GetDiscountByCode(new RequestGetDiscountByCode
        {
            Code = Code
        });
        return GetDiscountByValue(result);
    }


    public ResultDto<DiscountDto> GetDiscountById(Guid Id)
    {
        var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
        var result = grpc_discountService.GetDiscountById(new RequestGetDiscountById
        {
            Id = Id.ToString()
        });
        return GetDiscountByValue(result);
    }

    public ResultDto UseDiscount(Guid DiscountId)
    {
        var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
        var result = grpc_discountService.UseDiscount(new RequestUseDiscount
        {
            Id = DiscountId.ToString()
        });
        return new ResultDto
        {
            IsSuccess = result.IsSuccess
        };
    }
}
