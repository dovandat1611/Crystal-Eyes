namespace Crystal_Eyes_Controller.Dtos.Cart
{
	public class CartDto
	{
		public List<CartViewDto> Items { get; set; } = new List<CartViewDto>();
		public decimal TotalAmount { get; set; }
	}
}
