using System.ComponentModel.DataAnnotations;

namespace Crystal_Eyes_Controller.Dtos
{
	public class ChangePasswordViewModel
	{
		public int UserId { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
		[MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới.")]
		[Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
		public string ConfirmPassword { get; set; }
	}
}
