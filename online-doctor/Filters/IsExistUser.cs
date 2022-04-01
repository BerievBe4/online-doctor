using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace online_sdoctor.Filters
{
    public class IsExistUser : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string login = filterContext.HttpContext.Session.GetString("Login");

            if (string.IsNullOrEmpty(login))
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Index" }
                });
            }
            else
            {
                Controller controller = filterContext.Controller as Controller;

                controller.ViewBag.Login = filterContext.HttpContext.Session.GetString("Login");
                controller.ViewBag.RoleID = filterContext.HttpContext.Session.GetInt32("RoleID");
                controller.ViewBag.UserID = filterContext.HttpContext.Session.GetInt32("UserID");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
