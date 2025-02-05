using System.ComponentModel.DataAnnotations;

namespace Crystal_Eyes_Controller.Dtos
{
	public class CustomerViewModel
	{
		[Required(ErrorMessage = "Tên không được để trống")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Số điện thoại không được để trống")]
		[Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
		public string Phone { get; set; } = null!;

		[Required(ErrorMessage = "Ngày sinh không được để trống")]
		[DataType(DataType.Date)]
		public DateTime? Dob { get; set; }

		[Required(ErrorMessage = "Địa chỉ không được để trống")]
		public string Address { get; set; } = null!;

		public IFormFile? Image { get; set; }
	}
}
