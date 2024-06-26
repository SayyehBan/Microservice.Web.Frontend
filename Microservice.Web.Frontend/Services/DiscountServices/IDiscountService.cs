﻿using Microservice.Web.Frontend.Models.Dtos;

namespace Microservice.Web.Frontend.Services.DiscountServices;

public interface IDiscountService
{
    ResultDto<DiscountDto> GetDiscountByCode(string Code);
    ResultDto<DiscountDto> GetDiscountById(Guid Id);
    ResultDto UseDiscount(Guid DiscountId);
}
