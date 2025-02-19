using Microsoft.AspNetCore.Mvc.Rendering;

namespace LojiteksWeb.Extensions
{
    public static class String
    {
        public static string IsShown(this IHtmlHelper htmlHelper, string controllers, string cssClass = "show")
        {
            var currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            ReadOnlySpan<string> acceptedControllers = (controllers ?? currentController).Split(',');

            return acceptedControllers.Contains(currentController) ? cssClass : string.Empty;
        }

        public static string IsActive(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "active")
        {
            var currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            var currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            ReadOnlySpan<string> acceptedActions = (actions ?? currentAction)?.Split(',');
            ReadOnlySpan<string> acceptedControllers = (controllers ?? currentController)?.Split(',');

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : string.Empty;
        }
    }
}
