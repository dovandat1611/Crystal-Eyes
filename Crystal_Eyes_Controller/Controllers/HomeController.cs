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
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Crystal_Eyes_Controller.Controllers
{
    [CustomAuthorize]
	public class HomeController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMailSystemService _mailSystemService;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public HomeController(IUnitOfWork unitOfWork, IMailSystemService mailSystemService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_mailSystemService = mailSystemService;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}


		[HttpGet]
		public async Task<IActionResult> Index(string search = "best-seller")
        {
			var httpContext = _httpContextAccessor.HttpContext;

			var products = _unitOfWork.Product.Queryable().Include(x => x.Wishlists).Include(p => p.OrderDetails).Include(x => x.Colors)
			.Include(p => p.Feedbacks).Where(p => p.IsDelete == false && p.IsActive == true);

			switch (search)
			{
				case "best-seller":
					products = products.OrderByDescending(p => p.OrderDetails.Count);
					ViewBag.Search = search;
					break;
				case "rating":
					products = products.OrderByDescending(p => p.Feedbacks.Any() ? p.Feedbacks.Average(f => f.Star) : 0);
					ViewBag.Search = search;
					break;
				case "new":
					products = products.OrderByDescending(p => p.ProductId);
					ViewBag.Search = search;
					break;
				default:
					ViewBag.Search = search;
					break;
			}

			var productsDto = _mapper.Map<List<ProductViewDto>>(products);

			var wishlist = httpContext.Items["WishList"] as List<int>;

			foreach (var productDto in productsDto)
			{
				productDto.IsWishlist = wishlist?.Contains(productDto.ProductId) ?? false;
			}

			ViewBag.Products = productsDto;
			return View();
        }

		[HttpGet("shop")]
		public async Task<IActionResult> Shop(int category = 0, string menu = null, string price = null, string sort = null, string color = null)
		{
			var httpContext = _httpContextAccessor.HttpContext;

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

			var wishlist = httpContext.Items["WishList"] as List<int>;

			var productsDto = _mapper.Map<List<ProductViewDto>>(products);

			foreach (var productDto in productsDto)
			{
				productDto.IsWishlist = wishlist?.Contains(productDto.ProductId) ?? false;
			}

			ViewBag.Products = productsDto;
			ViewBag.Colors = colors;
			ViewBag.Categories = categories;
			ViewBag.SearchCategory = category;
			return View();
		}

		[HttpPost("shop")]
		public async Task<IActionResult> Shop(int category = 0, string queryName = null)
		{
			var httpContext = _httpContextAccessor.HttpContext;

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

			var wishlist = httpContext.Items["WishList"] as List<int>;

			var productsDto = _mapper.Map<List<ProductViewDto>>(products);

			foreach (var productDto in productsDto)
			{
				productDto.IsWishlist = wishlist?.Contains(productDto.ProductId) ?? false;
			}

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
			var httpContext = _httpContextAccessor.HttpContext;

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

			var wishlist = httpContext.Items["WishList"] as List<int>;

			productDetailDto.IsWishlist = wishlist?.Contains(productDetailDto.ProductId) ?? false;

			if(relatedProductDto.Count > 0)
			{
				foreach (var productDto in relatedProductDto)
				{
					productDto.IsWishlist = wishlist?.Contains(productDto.ProductId) ?? false;
				}
			}

			ViewBag.ProductDetail = productDetailDto;
			ViewBag.ProductDetailColor = colors;
			ViewBag.ProductDetailImage = images;
			ViewBag.RelateProduct = relatedProductDto;
			ViewBag.Review = reviewProductDto;
			return View();
		}

	}
}