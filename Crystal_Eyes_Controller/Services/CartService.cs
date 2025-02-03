using AutoMapper;
using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;

namespace Crystal_Eyes_Controller.Services
{
	public class CartService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;


		public CartService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_httpContextAccessor = httpContextAccessor;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		private ISession Session => _httpContextAccessor.HttpContext.Session;

		public CartDto GetCart()
		{
			var cartDto = new CartDto();
			var cartJson = Session.GetString("Cart");
			if (!string.IsNullOrEmpty(cartJson))
			{
				var cartItem = JsonConvert.DeserializeObject<List<CartItemDto>>(cartJson);

				var getCart = new List<CartViewDto>();

				foreach (var item in cartItem)
				{
					var existingItem = _unitOfWork.Product.Queryable().Include(x => x.Colors)
						.Where(x => x.ProductId == item.ProductId && x.Colors.Any(c => c.ColorId == item.Color)).FirstOrDefault();

					if (existingItem == null)
					{
						continue;
					}

					CartViewDto cartViewDto = new CartViewDto()
					{
						ProductId = existingItem.ProductId,
						Quantity = item.Quantity,
						Name = existingItem.Name,
						MainImage = existingItem.MainImage,
						Price = existingItem.Price * (100 - existingItem.Discount ?? 0) / 100,
						TotalPrice = (existingItem.Price * (100 - existingItem.Discount ?? 0) / 100) * item.Quantity,
						ColorId = item.Color,
						ColorName = existingItem.Colors.FirstOrDefault(c => c.ColorId == item.Color)?.ColorName ?? "Không xác định",
					};
					getCart.Add(cartViewDto);
				}

				if(getCart.Count() > 0)
				{
					cartDto.Items = getCart;
					cartDto.TotalAmount = getCart.Sum(x => x.TotalPrice);
				}
				return cartDto;
			}
			return cartDto;
		}

		public void AddToCart(int productId, int quantity, int color)
		{
			if (productId <= 0 || quantity <= 0 || color <= 0)
			{
				Console.WriteLine("cannot add to cart");
				return;
			}

			var cartJson = Session.GetString("Cart");
			Console.WriteLine($"Cart before add: {cartJson}");

			var cart = string.IsNullOrEmpty(cartJson)
				? new List<CartItemDto>()
				: JsonConvert.DeserializeObject<List<CartItemDto>>(cartJson);

			var existingItem = cart.FirstOrDefault(x => x.ProductId == productId && x.Color == color);

			if (existingItem != null)
			{
				Console.WriteLine("plus quantity");
				existingItem.Quantity += quantity;
			}
			else
			{
				Console.WriteLine("new cart item");
				cart.Add(new CartItemDto { ProductId = productId, Quantity = quantity, Color = color });
			}

			cartJson = JsonConvert.SerializeObject(cart);
			Session.SetString("Cart", cartJson);

			Console.WriteLine($"Cart after add: {cartJson}");
		}

		public void UpdateToCart(UpdateCartRequest updateCartRequest)
		{
			var cartJson = Session.GetString("Cart");
			var cart = string.IsNullOrEmpty(cartJson)
				? new List<CartItemDto>()
				: JsonConvert.DeserializeObject<List<CartItemDto>>(cartJson);

			foreach (var item in updateCartRequest.CartItems)
			{
				var existingItem = cart.FirstOrDefault(x => x.ProductId == item.ProductId && x.Color == item.Color);

				if(existingItem != null)
				{
					existingItem.Quantity = item.Quantity;
				}
			}

			cartJson = JsonConvert.SerializeObject(cart);
			Session.SetString("Cart", cartJson);
		}

		public void RemoveFromCart(int productId, int color)
		{
			var cartJson = Session.GetString("Cart");
			var cart = cartJson != null ? JsonConvert.DeserializeObject<List<CartItemDto>>(cartJson) : new List<CartItemDto>();
			cart.RemoveAll(x => x.ProductId == productId && x.Color == color);
			Session.SetString("Cart", JsonConvert.SerializeObject(cart));
		}

		public void ClearCart()
		{
			Session.Remove("Cart");
		}
	}
}
