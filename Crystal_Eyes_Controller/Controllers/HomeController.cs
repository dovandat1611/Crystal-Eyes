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
using Crystal_Eyes_Controller.UnitOfWork;
using Crystal_Eyes_Controller.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.WebRequestMethods;
using Crystal_Eyes_Controller.Services;
using Crystal_Eyes_Controller.Middleware;
using AutoMapper;
using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.Dtos.Product;
using Microsoft.IdentityModel.Tokens;
using Crystal_Eyes_Controller.Dtos.Feedback;
using System;

namespace Crystal_Eyes_Controller.Controllers
{
    [CustomAuthorize]
	public class HomeController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuthenticationService _authenticationService;
		private readonly IMailSystemService _mailSystemService;
		private readonly IMapper _mapper;


		public HomeController(IUnitOfWork unitOfWork, IAuthenticationService authenticationService, IMailSystemService mailSystemService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_authenticationService = authenticationService;
			_mailSystemService = mailSystemService;
			_mapper = mapper;
		}


		[HttpGet]
		public async Task<IActionResult> Index(int category = 0, string menu = null, string price = null, string sort = null, string color = null)
        {
			var products = _unitOfWork.Product.Queryable().Include(x => x.Wishlists).Include(p => p.OrderDetails).Include(x => x.Colors)
			.Include(p => p.Feedbacks).Where(p => p.IsDelete == false && p.IsActive == true);

			var colors = await _unitOfWork.Color.Queryable().Select(x => x.ColorName).Distinct().ToListAsync();

			var categories = await _unitOfWork.Category.Queryable().ToListAsync();

			if (ViewBag.IsLoggedIn != null && (bool)ViewBag.IsLoggedIn == true && ViewBag.RoleName == Constants.Role_Name.CUSTOMER)
			{
				int userId = int.Parse(ViewBag.UserId.ToString());

				var carts = await _unitOfWork.Cart.Queryable().Include(x => x.Product).Where(x => x.UserId == userId).ToListAsync();

				var cartsDto = _mapper.Map<List<CartViewDto>>(carts);

				var totalAmount = cartsDto.Sum(x => x.TotalPrice);

				ViewBag.Carts = cartsDto;
				ViewBag.TotalAmount = totalAmount;
			}

			if(category > 0 ||  menu != null || price != null || sort != null || color != null)
			{
				ViewBag.MainScreen = true;
			}

			if (category > 0)
			{
				products = products.Where(x => x.CategoryId ==  category);
			}

			switch (menu)
			{
				case "popular":
					products = products.OrderByDescending(p => p.OrderDetails.Count);
					ViewBag.Menu = menu;
					break;
				case "rating":
					products = products.OrderByDescending(p => p.Feedbacks.Any() ? p.Feedbacks.Average(f => f.Star) : 0);
					ViewBag.Menu = menu;
					break;
				case "new":
					products = products.OrderByDescending(p => p.ProductId);
					ViewBag.Menu = menu;
					break;
				default:
					ViewBag.Menu = menu;
					break;
			}


			switch (sort)
			{
				case "lth":
					products = products.OrderBy(p => p.Price * (100 - p.Discount ?? 0) / 100);
					ViewBag.Sort = sort;
					break;
				case "htl":
					products = products.OrderByDescending(p => p.Price * (100 - p.Discount ?? 0) / 100);
					ViewBag.Sort = sort;
					break;
				default:
					break;
			}

			switch (price)
			{
				case "0to1":
					products = products.Where(p => p.Price * (100 - p.Discount ?? 0) / 100 >= 0 && p.Price * (100 - p.Discount ?? 0) / 100 <= 100000);
					ViewBag.Price = price;
					break;
				case "1to3":
					products = products.Where(p => p.Price * (100 - p.Discount ?? 0) / 100 >= 100000 && p.Price * (100 - p.Discount ?? 0) / 100 <= 300000);
					ViewBag.Price = price;
					break;
				case "3to5":
					products = products.Where(p => p.Price * (100 - p.Discount ?? 0) / 100 >= 300000 && p.Price * (100 - p.Discount ?? 0) / 100 <= 500000);
					ViewBag.Price = price;
					break;
				case "5plus":
					products = products.Where(p => p.Price * (100 - p.Discount ?? 0) / 100 >= 500000);
					ViewBag.Price = price;
					break;
				default:
					ViewBag.Price = price;
					break;
			}

			if(color != null)
			{
				products = products.Where(x => x.Colors.Any(c => c.ColorName == color));
				ViewBag.Color = color;
			}

			var productsDto = _mapper.Map<List<ProductViewDto>>(products);

			ViewBag.Products = productsDto;
			ViewBag.Colors = colors;
			ViewBag.Categories = categories;
			ViewBag.SearchCategory = category;
			return View();
        }

