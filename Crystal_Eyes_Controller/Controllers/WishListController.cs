using Crystal_Eyes_Controller.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Crystal_Eyes_Controller.Controllers
{
	public class WishListController : Controller
	{
		private readonly IWishlistService _wishlistService;

		public WishListController(IWishlistService wishlistService)
		{
			_wishlistService = wishlistService;
		}

		[HttpGet("wish-list-process/{productId}")]
		public async Task<IActionResult> WishlistProcess(int productId)
		{
			var result = await _wishlistService.ProcessWishlistAsync(productId, HttpContext);
			return Json(new { result.message, result.action, totalWishlist = result.totalWishlist });
		}

	}
}
