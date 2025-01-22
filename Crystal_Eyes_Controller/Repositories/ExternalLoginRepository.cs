using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class ExternalLoginRepository : GenericRepository<ExternalLogin>, IExternalLoginRepository
	{
		public ExternalLoginRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
