namespace Crystal_Eyes_Controller.Dtos
{
	public class UserLoginDto
	{
		public int UserId { get; set; }
		public string RoleName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? Phone { get; set; }
		public DateTime? Dob { get; set; }
		public string? Address { get; set; }
		public string? Image { get; set; }
	}
}
