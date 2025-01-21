using Microsoft.AspNetCore.Mvc;

namespace Crystal_Eyes_Controller.Controllers
{
	public class CustomerController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}
		public IActionResult Register()
		{
			return View();
		}
		public IActionResult OTP()
		{
			return View();
		}
		public IActionResult ForgotPassword()
		{
			return View();
		}
		public IActionResult ResetPassword()
		{
			return View();
		}
		public IActionResult ChangePassword()
		{
			return View();
		}

		public IActionResult Shop()
		{
			return View();
		}

		public IActionResult OrderDetail()
		{
			return View();
		}

		public IActionResult HistoryOrder()
		{
			return View();
		}

		public IActionResult Checkout()
		{
			return View();
		}

		public IActionResult EditProfile()
		{
			return View();
		}

		public IActionResult Profile()
		{
			return View();
		}
		public IActionResult Wishlist()
		{
			return View();
		}
        public IActionResult ProductDetail()
        {
            return View();
        }
    }
}
