using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace dotnetserver.BoardNotificationHub
{
    public class SendNotificationAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next.Invoke();
            // Exit the filter if the result is not OkResult
            if (!(context.HttpContext.Response.StatusCode == 200))
            {
                return;
            }
            // Getting the boardGroup from cookies after the action method is executed
            string boardGroup;
            try
            {
                boardGroup = context.HttpContext.Request.Headers["Board-Group"];
                // Get IHubContext via Service Locator
                var hubContext = context.HttpContext.RequestServices.GetRequiredService<IHubContext<BoardNotificationsHub>>();
                // Invoke SendNotificationToGroup method of the hub
                await hubContext.Clients.Group(boardGroup).SendAsync("ReceiveNotification", boardGroup);
            }
            catch (Exception ex)
            {
                return;
            }
            
        }
    }
}

