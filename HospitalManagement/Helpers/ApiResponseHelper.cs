using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Helpers
{
    public static class ApiResponseHelper
    {
        public static IActionResult CreateSuccess<T>( T data , string message,  int statusCode =200)
        {
            return new ObjectResult(new ApiResponse<T> { StatusCode = statusCode, Message = message, Data = data })
            {
                StatusCode = statusCode
            };

        }

        public static IActionResult CreatFailure( string message, int statusCode)
        {
            return new ObjectResult(new ApiResponse<object> { StatusCode = statusCode =400, Message = message})
            {
                StatusCode = statusCode
            };
        }
    }
}
