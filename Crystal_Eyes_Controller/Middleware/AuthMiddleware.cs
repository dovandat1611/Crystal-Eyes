using Crystal_Eyes_Controller.Common;
using Crystal_Eyes_Controller.Models;
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
					context.Items["UserId"] = user.UserId.ToString();
					context.Items["Email"] = user.Email;
					context.Items["RoleName"] = user.RoleName;
					if (user.RoleName == Constants.Role_Name.CUSTOMER)
					{
						var totalWishList = await unitOfWork.Wishlist.Queryable().Where(x => x.UserId == user.UserId).CountAsync();
						var totalCart = await unitOfWork.Cart.Queryable().Where(x => x.UserId == user.UserId).CountAsync();
						context.Items["TotalCart"] = totalCart;
						context.Items["TotalWishList"] = totalWishList;
					}
				}
			}

			await _next(context);
		}
	}
}
