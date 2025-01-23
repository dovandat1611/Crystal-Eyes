using Crystal_Eyes_Controller.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Eyes_Controller.Controllers
{
	[Route("customer")]
	public class CustomerController : Controller
	{
		public CustomerController()
		{
		}
		public bool checkLogin()
		{
			bool checkL = true;
			var httpContext = HttpContext;
			if (httpContext != null)
			{
				string isCustomerAuthenticated = httpContext.Request.Cookies["customer"];
				if (string.IsNullOrEmpty(isCustomerAuthenticated))
				{
					checkL = false;
				}
			}
			return checkL;
		}

		[HttpGet("change-password")]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpGet("order-detail")]
		public IActionResult OrderDetail()
		{
			return View();
		}

		[HttpGet("history-order")]
		public IActionResult HistoryOrder()
		{
			return View();
		}

		[HttpGet("checkout")]
		public IActionResult Checkout()
		{
			return View();
		}

		[HttpGet("edit-profile")]
		public IActionResult EditProfile()
		{
			return View();
		}

		[HttpGet("profile")]
		public IActionResult Profile()
		{
			return View();
		}

		[HttpGet("wish-list")]
		public IActionResult Wishlist()
		{
			return View();
		}

    }
}
