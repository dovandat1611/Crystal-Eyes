using Crystal_Eyes_Controller.IServices;
using Newtonsoft.Json;

namespace Crystal_Eyes_Controller.Services
{
	public class WishlistService : IWishlistService
	{
		public async Task<(string message, string action, int totalWishlist)> ProcessWishlistAsync(int productId, HttpContext context)
		{
			// Lấy cookie "wishlist" (nếu có)
			var wishlistCookie = context.Request.Cookies["wishlist"];
			List<int> wishlist = wishlistCookie != null
				? JsonConvert.DeserializeObject<List<int>>(wishlistCookie)
				: new List<int>();

			string message = "Thực hiện không thành công";
			string action = string.Empty;

			if (wishlist.Contains(productId))
			{
				// Nếu sản phẩm đã có trong wishlist, xóa nó
				wishlist.Remove(productId);
				message = "Xóa thành công sản phẩm ra khỏi Yêu Thích";
				action = "Delete";
			}
			else
			{
				// Nếu sản phẩm chưa có trong wishlist, thêm vào danh sách
				wishlist.Add(productId);
				message = "Thêm thành công sản phẩm vào Yêu Thích";
				action = "Create";
			}

			// Lưu danh sách wishlist vào cookie (set thời gian hết hạn là 30 ngày)
			var cookieOptions = new CookieOptions
			{
				Expires = DateTime.Now.AddDays(30),
				HttpOnly = true,
				Secure = true  // Chỉ dùng trong môi trường HTTPS
			};
			context.Response.Cookies.Append("wishlist", JsonConvert.SerializeObject(wishlist), cookieOptions);

			return (message, action, wishlist.Count);
		}
	}
}
