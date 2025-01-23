namespace Crystal_Eyes_Controller.Dtos.Excel
{
	public class ExcelColumnConfiguration
	{
		public string ColumnName { get; set; } = null!;
		public int Position { get; set; }
		public bool IsRequired { get; set; }
	}
}
