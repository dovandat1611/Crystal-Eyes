using AutoMapper;
using Crystal_Eyes_Controller.Common;
using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.Dtos.Feedback;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.Services;
using Crystal_Eyes_Controller.UnitOfWork;
using Crystal_Eyes_Controller.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Crystal_Eyes_Controller.Controllers
{
	public class FeedbackController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMailSystemService _mailSystemService;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public FeedbackController(IUnitOfWork unitOfWork, IMailSystemService mailSystemService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_mailSystemService = mailSystemService;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpPost("add-feedback")]
		public async Task<IActionResult> AddFeedback(string email, string content, int rating, int productId)
		{
			if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
			{
				return Ok(new { success = false, message = "Email không hợp lệ!" });
			}

			if (string.IsNullOrEmpty(content) || content.Length < 10)
			{
				return Ok(new { success = false, message = "Nội dung đánh giá quá ngắn!" });
			}

			if (rating < 1 || rating > 5)
			{
				return Ok(new { success = false, message = "Số sao phải từ 1 đến 5!" });
			}

			var productExists = await _unitOfWork.Product.Queryable().FirstOrDefaultAsync(x => x.ProductId == productId);
			if (productExists == null)
			{
				return Ok(new { success = false, message = "Sản phẩm không tồn tại!" });
			}

			Feedback feedback = new Feedback()
			{
				ProductId = productId,
				Content = content,
				Star = rating,
				CreateDate = DateTime.UtcNow,
				IsDelete = false
			};

			var httpContext = _httpContextAccessor.HttpContext;

			if (httpContext == null)
			{
				return Ok(new { success = false, message = "Lỗi hệ thống!" });
			}

			var customerCookie = httpContext?.Request.Cookies["account"];

			if (!string.IsNullOrEmpty(customerCookie))
			{
				var user = await _unitOfWork.User.Queryable().FirstOrDefaultAsync(u => u.Email == customerCookie);
				if (user != null)
				{
					feedback.UserId = user.UserId;
				}
			}

			await _unitOfWork.Feedback.CreateAsync(feedback);
			await _unitOfWork.SaveChangesAsync();
			await _mailSystemService.SendEmailAsync(email, Constants.Email_Subject.ADD_FEEDBACK, EmailTemplates.Feedback());
			return Ok(new { success = true, message = "Đã thêm thành công đánh giá!" });
		}


		[HttpPost("get-feedback/{productId}")]
		public async Task<IActionResult> GetFeedback(int productId)
		{
			var exsitsFeedback = await _unitOfWork.Feedback.Queryable()
				.Include(x => x.User).ThenInclude(x => x.Customer)
				.Where(x => x.ProductId == productId)
				.OrderByDescending(x => x.FeedbackId)
				.Take(3)
				.ToListAsync();

			if (!exsitsFeedback.Any())
			{
				return Ok(new List<FeedbackViewDto>());
			}

			var feedbackDto = _mapper.Map<List<FeedbackViewDto>>(exsitsFeedback);

			return Ok(feedbackDto);
		}

	}
}
