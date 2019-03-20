using Microsoft.AspNetCore.Mvc;
using Utils;

namespace OnlineCharter.API.WebService.Models
{
    public static class ControllerExtensions
    {
        public static IActionResult Result<T>(this Controller controller, Result<T> result)
        {
            if (result.Successful)
            {
                return controller.Ok(new ExecutionResponse<T>
                {
                    Result = result.Value
                });
            }

            return controller.Ok(new ExecutionResponse<T>
            {
                Error = result.Error
            });
        }

        public static IActionResult Result(this Controller controller, Result result)
        {
            if (result.Successful)
            {
                return controller.Ok(new ExecutionResponse<string>
                {
                    Result = "Success"
                });
            }

            return controller.Ok(new ExecutionResponse<string>
            {
                Error = result.Error
            });
        }
    }
}
