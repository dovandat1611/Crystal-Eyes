using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.Utils;
using Crystal_Eyes_Controller.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Crystal_Eyes_Controller.Dtos.Authentication;

namespace Crystal_Eyes_Controller.Controllers
{
    public class HomeController : Controller
    {
		private readonly IUserRepository _userRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IMailSystemService _mailSystemService;


	   public HomeController(IUserRepository userRepository, ICustomerRepository customerRepository, IMailSystemService mailSystemService)
        {
			_userRepository = userRepository;
			_customerRepository = customerRepository;
			_mailSystemService = mailSystemService;
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

		private async Task<IActionResult> HandleAuthenticatedUserRedirect(string actionName, string controllerName)
		{
			var query = await _userRepository.QueryableAsync();
			var email = GetEmailUserLogin();

			var userLoggedIn = await query.FirstOrDefaultAsync(x => x.Email == email);
			if (userLoggedIn == null)
			{
				return RedirectToAction(actionName, controllerName);
			}

			if (userLoggedIn.RoleName == "Admin")
			{
				return RedirectToAction("Dashboard", "Admin");
			}

			return RedirectToAction("Index", "Home");
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
		public async Task<IActionResult> Login()
		{
			if (IsUserLoggedIn())
			{
				return await HandleAuthenticatedUserRedirect("Login", "Home");
			}

			return View();
		}

		[HttpGet("register")]
		public async Task<IActionResult> Register()
		{
			if (IsUserLoggedIn())
			{
				return await HandleAuthenticatedUserRedirect("Register", "Home");
			}

			return View();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginViewModel model)
		{

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var query = await _userRepository.QueryableAsync();
			var user = await query.Where(x => x.Email == model.Email).FirstOrDefaultAsync();

			if(user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password) == true)
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


		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model); 
			}

			var query = await _userRepository.QueryableAsync();
			var existingUser = await query.Where(x => x.Email == model.Email).FirstOrDefaultAsync();

			if (existingUser != null)
			{
				ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống");
				return View(model);
			}

			var newUser = new User
			{
				Email = model.Email,
				Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
				RoleName = "Customer",
				IsActive = true,
				IsVerify = true,
				CreatedAt = DateTime.UtcNow,
				IsExternalLogin = false
			};

			var addUser =  await _userRepository.CreateAndReturnAsync(newUser); 

			if(addUser != null)
			{

				var newCustomer = new Customer
				{
					UserId = addUser.UserId,
					Name = model.Name,
					Phone = model.Phone,
					Dob = model.Dob,
					Address = model.Address
				};

				var addCustomer = await _customerRepository.CreateAndReturnAsync(newCustomer);

				if(addCustomer != null)
				{
					// SEND EMAIL
					await _mailSystemService.SendEmailAsync(addUser.Email, Constants.Email_Subject.VERIFY, EmailTemplates.Verify(addCustomer.Name, AesEncryption.Encrypt(addCustomer.UserId.ToString())));

					TempData["RegisterSuccess"] = "Vui lòng check email để verify tài khoản";
					return RedirectToAction("Login", "Home");
				}

				ModelState.AddModelError("RegisterFail", "Đăng ký tài khoản thành công nhưng đăng ký khách hàng đang xảy ra lỗi");
				return View(model);

			}

			ModelState.AddModelError("RegisterFail", "Đăng ký tài khoản đang xảy ra lỗi");
			return View(model);
		}

		[HttpGet("otp")]
		public async Task<IActionResult> OTP()
		{
			if (IsUserLoggedIn())
			{
				return await HandleAuthenticatedUserRedirect("Login", "Home");
			}

			// Check thêm nếu không có sesstion forgotpassword account thì làm gì... 

			return View();
		}

		[HttpPost("otp")]
		public async Task<IActionResult> OTP(int OTP)
		{
			return View();
		}

		[HttpGet("forgot-password")]
		public async Task<IActionResult> ForgotPassword()
		{
			if (IsUserLoggedIn())
			{
				return await HandleAuthenticatedUserRedirect("ForgotPassword", "Home");
			}
			return View();
		}

		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword(int account)
		{
			return View();
		}

		[HttpGet("reset-password")]
		public async Task<IActionResult> ResetPassword()
		{
			if (IsUserLoggedIn())
			{
				return await HandleAuthenticatedUserRedirect("Login", "Home");
			}

			// Check thêm nếu không có sesstion forgotpassword account thì làm gì... 

			return View();
		}

	}
}