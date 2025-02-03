namespace Crystal_Eyes_Controller.IServices
{
	public interface IWishlistService
	{
		Task<(string message, string action, int totalWishlist)> ProcessWishlistAsync(int productId, HttpContext context);
	}
}
