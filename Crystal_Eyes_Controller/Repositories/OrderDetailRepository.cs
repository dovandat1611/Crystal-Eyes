using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
	{
		public OrderDetailRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
