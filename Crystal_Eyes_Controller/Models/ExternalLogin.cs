using System;
using System.Collections.Generic;

namespace Crystal_Eyes_Controller.Models;

public partial class ExternalLogin
{
	public int ExloginId { get; set; }

	public int? UserId { get; set; }

	public string ExternalProvider { get; set; } = null!;

	public string ExternalUserId { get; set; } = null!;

	public string? AccessToken { get; set; }

	public virtual User? User { get; set; }
}
