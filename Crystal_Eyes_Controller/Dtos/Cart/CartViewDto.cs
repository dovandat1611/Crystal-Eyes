namespace Crystal_Eyes_Controller.Dtos.Cart
{
	public class CartViewDto
	{
		public int ProductId { get; set; }

		public int ColorId { get; set; }

		public int Quantity { get; set; }

		public string Name { get; set; } = null!;

		public string? MainImage { get; set; }

		public string ColorName { get; set; } = null!;

		public decimal Price { get; set; }

		public decimal TotalPrice { get; set; }
	}
}
