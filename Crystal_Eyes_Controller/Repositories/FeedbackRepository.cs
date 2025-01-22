using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Repositories
{
	public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
	{
		public FeedbackRepository(CrystalEyesDbContext dbContext) : base(dbContext) { }
	}
}
