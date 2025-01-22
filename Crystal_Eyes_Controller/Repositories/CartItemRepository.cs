using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
	{
		public CartItemRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
