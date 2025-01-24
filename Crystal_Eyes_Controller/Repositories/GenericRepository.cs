using Crystal_Eyes_Controller.IRepositories;
using Crystal_Eyes_Controller.Models;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Eyes_Controller.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly CrystalEyesDbContext _dbContext;

		public GenericRepository(CrystalEyesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// Trả về IQueryable để hỗ trợ truy vấn linh hoạt
		public IQueryable<T> Queryable()
		{
			return _dbContext.Set<T>().AsQueryable();
		}

		// Thêm một thực thể
		public async Task<bool> CreateAsync(T entity)
		{
			if (entity == null) return false;

			await _dbContext.Set<T>().AddAsync(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}

		// Thêm một thực thể và trả về thực thể sau khi thêm
		public async Task<T> CreateAndReturnAsync(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			await _dbContext.Set<T>().AddAsync(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}

		// Lấy tất cả các thực thể
		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbContext.Set<T>().ToListAsync();
		}

		// Lấy một thực thể theo ID
		public async Task<T> GetByIdAsync(int id)
		{
			var entity = await _dbContext.Set<T>().FindAsync(id);

			if (entity == null)
			{
				throw new KeyNotFoundException($"Entity with ID {id} was not found.");
			}

			return entity;
		}

		// Xóa một thực thể theo ID
		public async Task<bool> DeleteAsync(int id)
		{
			var entity = await _dbContext.Set<T>().FindAsync(id);
			if (entity == null) return false;

			_dbContext.Set<T>().Remove(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}

		// Xóa nhiều thực thể
		public async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
		{
			if (entities == null || !entities.Any()) return false;

			_dbContext.Set<T>().RemoveRange(entities);
			return await _dbContext.SaveChangesAsync() > 0;
		}

		// Cập nhật một thực thể
		public async Task<bool> UpdateAsync(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			_dbContext.Set<T>().Update(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}

		// Thêm nhiều thực thể
		public async Task<bool> CreateRangeAsync(IEnumerable<T> entities)
		{
			if (entities == null || !entities.Any()) return false;

			await _dbContext.Set<T>().AddRangeAsync(entities);
			return await _dbContext.SaveChangesAsync() > 0;
		}
	}
}
