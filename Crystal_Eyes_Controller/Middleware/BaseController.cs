using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Eyes_Controller.Middleware
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var httpContext = context.HttpContext;

            ViewBag.IsLoggedIn = httpContext.Items["IsLoggedIn"];
        }
    }

}
