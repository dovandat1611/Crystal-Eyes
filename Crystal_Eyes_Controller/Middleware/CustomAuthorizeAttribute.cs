using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Crystal_Eyes_Controller.UnitOfWork;

namespace Crystal_Eyes_Controller.Middleware
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var actionName = context.ActionDescriptor.RouteValues["action"];
            var controllerName = context.ActionDescriptor.RouteValues["controller"];

            var customerCookie = httpContext?.Request.Cookies["account"];
            var unitOfWork = httpContext.RequestServices.GetService<IUnitOfWork>();

            if (unitOfWork == null)
            {
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
                return;
            }

            if (string.IsNullOrEmpty(customerCookie))
            {
                var publicActions = new List<string> { "Login", "Register", "ForgotPassword", "OTP", "Index", "Shop",
                    "ProductDetail", "Verify", "Register", "ResetPassword", "Checkout", "Wishlist"};

                if (controllerName == "Admin" || controllerName == "Customer" || !publicActions.Contains(actionName))
                {
                    context.Result = new RedirectToActionResult("Login", "Authentication", null);
                    return;
                }

                await next();
                return;
            }

            // 2. Kiểm tra người dùng đã đăng nhập
            var user = await unitOfWork.User.Queryable().FirstOrDefaultAsync(u => u.Email == customerCookie);

            if (user == null)
            {
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
                return;
            }

            // Kiểm tra vai trò
            if (user.RoleName == "Customer")
            {
                //if (controllerName == "Admin")
                //{
                //	context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
                //	return;
                //}

                var authActions = new List<string> { "Login", "Register", "ForgotPassword", "OTP", "Verify", "Register", "ResetPassword" };
                if (controllerName == "Admin" || controllerName == "Authentication" && authActions.Contains(actionName))
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                    return;
                }
            }

            else if (user.RoleName == "Admin")
            {
                //// Những trang không được phép truy cập khi là Admin
                //var customerActions = new List<string> { "Index", "Profile", "Shop", "Wishlist" };
                //if (controllerName == "Customer" || customerActions.Contains(actionName))
                //{
                //    context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
                //    return;
                //}

                // Những trang không được phép truy cập khi đã đăng nhập
                var authActions = new List<string> { "Login", "Register", "ForgotPassword", "OTP", "Verify", "Register", "ResetPassword" };
                if (controllerName == "Customer" || controllerName == "Home" || controllerName== "Authentication" && authActions.Contains(actionName))
                {
                    context.Result = new RedirectToActionResult("Dashboard", "Admin", null);
                    return;
                }
            }

            await next(); // Cho phép tiếp tục thực thi nếu hợp lệ
        }
    }
}
