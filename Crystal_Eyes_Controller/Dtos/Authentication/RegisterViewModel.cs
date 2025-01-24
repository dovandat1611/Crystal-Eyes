using System.ComponentModel.DataAnnotations;

namespace Crystal_Eyes_Controller.Dtos.Authentication
{
    public class RegisterViewModel
    {
		[Required(ErrorMessage = "Tên không được để trống")]
		[StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Số điện thoại không được để trống")]
		[Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Ngày sinh không được để trống")]
		[DataType(DataType.Date, ErrorMessage = "Định dạng ngày sinh không hợp lệ")]
		public DateTime Dob { get; set; }

		[Required(ErrorMessage = "Địa chỉ không được để trống")]
		[StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Email không được để trống")]
		[EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Mật khẩu không được để trống")]
		[DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
		public string RePassword { get; set; }
	}
}
