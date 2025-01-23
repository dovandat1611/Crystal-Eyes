using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
