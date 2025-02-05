using AutoMapper;
using CloudinaryDotNet.Actions;
using Crystal_Eyes_Controller.Dtos;
using Crystal_Eyes_Controller.Dtos.OrderDetail;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Middleware;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.UnitOfWork;
using Crystal_Eyes_Controller.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;

namespace Crystal_Eyes_Controller.Controllers
{
	[CustomAuthorize]
	[Route("customer")]
	public class CustomerController : BaseController
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMailSystemService _mailSystemService;
		private readonly IMapper _mapper;
		private readonly ICloudinaryService _cloudinaryService;


		public CustomerController(IUnitOfWork unitOfWork, IMailSystemService mailSystemService, IMapper mapper, ICloudinaryService cloudinaryService)
		{
			_unitOfWork = unitOfWork;
			_mailSystemService = mailSystemService;
			_mapper = mapper;
			_cloudinaryService = cloudinaryService;
		}


		[HttpGet("change-password")]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost("change-password")]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _unitOfWork.User.GetByIdAsync(model.UserId);

			user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
			await _unitOfWork.SaveChangesAsync();
			TempData["SuccessMessage"] = "Mật khẩu đã được đổi thành công!";
			return View(); 
		}


		[HttpGet("order-detail/{orderId}")]
		public async Task<IActionResult> OrderDetail(int orderId, int pageNumber = 1, int pageSize = 7)
		{
			var orderDetail = _unitOfWork.OrderDetail.Queryable().Include(x => x.Product).Where(x => x.OrderId == orderId);

			var paginatedOrdersDetail = await PaginatedList<OrderDetail>
				.CreateAsync(orderDetail.AsNoTracking(), pageNumber, pageSize);

			var orderDetailView = _mapper.Map<List<OrderDetailViewDto>>(orderDetail);

			ViewBag.Paging = paginatedOrdersDetail;
			ViewBag.OrderDetail = orderDetailView;
			return View();
		}

		[HttpGet("history-order")]
		public async Task<IActionResult>  HistoryOrder(int pageNumber = 1, int pageSize = 7)
		{
			int uid = int.Parse(ViewBag.UserId);

			var orders = _unitOfWork.Order.Queryable().Where(x => x.UserId == uid);

			var paginatedOrders = await PaginatedList<Order>
				.CreateAsync(orders.AsNoTracking(), pageNumber, pageSize);

			ViewBag.Orders = paginatedOrders;

			return View();
		}

		[HttpGet("edit-profile")]
		public async Task<IActionResult> EditProfile()
		{
			int uid = int.Parse(ViewBag.UserId);
			var user = await _unitOfWork.Customer.Queryable().FirstOrDefaultAsync(x => x.UserId == uid);
			ViewBag.User = user;
			return View();
		}

		[HttpPost("edit-profile")]
		public async Task<IActionResult> EditProfile(CustomerViewModel model)
		{
			int uid = int.Parse(ViewBag.UserId);
			var user = await _unitOfWork.Customer.Queryable().FirstOrDefaultAsync(x => x.UserId == uid);

			if (!ModelState.IsValid)
			{
				ViewBag.User = user;
				return View(model);
			}

			var imageUrl = string.Empty;

			if (model.Image != null || model.Image?.Length > 0)
			{
				var uploadResult = await _cloudinaryService.UploadImageAsync(model.Image);

				if (uploadResult != null)
				{
					imageUrl = uploadResult.SecureUrl.ToString();
				}
			}

			if(user != null)
			{
				user.Name = model.Name;
				user.Address = model.Address;
				user.Phone = model.Phone;
				user.Dob = model.Dob;
				if (!string.IsNullOrEmpty(imageUrl))
				{
					user.Image = imageUrl;
				}
				await _unitOfWork.SaveChangesAsync();
			}
			ViewBag.User = user;
			return View(model);
		}

		[HttpGet("profile")]
		public async Task<IActionResult> Profile()
		{
			int uid = int.Parse(ViewBag.UserId);
			var user =  await _unitOfWork.Customer.Queryable().FirstOrDefaultAsync(x => x.UserId == uid);
			ViewBag.User = user;
			return View();
		}

    }
}
