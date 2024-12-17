using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;


namespace AdvertisingAds.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var result = JsonConvert.SerializeObject(new { error = ex.Message });

            // Customize the status code based on the exception type (Optional)
            // if (ex is NotFoundException) code = HttpStatusCode.NotFound;

            context.Response.Clear();

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)code;


            return context.Response.WriteAsync(result);

        }

    }
}
