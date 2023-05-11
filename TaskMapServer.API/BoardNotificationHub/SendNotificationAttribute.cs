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
            if (!(resultContext.Result is OkResult))
            {
                return;
            }
            // Getting the boardGroup from cookies after the action method is executed
            string boardGroup;
            if (context.HttpContext.Request.Cookies.TryGetValue("boardGroup", out boardGroup))
            {
                // Get IHubContext via Service Locator
                var hubContext = context.HttpContext.RequestServices.GetRequiredService<IHubContext<BoardNotificationsHub>>();

                // Invoke SendNotificationToGroup method of the hub
                await hubContext.Clients.Group(boardGroup).SendAsync("ReceiveNotification", boardGroup);
            }
        }
    }
}

