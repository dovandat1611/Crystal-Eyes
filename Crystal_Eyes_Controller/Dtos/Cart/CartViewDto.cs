namespace Crystal_Eyes_Controller.Dtos.Cart
{
	public class CartViewDto
	{
		public int CartItemId { get; set; }

		public int? UserId { get; set; }

		public int? ProductId { get; set; }

		public int Quantity { get; set; }

		public string Name { get; set; } = null!;

		public string? MainImage { get; set; }

		public decimal Price { get; set; }

		public decimal TotalPrice { get; set; }
	}
}
