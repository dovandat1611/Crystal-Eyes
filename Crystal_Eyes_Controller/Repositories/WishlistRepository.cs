using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
	{
		public WishlistRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