		[HttpPost]
		public async Task<IActionResult> Index(string action, string queryName, int category = 0)
		{
			var productsQuery = _unitOfWork.Product.Queryable().Include(x => x.Wishlists).Include(p => p.OrderDetails).Include(x => x.Colors)
			.Include(p => p.Feedbacks).Where(p => p.IsDelete == false && p.IsActive == true);
			var colors = await _unitOfWork.Color.Queryable().Select(x => x.ColorName).Distinct().ToListAsync();


			if (action == "SearchName" && !string.IsNullOrEmpty(queryName))

			if (!string.IsNullOrWhiteSpace(queryName))
			{
				productsQuery = productsQuery.Where(p => p.Name.Contains(queryName));
			}

			if (category != 0)
			{
				productsQuery = productsQuery.Where(x => x.CategoryId == category);
			}

			var products = await productsQuery.ToListAsync();

			var categories = await _unitOfWork.Category.Queryable().ToListAsync();

			if (ViewBag.IsLoggedIn != null && (bool)ViewBag.IsLoggedIn == true && ViewBag.RoleName == Constants.Role_Name.CUSTOMER)
			{
				int userId = int.Parse(ViewBag.UserId.ToString());

				var carts = await _unitOfWork.Cart.Queryable().Include(x => x.Product).Where(x => x.UserId == userId).ToListAsync();

				var cartsDto = _mapper.Map<List<CartViewDto>>(carts);

				var totalAmount = cartsDto.Sum(x => x.TotalPrice);

				ViewBag.Carts = cartsDto;
				ViewBag.TotalAmount = totalAmount;
			}

			var productsDto = _mapper.Map<List<ProductViewDto>>(products);

			ViewBag.Products = productsDto;
			ViewBag.Categories = categories;


			//SEARCH
			ViewBag.SearchCategory = category;
			ViewBag.SearchName = queryName;
			ViewBag.Colors = colors;
			ViewBag.MainScreen = true;
			return View();
		}

		[HttpGet("shop")]
		public async Task<IActionResult> Shop(int category = 0, string menu = null, string price = null, string sort = null, string color = null)
		{
			var products = _unitOfWork.Product.Queryable().Include(x => x.Wishlists).Include(p => p.OrderDetails).Include(x => x.Colors)
			.Include(p => p.Feedbacks).Where(p => p.IsDelete == false && p.IsActive == true);

			var colors = await _unitOfWork.Color.Queryable().Select(x => x.ColorName).Distinct().ToListAsync();

			var categories = await _unitOfWork.Category.Queryable().ToListAsync();

			if (ViewBag.IsLoggedIn != null && (bool)ViewBag.IsLoggedIn == true && ViewBag.RoleName == Constants.Role_Name.CUSTOMER)
			{
				int userId = int.Parse(ViewBag.UserId.ToString());

				var carts = await _unitOfWork.Cart.Queryable().Include(x => x.Product).Where(x => x.UserId == userId).ToListAsync();

				var cartsDto = _mapper.Map<List<CartViewDto>>(carts);

				var totalAmount = cartsDto.Sum(x => x.TotalPrice);

				ViewBag.Carts = cartsDto;
				ViewBag.TotalAmount = totalAmount;
			}

			if (category > 0 || menu != null || price != null || sort != null || color != null)
			{
				ViewBag.MainScreen = true;
			}

			if (category > 0)
			{
				products = products.Where(x => x.CategoryId == category);
			}

			switch (menu)
			{
				case "popular":
					products = products.OrderByDescending(p => p.OrderDetails.Count);
					ViewBag.Menu = menu;
					break;
				case "rating":
					products = products.OrderByDescending(p => p.Feedbacks.Any() ? p.Feedbacks.Average(f => f.Star) : 0);
					ViewBag.Menu = menu;
					break;
				case "new":
					products = products.OrderByDescending(p => p.ProductId);
					ViewBag.Menu = menu;
					break;
				default:
					ViewBag.Menu = menu;
					break;
			}


			switch (sort)
			{
				case "lth":
					products = products.OrderBy(p => p.Price * (100 - p.Discount ?? 0) / 100);
					ViewBag.Sort = sort;
					break;
				case "htl":
					products = products.OrderByDescending(p => p.Price * (100 - p.Discount ?? 0) / 100);
					ViewBag.Sort = sort;
					break;
				default:
					break;
			}

			switch (price)
			{
				case "0to1":
					products = products.Where(p => p.Price * (100 - p.Discount ?? 0) / 100 >= 0 && p.Price * (100 - p.Discount ?? 0) / 100 <= 100000);
					ViewBag.Price = price;
					break;
				case "1to3":
					products = products.Where(p => p.Price * (100 - p.Discount ?? 0) / 100 >= 100000 && p.Price * (100 - p.Discount ?? 0) / 100 <= 300000);
					ViewBag.Price = price;
					break;
				case "3to5":
					products = products.Where(p => p.Price * (100 - p.Discount ?? 0) / 100 >= 300000 && p.Price * (100 - p.Discount ?? 0) / 100 <= 500000);
					ViewBag.Price = price;
					break;
				case "5plus":
					products = products.Where(p => p.Price * (100 - p.Discount ?? 0) / 100 >= 500000);
					ViewBag.Price = price;
					break;
				default:
					ViewBag.Price = price;
					break;
			}

			if (color != null)
			{
				products = products.Where(x => x.Colors.Any(c => c.ColorName == color));
				ViewBag.Color = color;
			}

			var productsDto = _mapper.Map<List<ProductViewDto>>(products);

			ViewBag.Products = productsDto;
			ViewBag.Colors = colors;
			ViewBag.Categories = categories;
			ViewBag.SearchCategory = category;
			return View();
		}

