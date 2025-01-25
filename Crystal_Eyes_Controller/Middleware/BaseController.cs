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
			ViewBag.UserId = httpContext.Items["UserId"];
			ViewBag.Email = httpContext.Items["Email"];
			ViewBag.RoleName = httpContext.Items["RoleName"];
			ViewBag.TotalCart = httpContext.Items["TotalCart"];
			ViewBag.TotalWishList = httpContext.Items["TotalWishList"];
		}
	}

}
