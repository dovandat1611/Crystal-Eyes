using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class User
{
	public int UserId { get; set; }

	public string RoleName { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Password { get; set; } = null!;

	public bool? IsVerify { get; set; }

	public bool? IsActive { get; set; }

	public DateTime? CreatedAt { get; set; }

	public bool? IsExternalLogin { get; set; }

	public virtual Admin? Admin { get; set; }

	public virtual Customer? Customer { get; set; }

	public virtual ICollection<ExternalLogin> ExternalLogins { get; } = new List<ExternalLogin>();

	public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

	public virtual ICollection<Order> Orders { get; } = new List<Order>();

	public virtual ICollection<UserOtp> UserOtps { get; } = new List<UserOtp>();
}
