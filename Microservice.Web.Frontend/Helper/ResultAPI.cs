using Microservice.Web.Frontend.Models.Dtos;
using RestSharp;

namespace Microservice.Web.Frontend.Helper;

public static class ResultAPI
{
    public static ResultDto GetResponseStatusCode(RestResponse response)
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
