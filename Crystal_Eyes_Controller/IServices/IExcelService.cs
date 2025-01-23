using Crystal_Eyes_Controller.Dtos.Excel;

namespace Crystal_Eyes_Controller.IServices
{
	public interface IExcelService
	{
		Task<List<Dictionary<string, string>>> ReadFromExcelAsync(IFormFile file, List<ExcelColumnConfiguration> columnConfigurations);
	}
}
