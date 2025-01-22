using System.ComponentModel.DataAnnotations;

namespace Crystal_Eyes_Controller.Dtos
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email không được để trống")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Mật khẩu không được để trống")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
