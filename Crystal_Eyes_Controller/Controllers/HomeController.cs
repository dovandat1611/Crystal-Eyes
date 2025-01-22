using Crystal_Eyes_Controller.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Crystal_Eyes_Controller.Controllers
{
    public class HomeController : Controller
    {
		public HomeController()
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

		public IActionResult Index()
        {
            return View();
        }

		[HttpGet("shop")]
		public IActionResult Shop()
		{
			return View();
		}

		[HttpGet("product-detail")]
		public IActionResult ProductDetail()
		{
			return View();
		}

		[HttpGet("logout")]
		public IActionResult Logout()
		{
			HttpContext.Response.Cookies.Delete("customer");
			return RedirectToAction("Login", "Customer");
		}

		[HttpGet("login")]
		public IActionResult Login()
		{
			ViewData["IsLogin"] = checkLogin();
			return View();
		}

		[HttpPost("login")]
		public IActionResult Login(LoginViewModel model)
		{
			ViewData["IsLogin"] = checkLogin();
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			if (model.Email == "admin@example.com" && model.Password == "password123")
			{
				HttpContext.Response.Cookies.Append("customer", "true");
				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError("LoginFail", "Email hoặc mật khẩu không đúng");
			return View(model);
		}

		[HttpGet("register")]
		public IActionResult Register()
		{
			ViewData["IsLogin"] = checkLogin();
			return View();
		}

		[HttpPost("register")]
		public IActionResult Register(RegisterViewModel model)
		{
			ViewData["IsLogin"] = checkLogin();
			return View();
		}

		[HttpGet("otp")]
		public IActionResult OTP()
		{
			return View();
		}

		[HttpPost("otp")]
		public IActionResult OTP(int OTP)
		{
			return View();
		}

		[HttpGet("forgot-password")]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost("forgot-password")]
		public IActionResult ForgotPassword(int account)
		{
			return View();
		}

		[HttpGet("reset-password")]
		public IActionResult ResetPassword()
		{
			return View();
		}

	}
}