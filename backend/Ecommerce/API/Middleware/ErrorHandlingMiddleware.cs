using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } 
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            //object errors = null;
            string error = "";
            bool success = false;

            switch (ex)
            {
                case RestException re:
                    logger.LogError(ex, "REST ERROR");
                    error = re.Error;
                    context.Response.StatusCode = (int)re.Code;
                    success = re.Success;
                    break;
                case Exception e:
                    logger.LogError(ex, "SERVER ERROR");
                    error = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            if (error != null)
            {
                var result = JsonSerializer.Serialize(new 
                {
                    success,
                    message = error // or just type error
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}