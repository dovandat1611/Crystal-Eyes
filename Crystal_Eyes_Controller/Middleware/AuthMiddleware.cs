using Crystal_Eyes_Controller.Common;
using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
				}
			}

			// Retrieve Wishlist from Cookie
			var wishlistCookie = context.Request.Cookies["wishlist"];
			List<int> wishlist = wishlistCookie != null
				? JsonConvert.DeserializeObject<List<int>>(wishlistCookie)
				: new List<int>();

			// Retrieve Cart from Session
			var cartSession = context.Session.GetString("Cart");
			List<CartItemDto> cart = cartSession != null
				? JsonConvert.DeserializeObject<List<CartItemDto>>(cartSession)
				: new List<CartItemDto>();


			// Set counts to context.Items
			context.Items["TotalWishList"] = wishlist.Count;
			context.Items["TotalCart"] = cart.Count;

			context.Items["WishList"] = wishlist;

			await _next(context);
		}
	}
}
