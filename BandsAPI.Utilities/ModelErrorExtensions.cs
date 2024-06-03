using BandsAPI.Utilities.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace BandsAPI.Utilities;
public static class ModelErrorExtensions
{
    public static void AddAllErrors(this ModelStateDictionary modelState, IServiceResult serviceResponse)
    {
        foreach (var item in serviceResponse.Errors)
        {
            modelState.AddModelError(item.Key, item.Value);
        }
    }
    public static void AddError(this IServiceResult response, string propName, string errorMessage)
    {
        response.Errors.Add(propName, errorMessage);
    }
}
