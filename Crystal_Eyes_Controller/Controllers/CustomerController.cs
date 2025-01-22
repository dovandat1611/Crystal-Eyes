using Crystal_Eyes_Controller.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Eyes_Controller.Controllers
{
	public class CustomerController : Controller
	{


		public bool checkLogin()
		{
			bool checkL= true;
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

		[HttpGet]
		public IActionResult Logout()
		{
			HttpContext.Response.Cookies.Delete("customer");
			return RedirectToAction("Login", "Customer");
		}


		[HttpGet]
		public IActionResult Login()
		{
			ViewData["IsLogin"] = checkLogin();
			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
			ViewData["IsLogin"] = checkLogin();
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			if (model.Email == "admin@example.com" && model.Password == "password123")
			{
				HttpContext.Response.Cookies.Append("customer","true");
				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError("LoginFail", "Email hoặc mật khẩu không đúng");
			return View(model);
		}

		[HttpGet]
		public IActionResult Register()
		{
			ViewData["IsLogin"] = checkLogin();
			return View();
		}

		[HttpPost]
		public IActionResult Register(string name, string phone, string email, DateTime dob, string username, string password, string repassword)
		{
			ViewData["IsLogin"] = checkLogin();
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
