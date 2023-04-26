using Condominiums.Api.Services.Base;

namespace Microsoft.AspNetCore.Mvc;

public static class ControllerExtensions
{
    public static IActionResult ActionResultByServiceResult<TServiceResult>(this ControllerBase controller, TServiceResult serviceResult)
    where TServiceResult : ServiceResult
    {
        Type serviceResultType = serviceResult.GetType();

        if (serviceResult.Success)
        {
            object? objectValue = null;

            if (serviceResultType.IsGenericType)
            {
                objectValue = serviceResultType.GetProperty("Extra")?.GetValue(serviceResult);
            }

            return controller.Ok(objectValue);
        }

        return controller.Problem(serviceResult.ErrorMessage, statusCode: serviceResult.HttpStatusCode);
    }
}
