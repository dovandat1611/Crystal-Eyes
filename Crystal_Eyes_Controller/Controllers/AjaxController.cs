using AutoMapper;
using Crystal_Eyes_Controller.Dtos.Product;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Eyes_Controller.Controllers
{
	public class AjaxController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;


		public AjaxController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet("search")]
		public async Task<IActionResult> Search(string query)
		{
			var queryable = _unitOfWork.Product.Queryable()
				.Where(p => p.IsDelete == false && p.IsActive == true);

			if (!string.IsNullOrWhiteSpace(query))
			{
				queryable = queryable.Where(p => p.Name.Contains(query));
			}

			var results = await queryable.ToListAsync();
			var productsDto = _mapper.Map<List<ProductViewDto>>(results);

			return Json(productsDto);
		}

		[HttpGet("wish-list-process/{productId}/{userId}")]
		public async Task<IActionResult> WishlistProcess(int productId, int userId)
		{	
			var queryAble = await _unitOfWork.Wishlist.Queryable()
				.Where(x => x.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync();

			string message = "Thực hiện không thành công";
			string action = string.Empty;
			if(queryAble != null)
			{
				var isDelete = await _unitOfWork.Wishlist.DeleteAsync(queryAble.WishlistId);
				if(isDelete == true)
				{
					message = "Xóa thành công sản phẩm ra khỏi Yêu Thích";
					action = "Delete";
				}
			}

			if (queryAble == null)
			{
				var wishList = new Wishlist()
				{
					ProductId = productId,
					UserId = userId
				};

				var isCreate = await _unitOfWork.Wishlist.CreateAsync(wishList);
				if (isCreate == true)
				{
					message = "Thêm thành công sản phẩm vào Yêu Thích";
					action = "Create";
				}
			}
			var totalWishlist = await _unitOfWork.Wishlist.Queryable().CountAsync(x => x.UserId == userId);
			return Json(new { message, action, totalWishlist});
		}


	}
}
