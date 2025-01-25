namespace Crystal_Eyes_Controller.Dtos.Product
{
	public class ProductViewDto
	{
		public int ProductId { get; set; }

		public int? CategoryId { get; set; }

		public string Name { get; set; } = null!;

		public string? MainImage { get; set; }

		public decimal Price { get; set; }

		public decimal? Discount { get; set; }

		public bool? IsWishlist { get; set; }
	}
}
