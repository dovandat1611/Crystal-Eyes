namespace Crystal_Eyes_Controller.Dtos.OrderDetail
{
	public class OrderDetailViewDto
	{
		public string Name { get; set; } = null!;
		public string? MainImage { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal? TotalPrice { get; set; }
	}
}
