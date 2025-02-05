using AutoMapper;
using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.Dtos.Product;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.Services;
using Crystal_Eyes_Controller.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;

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

	}
}
