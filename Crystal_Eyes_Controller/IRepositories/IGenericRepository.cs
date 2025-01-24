namespace Crystal_Eyes_Controller.IRepositories
{
	public interface IGenericRepository<T> where T : class
	{
		// Thêm một thực thể
		Task<bool> CreateAsync(T entity);

		// Thêm một thực thể và trả về thực thể sau khi thêm
		Task<T> CreateAndReturnAsync(T entity);

		// Lấy một thực thể theo ID
		Task<T> GetByIdAsync(int id);

		// Lấy tất cả các thực thể
		Task<IEnumerable<T>> GetAllAsync();

		// Trả về IQueryable để hỗ trợ truy vấn linh hoạt
		IQueryable<T> Queryable();

		// Cập nhật một thực thể
		Task<bool> UpdateAsync(T entity);

		// Xóa một thực thể theo ID
		Task<bool> DeleteAsync(int id);

		// Xóa nhiều thực thể
		Task<bool> DeleteRangeAsync(IEnumerable<T> entities);

		// Thêm nhiều thực thể
		Task<bool> CreateRangeAsync(IEnumerable<T> entities);
	}
}
