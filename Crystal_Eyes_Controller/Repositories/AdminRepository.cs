using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class AdminRepository : GenericRepository<Admin>, IAdminRepository
	{
		public AdminRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
