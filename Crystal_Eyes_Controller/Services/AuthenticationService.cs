using Crystal_Eyes_Controller.Dtos.Authentication;
using Crystal_Eyes_Controller.IServices;
using Crystal_Eyes_Controller.Models;
using Crystal_Eyes_Controller.UnitOfWork;
using Crystal_Eyes_Controller.Utils;
using Crystal_Eyes_Controller.Common;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Eyes_Controller.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMailSystemService _mailSystemService;

		public AuthenticationService(IUnitOfWork unitOfWork, IMailSystemService mailSystemService)
		{
			_unitOfWork = unitOfWork;
			_mailSystemService = mailSystemService;
		}

		public async Task<(bool IsSuccess, string Message)> RegisterAsync(RegisterViewModel model)
		{
			try
			{
				var newUser = new User
				{
					Email = model.Email,
					Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
					RoleName = "Customer",
					IsActive = true,
					IsVerify = false,
					CreatedAt = DateTime.UtcNow,
					IsExternalLogin = false
				};

				var addUser = await _unitOfWork.User.CreateAndReturnAsync(newUser);

				var newCustomer = new Customer
				{
					UserId = addUser.UserId,
					Name = model.Name,
					Phone = model.Phone,
					Dob = model.Dob,
					Address = model.Address,
					Image = Constants.Default_Avatar.CUSTOMER
				};

				await _unitOfWork.Customer.CreateAndReturnAsync(newCustomer);

				await _unitOfWork.SaveChangesAsync();

				await _mailSystemService.SendEmailAsync(
					newUser.Email,
					Constants.Email_Subject.VERIFY,
					EmailTemplates.Verify(newCustomer.Name, AesEncryption.Encrypt(newCustomer.UserId.ToString()))
				);
				return (true, "Vui lòng check email để verify tài khoản");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return (false, "Đăng ký tài khoản xảy ra lỗi, vui lòng thử lại.");
			}
		}

		public async Task<(bool IsSuccess, string Message)> VerifyAsync(string user_id)
		{
			try
			{
				if (string.IsNullOrEmpty(user_id))
				{
					return (false, "Mã xác thực không hợp lệ.");
				}

				if (!int.TryParse(AesEncryption.Decrypt(user_id), out int id))
				{
					return (false, "Mã xác thực không hợp lệ.");
				}

				var existingUser = await _unitOfWork.User.Queryable().FirstOrDefaultAsync(x => x.UserId == id);

				if (existingUser == null)
				{
					return (false, "Người dùng không tồn tại.");
				}

				if (existingUser.IsVerify == true)
				{
					return (false, "Tài khoản đã được xác thực trước đó.");
				}

				existingUser.IsVerify = true;
				await _unitOfWork.SaveChangesAsync();

				return (true, "Xác thực tài khoản thành công.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return (false, "Xác thực tài khoản xảy ra lỗi, vui lòng thử lại.");
			}
		}

	}
}
