using Crystal_Eyes_Controller.Dtos;
using Crystal_Eyes_Controller.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Eyes_Controller.Controllers
{
	[CustomAuthorize]
	[Route("customer")]
	public class CustomerController : BaseController
	{
		public CustomerController()
		{
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
