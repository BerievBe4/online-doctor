using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace online_doctor.Filters
{
    public class DataFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string login = filterContext.HttpContext.Session.GetString("Login");

            if (!string.IsNullOrEmpty(login))
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