		[HttpPost("shop")]
		public async Task<IActionResult> Shop(int category = 0, string queryName = null)
		{
			var productsQuery = _unitOfWork.Product.Queryable().Include(x => x.Wishlists).Include(p => p.OrderDetails).Include(x => x.Colors)
			.Include(p => p.Feedbacks).Where(p => p.IsDelete == false && p.IsActive == true);
			var colors = await _unitOfWork.Color.Queryable().Select(x => x.ColorName).Distinct().ToListAsync();


			if (!string.IsNullOrEmpty(queryName))
			{

				if (!string.IsNullOrWhiteSpace(queryName))
				{
					productsQuery = productsQuery.Where(p => p.Name.Contains(queryName));
				}
			}

			if (category != 0)
			{
				productsQuery = productsQuery.Where(x => x.CategoryId == category);
			}

			var products = await productsQuery.ToListAsync();

			var categories = await _unitOfWork.Category.Queryable().ToListAsync();

			if (ViewBag.IsLoggedIn != null && (bool)ViewBag.IsLoggedIn == true && ViewBag.RoleName == Constants.Role_Name.CUSTOMER)
			{
				int userId = int.Parse(ViewBag.UserId.ToString());

				var carts = await _unitOfWork.Cart.Queryable().Include(x => x.Product).Where(x => x.UserId == userId).ToListAsync();

				var cartsDto = _mapper.Map<List<CartViewDto>>(carts);

				var totalAmount = cartsDto.Sum(x => x.TotalPrice);

				ViewBag.Carts = cartsDto;
				ViewBag.TotalAmount = totalAmount;
			}

			var productsDto = _mapper.Map<List<ProductViewDto>>(products);

			ViewBag.Products = productsDto;
			ViewBag.Categories = categories;


			//SEARCH
			ViewBag.SearchCategory = category;
			ViewBag.SearchName = queryName;
			ViewBag.Colors = colors;
			ViewBag.MainScreen = true;
			return View();
		}

		[HttpGet("product-detail/{productId}")]
		public async Task<IActionResult> ProductDetail(int productId)
		{

			var product = await _unitOfWork.Product.Queryable()
				.Include(x => x.Wishlists)
				.Include(x => x.Category)
				.Where(p => p.IsDelete == false && p.IsActive == true)
				.FirstOrDefaultAsync(x => x.ProductId == productId);

			if (product == null)
			{
				return NotFound(); 
			}


			var relatedProduct = await _unitOfWork.Product.Queryable()
				.Include(x => x.Wishlists)
				.Include(x => x.Category)
				.Where(p => p.IsDelete == false && p.IsActive == true && p.CategoryId == product.CategoryId).ToListAsync();

			var colors = await _unitOfWork.Color.Queryable().Where(x => x.ProductId == productId).ToListAsync();
			var images = await _unitOfWork.Image.Queryable().Where(x => x.ProductId == productId).ToListAsync();
			var reviews = await _unitOfWork.Feedback.Queryable().Include(x => x.User).ThenInclude(x => x.Customer).Where(x => x.ProductId == productId).ToListAsync();


			var productDetailDto = _mapper.Map<ProductDetailViewDto>(product);
			var relatedProductDto = _mapper.Map<List<ProductViewDto>>(relatedProduct);
			var reviewProductDto = _mapper.Map<List<FeedbackViewDto>>(reviews);

			ViewBag.ProductDetail = productDetailDto;
			ViewBag.ProductDetailColor = colors;
			ViewBag.ProductDetailImage = images;
			ViewBag.RelateProduct = relatedProductDto;
			ViewBag.Review = reviewProductDto;
			return View();
		}

		[HttpGet("logout")]
		public IActionResult Logout()
		{	
			if(ViewBag.IsLoggedIn == true)
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

				HttpContext.Response.Cookies.Append("account", user.Email);

				if (user.RoleName == "Customer")
				{
					return RedirectToAction("Index", "Home");
				}

				if(user.RoleName == "Admin")
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