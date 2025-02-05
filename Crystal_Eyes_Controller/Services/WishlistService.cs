using Crystal_Eyes_Controller.IServices;
using Newtonsoft.Json;

namespace Crystal_Eyes_Controller.Services
{
	public class WishlistService : IWishlistService
	{
		public async Task<(string message, string action, int totalWishlist)> ProcessWishlistAsync(int productId, HttpContext context)
		{
			var wishlistCookie = context.Request.Cookies["wishlist"];
			List<int> wishlist = wishlistCookie != null
				? JsonConvert.DeserializeObject<List<int>>(wishlistCookie)
				: new List<int>();

			string message = "Thực hiện không thành công";
			string action = string.Empty;

			if (wishlist.Contains(productId))
			{
				wishlist.Remove(productId);
				message = "Xóa thành công sản phẩm ra khỏi Yêu Thích";
				action = "Delete";
			}
			else
			{
				wishlist.Add(productId);
				message = "Thêm thành công sản phẩm vào Yêu Thích";
				action = "Create";
			}

			context.Response.Cookies.Append("wishlist", JsonConvert.SerializeObject(wishlist));

			return (message, action, wishlist.Count);
		}

	}
}
