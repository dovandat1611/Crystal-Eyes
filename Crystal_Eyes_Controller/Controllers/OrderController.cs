using AutoMapper;
using Crystal_Eyes_Controller.Common;
using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.Services;
using Crystal_Eyes_Controller.UnitOfWork;
using Crystal_Eyes_Controller.Utils;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Crystal_Eyes_Controller.Controllers
{
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMailSystemService _mailSystemService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly CartService _cartService;


		public OrderController(IUnitOfWork unitOfWork, IMailSystemService mailSystemService, IHttpContextAccessor httpContextAccessor, CartService cartService)
		{
			_unitOfWork = unitOfWork;
			_mailSystemService = mailSystemService;
			_httpContextAccessor = httpContextAccessor;
			_cartService = cartService;
		}

		[HttpPost("create-order")]
		public async Task<IActionResult> AddToCart(string name, string phone, string email,string address, string content)
		{
			try
			{
				var httpContext = _httpContextAccessor.HttpContext;
				if (httpContext == null)
				{
					return BadRequest(new { success = false, message = "Lỗi hệ thống!" });
				}

				var cartSession = httpContext.Session.GetString("Cart");
				List<CartItemDto> cart = cartSession != null
					? JsonConvert.DeserializeObject<List<CartItemDto>>(cartSession)
					: new List<CartItemDto>();

				if (!cart.Any())
				{
					return BadRequest(new { success = false, message = "Giỏ hàng trống!" });
				}

				var getCart = new List<CartViewDto>();

				foreach (var item in cart)
				{
					var existingItem = await _unitOfWork.Product.Queryable()
						.Include(x => x.Colors)
						.FirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.Colors.Any(c => c.ColorId == item.Color));

					if (existingItem == null) continue;

					decimal discountPrice = existingItem.Discount.HasValue
						? existingItem.Price * (100 - existingItem.Discount.Value) / 100
						: existingItem.Price;

					getCart.Add(new CartViewDto
					{
						ProductId = existingItem.ProductId,
						Quantity = item.Quantity,
						MainImage = existingItem.MainImage,
						Name = existingItem.Name,
						Price = discountPrice,
						TotalPrice = discountPrice * item.Quantity,
						ColorId = item.Color,
						ColorName = existingItem.Colors.FirstOrDefault(c => c.ColorId == item.Color)?.ColorName ?? "Không xác định"
					});
				}

				var order = new Order
				{
					NameReceiver = name,
					PhoneReceiver = phone,
					AddressReceiver = address,
					ContentReservation = content,
					TotalAmount = getCart.Sum(x => x.TotalPrice),
					OrderStatus = "Pending",
					PendingAt = DateTime.UtcNow
				};

				// Lấy userId từ Claims
				var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim != null)
				{
					order.UserId = int.Parse(userIdClaim.Value);
				}

				var addOrder = await _unitOfWork.Order.CreateAndReturnAsync(order);

				if (addOrder == null)
				{
					return BadRequest(new { success = false, message = "Lỗi khi tạo đơn hàng!" });
				}

				var orderDetails = getCart.Select(item => new OrderDetail
				{
					OrderId = addOrder.OrderId,
					ProductId = item.ProductId,
					Price = item.Price,
					Quantity = item.Quantity,
					TotalPrice = item.TotalPrice
				}).ToList();

				await _unitOfWork.OrderDetail.CreateRangeAsync(orderDetails);
				await _unitOfWork.SaveChangesAsync();
				_cartService.ClearCart();
				await _mailSystemService.SendEmailAsync(email, Constants.Email_Subject.ADD_ORDER_SUCCESS, EmailTemplates.ADD_ORDER(addOrder, getCart));

				return Json(new { success = true, message = "Đặt hàng thành công hãy kiểm tra Email của bạn về thông báo đơn hàng!" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = "Lỗi hệ thống!", error = ex.Message });
			}
		}

	}
}
