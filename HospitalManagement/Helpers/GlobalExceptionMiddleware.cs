using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace HospitalManagement.Helpers
{
    public class GlobalExceptionMiddleware
    {

        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate requestDelegate,
                ILogger<GlobalExceptionMiddleware> logger
            )
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context); // Pass control to the next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurs : {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync( HttpContext context , Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


            var errorResponse = new ErrorResponse<object>
            {
                Message = "An unexpected error occurred.",
                Error = ex.Message,
                Data = null
            };

            var res = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(res); 
        }

       
    }
}
