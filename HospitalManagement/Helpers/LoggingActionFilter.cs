using Microsoft.AspNetCore.Mvc.Filters;

namespace HospitalManagement.Helpers
{
    public class LoggingActionFilter : IActionFilter
    {

        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }


        //before controller 
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            _logger.LogInformation($"\n\n========== START Executing {controller}/{action} ==========\n");
        }

        // after controller 
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            if (context.Exception == null)
            {
                _logger.LogInformation($"\n\n========== FINISHED Executing {controller}/{action} Successfully ==========\n");
            }
            else
            {
                _logger.LogError(context.Exception, $"\n\n EXCEPTION in {controller}/{action} ==========\n");
            }
        }
    }
}
