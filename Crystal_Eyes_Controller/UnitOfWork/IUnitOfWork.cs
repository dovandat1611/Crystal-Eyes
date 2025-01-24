using Crystal_Eyes_Controller.IRepositories;

namespace Crystal_Eyes_Controller.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IAdminRepository Admin { get; }
		ICartRepository Cart { get; }
		ICategoryRepository Category { get; }
		IColorRepository Color { get; }
		ICustomerRepository Customer { get; }
		IExternalLoginRepository ExternalLogin { get; }
		IFeedbackRepository Feedback { get; }
		IImageRepository Image { get; }
		IOrderDetailRepository OrderDetail { get; }
		IOrderRepository Order { get; }
		IProductRepository Product { get; }
		IUserRepository User { get; }
		IUserOtpRepository UserOtp { get; }
		IWishlistRepository Wishlist { get; }

		Task<int> SaveChangesAsync();
	}
}
