using Crystal_Eyes_Controller.Dtos;
using Crystal_Eyes_Controller.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Crystal_Eyes_Controller.Controllers
{
    public class HomeController : Controller
    {
		private readonly IUserRepository _userRepository;

		public HomeController(IUserRepository userRepository)
        {
			_userRepository = userRepository;
        }

		public bool IsUserLoggedIn()
		{
			var customerCookie = HttpContext?.Request.Cookies["account"];
			return !string.IsNullOrEmpty(customerCookie);
		}

		public string GetEmailUserLogin()
		{
			var customerCookie = HttpContext?.Request.Cookies["account"];
			return !string.IsNullOrEmpty(customerCookie) ? customerCookie : string.Empty;
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
			if (IsUserLoggedIn())
			{
				HttpContext.Response.Cookies.Delete("customer");
			}
			return RedirectToAction("Login", "Customer");
		}

		[HttpGet("login")]
		public IActionResult Login()
		{
			if (IsUserLoggedIn())
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			var query = await _userRepository.QueryableAsync();

			if (IsUserLoggedIn())
			{
				var email = GetEmailUserLogin();
				var userLoggedIn = await query.FirstOrDefaultAsync(x => x.Email == email);
				if (userLoggedIn == null)
				{
					return RedirectToAction("Login", "Account");
				}
				if (userLoggedIn.RoleName == "Admin")
				{
					return RedirectToAction("Dashboard", "Admin");
				}
				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await query.Where(x => x.Email == model.Email && x.Password == model.Password).SingleOrDefaultAsync();

			if(user != null)
			{	
				if(user.IsVerify == false)
				{
					ModelState.AddModelError("LoginFail", "Tài khoản bạn chưa xác thực. Vui lòng kiểm tra email");
					return View(model);
				}
				if(user.IsActive == false)
				{
					ModelState.AddModelError("LoginFail", "Tài khoản của bạn đã bị khóa");
					return View(model);
				}

				if(user.RoleName == "Customer")
				{
					HttpContext.Response.Cookies.Append("customer", user.Email);
					return RedirectToAction("Index", "Home");
				}

				if(user.RoleName == "Admin")
				{
					HttpContext.Response.Cookies.Append("admin", user.Email);
					return RedirectToAction("Dashboard", "Admin");
				}
				return View(model);
			}

			ModelState.AddModelError("LoginFail", "Email hoặc mật khẩu không đúng");
			return View(model);
		}

		[HttpGet("register")]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost("register")]
		public IActionResult Register(RegisterViewModel model)
		{
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