using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.Repositories;

namespace Crystal_Eyes_Controller.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly CrystalEyesDbContext _dbContext;

		private IAdminRepository _adminRepository;
		private ICategoryRepository _categoryRepository;
		private IColorRepository _colorRepository;
		private ICustomerRepository _customerRepository;
		private IExternalLoginRepository _externalLoginRepository;
		private IFeedbackRepository _feedbackRepository;
		private IImageRepository _imageRepository;
		private IOrderDetailRepository _orderDetailRepository;
		private IOrderRepository _orderRepository;
		private IProductRepository _productRepository;
		private IUserRepository _userRepository;
		private IUserOtpRepository _userOtpRepository;

		public UnitOfWork(CrystalEyesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IAdminRepository Admin => _adminRepository ??= new AdminRepository(_dbContext);

		public ICategoryRepository Category => _categoryRepository ??= new CategoryRepository(_dbContext);

		public IColorRepository Color => _colorRepository ??= new ColorRepository(_dbContext);

		public ICustomerRepository Customer => _customerRepository ??= new CustomerRepository(_dbContext);

		public IExternalLoginRepository ExternalLogin => _externalLoginRepository ??= new ExternalLoginRepository(_dbContext);

		public IFeedbackRepository Feedback => _feedbackRepository ??= new FeedbackRepository(_dbContext);

		public IImageRepository Image => _imageRepository ??= new ImageRepository(_dbContext);

		public IOrderDetailRepository OrderDetail => _orderDetailRepository ??= new OrderDetailRepository(_dbContext);

		public IOrderRepository Order => _orderRepository ??= new OrderRepository(_dbContext);

		public IProductRepository Product => _productRepository ??= new ProductRepository(_dbContext);

		public IUserRepository User => _userRepository ??= new UserRepository(_dbContext);

		public IUserOtpRepository UserOtp => _userOtpRepository ??= new UserOtpRepository(_dbContext);

		public async Task<int> SaveChangesAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}
	}
}
