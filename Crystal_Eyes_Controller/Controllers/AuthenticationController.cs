using AutoMapper;
using Crystal_Eyes_Controller.Common;
using Crystal_Eyes_Controller.Dtos.Authentication;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Middleware;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.UnitOfWork;
using Crystal_Eyes_Controller.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Eyes_Controller.Controllers
{
	[CustomAuthorize]
	public class AuthenticationController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuthenticationService _authenticationService;
		private readonly IMailSystemService _mailSystemService;
		private readonly IMapper _mapper;

		public AuthenticationController(IUnitOfWork unitOfWork, IAuthenticationService authenticationService, IMailSystemService mailSystemService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_authenticationService = authenticationService;
			_mailSystemService = mailSystemService;
			_mapper = mapper;
		}

		[HttpGet("logout")]
		public IActionResult Logout()
		{
			if (ViewBag.IsLoggedIn == true)
			{
				HttpContext.Response.Cookies.Delete("account");
			}
			return RedirectToAction("Login", "Home");
		}

		[HttpGet("login")]
		public IActionResult Login()
		{
			return View();
		}

		[HttpGet("register")]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginViewModel model)
		{

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _unitOfWork.User.Queryable().Where(x => x.Email == model.Email).FirstOrDefaultAsync();

			if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password) == true)
			{
				if (user.IsVerify == false)
				{
					ModelState.AddModelError("LoginFail", "Tài khoản bạn chưa xác thực. Vui lòng kiểm tra email");
					return View(model);
				}
				if (user.IsActive == false)
				{
					ModelState.AddModelError("LoginFail", "Tài khoản của bạn đã bị khóa");
					return View(model);
				}

				HttpContext.Response.Cookies.Append("account", user.Email);

				if (user.RoleName == "Customer")
				{
					return RedirectToAction("Index", "Home");
				}

				if (user.RoleName == "Admin")
				{
					return RedirectToAction("Dashboard", "Admin");
				}
			}
			ModelState.AddModelError("LoginFail", "Email hoặc mật khẩu không đúng");
			return View(model);
		}

		[HttpGet("verify-account")]
		public async Task<IActionResult> Verify(string code)
		{
			var result = await _authenticationService.VerifyAsync(code);
			ViewBag.Message = result.Message;
			ViewBag.IsSuccess = result.IsSuccess;
			return View();
		}


		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var existingUser = await _unitOfWork.User.Queryable().Where(x => x.Email == model.Email).FirstOrDefaultAsync();

			if (existingUser != null)
			{
				ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống");
				return View(model);
			}

			var result = await _authenticationService.RegisterAsync(model);

			if (result.IsSuccess)
			{
				TempData["MessageSuccess"] = result.Message;
				return RedirectToAction("Login", "Home");
			}

			ModelState.AddModelError("RegisterFail", result.Message);
			return View(model);
		}



		[HttpGet("forgot-password")]
		public async Task<IActionResult> ForgotPassword()
		{
			return View();
		}

		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				ViewBag.Message = "Vui lòng nhập email!";
				return View();
			}

			var user = await _unitOfWork.User.Queryable().FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
			{
				ViewBag.Message = "Email không tồn tại trong hệ thống!";
				return View();
			}

			var otp = new Random().Next(100000, 999999).ToString();

			var otpEntity = new UserOtp
			{
				UserId = user.UserId,
				OtpCode = otp,
				ExpiresAt = DateTime.UtcNow.AddMinutes(10),
				IsUse = false
			};

			await _unitOfWork.UserOtp.CreateAsync(otpEntity);
			await _unitOfWork.SaveChangesAsync();

			await _mailSystemService.SendEmailAsync(
				email,
				Constants.Email_Subject.RESET_PASSWORD,
				EmailTemplates.OTP(otp)
			);

			TempData["Email"] = email;
			return RedirectToAction("OTP", "Home");
		}

		[HttpGet("checkout")]
		public IActionResult Checkout()
		{
			return View();
		}

		[HttpGet("verify-otp")]
		public async Task<IActionResult> OTP()
		{
			var email = TempData["Email"] as string;

			if (string.IsNullOrEmpty(email))
			{
				return RedirectToAction("ForgotPassword");
			}

			ViewBag.Email = email;
			TempData["Email"] = email;
			return View();
		}

		[HttpPost("verify-otp")]
		public async Task<IActionResult> OTP(string email, string otpCode)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otpCode))
			{
				ViewBag.Message = "Vui lòng nhập đầy đủ thông tin!";
				return View();
			}

			var user = await _unitOfWork.User.Queryable().FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
			{
				ViewBag.Message = "Email không tồn tại trong hệ thống!";
				return View();
			}

			var otp = await _unitOfWork.UserOtp.Queryable()
				.Where(o => o.UserId == user.UserId && o.OtpCode == otpCode && o.IsUse == false && o.ExpiresAt > DateTime.UtcNow)
				.FirstOrDefaultAsync();

			if (otp == null)
			{
				ViewBag.Message = "Mã OTP không hợp lệ hoặc đã hết hạn!";
				return View();
			}

			otp.IsUse = true;
			await _unitOfWork.SaveChangesAsync();

			TempData["Email"] = email;
			return RedirectToAction("ResetPassword", "Home");
		}



		[HttpGet("reset-password")]
		public async Task<IActionResult> ResetPassword()
		{
			var email = TempData["Email"] as string;

			if (string.IsNullOrEmpty(email))
			{
				return RedirectToAction("ForgotPassword");
			}

			ViewBag.Email = email;
			TempData["Email"] = email;

			return View();
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword(string email, string newPassword)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword))
			{
				ViewBag.Message = "Vui lòng nhập đầy đủ thông tin!";
				return View();
			}

			var user = await _unitOfWork.User.Queryable().FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
			{
				ViewBag.Message = "Email không tồn tại!";
				return View();
			}

			user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
			await _unitOfWork.SaveChangesAsync();

			TempData["MessageSuccess"] = "Mật khẩu đã được đặt lại thành công!";
			return RedirectToAction("Login", "Home");
		}
	}
}
