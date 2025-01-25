using Crystal_Eyes_Controller.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Eyes_Controller.Middleware
{
	public class AuthMiddleware
	{
		private readonly RequestDelegate _next;

		public AuthMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
		{
			var customerCookie = context.Request.Cookies["account"];

			if (!string.IsNullOrEmpty(customerCookie))
			{
				var user = await unitOfWork.User.Queryable().FirstOrDefaultAsync(u => u.Email == customerCookie);
				if (user != null)
				{
					context.Items["IsLoggedIn"] = true;
				}
			}

			await _next(context);
		}
	}
}
