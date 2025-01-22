using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class CartRepository : GenericRepository<Cart>, ICartRepository
	{
		public CartRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
