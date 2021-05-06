using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Trill.Core.Exceptions;

namespace Trill.Api
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
        {
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                if (exception is CustomException customException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var response = new
                    {
                        code = customException.Code,
                        message = customException.Message
                    };
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }
                
                _logger.LogError(exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}