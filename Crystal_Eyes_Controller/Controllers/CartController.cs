using AutoMapper;
using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.Services;
using Crystal_Eyes_Controller.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Eyes_Controller.Controllers
{
	public class CartController : Controller
	{
		private readonly CartService _cartService;

		public CartController(CartService cartService)
		{
			_cartService = cartService;
		}

		[HttpPost("cart/add")]
		public IActionResult AddToCart(int productId, int quantity, int color)
		{
			_cartService.AddToCart(productId, quantity, color);
			return Json(new { success = true, message = "Thêm vào giỏ hàng thành công!" });
		}

		[HttpGet("cart")]
		public IActionResult GetCart()
		{
			var cart = _cartService.GetCart();
			return Json(cart);
		}

		[HttpPost("cart/remove")]
		public IActionResult RemoveFromCart(int productId, int color)
		{
			_cartService.RemoveFromCart(productId, color);
			return Json(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng!" });
		}

		[HttpPost("cart/clear")]
		public IActionResult ClearCart()
		{
			_cartService.ClearCart();
			return Json(new { success = true, message = "Đã xóa toàn bộ giỏ hàng!" });
		}

		[HttpPost("cart/update")]
		public IActionResult UpdateToCart([FromBody] UpdateCartRequest request)
		{
			if (request?.CartItems == null || !request.CartItems.Any())
			{
				return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
			}

			_cartService.UpdateToCart(request);

			return Json(new { success = true, message = "Cập nhật giỏ hàng của bạn thành công!" });
		}

	}

}
