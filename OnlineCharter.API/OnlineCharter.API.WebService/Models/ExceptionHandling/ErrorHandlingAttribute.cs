using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineCharter.API.WebService.Models.ExceptionHandling
{
    public class ErrorHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new OkObjectResult(
                new ExecutionResponse<string>
                {
                    Error = context.Exception.Message
                });

            context.ExceptionHandled = true;
        }   
    }
}
