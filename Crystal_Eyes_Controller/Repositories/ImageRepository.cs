using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class ImageRepository : GenericRepository<Image>, IImageRepository
	{
		public ImageRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
